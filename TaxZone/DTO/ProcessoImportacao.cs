using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaxZone.DTO
{
    public class ProcessoImportacao
    {
        public int NumProcesso { get; set; }
        public string CodEmpresa { get; set; }
        public string CodEstab { get; set; }
        //public string IndProcesso { get; set; }
        public string Descricao { get; set; }
        public int QtdLido { get; set; }
        public int QtdIns { get; set; }
        public int QtdAlt { get; set; }
        public int QtdIgn { get; set; }
        public int QtdErr { get; set; }
        public string Status { get; set; }
        public DateOnly DataIniMovto { get; set; }
        public DateOnly DataFimMovto { get; set; }
        public string CodUsuario { get; set; }
        public DateOnly DataIni { get; set; }
        
        public DateOnly DataFim { get; set; }
        
        
    }
}
