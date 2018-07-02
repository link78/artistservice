using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ArtistWebService.RepoService;
using DataAccessLayer.Entities;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ArtistWebService.Controllers
{
    [Route("api/artistalbums")]
    public class ArtistValuesController : Controller
    {
        private IArtistRepo repo;
        public ArtistValuesController(IArtistRepo _repo) => repo = _repo;

        [HttpGet("{id}")]
        public object Get(Guid id)
        {
            return repo.GetArtist(id) ?? NotFound();
        }

        [HttpGet]
        public object Artists()
        {
            return repo.GetArtists();
        }
        //[HttpGet]
        //public object GetArtistsWithSkip(int skip, int take)
        //{
        //    return repo.GetArtistsWithSkip(skip, take);
        //}

        [HttpPost]
        public Guid SaveArtist([FromBody]Artist artist)
        {
            return repo.SaveArtist(artist);
        }

        [HttpPut]
        public void UpdateArtist([FromBody]Artist artist)
        {
            repo.UpdateArtist(artist);
        }


        [HttpDelete("{id}")]
        public void DeleteArtist(Guid id)
        {
            repo.DeleteArtist(id);
        }
    }
}
