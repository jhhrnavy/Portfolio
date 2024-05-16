public class AttackState : IState
{
    private Enemy _enemy;

    public AttackState(Enemy enemy)
    {
        _enemy = enemy;
    }

    public void Enter()
    {
        _enemy.StopMoving();
        _enemy.currentGun.StartFiring();
    }

    public void Execute()
    {
        _enemy.Rotate();

        _enemy.currentGun.SetTargetPosition(_enemy.Fow.targetLastPosition);

        if (_enemy.Fow.visibleTargets.Count == 0)
            _enemy.ChangeState(new ChaseState(_enemy));


        if (_enemy.currentGun.currentAmmo <= 0 && !_enemy.currentGun.IsReloading)
            _enemy.ChangeState(new ReloadState(_enemy));
    }

    public void PhysicsExecute()
    {
    }

    public void Exit()
    {
        _enemy.currentGun.EndFiring();
    }

}
