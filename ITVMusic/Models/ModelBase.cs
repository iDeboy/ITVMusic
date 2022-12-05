using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITVMusic.Models {
    public abstract class ModelBase {

        protected ModelBase() { }
        protected ModelBase(MySqlDataReader reader) { }
    }
}
