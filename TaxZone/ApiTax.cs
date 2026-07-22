using Microsoft.Playwright;
using System.Text;
using System.Text.Json.Nodes;
using TaxZone.DTO;

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

        //public static TaxContext context = new();

        public ApiTax()
        {
 
        }

        public static async Task<string> GetCookie(string usuario, string senha)
        {
            var url = "https://www.onesourcetax.com/";

            REMOVIDO
            REMOVIDO

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

            return JsonNode.Parse(content)!;
        }

        public static async Task BaixarPdfAsync(string empresa, string url, string caminhoArquivo)
        {
            using HttpClient client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Get, url);

            AddHeaders(request, empresa);

            using HttpResponseMessage response = await client.SendAsync(request);
            response.EnsureSuccessStatusCode();

            byte[] bytes = await response.Content.ReadAsByteArrayAsync();

            await System.IO.File.WriteAllBytesAsync(caminhoArquivo, bytes);
        }

        /// <summary>
        /// Dispara o processo de automação fiscal baseado em uma string de entrada utilizando a sessão persistida.
        /// </summary>
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

        public static async Task SelecionaEmpresaEModulo(TaxContext context)
        {
            try
            {
                //configuration/empEstabConfig
                string url = "https://www.onesourcetax.com/amer1/oms-taxone-11/ws/configuration/empEstabConfig";

                string json_content = $$"""
                    {   "empresa":"{{Empresa.GetCodEmpresa(context.Empresa).ToString("000")}}",
                        "client":"{{Empresa.GetEmpresaTax(context.Empresa)}}",
                        "estabelecimento":"",
                        "codModLicParameter":"PROCESSOS CUSTOMIZADOS",
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

                /*
                //PerformMultiOperation
                url = "https://www.onesourcetax.com/amer1/oms-taxone-11/ws/ResumeOperation/PerformMultiOperation";

                json_content = $$$"""
                    {"menuPath":"Processos Customizados > Execução dos Processos Customizados","moduleExe":"safcp","parameters":{"targetName":"safcp","args":[["safcp2/w_processos_customizados/safgnfw1w_sheet_dw_simplesdw_sheetgetfocus",
                    "{\"vm\":\"{{{context.NewViews}}}\",\"menuPath\":\"Processos Customizados > Execução dos Processos Customizados\",\"moduleExe\":\"safcp\",\"commands\":[{\"command\":\"UPDATE_CURRENT_KEY\",\"data\":{\"key\":\"none\"}},{\"command\":\"UPDATE_DM_ROW_AND_COL\",\"data\":{\"dataManagerId\":\"37\",\"currentRow\":1,\"currentControlName\":\"compute_1\",\"displayedRowCount\":10,\"currentPage\":1}}]}","61","safcp"]]},"commands":[{"command":"UPDATE_CURRENT_KEY","data":{"key":"none"}},{"command":"UPDATE_DM_ROW_AND_COL","data":{"dataManagerId":"37","currentRow":1,"currentControlName":"compute_1","displayedRowCount":10,"currentPage":1}}],"storageID":"ed8bfa4a-6a47-4d09-b23e-1f1235bdca4d"}{"menuPath":"Processos Customizados > Execução dos Processos Customizados","moduleExe":"safcp","parameters":{"targetName":"safcp","args":[["safcp2/w_processos_customizados/safgnfw1w_sheet_dw_simplesdw_sheetgetfocus","{\"vm\":\"3d\",\"menuPath\":\"Processos Customizados > Execução dos Processos Customizados\",\"moduleExe\":\"safcp\",\"commands\":[{\"command\":\"UPDATE_CURRENT_KEY\",\"data\":{\"key\":\"none\"}},{\"command\":\"UPDATE_DM_ROW_AND_COL\",\"data\":{\"dataManagerId\":\"64\",\"currentRow\":1,\"currentControlName\":\"compute_1\",\"displayedRowCount\":10,\"currentPage\":1}}]}","61","safcp"]]},"commands":[{"command":"UPDATE_CURRENT_KEY","data":{"key":"none"}},{"command":"UPDATE_DM_ROW_AND_COL","data":{"dataManagerId":"64","currentRow":1,"currentControlName":"compute_1","displayedRowCount":10,"currentPage":1}}],
                    "storageID":"{{{context.StorageId}}}"}
                    """;

                root = await PostAsync(context.Empresa, url, json_content);
                
                
                if(root["ErrorOcurred"]?.GetValue<string>() == "true")
                {
                    string? mensagemErro = root["ExMessage"]?.GetValue<string>();

                    if (!string.IsNullOrEmpty(mensagemErro))
                        throw new Exception($"Erro ao selecionar empresa e módulo: {mensagemErro}");
                }

                */

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
                context.ProcId_t = root["MD"]?
                            .AsArray()
                            .FirstOrDefault(x => x?["UniqueID"]?
                                .GetValue<string>()?
                                .StartsWith("proc_id_t#") == true)?["UniqueID"]?
                            .GetValue<string>()?
                            .Split('#')
                            .LastOrDefault();

                context.T1 = root["MD"]?
                        .AsArray()
                        .FirstOrDefault(x => x?["UniqueID"]?
                            .GetValue<string>()?
                            .StartsWith("t_1#") == true)?["UniqueID"]?
                        .GetValue<string>()?
                        .Split('#')
                        .LastOrDefault();

                if (string.IsNullOrEmpty(context.NewViews2))
                    throw new Exception("Erro ao obter NewViews2 'w_processos_customizadoscb_executarclicked'");
 
                //safobfww_lib_proctab_frameworkselectionchangedd
                url = "https://www.onesourcetax.com/amer1/oms-taxone-11/ws/safcp2/w_lib_proc_customizado_taxbr/safobfww_lib_proctab_frameworkselectionchanged";

                json_content = $$$"""
                    {"vm":"{{{context.NewViews2}}}","menuPath":"Processos Customizados > Execução dos Processos Customizados","moduleExe":"safcp","parameters":{"oldindex":1,"newindex":1},"commands":[{"command":"UPDATE_CURRENT_KEY","data":{"key":"none"}},
                    {"command":"UPDATE_DM_ROW_AND_COL","data":{"dataManagerId":"{{{context.ProcId_t}}}","currentRow":0,"currentControlName":"","displayedRowCount":10,"currentPage":1}},{"command":"UPDATE_DM_ROW_AND_COL","data":{"dataManagerId":"c6","currentRow":0,"currentControlName":"","displayedRowCount":10,"currentPage":1}}],
                    "storageID":"{{{context.StorageId}}}"}
                        
                    """;

                root = await PostAsync(context.Empresa, url, json_content);

                context.PbAbrir = root["MD"]?
                        .AsArray()
                        .FirstOrDefault(x => x?["UniqueID"]?
                            .GetValue<string>()?
                            .StartsWith("pb_abrir#") == true)?["UniqueID"]?
                        .GetValue<string>()?
                        .Split('#')
                        .LastOrDefault();

                if (string.IsNullOrEmpty(context.PbAbrir))
                    throw new Exception("Erro ao obter PbAbrir 'safobfww_lib_proctab_frameworkselectionchangedd'");


                return;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            
        }

        public static async Task<TaxApiResponse> ProgramarRelatorio(string empresa, TaxContext context)
        {
            context.Empresa = empresa;

            try
            {
                if (string.IsNullOrEmpty(ConfigManager.Cookie))
                    throw new ArgumentException("Cookie não encontrado!");

                NotificationService.AtualizarStatusTax(
                                $"Programando relatório para {empresa}",
                                1);

                if (string.IsNullOrEmpty(context.StorageId))
                {
                    await ObterStorageId(context);
                    if (string.IsNullOrEmpty(context.StorageId)) return new TaxApiResponse(false, "Falha ao obter StorageId", empresa);

                    NotificationService.AtualizarStatusTax(
                            $"Programando relatório para {empresa}",
                            15);

                    await SelecionaEmpresaEModulo(context);
                    if (string.IsNullOrEmpty(context.StorageId)) return new TaxApiResponse(false, "Falha ao selecionar empresa e módulo", empresa); ;

                }

                NotificationService.AtualizarStatusTax(
                            $"Programando relatório para {empresa}",
                            30);

                await AbrirTelaProcessosCustomizados(context);


                NotificationService.AtualizarStatusTax(
                           $"Programando relatório para {empresa}",
                           45);

                //ConfigurarParâmetros

                await ParametrosRelatorio(empresa, context.ControlNumber, context.DataManagerId, context.StorageId, 3, param_empresa);
                await ParametrosRelatorio(empresa, context.ControlNumber, context.DataManagerId, context.StorageId, 4, param_estab);
                await ParametrosRelatorio(empresa, context.ControlNumber, context.DataManagerId, context.StorageId, 5, data_inicio);
                await ParametrosRelatorio(empresa, context.ControlNumber, context.DataManagerId, context.StorageId, 6, data_fim);

                NotificationService.AtualizarStatusTax(
                           $"Programando relatório para {empresa}",
                           60);

                if (buraco_nota == "S")
                    await ParametrosRelatorio2(empresa, context.ControlNumber, context.DataManagerId, context.ProcId_t, context.PbAbrir, context.T1, context.StorageId, 9, buraco_nota, 7);
                if (diferenca_capa_item == "S")
                    await ParametrosRelatorio2(empresa, context.ControlNumber, context.DataManagerId, context.ProcId_t, context.PbAbrir, context.T1, context.StorageId, 11, diferenca_capa_item, 9);
                if (icms_resumido == "S") 
                    await ParametrosRelatorio2(empresa, context.ControlNumber, context.DataManagerId, context.ProcId_t, context.PbAbrir, context.T1, context.StorageId, 14, icms_resumido, 12);
                if (notas_sem_item == "S")
                    await ParametrosRelatorio2(empresa, context.ControlNumber, context.DataManagerId, context.ProcId_t, context.PbAbrir, context.T1, context.StorageId, 15, notas_sem_item, 13);
                if (qtd_itens == "S")
                    await ParametrosRelatorio2(empresa, context.ControlNumber, context.DataManagerId, context.ProcId_t, context.PbAbrir, context.T1, context.StorageId, 16, qtd_itens, 14);
                if (qtd_notas == "S")
                    await ParametrosRelatorio2(empresa, context.ControlNumber, context.DataManagerId, context.ProcId_t, context.PbAbrir, context.T1, context.StorageId, 18, qtd_notas, 16);
                if (qtd_canceladas == "S")
                    await ParametrosRelatorio2(empresa, context.ControlNumber, context.DataManagerId, context.ProcId_t, context.PbAbrir, context.T1, context.StorageId, 19, qtd_canceladas, 17);
                if (extracao_canceladas == "S")
                    await ParametrosRelatorio2(empresa, context.ControlNumber, context.DataManagerId, context.ProcId_t, context.PbAbrir, context.T1, context.StorageId, 21, extracao_canceladas, 19);

                NotificationService.AtualizarStatusTax(
                           $"Programando relatório para {empresa}",
                           80);

                //Executar
                //safobfww_lib_proctab_frameworktabpage_parametrosdw_parametros_headerbuttonclicked
                string url = "https://www.onesourcetax.com/amer1/oms-taxone-11/ws/safcp2/w_lib_proc_customizado_taxbr/safobfww_lib_proctab_frameworktabpage_parametrosdw_parametros_headerbuttonclicked";

                string json_content = $$$"""
                    {"vm":"{{{context.NewViews2}}}","menuPath":"Processos Customizados > Execução dos Processos Customizados","moduleExe":"safcp",
                    "parameters":{"row":1,"dwo":"pb_executar#{{{context.Id}}}"},
                    "commands":[{"command":"UPDATE_CURRENT_KEY","data":{"key":"none"}},{"command":"UPDATE_DM_ROW_AND_COL","data":{"dataManagerId":"5e","currentRow":0,"currentControlName":"","displayedRowCount":10,"currentPage":1}},{"command":"UPDATE_DM_ROW_AND_COL","data":{"dataManagerId":"61","currentRow":0,"currentControlName":"","displayedRowCount":10,"currentPage":1}}],
                    "storageID":"{{{context.StorageId}}}"}
                    """;

                NotificationService.AtualizarStatusTax(
                           $"Programando relatório para {empresa}",
                           95);

                var root = await PostAsync(empresa, url, json_content);
                string? retorno = root["VD"]?["Commands"]?[0]?["parameters"]?["text"]?.GetValue<string>();

                return new TaxApiResponse(true, $"{retorno}", empresa);
            }
            catch (Exception ex)
            {
                return new TaxApiResponse(false, $"Falha ao executar HTTP POST: {ex.Message}", empresa);
            }
            finally
            {
                NotificationService.AtualizarStatusTax(
                           $"Finalizado",
                           100);
            }
        }

        public static async Task<bool> ObterRelatorio(F_Relatorios_Executados form, string empresa, TaxContext context, bool novo_contexto)
        {
            form.UpdateLoadingPercentage("0%");
            bool sucesso;
            string url, json_content;
            JsonNode? root;                
            context.Empresa = empresa;

            try
            {
                if (string.IsNullOrEmpty(ConfigManager.Cookie))
                    throw new ArgumentException("Cookie não encontrado!");

                if (novo_contexto)
                {
                    form.UpdateLoadingPercentage("15%");

                    await ObterStorageId(context);

                    form.UpdateLoadingPercentage("30%");

                    await SelecionaEmpresaEModulo(context);

                }

                form.UpdateLoadingPercentage("50%");

                await AbrirTelaProcessosCustomizados(context);

                form.UpdateLoadingPercentage("65%");

                url = $"https://www.onesourcetax.com/amer1/oms-taxone-11/ws/safcp2/w_lib_proc_customizado_taxbr/safobfww_lib_proctab_frameworkselectionchanged";

                json_content = $$$"""
                        {"vm":"{{{context.NewViews2}}}","menuPath":"Processos Customizados > Execução dos Processos Customizados","moduleExe":"safcp","parameters":{"oldindex":1,"newindex":2},
                        "dirty":{"tab_framework#{{{context.NewViews2}}}":{"selectedTabIndex":2}},
                        "commands":[{"command":"UPDATE_CURRENT_KEY","data":{"key":"none"}},{"command":"UPDATE_DM_ROW_AND_COL","data":{"dataManagerId":"{{{context.ProcId_t}}}","currentRow":0,"currentControlName":"","displayedRowCount":10,"currentPage":1}},
                        {"command":"UPDATE_DM_ROW_AND_COL","data":{"dataManagerId":"{{{context.T1}}}","currentRow":0,"currentControlName":"","displayedRowCount":10,"currentPage":1}}],
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

                form.UpdateLoadingPercentage("80%");

                //obter relatorios
                //url = $"https://www.onesourcetax.com/amer1/oms-taxone-11/ws/dataManagerController/getDataBundlePage?count=5&dataManagerId={context.PbAbrir}&start=1";
                url = $"https://www.onesourcetax.com/amer1/oms-taxone-11/ws/dataManagerController/getDataBundlePage?count=5&dataManagerId={id}&start=1";

                json_content = $$$"""
                    {"storageID":"{{{context.StorageId}}}"}
                    """;

                root = await PostAsync(empresa, url, json_content);

                form.UpdateLoadingPercentage("100%");

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

                form.PopulaDataGrid(context, processos);
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Falha catastrófica ao executar HTTP POST: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        public static async Task<bool> VerificaUltimoRelatorioConcluido(string empresa, TaxContext context, bool novo_contexto)
        {
            bool sucesso;
            string url, json_content;
            JsonNode? root;
            context.Empresa = empresa;

            try
            {
                if (string.IsNullOrEmpty(ConfigManager.Cookie))
                    throw new ArgumentException("Cookie não encontrado!");

                if (novo_contexto)
                {
                    await ObterStorageId(context);
                    await SelecionaEmpresaEModulo(context);
                }

                await AbrirTelaProcessosCustomizados(context);

                url = $"https://www.onesourcetax.com/amer1/oms-taxone-11/ws/safcp2/w_lib_proc_customizado_taxbr/safobfww_lib_proctab_frameworkselectionchanged";

                json_content = $$$"""
                        {"vm":"{{{context.NewViews2}}}","menuPath":"Processos Customizados > Execução dos Processos Customizados","moduleExe":"safcp","parameters":{"oldindex":1,"newindex":2},
                        "dirty":{"tab_framework#{{{context.NewViews2}}}":{"selectedTabIndex":2}},
                        "commands":[{"command":"UPDATE_CURRENT_KEY","data":{"key":"none"}},{"command":"UPDATE_DM_ROW_AND_COL","data":{"dataManagerId":"{{{context.ProcId_t}}}","currentRow":0,"currentControlName":"","displayedRowCount":10,"currentPage":1}},
                        {"command":"UPDATE_DM_ROW_AND_COL","data":{"dataManagerId":"{{{context.T1}}}","currentRow":0,"currentControlName":"","displayedRowCount":10,"currentPage":1}}],
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
                    return true;
                else
                    return false;

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Falha catastrófica ao executar HTTP POST: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        public static async Task<bool> BaixarRelatorio(TaxContext context, int row, string path = null)
        {
            try
            {
                if (string.IsNullOrEmpty(ConfigManager.Cookie))
                    throw new ArgumentException("Cookie não encontrado!");

                //safobfww_lib_proctab_frameworktabpage_processosdw_processosbuttonclicked
                string url = $"https://www.onesourcetax.com/amer1/oms-taxone-11/ws/safcp2/w_lib_proc_customizado_taxbr/safobfww_lib_proctab_frameworktabpage_processosdw_processosbuttonclicked";

                string json_content = $$$"""
                {"vm":"{{{context.NewViews2}}}","menuPath":"Processos Customizados > Execução dos Processos Customizados","moduleExe":"safcp",
                "parameters":{"row":{{{row}}},"dwo":"pb_abrir#{{{context.PbAbrir}}}"},"commands":[{"command":"UPDATE_CURRENT_KEY","data":{"key":"none"}},
                {"command":"UPDATE_DM_ROW_AND_COL","data":{"dataManagerId":"{{{context.PbAbrir}}}","currentRow":1,"currentControlName":"pb_abrir","displayedRowCount":10,"currentPage":1}},
                {"command":"UPDATE_DM_ROW_AND_COL","data":{"dataManagerId":"{{{context.T1}}}","currentRow":0,"currentControlName":"","displayedRowCount":10,"currentPage":1}}],
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
                    downloads.Add(BaixarPdfAsync(context.Empresa, urlBuraco, arquivoBuraco));

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
                    downloads.Add(BaixarPdfAsync(context.Empresa, urlItens, arquivoItens));

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

                    downloads.Add(BaixarPdfAsync(context.Empresa, urlNotas, arquivoNotas));

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

                    downloads.Add(BaixarPdfAsync(context.Empresa, urlCanceladas, arquivoCanceladas));
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

                    downloads.Add(BaixarPdfAsync(context.Empresa, urlCanceladas, arquivoCanceladas))    ;
                }
                catch (Exception ex) { }

                await Task.WhenAll(downloads);

                return true;
            }
            catch(Exception ex)
            {
                throw ex;
            }

        }


        //Chama o itemchanged para os parametros Empresa/Estabelecimento/DataInicio/DataFim
        public static async Task<string> ParametrosRelatorio(string empresa, string controlNumber, string dataManagerId, string storageID, int coluna, string valor)
        {
            //safobfwuo_lib_proc_parametrosdw_parametrositemchanged
            string url = "https://www.onesourcetax.com/amer1/oms-taxone-11/ws/safobfw/uo_lib_proc_parametros/safobfwuo_lib_proc_parametrosdw_parametrositemchanged";
            var request = new HttpRequestMessage(HttpMethod.Post, url);
            AddHeaders(request, empresa);

            string json_content = $$$"""
                                {
                   "vm":"{{{controlNumber}}}",
                   "menuPath":"Processos Customizados > Execução dos Processos Customizados",
                   "moduleExe":"safcp",
                   "parameters":{
                      "row":1,
                      "dwo":"col{{{coluna}}}#{{{dataManagerId}}}",
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
                            "dataManagerId":"{{{dataManagerId}}}",
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
                            "dataManagerId":"{{{dataManagerId}}}",
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
                "storageID":"{{{storageID}}}"
                }
                """;

            request.Content = new StringContent(json_content, Encoding.UTF8, "application/json");

            var response = await _client.SendAsync(request);
            var a = await response.Content.ReadAsStringAsync();
            var root = JsonNode.Parse(a)!;
            return a;
             
        }

        //Chama o parametrosclicked e itemchanged para todos os outros parâmetros
        public static async Task ParametrosRelatorio2(string empresa, string controlNumber, string dataManagerId, string proc_id_t, string t1_, string pb_abrir, string storageID, int coluna, string valor, int ordem)
        {
            //safobfwuo_lib_proc_parametrosdw_parametrositemchanged
            var url = "https://www.onesourcetax.com/amer1/oms-taxone-11/ws/safobfw/uo_lib_proc_parametros/safobfwuo_lib_proc_parametrosdw_parametrosclicked";
            var request = new HttpRequestMessage(HttpMethod.Post, url);
            AddHeaders(request, empresa);

            var json_content = $$$"""
                                {
                  "vm": "{{{controlNumber}}}",
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
                        "dataManagerId": "{{{pb_abrir}}}",
                        "currentRow": 0,
                        "currentControlName": "",
                        "displayedRowCount": 10,
                        "currentPage": 1
                      }
                    },
                    {
                      "command": "UPDATE_DM_ROW_AND_COL",
                      "data": {
                        "dataManagerId": "{{{t1_}}}",
                        "currentRow": 0,
                        "currentControlName": "",
                        "displayedRowCount": 10,
                        "currentPage": 1
                      }
                    },
                    {
                      "command": "UPDATE_BUNDLE_CURRENT_ROW_DELAYED",
                      "data": {
                        "dataManagerId": "{{{dataManagerId}}}",
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
                  "storageID": "{{{storageID}}}"
                }
                """;

             request.Content = new StringContent(json_content, Encoding.UTF8, "application/json");

             var response = await _client.SendAsync(request);
             var  a = await response.Content.ReadAsStringAsync();

            //safobfwuo_lib_proc_parametrosdw_parametrositemchanged
             url = "https://www.onesourcetax.com/amer1/oms-taxone-11/ws/safobfw/uo_lib_proc_parametros/safobfwuo_lib_proc_parametrosdw_parametrositemchanged";
             request = new HttpRequestMessage(HttpMethod.Post, url);
            AddHeaders(request, empresa);

             json_content = $$$"""
                                {
                  "vm": "{{{controlNumber}}}",
                  "menuPath": "Processos Customizados > Execução dos Processos Customizados",
                  "moduleExe": "safcp",
                  "parameters": {
                    "row": 1,
                      "dwo":"col{{{coluna}}}#{{{dataManagerId}}}",
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
                        "dataManagerId":"{{{pb_abrir}}}",
                        "currentRow": 0,
                        "currentControlName": "",
                        "displayedRowCount": 10,
                        "currentPage": 1
                      }
                    },
                    {
                      "command": "UPDATE_DM_ROW_AND_COL",
                      "data": {
                        "dataManagerId": "{{{t1_}}}",
                        "currentRow": 0,
                        "currentControlName": "",
                        "displayedRowCount": 10,
                        "currentPage": 1
                      }
                    }
                  ],
                  "storageID": "{{{storageID}}}"
                }
                """;

            request.Content = new StringContent(json_content, Encoding.UTF8, "application/json");

            response = await _client.SendAsync(request);
            //a = await response.Content.ReadAsStringAsync();

        }

        public async ValueTask DisposeAsync()
        {
            //if (_browser != null) await _browser.CloseAsync();
           // _playwright?.Dispose();
        }

    }
}