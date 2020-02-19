﻿using SehirRehberi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SehirRehberi.Data
{
    public interface IAppRepository
    {

        void Add<T>(T entity);
        void Delete<T>(T entity);
        bool SaveAll();

        List<City> GetCities();
        List<Photo> GetPhotosByCity(int cityId);
        City GetCityById(int cityId);
        Photo GetPhoto(int id);

    }
}
