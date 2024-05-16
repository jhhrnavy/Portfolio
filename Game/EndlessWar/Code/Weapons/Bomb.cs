using System.Collections;
using UnityEngine;

public class Bomb : NewWeapon
{
    [SerializeField] GameObject _explosionEffect;
    [SerializeField] private float _countdown = 3f;
    [SerializeField] private float _radius = 5f;
    [SerializeField] private float _explosionForce = 700f;
    [SerializeField] private LayerMask _targetLayer;

    public void Explode()
    {
        StartCoroutine(ExplodeRountine());
    }

    private void PerformExplode()
    {
        GameObject expEfx = Instantiate(_explosionEffect, transform.position, transform.rotation);

        Collider[] colls = Physics.OverlapSphere(transform.position, _radius, _targetLayer);

        foreach (Collider nearbyObject in colls)
        {
            if (nearbyObject.CompareTag("Obstacle"))
                continue;

            Rigidbody rb = nearbyObject.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddExplosionForce(_explosionForce, transform.position, _radius);
                Destroy(nearbyObject.gameObject, 1f);
            }
        }
        Destroy(expEfx, 0.5f);
        Destroy(gameObject);
    }

    public IEnumerator ExplodeRountine()
    {
        yield return new WaitForSeconds(_countdown);
        PerformExplode();
    }
}
