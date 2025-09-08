using Newtonsoft.Json;
using Pseven.Models;
using Pseven.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pseven.Data
{
    public class ExternalDataBase
    {
        #region Proprietà
        private TAbsDBErrorCodes errorcode;
        private readonly WrapperAbsDBJson AbsDB;
        #endregion

        #region Eventi
        #endregion

        #region Comandi
        #endregion

        #region Costruttori
        public ExternalDataBase()
        {
            AbsDB = new WrapperAbsDBJson();
        }
        #endregion

        #region Metodi
        private void GetConnection()
        {
            AbsDB.NewConnection(Statics.ABSDBFileName, Statics.JsonPath, out errorcode);
        }
        public Task<List<Cliente>> GetClienti()
        {
            GetConnection();
            List<Cliente> clienti = [];
            string filejson = AbsDB.Wrap[0].ExecuteSelectMaui("select * from CLIENTI ORDER BY NOME ASC", out errorcode);
            if (errorcode == TAbsDBErrorCodes.NoError)
            {
                var result = JsonConvert.DeserializeObject<ElencoClienti>(filejson);
                if (result != null)
                {
                    clienti = result.Clienti ?? ([]);
                }
            }
            return Task.FromResult(clienti ?? []);
        }
        public Task<List<Articolo>> GetArticoli()
        {
            GetConnection();
            List<Articolo> articoli = [];
            string filejson = AbsDB.Wrap[0].ExecuteSelectMaui("select * from articoli  where INUSO = '' or INUSO LIKE 'True'  ORDER BY  DESCRIZION ASC", out errorcode);
            if (errorcode == TAbsDBErrorCodes.NoError)
            {
                var result = JsonConvert.DeserializeObject<ElencoArticoli>(filejson);
                if (result != null)
                {
                    articoli = result.Articoli ?? ([]);
                }
            }
            return Task.FromResult(articoli ?? []);
        }
        public Task<List<Agente>> GetAgenti()
        {
            GetConnection();
            List<Agente> agenti = [];
            string filejson = AbsDB.Wrap[0].ExecuteSelectMaui("select * from AGENTI", out errorcode);
            if (errorcode == TAbsDBErrorCodes.NoError)
            {
                var result = JsonConvert.DeserializeObject<ElencoAgenti>(filejson);
                if (result != null)
                {
                    agenti = result.Agenti ?? ([]);
                }
            }
            return Task.FromResult(agenti ?? []);
        }
        public Task<List<PrezziCliente>> GetPrezziCliente()
        {
            GetConnection();
            List<PrezziCliente> prezzi = [];
            string filejson = AbsDB.Wrap[0].ExecuteSelectMaui("select * from CLIPREZZ", out errorcode);
            if (errorcode == TAbsDBErrorCodes.NoError)
            {
                var result = JsonConvert.DeserializeObject<ElencoPrezziCliente>(filejson);
                if (result != null)
                {
                    prezzi = result.PrezziCliente ?? ([]);
                }
            }
            return Task.FromResult(prezzi ?? []);
        }
        #endregion
    }
}
