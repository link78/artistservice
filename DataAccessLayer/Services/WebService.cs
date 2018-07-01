using DataAccessLayer.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Services
{
   public class WebService:IWebService
    {
        private DataContext context;

        public WebService(DataContext ctx) => context = ctx;



        public void Add<T>(T entity) where T : class
        {
            context.Add(entity);
        }

        public void Delete<T>(T entity) where T : class
        {
            context.Remove(entity);
        }

        public Album GetAlbum(Guid albumId)
        {
            return context
                .Albums
                .Include(a => a.Artist)
                .Where(a => a.Id == albumId)
                .FirstOrDefault();
                
        }

        public IEnumerable<Album> GetAlbums(Guid id)
        {
            return context
                .Albums
                .Include(a => a.Artist)
                .Where(a => a.Artist.Id == id)
                .OrderBy(a => a.Name).ToList();
        }

        public Artist GetArtist(Guid id)
        {
            return context.Artists
                .Include(a => a.Contact)
                .Include(a => a.Manager)
                .Where(c => c.Id == id)
                .FirstOrDefault();
        }

        public IEnumerable<Artist> GetArtists()
        {
            return context.Artists
                 .Include(a => a.Contact)
                 .Include(a => a.Manager)
                 .ToList();
                 
        }

        public Artist GetArtistWithAlbums(Guid id)
        {
            return context.Artists
                .Include(a => a.Contact)
                .Include(a => a.Manager)
                .Include(a=> a.Albums)
                .Where(c => c.Id == id)
                .FirstOrDefault();
        }

        public async Task<bool> SaveAllAsync()
        {
            return (await context.SaveChangesAsync()) > 0;
        }
    }
}
