using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

namespace AddAssetIDTable.BusinessClasses
{
    class GetConnectionString
    {
        public GetConnectionString()
        {
        }

        private string _serverName;
        private string _dbName;


        public GetConnectionString(string ServerName, string DBName)
        {
            _serverName = ServerName;
            _dbName = DBName;
            _connectionString = @"Data Source='" + ServerName + "';Initial Catalog='" + DBName + "';Integrated Security=True;Pooling=False";
            _connectionStringStart = @"Data Source='" + ServerName + "';Initial Catalog='tempdb';Integrated Security=True;Pooling=False";

        }

        public GetConnectionString(string ServerName)
        {
            _serverName = ServerName; 
            _connectionStringStart = @"Data Source='" + ServerName + "';Initial Catalog='tempdb';Integrated Security=True;Pooling=False";

        }


        private string _connectionString;
        private string _connectionStringStart;

        public string ConnectionString
        {
            get
            {
                return _connectionString;
            }

            set
            {
                _connectionString = @"Data Source='" + _serverName + "';Initial Catalog='" + _dbName + "';Integrated Security=True;Pooling=False";
            }
        }

        public string ConnectionStringStart
        {
            get
            {
                return _connectionStringStart;
            }

            set
            {
                _connectionString = @"Data Source='" + _serverName + "';Initial Catalog='tempdb';Integrated Security=True;Pooling=False";
            }
        }


    }
}
