using ExitGames.Client.Photon;
using Photon.Pun;
using UnityEngine;
using System;

public class PlayerTeam : MonoBehaviourPun
{
    [SerializeField] private TeamType _type;
    [SerializeField] private SkinnedMeshRenderer _bodyRenderer;
    [SerializeField] private SkinnedMeshRenderer _armRenderer;
    [SerializeField] private Color _teamAColor;
    [SerializeField] private Color _teamBColor;

    public TeamType Type { get => _type; }

    [PunRPC]
    public void SetTeam()
    {
        if (photonView.IsMine)
        {
            Hashtable customProperties = PhotonNetwork.LocalPlayer.CustomProperties;

            if (customProperties.ContainsKey("Team"))
            {
                object teamValue = customProperties["Team"];
                _type = (TeamType)Enum.Parse(typeof(TeamType), teamValue.ToString());
                SetPlayerColor(_type);
            }
        }
    }

    public void SetPlayerColor(TeamType team)
    {
        switch (team)
        {
            case TeamType.None:
                break;
            case TeamType.A:
                _bodyRenderer.material.SetColor("_RimColor", _teamAColor);
                _armRenderer.material.SetColor("_RimColor", _teamAColor);
                break;
            case TeamType.B:
                _bodyRenderer.material.SetColor("_RimColor", _teamBColor);
                _armRenderer.material.SetColor("_RimColor", _teamBColor);
                break;
        }
    }
}

