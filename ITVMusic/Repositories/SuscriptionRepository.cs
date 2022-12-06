using ITVMusic.Models;
using ITVMusic.Repositories.Bases;
using Microsoft.VisualBasic.ApplicationServices;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace ITVMusic.Repositories {
    public class SuscriptionRepository : RepositoryBase, ISuscriptionRepository {
        public async Task<bool> Add(SuscriptionModel? suscription) {

            if (suscription is null) return false;

            using (var connection = GetConnection()) {

                using var command = new MySqlCommand();

                await connection.OpenAsync();

                command.Connection = connection;

                command.CommandText = "Insert Into Suscripcion (Suscripcion_MetodoPago, Suscripcion_Tipo)\n";
                command.CommandText += "Value (@paymentMethod, @type);";

                command.Parameters.Add("@paymentMethod", MySqlDbType.VarChar).Value = suscription.PaymentMethod;
                command.Parameters.Add("@type", MySqlDbType.VarChar).Value = suscription.Type;

                command.ExecuteNonQuery();
            }

            return true;
        }

        public async Task<bool> AutenticateUserSuscription(UserModel? user) {

            bool isValidSuscription = false;

            if (user is null) return false;

            using (var connection = GetConnection()) {

                using var command = new MySqlCommand();

                await connection.OpenAsync();
                command.Connection = connection;
                command.CommandText = "Select Caducada From Suscripciones Where Usuario_NoControl = @noControl";
                command.Parameters.Add("@noControl", MySqlDbType.VarChar).Value = user.NoControl;

                bool caducada = Convert.ToBoolean(command.ExecuteScalar());

                isValidSuscription = caducada;
            }

            return isValidSuscription;
        }

        public Task<bool> Edit(SuscriptionModel? suscription) {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<SuscriptionModel>> GetByAll() {

            List<SuscriptionModel> allSuscriptions = new();

            using (var connection = GetConnection()) {

                using var command = new MySqlCommand();

                await connection.OpenAsync();

                command.Connection = connection;

                command.CommandText = "Select * From Suscripcion";

                using var reader = command.ExecuteReader();

                while (reader.Read()) {

                    allSuscriptions.Add(new SuscriptionModel(reader));

                }

            }

            return allSuscriptions;
        }

        public async Task<SuscriptionModel?> GetById(object? id) {

            if (id is not ushort suscriptionId) return null;

            SuscriptionModel? suscription = null;

            using (var connection = GetConnection()) {

                using var command = new MySqlCommand();

                await connection.OpenAsync();

                command.Connection = connection;

                command.CommandText = "Select * From Suscripcion Where Suscripcion_Codigo = @suscriptionId;";

                command.Parameters.Add("@suscriptionId", MySqlDbType.UInt16).Value = suscriptionId;

                using var reader = command.ExecuteReader();

                if (reader.Read()) {

                    suscription = new SuscriptionModel(reader);

                }

            }

            return suscription;

        }

        public Task<bool> RemoveById(object? id) {
            throw new NotImplementedException();
        }
    }
}
