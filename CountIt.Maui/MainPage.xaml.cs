using CountIt.Core.ViewModels;

namespace CountIt.Maui
{
    public partial class MainPage : ContentPage
    {     
            public MainPage(MainViewModel viewModel)
            {
                InitializeComponent();
                BindingContext = viewModel;
            }
    }
}
