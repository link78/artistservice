using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using DataAccessLayer.Services;
using ArtistWebService.Data;
using DataAccessLayer.Entities;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ArtistWebService.Controllers
{

    [Route("api/artists/{artistId}/albums")]
    public class AlbumsController : Controller
    {

        private IWebService service;
        private IMapper mapper;

        public AlbumsController(IWebService _service, IMapper _mapper)
        {
            service = _service;
            mapper = _mapper;
        }

        [HttpGet("")]
        public IActionResult GeAlbums(Guid artistId)
        {
            var albums = service.GetAlbums(artistId);
            return Ok(mapper.Map<IEnumerable<AlbumDto>>(albums));
        }

        [HttpGet("{id}",Name ="GetAlbum")]
        public IActionResult GetAlbum(Guid artistId, Guid id)
        {
            try
            {
                var albums = service.GetAlbum(id);
                if (albums == null) return NotFound();
                if (albums.Artist.Id != artistId)
                    return BadRequest("Albums not in specific artist container");

                return Ok(mapper.Map<AlbumDto>(albums));
            }
            catch (Exception)
            {

            }
            return BadRequest("Please try again");
        }

        [HttpPost(Name ="CreateAlbum")]
        public async Task<IActionResult>Create(Guid artistId,[FromBody] AlbumDto model)
        {

            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var list = service.GetArtist(artistId);
                if (list == null) return BadRequest("Could not find Artist");

                var album = mapper.Map<Album>(model);

                // adding to Album model
                service.Add(album);

                if(await service.SaveAllAsync())
                {
                    var url = Url.Link("GetAlbum", new { artistId = list.Id, id = album.Id });
                    return Created(url, mapper.Map<AlbumDto>(album));
                }
            }
            catch (Exception)
            {

            }
            return BadRequest("Please try again");
        }


        [HttpPut("{id}",Name ="UpdateArtist")]
        public async Task<IActionResult>Update(Guid artistId,Guid id ,[FromBody] AlbumDto model)
        {
            try
            {
                var album = service.GetAlbum(id);
                if (album == null) return NotFound();
                if (album.Artist.Id != artistId)
                    return BadRequest("Album not in specofic artist container");


                mapper.Map(model, album);
                
                if(await service.SaveAllAsync())
                {
                    return Ok(mapper.Map<AlbumDto>(album));
                }
            }
            catch (Exception)
            {

            }
            return BadRequest("Please try again");
        }

        [HttpDelete("{id}", Name = "DeleteAlbum")]
        public async Task<IActionResult> Delete(Guid artistId, Guid id)
        {
            try
            {
                var album = service.GetAlbum(id);
                if (album == null) return NotFound();
                if (album.Artist.Id != artistId)
                    return BadRequest("Album not in specofic artist container");


                service.Delete(album);

                if (await service.SaveAllAsync())
                {
                    return Ok($"Album with ID {id} has been deleted");
                }
            }
            catch (Exception)
            {

            }
            return BadRequest("Please try again");
        }
    }
}
