using System.Collections.Immutable;

namespace HanoiTower.Core
{
    public class HanoiTowerStateRecorder<THanoiTower> : HanoiTowerBase<HanoiTowerStateRecorder<THanoiTower>>
        where THanoiTower : IHanoiTower<THanoiTower>
    {
        private readonly THanoiTower _tower;
        public IImmutableList<THanoiTower> States;

        public HanoiTowerStateRecorder(THanoiTower tower)
            : this(tower, ImmutableList.Create<THanoiTower>(tower))
        {
        }

        private HanoiTowerStateRecorder(THanoiTower tower, IImmutableList<THanoiTower> states)
            : base(tower.NumberOfDisks)
        {
            _tower = tower;
            States = states;
        }

        public override HanoiTowerStateRecorder<THanoiTower> MoveDisk(HanoiMove move)
        {
            var nextTower = _tower.MoveDisk(move);
            return new HanoiTowerStateRecorder<THanoiTower>(nextTower, States.Add(nextTower));
        }
    }
}
