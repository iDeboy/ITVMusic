using ITVMusic.Models;
using ITVMusic.Repositories.Bases;
using ITVMusic.Util;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ITVMusic.Repositories {
    public class AlbumRepository : RepositoryBase, IAlbumRepository {
        public bool Add(AlbumModel? album) {

            if (album is null) return false;

            var connection = GetConnection();

            connection.Open();

            using (var command = connection.CreateCommand()) {

                command.CommandText = "Insert Into Album (Album_Titulo, Album_Lanzamiento, Album_Icono)\n";
                command.CommandText += "Values (@titulo, @lanzamiento, @icono);";

                command.Parameters.Add("@titulo", MySqlDbType.TinyText).Value = album.Title;
                command.Parameters.Add("@lanzamiento", MySqlDbType.Date).Value = album.RealeseDate;
                command.Parameters.Add("@icono", MySqlDbType.MediumBlob).Value = album.Icon.ToByteArray();

                command.ExecuteNonQuery();
            }

            connection.Close();

            return true;

        }

        public async Task<bool> AddAsync(AlbumModel? album) {

            if (album is null) return false;

            var connection = GetConnection();

            await connection.OpenAsync();

            using (var command = connection.CreateCommand()) {


                command.CommandText = "Insert Into Album (Album_Titulo, Album_Lanzamiento, Album_Icono)\n";
                command.CommandText += "Values (@titulo, @lanzamiento, @icono);";

                command.Parameters.Add("@titulo", MySqlDbType.TinyText).Value = album.Title;
                command.Parameters.Add("@lanzamiento", MySqlDbType.Date).Value = album.RealeseDate;
                command.Parameters.Add("@icono", MySqlDbType.MediumBlob).Value = await album.Icon.ToByteArrayAsync();

                await command.ExecuteNonQueryAsync();
            }

            await connection.CloseAsync();

            return true;
        }

        public bool Edit(AlbumModel? obj) {
            throw new NotImplementedException();
        }

        public Task<bool> EditAsync(AlbumModel? album) {
            throw new NotImplementedException();
        }

        public IEnumerable<AlbumModel> GetByAll() {

            List<AlbumModel> allAlbums = new();

            var connection = GetConnection();

            connection.Open();

            using (var commmand = connection.CreateCommand()) {

                commmand.CommandText = "Select * From Album;";

                using var reader = commmand.ExecuteReader();

                while (reader.Read()) {

                    allAlbums.Add(new AlbumModel(reader));

                }

            }

            connection.Close();

            return allAlbums;

        }

        public async Task<IEnumerable<AlbumModel>> GetByAllAsync() {

            List<AlbumModel> allAlbums = new();

            var connection = GetConnection();

            await connection.OpenAsync();

            using (var commmand = connection.CreateCommand()) {

                commmand.CommandText = "Select * From Album;";

                using var reader = await commmand.ExecuteReaderAsync();

                while (await reader.ReadAsync()) {

                    allAlbums.Add(new AlbumModel(reader));

                }

            }

            await connection.CloseAsync();

            return allAlbums;
        }

        public AlbumModel? GetById(object? id) {

            if (id is not uint albumId) return null;

            AlbumModel? album = null;

            var connection = GetConnection();

            connection.Open();

            using (var commmand = connection.CreateCommand()) {

                commmand.CommandText = "Select * From Album Where Album_Codigo = @albumId;";

                commmand.Parameters.Add("@albumId", MySqlDbType.UInt32).Value = albumId;

                using var reader = commmand.ExecuteReader();

                if (reader.Read()) album = new AlbumModel(reader);

            }

            connection.Close();

            return album;
        }

        public async Task<AlbumModel?> GetByIdAsync(object? id) {

            if (id is not uint albumId) return null;

            AlbumModel? album = null;

            var connection = GetConnection();

            await connection.OpenAsync();

            using (var commmand = connection.CreateCommand()) {

                commmand.CommandText = "Select * From Album Where Album_Codigo = @albumId;";

                commmand.Parameters.Add("@albumId", MySqlDbType.UInt32).Value = albumId;

                using var reader = await commmand.ExecuteReaderAsync();

                if (await reader.ReadAsync()) album = new AlbumModel(reader);

            }

            await connection.CloseAsync();

            return album;
        }

        public AlbumModel? GetFrom(AlmacenModel? almacen) {

            if (almacen is null) return null;

            uint albumId = 0;

            var connection = GetConnection();

            connection.Open();

            using (var command = connection.CreateCommand()) {

                command.CommandText = "Select Album_Codigo From Almacena Where Almacena_Codigo = @almacenId;";

                command.Parameters.Add("@almacenId", MySqlDbType.VarChar).Value = almacen.Id;

                using var reader = command.ExecuteReader();

                if (reader.Read()) {
                    albumId = Convert.ToUInt32(reader["Album_Codigo"]);
                }

            }

            connection.Close();

            return GetById(albumId);

        }

        public async Task<AlbumModel?> GetFromAsync(AlmacenModel? almacen) {

            if (almacen is null) return null;

            uint albumId = 0;

            var connection = GetConnection();

            await connection.OpenAsync();

            using (var command = connection.CreateCommand()) {

                command.CommandText = "Select Album_Codigo From Almacena Where Almacena_Codigo = @almacenId;";

                command.Parameters.Add("@almacenId", MySqlDbType.VarChar).Value = almacen.Id;

                using var reader = await command.ExecuteReaderAsync();

                if (await reader.ReadAsync()) {
                    albumId = Convert.ToUInt32(reader["Album_Codigo"]);
                }

            }

            await connection.CloseAsync();

            return await GetByIdAsync(albumId);

        }

        public bool RemoveById(object? id) {
            throw new NotImplementedException();
        }

        public Task<bool> RemoveByIdAsync(object? id) {
            throw new NotImplementedException();
        }
    }
}
