using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private GameObject _bulletImpactVfx;
    int damage = 3;

    private void Start()
    {
        Destroy(gameObject, 2f);
    }
    private void SpawnVfx()
    {
        if(_bulletImpactVfx != null)
        {
            var vfx = Instantiate(_bulletImpactVfx, transform.position, transform.rotation * Quaternion.Euler(0,180f,0));
            Destroy(vfx, 0.5f);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            other.GetComponent<Enemy>().GetHit(damage);
            Destroy(gameObject);
            SpawnVfx();
        }
        else if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerController>().GetHit(damage);
            Destroy(gameObject);
            SpawnVfx();
        }
        else if (other.CompareTag("Obstacle"))
        {
            Destroy(gameObject);
            SpawnVfx();
        }
    }
}
