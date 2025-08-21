using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pseven
{
    public interface IAbsDBQuery
    {
        int FieldCount { get; }
        int RecordCount { get; }
        int ConnectionID { get; }
        ILogWrapperAbsDB Log { get; set; }
        TListAbsDBQuery Fields { get; }

        TAbsDBStruct FieldByName(string fieldname);
        TAbsDBErrorCodes LastErrorCode { get; }
        string ExecuteSelectMaui(string SelectQuery, out TAbsDBErrorCodes ErrorCode);
        void ExecuteSelect(string SelectQuery, out TAbsDBErrorCodes ErrorCode);
        void ExecuteSelect(string SelectQuery);

    }
    public interface IWrapperAbsDB
    {
        List<IAbsDBQuery> Wrap { get; }
        TAbsDBErrorCodes LastErrorCode { get; }
        ILogWrapperAbsDB Log { get; set; }
        void NewConnection(string DBFileName, string OutFilePath, out TAbsDBErrorCodes ErrorCode);
        void NewConnection(string DBFileName, string OutFilePath);
        void RemoveConnection(int index);
    }
    public interface ILogWrapperAbsDB
    {
        string FileName { get; set; }
        int Level { get; set; }
        int MaxSizeMB { get; set; }
    }
    [Serializable()]
    public class InvalidFieldNameException : Exception
    {
        public InvalidFieldNameException() : base() { }
        public InvalidFieldNameException(string message) : base(message) { }
        public InvalidFieldNameException(string message, Exception inner) : base(message, inner) { }
        protected InvalidFieldNameException(System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
