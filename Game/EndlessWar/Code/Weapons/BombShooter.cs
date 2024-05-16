using UnityEngine;

public class BombShooter : PlayerCombat
{
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject _bombPrefabs;
    [SerializeField] private Transform _startPoint;
    [SerializeField] private float _throwForwardForce = 5f;
    [SerializeField] private float _throwUpwardForce = 5f;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    public override void Attack()
    {
        animator.SetTrigger("Throw");
    }

    // Animation event function
    public void ThrowBomb()
    {
        GameObject bomb = Instantiate(_bombPrefabs, _startPoint.position + transform.forward * 1f, Quaternion.identity);

        // Throwing Force
        Vector3 force = transform.forward * _throwForwardForce + transform.up * _throwUpwardForce;

        // Current character's movement velocity
        Vector3 velocity = transform.GetComponent<Rigidbody>().velocity;

        bomb.GetComponent<Rigidbody>().AddForce(force + velocity, ForceMode.Impulse);
        bomb.GetComponent<Bomb>().Explode();
    }

    #region Unused Method
    private Vector3 GetMouseHitPosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        Vector3 mousePosition = Vector3.zero;

        if (Physics.Raycast(ray, out hit))
        {
            mousePosition = hit.point;
            Debug.DrawRay(hit.point, hit.normal, Color.red, 3f);
        }

        return mousePosition;
    }

    #endregion
}
