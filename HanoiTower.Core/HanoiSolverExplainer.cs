using System.Collections.Immutable;

namespace HanoiTower.Core
{
    public class HanoiSolverExplainer<THanoiTower> : HanoiSolver<HanoiTowerSolutionContext<THanoiTower>>
        where THanoiTower : IHanoiTower<THanoiTower>
    {
        protected override HanoiTowerSolutionContext<THanoiTower> Solve(HanoiTowerSolutionContext<THanoiTower> tower, HanoiGoal goal)
        {
            var beginState = tower;
            var endState = base.Solve(tower, goal);

            var move = goal.Move.WithIndex(tower.Step + 1);

            return goal.NumberOfDisks == 1
                ? endState.WithSimpleSolution(beginState, goal, move)
                : endState.WithComplexSolution(beginState, goal, move);
        }
    }

    public class HanoiTowerSolutionContext<THanoiTower> : HanoiTowerBase<HanoiTowerSolutionContext<THanoiTower>>
        where THanoiTower : IHanoiTower<THanoiTower>
    {
        private readonly THanoiTower _tower;
        private readonly ImmutableStack<HanoiSolution<THanoiTower>> _solutions;
        public int Step { get; }

        public HanoiTowerSolutionContext(THanoiTower tower)
            : this(tower, ImmutableStack.Create<HanoiSolution<THanoiTower>>(), step: 0)
        {
        }

        private HanoiTowerSolutionContext(THanoiTower tower, ImmutableStack<HanoiSolution<THanoiTower>> solutions, int step)
            : base(tower.NumberOfDisks)
        {
            _tower = tower;
            _solutions = solutions;
            Step = step;
        }

        public override HanoiTowerSolutionContext<THanoiTower> MoveDisk(HanoiMove move) => new HanoiTowerSolutionContext<THanoiTower>(_tower.MoveDisk(move), _solutions, Step + 1);

        public HanoiTowerSolutionContext<THanoiTower> WithSimpleSolution(HanoiTowerSolutionContext<THanoiTower> beginState, HanoiGoal goal, IndexedHanoiMove move)
        {
            var solutions = _solutions.Push(HanoiSolution.Simple(beginState._tower, goal, move, endState: _tower, Step));
            return new HanoiTowerSolutionContext<THanoiTower>(_tower, solutions, Step);
        }

        public HanoiTowerSolutionContext<THanoiTower> WithComplexSolution(HanoiTowerSolutionContext<THanoiTower> beginState, HanoiGoal goal, IndexedHanoiMove move)
        {
            var solutions = _solutions
                .Pop(out var solutionAfterMove)
                .Pop(out var solutionMove)
                .Pop(out var solutionBeforeMove)
                .Push(HanoiSolution.Complex(beginState._tower, solutionBeforeMove, goal, solutionMove.Move, solutionAfterMove, endState: _tower, Step));

            return new HanoiTowerSolutionContext<THanoiTower>(_tower, solutions, Step);
        }

        public HanoiSolution<THanoiTower> GetSolution() => _solutions.Single();
    }
}
