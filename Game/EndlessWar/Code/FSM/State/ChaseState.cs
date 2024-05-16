using UnityEngine;

public class ChaseState : IState
{
    private Enemy _enemy;

    public ChaseState(Enemy enemy)
    {
        _enemy = enemy;
    }

    public void Enter()
    {
        _enemy.StartMoving();
    }

    public void Execute()
    {
        Debug.Log("ÃßÀûÁß");
        if (_enemy.Fow.visibleTargets.Count == 0)
        {

            if (_enemy.hasArrivedAtLastPoint)
            {
                _enemy.ChangeState(new IdleState(_enemy));
                return;
            }
        }
        else
        {
            _enemy.currentGun.SetTargetPosition(_enemy.Fow.targetLastPosition);
            if (_enemy.isfacingTarget)
            {
                //Attack
                _enemy.ChangeState(new AttackState(_enemy));
            }
        }
        //_enemy.Rotate();


    }
    public void PhysicsExecute()
    {
        if (_enemy.Fow.visibleTargets.Count == 0 && _enemy.isfacingTarget)
            _enemy.Move();

        _enemy.Rotate();

    }

    public void Exit()
    {
    }
}
