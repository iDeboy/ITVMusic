using ITVMusic.Models;
using ITVMusic.Repositories.Bases;
using ITVMusic.Util;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Media;

namespace ITVMusic.Repositories {
    public class PlaylistRepository : RepositoryBase, IPlaylistRepository {

        public bool Add(PlaylistModel? playlist) {

            if (playlist is null) return false;

            var connection = GetConnection();

            connection.Open();

            using (var command = connection.CreateCommand()) {

                command.Connection = connection;

                command.CommandText = "Insert Into PlayList (Playlist_Titulo, Playlist_Icono)\n";
                command.CommandText += "Values (@titulo, @icono);";

                command.Parameters.Add("@titulo", MySqlDbType.TinyText).Value = playlist.Title;
                command.Parameters.Add("@icono", MySqlDbType.MediumBlob).Value = playlist.Icon.ToByteArray();

                command.ExecuteNonQuery();

            }

            connection.Close();

            return true;

        }

        public async Task<bool> AddAsync(PlaylistModel? playlist) {

            if (playlist is null) return false;

            var connection = GetConnection();

            await connection.OpenAsync();

            using (var command = connection.CreateCommand()) {

                command.Connection = connection;

                command.CommandText = "Insert Into PlayList (Playlist_Titulo, Playlist_Icono)\n";
                command.CommandText += "Values (@titulo, @icono);";

                command.Parameters.Add("@titulo", MySqlDbType.TinyText).Value = playlist.Title;
                command.Parameters.Add("@icono", MySqlDbType.MediumBlob).Value = await playlist.Icon.ToByteArrayAsync();

                await command.ExecuteNonQueryAsync();

            }

            await connection.CloseAsync();

            return true;

        }

        public bool AttatchAuthor(UserModel? user, PlaylistModel? playlist) {

            if (playlist is null || user is null) return false;

            // Revisar si existe la playlist y el usuario en la base de datos

            if ((GetById(playlist.Id)) is null) return false;

            if (App.UserRepository.GetById(user.NoControl) is null) return false;

            var users = App.UserRepository.GetFrom(playlist);

            // Compobar que el autor no este ya en la playlist
            if (users is null || users.Any(u => u.NoControl == user.NoControl)) return false;

            var connection = GetConnection();

            connection.Open();

            using (var command = connection.CreateCommand()) {

                command.CommandText = "Insert Into Crea (Playlist_Codigo, Usuario_NoControl)\n";
                command.CommandText += "Values (@playlistId, @userId);";

                command.Parameters.Add("@playlistId", MySqlDbType.UInt32).Value = playlist.Id;
                command.Parameters.Add("@userId", MySqlDbType.VarChar).Value = user.NoControl;

                command.ExecuteNonQuery();
            }

            connection.Close();

            return true;

        }

        public async Task<bool> AttatchAuthorAsync(UserModel? user, PlaylistModel? playlist) {

            if (playlist is null || user is null) return false;

            // Revisar si existe la playlist y el usuario en la base de datos

            if ((await GetByIdAsync(playlist.Id)) is null) return false;

            if ((await App.UserRepository.GetByIdAsync(user.NoControl)) is null) return false;

            var users = await App.UserRepository.GetFromAsync(playlist);

            // Compobar que el autor no este ya en la playlist
            if (users is null || users.Any(u => u.NoControl == user.NoControl)) return false;

            var connection = GetConnection();

            await connection.OpenAsync();

            using (var command = connection.CreateCommand()) {

                command.CommandText = "Insert Into Crea (Playlist_Codigo, Usuario_NoControl)\n";
                command.CommandText += "Values (@playlistId, @userId);";

                command.Parameters.Add("@playlistId", MySqlDbType.UInt32).Value = playlist.Id;
                command.Parameters.Add("@userId", MySqlDbType.VarChar).Value = user.NoControl;

                await command.ExecuteNonQueryAsync();
            }

            await connection.CloseAsync();

            return true;

        }

        public bool AttatchSong(AlmacenModel? almacen, PlaylistModel? playlist) {

            if (playlist is null || almacen is null) return false;

            // Revisar si existe la playlist y la cancion en la base de datos

            if (GetById(playlist.Id) is null) return false;

            if (App.AlmacenRepository.GetByIdAsync(almacen.Id) is null) return false;

            var almacenes = App.AlmacenRepository.GetFrom(playlist);

            // Compobar que la cancion no este ya en la playlist
            if (almacenes is null || almacenes.Any(a => a.Id == almacen.Id)) return false;

            var connection = GetConnection();

            connection.Open();

            using (var command = connection.CreateCommand()) {

                command.CommandText = "Insert Into Agrega (Playlist_Codigo, Almacena_Codigo)\n";
                command.CommandText += "Values (@playlistId, @almacenId);";

                command.Parameters.Add("@playlistId", MySqlDbType.UInt32).Value = playlist.Id;
                command.Parameters.Add("@almacenId", MySqlDbType.UInt32).Value = almacen.Id;

                command.ExecuteNonQuery();
            }

            connection.Close();

            return true;

        }

        public async Task<bool> AttatchSongAsync(AlmacenModel? almacen, PlaylistModel? playlist) {

            if (playlist is null || almacen is null) return false;

            // Revisar si existe la playlist y la cancion en la base de datos

            if ((await GetByIdAsync(playlist.Id)) is null) return false;

            if ((await App.AlmacenRepository.GetByIdAsync(almacen.Id)) is null) return false;

            var almacenes = await App.AlmacenRepository.GetFromAsync(playlist);

            // Compobar que la cancion no este ya en la playlist
            if (almacenes is null || almacenes.Any(a => a.Id == almacen.Id)) return false;

            var connection = GetConnection();

            await connection.OpenAsync();

            using (var command = connection.CreateCommand()) {

                command.CommandText = "Insert Into Agrega (Playlist_Codigo, Almacena_Codigo)\n";
                command.CommandText += "Values (@playlistId, @almacenId);";

                command.Parameters.Add("@playlistId", MySqlDbType.UInt32).Value = playlist.Id;
                command.Parameters.Add("@almacenId", MySqlDbType.UInt32).Value = almacen.Id;

                await command.ExecuteNonQueryAsync();
            }

            await connection.CloseAsync();

            return true;

        }

        public bool Edit(PlaylistModel? playlist) {
            throw new NotImplementedException();
        }

        public Task<bool> EditAsync(PlaylistModel? obj) {
            throw new NotImplementedException();
        }

        public IEnumerable<PlaylistModel> GetByAll() {

            List<PlaylistModel> allPlaylists = new();

            var connection = GetConnection();

            connection.Open();

            using (var command = connection.CreateCommand()) {

                command.CommandText = "Select * From PlayList;";

                using var reader = command.ExecuteReader();

                while (reader.Read()) {
                    allPlaylists.Add(new PlaylistModel(reader));
                }

            }

            connection.Close();

            return allPlaylists;
        }

        public async Task<IEnumerable<PlaylistModel>> GetByAllAsync() {

            List<PlaylistModel> allPlaylists = new();

            var connection = GetConnection();

            await connection.OpenAsync();

            using (var command = connection.CreateCommand()) {

                command.CommandText = "Select * From PlayList;";

                using var reader = await command.ExecuteReaderAsync();

                while (await reader.ReadAsync()) {
                    allPlaylists.Add(new PlaylistModel(reader));
                }

            }

            await connection.CloseAsync();

            // Obtener las canciones de la playlist
            // Obtener los usuario de la playlist
            /*

            foreach (PlaylistModel playlist in allPlaylists) {

                var songsTask = GetSongs(playlist);
                var usersTask = GetUsers(playlist);

                await Task.WhenAll(songsTask, usersTask);

                var songs = playlist.Songs.AddRangeAsync(songsTask.Result);
                var users = playlist.Authors.AddRangeAsync(usersTask.Result);

                await Task.WhenAll(songs, users);

            }
            */

            return allPlaylists;

        }

        public PlaylistModel? GetById(object? id) {

            if (id is not uint playlistId) return null;

            PlaylistModel? playlist = null;

            var connection = GetConnection();

            connection.Open();

            using (var command = connection.CreateCommand()) {

                command.CommandText = "Select * From PlayList Where Playlist_Codigo = @playlistId;";

                command.Parameters.Add("@playlistId", MySqlDbType.Int32).Value = playlistId;

                using var reader = command.ExecuteReader();

                if (reader.Read()) playlist = new PlaylistModel(reader);

            }

            connection.Close();

            return playlist;

        }

        public async Task<PlaylistModel?> GetByIdAsync(object? id) {

            if (id is not uint playlistId) return null;

            PlaylistModel? playlist = null;

            var connection = GetConnection();

            await connection.OpenAsync();

            using (var command = connection.CreateCommand()) {

                command.CommandText = "Select * From PlayList Where Playlist_Codigo = @playlistId;";

                command.Parameters.Add("@playlistId", MySqlDbType.Int32).Value = playlistId;

                using var reader = await command.ExecuteReaderAsync();

                if (await reader.ReadAsync()) playlist = new PlaylistModel(reader);

            }

            await connection.CloseAsync();

            return playlist;

        }

        public IEnumerable<PlaylistModel>? GetFrom(UserModel? user) {

            if (user is null) return null;

            var playlists = new List<PlaylistModel?>();

            var ids = new List<uint>();

            var connection = GetConnection();

            connection.Open();

            using (var command = connection.CreateCommand()) {

                command.CommandText = "Select Playlist_Codigo From Crea Where Usuario_NoControl = @noControl;";

                command.Parameters.Add("@noControl", MySqlDbType.VarChar).Value = user.NoControl;

                using var reader = command.ExecuteReader();

                while (reader.Read()) {
                    ids.Add(Convert.ToUInt32(reader["Playlist_Codigo"]));
                }

            }

            connection.Close();


            foreach (var id in ids) {
                playlists.Add(GetById(id));
            }

            return from playlist in playlists
                   where playlist is not null
                   select playlist;

        }

        public async Task<IEnumerable<PlaylistModel>?> GetFromAsync(UserModel? user) {

            if (user is null) return null;

            var tasks = new List<Task<PlaylistModel?>>();

            var ids = new List<uint>();

            var connection = GetConnection();

            await connection.OpenAsync();

            using (var command = connection.CreateCommand()) {

                command.CommandText = "Select Playlist_Codigo From Crea Where Usuario_NoControl = @noControl;";

                command.Parameters.Add("@noControl", MySqlDbType.VarChar).Value = user.NoControl;

                using var reader = await command.ExecuteReaderAsync();

                while (await reader.ReadAsync()) {

                    ids.Add(Convert.ToUInt32(reader["Playlist_Codigo"]));
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

        public bool RemoveById(object? id) {
            throw new NotImplementedException();
        }

        public Task<bool> RemoveByIdAsync(object? id) {
            throw new NotImplementedException();
        }
    }
}
