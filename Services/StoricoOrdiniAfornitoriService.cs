using Pseven.Models;

namespace Pseven.Services
{
    public class StoricoOrdiniAfornitoriService
    {
        private readonly DatabaseService _databaseService = new();
        public async Task<List<StoricoOrdineAFornitore>> GetAllAsync()
        {
            var conn = await _databaseService.GetConnectionAsync();
            return conn.Table<StoricoOrdineAFornitore>().ToList();
        }
    }
}
