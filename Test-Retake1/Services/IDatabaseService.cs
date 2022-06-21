using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Test_Retake1.Dto;

namespace Test_Retake1.Services
{
    public interface IDatabaseService
    {

        Task<bool> DoesAlbumExistAsync(int idAlbum);

        Task<AlbumDto> GetAlbum(int idAlbum);

        Task<bool> DoesMusicianExistAsync(int idMusician);

        Task<bool> CanMusicianBeDeleted(int idMusician);

        Task DeleteMusicianAsync(int idMusician);

    }
}
