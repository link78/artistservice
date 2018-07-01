using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DataAccessLayer.Services;
using AutoMapper;
using ArtistWebService.Data;
using DataAccessLayer.Entities;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ArtistWebService.Controllers
{
   // [Authorize]
    [EnableCors("public")]
    [Route("api/artists")]
    public class ArtistsController : Controller
    {
        private IWebService service;
        private IMapper mapper;
        

        public ArtistsController(IWebService _service, IMapper _mapper)
        {
            service = _service;
            mapper = _mapper;
           
        }
        [EnableCors("limAdmin")]
        public IActionResult GetArtist()
        {
            var model = service.GetArtists();
            return Ok(mapper.Map<IEnumerable<ArtistDto>>(model));
        }

        [EnableCors("limAdmin")]
        [HttpGet("{id}", Name ="GetArtist")]
        public IActionResult Get(Guid id, bool included= false)
        {
            try
            {
                Artist model = null;
                if (included)
                    model = service.GetArtistWithAlbums(id);
                else
                    model = service.GetArtist(id);


                if (model == null)
                    return NotFound($"Artist {id} was not found");
                return Ok(mapper.Map<ArtistDto>(model));
            }
            catch
            {

            }
            return BadRequest();
        }

        [EnableCors("limAdmin")]
        [HttpPost]
        public async Task<IActionResult>Create([FromBody] Artist artist)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);
                service.Add(artist);

                if(await service.SaveAllAsync())
                {
                    var newUri = Url.Link("GetArtist", new { id = artist.Id });
                    return Created(newUri, artist);
                }
            }
            catch(Exception)
            {

            }

            return BadRequest("Error occur while creating new artist");
        }

        [EnableCors("limAdmin")]
        [HttpPut("{id}")]
        public async Task<IActionResult>Update(Guid id, [FromBody] Artist model)
        {
            try
            {
                var edit = service.GetArtist(id);
                if (edit == null)
                    return NotFound($"Could not find artist with a ID of {id}");

                // mapping
                edit.FullName = model.FullName ?? edit.FullName;
                edit.Gender = model.Gender ?? edit.Gender;
                edit.ArtistName = model.ArtistName ?? edit.ArtistName;

                edit.Contact = model.Contact ?? edit.Contact;  // object
               

                edit.Manager = model.Manager ?? edit.Manager;  // object
                


                if (await service.SaveAllAsync()) { return Ok(edit); }
            }
            catch (Exception)
            {

            }

            return BadRequest();
        }


        [EnableCors("limAdmin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult>Delete(Guid id)
        {
            try
            {
                var model = service.GetArtist(id);
                if (model == null)
                    return NotFound($"Could not find artist with ID od {id}");

                service.Delete(model);
                
                if(await service.SaveAllAsync())
                {
                    return Ok($"Artist with ID of {id} has been deleted");
                }
            }
            catch (Exception)
            {

            }

            return BadRequest("Could not delete this artist");
        }
    }
}
