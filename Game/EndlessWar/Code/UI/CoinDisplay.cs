using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CoinDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _coinText;

    private void Start()
    {
        UpdateCoinDisplay(CoinManager.Instance.GetCoins());
    }

    public void UpdateCoinDisplay(int coin)
    {
        _coinText.text = $"Coin : {coin}";
    }
}
