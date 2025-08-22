namespace Pseven.Maui.Services
{
    public static class DatabaseHelper//questa classe serve per copiare il database dalla cartella delle risorse alla cartella dei dati dell'applicazione
    {
        public static async Task<string> GetDatabasePathAsync()//percorso del database
        {
            string dbName = "database1.sqlite";
            string dbPath = Path.Combine(FileSystem.AppDataDirectory, dbName);

            if (!File.Exists(dbPath))
            {
                using var stream = await FileSystem.OpenAppPackageFileAsync(dbName);
                using var newFile = File.Create(dbPath);
                await stream.CopyToAsync(newFile);
            }

            return dbPath;
        }
    }
}
