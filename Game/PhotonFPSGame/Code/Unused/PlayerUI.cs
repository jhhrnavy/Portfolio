using UnityEngine.UI;
using TMPro;
using UnityEngine;
using Photon.Pun;

public class PlayerUI : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI _playerName;

    [SerializeField] Slider _health;

    [SerializeField]
    private Vector3 _screenOffset = new Vector3(0f, 30f, 0f);

    Health _target;
    Transform _targetTransform;
    Renderer _targetRenderer;
    float _targetHeight;

    CanvasGroup _canvasGroup;

    private void Awake()
    {
        _canvasGroup = this.GetComponent<CanvasGroup>();
        transform.SetParent(GameObject.Find("Canvas").GetComponent<Transform>(), false);
    }

    // Update is called once per frame
    void Update()
    {
        if (_target == null)
        {
            Destroy(this.gameObject);
            return;
        }

        //  헬스 UI 갱신..
        if (_health != null)
            _health.value = _target.CurHP;
    }

    private void LateUpdate()
    {
        //  시각화 설정..
        if (_targetRenderer != null)
            _canvasGroup.alpha = _targetRenderer.isVisible ? 1f : 0f;

        //  HP UI 위치 갱신..
        if (_targetTransform != null)
        {
            Vector3 targetPos = _targetTransform.position;
            targetPos.y += _targetHeight;

            transform.position = Camera.main.WorldToScreenPoint(targetPos) + _screenOffset;
        }
    }

    public void SetTarget(Health target)
    {
        if (target == null)
        {
            //Debug.LogError("<Color=Red><a>Missing</a></Color> PlayMakerManager target for PlayerUI.SetTarget.", this);
            return;
        }

        _target = target;
        _targetTransform = _target.GetComponent<Transform>();
        _targetRenderer = _target.GetComponentInChildren<Renderer>();

        CapsuleCollider targetCol = _target.GetComponent<CapsuleCollider>();
        //  타겟의 높이 정보 가져오기..
        if (targetCol != null)
            _targetHeight = targetCol.height;

        //  타겟의 이름 정복 가져오기..
        if (_playerName != null)
            _playerName.text = target.GetComponent<PhotonView>().Owner.NickName;

    }

}
