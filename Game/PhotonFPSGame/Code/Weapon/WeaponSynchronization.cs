using Photon.Pun;
using UnityEngine;

public class WeaponSynchronization : MonoBehaviour, IPunObservable
{
    [SerializeField] private float _lerpSpeed = 20f; // ���� �ӵ�
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
        // ��Ʈ��ũ�� ���� ����ȭ�� ��ġ�� ȸ������ �ε巴�� �̵�
        if (!photonView.IsMine)
        {
            //Debug.Log($"NetworkRotation : {_networkRotationEuler}");
            //transform.localPosition = Vector3.Lerp(transform.localPosition, _networkPosition, Time.deltaTime * _lerpSpeed);

            // x�� ���� ���
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
            // �����͸� ���� ��, ���� ��ġ�� ȸ���� ����
            //stream.SendNext(transform.localPosition);
            stream.SendNext(transform.rotation.eulerAngles);
            //Debug.Log($" Send NetworkRotation : {transform.rotation.eulerAngles}");
        }
        else
        {
            // �����͸� ���� ��, ��ġ�� ȸ���� ������Ʈ
            //_networkPosition = (Vector3)stream.ReceiveNext();
            _networkRotationEuler = (Vector3)stream.ReceiveNext();
            //Debug.Log($"Receive NetworkRotation : {_networkRotationEuler}");
        }
    }
}
