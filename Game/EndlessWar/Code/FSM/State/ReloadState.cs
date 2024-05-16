public class ReloadState : IState
{
    private Enemy _enemy;

    public ReloadState(Enemy enemy)
    {
        _enemy = enemy;
    }

    public void Enter()
    {
        _enemy.currentGun.Reload();
    }

    public void Execute()
    {
        if(_enemy.currentGun.currentAmmo == _enemy.currentGun.magazineSize)
            _enemy.ChangeState(new IdleState(_enemy));
    }

    public void PhysicsExecute()
    {

    }

    public void Exit()
    {

    }

}
