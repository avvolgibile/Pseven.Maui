using Pseven.Controls;
using Pseven.ViewModels;

namespace Pseven.Views;

public partial class MiniFormPage : ContentPage
{
	public MiniFormPage(MainPageViewModel viewmodel)
	{
		InitializeComponent();
		BindingContext = viewmodel;
	}
}