using SQLite;

namespace Pseven.Services;

public class DatabaseService
{
    private SQLiteConnection _connection;

    public async Task<SQLiteConnection> GetConnectionAsync()
    {
        if (_connection == null)
        {
            string dbPath = await DatabaseHelper.GetDatabasePathAsync();
            _connection = new SQLiteConnection(dbPath);
        }
        return _connection;
    }
}