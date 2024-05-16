using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinManager : PersistentSingleton<CoinManager>
{
    [SerializeField] private int _coins = 0;

    private void Start()
    {
        LoadCoin();
    }

    public void LoadCoin()
    {
        if (PlayerPrefs.HasKey("coins"))
            _coins = PlayerPrefs.GetInt("coins");
    }

    public void AddCoins(int amount)
    {
        _coins += amount;
        PlayerPrefs.SetInt("coins", _coins);
        Debug.Log($"Coins Added. Total Coins : {_coins}");
    }

    public int GetCoins()
    {
        return _coins;
    }
}
