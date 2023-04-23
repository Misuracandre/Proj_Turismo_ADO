using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Proj_Turismo_ADO.Models;
using Proj_Turismo_ADO.Services;

namespace Proj_Turismo_ADO.Controllers
{
    public class HotelsController
    {
        public bool Insert(Hotel hotel)
        {
            return new HotelsService().Insert(hotel);
        }

        public List<Hotel> FindAll()
        {
            return new HotelsService().FindAll();
        }

        public void UpdateHotel(Hotel hotel)
        {
            new HotelsService().UpdateHotel(hotel);
        }

        public void DeleteHotel(int id)
        {
            new HotelsService().DeleteHotel(id);
        }
    }
}
}
