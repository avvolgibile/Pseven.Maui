using Pseven.ViewModels;

namespace Pseven.Views;

public partial class DocumentoPage : ContentPage
{
    private readonly DocumentoViewModel _viewModel;
    public DocumentoPage(DocumentoViewModel viewmodel)
	{
		InitializeComponent();
        _viewModel = viewmodel;
        BindingContext = _viewModel;
    }
}