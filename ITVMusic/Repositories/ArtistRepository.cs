using ITVMusic.Models;
using ITVMusic.Repositories.Bases;
using ITVMusic.Util;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ITVMusic.Repositories {
    public class ArtistRepository : RepositoryBase, IArtistRepository {

        // private readonly ISongRepository songRepository;

        /*
        public ArtistRepository()
            : this(App.SongRepository) { }

        public ArtistRepository(ISongRepository songRepository) {
            this.songRepository = songRepository;
        }
        */
        public async Task<bool> Add(ArtistModel? artist) {

            if (artist is null) return false;

            using (var connection = GetConnection()) {

                using var command = new MySqlCommand();

                await connection.OpenAsync();

                command.Connection = connection;
                command.CommandText = "Insert Into Artista (Artista_Nombre, Artista_Descripcion, Artista_Icono)\n";
                command.CommandText += "Values(@name, @description, @icon);";

                command.Parameters.Add("@name", MySqlDbType.VarChar).Value = artist.Name;
                command.Parameters.Add("@description", MySqlDbType.TinyText).Value = artist.Description;
                command.Parameters.Add("@icon", MySqlDbType.MediumBlob).Value = artist.Icon;

                command.ExecuteNonQuery();
            }

            return true;
        }

        public Task<bool> Edit(ArtistModel? artist) {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<ArtistModel>> GetByAll() {

            var allArtists = new List<ArtistModel>();

            using (var connection = GetConnection()) {

                using var command = new MySqlCommand();

                await connection.OpenAsync();

                command.Connection = connection;
                command.CommandText = "Select * From Artista;";

                using var reader = command.ExecuteReader();

                while (reader.Read()) {
                    allArtists.Add(new ArtistModel(reader));
                }

            }

            // Obtener canciones que canta
            /*
            foreach (var artist in allArtists) {

                var songs = await GetSongs(artist);

                artist.Songs.AddRange(songs);
            }
            */
            return allArtists;
        }

        public async Task<ArtistModel?> GetById(object? id) {

            if (id is not uint artistId) return null;

            ArtistModel? artist = null;

            using (var connection = GetConnection()) {

                using var command = new MySqlCommand();

                await connection.OpenAsync();

                command.Connection = connection;

                command.CommandText = "Select * From Artista Where Artista_Codigo = @artistId;";

                command.Parameters.Add("@artistId", MySqlDbType.UInt32).Value = artistId;

                using var reader = command.ExecuteReader();

                if (reader.Read()) {
                    artist = new ArtistModel(reader);
                }

            }

            return artist;
        }

        public async Task<IEnumerable<SongModel>?> GetSongs(ArtistModel? artist) {

            if (artist is null) return null;

            List<SongModel> songs = new();
            List<Task<SongModel?>> songTasks = new();

            using (var connection = GetConnection()) {

                using var command = new MySqlCommand();

                await connection.OpenAsync();

                command.Connection = connection;

                command.CommandText = "Select Cancion_Codigo From Canta Where Artista_Codigo = @artistId";

                command.Parameters.Add("@artistId", MySqlDbType.UInt32).Value = artist.Id;

                using var reader = command.ExecuteReader();

                while (reader.Read()) {

                    var songId = Convert.ToUInt32(reader["Cancion_Codigo"]);

                    songTasks.Add(App.SongRepository.GetById(songId));

                }

                await Task.WhenAll(songTasks);

                foreach (var task in songTasks) {

                    if (task.Result is not null) songs.Add(task.Result);

                }

            }

            return songs;
        }

        public Task<bool> RemoveById(object? id) {
            throw new NotImplementedException();
        }
    }
}
