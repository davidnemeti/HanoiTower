using HanoiTower.Core;

namespace HanoiTower.ViewModel
{
    public class HanoiInitialData
    {
        public int NumberOfDisks { get; set; }
        public HanoiRod SourceRod { get; set; }
        public HanoiRod TargetRod { get; set; }
    }
}
