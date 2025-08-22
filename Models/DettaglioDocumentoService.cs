using Pseven.Maui.Models;

namespace Pseven.Maui.Services;

public class DettaglioDocumentoService
{
    private readonly DatabaseService _databaseService = new();

    public async Task<List<DettaglioDocumento>> GetAllAsync()
    {
        var conn = await _databaseService.GetConnectionAsync();
        return conn.Table<DettaglioDocumento>().ToList();
    }
}