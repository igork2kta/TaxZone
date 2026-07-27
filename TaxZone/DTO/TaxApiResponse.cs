using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaxZone.DTO
{
    public class TaxApiResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public string Empresa { get; set; }
        public bool Completed {  get; set; }

        public List<ProcessoImportacao> ProcessosImportacao {  get; set; }
        public List<ProcessoRelatorio> ProcessosRelatorio { get; set; }

        public TaxApiResponse(bool sucess, string message, string empresa)
        {
            Success = sucess;
            Message = message;
            Empresa = empresa;
        }
    }
}
