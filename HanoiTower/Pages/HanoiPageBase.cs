using HanoiTower.Core;
using HanoiTower.Services;
using HanoiTower.ViewModel;
using Microsoft.AspNetCore.Components;

namespace HanoiTower.Pages
{
    public abstract class HanoiPageBase<TResult> : ComponentBase, IDisposable
        where TResult : class
    {
        [Inject]
        private CalculatorService Calculator { get; set; } = null!;

        [CascadingParameter]
        private HanoiInitialData HanoiInitialData { get; set; } = null!;

        protected int NumberOfDisks { get; private set; }
        protected HanoiRod SourceRod { get; private set; }
        protected HanoiRod TargetRod { get; private set; }
        protected TResult? Result { get; private set; }

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            await CalculateResult();
            Calculator.Calculate += ClearResult;
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);

            if (!firstRender && Result is null)
            {
                await CalculateResult();
                StateHasChanged();
            }
        }

        void IDisposable.Dispose()
        {
            Calculator.Calculate -= ClearResult;
        }

        private void ClearResult(object? sender, HanoiInitialData data)
        {
            Result = null;
        }

        private async Task CalculateResult()
        {
            NumberOfDisks = HanoiInitialData.NumberOfDisks;
            SourceRod = HanoiInitialData.SourceRod;
            TargetRod = HanoiInitialData.TargetRod;

            Result = await GetCalculatedResult();
        }

        protected abstract Task<TResult> GetCalculatedResult();
    }
}
