using ITVMusic.Models;
using ITVMusic.Repositories.Bases;
using MaterialDesignThemes.Wpf.Transitions;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace ITVMusic.Repositories {
    public class AlmacenRepository : RepositoryBase, IAlmacenRepository {

        // private readonly ISongRepository songRepository;
        // private readonly IAlbumRepository albumRepository;
        /*
        public AlmacenRepository()
            : this(App.SongRepository, App.AlbumRepository) {

        }

        public AlmacenRepository(ISongRepository songRepository)
            : this(songRepository, new AlbumRepository()) {

        }
        public AlmacenRepository(IAlbumRepository albumRepository)
            : this(new SongRepository(), albumRepository) {

        }

        public AlmacenRepository(ISongRepository songRepository, IAlbumRepository albumRepository) {
            this.songRepository = songRepository;
            this.albumRepository = albumRepository;
        }
        */
        public async Task<bool> Add(AlmacenModel? almacen) {

            if (almacen is null) return false;

            if (almacen.Song is null || almacen.Album is null) return false;

            var all = GetByAll();

            var allAlbums = App.AlbumRepository.GetByAll();
            var allSongs = App.SongRepository.GetByAll();

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

        public Task<AlbumModel?> GetAlbum(AlmacenModel? almacen) {

            //if (almacen is null) return null;

            throw new NotImplementedException();
            //return await App.AlbumRepository.GetById(almacen.AlbumId);
        }

        public async Task<IEnumerable<AlmacenModel>> GetByAll() {

            List<AlmacenModel> allAlmacenes = new();

            using (var connection = GetConnection()) {

                await connection.OpenAsync();

                using var command = new MySqlCommand();

                command.Connection = connection;

                command.CommandText = "Select Al.Fecha, Al.Almacena_Codigo, Ca.*, A.*, C.CantidadEscuchada\n";
                command.CommandText += "From Almacena As Al\n";
                command.CommandText += "Left join Canciones As C on Al.Almacena_Codigo = C.Almacena_Codigo\n";
                command.CommandText += "Inner join Cancion As Ca on Al.Cancion_Codigo = Ca.Cancion_Codigo\n";
                command.CommandText += "Inner join Album As A on Al.Album_Codigo = A.Album_Codigo;";

                using var reader = await command.ExecuteReaderAsync();

                while (await reader.ReadAsync()) {
                    allAlmacenes.Add(new AlmacenModel(reader));
                }

            }

            return allAlmacenes;

        }

        public async Task<AlmacenModel?> GetById(object? id) {

            if (id is not uint almacenId) return null;

            AlmacenModel? almacen = null;

            using (var connection = GetConnection()) {

                using var command = new MySqlCommand();

                await connection.OpenAsync();

                command.Connection = connection;

                command.CommandText = "Select Al.Fecha, Al.Almacena_Codigo, Ca.*, A.*, C.CantidadEscuchada\n";
                command.CommandText += "From Almacena As Al\n";
                command.CommandText += "Left join Canciones As C on Al.Almacena_Codigo = C.Almacena_Codigo\n";
                command.CommandText += "Inner join Cancion As Ca on Al.Cancion_Codigo = Ca.Cancion_Codigo\n";
                command.CommandText += "Inner join Album As A on Al.Album_Codigo = A.Album_Codigo\n";
                command.CommandText += "Where Al.Almacena_Codigo = @almacenId;";

                command.Parameters.Add("@almacenId", MySqlDbType.UInt32).Value = almacenId;

                using var reader = await command.ExecuteReaderAsync();

                if (await reader.ReadAsync()) {

                    almacen = new AlmacenModel(reader);

                }

            }

            return almacen;
        }

        public async Task<uint> GetReproductions(AlmacenModel? almacen) {

            if (almacen is null) return 0;

            uint repros = 0;

            using (var connection = GetConnection()) {

                using var command = new MySqlCommand();

                await connection.OpenAsync();

                command.Connection = connection;

                command.CommandText = "Select CantidadEscuchada From Canciones Where Almacena_Codigo = @almacenId;";

                command.Parameters.Add("@almacenId", MySqlDbType.Int32).Value = almacen.Id;

                using var reader = command.ExecuteReader();

                if (reader.Read()) {
                    repros = Convert.ToUInt32(reader["CantidadEscuchada"]);
                }

            }

            return repros;
        }

        public Task<SongModel?> GetSong(AlmacenModel? almacen) {

            //if (almacen is null) return null;
            throw new NotImplementedException();
            //return await App.SongRepository.GetById(almacen.SongId);
        }

        public Task<bool> RemoveById(object? id) {
            throw new NotImplementedException();
        }
    }
}
