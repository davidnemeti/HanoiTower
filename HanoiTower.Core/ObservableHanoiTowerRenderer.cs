using System.Collections.Immutable;
using SkiaSharp;

namespace HanoiTower.Core
{
    public class ObservableHanoiTowerRenderer : IObservableHanoiTowerRenderer
    {
        private readonly SKImageInfo _info;
        private readonly SKCanvas _canvas;

        public double TowerPerImageHeight { get; init; } = 0.9;
        public double TowerPerImageWidth { get; init; } = 0.3;
        public double RodWidthPerHeight { get; init; } = 0.05;
        public double RodOverhangPerTowerHeight { get; init; } = 0.1;
        public double SmallestDiskWidthPerTowerWidth { get; init; } = 0.3;

        public ObservableHanoiTowerRenderer(SKImageInfo info, SKCanvas canvas)
        {
            _info = info;
            _canvas = canvas;
        }

        public void Render(ObservableHanoiTower tower)
        {
            _canvas.Clear();

            var towerWidth = (int)Math.Round(_info.Width * TowerPerImageWidth);
            var towerHeight = (int)Math.Round(_info.Height * TowerPerImageHeight);
            var gapBetweenTowers = (_info.Width - 3 * towerWidth) / 3;
            var xCenterTower = gapBetweenTowers / 2 + towerWidth / 2;

            DrawBase(towerHeight);

            DrawTower(tower.DisksOnRod1, xCenterTower, 0, towerWidth, towerHeight, tower.NumberOfDisks);
            xCenterTower += towerWidth + gapBetweenTowers;
            DrawTower(tower.DisksOnRod2, xCenterTower, 0, towerWidth, towerHeight, tower.NumberOfDisks);
            xCenterTower += towerWidth + gapBetweenTowers;
            DrawTower(tower.DisksOnRod3, xCenterTower, 0, towerWidth, towerHeight, tower.NumberOfDisks);
        }

        private void DrawBase(int towerHeight)
        {
            var baseHeight = _info.Height - towerHeight;
            _canvas.DrawRect(0, _info.Height - baseHeight, _info.Width - 1, _info.Height - 1, new SKPaint { Color = SKColors.Black, Style = SKPaintStyle.StrokeAndFill });
        }

        private void DrawTower(IImmutableStack<int> disksOnRod, int xCenter, int y, int towerWidth, int towerHeight, int numberOfDisks)
        {
            var rodOverhang = towerHeight * RodOverhangPerTowerHeight;
            var diskHeight = (int)Math.Round((towerHeight - rodOverhang) / numberOfDisks);
            var rodHeight = towerHeight;
            DrawRod(xCenter, rodHeight);
            var diskY = y + towerHeight - diskHeight;
            foreach (var disk in disksOnRod.Reverse())
            {
                var smallestDiskWidth = towerWidth * SmallestDiskWidthPerTowerWidth;
                var largestDiskWidth = (double) towerWidth;
                var diskWidth = (int)Math.Round(smallestDiskWidth * (numberOfDisks - disk) / (numberOfDisks - 1) + largestDiskWidth * (disk - 1) / (numberOfDisks - 1));

                DrawDisk(disk, xCenter, diskY, diskWidth, diskHeight);
                diskY -= diskHeight;
            }
        }

        private void DrawRod(int x, int rodHeight)
        {
            var rodWidth = rodHeight * RodWidthPerHeight;
            var rodPaint = new SKPaint { Color = SKColors.Gray, StrokeWidth = (float)rodWidth };
            _canvas.DrawLine(x, 0, x, rodHeight, rodPaint);
        }

        private void DrawDisk(int disk, int xCenter, int y, int diskWidth, int diskHeight)
        {
            var diskStrokePaint = new SKPaint { Color = SKColors.Black, Style = SKPaintStyle.Stroke };
            var diskFillPaint = new SKPaint { Color = GetDiskColor(disk), Style = SKPaintStyle.Fill };

            var x = xCenter - diskWidth / 2;
            var rx = 5;
            var ry = 5;

            _canvas.DrawRoundRect(x, y, diskWidth, diskHeight, rx, ry, diskFillPaint);
            _canvas.DrawRoundRect(x, y, diskWidth, diskHeight, rx, ry, diskStrokePaint);
        }

        private SKColor GetDiskColor(int disk) => BasicColors[(disk - 1) % BasicColors.Length];

        private static readonly SKColor[] BasicColors = new []
        {
            SKColors.Red,
            SKColors.Lime,
            SKColors.Silver,
            SKColors.Blue,
            SKColors.Purple,
            SKColors.Yellow,
            SKColors.Green,
            SKColors.Cyan,
            SKColors.Olive,
            SKColors.Magenta,
            SKColors.Gray,
            SKColors.Maroon,
            SKColors.Teal,
            SKColors.Navy,
        };
    }
}
