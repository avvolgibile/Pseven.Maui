using Newtonsoft.Json;
using Pseven.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Pseven.Services
{
    public static class DbUtils
    {
        public static string EscapeLikeValue(string value)
        {
            StringBuilder sb = new StringBuilder(value.Length);
            for (int i = 0; i < value.Length; i++)
            {
                char c = value[i];
                switch (c)
                {
                    case ']':
                    case '[':
                    case '%':
                    case '*':
                        sb.Append("[").Append(c).Append("]");
                        break;
                    case '\'':
                        sb.Append("''");
                        break;
                    default:
                        sb.Append(c);
                        break;
                }
            }
            return sb.ToString();
        }

    }
    public class ImportAbsDBDll
    {

        [DllImport("..\\MyDll.dll", CharSet = CharSet.Unicode, CallingConvention=CallingConvention.StdCall)]
        public static extern int CallNewConnection(string DBFileName, string JsonFilePath, StringBuilder ErrorConnection, string logfilename, int LogMaxSizeMb, int loglevel);

        [DllImport("..\\MyDll.dll", CharSet = CharSet.Unicode)]
        public static extern void CallExecuteSelect(string SelectQuery, int ConnectionID, StringBuilder Result, StringBuilder errorcode, string logfilename, int LogMaxSizeMb, int loglevel);

        [DllImport("..\\MyDll.dll", CharSet = CharSet.Unicode)]
        public static extern void CallRemoveConnection(int ConnectionID);
    }
    public class TAbsDBListFields : List<string>
    {
        public string AsString(int index) { return this[index]; }
        public int AsInteger(int index) { return Convert.ToInt32(this[index]); }
        public double AsDouble(int index) { return Convert.ToDouble(this[index]); }
        public DateTime AsDateTime(int index) { return Convert.ToDateTime(this[index]); }

    }
    public struct TAbsDBStruct
    {
        public string fieldname;
        public string fieldtype;
        public TAbsDBListFields fieldvalue;
    }
    public enum TAbsDBErrorCodes
    {
        NoError = -1, DBFilenNotExists = 2, DBCreateConnectionError = 3, InvalidConnectionId = 12, ExecuteQueryError = 13, SwapDataNotExists = 14
    }
    public class TListAbsDBQuery : List<TAbsDBStruct> { }
    public class TLogWrapperAbsDB : ILogWrapperAbsDB
    {
        private string filename;
        private int maxsizemb;
        private int level;
        public string FileName { get { return filename; } set { filename = value; } }
        public int Level { get { return level; } set { level = value; } }
        public int MaxSizeMB { get { return maxsizemb; } set { maxsizemb = value; } }
    }
    public class TAbsDBQueryJson : IAbsDBQuery
    {
        public TAbsDBQueryJson(int connid, TListAbsDBQuery fields)
        {
            connectionid = connid;
            this.fields = fields;
        }

        private int connectionid;
        private TListAbsDBQuery fields;
        private int fieldcount;
        private int recordcount;
        ILogWrapperAbsDB log;
        private TAbsDBErrorCodes lasterrorcode;

        public ILogWrapperAbsDB Log { get { return log; } set { log = value; } }

        public int ConnectionID
        {
            get => connectionid;
        }

        public TAbsDBStruct FieldByName(string fieldname)
        {
            int i = 0; int t = -1;
            while (i < Fields.Count() && t == -1)
            {
                if (fieldname.ToLower() == Fields[i].fieldname.ToLower()) t = i;
                else i++;
            }
            if (t > -1) return Fields[t];
            else /*return this.Fields[0]; //*/throw new InvalidFieldNameException("Field name does not exists");
        }
        public TListAbsDBQuery Fields
        {
            get => fields;
        }

        public int FieldCount
        {
            get => fieldcount;
        }

        public int RecordCount
        {
            get => recordcount;
        }

        public TAbsDBErrorCodes LastErrorCode
        {
            get => lasterrorcode;
        }

        private int StrToIntDef(string value, int defaultvalue = 0)
        {
            bool isNumeric = true;
            foreach (char c in value)
            {
                if (!char.IsNumber(c))
                {
                    isNumeric = false;
                    break;
                }
            }
            if (isNumeric) return Convert.ToInt32(value); else return defaultvalue;
        }
        public string ExecuteSelectMaui(string SelectQuery, out TAbsDBErrorCodes ErrorCode)
        {
            StringBuilder _errorcode = new StringBuilder(64);
            StringBuilder outjsonfilename = new StringBuilder(256);

            string logfilename = "";
            if (Log != null) { try { logfilename = Log.FileName; } catch { logfilename = ""; } }
            else { log = new TLogWrapperAbsDB(); }

            ImportAbsDBDll.CallExecuteSelect(SelectQuery, connectionid, outjsonfilename, _errorcode, logfilename, Log.MaxSizeMB, Log.Level);

            lasterrorcode = (TAbsDBErrorCodes)Convert.ToInt32(_errorcode.ToString());
            ErrorCode = lasterrorcode;
            //return outjsonfilename.ToString();
            using var reader = new StreamReader(outjsonfilename.ToString());
            return reader.ReadToEnd();
        }
        public void ExecuteSelect(string SelectQuery, out TAbsDBErrorCodes ErrorCode)
        {
            TAbsDBStruct adata;
            string jsonfilename;
            int i;
            //ErrorCode = TAbsDBErrorCodes.NoError;

            fieldcount = 0;

            StringBuilder _errorcode = new StringBuilder(64);
            StringBuilder outjsonfilename = new StringBuilder(256);

            string logfilename = "";
            if (Log != null) { try { logfilename = Log.FileName; } catch { logfilename = ""; } }
            else { log = new TLogWrapperAbsDB(); }

            ImportAbsDBDll.CallExecuteSelect(SelectQuery, connectionid, outjsonfilename, _errorcode, logfilename, Log.MaxSizeMB, Log.Level);

            lasterrorcode = (TAbsDBErrorCodes)Convert.ToInt32(_errorcode.ToString());
            ErrorCode = lasterrorcode;

            jsonfilename = outjsonfilename.ToString();

            if (!File.Exists(jsonfilename)) { ErrorCode = TAbsDBErrorCodes.SwapDataNotExists; lasterrorcode = ErrorCode; }

            if (ErrorCode == TAbsDBErrorCodes.NoError)
            {
                // Lettura file json          
                JsonTextReader reader = new JsonTextReader(new StreamReader(jsonfilename));

                bool binc = false;
                int idx = -1;

                bool exit = false;

                while (!exit)
                {
                    try
                    {
                        exit = !reader.Read();

                        if (reader.Value != null)
                        {
                            switch (idx)
                            {
                                case 0:
                                    // Lettura numero di campi
                                    fieldcount = StrToIntDef(reader.Value.ToString());
                                    break;
                                case 1:
                                    // Lettura nome campi e tipo
                                    i = 0;
                                    while (i < FieldCount)
                                    {

                                        adata.fieldname = reader.Value.ToString();
                                        exit = !reader.Read();
                                        adata.fieldtype = reader.Value.ToString();
                                        adata.fieldvalue = new TAbsDBListFields();
                                        Fields.Add(adata);
                                        if (i < FieldCount - 1) exit = !reader.Read();
                                        i++;
                                    }
                                    break;
                                default:
                                    // Lettura dati
                                    i = 0;
                                    while (i < FieldCount)
                                    {
                                        exit = !reader.Read();
                                        Fields[i].fieldvalue.Add(reader.Value.ToString());
                                        exit = !reader.Read();
                                        i++;
                                    }
                                    break;
                            }
                            binc = false;
                        }
                        else if (!binc) { binc = true; idx++; }
                    }
                    catch { }
                }

                recordcount = Fields[0].fieldvalue.Count;
                try { reader.CloseInput = true; reader.Close(); } catch { }

                // Distruzione file json
                try
                {
                    // Controlla se il file esiste
                    if (File.Exists(jsonfilename))
                    {
                        // Se esiste lo distrugge 
                        File.Delete(jsonfilename);
                    }
                }
                catch { }
            }

        }

        public void ExecuteSelect(string SelectQuery)
        {
            TAbsDBErrorCodes errorcode = TAbsDBErrorCodes.NoError;
            ExecuteSelect(SelectQuery, out errorcode);
        }
    }
    public class WrapperAbsDBJson : IWrapperAbsDB
    {
        private List<IAbsDBQuery> abs_db_wrap = new List<IAbsDBQuery>();

        TAbsDBErrorCodes lasterrorcode;
        ILogWrapperAbsDB log;

        public WrapperAbsDBJson()
        {
            log = null;
        }

        public WrapperAbsDBJson(ILogWrapperAbsDB log)
        {
            this.log = log;
        }
        public ILogWrapperAbsDB Log { get { return log; } set { log = value; } }
        public TAbsDBErrorCodes LastErrorCode
        {
            get => lasterrorcode;
        }
        public List<IAbsDBQuery> Wrap
        {
            get => abs_db_wrap;
        }
        //////private int ConnectionID_To_Idx(int ConnectionID)
        //////{
        //////    int x = -1; int i = 0;
        //////    while ((x == -1) && (i < abs_db_wrap.Count))
        //////    {
        //////        if (abs_db_wrap[i].ConnectionID == ConnectionID) x = i; else i++;
        //////    }
        //////    return x;
        //////}
        public void NewConnection(string DBFileName, string JsonFilePath, out TAbsDBErrorCodes ErrorCode)
        {
            IAbsDBQuery a;
            StringBuilder _errorcode = new StringBuilder(64);
            int connidx;
            ErrorCode = TAbsDBErrorCodes.NoError;

            string logfilename = "";
            if (Log != null) { try { logfilename = Log.FileName; } catch { logfilename = ""; } }
            else { log = new TLogWrapperAbsDB(); }
            // Passaggio dati verso DLL
            try
            {
                connidx = ImportAbsDBDll.CallNewConnection(DBFileName, JsonFilePath, _errorcode, logfilename, Log.MaxSizeMB, Log.Level);
                ErrorCode = (TAbsDBErrorCodes)Convert.ToInt32(_errorcode.ToString());
                lasterrorcode = ErrorCode;
                a = new TAbsDBQueryJson(connidx, new TListAbsDBQuery());
                abs_db_wrap.Add(a);
            }
            catch { connidx = -1; }
        }
        public void NewConnection(string DBFileName, string JsonFilePath)
        {
            TAbsDBErrorCodes errorcode = TAbsDBErrorCodes.NoError;
            NewConnection(DBFileName, JsonFilePath, out errorcode);
        }
        public void RemoveConnection(int index)
        {
            ImportAbsDBDll.CallRemoveConnection(abs_db_wrap[index].ConnectionID);
            int i = 0;
            while (i < abs_db_wrap[index].Fields.Count())
            {
                abs_db_wrap[index].Fields[i].fieldvalue.Clear(); i++;
            }
            abs_db_wrap.RemoveAt(index);
        }
    }
}
