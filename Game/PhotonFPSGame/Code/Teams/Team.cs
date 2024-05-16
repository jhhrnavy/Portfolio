using Photon.Pun;
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Team
{
    [SerializeField, HideInInspector] private string _teamName;
    [SerializeField] private TeamType _type;
    [SerializeField] private List<PlayerController> _players = new List<PlayerController>();
    [SerializeField] private List<Transform> _spawnPoints = new List<Transform>();

    public List<Transform> SpawnPoints { get => _spawnPoints; }
    public List<PlayerController> Players { get => _players;}
    public TeamType Type { get => _type;}

    public Team(TeamType type)
    {
        _type = type;
        _teamName = Type.ToString();
    }

    public void AddPlayers(PlayerController newPlayer)
    {
        _players.Add(newPlayer);
    }

    public void RemovePlayer(Photon.Realtime.Player player)
    {
        _players.RemoveAll(target => target.GetComponent<PhotonView>().Owner.ActorNumber == player.ActorNumber);
    }
}

public enum TeamType
{
    None = -1,
    A,
    B,
}