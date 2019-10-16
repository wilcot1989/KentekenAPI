namespace KentekenClient.Models
{
    public class KentekenRdwViewModel
    {
        public string Kenteken { get; set; }
        public string Voertuigsoort { get; set; }
        public string Merk { get; set; }
        public string Handelsbenaming { get; set; }
        public string Vervaldatum_apk { get; set; }
        public string Datum_tenaamstelling { get; set; }
        public decimal Bruto_bpm { get; set; }
        public string Inrichting { get; set; }
        public int Aantal_zitplaatsen { get; set; }
        public string Eerste_kleur { get; set; }
        public string Tweede_kleur { get; set; }
        public int Aantal_cilinders { get; set; }
        public decimal Cilinderinhoud { get; set; }
        public decimal Massa_ledig_voertuig { get; set; }
        public decimal Toegestane_maximum_massa_voertuig { get; set; }
        public decimal Massa_rijklaar { get; set; }
        public decimal Maximum_massa_trekken_ongeremd { get; set; }
        public decimal Maximum_trekken_massa_geremd { get; set; }
        public string Zuinigheidslabel { get; set; }
        public string Datum_eerste_toelating { get; set; }
        public string Datum_eerste_afgifte_nederland { get; set; }
        public decimal Catalogusprijs { get; set; }
        public string Wam_verzekerd { get; set; }
        public int Aantal_deuren { get; set; }
        public int Aantal_wielen { get; set; }
        public decimal Afstand_hart_koppeling_tot_achterzijde_voertuig { get; set; }
        public decimal Afstand_voorzijde_voertuig_tot_hart_koppeling { get; set; }
        public decimal Lengte { get; set; }
        public decimal Breedte { get; set; }
        public string Europese_voertuigcategorie { get; set; }
        public string Plaats_chassisnummer { get; set; }
        public decimal Technische_max_massa_voertuig { get; set; }
        public string Typegoedkeuringsnummer { get; set; }
        public decimal Wielbasis { get; set; }
        public string Export_indicator { get; set; }
        public string Openstaande_terugroepactie_indicator { get; set; }
        public string Taxi_indicator { get; set; }
    }
}
