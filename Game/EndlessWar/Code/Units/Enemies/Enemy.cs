using System.Collections;
using UnityEngine;
using UnityEngine.AI;
public class Enemy : MonoBehaviour
{
    private IState _currentState;
    private FieldOfView _fow;
    private Rigidbody _rb;
    private NavMeshAgent _pathfinder;

    public NewGun currentGun;

    public int hp = 10;
    public float moveSpeed = 5f;
    public float rotSpeed = 5f;

    public float minDotThreshold = 0.1f;

    private bool _isDead = false;
    public bool isfacingTarget = false;
    public bool hasArrivedAtLastPoint = false;

    public FieldOfView Fow { get => _fow; set => _fow = value; }

    private void Start()
    {
        _currentState = new IdleState(this);
        _fow = gameObject.GetComponent<FieldOfView>();
        _rb = gameObject.GetComponent<Rigidbody>();
        _pathfinder = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        _currentState.Execute();
    }

    private void FixedUpdate()
    {
        _currentState.PhysicsExecute();
    }

    public void ChangeState(IState newState)
    {
        _currentState.Exit();

        _currentState = newState;

        _currentState.Enter();
    }

    public void GetHit(int damage)
    {
        if (_isDead) return;

        hp -= damage;

        if (hp <= 0)
        {
            _isDead = true;
            ChangeState(new DeadState(this));
        }
    }

    public void Move()
    {
        _pathfinder.SetDestination(_fow.targetLastPosition);

        if (_pathfinder.velocity == Vector3.zero)
        {
            hasArrivedAtLastPoint = true;
        }
    }

    public void StopMoving()
    {
        _pathfinder.isStopped = true;
    }

    public void StartMoving()
    {
        _pathfinder.isStopped = false;
    }

    public void Rotate()
    {
        Vector3 direction = _fow.targetLastPosition - transform.position;
        direction.y = 0f;
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        Quaternion rotation = Quaternion.Lerp(_rb.rotation, targetRotation, rotSpeed * Time.fixedDeltaTime);
        _rb.MoveRotation(rotation);
        float dot = Vector3.Dot(transform.forward, direction.normalized);
        if (dot >= 0.95f)
            isfacingTarget = true;
        else
            isfacingTarget = false;
    }

    public void Die()
    {
        StopMoving();
        DropWeapon();
        StartCoroutine(DelayedDie());
    }

    public void DropWeapon()
    {
        // Physics Drop weapon
        currentGun.transform.SetParent(null);
        var gunRb = currentGun.GetComponent<Rigidbody>();
        gunRb.isKinematic = false;
        currentGun.GetComponent<Collider>().isTrigger = false;
        gunRb.velocity = GetComponent<Rigidbody>().velocity;
        gunRb.AddForce(transform.forward * 4f, ForceMode.Impulse); // Throwing force forward
        gunRb.AddForce(transform.up * 3f, ForceMode.Impulse); // Throwing force upward
        currentGun.gameObject.layer = 10; // Pickable
        currentGun = null;
    }

    private IEnumerator DelayedDie()
    {
        yield return new WaitForSeconds(1f);
        gameObject.SetActive(false);
    }

}
