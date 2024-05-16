using Photon.Pun;
using UnityEngine;

public class Flag : MonoBehaviourPun
{
    [SerializeField] private Vector3 _initPosition;

    // Start is called before the first frame update
    void Start()
    {
        _initPosition = transform.position;
    }

    [PunRPC]
    public void ResetPosition()
    {
        //Debug.Log($"RPC ResetPosition call, respawnPosition : {_initPosition}");
        transform.position = _initPosition;
    }
}
