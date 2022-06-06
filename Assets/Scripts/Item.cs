using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Item : MonoBehaviour
{
    private Rigidbody m_rigidbody;
    
    public string weaponInfo { get; private set; }
    public string weaponName { get; private set; }


    private void Awake()
    {
        m_rigidbody = GetComponent<Rigidbody>();
    }

    private void Init( Weapon weapon , Vector3 velocity )
    {
        if (weapon == null) Destroy(gameObject);

        weaponName = weapon.weaponName;
        weaponInfo = weapon.GetInfo();

        m_rigidbody.velocity = velocity;
    }

    public static Item Create(Weapon weapon, Vector3 position, Vector3 velocity )
    {
        Item item = Instantiate( weapon.itemModel , position, Quaternion.LookRotation(velocity) * Quaternion.Euler(0,90,0));
             item.Init( weapon , velocity );

        return item;
    }
}
