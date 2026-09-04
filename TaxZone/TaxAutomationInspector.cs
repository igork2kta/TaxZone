using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaxZone.DTO;

namespace TaxZone
{
    public static class TaxAutomationInspector
    {

        public static async void Start()
        {

            string cookie = await ApiTax.GetCookie(Config.UsuarioTax, Config.SenhaTax, true);
            Config.Cookie = cookie;

            ParametrosRelatorioImportacao parametros = new(
                    DateTime.Now,
                    DateTime.Now,
                    "TAX-AUTOMATION",
                    "TODOS",
                    "");


            var tarefas = Empresa.ListaEmpresas.Select(async empresa =>
            {
                var retorno = await ApiTax.ObterLogsProcessosImportacao(
                    empresa,
                    parametros,
                    null
                );

                return new
                {
                    Empresa = empresa,
                    Retorno = retorno
                };
            });

            var resultados = await Task.WhenAll(tarefas);
        }
    }
}
