using ITVMusic.Models;
using ITVMusic.Repositories.Bases;
using ITVMusic.Util;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ITVMusic.Repositories {
    public class PlaylistRepository : RepositoryBase, IPlaylistRepository {

        private IUserRepository userRepository;
        private IAlmacenRepository almacenRepository;

        public PlaylistRepository() {
            userRepository = new UserRepository();
            almacenRepository = new AlmacenRepository();
        }

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
            var allUsers = userRepository.GetByAll();

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
            var allSongs = almacenRepository.GetByAll();

            await Task.WhenAll(allPlaylists, allSongs);

            if (!allPlaylists.Result.Any(p => p.Id == playlist.Id)) return false;
            if (!allSongs.Result.Any(u => u.Id == song.Id)) return false;

            // Compobar que la cancion no este ya en la playlist
            if (allPlaylists.Result.Any(p => p.Id == playlist.Id && p.Songs.Any(s => s.Id == almacen.Id))) return false;

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

                using var commandPlaylist = new MySqlCommand();

                await connection.OpenAsync();
                commandPlaylist.Connection = connection;
                commandPlaylist.CommandText = "Select * From PlayList";

                using var reader = commandPlaylist.ExecuteReader();

                while (reader.Read()) {

                    var playlist = new PlaylistModel(reader);

                    var authors = new List<Task<UserModel?>>();

                    var songs = new List<Task<AlmacenModel?>>();

                    // Obtener los autores de la playlist
                    using (var commandCrea = new MySqlCommand()) {

                        commandCrea.Connection = connection;
                        commandCrea.CommandText = "Select Usuario_NoControl From Crea Where Playlist_Codigo = @playlistId Order By Fecha Asc;";

                        commandCrea.Parameters.Add("@playlistId", MySqlDbType.Int32).Value = playlist.Id;

                        using var crea = commandCrea.ExecuteReader();

                        while (crea.Read()) {

                            authors.Add(userRepository.GetById(crea["Usuario_NoControl"]));

                        }

                    }

                    // Obtener las canciones de la playlist
                    using (var commandAgrega = new MySqlCommand()) {

                        commandAgrega.Connection = connection;
                        commandAgrega.CommandText = "Select Almacena_Codigo From Agrega Where Playlist_Codigo = @playlistId Order By Fecha Desc;";

                        commandAgrega.Parameters.Add("@playlistId", MySqlDbType.Int32).Value = playlist.Id;

                        using var agrega = commandAgrega.ExecuteReader();

                        //Cambiar por almacen 

                        while (agrega.Read()) {

                            songs.Add(almacenRepository.GetById(Convert.ToInt32(agrega["Almacena_Codigo"])));

                        }

                    }

                    await Task.WhenAll(authors);

                    foreach (var author in authors) {
                        if (author.Result is not null) playlist.Authors.Add(author.Result);
                    }

                    await Task.WhenAll(songs);

                    foreach (var song in songs) {
                        if (song.Result is not null) playlist.Songs.Add(song.Result);
                    }

                    allPlaylists.Add(playlist);

                }

            }

            return allPlaylists;
        }

        public async Task<PlaylistModel?> GetById(object? id) {

            if (id is not uint playlistId) return null;

            var all = await GetByAll();

            return (from it in all
                    where it.Id == playlistId
                    select it).FirstOrDefault();

        }

        public Task<bool> RemoveById(object? id) {
            throw new NotImplementedException();
        }
    }
}
