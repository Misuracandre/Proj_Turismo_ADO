using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Proj_Turismo_ADO.Models;
using Proj_Turismo_ADO.Services;

namespace Proj_Turismo_ADO.Controllers
{
    public class TicketsController
    {
        public bool Insert(Ticket ticket)
        {
            return new TicketsService().Insert(ticket);
        }

        public List<Ticket> FindAll()
        {
            return new TicketsService().FindAll();
        }

        public void UpdateTicket(Ticket ticket)
        {
            new TicketsService().UpdateTicket(ticket);
        }

        public void DeleteTicket(int id)
        {
            new TicketsService().DeleteTicket(id);
        }
    }
}
}
