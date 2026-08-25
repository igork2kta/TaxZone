using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaxZone.DTO
{

    public sealed record ParametrosProcessosCustomizados {

        public string Empresa { get; init; }
        public string Estabelecimento { get; init; }
        public string DataInicio { get; init; }
        public string DataFim { get; init; }
        public string BuracoNota { get; init; }
        public string DiferencaCapaItem { get; init; }
        public string IcmsResumido { get; init; }
        public string NotasSemItem { get; init; }
        public string QuantidadeItens { get; init; }
        public string QuantidadeNotas { get; init; }
        public string QuantidadeCanceladas { get; init; }
        public string ExtracaoCanceladas { get; init; }

        public ParametrosProcessosCustomizados( string Empresa, string Estabelecimento, DateTime DataInicio, DateTime DataFim, bool BuracoNota, bool DiferencaCapaItem, bool IcmsResumido,
                                                bool NotasSemItem, bool QuantidadeItens, bool QuantidadeNotas, bool QuantidadeCanceladas, bool ExtracaoCanceladas)
        {
            this.Empresa = Empresa;
            this.Estabelecimento = Estabelecimento;
            this.DataInicio = DataInicio.ToString("ddMMyyyy000000");
            this.DataFim = DataFim.ToString("ddMMyyyy235959");
            this.BuracoNota = BuracoNota ? "S" : "N";
            this.DiferencaCapaItem = DiferencaCapaItem ? "S" : "N";
            this.IcmsResumido = IcmsResumido ? "S" : "N";
            this.NotasSemItem = NotasSemItem ? "S" : "N";
            this.QuantidadeItens = QuantidadeItens ? "S" : "N";
            this.QuantidadeNotas = QuantidadeNotas ? "S" : "N";
            this.QuantidadeCanceladas = QuantidadeCanceladas ? "S" : "N";
            this.ExtracaoCanceladas = ExtracaoCanceladas ? "S" : "N";
        }
    }         
}
