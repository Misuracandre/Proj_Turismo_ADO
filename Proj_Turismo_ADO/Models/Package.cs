using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proj_Turismo_ADO.Models
{
    public class Package
    {
        public int Id { get; set; }
        public Hotel IdHotel { get; set; }
        public Ticket IdTicket { get; set; }
        public Client IdClient { get; set; }
        public decimal? Value { get; set; }
        public DateTime Registration_Date { get; set; }
    }
}
