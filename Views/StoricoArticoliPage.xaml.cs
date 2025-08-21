
using Pseven.ViewModels;
namespace Pseven.Views;


public partial class StoricoArticoliPage : ContentPage
{
	public StoricoArticoliPage()
	{
		InitializeComponent();
	  BindingContext = new StoricoArticoliViewModel();
	}
}