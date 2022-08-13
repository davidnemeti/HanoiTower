namespace HanoiTower.Core
{
    public class HanoiTowerAggregate : HanoiTowerBase<HanoiTowerAggregate>
    {
        private readonly IReadOnlyList<IHanoiTower> _towers;

        public HanoiTowerAggregate(params IHanoiTower[] towers)
            : base(GetNumberOfDisks(towers))
        {
            _towers = towers;
        }

        public override HanoiTowerAggregate MoveDisk(HanoiMove move) =>
            new HanoiTowerAggregate(_towers.Select(tower => tower.MoveDisk(move)).ToArray());

        private static int GetNumberOfDisks(IReadOnlyList<IHanoiTower> towers)
        {
            var numberOfDisks = towers.First().NumberOfDisks;

            if (towers.Any(tower => tower.NumberOfDisks != numberOfDisks))
                throw new ArgumentException($"Some towers have number of disks different from {numberOfDisks}", nameof(towers));

            return numberOfDisks;
        }
    }

    public class HanoiTowerAggregate<THanoiTower1, THanoiTower2> : HanoiTowerBase<HanoiTowerAggregate<THanoiTower1, THanoiTower2>>
        where THanoiTower1 : IHanoiTower<THanoiTower1>
        where THanoiTower2 : IHanoiTower<THanoiTower2>
    {
        public THanoiTower1 Tower1 { get; }
        public THanoiTower2 Tower2 { get; }

        public HanoiTowerAggregate(THanoiTower1 tower1, THanoiTower2 tower2)
            : base(GetNumberOfDisks(tower1, tower2))
        {
            Tower1 = tower1;
            Tower2 = tower2;
        }

        public override HanoiTowerAggregate<THanoiTower1, THanoiTower2> MoveDisk(HanoiMove move) =>
            new HanoiTowerAggregate<THanoiTower1, THanoiTower2>(Tower1.MoveDisk(move), Tower2.MoveDisk(move));

        private static int GetNumberOfDisks(THanoiTower1 tower1, THanoiTower2 tower2)
        {
            if (tower2.NumberOfDisks != tower1.NumberOfDisks)
                throw new ArgumentException($"The number of disks of the second tower ({tower1.NumberOfDisks}) is different from the first tower's ({tower2.NumberOfDisks})", nameof(tower2));

            return tower1.NumberOfDisks;
        }
    }
}
