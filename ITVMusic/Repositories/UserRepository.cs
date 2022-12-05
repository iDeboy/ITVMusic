﻿using ITVMusic.Models;
using ITVMusic.Repositories.Bases;
using ITVMusic.Util;
using Microsoft.VisualBasic.ApplicationServices;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ITVMusic.Repositories {
    public class UserRepository : RepositoryBase, IUserRepository {

        private readonly IPlaylistRepository playlistRepository;
        private readonly IAlmacenRepository almacenRepository;

        public UserRepository() {
            playlistRepository = new PlaylistRepository();
            almacenRepository = new AlmacenRepository();
        }

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
                command.Parameters.Add("@genero", MySqlDbType.Enum).Value = user.Gender!.First();
                command.Parameters.Add("@correo", MySqlDbType.VarChar).Value = user.Email;
                command.Parameters.Add("@fechaNacimiento", MySqlDbType.Date).Value = user.Birthday;
                command.Parameters.Add("@telefono", MySqlDbType.UInt32).Value = string.IsNullOrWhiteSpace(user.PhoneNumber) ? null : user.PhoneNumber;
                command.Parameters.Add("@contraseña", MySqlDbType.VarChar).Value = user.Password;
                command.Parameters.Add("@icono", MySqlDbType.MediumBlob).Value = await user.Icon.ToByteArray();
                command.Parameters.Add("@suscripcionId", MySqlDbType.Byte).Value = user.SuscriptionId;

                command.ExecuteNonQuery();
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

                validUser = command.ExecuteScalar() != null;
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

                using var reader = command.ExecuteReader();

                while (reader.Read()) {

                    var user = new UserModel(reader);

                    // Obtener las playlists del usuario

                    var playlists = new List<Task<PlaylistModel?>>();

                    using (var commandPlaylist = new MySqlCommand()) {

                        commandPlaylist.Connection = connection;

                        commandPlaylist.CommandText = "Select Playlist_Codigo From Crea Where Usuario_NoControl = @noControl";
                        commandPlaylist.Parameters.Add("@noControl", MySqlDbType.VarChar).Value = user.NoControl;

                        using var crea = commandPlaylist.ExecuteReader();

                        while (crea.Read()) {

                            var playlistId = Convert.ToUInt32(crea["Playlist_Codigo"]);

                            playlists.Add(playlistRepository.GetById(playlistId));

                        }
                    }

                    // Obtener las canciones que ha escuchado el usuario

                    var songs = new List<Task<AlmacenModel?>>();

                    using (var commandSong = new MySqlCommand()) {

                        commandSong.Connection = connection;

                        commandSong.CommandText = "Select Almacena_Codigo From Escucha Where Usuario_NoControl = @noControl";
                        commandSong.Parameters.Add("@noControl", MySqlDbType.VarChar).Value = user.NoControl;

                        using var escucha = commandSong.ExecuteReader();

                        while (escucha.Read()) {

                            var almacenId = Convert.ToUInt32(escucha["Almacena_Codigo"]);

                            songs.Add(almacenRepository.GetById(almacenId));

                        }
                    }

                    await Task.WhenAll(songs);

                    foreach (var song in songs) {
                        if (song.Result is not null) user.Songs.Add(song.Result);
                    }

                    await Task.WhenAll(playlists);

                    foreach (var playlist in playlists) {

                        if (playlist.Result is not null) user.Playlists.Add(playlist.Result);

                    }

                    allUsers.Add(user);

                }

            }

            return allUsers;
        }

        public async Task<UserModel?> GetById(object? noControl) {

            if (noControl is not string userNoControl) return null;

            userNoControl = userNoControl.ToUpper();

            var all = await GetByAll();

            return (from it in all
                    where it.NoControl == userNoControl
                    select it).FirstOrDefault();
        }

        public async Task<UserModel?> GetByNoControlOrUsername(string? key) {

            if (key is null) return null;

            if (Regex.IsMatch(key, @"[A-z]\d{8}")) return await GetById(key);

            return await GetByUsername(key);
        }

        public async Task<UserModel?> GetByUsername(string? username) {

            if (username is null) return null;

            var all = await GetByAll();

            return (from it in all
                    where it.Nickname == username
                    select it).FirstOrDefault();

        }

        public async Task<bool> ListenToSong(UserModel? user, AlmacenModel? song) {

            if (user is null) return false;

            if (song is null) return false;

            if (song.Song is null || song.Album is null) return false;

            var allUsers = GetByAll();
            var allAlmacenes = almacenRepository.GetByAll();

            await Task.WhenAll(allUsers, allAlmacenes);

            // Comprobar que el usuario este en la base de datos
            if (!allUsers.Result.Any(u => u.NoControl == user.NoControl)) return false;

            // Comprobar que ese almacen exista
            if (!allAlmacenes.Result.Any(al => al.Id == song.Id)) return false;

            using (var connection = GetConnection()) {

                using var command = new MySqlCommand();

                await connection.OpenAsync();

                command.Connection = connection;

                command.CommandText = "Insert Into Escucha (Usuario_NoControl, Almacena_Codigo)\n";
                command.CommandText += "Values(@noControl, @almacenId);";

                command.Parameters.Add("@noControl", MySqlDbType.VarChar).Value = user.NoControl;
                command.Parameters.Add("@almacenId", MySqlDbType.UInt32).Value = song.Id;

                command.ExecuteNonQuery();
            }

            return true;
        }

        public Task<bool> RemoveById(object? noControl) {
            throw new NotImplementedException();
        }

    }
}
