using Pseven.ViewModels;

namespace Pseven.Views;

public partial class ArticoliPage : ContentPage
{
	private readonly ArticoliViewModel _viewModel;
	public ArticoliPage(ArticoliViewModel viewmodel)
	{
		InitializeComponent();
        _viewModel = viewmodel;
        BindingContext = _viewModel;
		
		//BindingContext = BaseViewModel;
	}
}