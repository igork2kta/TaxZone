using iText.Layout.Element;
using Microsoft.VisualBasic.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TaxZone.DTO;

namespace TaxZone
{
    public static class TaxAutomationInspector
    {

        public static async void Start()
        {
            //Faz login
            string cookie = await ApiTax.GetCookie(Config.UsuarioTax, Config.SenhaTax, true);
            Config.Cookie = cookie;

            //Consulta os logs de importação do Tax Automation
            var resultados = await ConsultarJob(Empresa.ListaEmpresas, "TAX-AUTOMATION");

            //Valida as empresas com erro
            var empresasComErro = ValidarErros(resultados);

            //Programa job para as empresas com erro

            ApiTax.ResetContext();
            var tasks = empresasComErro.Select(async empresa =>
            {
                return await ApiTax.ProgramarJob(empresa, null, null);
            });

            TaxApiResponse[] resultProgJob = await Task.WhenAll(tasks);

            List<string> empresasJobProgramado = new List<string>();
            foreach (var resultado in resultProgJob)
            {
                if (!resultado.Success)
                {
                    Logger.Log(resultado.Empresa, $"Falha ao programar JOB manual: {resultado.Message}");
                    empresasJobProgramado.Add(resultado.Empresa);
                }
                else
                {
                    Logger.Log(resultado.Empresa, $"JOB manual programado.");
                }
            }

            var taskConsultaJobManual = Task.Run(async () =>
            {
                await Task.Delay(TimeSpan.FromMinutes(5));
                return await ConsultarJob(empresasJobProgramado, Config.UsuarioTax);
            });

           
            var resultJobManual = await Task.WhenAll(taskConsultaJobManual);
            empresasComErro = ValidarErros(resultJobManual.SelectMany(x => x).ToArray());


        }

        public static async Task<TaxApiResponse[]> ConsultarJob(List<string> empresas, string usuario)
        {
            var tarefas = empresas.Select(async empresa =>
            {
                ParametrosRelatorioImportacao parametros = new(DateTime.Today, DateTime.Now, usuario, "", "");
                return await ApiTax.ObterLogsProcessosImportacao(empresa,parametros,null);
            });

            return await Task.WhenAll(tarefas);
        }

        public static List<string> ValidarErros(TaxApiResponse[] resultados)
        {
            List<string> empresasComErro = new List<string>();

            foreach (var resultado in resultados)
            {
                if (resultado != null)
                {
                    foreach (var log in resultado.ProcessosImportacao)
                    {
                        if (log.QtdErr > 0)
                        {
                            Logger.Log(resultado.Empresa, $"{log.Descricao} - Erros: {log.QtdErr}");

                            if (log.Descricao == "IMPX42" || log.Descricao == "IMPX43")
                            {
                                empresasComErro.Add(resultado.Empresa);
                                break;
                            }
                        }
                    }
                }
                else
                {
                    Logger.Log("GERAL", "Nenhum log encontrado.");
                }
            }

            return empresasComErro;
        }
    }
}
