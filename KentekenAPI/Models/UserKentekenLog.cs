using System;

namespace KentekenAPI.Models
{
    public class UserKentekenLog
    {
        public int ID { get; set; }
        public string User_Id  { get; set; }
        public string Kenteken { get; set; }
        public DateTime Log_Datetime { get; set; }
    }
}
