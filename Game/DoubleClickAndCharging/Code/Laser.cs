using System.Collections;
using UnityEngine;

public class Laser : MonoBehaviour
{
    #region Fields

    [SerializeField] private int damage = 3;
    [SerializeField] private float delay = 0.5f;
    private LineRenderer _laserLineRenderer;
    private Collider _coll;
    private bool _isFireLaser;
    private float _hitDelayTimer = 0f;

    #endregion

    #region Properties
    public bool IsFireLaser { get => _isFireLaser; }

    #endregion

    private void Awake()
    {
        _laserLineRenderer = GetComponent<LineRenderer>();
        _laserLineRenderer.positionCount = 2;
        _laserLineRenderer.enabled = false;
        _coll = GetComponent<Collider>();
        _coll.enabled = false;
        _isFireLaser = false;
    }

    public void Shot(Transform firePos)
    {
        StartCoroutine(ShotEffect(firePos));
    }

    private IEnumerator ShotEffect(Transform firePos)
    {
        _laserLineRenderer.SetPosition(0, firePos.position);
        _laserLineRenderer.SetPosition(1, firePos.position + firePos.forward * 100f);
        _laserLineRenderer.enabled = true;
        _coll.enabled = true;
        _isFireLaser = true;
        yield return new WaitForSeconds(2f);
        _laserLineRenderer.enabled = false;
        _coll.enabled = false;
        _isFireLaser = false;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            _hitDelayTimer -= Time.deltaTime;

            if (_hitDelayTimer <= 0)
            {
                other.GetComponent<Enemy>().GetDamaged(damage);
                _hitDelayTimer = delay;
            }
        }
    }

}
