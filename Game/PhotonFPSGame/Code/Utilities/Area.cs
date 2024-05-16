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
                // ���ھ� ����
                GameManager.Instance.InGame.ScoreManager?.AddScore(team);

                var flagHolder = other.GetComponent<FlagHolder>();
                var flag = flagHolder.GetFlag(); // ��� ��ü ��������

                if(flag != null)
                {
                    flag.photonView.RPC("ResetPosition", RpcTarget.All);
                    flagHolder.photonView.RPC("DropFlag", RpcTarget.All);
                }
            }
        }
    }
}
