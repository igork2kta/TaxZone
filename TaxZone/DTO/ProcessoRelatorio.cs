using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaxZone.DTO
{
    public class ProcessoRelatorio
    {
        public int NumProcesso { get; set; }
        public string InicioProcessamento { get; set; }
        public string FimProcessamento { get; set; }
        public string Usuario { get; set; }
        public string Status { get; set; }
        public string Detalhes { get; set; }
    }
}
