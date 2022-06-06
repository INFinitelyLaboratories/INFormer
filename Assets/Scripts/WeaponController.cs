using UnityEngine;

public class WeaponController : MonoBehaviour
{
    [SerializeField] private Weapon m_primarySlot;
    [SerializeField] private Weapon m_secondarySlot;
    [SerializeField] private Weapon m_meleeSlot;
    [SerializeField] private Weapon m_grenadeSlot;
    [SerializeField] private Weapon m_bombSlot;
    [SerializeField] private PlayerMovement m_movement;

    public CameraRotatement rotatement;

    private Weapon m_selectedWeapon;

    private void Awake()
    {
        Pickup("M4A1");
        Pickup("P2000");
        Pickup("Knife");

        SelectWeapon( m_primarySlot );
    }



    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) SelectWeapon( m_primarySlot );
        if (Input.GetKeyDown(KeyCode.Alpha2)) SelectWeapon( m_secondarySlot );
        if (Input.GetKeyDown(KeyCode.Alpha3)) SelectWeapon( m_meleeSlot );
        if (Input.GetKeyDown(KeyCode.Alpha4)) SelectWeapon( m_grenadeSlot );
        if (Input.GetKeyDown(KeyCode.Alpha5)) SelectWeapon( m_bombSlot );
        if (Input.GetKeyDown(KeyCode.G)) DropCurrentWeapon();
        if (Input.GetKeyDown(KeyCode.E)) TryPickup();


        InteractState primaryState = Input.GetMouseButtonDown(0) ? InteractState.start :
                                     Input.GetMouseButton(0) ? InteractState.hold :
                                     Input.GetMouseButtonUp(0) ? InteractState.end :
                                     InteractState.none;
        
        InteractState secondaryState = Input.GetMouseButtonDown(1) ? InteractState.start :
                                       Input.GetMouseButton(1) ? InteractState.hold :
                                       Input.GetMouseButtonUp(1) ? InteractState.end :
                                       InteractState.none;
        
        InteractState reloadState = Input.GetKeyDown(KeyCode.R) ? InteractState.start :
                                    Input.GetKey(KeyCode.R) ? InteractState.hold :
                                    Input.GetKeyUp(KeyCode.R) ? InteractState.end :
                                    InteractState.none;

        if(m_selectedWeapon != null) m_selectedWeapon.EquipmentUpdate( primaryState , secondaryState , reloadState );
    }



    private void SelectWeapon( Weapon weaponToSelect )
    {
        if (m_selectedWeapon == weaponToSelect || weaponToSelect == null) return;

        if(m_selectedWeapon != null) m_selectedWeapon.gameObject.SetActive(false);

        m_selectedWeapon = weaponToSelect;

        if(m_selectedWeapon != null) m_selectedWeapon.gameObject.SetActive(true);

        if(m_movement != null) m_movement.SetWeaponWeight( m_selectedWeapon.weight );
    }

    private void SelectAnotherWeapon()
    {
        if (m_primarySlot != null && m_selectedWeapon != m_primarySlot) { SelectWeapon(m_primarySlot); return; }
        if (m_secondarySlot != null && m_selectedWeapon != m_secondarySlot) { SelectWeapon(m_secondarySlot); return; }
        if (m_meleeSlot != null && m_selectedWeapon != m_meleeSlot) { SelectWeapon(m_meleeSlot); return; }
        if (m_grenadeSlot != null && m_selectedWeapon != m_grenadeSlot) { SelectWeapon(m_grenadeSlot); return; }
        if (m_bombSlot != null && m_selectedWeapon != m_bombSlot) { SelectWeapon(m_bombSlot); return; }
    }

    private void TryPickup()
    {
        if(Physics.Raycast( transform.parent.position , transform.parent.forward , out var hit , 3f , 1<<7 ))
        {
            if(hit.transform.TryGetComponent<Item>(out var item))
            {
                if(Pickup( item.weaponName , item.weaponInfo ))
                {
                    Destroy(item.gameObject);
                }
            }
        }
    }

    private bool Pickup(string weaponName , string weaponInfo = "")
    {
        Weapon newWeapon = Weapon.CreateWeaponByName(weaponName, transform);

        if (newWeapon == null) return false;

        newWeapon.SetInfo(weaponInfo);

        DropWeapon(newWeapon.slotID);

        switch (newWeapon.slotID)
        {
            case 1: m_primarySlot = newWeapon; break;
            case 2: m_secondarySlot = newWeapon; break;
            case 3: m_meleeSlot = newWeapon; break;
            case 4: m_grenadeSlot = newWeapon; break;
            case 5: m_bombSlot = newWeapon; break;
        }

        SelectAnotherWeapon();

        return true;
    }

    private void DropCurrentWeapon()
    {
        DropWeapon(m_selectedWeapon.slotID);
    }

    private void DropWeapon(int slot)
    {
        Weapon weapon = GetEquipmentWeaponBySlotID(slot);

        if (weapon == null) return;
        if (m_selectedWeapon == weapon) SelectAnotherWeapon();

        Item.Create(weapon, transform.parent.position, transform.forward * 7);

        Destroy(weapon.gameObject);
    }

    private Weapon GetEquipmentWeaponBySlotID(int slot)
    {
        switch(slot)
        {
            case 1: return m_primarySlot;
            case 2: return m_secondarySlot;
            case 3: return m_meleeSlot;
            case 4: return m_grenadeSlot;
            case 5: return m_bombSlot;

            default: return null;
        }
    }
}
