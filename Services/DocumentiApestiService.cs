using Pseven.Models;

namespace Pseven.Services
{
    public class DocumentiApestiService
    {
        private readonly DatabaseService _databaseService = new();
        public async Task<List<DocumentoAperto>> GetAllAsync()
        {
            var conn = await _databaseService.GetConnectionAsync();
            return conn.Table<DocumentoAperto>().ToList();
        }
    }
}


