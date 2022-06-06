using UnityEngine;

public class Knife : Weapon
{
    [SerializeField] private float m_rate;

    private float m_timeToHit;
    private Animator m_animator;

    private void Awake()
    {
        m_animator = GetComponent<Animator>();
    }

    public override void EquipmentUpdate(InteractState primaryState, InteractState secondaryState, InteractState reloadState)
    {
        if((primaryState == InteractState.start || primaryState == InteractState.hold)&&(m_timeToHit <= 0))
        {
            TryFire();
            m_timeToHit = 1 / m_rate;
        }

        m_timeToHit -= Time.deltaTime;
    }

    private void TryFire()
    {
        m_animator.Play("Knife Attack",7,0);
        ((PlayerMovement)FindObjectOfType(typeof(PlayerMovement))).AddForce( -transform.forward * 100f );
    }

    public override string GetInfo()
    {
        return "";
    }

    public override void SetInfo(string info)
    {
        //
    }
}
