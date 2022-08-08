namespace HanoiTower.Core
{
    public interface IHanoiTower
    {
        int NumberOfDisks { get; }
        IHanoiTower MoveDisk(HanoiMove move);
    }

    public interface IHanoiTower<out THanoiTower> : IHanoiTower
        where THanoiTower : IHanoiTower<THanoiTower>
    {
        new THanoiTower MoveDisk(HanoiMove move);
    }

    public abstract class HanoiTowerBase<THanoiTower> : IHanoiTower<THanoiTower>
        where THanoiTower : IHanoiTower<THanoiTower>
    {
        public int NumberOfDisks { get; }

        protected HanoiTowerBase(int numberOfDisks)
        {
            NumberOfDisks = numberOfDisks;
        }

        public abstract THanoiTower MoveDisk(HanoiMove move);

        IHanoiTower IHanoiTower.MoveDisk(HanoiMove move) => MoveDisk(move);
    }
}
