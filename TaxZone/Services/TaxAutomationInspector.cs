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
                }
                else
                {
                    Logger.Log(resultado.Empresa, $"JOB manual programado.");
                    empresasJobProgramado.Add(resultado.Empresa);
                }
            }

            await Task.Delay(TimeSpan.FromMinutes(5));

            ApiTax.ResetContext();

            resultados = await ConsultarJob(empresasJobProgramado, Config.UsuarioTax);
            
            empresasComErro = await ValidarErros(resultados, true);

            Environment.Exit(0);
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
                if (resultado != null && resultado.Success)
                {
                    int row = 1;

                    if(resultado.ProcessosImportacao.Count == 0)
                    {

                        Logger.Log(resultado.Empresa, $"Nenhum log encontrado.");

                        if (!empresasComErro.Contains(resultado.Empresa))
                            empresasComErro.Add(resultado.Empresa);

                        continue;
                    }

                    //Filtra para obter somente a última execução de cada relatório
                    var filtrados = resultado.ProcessosImportacao
                            .GroupBy(p => p.Descricao)
                            .Select(grupo => grupo
                                .OrderByDescending(p => p.DataIniMovto)
                                .First())
                            .ToList();

                    foreach (var log in filtrados)
                    {
                        if (log.Descricao == "IMPX431" || log.Descricao == "IMPX2013")
                            continue;

                        if (log.QtdErr > 0)
                        {
                            Logger.Log(resultado.Empresa, $"{log.Descricao} - Lidos: {log.QtdLido} / Erros: {log.QtdErr} / Inseridos: {log.QtdIns} / Ignorados: {log.QtdIgn}");

                            if (log.Descricao == "IMPX42" )
                            {
                                if (!empresasComErro.Contains(resultado.Empresa))
                                    empresasComErro.Add(resultado.Empresa);

                                if (gerarRelatorioReprocessamento)
                                {
                                    string caminhoRelatorio = $"J:\\Igor Pinheiro\\TaxOne\\Relatorio_SAFX42_{resultado.Empresa}_{DateTime.Now.Day}_{DateTime.Now.Month}.pdf";
                                    string caminhoCsv = $"J:\\Igor Pinheiro\\TaxOne\\Reprocessar_SAFX04_{resultado.Empresa}_{DateTime.Now.Day}_{DateTime.Now.Month}.csv";

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
                                    string caminhoRelatorio = $"J:\\Igor Pinheiro\\TaxOne\\Relatorio_SAFX43_{resultado.Empresa}_{DateTime.Now.Day}_{DateTime.Now.Month}.pdf";
                                    string caminhoCsv = $"J:\\Igor Pinheiro\\TaxOne\\Reprocessar_SAFX2013_{resultado.Empresa}_{DateTime.Now.Day}_{DateTime.Now.Month}";

                                    var response = await ApiTax.BaixarRelatorioProcessoImportacao(ApiTax.GetContext(resultado.Empresa), row, caminhoRelatorio);
                                    FuncoesTax.ImportarProdutos(true, null, caminhoRelatorio, caminhoCsv);

                                    File.Delete(caminhoRelatorio);
                                }
                            }
                        }
                        row++;
                    }

                }
                else
                {
                    Logger.Log("GERAL", $"Falha ao buscar logs. {resultado.Message}");
                }
            }

            return empresasComErro;
        }
    }
}
