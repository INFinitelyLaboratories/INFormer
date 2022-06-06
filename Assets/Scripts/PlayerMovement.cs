using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    #region Fields

    [Header("Walking...")]
        [SerializeField] private float m_walkSpeed;
        [SerializeField] private float m_walkSmoothAmount;
    [Header("Crouching...")]
        [SerializeField] private float m_crouchSpeed;
        [SerializeField] private float m_crouchSmoothAmount;
    [Header("Jumping...")]
        [SerializeField] private float m_flyMoveMultiplier;
        [SerializeField] private float m_gravityScale;
        [SerializeField] private float m_jumpForce;

    private CharacterController m_character;
    private Transform m_transform;
    private Vector3 m_inputValue;

    private Vector3 m_velocity => m_horizontalVelocity + m_verticalVelocity;
    private Vector3 m_horizontalVelocity;
    private Vector3 m_verticalVelocity;

    private float m_weight;

    private bool m_isPreviouslyGrounded;
    private bool m_isNeedToJump;
    private bool m_isCrouch;

    #endregion Fields

    private void Awake()
    {
        m_character = GetComponent<CharacterController>();
        m_transform = GetComponent<Transform>();
    }

    private void Update()
    {
        m_inputValue.x = +Input.GetAxisRaw("Vertical");
        m_inputValue.z = -Input.GetAxisRaw("Horizontal");

        m_isNeedToJump = Input.GetKey(KeyCode.Space);
        m_isCrouch = Input.GetKey(KeyCode.LeftControl);
    }

    private void FixedUpdate()
    {
        VelocityHandle();
        HorizontalVelocityHandle();
        VerticalVelocityHandle();
        CrouchHandle();

        m_character.Move(m_velocity * Time.fixedDeltaTime);
    }

    private void VerticalVelocityHandle()
    {
        if (m_character.isGrounded)
        {
            if (m_isNeedToJump)
            {
                m_isNeedToJump = false;
                m_verticalVelocity = Vector3.up * m_jumpForce;
            }
            else
            {
                m_verticalVelocity = Vector3.down * 10f;
            }
            if(m_isPreviouslyGrounded == false)
            {
                m_horizontalVelocity = Vector3.ClampMagnitude(m_horizontalVelocity, m_walkSpeed * 0.75f);
            }
        }
        else
        {
            if (m_isPreviouslyGrounded)
            {
                m_verticalVelocity = m_verticalVelocity.y > 0? m_verticalVelocity : Vector3.zero;
            }
            else
            {
                m_verticalVelocity += Physics.gravity * m_gravityScale * Time.fixedDeltaTime;
            }
        }

        m_isPreviouslyGrounded = m_character.isGrounded;
    }

    private void HorizontalVelocityHandle()
    {
        m_horizontalVelocity = Vector3.Lerp(
            m_horizontalVelocity, 
            m_transform.TransformDirection(m_inputValue.normalized) * ( m_isCrouch? m_crouchSpeed : m_walkSpeed ) / m_weight, 
            (m_character.isGrounded? m_walkSmoothAmount  : m_walkSmoothAmount / m_flyMoveMultiplier) * Time.fixedDeltaTime
        );


    }

    private void CrouchHandle()
    {
        m_character.height = Mathf.MoveTowards( m_character.height , m_isCrouch? 1f : 2f , m_crouchSmoothAmount * Time.fixedDeltaTime );
        m_character.center = Vector3.up * Mathf.MoveTowards( m_character.center.y , m_isCrouch? 0.5f : 0f , m_crouchSmoothAmount * Time.fixedDeltaTime );
    }
    
    private void VelocityHandle()
    {
        if( Mathf.Abs(m_horizontalVelocity.x) > Mathf.Abs(m_character.velocity.x) ) m_horizontalVelocity.x = m_character.velocity.x;
        if (Mathf.Abs(m_horizontalVelocity.z) > Mathf.Abs(m_character.velocity.z)) m_horizontalVelocity.z = m_character.velocity.z;
        if (Mathf.Abs(m_verticalVelocity.y) > Mathf.Abs(m_character.velocity.y)) m_verticalVelocity.y = m_character.velocity.y;
    }

    public void AddForce(Vector3 velocity)
    {
        m_horizontalVelocity.x += velocity.x;
        m_horizontalVelocity.z += velocity.z;
        m_verticalVelocity.y += velocity.y;
    }

    public void SetWeaponWeight(float newWeigth)
    {
        if (newWeigth < 1) return;

        m_weight = newWeigth;
    }
}
