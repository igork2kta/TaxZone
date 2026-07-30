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

        public string Modulo {  get; set; }

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

        public string d_consulta_rel_proc_imp_grid { get; set; }


        public string d_prog_job_imp_uf_tab_taxone { get; set; }
        public string d_lis_arquivos_imp { get; set; }
        public string d_dddw_empresa_usuario { get; set; }
        public string omssafil_safilcm2_m_man_job_imp_safil { get; set; }
        public string uo_parametros { get; set; }
        public string d_prog_job_imp_frmwk { get; set; } 
        public string dd_lib_proc_numeric { get; set; } 
        public string d_lib_proc_par_header { get; set; } 




    }
}
