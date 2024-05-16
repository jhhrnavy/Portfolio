using UnityEngine;
using Photon.Pun;
public class Area : MonoBehaviour
{
    [SerializeField] private TeamType team;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && other.GetComponent<FlagHolder>().IsHold)
        {
            //Debug.Log($"Flag Goal : IsMasterClient {PhotonNetwork.IsMasterClient}, {team}");

            if (PhotonNetwork.IsMasterClient)
            {
                // 스코어 갱신
                GameManager.Instance.InGame.ScoreManager?.AddScore(team);

                var flagHolder = other.GetComponent<FlagHolder>();
                var flag = flagHolder.GetFlag(); // 깃발 객체 가져오기

                if(flag != null)
                {
                    flag.photonView.RPC("ResetPosition", RpcTarget.All);
                    flagHolder.photonView.RPC("DropFlag", RpcTarget.All);
                }
            }
        }
    }
}
