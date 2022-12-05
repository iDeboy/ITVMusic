using ITVMusic.Models;
using ITVMusic.Repositories.Bases;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace ITVMusic.Repositories {
    public class AlmacenRepository : RepositoryBase, IAlmacenRepository {

        private readonly ISongRepository songRepository;
        private readonly IAlbumRepository albumRepository;

        public AlmacenRepository() {

            albumRepository = new AlbumRepository();
            songRepository = new SongRepository();

        }

        public async Task<bool> Add(AlmacenModel? almacen) {

            if (almacen is null) return false;

            if (almacen.Song is null || almacen.Album is null) return false;

            var all = GetByAll();

            var allAlbums = albumRepository.GetByAll();
            var allSongs = songRepository.GetByAll();

            await Task.WhenAll(all, allSongs, allAlbums);

            if (all.Result.Any(al => al.Song?.Id == almacen.Song.Id && al.Album?.Id == almacen.Album.Id)) return false;

            if (!allAlbums.Result.Any(a => a.Id == almacen.Album.Id)) return false;

            if (!allSongs.Result.Any(s => s.Id == almacen.Song.Id)) return false;

            using (var connection = GetConnection()) {

                using var command = new MySqlCommand();

                await connection.OpenAsync();

                command.Connection = connection;

                command.CommandText = "Insert Into Almacena (Album_Codigo, Cancion_Codigo)\n";
                command.CommandText += "Values (@albumId, @cancionId);";

                command.Parameters.Add("@albumId", MySqlDbType.UInt32).Value = almacen.Album.Id;
                command.Parameters.Add("@cancionId", MySqlDbType.UInt32).Value = almacen.Song.Id;

                command.ExecuteNonQuery();

            }

            return true;
        }

        public Task<bool> Edit(AlmacenModel? almacen) {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<AlmacenModel>> GetByAll() {

            List<AlmacenModel> allAlmacenes = new();

            using (var connection = GetConnection()) {

                using var command = new MySqlCommand();

                await connection.OpenAsync();

                command.Connection = connection;

                command.CommandText = "Select * From Almacena";

                using var reader = command.ExecuteReader();

                while (reader.Read()) {

                    var albumId = Convert.ToUInt32(reader["Album_Codigo"]);
                    var cancionId = Convert.ToUInt32(reader["Cancion_Codigo"]);

                    var almacen = new AlmacenModel(reader);

                    // Obtener las reproducciones
                    using (var commandCanciones = new MySqlCommand()) {

                        commandCanciones.Connection = connection;
                        commandCanciones.CommandText = "Select CantidadEscuchada From Canciones Where Almacena_Codigo = @almacenId";

                        commandCanciones.Parameters.Add("@almacenId", MySqlDbType.Int32).Value = almacen.Id;

                        using var canciones = commandCanciones.ExecuteReader();

                        if (canciones.Read()) {
                            almacen.Reproductions = Convert.ToUInt32(canciones["CantidadEscuchada"]);
                        }

                    }

                    // Obtener el album
                    var album = albumRepository.GetById(albumId);

                    // Obtener la cancion
                    var song = songRepository.GetById(cancionId);

                    await Task.WhenAll(album, song);

                    if (album.Result is null || song.Result is null) continue;

                    almacen.Album = album.Result;
                    almacen.Song = song.Result;

                    allAlmacenes.Add(almacen);

                }

            }

            return allAlmacenes;

        }

        public async Task<AlmacenModel?> GetById(object id) {

            if (id is not uint almacenId) return null;

            var all = await GetByAll();

            return (from it in all
                    where it.Id == almacenId
                    select it).FirstOrDefault();

        }

        public Task<bool> RemoveById(object id) {
            throw new NotImplementedException();
        }
    }
}
