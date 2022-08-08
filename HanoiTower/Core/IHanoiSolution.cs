namespace HanoiTower.Core
{
    public enum HanoiSolutionLocation
    {
        Undetermined,
        BeforeMove,
        AfterMove
    }

    public abstract record HanoiSolution<THanoiTower>
        where THanoiTower : IHanoiTower<THanoiTower>
    {
        public THanoiTower BeginState { get; }
        public HanoiGoal Goal { get; }
        public IndexedHanoiMove Move { get; }
        public THanoiTower EndState { get; }
        public HanoiSolutionLocation Location { get; init; }

        protected int Step { get; }

        private HanoiSolution(THanoiTower beginState, HanoiGoal goal, IndexedHanoiMove move, THanoiTower endState, int step)
        {
            BeginState = beginState;
            Goal = goal;
            Move = move;
            EndState = endState;
            Step = step;
        }

        public sealed record Simple : HanoiSolution<THanoiTower>
        {
            public new int Step => base.Step;

            public Simple(THanoiTower beginState, HanoiGoal goal, IndexedHanoiMove move, THanoiTower endState, int step)
                : base(beginState, goal, move, endState, step)
            {
            }
        }

        public sealed record Complex : HanoiSolution<THanoiTower>
        {
            public HanoiSolution<THanoiTower> SolutionBeforeMove { get; }
            public HanoiSolution<THanoiTower> SolutionAfterMove { get; }
            public int BeginStep { get; }

            public int EndStep => Step;

            public Complex(THanoiTower beginState, HanoiSolution<THanoiTower> solutionBeforeMove, HanoiGoal goal, IndexedHanoiMove move, HanoiSolution<THanoiTower> solutionAfterMove, THanoiTower endState, int step)
                : base(beginState, goal, move, endState, step)
            {
                SolutionBeforeMove = solutionBeforeMove with { Location = HanoiSolutionLocation.BeforeMove };
                SolutionAfterMove = solutionAfterMove with { Location = HanoiSolutionLocation.AfterMove };

                BeginStep = solutionBeforeMove is Complex complexSolutionBefore
                    ? complexSolutionBefore.BeginStep
                    : solutionBeforeMove.Step;
            }
        }
    }

    public static class HanoiSolution
    {
        public static HanoiSolution<THanoiTower>.Simple Simple<THanoiTower>(THanoiTower beginState, HanoiGoal goal, IndexedHanoiMove move, THanoiTower endState, int step)
            where THanoiTower : IHanoiTower<THanoiTower> =>
            new HanoiSolution<THanoiTower>.Simple(beginState, goal, move, endState, step);

        public static HanoiSolution<THanoiTower>.Complex Complex<THanoiTower>(THanoiTower beginState, HanoiSolution<THanoiTower> solutionBeforeMove, HanoiGoal goal, IndexedHanoiMove move, HanoiSolution<THanoiTower> solutionAfterMove, THanoiTower endState, int step)
            where THanoiTower : IHanoiTower<THanoiTower> =>
            new HanoiSolution<THanoiTower>.Complex(beginState, solutionBeforeMove, goal, move, solutionAfterMove, endState, step);
    }
}
