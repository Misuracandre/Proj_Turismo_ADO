using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proj_Turismo_ADO.Models
{
    public class Hotel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Address IdAddress { get; set; }
        public decimal Value { get; set; }
        public DateTime Registration_Date { get; set; }
    }
}
