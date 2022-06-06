using UnityEngine;

public class Target : MonoBehaviour , IDamageable
{
    [SerializeField] private Animation m_animation;

    private bool m_isDown;

    public void TakeDamage(int damage)
    {
        if (m_isDown == true) return;

        m_animation.Play("A_Target_Down");
        m_isDown = true;
        Invoke("Up",3f);
    }

    private void Up()
    {
        m_isDown = false;
        m_animation.Play("A_Target_Up");
    }
}
