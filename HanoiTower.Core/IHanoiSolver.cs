namespace HanoiTower.Core
{
    public interface IHanoiSolver<THanoiTower>
        where THanoiTower : IHanoiTower<THanoiTower>
    {
        THanoiTower Solve(THanoiTower tower, HanoiMove move);
    }
}
