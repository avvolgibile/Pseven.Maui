
using Pseven.Maui.Models;
using Pseven.Maui.ViewModels;
namespace Pseven.Views;


public partial class StoricoArticoliPage : ContentPage
{
	public StoricoArticoliPage()
	{
		InitializeComponent();
	  BindingContext = new DettaglioDocumentoViewModel();
	}
}