using ITVMusic.Models;
using ITVMusic.Repositories.Bases;
using ITVMusic.Util;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;

namespace ITVMusic.Repositories {
    public class SongRepository : RepositoryBase, ISongRepository {

        //private readonly IArtistRepository artistRepository;

        /*
        public SongRepository()
            : this(App.ArtistRepository) { }

        public SongRepository(IArtistRepository artistRepository) {
            this.artistRepository = artistRepository;
        }
        */
        public async Task<bool> Add(SongModel? song) {

            if (song is null) return false;

            using (var connection = GetConnection()) {

                using var command = new MySqlCommand();

                await connection.OpenAsync();

                command.Connection = connection;

                command.CommandText = "Insert Into Cancion (Cancion_Titulo, Cancion_Genero, Cancion_Bytes, Cancion_Duracion, Cancion_Icono)\n";
                command.CommandText += "Values (@titulo, @genero, @bytes, @duracion, @icono);";

                command.Parameters.Add("@titulo", MySqlDbType.TinyText).Value = song.Title;
                command.Parameters.Add("@genero", MySqlDbType.Set).Value = song.Genders;
                command.Parameters.Add("@bytes", MySqlDbType.MediumBlob).Value = song.Bytes;
                command.Parameters.Add("@duracion", MySqlDbType.Time).Value = song.Duration.ToTimeOnly();
                command.Parameters.Add("@icono", MySqlDbType.MediumBlob).Value = await song.Icon.ToByteArray();

                command.ExecuteNonQuery();
            }

            return true;

        }

        public async Task<bool> AttatchArtist(SongModel? song, ArtistModel? artist) {

            if (song is null || artist is null) return false;

            var allSongs = GetByAll();
            var allArtist = App.ArtistRepository.GetByAll();

            await Task.WhenAll(allSongs, allArtist);

            // Comprobar si la cancion esta en la base de datos
            if (!allSongs.Result.Any(s => s.Id == song.Id)) return false;

            // Comprobar si el artista esta en la base de datos
            if (!allArtist.Result.Any(a => a.Id == artist.Id)) return false;

            // Comprobar que el artista no este ya en la cancion
            if (allSongs.Result.Any(s => s.Id == song.Id && song.Artists.Any(a => a.Id == artist.Id))) return false;

            using (var connection = GetConnection()) {

                using var command = new MySqlCommand();

                await connection.OpenAsync();

                command.Connection = connection;

                command.CommandText = "Insert Into Canta (Cancion_Codigo, Artista_Codigo)\n";
                command.CommandText += "Value(@songId, @artistId);";

                command.Parameters.Add("@songId", MySqlDbType.UInt32).Value = song.Id;
                command.Parameters.Add("@artistId", MySqlDbType.UInt32).Value = artist.Id;

                command.ExecuteNonQuery();
            }

            return true;
        }

        public Task<bool> Edit(SongModel? song) {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<ArtistModel>?> GetArtists(SongModel? song) {

            if (song is null) return null;

            List<ArtistModel> artists = new();
            List<Task<ArtistModel?>> artistTasks = new();

            using (var connection = GetConnection()) {

                using var command = new MySqlCommand();

                await connection.OpenAsync();

                command.Connection = connection;

                command.CommandText = "Select Artista_Codigo From Canta Where Cancion_Codigo = @songId;";

                command.Parameters.Add("@songId", MySqlDbType.UInt32).Value = song.Id;

                using var reader = command.ExecuteReader();

                while (reader.Read()) {

                    var artistId = Convert.ToUInt32(reader["Artista_Codigo"]);

                    artistTasks.Add(App.ArtistRepository.GetById(artistId));

                }

                await Task.WhenAll(artistTasks);

                foreach (var task in artistTasks) {

                    if (task.Result is not null) artists.Add(task.Result);

                }

            }

            return artists;
        }

        public async Task<IEnumerable<SongModel>> GetByAll() {

            List<SongModel> allSongs = new();

            using (var connection = GetConnection()) {

                using var command = new MySqlCommand();

                await connection.OpenAsync();
                command.Connection = connection;
                command.CommandText = "Select * From Cancion;";

                using var reader = command.ExecuteReader();

                while (reader.Read()) {
                    allSongs.Add(new SongModel(reader));
                }

            }

            // Obtener los artistas de la cancion
            /*
            foreach (SongModel song in allSongs) {

                var artists = await GetArtists(song);

                song.Artists.AddRange(artists);
            }
            */
            return allSongs;
        }

        public async Task<SongModel?> GetById(object? id) {

            if (id is not uint songId) return null;

            SongModel? song = null;

            using (var connection = GetConnection()) {

                using var command = new MySqlCommand();

                await connection.OpenAsync();

                command.Connection = connection;

                command.CommandText = "Select * From Cancion Where Cancion_Codigo = @songId;";

                command.Parameters.Add("@songId", MySqlDbType.UInt32).Value = songId;

                using var reader = command.ExecuteReader();

                if (reader.Read()) {
                    song = new SongModel(reader);
                }

            }
            /*
            if (song is null) return null;

            var artists = await GetArtists(song);

            song.Artists.AddRange(artists);
            */
            return song;
        }

        public Task<bool> RemoveById(object? id) {
            throw new NotImplementedException();
        }
    }
}
