using JetBrains.Annotations;
using UnityEngine;

public class Goal : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Congratulations! You win!");
            UIManager.Instance?.Stopwatch.StopStopwatch();
            GameManager.Instance.PauseOrContinue();

            int reward = (MissionManager.Instance != null) ? MissionManager.Instance.GetCurrentMissionData().reward : 0;
            CoinManager.Instance.AddCoins(reward);

            MissionManager.Instance?.SetCurrentMissionClearTime(UIManager.Instance.Stopwatch.ClearTime);

            if (UIManager.Instance != null)
                UIManager.Instance.ShowMissionCompletePopup(reward);
            else
                GameManager.Instance.ReStart();
        }
    }
}
