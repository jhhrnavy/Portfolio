using UnityEngine;

public class DestroyParticle : MonoBehaviour
{
    private ParticleSystem _ps;

    private void Start()
    {
        _ps = GetComponent<ParticleSystem>();
    }

    private void Update()
    {
        if(_ps && !_ps.IsAlive())
        {
            Destroy(gameObject);
        }
    }
}
