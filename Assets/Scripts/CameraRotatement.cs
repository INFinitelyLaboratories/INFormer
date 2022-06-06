using UnityEngine;

public class CameraRotatement : MonoBehaviour
{
    [SerializeField] private float m_sensetivity;
    [SerializeField] private float m_rotateSmoothAmount;
    [SerializeField] private float m_offsetSmoothAmount;
    [SerializeField] private float m_maxAngle;

    [SerializeField] private Transform m_playerTransform;
    [SerializeField] private Transform m_cameraTransform;
    [SerializeField] private Transform m_interactorRoot;

    private Vector3 m_rotation;
    private Vector3 m_velocity;
    private Vector3 m_rootOffset;
    private Vector3 m_offset;

    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }


    private void Update()
    {
        m_velocity.y = Input.GetAxis("Mouse Y") * m_sensetivity;
        m_velocity.x = Input.GetAxis("Mouse X") * m_sensetivity;

        m_rotation.x -= m_velocity.y;
        m_rotation.y += m_velocity.x;

        m_rootOffset = Vector3.Lerp( m_rootOffset , -m_velocity.normalized / 50, 3f * Time.deltaTime );

        m_interactorRoot.localPosition = Vector3.Lerp( m_interactorRoot.localPosition , m_rootOffset, 30 * Time.deltaTime );

        m_rotation.x = Mathf.Clamp( m_rotation.x , -m_maxAngle, m_maxAngle);

        m_cameraTransform.localRotation = Quaternion.Lerp( m_cameraTransform.localRotation , Quaternion.Euler( m_rotation.x + m_offset.x , 90f , 0f) , m_rotateSmoothAmount * Time.deltaTime );
        m_playerTransform.localRotation = Quaternion.Lerp( m_playerTransform.localRotation , Quaternion.Euler( 00f , m_rotation.y + m_offset.y , 0f ) , m_rotateSmoothAmount * Time.deltaTime );

        m_offset = Vector3.Lerp( m_offset , Vector3.zero , m_offsetSmoothAmount * Time.deltaTime );
    }

    public void AddRecoil(float upForce)
    {
        m_offset += Vector3.left * upForce;
    }
}
