using HanoiTower.Core;

namespace HanoiTower.ViewModels
{
    public class HanoiInitialData
    {
        public int NumberOfDisks { get; set; }
        public HanoiRod SourceRod { get; set; }
        public HanoiRod TargetRod { get; set; }
    }
}
