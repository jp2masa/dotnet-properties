namespace DotNet.Properties.Services
{
    internal interface IDialogService<TViewModel>
    {
        void Show(TViewModel viewModel);
    }
}
