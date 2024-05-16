using Photon.Pun;
using UnityEngine;

public class Bullet : MonoBehaviourPun
{
    [SerializeField] public GameObject hitVFX;
    [SerializeField] private float _destroyTime = 4f;
    [SerializeField] private float _buleltSpeed = 15f;
    private double _startTime; 

    private void Start()
    {
        _startTime = PhotonNetwork.Time;
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null && photonView.IsMine)
            rb.AddForce(transform.forward * _buleltSpeed, ForceMode.Impulse);
    }

    private void Update()
    {
        double elapsedTime = PhotonNetwork.Time - _startTime;
        if(elapsedTime >= _destroyTime)
        {
            PhotonNetwork.Destroy(gameObject); // _destroyTime 이후 객체 제거
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (photonView.IsMine)
        {
            PhotonNetwork.Instantiate(hitVFX.name ,transform.position, transform.rotation * Quaternion.Euler(0f,180f,0f));

            if (other.CompareTag("Player"))
            {
                other.GetComponent<Health>().TakeDamage(3);
            }
            PhotonNetwork.Destroy(gameObject);
        }
    }
}
