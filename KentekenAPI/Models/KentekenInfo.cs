using System.ComponentModel.DataAnnotations;

namespace KentekenAPI.Models
{
    public class KentekenInfo
    {
        //Alle variabelen een string gemaakt. Dit omdat de database conversie niet helemaal goed gaat.
        //Beetje een dirty oplossing
        [Key]
        public string Kenteken { get; set; }

        public string Voertuigsoort { get; set; }
        public string Merk { get; set; }
        public string Handelsbenaming { get; set; }
        public string Bpm { get; set; }
        public string Cilinderinhoud { get; set; }
        public string MassaLedig { get; set; }
        public string MassaMax { get; set; }
        public string Catalogusprijs { get; set; }
        public string WamVerzekerd { get; set; }
    }
}
