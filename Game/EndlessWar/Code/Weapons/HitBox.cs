using UnityEngine;

public class HitBox : MonoBehaviour
{
    [SerializeField]
    private bool _isHit = false;
    [SerializeField]
    private Collider _hitCol;

    private void Update()
    {
        if (_hitCol == null)
        {
            _hitCol = null;
            _isHit = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            _isHit = true;
            _hitCol = other;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            _isHit = true;
            _hitCol = other;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            _isHit = false;
            _hitCol = null;
        }
    }

    public bool IsHit()
    {
        return _isHit;
    }

    public Collider GetDetectedCollider()
    {
        return _hitCol;
    }
}
