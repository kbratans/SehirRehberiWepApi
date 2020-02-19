using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SehirRehberi.Data;
using SehirRehberi.Dtos;
using SehirRehberi.Models;

namespace SehirRehberi.Controllers
{
    [Produces("application/json")]
    [Route("api/Cities")] 
    public class CitiesController : Controller
    {
        private IAppRepository _appRepository;
        private IMapper _mapper;

        public CitiesController(IAppRepository appRepository, IMapper mapper)
        {
            _appRepository = appRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public ActionResult GetCities()
        {

            var cities = _appRepository.GetCities();
            var citiesToReturn = _mapper.Map<List<CityForList>>(cities);
            return Ok(cities);
        }

        [HttpPost]
        [Route("add")]
        public ActionResult Add([FromBody]City city)
        {
            _appRepository.Add(city);
            _appRepository.SaveAll();
            return Ok(city);        
        }

        [HttpGet]
        [Route("detail")]
        public ActionResult GetCityById(int id)  
        {
            var city= _appRepository.GetCityById(id);
            var cityToReturn = _mapper.Map<CityForDetail>(city);
            return Ok(cityToReturn);
        }

        [HttpGet]
        [Route("photos")]
        public ActionResult GetPhotosByCity(int cityId)
        {
            var photos = _appRepository.GetPhotosByCity(cityId);
            return Ok(photos);
        }

    }
}
