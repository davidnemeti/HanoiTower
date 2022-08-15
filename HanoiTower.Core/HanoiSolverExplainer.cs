﻿using System.Collections.Immutable;

namespace HanoiTower.Core
{
    public class HanoiSolverExplainer<THanoiTower> : HanoiSolver<HanoiTowerExplanationContext<THanoiTower>>
        where THanoiTower : IHanoiTower<THanoiTower>
    {
        protected override HanoiTowerExplanationContext<THanoiTower> Solve(HanoiTowerExplanationContext<THanoiTower> tower, HanoiGoal goal)
        {
            var beginState = tower;
            var endState = base.Solve(tower, goal);

            var move = goal.Move.WithIndex(tower.Step + 1);

            return goal.NumberOfDisks == 1
                ? endState.WithSimpleExplanation(beginState, goal, move)
                : endState.WithComplexExplanation(beginState, goal, move);
        }
    }

    public class HanoiTowerExplanationContext<THanoiTower> : HanoiTowerBase<HanoiTowerExplanationContext<THanoiTower>>
        where THanoiTower : IHanoiTower<THanoiTower>
    {
        private readonly THanoiTower _tower;
        private readonly ImmutableStack<HanoiExplanation<THanoiTower>> _explanations;
        public int Step { get; }

        public HanoiTowerExplanationContext(THanoiTower tower)
            : this(tower, ImmutableStack.Create<HanoiExplanation<THanoiTower>>(), step: 0)
        {
        }

        private HanoiTowerExplanationContext(THanoiTower tower, ImmutableStack<HanoiExplanation<THanoiTower>> explanations, int step)
            : base(tower.NumberOfDisks)
        {
            _tower = tower;
            _explanations = explanations;
            Step = step;
        }

        public override HanoiTowerExplanationContext<THanoiTower> MoveDisk(HanoiMove move) => new HanoiTowerExplanationContext<THanoiTower>(_tower.MoveDisk(move), _explanations, Step + 1);

        public HanoiTowerExplanationContext<THanoiTower> WithSimpleExplanation(HanoiTowerExplanationContext<THanoiTower> beginState, HanoiGoal goal, IndexedHanoiMove move)
        {
            var explanations = _explanations.Push(HanoiExplanation.Simple(beginState._tower, goal, move, endState: _tower, Step));
            return new HanoiTowerExplanationContext<THanoiTower>(_tower, explanations, Step);
        }

        public HanoiTowerExplanationContext<THanoiTower> WithComplexExplanation(HanoiTowerExplanationContext<THanoiTower> beginState, HanoiGoal goal, IndexedHanoiMove move)
        {
            var explanations = _explanations
                .Pop(out var explanationAfterMove)
                .Pop(out var explanationMove)
                .Pop(out var explanationBeforeMove)
                .Push(HanoiExplanation.Complex(beginState._tower, explanationBeforeMove, goal, explanationMove.Move, explanationAfterMove, endState: _tower, Step));

            return new HanoiTowerExplanationContext<THanoiTower>(_tower, explanations, Step);
        }

        public HanoiExplanation<THanoiTower> GetExplanation() => _explanations.Single();
    }
}
