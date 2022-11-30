using ITVMusic.Models;
using ITVMusic.Repositories.Bases;
using Microsoft.VisualBasic.ApplicationServices;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net;
using System.Threading.Tasks;

namespace ITVMusic.Repositories {
    public class SuscriptionRepository : RepositoryBase, ISuscriptionRepository {
        public Task<bool> Add(SuscriptionModel? suscription) {
            throw new NotImplementedException();
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

                bool caducada = Convert.ToBoolean(await command.ExecuteScalarAsync());

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

                using var reader = await command.ExecuteReaderAsync();

                while (await reader.ReadAsync()) {

                    allSuscriptions.Add(new() {
                        Id = (byte)reader["Suscripcion_CodigoSuscripcion"],
                        PaymentMethod = (string)reader["Suscripcion_MetodoPago"],
                        Type = (string)reader["Suscripcion_Tipo"],
                    });

                }

            }

            return allSuscriptions;
        }

        public Task<SuscriptionModel?> GetById(uint id) {
            throw new NotImplementedException();
        }

        public Task<bool> Remove(uint id) {
            throw new NotImplementedException();
        }
    }
}
