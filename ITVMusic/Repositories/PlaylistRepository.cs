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

        //private readonly IUserRepository userRepository;
        //private readonly IAlmacenRepository almacenRepository;

        /*
        public PlaylistRepository()
            : this(App.UserRepository, App.AlmacenRepository) {

        }

        public PlaylistRepository(IUserRepository userRepository)
            : this(userRepository, new AlmacenRepository()) {

        }
        public PlaylistRepository(IAlmacenRepository almacenRepository)
            : this(new UserRepository(), almacenRepository) {

        }

        public PlaylistRepository(IUserRepository userRepository, IAlmacenRepository almacenRepository) {
            this.userRepository = userRepository;
            this.almacenRepository = almacenRepository;
        }
        */
        public async Task<bool> Add(PlaylistModel? playlist) {

            if (playlist is null) return false;

            using (var connection = GetConnection()) {

                using var command = new MySqlCommand();

                await connection.OpenAsync();

                command.Connection = connection;

                command.CommandText = "Insert Into PlayList (Playlist_Titulo, Playlist_Icono)\n";
                command.CommandText += "Values (@titulo, @icono);";

                command.Parameters.Add("@titulo", MySqlDbType.TinyText).Value = playlist.Title;
                command.Parameters.Add("@icono", MySqlDbType.MediumBlob).Value = await playlist.Icon.ToByteArray();

                command.ExecuteNonQuery();

            }

            return true;
        }

        public async Task<bool> AttatchAuthor(UserModel? user, PlaylistModel? playlist) {

            if (playlist is null || user is null) return false;

            // Revisar si existe la playlist y el usuario en la base de datos
            var allPlaylists = GetByAll();
            var allUsers = App.UserRepository.GetByAll();

            await Task.WhenAll(allPlaylists, allUsers);

            if (!allPlaylists.Result.Any(p => p.Id == playlist.Id)) return false;
            if (!allUsers.Result.Any(u => u.NoControl == user.NoControl)) return false;

            // Compobar que el autor no este ya en la playlist
            if (allPlaylists.Result.Any(p => p.Id == playlist.Id && p.Authors.Any(u => u.NoControl == user.NoControl))) return false;

            using (var connection = GetConnection()) {

                using var command = new MySqlCommand();

                await connection.OpenAsync();

                command.Connection = connection;

                command.CommandText = "Insert Into Crea (Playlist_Codigo, Usuario_NoControl)\n";
                command.CommandText += "Values (@playlistId, @userId);";

                command.Parameters.Add("@playlistId", MySqlDbType.UInt32).Value = playlist.Id;
                command.Parameters.Add("@userId", MySqlDbType.VarChar).Value = user.NoControl;

                command.ExecuteNonQuery();
            }

            return true;
        }

        public async Task<bool> AttatchSong(AlmacenModel? song, PlaylistModel? playlist) {

            if (playlist is null || song is null) return false;

            // Revisar si existe la playlist y la cancion en la base de datos
            var allPlaylists = GetByAll();
            var allSongs = App.AlmacenRepository.GetByAll();

            await Task.WhenAll(allPlaylists, allSongs);

            if (!allPlaylists.Result.Any(p => p.Id == playlist.Id)) return false;
            if (!allSongs.Result.Any(u => u.Id == song.Id)) return false;

            // Compobar que la cancion no este ya en la playlist
            if (allPlaylists.Result.Any(p => p.Id == playlist.Id && p.Songs.Any(s => s.Id == song.Id))) return false;

            using (var connection = GetConnection()) {

                using var command = new MySqlCommand();

                await connection.OpenAsync();

                command.Connection = connection;

                command.CommandText = "Insert Into Agrega (Playlist_Codigo, Almacena_Codigo)\n";
                command.CommandText += "Values (@playlistId, @almacenId);";

                command.Parameters.Add("@playlistId", MySqlDbType.UInt32).Value = playlist.Id;
                command.Parameters.Add("@almacenId", MySqlDbType.UInt32).Value = song.Id;

                command.ExecuteNonQuery();
            }

            return true;

        }

        public Task<bool> Edit(PlaylistModel? playlist) {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<PlaylistModel>> GetByAll() {

            List<PlaylistModel> allPlaylists = new();

            using (var connection = GetConnection()) {

                using var command = new MySqlCommand();

                await connection.OpenAsync();
                command.Connection = connection;
                command.CommandText = "Select * From PlayList;";

                using var reader = await command.ExecuteReaderAsync();

                while (await reader.ReadAsync()) {
                    allPlaylists.Add(new PlaylistModel(reader));
                }

            }

            // Obtener las canciones de la playlist
            // Obtener los usuario de la playlist


            foreach (PlaylistModel playlist in allPlaylists) {

                var songsTask = GetSongs(playlist);
                var usersTask = GetUsers(playlist);

                await Task.WhenAll(songsTask, usersTask);

                var songs = playlist.Songs.AddRangeAsync(songsTask.Result);
                var users = playlist.Authors.AddRangeAsync(usersTask.Result);

                await Task.WhenAll(songs, users);

            }


            return allPlaylists;
        }

        public async Task<PlaylistModel?> GetById(object? id) {

            if (id is not uint playlistId) return null;

            PlaylistModel? playlist = null;

            using (var connection = GetConnection()) {

                using var command = new MySqlCommand();

                await connection.OpenAsync();

                command.Connection = connection;

                command.CommandText = "Select * From PlayList Where Playlist_Codigo = @playlistId;";

                command.Parameters.Add("@playlistId", MySqlDbType.Int32).Value = playlistId;

                using var reader = command.ExecuteReader();

                if (reader.Read()) {
                    playlist = new PlaylistModel(reader);
                }

            }

            return playlist;
        }

        public async Task<IEnumerable<AlmacenModel>?> GetSongs(PlaylistModel? playlist) {

            if (playlist is null) return null;

            List<AlmacenModel> songs = new();
            List<Task<AlmacenModel?>> songsTasks = new();

            using (var connection = GetConnection()) {

                using var command = new MySqlCommand();

                await connection.OpenAsync();

                command.Connection = connection;

                command.CommandText = "Select Almacena_Codigo From Agrega Where Playlist_Codigo = @playlistId Order By Fecha Desc;";

                command.Parameters.Add("@playlistId", MySqlDbType.Int32).Value = playlist.Id;

                using var reader = command.ExecuteReader();

                while (reader.Read()) {

                    var almacenId = Convert.ToUInt32(reader["Almacena_Codigo"]);

                    songsTasks.Add(App.AlmacenRepository.GetById(almacenId));

                }

                await Task.WhenAll(songsTasks);

                foreach (var task in songsTasks) {

                    if (task.Result is not null) songs.Add(task.Result);

                }

            }

            return songs;

        }

        public async Task<IEnumerable<UserModel>?> GetUsers(PlaylistModel? playlist) {

            if (playlist is null) return null;

            List<UserModel> users = new();
            List<Task<UserModel?>> usersTasks = new();

            using (var connection = GetConnection()) {

                using var command = new MySqlCommand();

                await connection.OpenAsync();

                command.Connection = connection;

                command.CommandText = "Select Usuario_NoControl From Crea Where Playlist_Codigo = @playlistId Order By Fecha Asc;";

                command.Parameters.Add("@playlistId", MySqlDbType.Int32).Value = playlist.Id;

                using var reader = await command.ExecuteReaderAsync();

                while (await reader.ReadAsync()) {

                    var noControl = Convert.ToString(reader["Usuario_NoControl"]);

                    usersTasks.Add(App.UserRepository.GetById(noControl));

                }

                await Task.WhenAll(usersTasks);

                foreach (var task in usersTasks) {

                    if (task.Result is not null) users.Add(task.Result);

                }

            }

            return users;
        }

        public Task<bool> RemoveById(object? id) {
            throw new NotImplementedException();
        }
    }
}
