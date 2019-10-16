using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KentekenClient.Models
{
    public class KentekenLogViewModel
    {
        public int ID { get; set; }
        public string User_Id { get; set; }
        public string Kenteken { get; set; }
        public DateTime Log_Datetime { get; set; }
    }
}
