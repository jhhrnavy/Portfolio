using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private WaitingRoomManager _roomManager;
    [SerializeField] LobbyUI _lobbyUI;
    [SerializeField] GameObject _playerPref;
    List<RoomInfo> _myRoomList = new List<RoomInfo>();

    private void Awake()
    {
        PhotonNetwork.LogLevel = PunLogLevel.Full;
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.None; // 커서 락해제

        if (!PhotonNetwork.IsConnected)
            _lobbyUI.ShowPanel(PanelType.Login);
        else if(PhotonNetwork.IsConnected && PhotonNetwork.InRoom)
        {
            JoinWaitingRoom();
        }
    }

    //public override void OnEnable()
    //{
    //    base.OnEnable(); // OnEnable, OnDisable 를 오버라이드 한 경우 베이스 클래스 메소드를 항상 호출해야 함.
    //    Debug.Log(PhotonNetwork.InRoom);
    //    if (PhotonNetwork.InRoom)
    //    {
    //        Debug.Log($"Show Room");
    //        _lobbyUI.ShowPanel(PanelType.Room);
    //    }
    //    else
    //    {

    //    }
    //}

    private void Update()
    {
        _lobbyUI.StateText = PhotonNetwork.NetworkClientState.ToString();
        _lobbyUI.UpdateUIPlayerCountInLobby(PhotonNetwork.CountOfPlayersInRooms, PhotonNetwork.CountOfPlayers);

        if (Input.GetKeyDown(KeyCode.Escape))
            _lobbyUI.ShowPauseMenu();
    }

    #region Pun Callback Methods
    public override void OnDisconnected(DisconnectCause cause)
    {
        base.OnDisconnected(cause);
    }

    public override void OnConnected()
    {
        base.OnConnected();
        //Debug.Log("OnConnected..");
        Debug.Log(PhotonNetwork.IsConnected);
    }

    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
        PhotonNetwork.LocalPlayer.NickName = _lobbyUI.NickInput;
        //Debug.Log($"LocalPlayer.NickName : {PhotonNetwork.LocalPlayer.NickName}");
        var playerData = PlayerData.Instance;

        // 처음 접속 시 닉네임 저장
        if (playerData.GetPlayerName() == string.Empty)
            PlayerData.Instance.SetPlayerName(PhotonNetwork.LocalPlayer.NickName);

        //PhotonNetwork.NetworkingClient.EnableLobbyStatistics = true;

        PhotonNetwork.JoinLobby();

        _lobbyUI.ShowPanel(PanelType.Lobby);
    }

    public override void OnJoinedLobby()
    {
        base.OnJoinedLobby();
        //Debug.Log("Loading Lobby...");
        // 로비에 재입장 시 로컬플레이어 닉네임 재설정
        if (PhotonNetwork.LocalPlayer.NickName == string.Empty)
        {
            PhotonNetwork.LocalPlayer.NickName = PlayerData.Instance.GetPlayerName();

        }
        _lobbyUI.NickText = PhotonNetwork.LocalPlayer.NickName;
    }

    public override void OnCreatedRoom()
    {
    }

    public override void OnJoinedRoom()
    {
        _lobbyUI.ShowPanel(PanelType.Room);
        //PhotonNetwork.LoadLevel("Room");
    }

    public override void OnLeftRoom()
    {
        _lobbyUI.ShowPanel(PanelType.Lobby);
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        base.OnRoomListUpdate(roomList);

        int roomCount = roomList.Count;

        for (int i = 0; i < roomCount; i++)
        {
            if (!roomList[i].RemovedFromList)
            {
                if (!_myRoomList.Contains(roomList[i]))
                    _myRoomList.Add(roomList[i]);
                else
                    _myRoomList[_myRoomList.IndexOf(roomList[i])] = roomList[i];
            }

            else if (_myRoomList.IndexOf(roomList[i]) != -1)
            {
                _myRoomList.RemoveAt(_myRoomList.IndexOf(roomList[i]));
            }
        }

        UpdateRoomList();
    }

    #endregion

    #region On Click Bttn Methods

    public void OnClickBttnCreateRoom()
    {
        if (_lobbyUI.RoomNameInput == string.Empty)
        {
            //Debug.Log("방제목을 입력해주세요");
            return;
        }

        PhotonNetwork.CreateRoom(_lobbyUI.RoomNameInput, new RoomOptions { MaxPlayers = (byte)(_lobbyUI.PlayerCountDropdown.value + 2) });
    }

    public void OnClickBttnLeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }

    #endregion

    public void Connect()
    {
        //Debug.Log("Connecting...");

        Debug.Log(PhotonNetwork.IsConnected);
        if (PhotonNetwork.IsConnected)
            PhotonNetwork.JoinLobby();
        else
        {
            if (_lobbyUI.NickInput == string.Empty)
            {
                Debug.Log("닉네임을 입력해주세요");
                return;
            }
            //Debug.Log("ConnectUsingSettings");
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    private void UpdateRoomList()
    {
        _lobbyUI.ClearAllRoomButton();

        for (int i = 0; i < _myRoomList.Count; i++)
        {
            _lobbyUI.CreateRoomButton(_myRoomList[i].Name, _myRoomList[i].MaxPlayers);
        }
    }

    public void JoinWaitingRoom()
    {
        Debug.Log($"Show Room");

        Cursor.lockState = CursorLockMode.None; // 커서 락해제
        _lobbyUI.ShowPanel(PanelType.Room);
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif    
    }

}
