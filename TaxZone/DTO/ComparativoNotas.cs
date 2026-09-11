namespace TaxZone.DTO
{
    public class ComparativoNotas
    {
        public int Id { get; set; }
        public int Ano { get; set; }
        public int Mes { get; set; }
        public string Empresa { get; set; }
        public string Estabelecimento { get; set; }
        public string Tipo { get; set; }
        public int QtdSifar { get; set; }
        public int QtdTax { get; set; }
        public string Status { get; set; }
    }
}
