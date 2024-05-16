public class IdleState : IState
{
    private Enemy _enemy;

    public IdleState(Enemy enemy)
    {
        _enemy = enemy;
        _enemy.hasArrivedAtLastPoint = false;
    }

    public void Enter()
    {
    }

    public void Exit()
    {
    }

    public void Execute()
    {
        if (_enemy.Fow.visibleTargets.Count > 0)
        {
            _enemy.ChangeState(new ChaseState(_enemy));
        }
    }

    public void PhysicsExecute()
    {
    }
}
