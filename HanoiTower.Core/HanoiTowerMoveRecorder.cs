using System.Collections.Immutable;

namespace HanoiTower.Core
{
    public class HanoiTowerMoveRecorder : HanoiTowerBase<HanoiTowerMoveRecorder>
    {
        public IImmutableList<HanoiMove> Moves;

        public HanoiTowerMoveRecorder(int numberOfDisks)
            : this(numberOfDisks, ImmutableList.Create<HanoiMove>())
        {
        }

        private HanoiTowerMoveRecorder(int numberOfDisks, IImmutableList<HanoiMove> moves)
            : base(numberOfDisks)
        {
            Moves = moves;
        }

        public override HanoiTowerMoveRecorder MoveDisk(HanoiMove move) => new HanoiTowerMoveRecorder(NumberOfDisks, Moves.Add(move));
    }
}
