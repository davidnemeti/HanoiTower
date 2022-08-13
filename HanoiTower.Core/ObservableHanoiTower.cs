using System.Collections.Immutable;

namespace HanoiTower.Core
{
    public class ObservableHanoiTower : HanoiTowerBase<ObservableHanoiTower>
    {
        public IImmutableStack<int> DisksOnRod1 { get; }
        public IImmutableStack<int> DisksOnRod2 { get; }
        public IImmutableStack<int> DisksOnRod3 { get; }

        public ObservableHanoiTower(int numberOfDisks, HanoiRod initRod)
            : this(numberOfDisks, GetDisksForRod(numberOfDisks, HanoiRod.Rod1, initRod), GetDisksForRod(numberOfDisks, HanoiRod.Rod2, initRod), GetDisksForRod(numberOfDisks, HanoiRod.Rod3, initRod))
        {
        }

        private ObservableHanoiTower(int numberOfDisks, IImmutableStack<int> disksOnRod1, IImmutableStack<int> disksOnRod2, IImmutableStack<int> disksOnRod3)
            : base(numberOfDisks)
        {
            DisksOnRod1 = disksOnRod1;
            DisksOnRod2 = disksOnRod2;
            DisksOnRod3 = disksOnRod3;
        }

        private static IImmutableStack<int> GetDisksForRod(int numberOfDisks, HanoiRod rod, HanoiRod initRod) =>
            rod == initRod
                ? ImmutableStack.CreateRange(Enumerable.Range(1, numberOfDisks).Reverse())
                : ImmutableStack.Create<int>();

        public override ObservableHanoiTower MoveDisk(HanoiMove move)
        {
            var disksOnRod1 = DisksOnRod1;
            var disksOnRod2 = DisksOnRod2;
            var disksOnRod3 = DisksOnRod3;

            int disk = -1;
            ApplyActionOnRod(move.Source, disksOnRod => disksOnRod.Pop(out disk));
            ApplyActionOnRod(move.Target, disksOnRod =>
            {
                if (!disksOnRod.IsEmpty && disk > disksOnRod.Peek())
                    throw new InvalidOperationException($"Cannot put a larger disk ({disk}) onto a smaller one ({disksOnRod.Peek()})");

                return disksOnRod.Push(disk);
            });

            return new ObservableHanoiTower(NumberOfDisks, disksOnRod1, disksOnRod2, disksOnRod3);

            void ApplyActionOnRod(HanoiRod rod, Func<IImmutableStack<int>, IImmutableStack<int>> rodAction)
            {
                switch (rod)
                {
                    case HanoiRod.Rod1:
                        disksOnRod1 = rodAction(disksOnRod1);
                        break;

                    case HanoiRod.Rod2:
                        disksOnRod2 = rodAction(disksOnRod2);
                        break;

                    case HanoiRod.Rod3:
                        disksOnRod3 = rodAction(disksOnRod3);
                        break;

                    default:
                        throw new ArgumentOutOfRangeException(nameof(rod), rod, $"Unknown rod: {rod}");
                };
            }
        }
    }
}
