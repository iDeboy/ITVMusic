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
                Server = "192.168.0.152",
                Database = "ITVMusic",
                UserID = "App",
                Password = "hono2002",
            }.ConnectionString;

        }

        protected MySqlConnection GetConnection() {
            return new(ConnectionString);
        }

    }
}
