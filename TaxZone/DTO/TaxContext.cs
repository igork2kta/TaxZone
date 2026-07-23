using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaxZone.DTO
{
    public class TaxContext
    {
        public string Empresa { get; set; }

        public string StorageId { get; set; }

        public string NewViews { get; set; }

        public string UniqueId { get; set; }

        public string NewViews2 { get; set; }

        public string ControlNumber { get; set; }

        public string DataManagerId { get; set; }

        //DataManagerId da tela de processos customizados
        public string d_lib_proc_processos { get; set; }

        //DataManagerId 2 da tela de lista de arquivos customizados
        public string d_lib_proc_lista_arquivos { get; set; }

        public string d_lib_proc_lista_arquivos_header_taxbr { get; set; }

        public string Id { get; set; }

        public string UniqueIdListaArquivos { get; set; }
    }
}
