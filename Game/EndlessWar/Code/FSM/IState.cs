public interface IState
{
    void Enter();
    void Execute();
    void PhysicsExecute();
    void Exit();
}
