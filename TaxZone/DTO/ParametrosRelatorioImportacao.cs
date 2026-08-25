using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaxZone.DTO
{
    public class ParametrosRelatorioImportacao
    {
        public ParametrosRelatorioImportacao(DateTime dataInicio, DateTime dataFim, string usuario, string estabelecimento, string descricao)
        {
            DataInicio = dataInicio;
            DataFim = dataFim;
            Usuario = usuario;
            Estabelecimento = estabelecimento;
            Descricao = descricao;
        }

        public DateTime DataInicio { get; set; }
        public DateTime DataFim { get; set; }
        public string Usuario { get; set; }
        public string Estabelecimento { get; set; }
        public string Descricao { get; set; }
    }
}
