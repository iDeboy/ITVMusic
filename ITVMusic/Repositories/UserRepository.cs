using ITVMusic.Models;
using ITVMusic.Repositories.Bases;
using ITVMusic.Util;
using Microsoft.VisualBasic.ApplicationServices;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ITVMusic.Repositories {
    public class UserRepository : RepositoryBase, IUserRepository {
        public bool Add(UserModel? user) {

            if (user is null) return false;

            if (GetById(user.NoControl) is not null) return false;
            if (GetByUsername(user.Nickname) is not null) return false;
            if (GetByEmail(user.Email) is not null) return false;

            var connection = GetConnection();

            connection.Open();

            using (var command = connection.CreateCommand()) {

                command.CommandText = "Insert Into Usuario (Usuario_NoControl, Usuario_Nombre, Usuario_ApellidoPaterno, Usuario_ApellidoMaterno, Usuario_Nickname, Usuario_Genero, Usuario_Correo, Usuario_FechaNacimiento, Usuario_Telefono, Usuario_Contraseña, Usuario_Icono, Suscripcion_CodigoSuscripcion)\n";
                command.CommandText += "Values(@noControl, @nombre, @apellidoPat, @apellidoMat, @nickname, @genero, @correo, @fechaNacimiento, @telefono, @contraseña, @icono, @suscripcionId);";

                command.Parameters.Add("@noControl", MySqlDbType.VarChar).Value = user.NoControl;
                command.Parameters.Add("@nombre", MySqlDbType.VarChar).Value = user.Name;
                command.Parameters.Add("@apellidoPat", MySqlDbType.VarChar).Value = user.LastNamePat;
                command.Parameters.Add("@apellidoMat", MySqlDbType.VarChar).Value = user.LastNameMat;
                command.Parameters.Add("@nickname", MySqlDbType.VarChar).Value = user.Nickname;
                command.Parameters.Add("@genero", MySqlDbType.Enum).Value = user.Gender!.First();
                command.Parameters.Add("@correo", MySqlDbType.VarChar).Value = user.Email;
                command.Parameters.Add("@fechaNacimiento", MySqlDbType.Date).Value = user.Birthday;
                command.Parameters.Add("@telefono", MySqlDbType.UInt32).Value = string.IsNullOrWhiteSpace(user.PhoneNumber) ? null : user.PhoneNumber;
                command.Parameters.Add("@contraseña", MySqlDbType.VarChar).Value = user.Password;
                command.Parameters.Add("@icono", MySqlDbType.MediumBlob).Value = user.Icon.ToByteArray();
                command.Parameters.Add("@suscripcionId", MySqlDbType.Byte).Value = user.SuscriptionId;

                command.ExecuteNonQuery();
            }

            connection.Close();

            return true;
        }

        public async Task<bool> AddAsync(UserModel? user) {

            if (user is null) return false;

            if ((await GetByIdAsync(user.NoControl)) is not null) return false;
            if ((await GetByUsernameAsync(user.Nickname)) is not null) return false;
            if ((await GetByEmailAsync(user.Email)) is not null) return false;

            var connection = GetConnection();

            await connection.OpenAsync();

            using (var command = connection.CreateCommand()) {

                command.CommandText = "Insert Into Usuario (Usuario_NoControl, Usuario_Nombre, Usuario_ApellidoPaterno, Usuario_ApellidoMaterno, Usuario_Nickname, Usuario_Genero, Usuario_Correo, Usuario_FechaNacimiento, Usuario_Telefono, Usuario_Contraseña, Usuario_Icono, Suscripcion_CodigoSuscripcion)\n";
                command.CommandText += "Values(@noControl, @nombre, @apellidoPat, @apellidoMat, @nickname, @genero, @correo, @fechaNacimiento, @telefono, @contraseña, @icono, @suscripcionId);";

                command.Parameters.Add("@noControl", MySqlDbType.VarChar).Value = user.NoControl;
                command.Parameters.Add("@nombre", MySqlDbType.VarChar).Value = user.Name;
                command.Parameters.Add("@apellidoPat", MySqlDbType.VarChar).Value = user.LastNamePat;
                command.Parameters.Add("@apellidoMat", MySqlDbType.VarChar).Value = user.LastNameMat;
                command.Parameters.Add("@nickname", MySqlDbType.VarChar).Value = user.Nickname;
                command.Parameters.Add("@genero", MySqlDbType.Enum).Value = user.Gender!.First();
                command.Parameters.Add("@correo", MySqlDbType.VarChar).Value = user.Email;
                command.Parameters.Add("@fechaNacimiento", MySqlDbType.Date).Value = user.Birthday;
                command.Parameters.Add("@telefono", MySqlDbType.UInt32).Value = string.IsNullOrWhiteSpace(user.PhoneNumber) ? null : user.PhoneNumber;
                command.Parameters.Add("@contraseña", MySqlDbType.VarChar).Value = user.Password;
                command.Parameters.Add("@icono", MySqlDbType.MediumBlob).Value = await user.Icon.ToByteArrayAsync();
                command.Parameters.Add("@suscripcionId", MySqlDbType.Byte).Value = user.SuscriptionId;

                await command.ExecuteNonQueryAsync();
            }

            await connection.CloseAsync();

            return true;
        }

        public bool AutenticateUser(NetworkCredential credential) {

            bool validUser = false;

            bool isNoControl = Regex.IsMatch(credential.UserName, @"[A-z]\d{8}");

            var connection = GetConnection();

            connection.Open();

            using (var command = connection.CreateCommand()) {

                if (isNoControl) {
                    command.CommandText = "Select * From Usuario Where Usuario_NoControl = @username And Usuario_Contraseña = @password;";
                    credential.UserName = credential.UserName.ToUpper();
                } else {
                    command.CommandText = "Select * From Usuario Where Usuario_Nickname = @username And Usuario_Contraseña = @password;";
                }

                command.Parameters.Add("@username", MySqlDbType.VarChar).Value = credential.UserName;
                command.Parameters.Add("@password", MySqlDbType.VarChar).Value = credential.Password;

                validUser = command.ExecuteScalar() != null;
            }

            connection.Close();

            return validUser;

        }

        public async Task<bool> AutenticateUserAsync(NetworkCredential credential) {

            bool validUser = false;

            bool isNoControl = Regex.IsMatch(credential.UserName, @"[A-z]\d{8}");

            var connection = GetConnection();

            await connection.OpenAsync();

            using (var command = connection.CreateCommand()) {

                if (isNoControl) {
                    command.CommandText = "Select * From Usuario Where Usuario_NoControl = @username And Usuario_Contraseña = @password;";
                    credential.UserName = credential.UserName.ToUpper();
                } else {
                    command.CommandText = "Select * From Usuario Where Usuario_Nickname = @username And Usuario_Contraseña = @password;";
                }

                command.Parameters.Add("@username", MySqlDbType.VarChar).Value = credential.UserName;
                command.Parameters.Add("@password", MySqlDbType.VarChar).Value = credential.Password;

                validUser = await command.ExecuteScalarAsync() != null;
            }

            await connection.CloseAsync();

            return validUser;
        }

        public bool Edit(UserModel? obj) {
            throw new NotImplementedException();
        }

        public Task<bool> EditAsync(UserModel? user) {
            throw new NotImplementedException();
        }

        public IEnumerable<UserModel> GetByAll() {

            List<UserModel> allUsers = new();

            var connection = GetConnection();

            connection.Open();

            using (var command = connection.CreateCommand()) {

                command.CommandText = "Select * From Usuario;";

                using var reader = command.ExecuteReader();

                while (reader.Read()) {
                    allUsers.Add(new UserModel(reader));
                }

            }

            connection.Close();

            return allUsers;
        }

        public async Task<IEnumerable<UserModel>> GetByAllAsync() {

            List<UserModel> allUsers = new();

            var connection = GetConnection();

            await connection.OpenAsync();

            using (var command = connection.CreateCommand()) {

                command.CommandText = "Select * From Usuario;";

                using var reader = await command.ExecuteReaderAsync();

                while (await reader.ReadAsync()) {
                    allUsers.Add(new UserModel(reader));
                }

            }

            await connection.CloseAsync();

            return allUsers;
        }

        public UserModel? GetByEmail(string? email) {

            if (email is null) return null;

            UserModel? user = null;

            var connection = GetConnection();

            connection.Open();

            using (var command = connection.CreateCommand()) {

                command.CommandText = "Select * From Usuario Where Usuario_Correo = @email;";

                command.Parameters.Add("@email", MySqlDbType.VarChar).Value = email;

                using var reader = command.ExecuteReader();

                if (reader.Read()) user = new UserModel(reader);

            }

            connection.Close();

            return user;
        }

        public async Task<UserModel?> GetByEmailAsync(string? email) {

            if (email is null) return null;

            UserModel? user = null;

            var connection = GetConnection();

            await connection.OpenAsync();

            using (var command = connection.CreateCommand()) {

                command.CommandText = "Select * From Usuario Where Usuario_Correo = @email;";

                command.Parameters.Add("@email", MySqlDbType.VarChar).Value = email;

                using var reader = await command.ExecuteReaderAsync();

                if (await reader.ReadAsync()) user = new UserModel(reader);


            }

            await connection.CloseAsync();

            return user;
        }

        public UserModel? GetById(object? noControl) {

            if (noControl is not string userNoControl) return null;

            if (string.IsNullOrWhiteSpace(userNoControl)) return null;

            userNoControl = userNoControl.ToUpper();

            UserModel? user = null;

            var connection = GetConnection();

            connection.Open();

            using (var command = connection.CreateCommand()) {

                command.CommandText = "Select * From Usuario Where Usuario_NoControl = @noControl;";

                command.Parameters.Add("@noControl", MySqlDbType.VarChar).Value = userNoControl;

                using var reader = command.ExecuteReader();

                if (reader.Read()) user = new UserModel(reader);

            }

            return user;

        }

        public async Task<UserModel?> GetByIdAsync(object? noControl) {

            if (noControl is not string userNoControl) return null;

            if (string.IsNullOrWhiteSpace(userNoControl)) return null;

            userNoControl = userNoControl.ToUpper();

            UserModel? user = null;

            var connection = GetConnection();

            await connection.OpenAsync();

            using (var command = connection.CreateCommand()) {

                command.CommandText = "Select * From Usuario Where Usuario_NoControl = @noControl;";

                command.Parameters.Add("@noControl", MySqlDbType.VarChar).Value = userNoControl;

                using var reader = await command.ExecuteReaderAsync();

                if (await reader.ReadAsync()) user = new UserModel(reader);

            }

            return user;
        }

        public UserModel? GetByNoControlOrUsername(string? key) {

            if (key is null) return null;

            if (Regex.IsMatch(key, @"[A-z]\d{8}")) return GetById(key);

            return GetByUsername(key);

        }

        public async Task<UserModel?> GetByNoControlOrUsernameAsync(string? key) {

            if (key is null) return null;

            if (Regex.IsMatch(key, @"[A-z]\d{8}")) return await GetByIdAsync(key);

            return await GetByUsernameAsync(key);
        }

        public UserModel? GetByUsername(string? username) {

            if (username is null) return null;

            UserModel? user = null;

            var connection = GetConnection();

            connection.Open();

            using (var command = connection.CreateCommand()) {

                command.CommandText = "Select * From Usuario Where Usuario_Nickname = @nickname;";

                command.Parameters.Add("@nickname", MySqlDbType.VarChar).Value = username;

                using var reader = command.ExecuteReader();

                if (reader.Read()) user = new UserModel(reader);

            }

            connection.Close();

            return user;

        }

        public async Task<UserModel?> GetByUsernameAsync(string? username) {

            if (username is null) return null;

            UserModel? user = null;

            var connection = GetConnection();

            await connection.OpenAsync();

            using (var command = connection.CreateCommand()) {

                command.CommandText = "Select * From Usuario Where Usuario_Nickname = @nickname;";

                command.Parameters.Add("@nickname", MySqlDbType.VarChar).Value = username;

                using var reader = await command.ExecuteReaderAsync();

                if (await reader.ReadAsync()) user = new UserModel(reader);

            }

            await connection.CloseAsync();

            return user;
        }

        public IEnumerable<UserModel>? GetFrom(PlaylistModel playlist) {

            if (playlist is null) return null;

            var ids = new List<string?>();
            var users = new List<UserModel?>();

            var connection = GetConnection();

            connection.Open();

            using (var command = connection.CreateCommand()) {

                command.CommandText = "Select Usuario_NoControl From Crea Where Playlist_Codigo = @playlistId Order By Fecha Asc;";

                command.Parameters.Add("@playlistId", MySqlDbType.Int32).Value = playlist.Id;

                using var reader = command.ExecuteReader();

                while (reader.Read()) {
                    ids.Add(Convert.ToString(reader["Usuario_NoControl"]));
                }

            }

            connection.Close();

            foreach (var id in ids) {
                users.Add(GetById(id));
            }

            return from user in users
                   where user is not null
                   select user;

        }

        public async Task<IEnumerable<UserModel>?> GetFromAsync(PlaylistModel playlist) {

            if (playlist is null) return null;

            var ids = new List<string?>();
            var tasks = new List<Task<UserModel?>>();

            var connection = GetConnection();

            await connection.OpenAsync();

            using (var command = connection.CreateCommand()) {

                command.CommandText = "Select Usuario_NoControl From Crea Where Playlist_Codigo = @playlistId Order By Fecha Asc;";

                command.Parameters.Add("@playlistId", MySqlDbType.Int32).Value = playlist.Id;

                using var reader = await command.ExecuteReaderAsync();

                while (await reader.ReadAsync()) {
                    ids.Add(Convert.ToString(reader["Usuario_NoControl"]));
                }

            }

            await connection.CloseAsync();

            foreach (var id in ids) {
                tasks.Add(GetByIdAsync(id));
            }

            await Task.WhenAll(tasks);

            return from task in tasks
                   where task.Result is not null
                   select task.Result;

        }

        public bool ListenToSong(UserModel? user, AlmacenModel? song) {

            if (user is null) return false;

            if (song is null) return false;

            if (song.Song is null || song.Album is null) return false;

            var userChecked = GetById(user.NoControl);
            var almacenChecked = App.AlmacenRepository.GetById(song.Id);

            // Comprobar que el usuario este en la base de datos
            if (userChecked is null) return false;

            // Comprobar que ese almacen exista
            if (almacenChecked is null) return false;

            var connection = GetConnection();

            connection.Open();

            using (var command = connection.CreateCommand()) {

                command.CommandText = "Insert Into Escucha (Usuario_NoControl, Almacena_Codigo)\n";
                command.CommandText += "Values(@noControl, @almacenId);";

                command.Parameters.Add("@noControl", MySqlDbType.VarChar).Value = user.NoControl;
                command.Parameters.Add("@almacenId", MySqlDbType.UInt32).Value = song.Id;

                command.ExecuteNonQuery();

            }

            connection.Close();

            return true;

        }

        public async Task<bool> ListenToSongAsync(UserModel? user, AlmacenModel? song) {

            if (user is null) return false;

            if (song is null) return false;

            if (song.Song is null || song.Album is null) return false;

            var userChecked = GetByIdAsync(user.NoControl);
            var almacenChecked = App.AlmacenRepository.GetByIdAsync(song.Id);

            await Task.WhenAll(userChecked, almacenChecked);

            // Comprobar que el usuario este en la base de datos
            if (userChecked is null) return false;

            // Comprobar que ese almacen exista
            if (almacenChecked is null) return false;

            var connection = GetConnection();

            await connection.OpenAsync();

            using (var command = connection.CreateCommand()) {

                command.CommandText = "Insert Into Escucha (Usuario_NoControl, Almacena_Codigo)\n";
                command.CommandText += "Values(@noControl, @almacenId);";

                command.Parameters.Add("@noControl", MySqlDbType.VarChar).Value = user.NoControl;
                command.Parameters.Add("@almacenId", MySqlDbType.UInt32).Value = song.Id;

                await command.ExecuteNonQueryAsync();
            }

            await connection.CloseAsync();

            return true;
        }

        public bool RemoveById(object? id) {
            throw new NotImplementedException();
        }

        public Task<bool> RemoveByIdAsync(object? noControl) {
            throw new NotImplementedException();
        }

    }
}
