using ITVMusic.Models;
using ITVMusic.Repositories.Bases;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ITVMusic.Repositories {
    public class ArtistRepository : RepositoryBase, IArtistRepository {

        public bool Add(ArtistModel? artist) {

            if (artist is null) return false;

            var connection = GetConnection();

            connection.Open();

            using (var command = connection.CreateCommand()) {

                command.CommandText = "Insert Into Artista (Artista_Nombre, Artista_Descripcion, Artista_Icono)\n";
                command.CommandText += "Values(@name, @description, @icon);";

                command.Parameters.Add("@name", MySqlDbType.VarChar).Value = artist.Name;
                command.Parameters.Add("@description", MySqlDbType.TinyText).Value = artist.Description;
                command.Parameters.Add("@icon", MySqlDbType.MediumBlob).Value = artist.Icon;

                command.ExecuteNonQuery();
            }

            connection.Close();

            return true;

        }

        public async Task<bool> AddAsync(ArtistModel? artist) {

            if (artist is null) return false;

            var connection = GetConnection();

            await connection.OpenAsync();

            using (var command = connection.CreateCommand()) {

                command.CommandText = "Insert Into Artista (Artista_Nombre, Artista_Descripcion, Artista_Icono)\n";
                command.CommandText += "Values(@name, @description, @icon);";

                command.Parameters.Add("@name", MySqlDbType.VarChar).Value = artist.Name;
                command.Parameters.Add("@description", MySqlDbType.TinyText).Value = artist.Description;
                command.Parameters.Add("@icon", MySqlDbType.MediumBlob).Value = artist.Icon;

                await command.ExecuteNonQueryAsync();
            }

            await connection.CloseAsync();

            return true;

        }

        public bool Edit(ArtistModel? artist) {
            throw new NotImplementedException();
        }

        public Task<bool> EditAsync(ArtistModel? artist) {
            throw new NotImplementedException();
        }

        public IEnumerable<ArtistModel> GetByAll() {

            var allArtists = new List<ArtistModel>();

            var connection = GetConnection();

            connection.Open();

            using (var command = connection.CreateCommand()) {

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

        public async Task<IEnumerable<ArtistModel>> GetByAllAsync() {

            var allArtists = new List<ArtistModel>();

            var connection = GetConnection();

            await connection.OpenAsync();

            using (var command = connection.CreateCommand()) {

                command.CommandText = "Select * From Artista;";

                using var reader = await command.ExecuteReaderAsync();

                while (await reader.ReadAsync()) {
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

        public ArtistModel? GetById(object? id) {

            if (id is not uint artistId) return null;

            ArtistModel? artist = null;

            var connection = GetConnection();

            connection.Open();

            using (var command = connection.CreateCommand()) {

                command.CommandText = "Select * From Artista Where Artista_Codigo = @artistId;";

                command.Parameters.Add("@artistId", MySqlDbType.UInt32).Value = artistId;

                using var reader = command.ExecuteReader();

                if (reader.Read()) artist = new ArtistModel(reader);

            }

            return artist;

        }

        public async Task<ArtistModel?> GetByIdAsync(object? id) {

            if (id is not uint artistId) return null;

            ArtistModel? artist = null;

            var connection = GetConnection();

            await connection.OpenAsync();

            using (var command = connection.CreateCommand()) {

                command.CommandText = "Select * From Artista Where Artista_Codigo = @artistId;";

                command.Parameters.Add("@artistId", MySqlDbType.UInt32).Value = artistId;

                using var reader = await command.ExecuteReaderAsync();

                if (await reader.ReadAsync()) artist = new ArtistModel(reader);

            }

            return artist;

        }

        public IEnumerable<ArtistModel>? GetFrom(SongModel? song) {

            if (song is null) return null;

            var ids = new List<uint>();

            var artists = new List<ArtistModel?>();

            var connection = GetConnection();

            connection.Open();

            using (var command = connection.CreateCommand()) {

                command.CommandText = "Select Artista_Codigo From Canta Where Cancion_Codigo = @songId;";

                command.Parameters.Add("@songId", MySqlDbType.UInt32).Value = song.Id;

                using var reader = command.ExecuteReader();

                while (reader.Read()) {
                    ids.Add(Convert.ToUInt32(reader["Artista_Codigo"]));
                }

            }

            connection.Close();

            foreach (var id in ids) {
                artists.Add(GetById(id));
            }

            return from artist in artists
                   where artist is not null
                   select artist;

        }

        public async Task<IEnumerable<ArtistModel>?> GetFromAsync(SongModel? song) {

            if (song is null) return null;

            var ids = new List<uint>();

            var tasks = new List<Task<ArtistModel?>>();

            var connection = GetConnection();

            await connection.OpenAsync();

            using (var command = connection.CreateCommand()) {

                command.CommandText = "Select Artista_Codigo From Canta Where Cancion_Codigo = @songId;";

                command.Parameters.Add("@songId", MySqlDbType.UInt32).Value = song.Id;

                using var reader = await command.ExecuteReaderAsync();

                while (await reader.ReadAsync()) {
                    ids.Add(Convert.ToUInt32(reader["Artista_Codigo"]));
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
