using UnityEngine;

public class ParticleManager : MonoBehaviour
{
    private static System.Collections.Generic.List<ParticleSystem> m_particles = new System.Collections.Generic.List<ParticleSystem>();

    private void Awake()
    {
        if (FindObjectsOfType<ParticleManager>().Length > 1) throw new UnityException("Не допускается использование более одного менеджера частиц!");

        foreach (Transform child in transform)
        {
            if (child.TryGetComponent<ParticleSystem>(out var particle))
            {
                m_particles.Add(particle);
            }
        }
    }

    public static void PlayParticle(string particleName, Vector3 position, Vector3 rotation)
    {
        foreach ( ParticleSystem particle in m_particles )
        {
            if( particle.name == particleName )
            {
                particle.transform.position = position;
                particle.transform.eulerAngles = rotation;

                particle.Play(false);
            }
        }
    }
}
