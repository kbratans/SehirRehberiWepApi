using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using SehirRehberi.Data;
using SehirRehberi.Dtos;
using SehirRehberi.Helper;
using SehirRehberi.Models;

namespace SehirRehberi.Controllers
{
    [Produces("application/json")]
    [Route("api/cities/{cityId}/photos")]
    public class PhotosController : Controller
    {
        private IAppRepository _appRepository;
        private IMapper _mapper;
        IOptions<CloudinarySettings> _cloudinaryConfig;

        private Cloudinary _cloudinary;


        public PhotosController(IAppRepository appRepository,IMapper mapper, IOptions<CloudinarySettings> cloudinaryConfig)
        {
            _appRepository = appRepository;
            _mapper = mapper;
            _cloudinaryConfig = cloudinaryConfig;

            Account account = new Account(_cloudinaryConfig.Value.CloudName, _cloudinaryConfig.Value.ApiKey, _cloudinaryConfig.Value.ApiSecret);

            _cloudinary = new Cloudinary(account);
        }

        public object ClaimType { get; private set; }

        [HttpPost]
        public ActionResult AddPhotoForCity(int cityId,[FromBody]PhotoForCreation photoForCreation)
        {
            var city = _appRepository.GetCityById(cityId);

            if (city == null)
            {

                return BadRequest("could not find the city");
            }

            var currentUserId= int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            if(currentUserId!=city.UserId)
            {
                return Unauthorized();
            }
            var file = photoForCreation.File;
            var uploadResult = new ImageUploadResult();

            if(file.Length>0)
            {
                using (var stream = file.OpenReadStream())
                {
                    var uploadParams = new ImageUploadParams
                    {

                        File = new FileDescription(file.Name, stream)
                    };

                    uploadResult = _cloudinary.Upload(uploadParams);

                }
            }

            photoForCreation.Url = uploadResult.Uri.ToString();
            photoForCreation.PublicId = uploadResult.PublicId;

            var photo = _mapper.Map<Photo>(photoForCreation);
            photo.City = city;

            if (!city.Photos.Any(p => p.IsMain))
            {
                photo.IsMain = true;

            }
            city.Photos.Add(photo);

            if(_appRepository.SaveAll())
            {
                var photoToReturn = _mapper.Map<PhotoForReturn>(photo);
                return CreatedAtRoute("GetPhoto", new { id = photo.Id }, photoToReturn);
            }

            return BadRequest("Could not add the photo");
        }

          [HttpGet("{id}",Name ="GetPhoto")]
        public ActionResult GetPhoto(int id)
        {

            var photoFromDb = _appRepository.GetPhoto(id);
            var photo = _mapper.Map<PhotoForReturn>(photoFromDb);
            return Ok(photo);


        }
    }
}