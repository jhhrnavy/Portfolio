using Photon.Pun;
using UnityEngine;

public interface IGameState
{
    void EnterState();
    void UpdateState();
    void ExitState();
}

public class StartState : IGameState
{
    private GameManager _gm;

    public StartState(GameManager gm)
    {
        _gm = gm;
    }

    public void EnterState()
    {
        _gm.InGame.ScoreManager.SetInit();
        _gm.InGame.SpawnPlayer();
        Cursor.lockState = CursorLockMode.Locked;
        _gm.ChangeState(new WaitingState(_gm));
    }

    public void UpdateState()
    {
    }

    public void ExitState()
    {
    }
}

public class WaitingState : IGameState
{
    private GameManager _gm;

    public WaitingState(GameManager gm)
    {
        _gm = gm;
    }

    public void EnterState()
    {
        _gm.InGame.ResetRound();
        _gm.InGame.RespawnAllPlayer();
    }

    public void UpdateState()
    {
        //Debug.Log(_gm.CheckPlayersCount());
        if (_gm.CheckPlayersCount()) // 플레이어 씬 참가 확인
        {
            
            _gm.ChangeState(new PreparationState(_gm));
        }
    }

    public void ExitState()
    {

    }
}

public class PreparationState : IGameState
{
    private GameManager _gm;

    public PreparationState(GameManager gm)
    {
        _gm = gm;
    }

    public void EnterState()
    {
        //Debug.Log("Enter Preparation State");
        _gm.GameStart(); // 카운트 다운 시작
    }

    public void UpdateState()
    {
        if (_gm.InGame.CountdownTimer.IsFinished)
        {
            _gm.ChangeState(new PlayingState(_gm));
        }
    }

    public void ExitState()
    {
    }
}

public class PlayingState : IGameState
{
    private GameManager _gm;

    public PlayingState(GameManager gm)
    {
        _gm = gm;
    }

    public void EnterState()
    {
        _gm.InGame.Timer.StartTimer();
    }

    public void UpdateState()
    {
        if (_gm.InGame.ScoreManager == null) return;

        // 라운드 종료 조건
        if (_gm.InGame.Timer.IsEnded || _gm.InGame.ScoreManager.HasReachedTargetScore())
        {
            _gm.InGame.Timer.PauseTimer();
            _gm.ChangeState(new RoundEndState(_gm));
        }
    }

    public void ExitState()
    {

    }
}

public class RoundEndState : IGameState
{
    private GameManager _gm;

    public RoundEndState(GameManager gm)
    {
        _gm = gm;
    }

    public void EnterState()
    {
        //Debug.Log("RoundEnd!!");
        var sm = _gm.InGame.ScoreManager;
        if (sm == null) return;
        TeamType winTeam = sm.GetWinningTeamInRound(); // 승리한 팀
        sm.AddWinCount(winTeam.ToString()); // 승리한 팀의 승리 횟수 증가
    }

    public void UpdateState()
    {

    }

    public void ExitState()
    {
        //Debug.Log("Exit RoundEnd State");
    }
}

public class GameOverState : IGameState
{
    private GameManager _gm;
    private double _startTime;
    private float _resultAnimDuration =5f;
    public GameOverState(GameManager gm)
    {
        _gm = gm;
    }

    public void EnterState()
    {
        //Debug.Log("Enter GameOver State");
        _gm.InGame.GameOver(); // 결과창 호출
        _startTime = PhotonNetwork.Time;
    }

    public void UpdateState()
    {
        double elapsedTime = PhotonNetwork.Time - _startTime;
        if(elapsedTime >= _resultAnimDuration) 
        {
            _gm.InGame.ReturnToLobby();
        }
    }

    public void ExitState()
    {

    }
}
