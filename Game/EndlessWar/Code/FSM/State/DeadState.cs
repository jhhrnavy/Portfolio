using UnityEngine;

public class DeadState : MonoBehaviour, IState
{
    private Enemy _enemy;

    public DeadState(Enemy enemy)
    {
        _enemy = enemy;
    }

    public void Enter()
    {
        Debug.Log("Dead");
        _enemy.Die();
    }

    public void Execute()
    {
    }
    public void PhysicsExecute()
    {
    }

    public void Exit()
    {
    }

}
