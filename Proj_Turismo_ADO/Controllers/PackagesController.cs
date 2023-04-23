using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Proj_Turismo_ADO.Models;
using Proj_Turismo_ADO.Services;

namespace Proj_Turismo_ADO.Controllers
{
    public class PackagesController
    {
        public bool Insert(Package package)
        {
            return new PackagesService().Insert(package);
        }

        public List<Package> FindAll()
        {
            return new PackagesService().FindAll();
        }

        public void UpdatePackage(Package package)
        {
            new PackagesService().UpdatePackage(package);
        }

        public void DeletePackage(int id)
        {
            new PackagesService().DeletePackage(id);
        }
    }
}
}
