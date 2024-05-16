using UnityEngine;
using Photon.Pun;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviourPun
{
    // Reference
    [SerializeField] private Health _playerHealth;
    [SerializeField] private Animator _anim;
    [SerializeField] private GameObject _playerUIPref;
    [SerializeField] private GameObject _body;
    [SerializeField] private GameObject _Arm;
    private Rigidbody _rb;

    // SpawnPoint
    [SerializeField] private Vector3 _spawnPosition;
    [SerializeField] private Quaternion _spawnRotation;

    // Movement
    [SerializeField] float _walkSpeed = 2f;
    [SerializeField] float _runSpeed = 4f;
    [SerializeField] float _moveSpeed;
    [SerializeField] float _jumpForce = 5f;

    // Player rotate
    [SerializeField] GameObject _playerCam;
    [SerializeField] private Transform _spine; // 상체
    [SerializeField] float _mouseSensitivity = 100f;
    private float xRotation = 0f;

    // Fire
    [SerializeField] Rifle _rifle;
    public float fireDelay = 0.1f;
    public float fireTimer = 0;
    public bool isFire = false;
    public bool canMove = true;

    // Run
    bool _isRun;
    [SerializeField] bool _isGrounded;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _playerHealth = GetComponent<Health>();
        _anim = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        if (_playerHealth)
        {
            _playerHealth.OnPlayerDeath += HandleDealth;
        }
    }

    private void OnDisable()
    {
        if (_playerHealth)
        {
            _playerHealth.OnPlayerDeath -= HandleDealth;
        }
    }

    private void Start()
    {
        //UnityEngine.SceneManagement.SceneManager.sceneLoaded += OnSceneLoaded;

        if (photonView.IsMine) // 로컬 플레이어는 총과 손만 렌더링
        {
            SetInit();
            _body.SetActive(false);
            _Arm.SetActive(false);
        }
    }

    private void Update()
    {
        if (!photonView.IsMine || !canMove || EventSystem.current.IsPointerOverGameObject()) return;

        Move();
        Jump();
        Rotate();
        Fire();
        Reload();

    }

    private void FixedUpdate()
    {
        // 객체와 충돌 이후 회절하는 상황 방지
        _rb.angularVelocity = Vector3.zero;
    }


    #region Synchronization / RPC Methods

    public void SetInit()
    {
        if (_playerHealth != null)
        {
            photonView.RPC("UpdateHealth", RpcTarget.All, _playerHealth.MaxHp);
        }
        _rifle.SetInit();
    }

    public void MoveToSpawnPoint()
    {
        photonView.RPC("MoveToSpawnPointRPC", RpcTarget.All);
    }

    [PunRPC]
    public void SetSpawnPointRPC(Vector3 position, Quaternion rotation)
    {
        _spawnPosition = position;
        _spawnRotation = rotation;
    }

    [PunRPC]
    public void AddPlayerToPlayerList(int viewID)
    {
        GameManager.Instance?.AddPlayerToPlayerList(viewID);
    }

    [PunRPC]
    public void MoveToSpawnPointRPC()
    {
        transform.position = _spawnPosition;
        transform.rotation = _spawnRotation;
    }

    #endregion

    #region Player Actions

    private void Rotate()
    {
        float mouseX = Input.GetAxis("Mouse X") * _mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * _mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        _playerCam.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);
    }

    private void Move()
    {
        // Horizontal Move
        float v = Input.GetAxis("Vertical");
        _anim.SetFloat("Vertical", v);

        // Vertical Move
        float h = Input.GetAxis("Horizontal");
        _anim.SetFloat("Horizontal", h);


        if (_isRun)
        {
            _moveSpeed = _runSpeed;
        }
        else
            _moveSpeed = _walkSpeed;

        transform.Translate(new Vector3(h, 0, v) * Time.deltaTime * _moveSpeed);

        // Run
        if (Input.GetKeyDown(KeyCode.LeftShift) && !_isRun)
        {
            _anim.SetBool("Run", true);
            _isRun = true;
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift) && _isRun)
        {
            _anim.SetBool("Run", false);
            _isRun = false;
        }
    }

    private void Fire()
    {
        if (_rifle.CurrentAmmo <= 0 || _rifle.IsReloading) return;

        // Fire
        float fire = Input.GetAxis("Fire1");
        fireTimer -= Time.deltaTime;
        if (fire == 1 && fireTimer < 0)
        {
            isFire = true;
        }
        if (isFire)
        {
            fireTimer = fireDelay;

            //_rifle.GetComponent<PhotonView>().RPC("Fire", RpcTarget.All);

            _rifle.Fire();

            _anim.SetTrigger("Fire");
            isFire = false;
        }
    }

    private void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && _isGrounded)
        {
            _rb.AddForce(Vector3.up * _jumpForce, ForceMode.Impulse);
            _isGrounded = false;
        }
    }

    private void Reload()
    {
        if(Input.GetKeyDown(KeyCode.R) && !_rifle.IsReloading)
            _rifle.Reload();
    }

    private void HandleDealth()
    {
        // Respawn player
        //Debug.Log("HandleDealth Call");

        if (GetComponent<FlagHolder>())
        {
            photonView.RPC("DropFlag", RpcTarget.All);
        }

        SetInit();
        GameManager.Instance.InGame.RespawnPlayer(this);
    }

    #endregion

    // Unused
    //void OnSceneLoaded(UnityEngine.SceneManagement.Scene scene, UnityEngine.SceneManagement.LoadSceneMode loadingMode)
    //{
    //    this.CalledOnLevelWasLoaded();
    //}

    //void CalledOnLevelWasLoaded()
    //{
    //    if(this != null)
    //    {
    //        GameObject _uiGo = Instantiate(this._playerUIPref);
    //        _uiGo.SendMessage("SetTarget", GetComponent<Health>(), SendMessageOptions.RequireReceiver);
    //    }
    //}

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            _isGrounded = true;
        }
    }

}
