using Pseven.Models;

namespace Pseven.Services;

public class StoricoArticoliService
{
    private readonly DatabaseService _databaseService = new();

    public async Task<List<StoricoArticolo>> GetAllAsync()
    {
        var conn = await _databaseService.GetConnectionAsync();
        return conn.Table<StoricoArticolo>().ToList();
    }
}