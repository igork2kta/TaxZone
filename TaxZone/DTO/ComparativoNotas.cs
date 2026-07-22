using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
