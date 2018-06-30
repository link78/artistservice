using DataAccessLayer.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Services
{
   public interface IWebService
    {
        void Add<T>(T entity) where T : class;
        void Delete<T>(T entity) where T : class;

        Task<bool> SaveAllAsync();

        // Artists
        IEnumerable<Artist> GetArtists();
        Artist GetArtist(Guid id);
        Artist GetArtistWithAlbums(Guid id);

        // Albums
        IEnumerable<Album> GetAlbums(Guid id);
        Album GetAlbum(Guid albumId);
    }
}
