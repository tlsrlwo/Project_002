
namespace Project002
{
    public abstract class AttackingBaseState
    {
        public abstract void EnterState(PlayerController controller);

        public abstract void UpdateState(PlayerController controller);

    }
}