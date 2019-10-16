using System;

namespace KentekenClient.Models
{
    public class JwtModel
    {
        public string Token { get; set; }
        public DateTime Expiration { get; set; }
    }

}
