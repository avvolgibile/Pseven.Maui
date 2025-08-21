using Pseven.Controls;
using Pseven.ViewModels;

namespace Pseven.Views;

public partial class MiniFormPage : ContentPage
{
	public MiniFormPage(OrdineViewModel viewmodel)
	{
		InitializeComponent();
		BindingContext = viewmodel;
	}
}