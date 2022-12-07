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
        public bool Add(SuscriptionModel? suscription) {

            if (suscription is null) return false;

            var connection = GetConnection();

            connection.Open();

            using (var command = connection.CreateCommand()) {

                command.CommandText = "Insert Into Suscripcion (Suscripcion_MetodoPago, Suscripcion_Tipo)\n";
                command.CommandText += "Value (@paymentMethod, @type);";

                command.Parameters.Add("@paymentMethod", MySqlDbType.VarChar).Value = suscription.PaymentMethod;
                command.Parameters.Add("@type", MySqlDbType.VarChar).Value = suscription.Type;

                command.ExecuteNonQuery();
            }

            connection.Open();

            return true;
        }

        public async Task<bool> AddAsync(SuscriptionModel? suscription) {

            if (suscription is null) return false;

            var connection = GetConnection();

            await connection.OpenAsync();

            using (var command = connection.CreateCommand()) {

                command.CommandText = "Insert Into Suscripcion (Suscripcion_MetodoPago, Suscripcion_Tipo)\n";
                command.CommandText += "Value (@paymentMethod, @type);";

                command.Parameters.Add("@paymentMethod", MySqlDbType.VarChar).Value = suscription.PaymentMethod;
                command.Parameters.Add("@type", MySqlDbType.VarChar).Value = suscription.Type;

                await command.ExecuteNonQueryAsync();
            }

            await connection.CloseAsync();

            return true;
        }

        public bool AutenticateUserSuscription(UserModel? user) {

            bool isValidSuscription = false;

            if (user is null) return false;

            var connection = GetConnection();

            connection.Open();

            using (var command = connection.CreateCommand()) {

                command.CommandText = "Select Caducada From Suscripciones Where Usuario_NoControl = @noControl";
                command.Parameters.Add("@noControl", MySqlDbType.VarChar).Value = user.NoControl;

                bool caducada = Convert.ToBoolean(command.ExecuteScalar());

                isValidSuscription = caducada;
            }

            connection.Close();

            return isValidSuscription;
        }

        public async Task<bool> AutenticateUserSuscriptionAsync(UserModel? user) {

            bool isValidSuscription = false;

            if (user is null) return false;

            var connection = GetConnection();

            await connection.OpenAsync();

            using (var command = connection.CreateCommand()) {

                command.CommandText = "Select Caducada From Suscripciones Where Usuario_NoControl = @noControl";
                command.Parameters.Add("@noControl", MySqlDbType.VarChar).Value = user.NoControl;

                bool caducada = Convert.ToBoolean(await command.ExecuteScalarAsync());

                isValidSuscription = caducada;
            }

            await connection.CloseAsync();

            return isValidSuscription;
        }

        public bool Edit(SuscriptionModel? suscription) {
            throw new NotImplementedException();
        }

        public Task<bool> EditAsync(SuscriptionModel? obj) {
            throw new NotImplementedException();
        }

        public IEnumerable<SuscriptionModel> GetByAll() {

            List<SuscriptionModel> allSuscriptions = new();

            var connection = GetConnection();

            connection.Open();

            using (var command = connection.CreateCommand()) {

                command.CommandText = "Select * From Suscripcion;";

                using var reader = command.ExecuteReader();

                while (reader.Read())
                    allSuscriptions.Add(new SuscriptionModel(reader));

            }

            connection.Close();

            return allSuscriptions;
        }

        public async Task<IEnumerable<SuscriptionModel>> GetByAllAsync() {

            List<SuscriptionModel> allSuscriptions = new();

            var connection = GetConnection();

            await connection.OpenAsync();

            using (var command = connection.CreateCommand()) {

                command.CommandText = "Select * From Suscripcion;";

                using var reader = await command.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                    allSuscriptions.Add(new SuscriptionModel(reader));

            }

            await connection.CloseAsync();

            return allSuscriptions;
        }

        public SuscriptionModel? GetById(object? id) {

            if (id is not ushort suscriptionId) return null;

            SuscriptionModel? suscription = null;

            var connection = GetConnection();

            connection.Open();

            using (var command = connection.CreateCommand()) {

                command.CommandText = "Select * From Suscripcion Where Suscripcion_Codigo = @suscriptionId;";

                command.Parameters.Add("@suscriptionId", MySqlDbType.UInt16).Value = suscriptionId;

                using var reader = command.ExecuteReader();

                if (reader.Read()) suscription = new SuscriptionModel(reader);

            }

            connection.Close();

            return suscription;

        }

        public async Task<SuscriptionModel?> GetByIdAsync(object? id) {

            if (id is not ushort suscriptionId) return null;

            SuscriptionModel? suscription = null;

            var connection = GetConnection();

            await connection.OpenAsync();

            using (var command = connection.CreateCommand()) {

                command.CommandText = "Select * From Suscripcion Where Suscripcion_Codigo = @suscriptionId;";

                command.Parameters.Add("@suscriptionId", MySqlDbType.UInt16).Value = suscriptionId;

                using var reader = await command.ExecuteReaderAsync();

                if (await reader.ReadAsync()) suscription = new SuscriptionModel(reader);

            }

            await connection.CloseAsync();

            return suscription;
        }

        public bool RemoveById(object? id) {
            throw new NotImplementedException();
        }

        public Task<bool> RemoveByIdAsync(object? id) {
            throw new NotImplementedException();
        }
    }
}
