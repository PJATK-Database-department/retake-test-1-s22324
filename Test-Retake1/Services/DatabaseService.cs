using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Test_Retake1.Dto;

namespace Test_Retake1.Services
{
    public class DatabaseService : IDatabaseService
    {

        private string _connectionString;

        public DatabaseService(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("Default");

        }

        public async Task<bool> DoesAlbumExistAsync(int idAlbum)
        {
            using (var con = new SqlConnection(_connectionString))
            {
                var cmd = new SqlCommand(
                    "SELECT IIF(COUNT(1) > 0, 1, 0) FROM Album WHERE IdAlbum = " + idAlbum, con);
                await con.OpenAsync();
                var result = await cmd.ExecuteScalarAsync();
                await con.CloseAsync();
                return Convert.ToBoolean(result);
            }
        }

        public async Task<AlbumDto> GetAlbum(int idAlbum)
        {
            var result = new AlbumDto();
            var tracks = new List<TrackDto>();
            using (var con = new SqlConnection(_connectionString))
            {
                var cmdTracks = new SqlCommand(
                    "SELECT TrackName, Duration FROM Track WHERE IdAlbum = @idAlbum ORDER BY Duration", con);
                cmdTracks.Parameters.AddWithValue("idAlbum", idAlbum);
                await con.OpenAsync();
                var dr = await cmdTracks.ExecuteReaderAsync();
                while (await dr.ReadAsync())
                {
                    TrackDto track = new TrackDto();
                    track.TrackName = dr[0].ToString();
                    track.Duration = double.Parse(dr[1].ToString());
                    tracks.Add(track);
                }

                var cmdAlbum = new SqlCommand(
                    "SELECT AlbumName, PublishDate, IdMusicLabel FROM Album WHERE IdAlbum = " + idAlbum, con);
                var dr1 = await cmdAlbum.ExecuteReaderAsync();
                while (await dr.ReadAsync())
                {
                    result.AlbumName = dr[0].ToString();
                    result.PublishDate = DateTime.Parse(dr[1].ToString());
                    result.MusicLabel = await GetMusicLabelNameAsync(Int32.Parse(dr[2].ToString())); 
                }
                await con.CloseAsync();
                return result;
            }

        }

        public async Task<string> GetMusicLabelNameAsync(int idMusicLabel)
        {
            using (var con = new SqlConnection(_connectionString))
            {
                var cmd = new SqlCommand(
                    "SELECT Name FROM MusicLabel WHERE IdMusicLabel = " + idMusicLabel, con);
                await con.OpenAsync();
                var dr = await cmd.ExecuteReaderAsync();
                await dr.ReadAsync();
                var result = dr[0].ToString();
                await con.CloseAsync();
                return result;
            }
        }

        public async Task<bool> DoesMusicianExistAsync(int idMusician)
        {
            using (var con = new SqlConnection(_connectionString))
            {
                var cmd = new SqlCommand(
                    "SELECT IIF(COUNT(1) > 0, 1, 0) FROM Musician WHERE IdMusician = " + idMusician, con);
                await con.OpenAsync();
                var result = await cmd.ExecuteScalarAsync();
                await con.CloseAsync();
                return Convert.ToBoolean(result);
            }
        }

        public async Task DeleteMusicianAsync(int idMusician)
        {
            using (var con = new SqlConnection(_connectionString))
            {
                await con.OpenAsync();
                SqlTransaction tran = con.BeginTransaction();
                try
                {
                    var cmd = new SqlCommand(
                        "DELETE FROM Musician WHERE IdMusician = " + idMusician, con, tran);
                    await cmd.ExecuteNonQueryAsync();
                    await tran.CommitAsync();
                    await con.CloseAsync();
                }
                catch (Exception)
                {
                    await tran.RollbackAsync();
                    await con.CloseAsync();
                }
            }
        }

        public async Task<bool> CanMusicianBeDeleted(int idMusician)
        {
            
            var tracks = new List<MusicianTrackDto>();
            using (var con = new SqlConnection(_connectionString))
            {
                var cmdTracks = new SqlCommand(
                    "SELECT IdTrack FROM Musician_Track WHERE IdMusician = " + idMusician, con);
                await con.OpenAsync();
                var dr = await cmdTracks.ExecuteReaderAsync();
                while (await dr.ReadAsync())
                {
                    MusicianTrackDto track = new MusicianTrackDto();
                    track.IdTrack = Int32.Parse(dr[0].ToString());
                    tracks.Add(track); // list of idtrack associated to the musican
                }

                foreach(MusicianTrackDto track in tracks)
                {
                    var cmd = new SqlCommand(
                       "SELECT IdMusicAlbum FROM Track WHERE IdTrack = " + track.IdTrack, con);
                    var dr2 = await cmd.ExecuteReaderAsync();
                    while(await dr2.ReadAsync())
                    {
                        track.IdMusicAlbum = Int32.Parse(dr2[0].ToString());
                    }
                }
                foreach(MusicianTrackDto track in tracks)
                {
                    if(track.IdMusicAlbum != null) 
                        // if any of the tracks that are associated to music albums have existing music albums,
                        // the artist cant be removed, if there is not a single null album artist cannot be removed
                    {
                        return false;
                    }
                }
                await con.CloseAsync();
                return true;
            }
            //Musician can be deleted only if he is involved in creating songs that have not yet appeared in the target albums.
            // So if he is involved in even a single song, that does have an album he cant be deleted.


            // check wether any of this musicians song has idmusicalbum = null
        }

    }
}
