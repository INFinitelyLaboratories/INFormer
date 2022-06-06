using UnityEngine;
using System.Collections;

public class Firearm : Weapon
{
    [SerializeField] private int m_maxAmmo;
    [SerializeField] private int m_maxAmmoInBag;
    [SerializeField] private float m_rate;
    [SerializeField] private int m_damage;
    [SerializeField] private float m_recoil;
    [SerializeField] private float m_reloadTime;
    [SerializeField] private float m_EquipTime;
    [SerializeField] private bool m_isAutomatic;
    [SerializeField] private Transform m_muzzle;

    private Coroutine m_clampFire;
    private Animator m_animator;
    private WeaponController m_controller;

    private int m_ammo;
    private int m_ammoInBag;

    private bool m_isReloading;
    private float m_TimeToShoot;

    public override string GetInfo()
    {
        return System.String.Format( "{0}:{1}" , m_ammo , m_ammoInBag );
    }

    public override void SetInfo(string info)
    {
        string[] infos = info.Split(':');

        if (info != "")
        {
            int.TryParse(infos[0], out m_ammo);
            int.TryParse(infos[1], out m_ammoInBag);
        }
        else
        {
            m_ammo = m_maxAmmo;
            m_ammoInBag = m_maxAmmoInBag;
        }

        if(this.enabled == true) Visualizer.UpdateVisualizerData("Ammo", m_ammo + "/" + m_ammoInBag);
    }

    private void Awake()
    {
        m_animator = GetComponent<Animator>();

        m_controller = transform.parent.GetComponent<WeaponController>();
    }

    private void OnEnable()
    {
        Visualizer.UpdateVisualizerData("Ammo", m_ammo + "/" + m_ammoInBag);

        m_TimeToShoot = m_EquipTime;

        if(m_ammo == 0)
        {
            Reload();
        }
    }

    private void OnDisable()
    {
        m_isReloading = false;
    }

    public override void EquipmentUpdate( InteractState primaryState , InteractState secondaryState , InteractState reloadState )
    {
        if( primaryState == InteractState.start && m_isAutomatic == false && m_TimeToShoot <= 0)
        {
            TryFire();
            m_TimeToShoot = 1 / m_rate;
        }
        
        if ((primaryState == InteractState.start || primaryState == InteractState.hold) && m_isAutomatic == true && m_TimeToShoot <= 0)
        {
            TryFire();
            m_TimeToShoot = 1 / m_rate;
        }
        
        if ( reloadState == InteractState.start )
        {
            Reload();
        }

        m_TimeToShoot -= Time.deltaTime;
    }


    private void TryFire()
    {
        if (m_isReloading) return;
        if (m_ammo > 0)
        {
            m_animator.Play("Fire", 0, 0f);
            ParticleManager.PlayParticle("Flash", m_muzzle.position , Camera.main.transform.eulerAngles);

            m_ammo--;

            Visualizer.UpdateVisualizerData("Ammo", m_ammo + "/" + m_ammoInBag);

            if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward , out var hit, 1000, ~~3))
            {
                ParticleManager.PlayParticle("Hit" , hit.point , -Camera.main.transform.eulerAngles);

                if( hit.transform.TryGetComponent<IDamageable>(out var damageable) )
                {
                    damageable.TakeDamage(m_damage);
                }
            }

            m_controller.rotatement.AddRecoil(m_recoil);
        }

        if(m_ammo == 0)
        {
            Invoke(nameof(Reload) , 0.2f);
        }
    }

    private void Reload()
    {
        if (m_isReloading == false && m_ammoInBag > 0 && m_ammo < m_maxAmmo)
        {
            StartCoroutine(_Reload());

            m_animator.CrossFade("Reload", 0.4f , 0, 0f);
        }
    }

    private IEnumerator _Reload()
    {
        m_isReloading = true;

        yield return new WaitForSeconds( m_reloadTime );

        m_isReloading = false;

        int newAmmo = (m_ammoInBag >= m_maxAmmo - m_ammo)? m_maxAmmo - m_ammo : m_ammoInBag;

        m_ammo += newAmmo;
        m_ammoInBag -= newAmmo;

        Visualizer.UpdateVisualizerData( "Ammo" , m_ammo + "/" + m_ammoInBag);
    }
}
