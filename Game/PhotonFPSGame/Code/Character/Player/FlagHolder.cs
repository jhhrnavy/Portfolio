using UnityEngine;
using Photon.Pun;

public class FlagHolder : MonoBehaviourPunCallbacks
{
    // Player의 component로 플레이어가 깃발을 가질 수 있게 하는 클래스
    [SerializeField] private float _rayLength = 3f;
    [SerializeField] Transform _holdTransform;
    [SerializeField] GameObject _targetFlag;

    [SerializeField] bool _isHold = false;

    public bool IsHold { get => _isHold; }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.F) && photonView.IsMine)
        {
            CheckFlag();
        }
    }

    private void CheckFlag()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, _rayLength))
        {
            if (hit.collider.CompareTag("Flag"))
            {
                PhotonView hitPhotonView = hit.collider.gameObject.GetPhotonView();
                if(hitPhotonView != null)
                {
                    photonView.RPC("HoldFlag", RpcTarget.All, hitPhotonView.ViewID);
                }
            }
        }
    }

    [PunRPC]
    public void HoldFlag(int targetViewID)
    {
        _isHold = true;
        _targetFlag = PhotonView.Find(targetViewID).gameObject;
        _targetFlag.transform.position = _holdTransform.position;
        _targetFlag.transform.localScale = Vector3.one * 0.3f;
        _targetFlag.transform.SetParent(_holdTransform.parent);
    }

    [PunRPC]
    public void DropFlag()
    {
        if (_isHold && _targetFlag != null)
        {
            _targetFlag.transform.parent = null;
            _isHold = false;
            _targetFlag.transform.localScale = Vector3.one;
            _targetFlag = null;
            //_targetFlag.transform.position = transform.position + transform.forward * 2f + Vector3.up * 1f;
        }
    }

    public Flag GetFlag()
    {
        if(_isHold && _targetFlag != null)
        {
            return _targetFlag.GetComponent<Flag>();
        }
        return null;
    }
}
