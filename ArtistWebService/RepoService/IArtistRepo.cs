using DataAccessLayer.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ArtistWebService.RepoService
{
  public  interface IArtistRepo
    {
        object GetArtist(Guid id);
        object GetArtistsWithSkip(int skip, int take);
        object GetArtists();
        Guid SaveArtist(Artist artist);
        void UpdateArtist(Artist artist);
        void DeleteArtist(Guid id);
    }

    public class ArtistRepo : IArtistRepo
    {
        private DataContext context;

        public ArtistRepo(DataContext _context) => context = _context;

        public void DeleteArtist(Guid id)
        {
            context.Artists.Remove(new Artist { Id = id });
            context.SaveChanges();

        }

        public object GetArtist(Guid id)
        {
            return context.Artists.Include(a=>a.Contact).Include(a=>a.Manager)
                .Select(a=> new
                {
                    Id =a.Id, Name=a.FullName, ArtistName=a.ArtistName, Gender=a.Gender,
                    Contact = new
                    {
                        Id= a.Contact.Id,
                        Email= a.Contact.Email,
                        Phone= a.Contact.Phone,
                        Tweeter= a.Contact.Tweeter
                    },
                    Manager = new
                    {
                        Id = a.Manager.Id,
                        Name= a.Manager.FullName,
                        Email = a.Manager.Email,
                        Phone = a.Manager.Phone,
                        Tweeter = a.Manager.Tweeter
                    }
                    
                })
                .FirstOrDefault(a => a.Id == id);
        }

        public object GetArtists()
        {
            return context.Artists.Include(a => a.Contact)
                .Include(a => a.Manager)
                .OrderBy(a => a.Id)
                .Select(a => new
                {
                    Id = a.Id,
                    Name = a.FullName,
                    ArtistName = a.ArtistName,
                    Gender = a.Gender,
                    Contact = new
                    {
                        Id = a.Contact.Id,
                        Email = a.Contact.Email,
                        Phone = a.Contact.Phone,
                        Tweeter = a.Contact.Tweeter
                    },
                    Manager = new
                    {
                        Id = a.Manager.Id,
                        Name = a.Manager.FullName,
                        Email = a.Manager.Email,
                        Phone = a.Manager.Phone,
                        Tweeter = a.Manager.Tweeter
                    }

                });
        }

        public object GetArtistsWithSkip(int skip, int take)
        {
            return context.Artists.Include(a => a.Contact)
                .Include(a => a.Manager)
                .OrderBy(a => a.Id)
                .Skip(skip)
                .Take(take)
                .Select(a => new
                {
                    Id = a.Id,
                    Name = a.FullName,
                    ArtistName = a.ArtistName,
                    Gender = a.Gender,
                    Contact = new
                    {
                        Id = a.Contact.Id,
                        Email = a.Contact.Email,
                        Phone = a.Contact.Phone,
                        Tweeter = a.Contact.Tweeter
                    },
                    Manager = new
                    {
                        Id = a.Manager.Id,
                        Name = a.Manager.FullName,
                        Email = a.Manager.Email,
                        Phone = a.Manager.Phone,
                        Tweeter = a.Manager.Tweeter
                    }

                });
        }

        public Guid SaveArtist(Artist artist)
        {
            context.Artists.Add(artist);
            context.SaveChanges();
            return artist.Id;
        }

        public void UpdateArtist(Artist artist)
        {
            context.Artists.Update(artist);
            context.SaveChanges();
        }
    }
}
