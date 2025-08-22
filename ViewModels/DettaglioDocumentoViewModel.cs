using System.Collections.ObjectModel;
using Pseven.Maui.Models;
using Pseven.Maui.Services;

namespace Pseven.Maui.ViewModels;

public class DettaglioDocumentoViewModel
{
    public ObservableCollection<DettaglioDocumento> Dettagli { get; set; } = new();

    private readonly DettaglioDocumentoService _service = new();

    public DettaglioDocumentoViewModel()
    {
        CaricaDati();
    }

    private async void CaricaDati()
    {
        var lista = await _service.GetAllAsync();
        foreach (var item in lista)
            Dettagli.Add(item);
    }
}
