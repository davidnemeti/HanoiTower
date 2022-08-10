using HanoiTower.ViewModel;

namespace HanoiTower.Services
{
    public class CalculatorService
    {
        public event EventHandler<HanoiInitialData>? Calculate;

        public void OnCalculate(object sender, HanoiInitialData data)
        {
            Calculate?.Invoke(sender, data);
        }
    }
}
