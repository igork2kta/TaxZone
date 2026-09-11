using iText.Layout.Element;
using Microsoft.Playwright;
using Microsoft.VisualBasic.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TaxZone.DTO;
using TaxZone.Infrastructure;
using TaxZone.Utils;

namespace TaxZone.Services
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
            var empresasComErro = await ValidarErros(resultados, false);

            //Programa job para as empresas com erro

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
                }
                else
                {
                    Logger.Log(resultado.Empresa, $"JOB manual programado.");
                    empresasJobProgramado.Add(resultado.Empresa);
                }
            }

            await Task.Delay(TimeSpan.FromMinutes(5));

            resultados = await ConsultarJob(empresasJobProgramado, Config.UsuarioTax);
            
            empresasComErro = await ValidarErros(resultados, true);


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

        public static async Task<List<string>> ValidarErros(TaxApiResponse[] resultados, bool gerarRelatorioReprocessamento)
        {
            List<string> empresasComErro = new List<string>();

            foreach (var resultado in resultados)
            {
                if (resultado != null)
                {
                    int row = 1;
                    foreach (var log in resultado.ProcessosImportacao)
                    {
                        if (log.Descricao == "IMPX431" || log.Descricao == "IMPX2013")
                            continue;

                        if (log.QtdErr > 0)
                        {
                            Logger.Log(resultado.Empresa, $"{log.Descricao} - Erros: {log.QtdErr}");

                            if (log.Descricao == "IMPX42" )
                            {
                                if (!empresasComErro.Contains(resultado.Empresa))
                                    empresasComErro.Add(resultado.Empresa);

                                if (gerarRelatorioReprocessamento)
                                {
                                    string caminhoRelatorio = $"J:\\Igor Pinheiro\\TaxOne\\Relatorio_SAFX42_{resultado.Empresa}_{DateTime.Now.Day}.pdf";
                                    string caminhoCsv = $"J:\\Igor Pinheiro\\TaxOne\\Reprocessar_SAFX04_{resultado.Empresa}_{DateTime.Now.Day}.csv";

                                    var response = await ApiTax.BaixarRelatorioProcessoImportacao(ApiTax.GetContext(resultado.Empresa), row, caminhoRelatorio );
                                    FuncoesTax.ImportarPessoaFisicaJuridica(true, false, false, null, caminhoRelatorio, caminhoCsv);

                                    File.Delete(caminhoRelatorio);
                                }

   
                            }
                            if (log.Descricao == "IMPX43")
                            {
                                if (!empresasComErro.Contains(resultado.Empresa))
                                    empresasComErro.Add(resultado.Empresa);

                                if (gerarRelatorioReprocessamento)
                                {
                                    string caminhoRelatorio = $"J:\\Igor Pinheiro\\TaxOne\\Relatorio_SAFX43_{resultado.Empresa}_{DateTime.Now.Day}.pdf";
                                    //string caminhoCsv = $"J:\\Igor Pinheiro\\TaxOne\\Reprocessar_SAFX04_{resultado.Empresa}_{DateTime.Now.Day}.csv";

                                    var response = await ApiTax.BaixarRelatorioProcessoImportacao(ApiTax.GetContext(resultado.Empresa), row, caminhoRelatorio);

                                }

                                
                            }
                        }
                        row++;
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
