using Photon.Pun;
using System;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviourPunCallbacks, IPunObservable
{
    private Animator _anim;
    private string hitAnim = "GetDamaged";
    [SerializeField] private Slider _healthBar;
    [SerializeField] private float _maxHp = 10f;
    [SerializeField] private float _curHp;
    [SerializeField] private bool _isDead;

    public event Action OnPlayerDeath;

    #region Properties

    public float MaxHp => _maxHp;
    public float CurHP => _curHp;
    public bool IsDead { get => _isDead; }

    #endregion

    private void Awake()
    {
        _anim = GetComponent<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {
        _curHp = _maxHp;
        SetHealthBarDisplay(_maxHp, _curHp); // LocalPlayer의 HP바 UI 활성화
    }

    public void TakeDamage(int damage)
    {
        _curHp -= damage;
        photonView.RPC("UpdateHealth", RpcTarget.All, _curHp);
    }

    [PunRPC]
    public void UpdateHealth(float health)
    {
        _curHp = health;
        _anim.SetTrigger(hitAnim);
        UpdateHealthBarDisplay(_curHp);

        if (_curHp <= 0)
        {
            //Debug.Log("Player is dead.");
            OnPlayerDeath?.Invoke();
        }
    }

    private void SetHealthBarDisplay(float maxHp, float curHp)
    {
        if (photonView.IsMine)
        {
            _healthBar.maxValue = maxHp;
            _healthBar.minValue = 0f;
            _healthBar.value = curHp;
        }
        else
        {
            _healthBar.gameObject.SetActive(false);
        }
    }

    private void UpdateHealthBarDisplay(float curHp)
    {
        if(_healthBar.IsActive())
            _healthBar.value = curHp;
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if(stream.IsWriting)
        {
            stream.SendNext(_curHp);
        }
        else
        {
            _curHp = (float)stream.ReceiveNext();
        }
    }
}
