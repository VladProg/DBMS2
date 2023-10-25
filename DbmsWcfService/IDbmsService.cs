using DbmsWcfService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace DbmsWcfService
{
    [ServiceContract]
    public interface IDbmsService
    {
        [OperationContract]
        void CreateDatabase(string dbName);
        [OperationContract]
        DatabaseInfo GetDatabase(string dbName);
        [OperationContract]
        void DeleteDatabase(string dbName);
        [OperationContract]
        int AddTable(string dbName, string tableName, Column[] columns);
        [OperationContract]
        void RemoveTable(string dbName, int tableid);
        [OperationContract]
        Table GetTable(string dbName, int tableId);
        [OperationContract]
        int AddRow(string dbName, int tableId, string[] cells);
        [OperationContract]
        void RemoveRow(string dbName, int tableId, int rowId);
        [OperationContract]
        void ValidateCell(string dbName, int tableId, int columnId, string value);
        [OperationContract]
        void UpdateCell(string dbName, int tableId, int rowId, int columnId, string value);
        [OperationContract]
        Table GetTableDifference(string dbName, int leftTableId, int rightTableId);
        [OperationContract]
        bool TableExists(string dbName, int tableId);
    }
}
