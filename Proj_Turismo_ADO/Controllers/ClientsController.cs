using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Proj_Turismo_ADO.Models;
using Proj_Turismo_ADO.Services;

namespace Proj_Turismo_ADO.Controllers
{
    public class ClientsController
    {
        public bool Insert(Client client)
        {
            return new ClientsService().Insert(client);
        }

        public List<Client> FindAll()
        {
            return new ClientsService().FindAll();
        }

        public void UpdateClient(Client client)
        {
            new ClientsService().UpdateClient(client);
        }

        public void DeleteClient(int id)
        {
            new ClientsService().DeleteClient(id);
        }
    }
}
}
