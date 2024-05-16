using Photon.Pun;
using UnityEngine;

public class WeaponSynchronization : MonoBehaviour, IPunObservable
{
    [SerializeField] private float _lerpSpeed = 20f; // 보간 속도
    [SerializeField] private Transform _camera;
    private Vector3 _networkPosition;
    private Vector3 _networkRotationEuler;
    private PhotonView photonView;

    public void Awake()
    {
        photonView = GetComponent<PhotonView>();
    }

    public void Update()
    {
        // 네트워크를 통해 동기화된 위치와 회전으로 부드럽게 이동
        if (!photonView.IsMine)
        {
            //Debug.Log($"NetworkRotation : {_networkRotationEuler}");
            //transform.localPosition = Vector3.Lerp(transform.localPosition, _networkPosition, Time.deltaTime * _lerpSpeed);

            // x축 값만 허용
            _networkRotationEuler.y = 0f;
            _networkRotationEuler.z = 0f;
            Quaternion rotation = Quaternion.Euler(_networkRotationEuler);

            transform.localRotation = Quaternion.Lerp(transform.localRotation, rotation, Time.deltaTime * _lerpSpeed);
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            // 데이터를 보낼 때, 현재 위치와 회전을 전송
            //stream.SendNext(transform.localPosition);
            stream.SendNext(transform.rotation.eulerAngles);
            //Debug.Log($" Send NetworkRotation : {transform.rotation.eulerAngles}");
        }
        else
        {
            // 데이터를 받을 때, 위치와 회전을 업데이트
            //_networkPosition = (Vector3)stream.ReceiveNext();
            _networkRotationEuler = (Vector3)stream.ReceiveNext();
            //Debug.Log($"Receive NetworkRotation : {_networkRotationEuler}");
        }
    }
}
