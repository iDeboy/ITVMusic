using ITVMusic.Models;
using ITVMusic.Repositories.Bases;
using ITVMusic.Util;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ITVMusic.Repositories {
    public class UserRepository : RepositoryBase, IUserRepository {
        public async Task<bool> Add(UserModel? user) {

            if (user is null) return false;

            var all = await GetByAll();

            if (all.Any(u => u.NoControl == user.NoControl)) return false;

            if (all.Any(u => u.Nickname == user.Nickname)) return false;

            if (all.Any(u => u.Email == user.Email)) return false;

            using (var connection = GetConnection()) {

                using var command = new MySqlCommand();

                await connection.OpenAsync();
                command.Connection = connection;
                command.CommandText = "Insert Into Usuario (Usuario_NoControl, Usuario_Nombre, Usuario_ApellidoPaterno, Usuario_ApellidoMaterno, Usuario_Nickname, Usuario_Genero, Usuario_Correo, Usuario_FechaNacimiento, Usuario_Telefono, Usuario_Contraseña, Usuario_Icono, Suscripcion_CodigoSuscripcion)\n";
                command.CommandText += "Values(@noControl, @nombre, @apellidoPat, @apellidoMat, @nickname, @genero, @correo, @fechaNacimiento, @telefono, @contraseña, @icono, @suscripcionId);";

                command.Parameters.Add("@noControl", MySqlDbType.VarChar).Value = user.NoControl;
                command.Parameters.Add("@nombre", MySqlDbType.VarChar).Value = user.Name;
                command.Parameters.Add("@apellidoPat", MySqlDbType.VarChar).Value = user.LastNamePat;
                command.Parameters.Add("@apellidoMat", MySqlDbType.VarChar).Value = user.LastNameMat;
                command.Parameters.Add("@nickname", MySqlDbType.VarChar).Value = user.Nickname;
                command.Parameters.Add("@genero", MySqlDbType.Enum).Value = user.Gender?.First();
                command.Parameters.Add("@correo", MySqlDbType.VarChar).Value = user.Email;
                command.Parameters.Add("@fechaNacimiento", MySqlDbType.Date).Value = user.Birthday;
                command.Parameters.Add("@telefono", MySqlDbType.UInt32).Value = string.IsNullOrWhiteSpace(user.PhoneNumber) ? null : user.PhoneNumber;
                command.Parameters.Add("@contraseña", MySqlDbType.VarChar).Value = user.Password;
                command.Parameters.Add("@icono", MySqlDbType.MediumBlob).Value = user.Icon?.ToByteArray();
                command.Parameters.Add("@suscripcionId", MySqlDbType.Byte).Value = user.SuscriptionId;
           
                await command.ExecuteNonQueryAsync();
            }

            return true;
        }
        public async Task<bool> AutenticateUser(NetworkCredential credential) {

            bool validUser = false;

            bool isNoControl = Regex.IsMatch(credential.UserName, @"[A-z]\d{8}");

            using (var connection = GetConnection()) {

                using var command = new MySqlCommand();

                await connection.OpenAsync();
                command.Connection = connection;

                if (isNoControl) {
                    command.CommandText = "Select * From Usuario Where Usuario_NoControl = @username And Usuario_Contraseña = @password";
                    credential.UserName = credential.UserName.ToUpper();
                } else {
                    command.CommandText = "Select * From Usuario Where Usuario_Nickname = @username And Usuario_Contraseña = @password";
                }

                command.Parameters.Add("@username", MySqlDbType.VarChar).Value = credential.UserName;
                command.Parameters.Add("@password", MySqlDbType.VarChar).Value = credential.Password;

                validUser = await command.ExecuteScalarAsync() != null;
            }

            return validUser;
        }

        public Task<bool> Edit(UserModel? user) {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<UserModel>> GetByAll() {

            List<UserModel> allUsers = new();

            using (var connection = GetConnection()) {

                using var command = new MySqlCommand();

                await connection.OpenAsync();
                command.Connection = connection;
                command.CommandText = "Select * From Usuario";

                using var reader = await command.ExecuteReaderAsync();

                while (await reader.ReadAsync()) {

                    allUsers.Add(new() {
                        NoControl = (string)reader["Usuario_NoControl"],
                        SuscriptionId = (byte)reader["Suscripcion_CodigoSuscripcion"],
                        Icon = reader["Usuario_Icono"].ToImage(),
                        Name = (string)reader["Usuario_Nombre"],
                        LastNamePat = (string)reader["Usuario_ApellidoPaterno"],
                        LastNameMat = (string)reader["Usuario_ApellidoMaterno"],
                        Nickname = (string)reader["Usuario_Nickname"],
                        Gender = (string)reader["Usuario_Genero"],
                        Email = (string)reader["Usuario_Correo"],
                        Birthday = ((DateTime)reader["Usuario_FechaNacimiento"]).ToDateOnly(),
                        PhoneNumber = reader["Usuario_Telefono"].ToString(),
                        Password = string.Empty,
                        ContratationDate = (DateTime)reader["Usuario_FechaContratacion"],
                    });

                }

            }

            return allUsers;
        }

        public async Task<UserModel?> GetByNoControl(string? noControl) {

            UserModel? user = null;

            noControl = noControl?.ToUpper();

            using (var connection = GetConnection()) {

                using var command = new MySqlCommand();

                await connection.OpenAsync();
                command.Connection = connection;

                command.CommandText = "Select * From Usuario Where Usuario_NoControl = @noControl";
                command.Parameters.Add("@noControl", MySqlDbType.VarChar).Value = noControl;

                using var reader = await command.ExecuteReaderAsync();

                if (await reader.ReadAsync()) {
                    user = new UserModel() {
                        NoControl = (string)reader["Usuario_NoControl"],
                        SuscriptionId = (byte)reader["Suscripcion_CodigoSuscripcion"],
                        Icon = reader["Usuario_Icono"].ToImage(),
                        Name = (string)reader["Usuario_Nombre"],
                        LastNamePat = (string)reader["Usuario_ApellidoPaterno"],
                        LastNameMat = (string)reader["Usuario_ApellidoMaterno"],
                        Nickname = (string)reader["Usuario_Nickname"],
                        Gender = (string)reader["Usuario_Genero"],
                        Email = (string)reader["Usuario_Correo"],
                        Birthday = ((DateTime)reader["Usuario_FechaNacimiento"]).ToDateOnly(),
                        PhoneNumber = reader["Usuario_Telefono"].ToString(),
                        Password = string.Empty,
                        ContratationDate = (DateTime)reader["Usuario_FechaContratacion"],
                    };
                }


            }

            return user;
        }

        public async Task<UserModel?> GetByNoControlOrUsername(string? key) {

            if (key is null) return null;

            if (Regex.IsMatch(key, @"[A-z]\d{8}")) return await GetByNoControl(key);

            return await GetByUsername(key);
        }

        public async Task<UserModel?> GetByUsername(string? username) {

            UserModel? user = null;

            using (var connection = GetConnection()) {

                using var command = new MySqlCommand();

                await connection.OpenAsync();
                command.Connection = connection;

                command.CommandText = "Select * From Usuario Where Usuario_Nickname = @username";
                command.Parameters.Add("@username", MySqlDbType.VarChar).Value = username;

                using var reader = await command.ExecuteReaderAsync();

                if (await reader.ReadAsync()) {
                    user = new UserModel() {
                        NoControl = (string)reader["Usuario_NoControl"],
                        SuscriptionId = (ushort)reader["Suscripcion_CodigoSuscripcion"],
                        Icon = reader["Usuario_Icono"].ToImage(),
                        Name = (string)reader["Usuario_Nombre"],
                        LastNamePat = (string)reader["Usuario_ApellidoPaterno"],
                        LastNameMat = (string)reader["Usuario_ApellidoMaterno"],
                        Nickname = (string)reader["Usuario_Nickname"],
                        Gender = (string)reader["Usuario_Genero"],
                        Email = (string)reader["Usuario_Correo"],
                        Birthday = ((DateTime)reader["Usuario_FechaNacimiento"]).ToDateOnly(),
                        PhoneNumber = reader["Usuario_Telefono"].ToString(),
                        Password = string.Empty,
                        ContratationDate = (DateTime)reader["Usuario_FechaContratacion"],
                    };
                }

            }

            return user;
        }

        public Task<bool> Remove(string? noControl) {
            throw new NotImplementedException();
        }
    }
}
