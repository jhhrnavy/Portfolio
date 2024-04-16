using UnityEngine;

public class BigBomb : MonoBehaviour
{
    [SerializeField] private GameObject _explosionEffect;

    [Header("[ Explosion ]")] 
    [SerializeField] private float _range = 1f;
    [SerializeField] private float _timer = 0.8f;

    // Update is called once per frame
    void Update()
    {
        _timer -= Time.deltaTime;

        if(_timer < 0)
        {
            // vfx
            var explosionVfx = Instantiate(_explosionEffect,transform.position,Quaternion.identity);
            Destroy(explosionVfx, 0.5f);

            Collider[] cols = Physics.OverlapSphere(transform.position, _range, 1 << 7);
            if (cols.Length > 0)
            {
                for (int i = 0; i < cols.Length; i++)
                {
                    if (cols[i].CompareTag("Enemy"))
                    {
                        Enemy _eTmp = cols[i].GetComponent<Enemy>();
                        _eTmp.GetDamaged(10);
                    }
                }
            }

            Destroy(gameObject);
        }

    }
}
