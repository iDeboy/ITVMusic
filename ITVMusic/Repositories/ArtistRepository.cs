using ITVMusic.Models;
using ITVMusic.Repositories.Bases;
using Microsoft.VisualBasic.ApplicationServices;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITVMusic.Repositories {
    public class ArtistRepository : RepositoryBase, IArtistRepository {

        private readonly ISongRepository songRepository;
        public ArtistRepository() {
            songRepository = new SongRepository();
        }

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
                command.CommandText = "Select * From Artista";

                using var reader = command.ExecuteReader();


                while (reader.Read()) {

                    var artist = new ArtistModel(reader);

                    var songs = new List<Task<SongModel?>>();

                    // Canciones que canta
                    using (var commandCanta = new MySqlCommand()) {

                        commandCanta.Connection = connection;

                        commandCanta.CommandText = "Select Cancion_Codigo From Canta Where Artista_Codigo = @artistId";

                        commandCanta.Parameters.Add("@artistId", MySqlDbType.UInt32).Value = artist.Id;

                        using var canta = commandCanta.ExecuteReader();

                        while (canta.Read()) {
                            var songId = Convert.ToUInt32(canta["Cancion_Codigo"]);

                            songs.Add(songRepository.GetById(songId));
                        }

                    }

                    await Task.WhenAll(songs);

                    foreach (var song in songs) {
                        if (song.Result is not null) artist.Songs.Add(song.Result);
                    }

                    allArtists.Add(artist);

                }

            }

            return allArtists;
        }

        public async Task<ArtistModel?> GetById(object id) {

            if (id is not uint artistId) return null;

            var all = await GetByAll();

            return (from it in all
                    where it.Id == artistId
                    select it).FirstOrDefault();

        }

        public Task<bool> RemoveById(object id) {
            throw new NotImplementedException();
        }
    }
}
