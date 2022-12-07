using ITVMusic.Models;
using ITVMusic.Repositories.Bases;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Media;

namespace ITVMusic.Repositories {
    public class AlmacenRepository : RepositoryBase, IAlmacenRepository {

        public bool Add(AlmacenModel? almacen) {

            if (almacen is null) return false;

            if (almacen.Song is null || almacen.Album is null) return false;

            if (GetFrom(almacen.Song, almacen.Album) is not null) return false;

            if (App.AlbumRepository.GetById(almacen.Song.Id) is null) return false;

            if (App.SongRepository.GetById(almacen.Album.Id) is null) return false;

            var connection = GetConnection();

            connection.Open();

            using (var command = connection.CreateCommand()) {

                command.CommandText = "Insert Into Almacena (Album_Codigo, Cancion_Codigo)\n";
                command.CommandText += "Values (@albumId, @cancionId);";

                command.Parameters.Add("@albumId", MySqlDbType.UInt32).Value = almacen.Album.Id;
                command.Parameters.Add("@cancionId", MySqlDbType.UInt32).Value = almacen.Song.Id;

                command.ExecuteNonQuery();

            }

            connection.Close();

            return true;

        }

        public async Task<bool> AddAsync(AlmacenModel? almacen) {

            if (almacen is null) return false;

            if (almacen.Song is null || almacen.Album is null) return false;

            if ((await GetFromAsync(almacen.Song, almacen.Album)) is not null) return false;

            if ((await App.AlbumRepository.GetByIdAsync(almacen.Song.Id)) is null) return false;

            if ((await App.SongRepository.GetByIdAsync(almacen.Album.Id)) is null) return false;

            var connection = GetConnection();

            await connection.OpenAsync();

            using (var command = connection.CreateCommand()) {

                command.CommandText = "Insert Into Almacena (Album_Codigo, Cancion_Codigo)\n";
                command.CommandText += "Values (@albumId, @cancionId);";

                command.Parameters.Add("@albumId", MySqlDbType.UInt32).Value = almacen.Album.Id;
                command.Parameters.Add("@cancionId", MySqlDbType.UInt32).Value = almacen.Song.Id;

                await command.ExecuteNonQueryAsync();

            }

            await connection.CloseAsync();

            return true;

        }

        public bool Edit(AlmacenModel? almacen) {
            throw new NotImplementedException();
        }

        public Task<bool> EditAsync(AlmacenModel? almacen) {
            throw new NotImplementedException();
        }

        public IEnumerable<AlmacenModel> GetByAll() {

            List<AlmacenModel> allAlmacenes = new();

            var connection = GetConnection();

            connection.Open();

            using (var command = connection.CreateCommand()) {

                command.CommandText = "Select * From Almacena;\n";

                using var reader = command.ExecuteReader();

                while (reader.Read()) {
                    allAlmacenes.Add(new AlmacenModel(reader));
                }

            }

            connection.Close();

            foreach (var almacen in allAlmacenes) {
                almacen.Reproductions = GetReproductions(almacen);
            }

            return allAlmacenes;

        }

        public async Task<IEnumerable<AlmacenModel>> GetByAllAsync() {

            List<AlmacenModel> allAlmacenes = new();

            var connection = GetConnection();

            await connection.OpenAsync();

            using (var command = connection.CreateCommand()) {

                command.CommandText = "Select * From Almacena;\n";

                using var reader = await command.ExecuteReaderAsync();

                while (await reader.ReadAsync()) {
                    allAlmacenes.Add(new AlmacenModel(reader));
                }

            }

            await connection.CloseAsync();

            var repros = new List<Task<uint>>();

            foreach (var almacen in allAlmacenes) {
                repros.Add(GetReproductionsAsync(almacen));
            }

            await Task.WhenAll(repros);

            for (int i = 0; i < allAlmacenes.Count; i++) {
                allAlmacenes[i].Reproductions = repros[i].Result;
            }

            return allAlmacenes;

        }

        public AlmacenModel? GetById(object? id) {

            if (id is not uint almacenId) return null;

            AlmacenModel? almacen = null;

            var connection = GetConnection();

            connection.Open();

            using (var command = connection.CreateCommand()) {

                command.CommandText = "Select * From Almacena Where Almacena_Codigo = @almacenId;\n";

                command.Parameters.Add("@almacenId", MySqlDbType.UInt32).Value = almacenId;

                using var reader = command.ExecuteReader();

                if (reader.Read()) almacen = new AlmacenModel(reader);

            }

            connection.Close();

            if (almacen is not null)
                almacen.Reproductions = GetReproductions(almacen);

            return almacen;
        }

        public async Task<AlmacenModel?> GetByIdAsync(object? id) {

            if (id is not uint almacenId) return null;

            AlmacenModel? almacen = null;

            var connection = GetConnection();

            await connection.OpenAsync();

            using (var command = connection.CreateCommand()) {

                command.CommandText = "Select * From Almacena Where Almacena_Codigo = @almacenId;\n";

                command.Parameters.Add("@almacenId", MySqlDbType.UInt32).Value = almacenId;

                using var reader = await command.ExecuteReaderAsync();

                if (await reader.ReadAsync()) almacen = new AlmacenModel(reader);

            }

            await connection.CloseAsync();

            if (almacen is not null)
                almacen.Reproductions = await GetReproductionsAsync(almacen);

            return almacen;

        }

        public AlmacenModel? GetFrom(SongModel? song, AlbumModel? album) {

            if (song is null || album is null) return null;

            AlmacenModel? almacen = null;

            var connection = GetConnection();

            connection.Open();

            using (var command = connection.CreateCommand()) {

                command.CommandText = "Select * From Almacena Where Cancion_Codigo = @songId And Album_Codigo = @albumId;\n";

                command.Parameters.Add("@almacenId", MySqlDbType.UInt32).Value = song.Id;
                command.Parameters.Add("@albumId", MySqlDbType.UInt32).Value = album.Id;

                using var reader = command.ExecuteReader();

                if (reader.Read()) almacen = new AlmacenModel(reader);

            }

            connection.Close();

            return almacen;

        }

        public IEnumerable<AlmacenModel>? GetFrom(UserModel? user) {

            if (user is null) return null;

            var almacenes = new List<AlmacenModel?>();

            var ids = new List<uint>();

            var connection = GetConnection();

            connection.Open();

            using (var command = connection.CreateCommand()) {

                command.CommandText = "Select Almacena_Codigo From Escucha Where Usuario_NoControl = @noControl;";

                command.Parameters.Add("@noControl", MySqlDbType.VarChar).Value = user.NoControl;

                using var reader = command.ExecuteReader();

                while (reader.Read()) {
                    ids.Add(Convert.ToUInt32(reader["Almacena_Codigo"]));
                }

            }

            connection.Close();

            foreach (var id in ids) {
                almacenes.Add(GetById(id));
            }

            return from almacen in almacenes
                   where almacen is not null
                   select almacen;

        }

        public IEnumerable<AlmacenModel>? GetFrom(PlaylistModel? playlist) {

            if (playlist is null) return null;

            var ids = new List<uint>();

            List<AlmacenModel?> almacenes = new();

            var connection = GetConnection();

            connection.Open();

            using (var command = connection.CreateCommand()) {

                command.Connection = connection;

                command.CommandText = "Select Almacena_Codigo From Agrega Where Playlist_Codigo = @playlistId Order By Fecha Desc;";

                command.Parameters.Add("@playlistId", MySqlDbType.Int32).Value = playlist.Id;

                using var reader = command.ExecuteReader();

                while (reader.Read()) {
                    ids.Add(Convert.ToUInt32(reader["Almacena_Codigo"]));
                }

            }

            connection.Close();

            foreach (var id in ids) {
                almacenes.Add(GetById(id));
            }

            return from almacen in almacenes
                   where almacen is not null
                   select almacen;

        }

        public IEnumerable<AlmacenModel>? GetFrom(AlbumModel? album) {

            if (album is null) return null;

            var almacenes = new List<AlmacenModel?>();
            var ids = new List<uint>();

            var connection = GetConnection();

            connection.Open();

            using (var command = connection.CreateCommand()) {

                command.CommandText = "Select Cancion_Codigo From Almacena Where Album_Codigo = @albumId Order By Fecha Desc;";

                command.Parameters.Add("@albumId", MySqlDbType.Int32).Value = album.Id;

                using var reader = command.ExecuteReader();

                while (reader.Read()) {
                    ids.Add(Convert.ToUInt32(reader["Cancion_Codigo"]));
                }

            }

            connection.Close();

            foreach (var id in ids) {
                almacenes.Add(GetById(id));
            }

            return from almacen in almacenes
                   where almacen is not null
                   select almacen;

        }

        public async Task<AlmacenModel?> GetFromAsync(SongModel? song, AlbumModel? album) {

            if (song is null || album is null) return null;

            AlmacenModel? almacen = null;

            var connection = GetConnection();

            await connection.OpenAsync();

            using (var command = connection.CreateCommand()) {

                command.CommandText = "Select * From Almacena Where Cancion_Codigo = @songId And Album_Codigo = @albumId;\n";

                command.Parameters.Add("@almacenId", MySqlDbType.UInt32).Value = song.Id;
                command.Parameters.Add("@albumId", MySqlDbType.UInt32).Value = album.Id;

                using var reader = await command.ExecuteReaderAsync();

                if (await reader.ReadAsync()) almacen = new AlmacenModel(reader);

            }

            await connection.CloseAsync();

            return almacen;
        }

        public async Task<IEnumerable<AlmacenModel>?> GetFromAsync(UserModel? user) {

            if (user is null) return null;

            var tasks = new List<Task<AlmacenModel?>>();

            var ids = new List<uint>();

            var connection = GetConnection();

            await connection.OpenAsync();

            using (var command = connection.CreateCommand()) {

                command.CommandText = "Select Almacena_Codigo From Escucha Where Usuario_NoControl = @noControl;";

                command.Parameters.Add("@noControl", MySqlDbType.VarChar).Value = user.NoControl;

                using var reader = await command.ExecuteReaderAsync();

                while (await reader.ReadAsync()) {
                    ids.Add(Convert.ToUInt32(reader["Almacena_Codigo"]));
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

        public async Task<IEnumerable<AlmacenModel>?> GetFromAsync(PlaylistModel? playlist) {

            if (playlist is null) return null;

            var ids = new List<uint>();

            List<Task<AlmacenModel?>> tasks = new();

            var connection = GetConnection();

            await connection.OpenAsync();

            using (var command = connection.CreateCommand()) {

                command.Connection = connection;

                command.CommandText = "Select Almacena_Codigo From Agrega Where Playlist_Codigo = @playlistId Order By Fecha Desc;";

                command.Parameters.Add("@playlistId", MySqlDbType.Int32).Value = playlist.Id;

                using var reader = await command.ExecuteReaderAsync();

                while (await reader.ReadAsync()) {
                    ids.Add(Convert.ToUInt32(reader["Almacena_Codigo"]));
                }

            }

            await connection.CloseAsync();

            foreach (var id in ids) {
                tasks.Add(GetByIdAsync(id));
            }

            await Task.WhenAll(tasks);

            var almacenes = (from task in tasks
                             where task.Result is not null
                             select task.Result).ToList();

            var songs = (from almacen in almacenes
                         select App.SongRepository.GetFromAsync(almacen)).ToList();

            var albums = (from almacen in almacenes
                          select App.AlbumRepository.GetFromAsync(almacen)).ToList();

            await Task.WhenAll(songs);

            await Task.WhenAll(albums);

            for (int i = 0; i < almacenes.Count; i++) {
                almacenes[i].Song = songs[i].Result;
                almacenes[i].Album = albums[i].Result;
            }

            return almacenes;

        }

        public async Task<IEnumerable<AlmacenModel>?> GetFromAsync(AlbumModel? album) {

            if (album is null) return null;

            var tasks = new List<Task<AlmacenModel?>>();
            var ids = new List<uint>();

            var connection = GetConnection();

            await connection.OpenAsync();

            using (var command = connection.CreateCommand()) {

                command.CommandText = "Select Cancion_Codigo From Almacena Where Album_Codigo = @albumId Order By Fecha Desc;";

                command.Parameters.Add("@albumId", MySqlDbType.Int32).Value = album.Id;

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

            var almacenes = (from task in tasks
                             where task.Result is not null
                             select task.Result).ToList();

            var songs = (from almacen in almacenes
                         select App.SongRepository.GetFromAsync(almacen)).ToList();

            var albums = (from almacen in almacenes
                          select App.AlbumRepository.GetFromAsync(almacen)).ToList();

            await Task.WhenAll(songs);

            await Task.WhenAll(albums);

            for (int i = 0; i < almacenes.Count; i++) {
                almacenes[i].Song = songs[i].Result;
                almacenes[i].Album = albums[i].Result;
            }

            return almacenes;

        }

        public uint GetReproductions(AlmacenModel? almacen) {

            if (almacen is null) return 0;

            uint repros = 0;

            var connection = GetConnection();

            connection.Open();

            using (var command = connection.CreateCommand()) {

                command.CommandText = "Select CantidadEscuchada From Canciones Where Almacena_Codigo = @almacenId;";

                command.Parameters.Add("@almacenId", MySqlDbType.Int32).Value = almacen.Id;

                using var reader = command.ExecuteReader();

                if (reader.Read()) repros = Convert.ToUInt32(reader["CantidadEscuchada"]);

            }

            connection.Close();

            return repros;

        }

        public async Task<uint> GetReproductionsAsync(AlmacenModel? almacen) {

            if (almacen is null) return 0;

            uint repros = 0;

            var connection = GetConnection();

            await connection.OpenAsync();

            using (var command = connection.CreateCommand()) {

                command.CommandText = "Select CantidadEscuchada From Canciones Where Almacena_Codigo = @almacenId;";

                command.Parameters.Add("@almacenId", MySqlDbType.Int32).Value = almacen.Id;

                using var reader = await command.ExecuteReaderAsync();

                if (await reader.ReadAsync()) repros = Convert.ToUInt32(reader["CantidadEscuchada"]);

            }

            await connection.CloseAsync();

            return repros;

        }

        public bool RemoveById(object? id) {
            throw new NotImplementedException();
        }

        public Task<bool> RemoveByIdAsync(object? id) {
            throw new NotImplementedException();
        }

    }
}
