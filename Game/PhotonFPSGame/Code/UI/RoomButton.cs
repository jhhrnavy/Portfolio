using UnityEngine;
using TMPro;
using Photon.Pun;

public class RoomButton : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _roomNameTmp;
    [SerializeField] TextMeshProUGUI _maxPlayerCount;

    public void SetRoomData(string roomName, int maxPlayerCount)
    {
        _roomNameTmp.text = roomName;
        _maxPlayerCount.text = maxPlayerCount.ToString();
    }

    public void OnClickBttnJoinRoom()
    {
        PhotonNetwork.JoinRoom(_roomNameTmp.text);
    }
}
