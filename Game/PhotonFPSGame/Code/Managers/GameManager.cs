using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

/// <summary>
/// 게임 상태 관리
/// </summary>
public class GameManager : MonoBehaviourPunCallbacks
{
    public static GameManager Instance;

    private IGameState _currentState;
    private InGameController _inGameController;

    public InGameController InGame { get => _inGameController;}

    private void Awake()
    {
        _inGameController = GameObject.Find("InGameController").GetComponent<InGameController>();

        if (Instance == null)
            Instance = this;
    }

    #region Start And Update

    private void Start()
    {
        if (!PhotonNetwork.IsConnected)
        {
            PhotonNetwork.LoadLevel("Lobby");
            return;
        }
        ChangeState(new StartState(this));
    }

    private void Update()
    {
        if(!PhotonNetwork.IsConnected) return;

        // Game State Update
        if (PhotonNetwork.IsMasterClient)
            _currentState.UpdateState();
    }

    #endregion

    #region MonoBehaviourPunCallbacks

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        // 게임을 진행중인 플레이어 리스트 갱신
        RemovePlayerInPlayerList(otherPlayer);
    }

    public override void OnLeftRoom()
    {
        PhotonNetwork.LoadLevel("Lobby");
    }

    #endregion

    #region Game State Management Methods

    public void ChangeState(IGameState newState)
    {
        if (!PhotonNetwork.IsMasterClient) return;

        _currentState?.ExitState();
        _currentState = newState;
        _currentState.EnterState();
    }

    #endregion

    #region Players Management Methods

    public void AddPlayerToPlayerList(int viewID)
    {
        // ViewID를 사용하여 해당 플레이어 검색
        PhotonView targetView = PhotonView.Find(viewID);
        if(targetView != null)
        {
            PlayerController playerController = targetView.GetComponent<PlayerController>();
            playerController.gameObject.name = targetView.Owner.NickName;

            if (playerController != null) 
            {
                var teamType = playerController.GetComponent<PlayerTeam>().Type;
                InGame.TeamList[(int)teamType].AddPlayers(playerController);
            }
        }
    }

    public void RemovePlayerInPlayerList(Player player)
    {
        foreach (var team in InGame.TeamList)
        {
            team.RemovePlayer(player);
        }
    }

    public bool CheckPlayersCount()
    {
        int totalCount = 0; // 스폰된 플레이어 수
        foreach (var team in InGame.TeamList)
        {
            totalCount += team.Players.Count;
        }
        if (PhotonNetwork.CurrentRoom.PlayerCount == totalCount)
        {
            return true;
        }
        return false;
    }

    #endregion

    public void GameStart()
    {
        InGame.CountdownTimer.StartCountDown();
    }

    public void OnClickBttnLeaveRoom()
    {
        PhotonNetwork.LocalPlayer.CustomProperties.Clear(); // 방 퇴장 시 플레이어의 커스텀 속성 정보 초기화
        PhotonNetwork.LeaveRoom(this);
    }
}

public enum GameState
{
    Start = 0,
    Waiting,
    Preparation,
    Playing,
    RoundEnd,
    GameOver
}
