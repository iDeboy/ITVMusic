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
    public class SongRepository : RepositoryBase, ISongRepository {

        public bool Add(SongModel? song) {

            if (song is null) return false;

            var connection = GetConnection();

            connection.Open();

            using (var command = connection.CreateCommand()) {

                command.CommandText = "Insert Into Cancion (Cancion_Titulo, Cancion_Genero, Cancion_Bytes, Cancion_Duracion, Cancion_Icono)\n";
                command.CommandText += "Values (@titulo, @genero, @bytes, @duracion, @icono);";

                command.Parameters.Add("@titulo", MySqlDbType.TinyText).Value = song.Title;
                command.Parameters.Add("@genero", MySqlDbType.Set).Value = song.Genders;
                command.Parameters.Add("@bytes", MySqlDbType.MediumBlob).Value = song.Bytes;
                command.Parameters.Add("@duracion", MySqlDbType.Time).Value = song.Duration.ToTimeOnly();
                command.Parameters.Add("@icono", MySqlDbType.MediumBlob).Value = song.Icon.ToByteArray();

                command.ExecuteNonQuery();
            }

            connection.Close();

            return true;

        }

        public async Task<bool> AddAsync(SongModel? song) {

            if (song is null) return false;

            var connection = GetConnection();

            await connection.OpenAsync();

            using (var command = connection.CreateCommand()) {

                command.CommandText = "Insert Into Cancion (Cancion_Titulo, Cancion_Genero, Cancion_Bytes, Cancion_Duracion, Cancion_Icono)\n";
                command.CommandText += "Values (@titulo, @genero, @bytes, @duracion, @icono);";

                command.Parameters.Add("@titulo", MySqlDbType.TinyText).Value = song.Title;
                command.Parameters.Add("@genero", MySqlDbType.Set).Value = song.Genders;
                command.Parameters.Add("@bytes", MySqlDbType.MediumBlob).Value = song.Bytes;
                command.Parameters.Add("@duracion", MySqlDbType.Time).Value = song.Duration.ToTimeOnly();
                command.Parameters.Add("@icono", MySqlDbType.MediumBlob).Value = await song.Icon.ToByteArrayAsync();

                await command.ExecuteNonQueryAsync();
            }

            await connection.CloseAsync();

            return true;

        }

        public bool AttatchArtist(SongModel? song, ArtistModel? artist) {

            if (song is null || artist is null) return false;

            // Comprobar si la cancion esta en la base de datos
            if (GetById(song.Id) is null) return false;

            // Comprobar si el artista esta en la base de datos
            if (App.ArtistRepository.GetById(artist.Id) is null) return false;

            // Comprobar que el artista no este ya en la cancion
            var artists = App.ArtistRepository.GetFrom(song);

            if (artists is null || artists.Any(a => a.Id == artist.Id)) return false;

            var connection = GetConnection();

            connection.Open();

            using (var command = connection.CreateCommand()) {

                command.CommandText = "Insert Into Canta (Cancion_Codigo, Artista_Codigo)\n";
                command.CommandText += "Value(@songId, @artistId);";

                command.Parameters.Add("@songId", MySqlDbType.UInt32).Value = song.Id;
                command.Parameters.Add("@artistId", MySqlDbType.UInt32).Value = artist.Id;

                command.ExecuteNonQuery();
            }

            connection.Close();

            return true;

        }

        public async Task<bool> AttatchArtistAsync(SongModel? song, ArtistModel? artist) {

            if (song is null || artist is null) return false;

            // Comprobar si la cancion esta en la base de datos
            if ((await GetByIdAsync(song.Id)) is null) return false;

            // Comprobar si el artista esta en la base de datos
            if ((await App.ArtistRepository.GetByIdAsync(artist.Id)) is null) return false;

            // Comprobar que el artista no este ya en la cancion
            var artists = await App.ArtistRepository.GetFromAsync(song);

            if (artists is null || artists.Any(a => a.Id == artist.Id)) return false;

            var connection = GetConnection();

            await connection.OpenAsync();

            using (var command = connection.CreateCommand()) {

                command.CommandText = "Insert Into Canta (Cancion_Codigo, Artista_Codigo)\n";
                command.CommandText += "Value(@songId, @artistId);";

                command.Parameters.Add("@songId", MySqlDbType.UInt32).Value = song.Id;
                command.Parameters.Add("@artistId", MySqlDbType.UInt32).Value = artist.Id;

                await command.ExecuteNonQueryAsync();
            }

            await connection.CloseAsync();

            return true;

        }

        public bool Edit(SongModel? song) {
            throw new NotImplementedException();
        }

        public Task<bool> EditAsync(SongModel? obj) {
            throw new NotImplementedException();
        }

        public IEnumerable<SongModel> GetByAll() {

            List<SongModel> allSongs = new();

            var connection = GetConnection();

            connection.Open();

            using (var command = connection.CreateCommand()) {

                command.CommandText = "Select * From Cancion;";

                using var reader = command.ExecuteReader();

                while (reader.Read()) {
                    allSongs.Add(new SongModel(reader));
                }

            }

            connection.Close();

            // Obtener los artistas de la cancion
            /*
            foreach (SongModel song in allSongs) {

                var artists = await GetArtists(song);

                song.Artists.AddRange(artists);
            }
            */
            return allSongs;

        }

        public async Task<IEnumerable<SongModel>> GetByAllAsync() {

            List<SongModel> allSongs = new();

            var connection = GetConnection();

            await connection.OpenAsync();

            using (var command = connection.CreateCommand()) {

                command.CommandText = "Select * From Cancion;";

                using var reader = await command.ExecuteReaderAsync();

                while (await reader.ReadAsync()) {
                    allSongs.Add(new SongModel(reader));
                }

            }

            await connection.CloseAsync();

            // Obtener los artistas de la cancion
            /*
            foreach (SongModel song in allSongs) {

                var artists = await GetArtists(song);

                song.Artists.AddRange(artists);
            }
            */
            return allSongs;

        }

        public SongModel? GetById(object? id) {

            if (id is not uint songId) return null;

            SongModel? song = null;

            var connection = GetConnection();

            connection.Open();

            using (var command = connection.CreateCommand()) {

                command.CommandText = "Select * From Cancion Where Cancion_Codigo = @songId;";

                command.Parameters.Add("@songId", MySqlDbType.UInt32).Value = songId;

                using var reader = command.ExecuteReader();

                if (reader.Read()) song = new SongModel(reader);

            }

            connection.Close();
            /*
            if (song is null) return null;

            var artists = await GetArtists(song);

            song.Artists.AddRange(artists);
            */
            return song;

        }

        public async Task<SongModel?> GetByIdAsync(object? id) {

            if (id is not uint songId) return null;

            SongModel? song = null;

            var connection = GetConnection();

            await connection.OpenAsync();

            using (var command = connection.CreateCommand()) {

                command.CommandText = "Select * From Cancion Where Cancion_Codigo = @songId;";

                command.Parameters.Add("@songId", MySqlDbType.UInt32).Value = songId;

                using var reader = await command.ExecuteReaderAsync();

                if (await reader.ReadAsync()) song = new SongModel(reader);

            }

            await connection.CloseAsync();
            /*
            if (song is null) return null;

            var artists = await GetArtists(song);

            song.Artists.AddRange(artists);
            */
            return song;

        }

        public IEnumerable<SongModel>? GetFrom(ArtistModel? artist) {

            if (artist is null) return null;

            var ids = new List<uint>();
            var songs = new List<SongModel?>();

            var connection = GetConnection();

            connection.Open();

            using (var command = connection.CreateCommand()) {

                command.CommandText = "Select Cancion_Codigo From Canta Where Artista_Codigo = @artistId";

                command.Parameters.Add("@artistId", MySqlDbType.UInt32).Value = artist.Id;

                using var reader = command.ExecuteReader();

                while (reader.Read()) {
                    ids.Add(Convert.ToUInt32(reader["Cancion_Codigo"]));
                }

            }

            connection.Close();

            foreach (var id in ids) {
                songs.Add(GetById(id));
            }

            return from song in songs
                   where song is not null
                   select song;

        }

        public SongModel? GetFrom(AlmacenModel? almacen) {

            if (almacen is null) return null;

            uint songId = 0;

            var connection = GetConnection();

            connection.Open();

            using (var command = connection.CreateCommand()) {

                command.CommandText = "Select Cancion_Codigo From Almacena Where Almacena_Codigo = @almacenId;";

                command.Parameters.Add("@almacenId", MySqlDbType.VarChar).Value = almacen.Id;

                using var reader = command.ExecuteReader();

                if (reader.Read()) {
                    songId = Convert.ToUInt32(reader["Cancion_Codigo"]);
                }

            }

            connection.Close();

            return GetById(songId);

        }

        public async Task<IEnumerable<SongModel>?> GetFromAsync(ArtistModel? artist) {

            if (artist is null) return null;

            var ids = new List<uint>();
            var tasks = new List<Task<SongModel?>>();

            var connection = GetConnection();

            await connection.OpenAsync();

            using (var command = connection.CreateCommand()) {

                command.CommandText = "Select Cancion_Codigo From Canta Where Artista_Codigo = @artistId";

                command.Parameters.Add("@artistId", MySqlDbType.UInt32).Value = artist.Id;

                using var reader = await command.ExecuteReaderAsync();

                while (await reader.ReadAsync()) {
                    ids.Add(Convert.ToUInt32(reader["Cancion_Codigo"]));
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

        public async Task<SongModel?> GetFromAsync(AlmacenModel? almacen) {

            if (almacen is null) return null;

            uint songId = 0;

            var connection = GetConnection();

            await connection.OpenAsync();

            using (var command = connection.CreateCommand()) {

                command.CommandText = "Select Cancion_Codigo From Almacena Where Almacena_Codigo = @almacenId;";

                command.Parameters.Add("@almacenId", MySqlDbType.VarChar).Value = almacen.Id;

                using var reader = await command.ExecuteReaderAsync();

                if (await reader.ReadAsync()) {
                    songId = Convert.ToUInt32(reader["Cancion_Codigo"]);
                }

            }

            await connection.CloseAsync();

            return await GetByIdAsync(songId);

        }

        public bool RemoveById(object? id) {
            throw new NotImplementedException();
        }

        public Task<bool> RemoveByIdAsync(object? id) {
            throw new NotImplementedException();
        }

    }
}
