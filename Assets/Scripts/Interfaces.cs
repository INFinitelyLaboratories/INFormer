using UnityEngine;

public interface IDamageable
{
    public void TakeDamage(int damage);
}

public interface IWeaponOnwer
{
    public Transform GetCameraTransform();
}