using Microsoft.Maui.Controls.Shapes;
using Pseven.Varie;
using Pseven.ViewModels;
using Pseven.Controls;

namespace Pseven.Views;

public partial class OrdinePage : BasePage
{
	private readonly OrdineViewModel _viewModel;
    public OrdinePage(OrdineViewModel viewmodel)
	{
        InitializeComponent();
        _viewModel = viewmodel;
        BindingContext = _viewModel;
      
    }
   
}