using Microsoft.Playwright;
using System.Configuration;
using System.Diagnostics;
using System.Globalization;
using System.Net.Http.Json;
using System.Reflection;
using System.Security.Policy;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using TaxZone.DTO;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace TaxZone
{
    public class ApiTax : IAsyncDisposable
    {
        private static readonly HttpClient _client = new HttpClient();

        public static string param_empresa;
        public static string param_estab;
        public static string data_inicio;
        public static string data_fim;
        public static string buraco_nota;
        public static string diferenca_capa_item;
        public static string icms_resumido;
        public static string notas_sem_item;
        public static string qtd_itens;
        public static string qtd_notas;
        public static string qtd_canceladas;
        public static string extracao_canceladas;

        public ApiTax()
        {
 
        }

        public static async Task<string> GetCookie(string usuario, string senha)
        {
            var url = "https://www.onesourcetax.com/";

            using var playwright = await Playwright.CreateAsync();

            await using var browser = await playwright.Chromium.LaunchAsync(
                new BrowserTypeLaunchOptions
                {
                    Channel = "msedge",
                    Headless = false
                    //Headless = true
                });

            var context = await browser.NewContextAsync();
            var page = await context.NewPageAsync();

            await page.GotoAsync(url);

            await page.GetByRole(AriaRole.Textbox, new() { Name = "Username" })
                .FillAsync(usuario);

            await page.GetByRole(AriaRole.Textbox, new() { Name = "Password" })
                .FillAsync(senha);

            await page.GetByRole(AriaRole.Button, new() { Name = "Sign In" })
                .ClickAsync();

            // Aguarda navegação após login
            await page.WaitForLoadStateAsync(LoadState.NetworkIdle);

            // Caso apareça o card TAX ONE depois do login
            try
            {
                await page.GetByRole(AriaRole.Listitem, new() { Name = "TAX ONE" })
                    .ClickAsync(new() { Timeout = 5000 });

                await page.WaitForLoadStateAsync(LoadState.NetworkIdle);

                await page.GetByRole(AriaRole.Listitem, new() { Name = "001 - EMR" })
                    .ClickAsync(new() { Timeout = 5000 });
                await page.WaitForLoadStateAsync(LoadState.NetworkIdle);

            }
            catch
            {
                // Ignora caso não apareça
            }

            Thread.Sleep(3000);

            var cookies = await context.CookiesAsync();

            foreach (var cookie in cookies)
            {
                Console.WriteLine($"{cookie.Name}={cookie.Value}");
            }

            var cookieHeader = string.Join(
                "; ",
                cookies.Select(c => $"{c.Name}={c.Value}")
            );

            return cookieHeader;
        }

        public static async Task RenewCookie()
        {
            try
            {
                string url = "https://www.onesourcetax.com/amer1/home-security/api/security/v1/sessions/renew";
                using var request = new HttpRequestMessage(HttpMethod.Put, url);

                AddHeaders(request, "EMR");

                var response = await _client.SendAsync(request);

                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();

                if(content == "false") 
                    MessageBox.Show("Erro na renovação dos cookies", "Erro na renovação dos cookies", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch(Exception ex) 
            {
                MessageBox.Show(ex.Message, "Erro na renovação dos cookies", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }


        }
        public static void AddHeaders(HttpRequestMessage request, string empresa)
        {
            request.Headers.Add("Cookie", ConfigManager.Cookie);
            request.Headers.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64)");
            request.Headers.Add("Accept", "application/json, text/plain, */*");
            request.Headers.Add("x-taxautomation-tenant", Empresa.GetUsuarioTaxAutomation(empresa));
            request.Headers.Add("x-taxautomation-user", "Energisa.ips10");
            request.Headers.Add("x-lonestar-product-firmid", Empresa.GetEmpresaTax(empresa));
            request.Headers.Add("X-LoneStar-IsCMEnabled", "true");
            request.Headers.Add("Origin", "https://www.onesourcetax.com");
            
        }

        private static async Task<JsonNode> PostAsync(string empresa, string url, string? json = null)
        {
            using var request = new HttpRequestMessage(HttpMethod.Post, url);

            AddHeaders(request, empresa);

            if (json != null)
                request.Content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _client.SendAsync(request);

            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            
            if(string.IsNullOrEmpty(content))
                return "{}";

            return JsonNode.Parse(content)!;
        }

        public static async Task BaixarArquivoAsync(string empresa, string url, string caminhoArquivo)
        {
            using HttpClient client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Get, url);

            AddHeaders(request, empresa);

            using HttpResponseMessage response = await client.SendAsync(request);
            response.EnsureSuccessStatusCode();

            byte[] bytes = await response.Content.ReadAsByteArrayAsync();

            await System.IO.File.WriteAllBytesAsync(caminhoArquivo, bytes);
        }

        #region TAX AUTOMATION
        public static async Task<TaxApiResponse> ProgramarTaxAutomation(string empresa)
        {
            int index_fluxo = Empresa.GetIndexFluxoTaxAutomation(empresa);

            string url = $"https://www.onesourcetax.com/amer1/oms-mastersaf-taxautomation-11/fluxos/{index_fluxo}/executar";

            var request = new HttpRequestMessage(HttpMethod.Put, url);

            try
            {
                if (string.IsNullOrEmpty(ConfigManager.Cookie))
                    throw new ArgumentException("Cookie não encontrado!");

                AddHeaders(request, empresa);

                HttpResponseMessage response = await _client.SendAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    string respostaTexto = await response.Content.ReadAsStringAsync();
                    return new TaxApiResponse(true, $"{empresa} - Job programado com sucesso", empresa);

                }
                else
                {
                    return new TaxApiResponse(true, $"Erro na requisição: {response.StatusCode} - {response.ReasonPhrase}", empresa);
                }
            }
            catch (Exception ex)
            {
                return new TaxApiResponse(false, $"Falha ao executar HTTP POST: {ex.Message}", empresa);
            }
        }

        public static async Task<TaxApiResponse> VerificarStatusExecucao(string empresa)
        {
            int index_fluxo = Empresa.GetIndexFluxoTaxAutomation(empresa);

            string url = $"https://www.onesourcetax.com/amer1/oms-mastersaf-taxautomation-11/fluxos/{index_fluxo}/execucoes?pagina=0&tamanhoPagina=3";

            var request = new HttpRequestMessage(HttpMethod.Get, url);

            try
            {
                if (string.IsNullOrEmpty(ConfigManager.Cookie))
                    throw new ArgumentException("Cookie não encontrado!");

                AddHeaders(request, empresa);

                HttpResponseMessage response = await _client.SendAsync(request);

                if (response.IsSuccessStatusCode)
                {

                    string json = await response.Content.ReadAsStringAsync();

                    JsonNode root = JsonNode.Parse(json)!;

                    var fluxoMaisRecente = root["content"]!
                        .AsArray()
                        .OrderByDescending(x => DateTime.Parse(x!["dataAgendamento"]!.ToString()))
                        .FirstOrDefault();

                    if (fluxoMaisRecente == null)
                        return new TaxApiResponse(false, "Última execução não encontrada.", empresa);
                    
                        

                    var execucoes = fluxoMaisRecente["execucoes"]!.AsArray();

                    bool todasConcluidas = execucoes.All(execucao =>
                        execucao!["status"]?.ToString() == "COMPLETED");

                    if(todasConcluidas)
                        return new TaxApiResponse(true, "Última execução concluída com sucesso!", empresa);
                    else
                        return new TaxApiResponse(true, "A última execução ainda não foi concluída. Verifique novamente mais tarde.", empresa);

                }
                else
                {
                    return new TaxApiResponse(false, $"Erro na requisição: {response.StatusCode} - {response.ReasonPhrase}", empresa);
                }
            }
            catch (Exception ex)
            {
                return new TaxApiResponse(false, $"Falha ao executar HTTP POST: {ex.Message}", empresa);
            }
        }

        #endregion

        #region COMUM
        public static async Task ObterStorageId(TaxContext context)
        {
            try
            {
                string url = "https://www.onesourcetax.com/amer1/oms-taxone-11/ws/configuration/storageID";

                string json_content = "{\"storageID\":\"\"}";

                var root = await PostAsync(context.Empresa, url, json_content);

                context.StorageId = root["storageID"].ToString();

                if(string.IsNullOrEmpty(context.StorageId))
                    throw new Exception("Erro ao obter storageId");

            }
            catch(Exception ex)
            {
                throw new Exception($"Falha ao obter storageId:\n{ex.Message}", ex);
            }
            
        }

        public static async Task SelecionaEmpresaEModulo(TaxContext context, string modulo)
        {
            try
            {
                //Modulos: "PROCESSOS CUSTOMIZADOS", "JOB SERVIDOR"
                //configuration/empEstabConfig
                string url = "https://www.onesourcetax.com/amer1/oms-taxone-11/ws/configuration/empEstabConfig";

                string json_content = $$"""
                    {   "empresa":"{{Empresa.GetCodEmpresa(context.Empresa).ToString("000")}}",
                        "client":"{{Empresa.GetEmpresaTax(context.Empresa)}}",
                        "estabelecimento":"",
                        "codModLicParameter":"{{modulo}}",
                        "storageID":"{{context.StorageId}}"}
                    """;

                var root = await PostAsync(context.Empresa, url, json_content);

                //Abrir módulo
                //safcp/safcpsafcpopen
                url = "https://www.onesourcetax.com/amer1/oms-taxone-11/ws/safcp/safcp/safcpsafcpopen";

                json_content = $$"""
                    { "storageID":"{{context.StorageId}}"}
                    """;

                root = await PostAsync(context.Empresa, url, json_content);

                context.StorageId = root["storageID"].ToString();
                string? mensagemErro = root["Commands"]?
                    .AsArray()
                    .Select(c => c?["parameters"]?["text"]?.GetValue<string>())
                    .LastOrDefault(t => !string.IsNullOrEmpty(t));

                if (!string.IsNullOrEmpty(mensagemErro))
                    throw new Exception($"Erro ao selecionar empresa e módulo: {mensagemErro}");

            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao selecionar empresa e módulo. {ex.Message}", ex);
            }
            

        }

        #endregion

        #region PROCESSOS CUSTOMIZADOS
        public static async Task AbrirTelaProcessosCustomizados(TaxContext context)
        {
            try
            {
                //Abrir tela de processos customizados
                //safcp/m_processoscustomizadosclicked
                string url = "https://www.onesourcetax.com/amer1/oms-taxone-11/ws/safcp1/m_mdi_safcp_taxbr/m_processoscustomizadosclicked";

                string json_content = $$$"""
                    {   "vm":"a",
                        "menuPath":"Processos Customizados > Execução dos Processos Customizados",
                        "moduleExe":"safcp","commands":[{"command":"UPDATE_CURRENT_KEY","data":{"key":"none"}}],
                        "storageID":"{{{context.StorageId}}}"}
                    """;

                //Precisa chamar duas vezes para funcionar? 
                var root = await PostAsync(context.Empresa, url, json_content);
                //root = await PostAsync(context.Empresa, url, json_content);

                context.NewViews = root["VD"]?["NewViews"]?[0]?.GetValue<string>();

                if (string.IsNullOrEmpty(context.NewViews))
                    throw new Exception("Erro ao obter NewViews 'm_processoscustomizadosclicked'");


                //safcp2w_processos_customizadosdw_sheetclicked
                url = "https://www.onesourcetax.com/amer1/oms-taxone-11/ws/safcp2/w_processos_customizados/safcp2w_processos_customizadosdw_sheetclicked";

                json_content = $$$"""
                    {"vm":"{{{context.NewViews}}}","menuPath":"Processos Customizados > Execução dos Processos Customizados","moduleExe":"safcp","parameters":{"xpos":0,"ypos":0,"row":1,"dwo":""},"commands":[{"command":"UPDATE_CURRENT_KEY","data":{"key":"none"}}],
                    "storageID":"{{{context.StorageId}}}"}
                    """;

                root = await PostAsync(context.Empresa, url, json_content);

                context.UniqueId = root["MD"]?[1]?["UniqueID"]?.GetValue<string>();

                if (string.IsNullOrEmpty(context.UniqueId))
                    throw new Exception("Erro ao obter UniqueId 'safcp2w_processos_customizadosdw_sheetclicked'");
               
                
                //safcp2w_processos_customizadosdw_sheetclicked -2
                url = "https://www.onesourcetax.com/amer1/oms-taxone-11/ws/safcp2/w_processos_customizados/safcp2w_processos_customizadosdw_sheetclicked";

                json_content = $$$"""
                    {"vm":"{{{context.NewViews}}}","menuPath":"Processos Customizados > Execução dos Processos Customizados","moduleExe":"safcp",
                    "parameters":{"xpos":0,"ypos":0,"row":1,"dwo":"compute_1#{{{context.UniqueId}}}"},"commands":[{"command":"UPDATE_CURRENT_KEY","data":{"key":"none"}}],
                    "storageID":"{{{context.StorageId}}}"}
                    """;

                root = await PostAsync(context.Empresa, url, json_content);

                //w_processos_customizadoscb_executarclicked
                url = "https://www.onesourcetax.com/amer1/oms-taxone-11/ws/safcp2/w_processos_customizados/safcp2w_processos_customizadoscb_executarclicked";

                json_content = $$$"""
                    {"vm":"{{{context.NewViews}}}","menuPath":"Processos Customizados > Execução dos Processos Customizados","moduleExe":"safcp","commands":[{"command":"UPDATE_CURRENT_KEY","data":{"key":"none"}},{"command":"UPDATE_DM_ROW_AND_COL",
                    "data":{"dataManagerId":"{{{context.UniqueId}}}","currentRow":1,"currentControlName":"compute_1","displayedRowCount":10,"currentPage":1}}],
                    "storageID":"{{{context.StorageId}}}"}
                    """;

                root = await PostAsync(context.Empresa, url, json_content);

                context.NewViews2 = root["VD"]?["NewViews"]?[0]?.GetValue<string>();
                context.DataManagerId = root["VD"]?["Commands"]?[0]?["parameters"]?["dataManagerId"]?.GetValue<string>();
                string controlId = root["VD"]?["Commands"]?[2]?["parameters"]?["controlId"]?.GetValue<string>();

                context.ControlNumber = root["VD"]?["Commands"]?[2]?["parameters"]?["controlId"]?
                        .GetValue<string>()
                        .Split('#')[1];

                string uniqueId2 = root["MD"]?[2]?["UniqueID"]?.GetValue<string>();
                context.Id = uniqueId2?.Split('#').LastOrDefault();

                /*context.ProcId_t = root["MD"]?
                            .AsArray()
                            .FirstOrDefault(x => x?["UniqueID"]?
                                .GetValue<string>()?
                                .StartsWith("proc_id_t#") == true)?["UniqueID"]?
                            .GetValue<string>()?
                            .Split('#')
                            .LastOrDefault();
                */
                context.d_lib_proc_processos = root["MD"]?[35]?[0]?.GetValue<string>();

                context.d_lib_proc_lista_arquivos = root["MD"]?[56]?[0]?.GetValue<string>();
                /*
                context.ProcessosId = root["MD"]?
                        .AsArray()
                        .FirstOrDefault(x => x?["UniqueID"]?
                            .GetValue<string>()?
                            .StartsWith("t_1#") == true)?["UniqueID"]?
                        .GetValue<string>()?
                        .Split('#')
                        .LastOrDefault();
                */
                context.UniqueIdListaArquivos = root["MD"][185][0].GetValue<string>();

                //context.d_lib_proc_lista_arquivos_header_taxbr = root["MD"]?[169]?[0]?.GetValue<string>();

                //context.AbaProcessosId = context.d_lib_proc_processos;

                //preciso o uniqueid do objeto com nome d_lib_proc_lista_arquivos_header_taxbr, mas às vezes tem duas e a primeira pode ser a errada, então tem que fazer essa maracutaia
                JsonObject obj = root["MD"]!.AsArray()
                .OfType<JsonObject>()
                .FirstOrDefault(o =>
                    o["name"]?.ToString() == "genericas/safobfw/d_lib_proc_lista_arquivos_header_taxbr/d_lib_proc_lista_arquivos_header_taxbr" &&
                    o["column"] is JsonArray cols &&
                    cols.OfType<JsonObject>().Any(c => c["name"]?.ToString() == "todos"));

                context.d_lib_proc_lista_arquivos_header_taxbr = obj?["UniqueID"]?.GetValue<string>();


                if (string.IsNullOrEmpty(context.NewViews2))
                    throw new Exception("Erro ao obter NewViews2 'w_processos_customizadoscb_executarclicked'");
 
               
                return;

            }
            catch (Exception ex)
            {
                throw ex;
            }
            
        }

        public static async Task<TaxApiResponse> ProgramarRelatorio(TaxContext context, IProgress<Progresso>? progresso = null)
        {
            string modulo = "PROCESSOS CUSTOMIZADOS";

            try
            {
                if (string.IsNullOrEmpty(ConfigManager.Cookie))
                    throw new ArgumentException("Cookie não encontrado!");

                progresso?.Report(new Progresso($"Programando relatório para {context.Empresa}", 1));

                if (string.IsNullOrEmpty(context.StorageId))
                {
                    await ObterStorageId(context);
                    if (string.IsNullOrEmpty(context.StorageId)) return new TaxApiResponse(false, "Falha ao obter StorageId", context.Empresa);

                    progresso?.Report(new Progresso($"Programando relatório para {context.Empresa}", 15));
                }

                if(context.Modulo != modulo)
                {
                    await SelecionaEmpresaEModulo(context, modulo);
                    if (string.IsNullOrEmpty(context.StorageId)) return new TaxApiResponse(false, "Falha ao selecionar empresa e módulo", context.Empresa);
                }


                progresso?.Report(new Progresso($"Programando relatório para {context.Empresa}", 30));

                await AbrirTelaProcessosCustomizados(context);


                progresso?.Report(new Progresso($"Programando relatório para {context.Empresa}", 45));

                //ConfigurarParâmetros

                var downloads = new List<Task>();

                //ParametrosRelatorio provavelmente pode ser removido, em testes
                await ParametrosRelatorio2(context, 3, param_empresa,3);
                await ParametrosRelatorio2(context, 4, param_estab,4);
                await ParametrosRelatorio2(context, 5, data_inicio,5);
                await ParametrosRelatorio2(context, 6, data_fim,6);

                progresso?.Report(new Progresso($"Programando relatório para {context.Empresa}", 65));

                if (buraco_nota == "S")
                    await ParametrosRelatorio2(context, 9, buraco_nota, 7);
                if (diferenca_capa_item == "S")
                    await ParametrosRelatorio2(context, 11, diferenca_capa_item, 9);
                if (icms_resumido == "S")
                    await ParametrosRelatorio2(context,  14, icms_resumido, 12);
                if (notas_sem_item == "S")
                    await ParametrosRelatorio2(context, 15, notas_sem_item, 13);
                if (qtd_itens == "S")
                    await ParametrosRelatorio2(context, 16, qtd_itens, 14);
                if (qtd_notas == "S")
                    await ParametrosRelatorio2(context, 18, qtd_notas, 16);
                if (qtd_canceladas == "S")
                    await ParametrosRelatorio2(context, 19, qtd_canceladas, 17);
                if (extracao_canceladas == "S")
                    await ParametrosRelatorio2(context, 21, extracao_canceladas, 19);

                //await Task.WhenAll(downloads);

                progresso?.Report(new Progresso($"Programando relatório para {context.Empresa}", 80));

                //Executar
                //safobfww_lib_proctab_frameworktabpage_parametrosdw_parametros_headerbuttonclicked
                string url = "https://www.onesourcetax.com/amer1/oms-taxone-11/ws/safcp2/w_lib_proc_customizado_taxbr/safobfww_lib_proctab_frameworktabpage_parametrosdw_parametros_headerbuttonclicked";

                string json_content = $$$"""
                    {"vm":"{{{context.NewViews2}}}","menuPath":"Processos Customizados > Execução dos Processos Customizados","moduleExe":"safcp",
                    "parameters":{"row":1,"dwo":"pb_executar#{{{context.Id}}}"},
                    "commands":[{"command":"UPDATE_CURRENT_KEY","data":{"key":"none"}},{"command":"UPDATE_DM_ROW_AND_COL","data":{"dataManagerId":"5e","currentRow":0,"currentControlName":"","displayedRowCount":10,"currentPage":1}},{"command":"UPDATE_DM_ROW_AND_COL","data":{"dataManagerId":"61","currentRow":0,"currentControlName":"","displayedRowCount":10,"currentPage":1}}],
                    "storageID":"{{{context.StorageId}}}"}
                    """;

                progresso?.Report(new Progresso($"Programando relatório para {context.Empresa}", 95));

                var root = await PostAsync(context.Empresa, url, json_content);
                string? retorno = root["VD"]?["Commands"]?[0]?["parameters"]?["text"]?.GetValue<string>();

                if(string.IsNullOrEmpty(retorno))
                    return new TaxApiResponse(false, "Falha ao programar job", context.Empresa);
                else
                   return new TaxApiResponse(true, $"{retorno}", context.Empresa);
            }
            catch (Exception ex)
            {
                progresso?.Report(new Progresso($"Falha {context.Empresa}", 100));
                return new TaxApiResponse(false, $"Falha ao executar HTTP POST: {ex.Message}", context.Empresa);

            }
            finally
            {
                progresso?.Report(new Progresso($"Finalizado {context.Empresa}", 100));
            }
        }

        public static async Task<TaxApiResponse> ObterRelatorio(TaxContext context,  IProgress<Progresso>? progresso = null)
        {
            string modulo = "PROCESSOS CUSTOMIZADOS";
            progresso?.Report(new Progresso($"0%", 0));
            string url, json_content;
            JsonNode? root;               

            try
            {
                if (string.IsNullOrEmpty(ConfigManager.Cookie))
                    throw new ArgumentException("Cookie não encontrado!");

                if (string.IsNullOrEmpty(context.StorageId))
                {
                    await ObterStorageId(context);
                    if (string.IsNullOrEmpty(context.StorageId)) return new TaxApiResponse(false, "Falha ao obter StorageId", context.Empresa);

                    progresso?.Report(new Progresso($"15%", 15));

                }

                if (context.Modulo != modulo)
                {
                    await SelecionaEmpresaEModulo(context, modulo);
                    if (string.IsNullOrEmpty(context.StorageId)) return new TaxApiResponse(false, "Falha ao selecionar empresa e módulo", context.Empresa);
                }

                progresso?.Report(new Progresso($"50%", 50));

                await AbrirTelaProcessosCustomizados(context);

                progresso?.Report(new Progresso($"65%", 65));

                url = $"https://www.onesourcetax.com/amer1/oms-taxone-11/ws/safcp2/w_lib_proc_customizado_taxbr/safobfww_lib_proctab_frameworkselectionchanged";

                json_content = $$$"""
                        {"vm":"{{{context.NewViews2}}}","menuPath":"Processos Customizados > Execução dos Processos Customizados","moduleExe":"safcp","parameters":{"oldindex":1,"newindex":2},
                        "dirty":{"tab_framework#{{{context.NewViews2}}}":{"selectedTabIndex":2}},
                        "commands":[{"command":"UPDATE_CURRENT_KEY","data":{"key":"none"}},{"command":"UPDATE_DM_ROW_AND_COL","data":{"dataManagerId":"{{{context.d_lib_proc_processos}}}","currentRow":0,"currentControlName":"","displayedRowCount":10,"currentPage":1}},
                        {"command":"UPDATE_DM_ROW_AND_COL","data":{"dataManagerId":"{{{context.d_lib_proc_lista_arquivos}}}","currentRow":0,"currentControlName":"","displayedRowCount":10,"currentPage":1}}],
                        "storageID":"{{{context.StorageId}}}"}
                        """;

                root = await PostAsync(context.Empresa, url, json_content);

                string? id = root["MD"]?
                        .AsArray()
                        .FirstOrDefault(x => x?["UniqueID"]?
                            .GetValue<string>()?
                            .StartsWith("pb_abrir#") == true)?["UniqueID"]?
                        .GetValue<string>()?
                        .Split('#')
                        .LastOrDefault();

                progresso?.Report(new Progresso($"80%", 80));

                //obter relatorios
                url = $"https://www.onesourcetax.com/amer1/oms-taxone-11/ws/dataManagerController/getDataBundlePage?count=5&dataManagerId={id}&start=1";

                json_content = $$$"""
                    {"storageID":"{{{context.StorageId}}}"}
                    """;

                root = await PostAsync(context.Empresa, url, json_content);

                progresso?.Report(new Progresso($"100%", 100));

                List<ProcessoRelatorio> processos = new();

                JsonArray registros = root[3]!.AsArray();

                foreach (JsonNode? node in registros)
                {
                    JsonArray item = node!.AsArray();

                    processos.Add(new ProcessoRelatorio
                    {
                        NumProcesso = item[3]!.GetValue<int>(),
                        InicioProcessamento = item[4]!.GetValue<string>(),
                        FimProcessamento = item[5]!.GetValue<string>(),
                        Usuario = item[8]!.GetValue<string>(),
                        Status = item[9]!.GetValue<string>().ToUpper(),
                        Detalhes = item[10]?.GetValue<string>()
                    });
                }

                var retorno = new TaxApiResponse(true, "Sucesso", context.Empresa);
                retorno.ProcessosRelatorio = processos;

                //form.PopulaDataGrid(context, processos);
                return retorno;
            }
            catch (Exception ex)
            {
                progresso?.Report(new Progresso($"100%", 100));
                return new TaxApiResponse(false, $"Falha ao executar HTTP POST: {ex.Message}", context.Empresa);
            }
        }

        public static async Task<TaxApiResponse> VerificaUltimoRelatorioConcluido(string empresa, TaxContext context, bool novo_contexto)
        {

            string url, json_content;
            string modulo = "PROCESSOS CUSTOMIZADOS";
            JsonNode? root;
            context.Empresa = empresa;

            try
            {
                if (string.IsNullOrEmpty(ConfigManager.Cookie))
                    throw new ArgumentException("Cookie não encontrado!");

                if (novo_contexto)
                {
                    await ObterStorageId(context);
                    if (string.IsNullOrEmpty(context.StorageId)) return new TaxApiResponse(false, "Falha ao obter StorageId", context.Empresa);

                }

                if (context.Modulo != modulo)
                {
                    await SelecionaEmpresaEModulo(context, modulo);
                    if (string.IsNullOrEmpty(context.StorageId)) 
                        return new TaxApiResponse(false, "Falha ao selecionar empresa e módulo", context.Empresa);
                }


                await AbrirTelaProcessosCustomizados(context);

                url = $"https://www.onesourcetax.com/amer1/oms-taxone-11/ws/safcp2/w_lib_proc_customizado_taxbr/safobfww_lib_proctab_frameworkselectionchanged";

                json_content = $$$"""
                        {"vm":"{{{context.NewViews2}}}","menuPath":"Processos Customizados > Execução dos Processos Customizados","moduleExe":"safcp","parameters":{"oldindex":1,"newindex":2},
                        "dirty":{"tab_framework#{{{context.NewViews2}}}":{"selectedTabIndex":2}},
                        "commands":[{"command":"UPDATE_CURRENT_KEY","data":{"key":"none"}},{"command":"UPDATE_DM_ROW_AND_COL","data":{"dataManagerId":"{{{context.d_lib_proc_processos}}}","currentRow":0,"currentControlName":"","displayedRowCount":10,"currentPage":1}},
                        {"command":"UPDATE_DM_ROW_AND_COL","data":{"dataManagerId":"{{{context.d_lib_proc_lista_arquivos}}}","currentRow":0,"currentControlName":"","displayedRowCount":10,"currentPage":1}}],
                        "storageID":"{{{context.StorageId}}}"}
                        """;

                root = await PostAsync(empresa, url, json_content);

                string? id = root["MD"]?
                        .AsArray()
                        .FirstOrDefault(x => x?["UniqueID"]?
                            .GetValue<string>()?
                            .StartsWith("pb_abrir#") == true)?["UniqueID"]?
                        .GetValue<string>()?
                        .Split('#')
                        .LastOrDefault();

                //obter relatorios
                url = $"https://www.onesourcetax.com/amer1/oms-taxone-11/ws/dataManagerController/getDataBundlePage?count=5&dataManagerId={id}&start=1";

                json_content = $$$"""
                    {"storageID":"{{{context.StorageId}}}"}
                    """;

                root = await PostAsync(empresa, url, json_content);

                List<ProcessoRelatorio> processos = new();

                JsonArray registros = root[3]!.AsArray();

                if (registros[1]![9]!.GetValue<string>().ToUpper() == "ENCERRADO")
                    return new TaxApiResponse(true, $"Sucesso", context.Empresa) { Completed = true };
                else
                    return new TaxApiResponse(true, $"Sucesso", context.Empresa) { Completed = false};

            }
            catch (Exception ex)
            {
                return new TaxApiResponse(false, $"Falha ao executar HTTP POST: {ex.Message}", context.Empresa);
            }
        }

        public static async Task<bool> BaixarRelatorio(TaxContext context, int row, int procId, string path = null)
        {
            try
            {
                if (string.IsNullOrEmpty(ConfigManager.Cookie))
                    throw new ArgumentException("Cookie não encontrado!");

                //safobfww_lib_proctab_frameworktabpage_processosdw_processosbuttonclicked
                string url = $"https://www.onesourcetax.com/amer1/oms-taxone-11/ws/safcp2/w_lib_proc_customizado_taxbr/safobfww_lib_proctab_frameworktabpage_processosdw_processosbuttonclicked";

                string json_content = $$$"""
                {"vm":"{{{context.NewViews2}}}","menuPath":"Processos Customizados > Execução dos Processos Customizados","moduleExe":"safcp",
                "parameters":{"row":{{{row}}},"dwo":"pb_abrir#{{{context.d_lib_proc_processos}}}"},"commands":[{"command":"UPDATE_CURRENT_KEY","data":{"key":"none"}},
                {"command":"UPDATE_DM_ROW_AND_COL","data":{"dataManagerId":"{{{context.d_lib_proc_processos}}}","currentRow":1,"currentControlName":"pb_abrir","displayedRowCount":10,"currentPage":1}},
                {"command":"UPDATE_DM_ROW_AND_COL","data":{"dataManagerId":"{{{context.d_lib_proc_lista_arquivos}}}","currentRow":0,"currentControlName":"","displayedRowCount":10,"currentPage":1}}],
                "storageID":"{{{context.StorageId}}}"}
                """;

                var root = await PostAsync(context.Empresa, url, json_content);

                var md = root["MD"]!.AsArray();


                if (string.IsNullOrEmpty(path))
                {
                    using FolderBrowserDialog dialog = new FolderBrowserDialog();

                    dialog.Description = "Selecione a pasta para salvar os PDFs";

                    if (dialog.ShowDialog() != DialogResult.OK)
                        return false;

                    path = dialog.SelectedPath;
                }


                var downloads = new List<Task>();

                try
                {
                    // Índice do objeto que contém o texto
                    int indice_buraco = md
                        .Select((item, index) => new { Item = item, Index = index })
                        .First(x => x.Item?["text"]?.GetValue<string>() == "Buraco Nota")
                        .Index;

                    // Primeiro group1# após esse objeto
                    string? id_buraco = md
                        .Skip(indice_buraco + 1)
                        .Select(x => x?["UniqueID"]?.GetValue<string>())
                        .FirstOrDefault(x => x?.StartsWith("group1#") == true)?
                        .Split('#')
                        .LastOrDefault();

                    string urlBuraco = $"https://www.onesourcetax.com/amer1/oms-taxone-11/ws/dataManagerController/printDataManager?dataManagerId={id_buraco}&storageID={context.StorageId}";
                    string arquivoBuraco = Path.Combine(path, $"BURACO_{context.Empresa}.pdf");
                    downloads.Add(BaixarArquivoAsync(context.Empresa, urlBuraco, arquivoBuraco));

                }
                catch (Exception ex) { }

                try
                {
                    // Índice do objeto que contém o texto
                    int indice_itens = md
                        .Select((item, index) => new { Item = item, Index = index })
                        .First(x => x.Item?["text"]?.GetValue<string>() == "Itens por Estabelecimento")
                        .Index;

                    // Primeiro group1# após esse objeto
                    string? id_itens = md
                        .Skip(indice_itens + 1)
                        .Select(x => x?["UniqueID"]?.GetValue<string>())
                        .FirstOrDefault(x => x?.StartsWith("group1#") == true)?
                        .Split('#')
                        .LastOrDefault();

                    string urlItens = $"https://www.onesourcetax.com/amer1/oms-taxone-11/ws/dataManagerController/printDataManager?dataManagerId={id_itens}&storageID={context.StorageId}";
                    string arquivoItens = Path.Combine(path, $"ITENS_{context.Empresa}.pdf");
                    downloads.Add(BaixarArquivoAsync(context.Empresa, urlItens, arquivoItens));

                }

                catch (Exception ex) { }

                try
                {
                    int indice_notas = md
                    .Select((item, index) => new { Item = item, Index = index })
                    .First(x => x.Item?["text"]?.GetValue<string>() == "Notas Estabelecimento")
                    .Index;

                    string? id_notas = md
                        .Skip(indice_notas + 1)
                        .Select(x => x?["UniqueID"]?.GetValue<string>())
                        .FirstOrDefault(x => x?.StartsWith("group1#") == true)?
                        .Split('#')
                        .LastOrDefault();

                    string urlNotas = $"https://www.onesourcetax.com/amer1/oms-taxone-11/ws/dataManagerController/printDataManager?dataManagerId={id_notas}&storageID={context.StorageId}";
                    string arquivoNotas = Path.Combine(path, $"NOTAS_{context.Empresa}.pdf");

                    downloads.Add(BaixarArquivoAsync(context.Empresa, urlNotas, arquivoNotas));

                }
                catch (Exception ex) { }

                try
                {
                    int indice_canceladas = md
                    .Select((item, index) => new { Item = item, Index = index })
                    .First(x => x.Item?["text"]?.GetValue<string>() == "Notas Canceladas")
                    .Index;

                    string? id_canceladas = md
                        .Skip(indice_canceladas + 1)
                        .Select(x => x?["UniqueID"]?.GetValue<string>())
                        .FirstOrDefault(x => x?.StartsWith("group1#") == true)?
                        .Split('#')
                        .LastOrDefault();

                    string urlCanceladas = $"https://www.onesourcetax.com/amer1/oms-taxone-11/ws/dataManagerController/printDataManager?dataManagerId={id_canceladas}&storageID={context.StorageId}";
                    string arquivoCanceladas = Path.Combine(path, $"CANC_{context.Empresa}.pdf");

                    downloads.Add(BaixarArquivoAsync(context.Empresa, urlCanceladas, arquivoCanceladas));
                }
                catch (Exception ex) { }

                try
                {
                    int indice_icms = md
                    .Select((item, index) => new { Item = item, Index = index })
                    .First(x => x.Item?["text"]?.GetValue<string>() == "Mont. ICMS Res. Estab.")
                    .Index;

                    string? id_icms = md
                        .Skip(indice_icms + 1)
                        .Select(x => x?["UniqueID"]?.GetValue<string>())
                        .FirstOrDefault(x => x?.StartsWith("group1#") == true)?
                        .Split('#')
                        .LastOrDefault();

                    string urlCanceladas = $"https://www.onesourcetax.com/amer1/oms-taxone-11/ws/dataManagerController/printDataManager?dataManagerId={id_icms}&storageID={context.StorageId}";
                    string arquivoCanceladas = Path.Combine(path, $"ICMS_{context.Empresa}.pdf");

                    downloads.Add(BaixarArquivoAsync(context.Empresa, urlCanceladas, arquivoCanceladas))    ;
                }
                catch (Exception ex) { }

                await Task.WhenAll(downloads);

                //Se o procid for informado, baixa os arquivos zip da área de transferencia do tax
                if(procId > 0)
                {

                    url = $"https://www.onesourcetax.com/amer1/oms-taxone-11/ws/dataManagerController/getDataBundlePage?count=10&dataManagerId={context.d_lib_proc_lista_arquivos}&start=1";

                    json_content = $$$"""
                         {
                          "storageID": "{{{context.StorageId}}}"
                        }
                        """;

                    root = await PostAsync(context.Empresa, url, json_content);
                    int total_itens = root[0]?[0]?.GetValue<int>() ?? 0;

                    if (total_itens == 0) return true;

                    bool jaExisteAreaTransferencia = await BaixarAreaTransferenciaPorProcId(context, procId, path);

                    if (jaExisteAreaTransferencia) return true;

                    //trocar para aba ARQUIVOS
                    url = $"https://www.onesourcetax.com/amer1/oms-taxone-11/ws/safcp2/w_lib_proc_customizado_taxbr/safobfww_lib_proctab_frameworkselectionchanged";

                    
                    json_content = $$$"""
                    { "vm": "{{{context.NewViews2}}}",
                      "menuPath": "Processos Customizados > Execução dos Processos Customizados","moduleExe": "safcp",
                      "parameters": {"oldindex": 2,"newindex": 4},"dirty": {"tab_framework#{{{context.NewViews2}}}": {"selectedTabIndex": 4}},"commands": [{"command": "UPDATE_CURRENT_KEY","data": {"key": "none"} },
                      {"command": "UPDATE_DM_ROW_AND_COL","data": {"dataManagerId": "{{{context.d_lib_proc_processos}}}","currentRow": 1,"currentControlName": "pb_abrir","displayedRowCount": 10,"currentPage": 1}},
                      {"command": "UPDATE_DM_ROW_AND_COL","data": {"dataManagerId": "{{{context.d_lib_proc_lista_arquivos}}}","currentRow": 1,"currentControlName": "","displayedRowCount": 10,"currentPage": 1}}],
                      "storageID": "{{{context.StorageId}}}"}
                    """;

                    root = await PostAsync(context.Empresa, url, json_content);

            
                
                    url = $"https://www.onesourcetax.com/amer1/oms-taxone-11/ws/ResumeOperation/PerformMultiOperation";

                    json_content = $$$"""
                    {
                      "menuPath": "Processos Customizados > Execução dos Processos Customizados",
                      "moduleExe": "safcp",
                      "parameters": {
                        "targetName": "safcp",
                        "args": [
                          [
                            "safcp2/w_lib_proc_customizado_taxbr/safobfww_lib_proctab_frameworktabpage_arqdw_arquivos_headerclicked",
                            "{\"vm\":\"{{{context.NewViews2}}}\",\"menuPath\":\"Processos Customizados > Execução dos Processos Customizados\",\"moduleExe\":\"safcp\",\"parameters\":{\"ypos\":0,\"row\":1,\"dwo\":\"todos#{{{context.d_lib_proc_lista_arquivos_header_taxbr}}}\"},\"commands\":[{\"command\":\"UPDATE_CURRENT_KEY\",\"data\":{\"key\":\"none\"}},{\"command\":\"UPDATE_DM_ROW_AND_COL\",\"data\":{\"dataManagerId\":\"{{{context.d_lib_proc_processos}}}\",\"currentRow\":1,\"currentControlName\":\"pb_abrir\",\"displayedRowCount\":10,\"currentPage\":1}},{\"command\":\"UPDATE_DM_ROW_AND_COL\",\"data\":{\"dataManagerId\":\"{{{context.d_lib_proc_lista_arquivos}}}\",\"currentRow\":1,\"currentControlName\":\"c_selecionar\",\"displayedRowCount\":10,\"currentPage\":1}}]}",
                            "{{{context.NewViews2}}}",
                            "safcp"
                          ],
                          [
                            "safcp2/w_lib_proc_customizado_taxbr/safobfww_lib_proctab_frameworktabpage_arqdw_arquivos_headeritemchanged",
                            "{\"vm\":\"{{{context.NewViews2}}}\",\"menuPath\":\"Processos Customizados > Execução dos Processos Customizados\",\"moduleExe\":\"safcp\",\"parameters\":{\"row\":1,\"dwo\":\"todos#{{{context.d_lib_proc_lista_arquivos_header_taxbr}}}\",\"data\":\"1\"},\"commands\":[{\"command\":\"UPDATE_CURRENT_KEY\",\"data\":{\"key\":\"none\"}},{\"command\":\"UPDATE_BUNDLE_CURRENT_ROW_DELAYED\",\"data\":{\"dataManagerId\":\"4e\",\"bundle\":[{\"0\":\"char(500)\",\"1\":\"char(1)\",\"2\":\"number\",\"3\":\"char(1)\",\"4\":\"char(1)\"},{},[[{\"WM$%S\":3,\"WM$%CS\":\"11011\",\"computed\":{}},\"TAXONEDIR_ENERGISA\",\"S\",0,\"N\",\"1\"]],[\"diretorio\",\"localizacao\",\"max_size\",\"gera_sem_num_processo\",\"todos\"]],\"updatedColumns\":[5]}},{\"command\":\"UPDATE_DM_ROW_AND_COL\",\"data\":{\"dataManagerId\":\"{{{context.d_lib_proc_processos}}}\",\"currentRow\":1,\"currentControlName\":\"pb_abrir\",\"displayedRowCount\":10,\"currentPage\":1}},{\"command\":\"UPDATE_DM_ROW_AND_COL\",\"data\":{\"dataManagerId\":\"{{{context.d_lib_proc_lista_arquivos}}}\",\"currentRow\":1,\"currentControlName\":\"c_selecionar\",\"displayedRowCount\":10,\"currentPage\":1}}]}",
                            "{{{context.NewViews2}}}",
                            "safcp"
                          ]
                        ]
                      },
                      "commands": [
                        {
                          "command": "UPDATE_CURRENT_KEY",
                          "data": {
                            "key": "none"
                          }
                        },
                        {
                          "command": "UPDATE_DM_ROW_AND_COL",
                          "data": {
                            "dataManagerId": "{{{context.d_lib_proc_processos}}}",
                            "currentRow": 1,
                            "currentControlName": "pb_abrir",
                            "displayedRowCount": 10,
                            "currentPage": 1
                          }
                        },
                        {
                          "command": "UPDATE_DM_ROW_AND_COL",
                          "data": {
                            "dataManagerId": "{{{context.d_lib_proc_lista_arquivos}}}",
                            "currentRow": 1,
                            "currentControlName": "c_selecionar",
                            "displayedRowCount": 10,
                            "currentPage": 1
                          }
                        }
                      ],
                      "storageID": "{{{context.StorageId}}}"
                    }
                    """;

                    root = await PostAsync(context.Empresa, url, json_content);

                    //SALVAR ARQUIVOS SELECIONADOS
                    url = $"https://www.onesourcetax.com/amer1/oms-taxone-11/ws/safcp2/w_lib_proc_customizado_taxbr/safobfww_lib_proctab_frameworktabpage_arqdw_arquivos_headerbuttonclicked";

                    json_content = $$$"""
                    {
                      "vm": "{{{context.NewViews2}}}",
                      "menuPath": "Processos Customizados > Execução dos Processos Customizados",
                      "moduleExe": "safcp",
                      "parameters": {
                        "row": 1,
                        "dwo": "pb_salvar#{{{context.d_lib_proc_lista_arquivos_header_taxbr}}}"
                      },
                      "commands": [
                        {
                          "command": "UPDATE_CURRENT_KEY",
                          "data": {
                            "key": "none"
                          }
                        },
                        {
                          "command": "UPDATE_DM_ROW_AND_COL",
                          "data": {
                            "dataManagerId": "{{{context.d_lib_proc_processos}}}",
                            "currentRow": 1,
                            "currentControlName": "pb_abrir",
                            "displayedRowCount": 10,
                            "currentPage": 1
                          }
                        },
                        {
                          "command": "UPDATE_DM_ROW_AND_COL",
                          "data": {
                            "dataManagerId": "{{{context.d_lib_proc_lista_arquivos}}}",
                            "currentRow": 1,
                            "currentControlName": "c_selecionar",
                            "displayedRowCount": 10,
                            "currentPage": 1
                          }
                        }
                      ],
                     "storageID": "{{{context.StorageId}}}"
                    }
                    """;

                    root = await PostAsync(context.Empresa, url, json_content);

                    if (root[2]?[0]?[0]?[0]?["text"].GetValue<string>() != "Operação realizada com sucesso.")
                        return false;

                    await BaixarAreaTransferenciaPorProcId(context, procId, path); jaExisteAreaTransferencia = await BaixarAreaTransferenciaPorProcId(context, procId, path);
                }

                return true;
            }
            catch(Exception ex)
            {
                throw ex;
            }

        }

        public static async Task<bool> BaixarAreaTransferenciaPorProcId(TaxContext context, int procId, string path)
        {
            try
            {
                //ACESSAR ÁREA DE TRANSFERENCIA DE ARQUIVOS
                using HttpClient client = new HttpClient();
                string url = "https://www.onesourcetax.com/amer1/oms-taxone-11/ws/NAS/fileTransfer/files?isZipFileOnly=true";

                var request = new HttpRequestMessage(HttpMethod.Get, url);
                AddHeaders(request, context.Empresa);
                using HttpResponseMessage response = await client.SendAsync(request);
                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();

                var jsonArray = JsonNode.Parse(content)?.AsArray();

                var resultado = jsonArray?
                    .Where(node => node["name"]?.ToString().Contains(procId.ToString()) == true)
                    .GroupBy(node =>
                    {
                        var nome = node["name"]?.ToString() ?? "";

                        // Remove a extensão
                        nome = Path.GetFileNameWithoutExtension(nome);

                        // Remove tudo após o último "_"
                        int ultimoUnderscore = nome.LastIndexOf('_');
                        return ultimoUnderscore > 0
                            ? nome.Substring(0, ultimoUnderscore)
                            : nome;
                    })
                    .Select(group => group
                        .OrderByDescending(n => n["fileDate"]?.GetValue<long>() ?? 0)
                        .First())
                    .Select(node => new
                    {
                        Name = node["name"]?.ToString(),
                        HashPath = node["hashPath"]?.GetValue<long>()
                    })
                    .ToList();

                var downloads = new List<Task>();

                if (resultado.Count == 0) return false;

                foreach (var item in resultado)
                {
                    string path_ = Path.Combine(path, item.Name);
                    url = $"https://www.onesourcetax.com/amer1/oms-taxone-11/ws/NAS/fileTransfer/files/download?hash={item.HashPath}&path=Download%5C{item.Name}";
                    downloads.Add(BaixarArquivoAsync(context.Empresa, url, path_));
                    //Console.WriteLine($"Nome: {item.Name} | HashPath: {item.HashPath}");
                }
                await Task.WhenAll(downloads);

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        //Chama o itemchanged para os parametros Empresa/Estabelecimento/DataInicio/DataFim
        public static async Task ParametrosRelatorio(TaxContext context, int coluna, string valor)
        {
            //safobfwuo_lib_proc_parametrosdw_parametrositemchanged
            string url = "https://www.onesourcetax.com/amer1/oms-taxone-11/ws/safobfw/uo_lib_proc_parametros/safobfwuo_lib_proc_parametrosdw_parametrositemchanged";

            string json_content = $$$"""
                                {
                   "vm":"{{{context.ControlNumber}}}",
                   "menuPath":"Processos Customizados > Execução dos Processos Customizados",
                   "moduleExe":"safcp",
                   "parameters":{
                      "row":1,
                      "dwo":"col{{{coluna}}}#{{{context.DataManagerId}}}",
                      "data":"{{{valor}}}"
                   },
                   "commands":[
                      {
                         "command":"UPDATE_CURRENT_KEY",
                         "data":{
                            "key":"none"
                         }
                      },
                      {
                         "command":"UPDATE_DM_ROW_AND_COL",
                         "data":{
                            "dataManagerId":"{{{context.DataManagerId}}}",
                            "currentRow":0,
                            "currentControlName":"",
                            "displayedRowCount":10,
                            "currentPage":1
                         }
                      },
                      {
                         "command":"UPDATE_DM_ROW_AND_COL",
                         "data":{
                            "dataManagerId":"47",
                            "currentRow":0,
                            "currentControlName":"",
                            "displayedRowCount":10,
                            "currentPage":1
                         }
                      },
                      {
                         "command":"UPDATE_BUNDLE_CURRENT_ROW_DELAYED",
                         "data":{
                            "dataManagerId":"{{{context.DataManagerId}}}",
                            "bundle":[
                               {
                                  "0":"char(120)",
                                  "1":"char(120)",
                                  "2":"char(120)",
                                  "3":"char(120)",
                                  "4":"date",
                                  "5":"date",
                                  "6":"char(120)",
                                  "7":"char(120)",
                                  "8":"char(120)",
                                  "9":"char(120)",
                                  "10":"char(120)",
                                  "11":"char(120)",
                                  "12":"char(120)",
                                  "13":"char(120)",
                                  "14":"char(120)",
                                  "15":"char(120)",
                                  "16":"char(120)",
                                  "17":"char(120)",
                                  "18":"char(120)"
                               },
                               {

                               },
                               [
                                  [
                                     {
                                        "WM$%S":3,
                                        "WM$%CS":"1111111111111111111",
                                        "computed":{

                                        }
                                     },
                                     "S",
                                     "S",
                                     "{{{param_empresa}}}",
                                     "{{{param_estab}}}",
                                     "{{{data_inicio}}}",
                                     "{{{data_fim}}}",
                                     "{{{buraco_nota}}}",
                                     "N",
                                     "{{{diferenca_capa_item}}}",
                                     "N",
                                     "N",
                                     "{{{icms_resumido}}}",
                                     "{{{notas_sem_item}}}",
                                     "{{{qtd_itens}}}",
                                     "N",
                                     "{{{qtd_notas}}}",
                                     "{{{qtd_canceladas}}}",
                                     "N",
                                     "{{{extracao_canceladas}}}"
                                  ]
                               ],
                               [
                                  "col1",
                                  "col2",
                                  "col3",
                                  "col4",
                                  "col5",
                                  "col6",
                                  "col9",
                                  "col10",
                                  "col11",
                                  "col12",
                                  "col13",
                                  "col14",
                                  "col15",
                                  "col16",
                                  "col17",
                                  "col18",
                                  "col19",
                                  "col20",
                                  "col21"
                               ]
                            ],
                            "updatedColumns":[
                               {{{coluna}}}
                            ]
                         }
                      },
                      {
                         "command":"UPDATE_DM_ROW_AND_COL",
                         "data":{
                            "dataManagerId":"d4",
                            "currentRow":1,
                            "currentControlName":"descricao",
                            "displayedRowCount":2,
                            "currentPage":1
                         }
                      }
                   ],
                "storageID":"{{{context.StorageId}}}"
                }
                """;

            await PostAsync(context.Empresa, url, json_content);

        }

        //Chama o parametrosclicked e itemchanged para todos os outros parâmetros
        public static async Task ParametrosRelatorio2(TaxContext context, int coluna, string valor, int ordem)
        {
            //safobfwuo_lib_proc_parametrosdw_parametrositemchanged
            var url = "https://www.onesourcetax.com/amer1/oms-taxone-11/ws/safobfw/uo_lib_proc_parametros/safobfwuo_lib_proc_parametrosdw_parametrosclicked";

            var json_content = $$$"""
                                {
                  "vm": "{{{context.ControlNumber}}}",
                  "menuPath": "Processos Customizados > Execução dos Processos Customizados",
                  "moduleExe": "safcp",
                  "parameters": {
                    "xpos": 0,
                    "ypos": 0,
                    "row": 0
                  },
                  "commands": [
                    {
                      "command": "UPDATE_CURRENT_KEY",
                      "data": {
                        "key": "none"
                      }
                    },
                    {
                      "command": "UPDATE_DM_ROW_AND_COL",
                      "data": {
                        "dataManagerId": "{{{context.d_lib_proc_lista_arquivos}}}",
                        "currentRow": 0,
                        "currentControlName": "",
                        "displayedRowCount": 10,
                        "currentPage": 1
                      }
                    },
                    {
                      "command": "UPDATE_DM_ROW_AND_COL",
                      "data": {
                        "dataManagerId": "{{{context.d_lib_proc_processos}}}",
                        "currentRow": 0,
                        "currentControlName": "",
                        "displayedRowCount": 10,
                        "currentPage": 1
                      }
                    },
                    {
                      "command": "UPDATE_BUNDLE_CURRENT_ROW_DELAYED",
                      "data": {
                        "dataManagerId": "{{{context.DataManagerId}}}",
                        "bundle": [
                          {
                            "0": "char(120)",
                            "1": "char(120)",
                            "2": "char(120)",
                            "3": "char(120)",
                            "4": "date",
                            "5": "date",
                            "6": "char(120)",
                            "7": "char(120)",
                            "8": "char(120)",
                            "9": "char(120)",
                            "10": "char(120)",
                            "11": "char(120)",
                            "12": "char(120)",
                            "13": "char(120)",
                            "14": "char(120)",
                            "15": "char(120)",
                            "16": "char(120)",
                            "17": "char(120)",
                            "18": "char(120)"
                          },
                          {},
                          [
                            [
                              {
                                "WM$%S": 3,
                                "WM$%CS": "1111111111111111111",
                                "computed": {}
                              },
                              "S",
                              "S",
                                "{{{param_empresa}}}",
                                "{{{param_estab}}}",
                                "{{{data_inicio}}}",
                                "{{{data_fim}}}",
                                "{{{buraco_nota}}}",
                                "N",
                                "{{{diferenca_capa_item}}}",
                                "N",
                                "N",
                                "{{{icms_resumido}}}",
                                "{{{notas_sem_item}}}",
                                "{{{qtd_itens}}}",
                                "N",
                                "{{{qtd_notas}}}",
                                "{{{qtd_canceladas}}}",
                                "N",
                                "{{{extracao_canceladas}}}"
                            ]
                          ],
                          [
                            "col1",
                            "col2",
                            "col3",
                            "col4",
                            "col5",
                            "col6",
                            "col9",
                            "col10",
                            "col11",
                            "col12",
                            "col13",
                            "col14",
                            "col15",
                            "col16",
                            "col17",
                            "col18",
                            "col19",
                            "col20",
                            "col21"
                          ]
                        ],
                        "updatedColumns": [
                          {{{ordem}}}
                        ]
                      }
                    }
                  ],
                  "storageID": "{{{context.StorageId}}}"
                }
                """;

             await PostAsync(context.Empresa, url, json_content);

            /*
            //safobfwuo_lib_proc_parametrosdw_parametrositemchanged
             url = "https://www.onesourcetax.com/amer1/oms-taxone-11/ws/safobfw/uo_lib_proc_parametros/safobfwuo_lib_proc_parametrosdw_parametrositemchanged";

             json_content = $$$"""
                                {
                  "vm": "{{{context.ControlNumber}}}",
                  "menuPath": "Processos Customizados > Execução dos Processos Customizados",
                  "moduleExe": "safcp",
                  "parameters": {
                    "row": 1,
                      "dwo":"col{{{coluna}}}#{{{context.DataManagerId}}}",
                      "data":"{{{valor}}}"
                  },
                  "commands": [
                    {
                      "command": "UPDATE_CURRENT_KEY",
                      "data": {
                        "key": "none"
                      }
                    },
                    {
                      "command": "UPDATE_DM_ROW_AND_COL",
                      "data": {
                        "dataManagerId":"{{{context.d_lib_proc_lista_arquivos}}}",
                        "currentRow": 0,
                        "currentControlName": "",
                        "displayedRowCount": 10,
                        "currentPage": 1
                      }
                    },
                    {
                      "command": "UPDATE_DM_ROW_AND_COL",
                      "data": {
                        "dataManagerId": "{{{context.d_lib_proc_processos}}}",
                        "currentRow": 0,
                        "currentControlName": "",
                        "displayedRowCount": 10,
                        "currentPage": 1
                      }
                    }
                  ],
                  "storageID": "{{{context.StorageId}}}"
                }
                """;

            await PostAsync(context.Empresa, url, json_content);
            */
        }

        #endregion

        #region LOGS PROCESSOS IMPORTACAO
        public static async Task<TaxApiResponse> ObterLogsProcessosImportacao(TaxContext context, ParametrosRelatorioImportacao parametros, IProgress<Progresso>? progresso = null)
        {
            try
            {
                string modulo = "JOB SERVIDOR";

                if (string.IsNullOrEmpty(ConfigManager.Cookie))
                    throw new ArgumentException("Cookie não encontrado!");

                if (string.IsNullOrEmpty(context.StorageId))
                {
                    await ObterStorageId(context);
                    if (string.IsNullOrEmpty(context.StorageId)) return new TaxApiResponse(false, "Falha ao obter StorageId", context.Empresa);

                    progresso?.Report(new Progresso($"15%", 15));
                    
                }

                if (context.Modulo != modulo)
                {
                    await SelecionaEmpresaEModulo(context, modulo);
                    if (string.IsNullOrEmpty(context.StorageId))
                        return new TaxApiResponse(false, "Falha ao selecionar empresa e módulo", context.Empresa);
                }

                progresso?.Report(new Progresso($"30%", 30));

                string url = "https://www.onesourcetax.com/amer1/oms-taxone-11/ws/ResumeOperation/prepareStartupApp";

                string json_content = $$$"""
                        {
                      "storageID": "{{{context.StorageId}}}"
                    }
                    """;
                //Não tem retorno
                await PostAsync(context.Empresa, url, json_content);

                progresso?.Report(new Progresso($"40%", 40));

                url = "https://www.onesourcetax.com/amer1/oms-taxone-11/ws/safilcm1/safil/safilcm1safilopen";

                //Reaproveita o json anterior
                var root = await PostAsync(context.Empresa, url, json_content);

                progresso?.Report(new Progresso($"50%", 50));

                //Abrir tela Controles>Relatórios>Importação
                url = "https://www.onesourcetax.com/amer1/oms-taxone-11/ws/safilcm2/m_mdi_safil/m_importacaonovaclicked";

                json_content = $$$"""
                        {"vm": "a","menuPath": "Controles > Relatórios > Relatório por Processo > Importação","moduleExe": "safil","commands": [{"command": "UPDATE_CURRENT_KEY","data": {"key": "none"}}],
                        "storageID": "{{{context.StorageId}}}"}
                    """;

                root = await PostAsync(context.Empresa, url, json_content);

                progresso?.Report(new Progresso($"60%", 60));

                context.NewViews = root["VD"]?["NewViews"]?[0]?.GetValue<string>();
                context.DataManagerId = root["VD"]?["Commands"]?[0]?["parameters"]?["dataManagerId"]?.GetValue<string>();

                JsonObject obj = root["MD"]!.AsArray()
                .OfType<JsonObject>()
                .FirstOrDefault(o =>
                    o["name"]?.ToString() == "safil/safilcm3/d_consulta_rel_proc_imp_grid/d_consulta_rel_proc_imp_grid")
                    ;

                context.d_consulta_rel_proc_imp_grid = obj?["UniqueID"]?.GetValue<string>();

                var downloads = new List<Task>();

                downloads.Add(ParametroLogProcessoImportacao(context, "dat_inicio", parametros.DataInicio.ToString("dd-MM-yyyy"), 2, parametros));
                downloads.Add(ParametroLogProcessoImportacao(context, "dat_fim", parametros.DataFim.ToString("dd-MM-yyyy"), 3, parametros));
                downloads.Add(ParametroLogProcessoImportacao(context, "ind_situacao", parametros.Status, 8, parametros));

                if (!string.IsNullOrEmpty(parametros.Usuario))
                    downloads.Add(ParametroLogProcessoImportacao(context, "usuario", parametros.Usuario, 6, parametros));
                
                if (!string.IsNullOrEmpty(parametros.Estabelecimento))
                    downloads.Add(ParametroLogProcessoImportacao(context, "cod_estab", parametros.Estabelecimento, 8, parametros));

                if (!string.IsNullOrEmpty(parametros.Descricao))
                    downloads.Add(ParametroLogProcessoImportacao(context, "descricao", parametros.Descricao, 9, parametros));
                await Task.WhenAll(downloads);

                progresso?.Report(new Progresso($"80%", 80));

                //Botão pesquisar
                url = "https://www.onesourcetax.com/amer1/oms-taxone-11/ws/safilcm3/w_consulta_rel_proc_imp/safilcm3w_consulta_rel_proc_impcb_pesquisarclicked";

                json_content = $$$"""
                      {"vm": "{{{context.NewViews}}}",
                      "menuPath": "Controles > Relatórios > Relatório por Processo > Importação",
                      "moduleExe": "safil",
                      "commands": [
                        {
                          "command": "UPDATE_CURRENT_KEY",
                          "data": {
                            "key": "none"
                          }
                        },
                        {
                          "command": "UPDATE_DM_ROW_AND_COL",
                          "data": {
                            "dataManagerId": "{{{context.DataManagerId}}}",
                            "currentRow": 0,
                            "currentControlName": "",
                            "displayedRowCount": 0,
                            "currentPage": 1
                          }
                        }
                      ],
                      "storageID": "{{{context.StorageId}}}"
                    }
                    """;

                root = await PostAsync(context.Empresa, url, json_content);

                progresso?.Report(new Progresso($"90%", 90));

                //Recuperar dados
                url = $"https://www.onesourcetax.com/amer1/oms-taxone-11/ws/dataManagerController/getDataBundlePage?count=44&dataManagerId={context.d_consulta_rel_proc_imp_grid}&start=1";

                json_content = $$$"""
                    {
                      "storageID": "{{{context.StorageId}}}"
                    }
                    """;

                root = await PostAsync(context.Empresa, url, json_content);


                List<ProcessoImportacao> processos = new();

                foreach (JsonArray linha in root[3]!.AsArray())
                {

                    processos.Add(new ProcessoImportacao
                    {
                        NumProcesso = linha[2]!.GetValue<int>(),
                        CodEmpresa = linha[3]!.GetValue<string>(),
                        CodEstab = linha[4]?.GetValue<string>(),
                        //IndProcesso = linha[5]!.GetValue<string>(),
                        Status = linha[6]!.GetValue<string>(),
                        CodUsuario = linha[8]!.GetValue<string>(),
                        Descricao = linha[9]!.GetValue<string>(),
                        QtdLido = linha[13]!.GetValue<int>(),
                        QtdIns = linha[14]!.GetValue<int>(),
                        QtdAlt = linha[15]!.GetValue<int>(),
                        QtdIgn = linha[16]!.GetValue<int>(),
                        QtdErr = linha[17]!.GetValue<int>(),
                        DataIni = DateOnly.ParseExact(linha[7]!.GetValue<string>(),"ddMMyyyyHHmmss", CultureInfo.InvariantCulture),
                        DataFim = DateOnly.ParseExact(linha[10]!.GetValue<string>(), "ddMMyyyyHHmmss", CultureInfo.InvariantCulture),
                        DataIniMovto = linha[11] is null ? DateOnly.MaxValue : DateOnly.ParseExact(linha[11]!.GetValue<string>(), "ddMMyyyyHHmmss", CultureInfo.InvariantCulture),
                        DataFimMovto = linha[12] is null ? DateOnly.MaxValue: DateOnly.ParseExact(linha[12]!.GetValue<string>(), "ddMMyyyyHHmmss", CultureInfo.InvariantCulture),

                    });
                }
                var response = new TaxApiResponse(true, $"Sucesso", context.Empresa);

                response.ProcessosImportacao = processos;

                progresso?.Report(new Progresso($"100%", 100));

                return response;

            }
            catch (Exception ex)
            {
                return new TaxApiResponse(false, $"Falha ao executar HTTP POST: {ex.Message}", context.Empresa);
                progresso?.Report(new Progresso($"100%", 100));
            }
        }

        public static async Task ParametroLogProcessoImportacao(TaxContext context, string dwo, string value, int index, ParametrosRelatorioImportacao parametros)
        {
            try
            {
                string usuario = "null";
                string estabelecimento = "null";
                string descricao = "null";

                if (!string.IsNullOrEmpty(parametros.Usuario))
                    usuario = $"\"{parametros.Usuario}\"";

                if (!string.IsNullOrEmpty(parametros.Estabelecimento))
                    estabelecimento = $"\"{parametros.Estabelecimento}\"";

                if (!string.IsNullOrEmpty(parametros.Descricao))
                    descricao = $"\"{parametros.Descricao}\"";



                string url = "https://www.onesourcetax.com/amer1/oms-taxone-11/ws/safilcm3/w_consulta_rel_proc_imp/safgnfw1w_sheet_dw_simplesdw_sheetclicked";

                 string json_content = $$$"""
                       {
                      "vm": "{{{context.NewViews}}}",
                      "menuPath": "Controles > Relatórios > Relatório por Processo > Importação",
                      "moduleExe": "safil",
                      "parameters": {
                        "xpos": 0,
                        "ypos": 0,
                        "row": 1,
                        "dwo": "{{{dwo}}}#{{{context.DataManagerId}}}"
                      },
                      "commands": [
                        {
                          "command": "UPDATE_CURRENT_KEY",
                          "data": {
                            "key": "none"
                          }
                        },
                        {
                          "command": "UPDATE_BUNDLE_CURRENT_ROW_DELAYED",
                          "data": {
                            "dataManagerId": "{{{context.DataManagerId}}}",
                            "bundle": [
                              {
                                "0": "number",
                                "1": "date",
                                "2": "date",
                                "3": "char(1)",
                                "4": "number",
                                "5": "char(100)",
                                "6": "char(3)",
                                "7": "char(6)",
                                "8": "char(8)"
                              },
                              {},
                              [
                                [
                                  {
                                    "WM$%S": 2,
                                    "WM$%CS": "000000000",
                                    "computed": {}
                                  },
                                  null,
                                  "{{{parametros.DataInicio.ToString("ddMMyyyy000000")}}}",
                                  "{{{parametros.DataFim.ToString("ddMMyyyy000000")}}}",
                                  "{{{parametros.Status}}}",
                                  null,
                                  {{{usuario}}},
                                  null,
                                  {{{estabelecimento}}},
                                  {{{descricao}}}
                                ]
                              ],
                              [
                                "num_proc",
                                "dat_inicio",
                                "dat_fim",
                                "ind_situacao",
                                "num_proc_fim",
                                "usuario",
                                "cod_empresa",
                                "cod_estab",
                                "descricao"
                              ]
                            ],
                            "updatedColumns": [
                              {{{index}}}
                            ]
                          }
                        },
                        {
                          "command": "UPDATE_DM_ROW_AND_COL",
                          "data": {
                            "dataManagerId": "{{{context.d_consulta_rel_proc_imp_grid}}}",
                            "currentRow": 0,
                            "currentControlName": "",
                            "displayedRowCount": 0,
                            "currentPage": 1
                          }
                        }
                      ],
                      "storageID": "{{{context.StorageId}}}"
                    }
                    """;
                
                var root = await PostAsync(context.Empresa, url, json_content);
                

                url = "https://www.onesourcetax.com/amer1/oms-taxone-11/ws/safilcm3/w_consulta_rel_proc_imp/safilcm3w_consulta_rel_proc_impdw_sheetitemchanged";

                json_content = $$$"""
                       {
                      "vm": "{{{context.NewViews}}}",
                      "menuPath": "Controles > Relatórios > Relatório por Processo > Importação",
                      "moduleExe": "safil",
                      "parameters": {
                        "row": 1,
                        "dwo": "{{{dwo}}}#{{{context.DataManagerId}}}",
                        "data": "{{{value}}}"
                      },
                      "commands": [
                        {
                          "command": "UPDATE_CURRENT_KEY",
                          "data": {
                            "key": "none"
                          }
                        },
                        {
                          "command": "UPDATE_DM_ROW_AND_COL",
                          "data": {
                            "dataManagerId": "{{{context.d_consulta_rel_proc_imp_grid}}}",
                            "currentRow": 0,
                            "currentControlName": "",
                            "displayedRowCount": 0,
                            "currentPage": 1
                          }
                        }
                      ],
                      "storageID": "{{{context.StorageId}}}"
                    }
                    """;

                root = await PostAsync(context.Empresa, url, json_content);
                
            }
            catch (Exception ex)
            {
                throw new Exception($"Falha ao executar ParametroRelatorioImportacao: {ex.Message}");
            }
        }

        public static async Task<TaxApiResponse> BaixarRelatorioProcessoImportacao(TaxContext context, int row, string path)
        {
            if (string.IsNullOrEmpty(ConfigManager.Cookie))
                throw new ArgumentException("Cookie não encontrado!");

            //safobfww_lib_proctab_frameworktabpage_processosdw_processosbuttonclicked
            string url = $"https://www.onesourcetax.com/amer1/oms-taxone-11/ws/safilcm3/w_consulta_rel_proc_imp/safilcm3w_consulta_rel_proc_impdw_importacaoitemchanged";

            string json_content = $$$"""
                {"vm":"{{{context.NewViews}}}","menuPath":"Controles > Relatórios > Relatório por Processo > Importação","moduleExe":"safil",
                "parameters":{"row":{{{row}}},"dwo":"acao#{{{context.d_consulta_rel_proc_imp_grid}}}","data":"1"},
                "commands":[{"command":"UPDATE_CURRENT_KEY","data":{"key":"none"}},{"command":"UPDATE_BUNDLE_DELAYED",
                "data":{"dataManagerId":"{{{context.d_consulta_rel_proc_imp_grid}}}","updatedRows":[1],"bundle":[{"0":"char(1)","1":"decimal(0)","2":"char(3)","3":"char(6)","4":"char(3)","5":"char(22)","6":"datetime","7":"char(100)","8":"char(8)","9":"datetime","10":"datetime","11":"datetime","12":"decimal(0)","13":"decimal(0)","14":"decimal(0)","15":"decimal(0)","16":"decimal(0)"},{},
                [[{"WM$%S":0,"computed":{}},"1",1272607,"191",null,"IMP","Finalizado com sucesso","28072026090654","Energisa.ips10","IMPX431","28072026090726","01011900000000","28072026000000",67149,66171,0,978,0]],["acao","num_processo","cod_empresa","cod_estab","ind_processo","status","data_ini","cod_usuario","descricao","data_fim","data_ini_movto","data_fim_movto","qtd_lido","qtd_ins","qtd_alt","qtd_ign","qtd_err"]],"dirtyColumns":"@1:1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,16,17,"}},
                {"command":"UPDATE_DM_ROW_AND_COL","data":{"dataManagerId":"{{{context.d_consulta_rel_proc_imp_grid}}}","currentRow":1,"currentControlName":"acao","displayedRowCount":4,"currentPage":1}}],
                "storageID":"{{{context.StorageId}}}"}
                """;

            var root = await PostAsync(context.Empresa, url, json_content);

            string? id = root["MD"]?
                .AsArray()
                .FirstOrDefault()?["UniqueID"]?
                .GetValue<string>()?
                .Split('#')
                .Last();

            url = $"https://www.onesourcetax.com/amer1/oms-taxone-11/ws/dataManagerController/printDataManager?dataManagerId={id}&storageID={context.StorageId}";

            

            await BaixarArquivoAsync(context.Empresa, url, path);

            return new TaxApiResponse(true, "Relatório baixado com sucesso", context.Empresa);

        }
        #endregion

        public async ValueTask DisposeAsync()
        {
            //if (_browser != null) await _browser.CloseAsync();
           // _playwright?.Dispose();
        }

    }
}