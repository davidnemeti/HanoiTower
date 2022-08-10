namespace HanoiTower.Core
{
    public class HanoiSolver<THanoiTower> : IHanoiSolver<THanoiTower>
        where THanoiTower : IHanoiTower<THanoiTower>
    {
        public THanoiTower Solve(THanoiTower tower, HanoiMove move) => Solve(tower, new HanoiGoal(move, tower.NumberOfDisks));

        protected virtual THanoiTower Solve(THanoiTower tower, HanoiGoal goal)
        {
            if (goal.NumberOfDisks < 1)
                throw new ArgumentOutOfRangeException(nameof(goal), goal, $"Cannot move {goal.NumberOfDisks} number of disks");
            else if (goal.Move.Target == goal.Move.Source)
                return tower;
            else if (goal.NumberOfDisks == 1)
                return tower.MoveDisk(goal.Move);
            else
            {
                var thirdRod = GetThirdRod(goal.Move.Source, goal.Move.Target);

                tower = Solve(tower, new HanoiGoal(new HanoiMove(goal.Move.Source, thirdRod), goal.NumberOfDisks - 1));
                tower = Solve(tower, goal with { NumberOfDisks = 1 });
                tower = Solve(tower, new HanoiGoal(new HanoiMove(thirdRod, goal.Move.Target), goal.NumberOfDisks - 1));

                return tower;
            }

            static HanoiRod GetThirdRod(HanoiRod rod1, HanoiRod rod2) =>
                (rod1, rod2) switch
                {
                    (HanoiRod.Rod1, HanoiRod.Rod2) or (HanoiRod.Rod2, HanoiRod.Rod1) => HanoiRod.Rod3,
                    (HanoiRod.Rod1, HanoiRod.Rod3) or (HanoiRod.Rod3, HanoiRod.Rod1) => HanoiRod.Rod2,
                    (HanoiRod.Rod2, HanoiRod.Rod3) or (HanoiRod.Rod3, HanoiRod.Rod2) => HanoiRod.Rod1,
                    _ => throw new InvalidOperationException("Invalid rods")
                };
        }
    }
}
