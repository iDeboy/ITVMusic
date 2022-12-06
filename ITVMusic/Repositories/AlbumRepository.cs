using ITVMusic.Models;
using ITVMusic.Repositories.Bases;
using ITVMusic.Util;
using ITVMusic.Views;
using Microsoft.VisualBasic.ApplicationServices;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ITVMusic.Repositories {
    public class AlbumRepository : RepositoryBase, IAlbumRepository {

        // private readonly IAlmacenRepository almacenRepository;
        /*
        public AlbumRepository()
            : this(App.AlmacenRepository) {
        }

        public AlbumRepository(IAlmacenRepository almacenRepository) {
            this.almacenRepository = almacenRepository;
        }
        */
        public async Task<bool> Add(AlbumModel? album) {

            if (album is null) return false;

            using (var connection = GetConnection()) {

                using var command = new MySqlCommand();

                await connection.OpenAsync();

                command.Connection = connection;

                command.CommandText = "Insert Into Album (Album_Titulo, Album_Lanzamiento, Album_Icono)\n";
                command.CommandText += "Values (@titulo, @lanzamiento, @icono);";

                command.Parameters.Add("@titulo", MySqlDbType.TinyText).Value = album.Title;
                command.Parameters.Add("@lanzamiento", MySqlDbType.Date).Value = album.RealeseDate;
                command.Parameters.Add("@icono", MySqlDbType.MediumBlob).Value = await album.Icon.ToByteArray();

                command.ExecuteNonQuery();
            }

            return true;

        }

        public Task<bool> Edit(AlbumModel? album) {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<AlbumModel>> GetByAll() {

            List<AlbumModel> allAlbums = new();

            using (var connection = GetConnection()) {

                using var commmand = new MySqlCommand();

                await connection.OpenAsync();
                commmand.Connection = connection;
                commmand.CommandText = "Select * From Album;";

                using var reader = await commmand.ExecuteReaderAsync();

                while (await reader.ReadAsync()) {

                    allAlbums.Add(new AlbumModel(reader));

                }

            }

            // Obtener las canciones del album

            foreach (AlbumModel album in allAlbums) {
                var songs = await GetSongs(album);

                album.Songs.AddRange(songs);
            }

            return allAlbums;
        }

        public async Task<AlbumModel?> GetById(object? id) {

            if (id is not uint albumId) return null;

            AlbumModel? album = null;

            using (var connection = GetConnection()) {

                using var commmand = new MySqlCommand();

                await connection.OpenAsync();

                commmand.Connection = connection;

                commmand.CommandText = "Select * From Album Where Album_Codigo = @albumId;";

                commmand.Parameters.Add("@albumId", MySqlDbType.UInt32).Value = albumId;

                using var reader = await commmand.ExecuteReaderAsync();

                if (await reader.ReadAsync()) {

                    album = new AlbumModel(reader);

                }

            }

            return album;
        }

        public async Task<IEnumerable<AlmacenModel>?> GetSongs(AlbumModel? album) {

            if (album is null) return null;

            var songs = new List<AlmacenModel>();

            var songTasks = new List<Task<AlmacenModel?>>();

            using (var connection = GetConnection()) {

                using var command = new MySqlCommand();

                await connection.OpenAsync();

                command.Connection = connection;

                command.CommandText = "Select Cancion_Codigo From Almacena Where Album_Codigo = @albumId Order By Fecha Desc;";

                command.Parameters.Add("@albumId", MySqlDbType.Int32).Value = album.Id;

                using var reader = command.ExecuteReader();

                while (reader.Read()) {

                    var songId = Convert.ToUInt32(reader["Cancion_Codigo"]);

                    songTasks.Add(App.AlmacenRepository.GetById(songId));

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
