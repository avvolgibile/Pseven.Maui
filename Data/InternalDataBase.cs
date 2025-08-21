using Pseven.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pseven.Data
{
    public class InternalDataBase
    {
        SQLiteAsyncConnection DataBase;
        public InternalDataBase()
        {
            
        }
        async Task Init()
        {
            if (DataBase is not null)
                return;

            DataBase = new SQLiteAsyncConnection(Constants.DatabasePath, Constants.Flags);
            await DataBase.CreateTableAsync<Documento>();
            await DataBase.CreateTableAsync<DettaglioDocumento>();
            await DataBase.CreateTableAsync<Cliente>();
            await DataBase.CreateTableAsync<Articolo>();
            await DataBase.CreateTableAsync<ArticoloNote>();
        }

        //DOCUMENTO
        public async Task<List<Documento>> GetDocumentiAsync()
        {
            await Init();
            return await DataBase.Table<Documento>().ToListAsync();
        }
        public async Task<Documento> GetDocumentoAsync(int id)
        {
            await Init();
            return await DataBase.Table<Documento>().Where(x=>x.DocumentoId == id).FirstOrDefaultAsync();
        }
        public async Task<int> SaveDocumentoAsync(Documento documento)
        {
            await Init();
            if(documento.DocumentoId != 0)
            {
                return await DataBase.UpdateAsync(documento);
            }
            else
            {
                return await DataBase.InsertAsync(documento);
            }
        }
        public async Task<int> DeleteDocumentoAsync(Documento documento)
        {
            await Init();
            return await DataBase.DeleteAsync(documento);
        }

        //CLIENTE
        public async Task<List<Cliente>> GetClientiAsync()
        {
            await Init();
            return await DataBase.Table<Cliente>().ToListAsync();
        }
        public async Task<Cliente> GetClienteAsync(int id)
        {
            await Init();
            return await DataBase.Table<Cliente>().Where(x => x.ClienteId == id).FirstOrDefaultAsync();
        }
        public async Task<Cliente> GetClienteByCodiceAsync(string codice)
        {
            await Init();
            return await DataBase.Table<Cliente>().Where(x => x.CodiceCliente == codice).FirstOrDefaultAsync();
        }
        public async Task<List<Cliente>> GetClientiByRagioneSocialeAsync(string ragionesociale)
        {
            await Init();
            return await DataBase.Table<Cliente>().Where(x => x.RagioneSociale == ragionesociale).ToListAsync();
        }
        public async Task<List<Cliente>> GetClientiByCodiceAsync(string codice)
        {
            await Init();
            return await DataBase.Table<Cliente>().Where(x => x.CodiceCliente.Contains(codice)).ToListAsync();
        }
        public async Task<int> SaveClienteAsync(Cliente cliente)
        {
            await Init();
            if (cliente.ClienteId != 0)
            {
                return await DataBase.UpdateAsync(cliente);
            }
            else
            {
                return await DataBase.InsertAsync(cliente);
            }
        }
        public async Task<int> DeleteClienteAsync(Cliente cliente)
        {
            await Init();
            return await DataBase.DeleteAsync(cliente);
        }

        //ARTICOLO
        public async Task<int> CountArticoliAsync()
        {
            await Init();
            return await DataBase.Table<Articolo>().CountAsync();
        }
        public async Task<List<Articolo>> GetArticoliAsync()
        {
            await Init();
            return await DataBase.Table<Articolo>().ToListAsync();
        }
        public async Task<Articolo> GetArticoloAsync(int id)
        {
            await Init();
            return await DataBase.Table<Articolo>().Where(x => x.ArticoloId == id).FirstOrDefaultAsync();
        }
        public async Task<int> SaveArticoloAsync(Articolo articolo)
        {
            await Init();
            if (articolo.ArticoloId != 0)
            {
                return await DataBase.UpdateAsync(articolo);
            }
            else
            {
                return await DataBase.InsertAsync(articolo);
            }
        }
        public async Task<int> DeleteArticoloAsync(Articolo articolo)
        {
            await Init();
            return await DataBase.DeleteAsync(articolo);
        }
    }
}
