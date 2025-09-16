using System.Net.Http;
using System.Net.Http.Json;
using System.Collections.ObjectModel;
using Pseven.Models;

namespace Pseven.Services;

public class ApiService
{
    private readonly HttpClient _httpClient = new();

    private const string Url = "https://localhost:7208/api/MainPageInput";

    public async Task<ObservableCollection<MainPageInput>> GetDocumentiAsync()
    {
        var result = await _httpClient.GetFromJsonAsync<ObservableCollection<MainPageInput>>(Url);
        return result ?? new ObservableCollection<MainPageInput>();
    }
}