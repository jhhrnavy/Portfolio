using ExitGames.Client.Photon.StructWrapping;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using Hashtable = ExitGames.Client.Photon.Hashtable;


public class WaitingRoomManager : MonoBehaviourPunCallbacks
{
    private const string TEAM_PROPERTY_KEY = "Team";

    // A���� B���� �÷��̾� ����Ʈ ���
    [SerializeField] private TextMeshProUGUI[] _teamAPlayerNameTexts;
    [SerializeField] private TextMeshProUGUI[] _teamBPlayerNameTexts;

    #region Pun Callback Methods

    public override void OnEnable()
    {
        base.OnEnable();

        if(PhotonNetwork.IsConnected && PhotonNetwork.InRoom) // ���� ���� ���� �������� ����
        {
            SetInitRoom();
            UpdatePlayersInRoom();
        }
    }

    public override void OnJoinedRoom()
    {
        SetInitRoom(); // �� �ִ� �ο� ���� ���߾� �÷��̾� ĭ ���߱�
        AssignTeam(); // �÷��̾� �� ����

    }

    public override void OnLeftRoom()
    {
        // ���� �÷��̾ ���� ������ �� ���� �ʱ�ȭ
        ClearPlayerTeamProperty();
    }

    // �濡 ���ο� �÷��̾ �������� �� ȣ���
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        UpdatePlayersInRoom(); // �÷��̾� ��� ����
    }

    // �÷��̾ ���� ������ �� ȣ���
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        UpdatePlayersInRoom(); // �÷��̾� ��� ����
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {
        UpdatePlayersInRoom(); // �÷��̾� ��� ����
    }

    public void OnClickBttnStartGame()
    {
        // ���常 ���� ���� ����
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

        // ���� ���� �� �� �ο� �� Ȯ�� �� ���� ���� �÷��̾� �̸� ����
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

        // �ο����� ���� ���� ���� ���� �÷��̾�� �� �Ҵ�
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

        // �ο� ���� ���� �÷��̾� �г���ĭ Ȱ��ȭ
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

        int teamACount = 0; // �� A �÷��̾� ��
        int teamBCount = 0; // �� B �÷��̾� ��

        // ��ü �÷��̾� ��ȸ
        foreach (Player player in PhotonNetwork.PlayerList)
        {
            // �÷��̾��� 'Team' Ŀ���� ������Ƽ ��������
            object teamValue;
            if (player.CustomProperties.TryGetValue(TEAM_PROPERTY_KEY, out teamValue))
            {
                string team = teamValue as string;
                // �� A�� ���
                if (team == "A" && teamACount < _teamAPlayerNameTexts.Length)
                {
                    _teamAPlayerNameTexts[teamACount++].text = player.NickName; // �̸� ����
                }
                // �� B�� ���
                else if (team == "B" && teamBCount < _teamBPlayerNameTexts.Length)
                {
                    _teamBPlayerNameTexts[teamBCount++].text = player.NickName; // �̸� ����
                }
            }
        }

        // �÷��̾� ���� ������ ��� ������ TextMeshProUGUI ������Ʈ�� ���
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
            // �÷��̾��� �г��� ��������
            string playerName = player.NickName;

            // �÷��̾��� 'Team' Ŀ���� ������Ƽ ��������
            object teamObj;
            string playerTeam = ""; // �⺻���� �� ���ڿ��� ����
            if (player.CustomProperties.TryGetValue(TEAM_PROPERTY_KEY, out teamObj))
            {
                playerTeam = teamObj as string; // ���������� ���� ������� ���ڿ��� ��ȯ
            }

            // ����� �α׿� �÷��̾� ���� ���
            log += $"Player Name: {playerName}, Team: {playerTeam}";
        }

        //Debug.Log(log);
    }

    #endregion

}
