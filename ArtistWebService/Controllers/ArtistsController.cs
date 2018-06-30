using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DataAccessLayer.Services;
using AutoMapper;
using ArtistWebService.Data;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ArtistWebService.Controllers
{
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
        // GET: /<controller>/
        public IActionResult GetArtist()
        {
            var model = service.GetArtists();
            return Ok(mapper.Map<IEnumerable<ArtistDto>>(model));
        }
    }
}
