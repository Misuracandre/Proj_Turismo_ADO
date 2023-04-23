using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Proj_Turismo_ADO.Models;
using Proj_Turismo_ADO.Services;

namespace Proj_Turismo_ADO.Controllers
{
    public class AddressesControler
    {
        public bool Insert(Address address)
        {
            return new AddressesService().Insert(address);
        }

        public List<Address> FindAll()
        {
            return new AddressesService().FindAll();
        }

        public void UpdateAddress(Address address)
        {
            new AddressesService().UpdateAddress(address);
        }

        public void DeleteAddress(int id)
        {
            new AddressesService().DeleteAddress(id);
        }
    }
}
