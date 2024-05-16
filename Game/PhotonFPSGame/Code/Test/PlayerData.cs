using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData : Singleton<PlayerData>
{
    [SerializeField] private string nickname;

    public void SetPlayerName(string nickname)
    {
        this.nickname = nickname;
    }

    public string GetPlayerName()
    {
        return nickname;
    }
}
