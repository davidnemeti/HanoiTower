namespace HanoiTower.Core
{
    public record HanoiGoal(HanoiMove Move, int NumberOfDisks)
    {
        public override string ToString() => $"{Move.Source} => {Move.Target} ({NumberOfDisks} disks)";
    }
}
