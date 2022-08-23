namespace HanoiTower.Core
{
    public record HanoiMove
    {
        public HanoiRod Source { get; init; }
        public HanoiRod Target { get; init; }

        public HanoiMove(HanoiRod source, HanoiRod target)
        {
            if (source == target)
                throw new ArgumentException($"Invalid move (source and target is the same): {source} => {target}", nameof(target));

            Source = source;
            Target = target;
        }

        public override string ToString() => $"{Source} => {Target}";
    }

    public record IndexedHanoiMove(HanoiRod Source, HanoiRod Target, int Index) : HanoiMove(Source, Target)
    {
        public override string ToString() => $"{base.ToString()} (step {Index})";
    }

    public static class HanoiMoveExtensions
    {
        public static IndexedHanoiMove WithIndex(this HanoiMove move, int index) => new IndexedHanoiMove(move.Source, move.Target, index);
    }
}
