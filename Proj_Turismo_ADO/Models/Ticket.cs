using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proj_Turismo_ADO.Models
{
    public class Ticket
    {
        public int Id { get; set; }
        public Address? IdOrigin { get; set; }
        public Address? IdDestination { get; set; }
        public Client? IdClient { get; set; }
        public decimal Value { get; set; }
        public DateTime Registration_Date { get; set; }
    }
}
