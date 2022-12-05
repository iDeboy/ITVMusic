using ITVMusic.Models;
using ITVMusic.Repositories.Bases;
using ITVMusic.Util;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITVMusic.Repositories {
    public class SongRepository : RepositoryBase, ISongRepository {

        private readonly IArtistRepository artistRepository;
        public SongRepository() {
            artistRepository = new ArtistRepository();
        }

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
            var allArtist = artistRepository.GetByAll();

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

        public async Task<IEnumerable<SongModel>> GetByAll() {

            List<SongModel> allSongs = new();

            using (var connection = GetConnection()) {

                using var command = new MySqlCommand();

                await connection.OpenAsync();
                command.Connection = connection;
                command.CommandText = "Select * From Cancion";

                using var reader = command.ExecuteReader();

                while (reader.Read()) {

                    var cancion = new SongModel(reader);

                    // Obtener los artistas de la cancion
                    var artistas = new List<Task<ArtistModel?>>();

                    using (var commandCanta = new MySqlCommand()) {

                        commandCanta.Connection = connection;

                        commandCanta.CommandText = "Select Artista_Codigo From Canta Where Cancion_Codigo = @songId";

                        commandCanta.Parameters.Add("@songId", MySqlDbType.UInt32).Value = cancion.Id;

                        using var canta = commandCanta.ExecuteReader();

                        while (canta.Read()) {

                            var artistId = Convert.ToUInt32(canta["Artista_Codigo"]);

                            artistas.Add(artistRepository.GetById(artistId));

                        }

                    }

                    await Task.WhenAll(artistas);

                    foreach (var artista in artistas) {
                        if (artista.Result is not null) cancion.Artists.Add(artista.Result);
                    }

                    allSongs.Add(cancion);
                }

            }

            return allSongs;

        }

        public async Task<SongModel?> GetById(object id) {

            if (id is not uint songId) return null;

            var all = await GetByAll();

            return (from it in all
                    where it.Id == songId
                    select it).FirstOrDefault();

        }

        public Task<bool> RemoveById(object id) {
            throw new NotImplementedException();
        }
    }
}
