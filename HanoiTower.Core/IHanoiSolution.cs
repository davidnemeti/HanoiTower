namespace HanoiTower.Core
{
    public enum HanoiExplanationLocation
    {
        Undetermined,
        BeforeMove,
        AfterMove
    }

    public abstract record HanoiExplanation<THanoiTower>
        where THanoiTower : IHanoiTower<THanoiTower>
    {
        public THanoiTower BeginState { get; }
        public HanoiGoal Goal { get; }
        public IndexedHanoiMove Move { get; }
        public THanoiTower EndState { get; }
        public HanoiExplanationLocation Location { get; init; }

        protected int Step { get; }

        private HanoiExplanation(THanoiTower beginState, HanoiGoal goal, IndexedHanoiMove move, THanoiTower endState, int step)
        {
            BeginState = beginState;
            Goal = goal;
            Move = move;
            EndState = endState;
            Step = step;
        }

        public sealed record Simple : HanoiExplanation<THanoiTower>
        {
            public new int Step => base.Step;

            public Simple(THanoiTower beginState, HanoiGoal goal, IndexedHanoiMove move, THanoiTower endState, int step)
                : base(beginState, goal, move, endState, step)
            {
            }
        }

        public sealed record Complex : HanoiExplanation<THanoiTower>
        {
            public HanoiExplanation<THanoiTower> ExplanationBeforeMove { get; }
            public HanoiExplanation<THanoiTower> ExplanationAfterMove { get; }
            public int BeginStep { get; }

            public int EndStep => Step;

            public Complex(THanoiTower beginState, HanoiExplanation<THanoiTower> explanationBeforeMove, HanoiGoal goal, IndexedHanoiMove move, HanoiExplanation<THanoiTower> explanationAfterMove, THanoiTower endState, int step)
                : base(beginState, goal, move, endState, step)
            {
                ExplanationBeforeMove = explanationBeforeMove with { Location = HanoiExplanationLocation.BeforeMove };
                ExplanationAfterMove = explanationAfterMove with { Location = HanoiExplanationLocation.AfterMove };

                BeginStep = explanationBeforeMove is Complex complexExplanationBefore
                    ? complexExplanationBefore.BeginStep
                    : explanationBeforeMove.Step;
            }
        }
    }

    public static class HanoiExplanation
    {
        public static HanoiExplanation<THanoiTower>.Simple Simple<THanoiTower>(THanoiTower beginState, HanoiGoal goal, IndexedHanoiMove move, THanoiTower endState, int step)
            where THanoiTower : IHanoiTower<THanoiTower> =>
            new HanoiExplanation<THanoiTower>.Simple(beginState, goal, move, endState, step);

        public static HanoiExplanation<THanoiTower>.Complex Complex<THanoiTower>(THanoiTower beginState, HanoiExplanation<THanoiTower> explanationBeforeMove, HanoiGoal goal, IndexedHanoiMove move, HanoiExplanation<THanoiTower> explanationAfterMove, THanoiTower endState, int step)
            where THanoiTower : IHanoiTower<THanoiTower> =>
            new HanoiExplanation<THanoiTower>.Complex(beginState, explanationBeforeMove, goal, move, explanationAfterMove, endState, step);
    }
}
