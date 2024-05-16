using ExitGames.Client.Photon.StructWrapping;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using Hashtable = ExitGames.Client.Photon.Hashtable;


public class WaitingRoomManager : MonoBehaviourPunCallbacks
{
    private const string TEAM_PROPERTY_KEY = "Team";

    // A팀과 B팀의 플레이어 리스트 목록
    [SerializeField] private TextMeshProUGUI[] _teamAPlayerNameTexts;
    [SerializeField] private TextMeshProUGUI[] _teamBPlayerNameTexts;

    #region Pun Callback Methods

    public override void OnEnable()
    {
        base.OnEnable();

        if(PhotonNetwork.IsConnected && PhotonNetwork.InRoom) // 게임 종료 이후 대기방으로 복귀
        {
            SetInitRoom();
            UpdatePlayersInRoom();
        }
    }

    public override void OnJoinedRoom()
    {
        SetInitRoom(); // 방 최대 인원 수에 맞추어 플레이어 칸 맞추기
        AssignTeam(); // 플레이어 팀 설정

    }

    public override void OnLeftRoom()
    {
        // 로컬 플레이어가 방을 나갈때 팀 정보 초기화
        ClearPlayerTeamProperty();
    }

    // 방에 새로운 플레이어가 입장했을 때 호출됨
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        UpdatePlayersInRoom(); // 플레이어 목록 갱신
    }

    // 플레이어가 방을 떠났을 때 호출됨
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        UpdatePlayersInRoom(); // 플레이어 목록 갱신
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {
        UpdatePlayersInRoom(); // 플레이어 목록 갱신
    }

    public void OnClickBttnStartGame()
    {
        // 방장만 게임 시작 가능
        if (PhotonNetwork.IsMasterClient)
            photonView.RPC("LoadGamePlayScene", RpcTarget.All);
    }

    #endregion

    #region RPC Methods

    [PunRPC]
    public void LoadGamePlayScene()
    {
        PhotonNetwork.LoadLevel("Room");
    }

    #endregion

    #region Private Methods

    public void AssignTeam()
    {
        int teamACount = 0;
        int teamBCount = 0;

        // 현재 방의 각 팀 인원 수 확인 및 참여 중인 플레이어 이름 설정
        foreach (Player player in PhotonNetwork.PlayerList)
        {
            //Debug.Log($"{player.NickName}");
            if (player.CustomProperties.ContainsKey(TEAM_PROPERTY_KEY))
            {
                var playerTeam = player.CustomProperties[TEAM_PROPERTY_KEY];
                //Debug.Log($"{player.NickName}. Team : {playerTeam}");

                if ((string)playerTeam == "A")
                {
                    teamACount++;
                }
                else if ((string)playerTeam == "B")
                {
                    teamBCount++;
                }
            }
        }

        // 인원수가 적은 팀에 새로 들어온 플레이어에게 팀 할당
        string assignedTeam = teamACount <= teamBCount ? "A" : "B";
        PhotonNetwork.LocalPlayer.SetCustomProperties(new Hashtable { { TEAM_PROPERTY_KEY, assignedTeam } });
        //Debug.Log($"Assigned to Team {assignedTeam}");
    }

    public void SetInitRoom()
    {
        // Clear all nickname text
        for (int i = 0; i < 4; i++)
        {
            _teamAPlayerNameTexts[i].text = "";
            _teamBPlayerNameTexts[i].text = "";

            _teamAPlayerNameTexts[i].transform.parent.gameObject.SetActive(false);
            _teamBPlayerNameTexts[i].transform.parent.gameObject.SetActive(false);
        }

        // 인원 수에 따라 플레이어 닉네임칸 활성화
        for (int i = 0; i < PhotonNetwork.CurrentRoom.MaxPlayers; i++)
        {
            if(i % 2 < 1)
            {
                _teamAPlayerNameTexts[i - (i/2)].transform.parent.gameObject.SetActive(true);
            }
            else if(i % 2 >= 1) 
            {
                _teamBPlayerNameTexts[i / 2].transform.parent.gameObject.SetActive(true);
            }
        }
    }

    public void UpdatePlayersInRoom()
    {
        //DebugPlayerList();

        int teamACount = 0; // 팀 A 플레이어 수
        int teamBCount = 0; // 팀 B 플레이어 수

        // 전체 플레이어 순회
        foreach (Player player in PhotonNetwork.PlayerList)
        {
            // 플레이어의 'Team' 커스텀 프로퍼티 가져오기
            object teamValue;
            if (player.CustomProperties.TryGetValue(TEAM_PROPERTY_KEY, out teamValue))
            {
                string team = teamValue as string;
                // 팀 A인 경우
                if (team == "A" && teamACount < _teamAPlayerNameTexts.Length)
                {
                    _teamAPlayerNameTexts[teamACount++].text = player.NickName; // 이름 설정
                }
                // 팀 B인 경우
                else if (team == "B" && teamBCount < _teamBPlayerNameTexts.Length)
                {
                    _teamBPlayerNameTexts[teamBCount++].text = player.NickName; // 이름 설정
                }
            }
        }

        // 플레이어 수가 부족한 경우 나머지 TextMeshProUGUI 컴포넌트를 비움
        for (int i = teamACount; i < _teamAPlayerNameTexts.Length; i++)
        {
            _teamAPlayerNameTexts[i].text = "";
        }
        for (int i = teamBCount; i < _teamBPlayerNameTexts.Length; i++)
        {
            _teamBPlayerNameTexts[i].text = "";
        }
    }
    private void ClearPlayerTeamProperty()
    {
        PhotonNetwork.LocalPlayer.CustomProperties.Clear();
    }

    #endregion

    #region //Debugging Methods

    private void DebugPlayerList()
    {
        string log = "";
        foreach (Player player in PhotonNetwork.PlayerList)
        {
            // 플레이어의 닉네임 가져오기
            string playerName = player.NickName;

            // 플레이어의 'Team' 커스텀 프로퍼티 가져오기
            object teamObj;
            string playerTeam = ""; // 기본값은 빈 문자열로 설정
            if (player.CustomProperties.TryGetValue(TEAM_PROPERTY_KEY, out teamObj))
            {
                playerTeam = teamObj as string; // 성공적으로 값을 얻었으면 문자열로 변환
            }

            // 디버그 로그에 플레이어 정보 출력
            log += $"Player Name: {playerName}, Team: {playerTeam}";
        }

        //Debug.Log(log);
    }

    #endregion

}
