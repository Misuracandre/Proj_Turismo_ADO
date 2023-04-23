using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Proj_Turismo_ADO.Models;
using Proj_Turismo_ADO.Services;

namespace Proj_Turismo_ADO.Controllers
{
    public class CitiesController
    {
        public bool Insert(City city)
        {
            return new CitiesService().Insert(city);
        }

        public List<City> FindAll()
        {
            return new CitiesService().FindAll();
        }

        public void UpdateCity(City city)
        {
            new CitiesService().UpdateCity(city);
        }

        public void DeleteCity(int id)
        {
            new CitiesService().DeleteCity(id);
        }
    }
}

