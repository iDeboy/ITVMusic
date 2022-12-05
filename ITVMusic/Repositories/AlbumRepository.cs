using ITVMusic.Models;
using ITVMusic.Repositories.Bases;
using ITVMusic.Util;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;
using System.Windows.Media;

namespace ITVMusic.Repositories {
    public class AlbumRepository : RepositoryBase, IAlbumRepository {

        private readonly IAlmacenRepository almacenRepository;

        public AlbumRepository() {
            almacenRepository = new AlmacenRepository();
        }

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
                commmand.CommandText = "Select * From Album";

                using var reader = commmand.ExecuteReader();

                while (reader.Read()) {

                    var album = new AlbumModel(reader);

                    var songs = new List<Task<AlmacenModel?>>();

                    // Obtener las canciones del album
                    using var commandAgrega = new MySqlCommand();

                    commandAgrega.Connection = connection;
                    commandAgrega.CommandText = "Select Cancion_Codigo From Almacena Where Album_Codigo = @albumId Order By Fecha Desc;";

                    commandAgrega.Parameters.Add("@albumId", MySqlDbType.Int32).Value = album.Id;

                    using var agrega = commandAgrega.ExecuteReader();

                    while (agrega.Read()) {

                        var almacenaId = Convert.ToInt32(agrega["Almacena_Codigo"]));

                        songs.Add(almacenRepository.GetById(almacenaId));
                    }

                    await Task.WhenAll(songs);

                    foreach (var song in songs) {
                        if (song.Result is not null) album.Songs.Add(song.Result);
                    }

                }

            }

            return allAlbums;

        }

        public async Task<AlbumModel?> GetById(object id) {

            if (id is not uint albumId) return null;

            var all = await GetByAll();

            return (from it in all
                    where it.Id == albumId
                    select it).FirstOrDefault();

        }

        public Task<bool> RemoveById(object id) {
            throw new NotImplementedException();
        }
    }
}
