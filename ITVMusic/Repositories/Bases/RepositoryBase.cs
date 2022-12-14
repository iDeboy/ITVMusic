using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITVMusic.Repositories.Bases {
    public abstract class RepositoryBase {

        public string? ConnectionString { get; }

        protected RepositoryBase() {

            ConnectionString = new MySqlConnectionStringBuilder() {
                Server = "localhost",
                //Server = "192.168.0.152",
                //Server = "172.20.10.12",
                Database = "ITVMusic",
                UserID = "root",
                Password = "hono2002",
                //ConnectionTimeout = 5
            }.ConnectionString;

        }

        protected MySqlConnection GetConnection() {
            return new(ConnectionString);
        }

    }
}
