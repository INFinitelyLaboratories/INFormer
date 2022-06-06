using UnityEngine;
using System.Linq;

[RequireComponent(typeof(Animator))]
public abstract class Weapon : MonoBehaviour
{
    [SerializeField] private string m_weaponName;
    [SerializeField] private Item m_itemModel;
    [SerializeField] private int m_slotID;
    [SerializeField] private float m_weight;
    public Item itemModel => m_itemModel;
    public int slotID => m_slotID;
    public string weaponName => m_weaponName;
    public float weight => m_weight;

    public abstract string GetInfo();
    public abstract void SetInfo(string info);
    public abstract void EquipmentUpdate(InteractState primaryState, InteractState secondaryState, InteractState reloadState);

    public static Weapon CreateWeaponByName(string name, Transform parent)
    {
        if (m_weapons.Count == 0) m_weapons = Resources.LoadAll<Weapon>("Weapons/").ToList();

        Debug.Log(m_weapons.Count);

        foreach( Weapon weapon in m_weapons )
        {
            if(weapon.name == name)
            {
                return Instantiate(weapon, parent);
            }
        }

        return null;
    }

    private static System.Collections.Generic.List<Weapon> m_weapons = new System.Collections.Generic.List<Weapon>(0);
}

public enum InteractState
{
    start,
    hold,
    end,
    none
}