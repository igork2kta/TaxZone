//using Microsoft.Playwright;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Reflection;
using System.Security.Policy;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace TaxZone
{
    public class ApiTax : IAsyncDisposable
    {
        private static readonly HttpClient _client = new HttpClient();

        public static string param_empresa = "*";
        public static string param_estab = "*";
        public static string data_inicio = "01072026000000";
        public static string data_fim = "13072026000000";
        public static string buraco_nota = "N";
        public static string diferenca_capa_item = "N";
        public static string icms_resumido = "N";
        public static string notas_sem_item = "N";
        public static string qtd_itens = "N";
        public static string qtd_notas = "N";
        public static string qtd_canceladas = "N";
        public static string extracao_canceladas = "N";


        public ApiTax()
        {
 
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

        /// <summary>
        /// Dispara o processo de automação fiscal baseado em uma string de entrada utilizando a sessão persistida.
        /// </summary>
        public static async Task ProgramarTaxAutomation(string empresa)
        {
            int index_fluxo = Empresa.GetIndexFluxoTaxAutomation(empresa);

            string url = $"https://www.onesourcetax.com/amer1/oms-mastersaf-taxautomation-11/fluxos/{index_fluxo}/executar";

            var request = new HttpRequestMessage(HttpMethod.Put, url);

            if (string.IsNullOrEmpty(ConfigManager.Cookie))
            {
                MessageBox.Show("Cookie não encontrado!", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            AddHeaders(request, empresa);

            try
            {
                HttpResponseMessage response = await _client.SendAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    string respostaTexto = await response.Content.ReadAsStringAsync();
                    MessageBox.Show($"Requisição enviada! Resposta: {respostaTexto}", "Sucesso!", MessageBoxButtons.OK, MessageBoxIcon.Information);

                }
                else
                {
                    MessageBox.Show($"Erro na requisição: {response.StatusCode} - {response.ReasonPhrase}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Falha catastrófica ao executar HTTP POST: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public static async Task VerificarStatusExecucao(string empresa)
        {
            int index_fluxo = Empresa.GetIndexFluxoTaxAutomation(empresa);

            string url = $"https://www.onesourcetax.com/amer1/oms-mastersaf-taxautomation-11/fluxos/{index_fluxo}/execucoes?pagina=0&tamanhoPagina=3";

            var request = new HttpRequestMessage(HttpMethod.Get, url);

            if (string.IsNullOrEmpty(ConfigManager.Cookie))
            {
                MessageBox.Show("Cookie não encontrado!", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            AddHeaders(request, empresa);

            try
            {
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
                    {
                        MessageBox.Show("Última execução não encontrada.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                        

                    var execucoes = fluxoMaisRecente["execucoes"]!.AsArray();

                    bool todasConcluidas = execucoes.All(execucao =>
                        execucao!["status"]?.ToString() == "COMPLETED");

                    if(todasConcluidas)
                        MessageBox.Show("Última execução concluída com sucesso!", "Sucesso!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    else
                        MessageBox.Show("A última execução ainda não foi concluída. Verifique novamente mais tarde.", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                }
                else
                {
                    MessageBox.Show($"Erro na requisição: {response.StatusCode} - {response.ReasonPhrase}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Falha catastrófica ao executar HTTP POST: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public static async Task ProgramarJob(string empresa)
        {
            if (string.IsNullOrEmpty(ConfigManager.Cookie))
            {
                MessageBox.Show("Cookie não encontrado!", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                string json_content;
                //string json_response;

                //Obter storage ID
                string url = "https://www.onesourcetax.com/amer1/oms-taxone-11/ws/configuration/storageID";

                json_content = "{\"storageID\":\"\"}";

                var root = await PostAsync(empresa, url, json_content);

                string storageID = root["storageID"].ToString();

                //Abrir empresa
                //configuration/empEstabConfig
                url = "https://www.onesourcetax.com/amer1/oms-taxone-11/ws/configuration/empEstabConfig";

                json_content = $$"""
                    {   "empresa":"{{Empresa.GetCodEmpresa(empresa).ToString("000")}}",
                        "client":"{{Empresa.GetEmpresaTax(empresa)}}",
                        "estabelecimento":"",
                        "codModLicParameter":"PROCESSOS CUSTOMIZADOS",
                        "storageID":"{{storageID}}"}
                    """;

                root = await PostAsync(empresa, url, json_content);

                storageID = root["storageID"].ToString();

                //Abrir módulo
                //safcp/safcpsafcpopen
                url = "https://www.onesourcetax.com/amer1/oms-taxone-11/ws/safcp/safcp/safcpsafcpopen";

                json_content = $$"""
                    { "storageID":"{{storageID}}"}
                    """;

                root = await PostAsync(empresa, url, json_content);

                storageID = root["storageID"].ToString();
                string? mensagemErro = root["Commands"]?
                    .AsArray()
                    .Select(c => c?["parameters"]?["text"]?.GetValue<string>())
                    .LastOrDefault(t => !string.IsNullOrEmpty(t));

                if (!string.IsNullOrEmpty(mensagemErro))
                {
                    MessageBox.Show(mensagemErro, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }


                //Abrir tela de processos customizados
                //safcp/m_processoscustomizadosclicked
                url = "https://www.onesourcetax.com/amer1/oms-taxone-11/ws/safcp1/m_mdi_safcp_taxbr/m_processoscustomizadosclicked";

                json_content = $$$"""
                    {   "vm":"a",
                        "menuPath":"Processos Customizados > Execução dos Processos Customizados",
                        "moduleExe":"safcp","commands":[{"command":"UPDATE_CURRENT_KEY","data":{"key":"none"}}],
                        "storageID":"{{{storageID}}}"}
                    """;

                //Precisa chamar duas vezes para funcionar?
                root = await PostAsync(empresa, url, json_content);
                root = await PostAsync(empresa, url, json_content);

                string newViews = root["VD"]?["NewViews"]?[0]?.GetValue<string>();

                //PerformMultiOperation
                url = "https://www.onesourcetax.com/amer1/oms-taxone-11/ws/ResumeOperation/PerformMultiOperation";

                json_content = $$$"""
                    {"menuPath":"Processos Customizados > Execução dos Processos Customizados","moduleExe":"safcp","parameters":{"targetName":"safcp","args":[["safcp2/w_processos_customizados/safgnfw1w_sheet_dw_simplesdw_sheetgetfocus",
                    "{\"vm\":\"{{{newViews}}}\",\"menuPath\":\"Processos Customizados > Execução dos Processos Customizados\",\"moduleExe\":\"safcp\",\"commands\":[{\"command\":\"UPDATE_CURRENT_KEY\",\"data\":{\"key\":\"none\"}},{\"command\":\"UPDATE_DM_ROW_AND_COL\",\"data\":{\"dataManagerId\":\"64\",\"currentRow\":1,\"currentControlName\":\"compute_1\",\"displayedRowCount\":10,\"currentPage\":1}}]}","61","safcp"]]},"commands":[{"command":"UPDATE_CURRENT_KEY","data":{"key":"none"}},{"command":"UPDATE_DM_ROW_AND_COL","data":{"dataManagerId":"64","currentRow":1,"currentControlName":"compute_1","displayedRowCount":10,"currentPage":1}}],"storageID":"ed8bfa4a-6a47-4d09-b23e-1f1235bdca4d"}{"menuPath":"Processos Customizados > Execução dos Processos Customizados","moduleExe":"safcp","parameters":{"targetName":"safcp","args":[["safcp2/w_processos_customizados/safgnfw1w_sheet_dw_simplesdw_sheetgetfocus","{\"vm\":\"3d\",\"menuPath\":\"Processos Customizados > Execução dos Processos Customizados\",\"moduleExe\":\"safcp\",\"commands\":[{\"command\":\"UPDATE_CURRENT_KEY\",\"data\":{\"key\":\"none\"}},{\"command\":\"UPDATE_DM_ROW_AND_COL\",\"data\":{\"dataManagerId\":\"64\",\"currentRow\":1,\"currentControlName\":\"compute_1\",\"displayedRowCount\":10,\"currentPage\":1}}]}","61","safcp"]]},"commands":[{"command":"UPDATE_CURRENT_KEY","data":{"key":"none"}},{"command":"UPDATE_DM_ROW_AND_COL","data":{"dataManagerId":"64","currentRow":1,"currentControlName":"compute_1","displayedRowCount":10,"currentPage":1}}],
                    "storageID":"{{{storageID}}}"}
                    """;

                root = await PostAsync(empresa, url, json_content);

                //safcp2w_processos_customizadosdw_sheetclicked
                url = "https://www.onesourcetax.com/amer1/oms-taxone-11/ws/safcp2/w_processos_customizados/safcp2w_processos_customizadosdw_sheetclicked";

                json_content = $$$"""
                    {"vm":"{{{newViews}}}","menuPath":"Processos Customizados > Execução dos Processos Customizados","moduleExe":"safcp","parameters":{"xpos":0,"ypos":0,"row":1,"dwo":""},"commands":[{"command":"UPDATE_CURRENT_KEY","data":{"key":"none"}}],
                    "storageID":"{{{storageID}}}"}
                    """;

                root = await PostAsync(empresa, url, json_content);

                string? uniqueId = root["MD"]?[1]?["UniqueID"]?.GetValue<string>();

                //safcp2w_processos_customizadosdw_sheetclicked -2
                url = "https://www.onesourcetax.com/amer1/oms-taxone-11/ws/safcp2/w_processos_customizados/safcp2w_processos_customizadosdw_sheetclicked";

                json_content = $$$"""
                    {"vm":"{{{newViews}}}","menuPath":"Processos Customizados > Execução dos Processos Customizados","moduleExe":"safcp",
                    "parameters":{"xpos":0,"ypos":0,"row":1,"dwo":"compute_1#{{{uniqueId}}}"},"commands":[{"command":"UPDATE_CURRENT_KEY","data":{"key":"none"}}],
                    "storageID":"{{{storageID}}}"}
                    """;

                root = await PostAsync(empresa, url, json_content);

                //w_processos_customizadoscb_executarclicked
                url = "https://www.onesourcetax.com/amer1/oms-taxone-11/ws/safcp2/w_processos_customizados/safcp2w_processos_customizadoscb_executarclicked";

                json_content = $$$"""
                    {"vm":"{{{newViews}}}","menuPath":"Processos Customizados > Execução dos Processos Customizados","moduleExe":"safcp","commands":[{"command":"UPDATE_CURRENT_KEY","data":{"key":"none"}},{"command":"UPDATE_DM_ROW_AND_COL",
                    "data":{"dataManagerId":"{{{uniqueId}}}","currentRow":1,"currentControlName":"compute_1","displayedRowCount":10,"currentPage":1}}],
                    "storageID":"{{{storageID}}}"}
                    """;

                root = await PostAsync(empresa, url, json_content);

                string newViews2 = root["VD"]?["NewViews"]?[0]?.GetValue<string>();
                string? dataManagerId = root["VD"]?["Commands"]?[0]?["parameters"]?["dataManagerId"]?.GetValue<string>();
                string? controlId = root["VD"]?["Commands"]?[2]?["parameters"]?["controlId"]?.GetValue<string>();
                string? controlNumber = root["VD"]?["Commands"]?[2]?["parameters"]?["controlId"]?
                        .GetValue<string>()
                        .Split('#')[1];
                string? uniqueId2 = root["MD"]?[2]?["UniqueID"]?.GetValue<string>();
                string? id = uniqueId2?.Split('#').LastOrDefault();
                string? proc_id_t = root["MD"]?
                            .AsArray()
                            .FirstOrDefault(x => x?["UniqueID"]?
                                .GetValue<string>()?
                                .StartsWith("proc_id_t#") == true)?["UniqueID"]?
                            .GetValue<string>()?
                            .Split('#')
                            .LastOrDefault();


                string? t1_ = root["MD"]?
                        .AsArray()
                        .FirstOrDefault(x => x?["UniqueID"]?
                            .GetValue<string>()?
                            .StartsWith("t_1#") == true)?["UniqueID"]?
                        .GetValue<string>()?
                        .Split('#')
                        .LastOrDefault();

                //safobfww_lib_proctab_frameworkselectionchangedd
                url = "https://www.onesourcetax.com/amer1/oms-taxone-11/ws/safcp2/w_lib_proc_customizado_taxbr/safobfww_lib_proctab_frameworkselectionchanged";

                json_content = $$$"""
                    {"vm":"{{{newViews2}}}","menuPath":"Processos Customizados > Execução dos Processos Customizados","moduleExe":"safcp","parameters":{"oldindex":1,"newindex":1},"commands":[{"command":"UPDATE_CURRENT_KEY","data":{"key":"none"}},
                    {"command":"UPDATE_DM_ROW_AND_COL","data":{"dataManagerId":"{{{proc_id_t}}}","currentRow":0,"currentControlName":"","displayedRowCount":10,"currentPage":1}},{"command":"UPDATE_DM_ROW_AND_COL","data":{"dataManagerId":"c6","currentRow":0,"currentControlName":"","displayedRowCount":10,"currentPage":1}}],
                    "storageID":"{{{storageID}}}"}
                        
                    """;

                root = await PostAsync(empresa, url, json_content);

                string? pb_abrir = root["MD"]?
                        .AsArray()
                        .FirstOrDefault(x => x?["UniqueID"]?
                            .GetValue<string>()?
                            .StartsWith("pb_abrir#") == true)?["UniqueID"]?
                        .GetValue<string>()?
                        .Split('#')
                        .LastOrDefault();

                //ConfigurarParâmetros

                await ParametrosRelatorio(empresa, controlNumber, dataManagerId, storageID, 3, param_empresa);
                await ParametrosRelatorio(empresa, controlNumber, dataManagerId, storageID, 4, param_estab);
                await ParametrosRelatorio(empresa, controlNumber, dataManagerId, storageID, 5, data_inicio);
                await ParametrosRelatorio(empresa, controlNumber, dataManagerId, storageID, 6, data_fim);
                if(buraco_nota == "S")
                    await ParametrosRelatorio2(empresa, controlNumber, dataManagerId, proc_id_t, pb_abrir, t1_, storageID, 9, buraco_nota, 7);
                if (diferenca_capa_item == "S")
                await ParametrosRelatorio2(empresa, controlNumber, dataManagerId, proc_id_t, pb_abrir, t1_, storageID, 10, diferenca_capa_item, 8);
                if (icms_resumido == "S") 
                    await ParametrosRelatorio2(empresa, controlNumber, dataManagerId, proc_id_t, pb_abrir, t1_, storageID, 11, icms_resumido, 9);
                if (notas_sem_item == "S")
                    await ParametrosRelatorio2(empresa, controlNumber, dataManagerId, proc_id_t, pb_abrir, t1_, storageID, 15, notas_sem_item, 13);
                qtd_itens = "S";
                    await ParametrosRelatorio2(empresa, controlNumber, dataManagerId, proc_id_t, pb_abrir, t1_,storageID, 16, qtd_itens, 14);
                qtd_notas = "S";
                    await ParametrosRelatorio2(empresa, controlNumber, dataManagerId, proc_id_t, pb_abrir, t1_, storageID, 18, qtd_notas, 16);
                qtd_canceladas = "S";
                    await ParametrosRelatorio2(empresa, controlNumber, dataManagerId, proc_id_t, pb_abrir, t1_, storageID, 19, qtd_canceladas, 17);
                if (extracao_canceladas == "S")
                    await ParametrosRelatorio2(empresa, controlNumber, dataManagerId, proc_id_t, pb_abrir, t1_, storageID, 21, extracao_canceladas, 19);


                //Executar
                //safobfww_lib_proctab_frameworktabpage_parametrosdw_parametros_headerbuttonclicked
                url = "https://www.onesourcetax.com/amer1/oms-taxone-11/ws/safcp2/w_lib_proc_customizado_taxbr/safobfww_lib_proctab_frameworktabpage_parametrosdw_parametros_headerbuttonclicked";

                json_content = $$$"""
                    {"vm":"{{{newViews2}}}","menuPath":"Processos Customizados > Execução dos Processos Customizados","moduleExe":"safcp",
                    "parameters":{"row":1,"dwo":"pb_executar#{{{id}}}"},
                    "commands":[{"command":"UPDATE_CURRENT_KEY","data":{"key":"none"}},{"command":"UPDATE_DM_ROW_AND_COL","data":{"dataManagerId":"5e","currentRow":0,"currentControlName":"","displayedRowCount":10,"currentPage":1}},{"command":"UPDATE_DM_ROW_AND_COL","data":{"dataManagerId":"61","currentRow":0,"currentControlName":"","displayedRowCount":10,"currentPage":1}}],
                    "storageID":"{{{storageID}}}"}
                    """;

                root = await PostAsync(empresa, url, json_content);
                string? retorno = root["VD"]?["Commands"]?[0]?["parameters"]?["text"]?.GetValue<string>();

                MessageBox.Show(retorno, "Atenção",  MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                                   
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Falha catastrófica ao executar HTTP POST: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            a = await response.Content.ReadAsStringAsync();

        }

        public async ValueTask DisposeAsync()
        {
            //if (_browser != null) await _browser.CloseAsync();
           // _playwright?.Dispose();
        }
    }
}