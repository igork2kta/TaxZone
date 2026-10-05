using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas.Parser;
using System.Data;
using System.Diagnostics;
using System.Text;
using System.Text.RegularExpressions;
using TaxZone.Data;
using TaxZone.DTO;
using TaxZone.Infrastructure;
using TaxZone.Services;

namespace TaxZone.Utils
{
    public static class FuncoesTax
    {
        public static RetornoOperacao DiferencaItens(bool gerarArquivo, bool fracionar, string? empresa = null)
        {
            string diferencaCapaItem = string.Empty;
            string notasSemItem = string.Empty;

            using (OpenFileDialog dialog = new OpenFileDialog())
            {
                dialog.Title = "Selecione os arquivos";
                dialog.Filter = "Arquivos CSV ou ZIP (*.csv;*.zip)|*.csv;*.zip|Todos os arquivos (*.*)|*.*";
                dialog.Multiselect = true;
                dialog.InitialDirectory = Config.DiretorioPadraoEntrada;

                if (dialog.ShowDialog() != DialogResult.OK)
                    return new RetornoOperacao(false, "Cancelado pelo usuário");

                foreach (string arquivo in dialog.FileNames)
                {
                    string nome = Path.GetFileNameWithoutExtension(arquivo).ToUpper();

                    if (nome.Contains("DIFERENCA_CAPA_ITEM"))
                        diferencaCapaItem = CsvClass.CopiarNotas(1, arquivo);
                    
                    else if (nome.Contains("NOTAS_SEM_ITEM"))
                        notasSemItem = CsvClass.CopiarNotas(2, arquivo);
                }
            }

            if (string.IsNullOrEmpty(diferencaCapaItem) && string.IsNullOrEmpty(notasSemItem))
                return new RetornoOperacao(false, "Nenhuma nota encontrada.");
            

            List<int> resultado = diferencaCapaItem
                .Split(',', StringSplitOptions.RemoveEmptyEntries)
                .Concat(notasSemItem.Split(',', StringSplitOptions.RemoveEmptyEntries))
                .Select(x => x.Trim())
                .Select(int.Parse)
                .Distinct()
                .ToList();

            RetornoOperacao retorno = new RetornoOperacao(true, $"Concluído! {resultado.Count} notas encontradas na diferença.");

            if (gerarArquivo)
                CsvClass.WriteIntListToCsv(resultado, fracionar);
            
            else
                retorno = Util.DividirValoresAreaTransferencia(resultado, fracionar);


            if (!Globais.inspector)
            {
                var parametros = new DocumentoFiscalParametros
                {
                    Notas = Util.DividirValoresIn(string.Join(",", resultado), "TO_NUMBER(NUM_DOCFIS)", false),
                    DataInicial = Globais.dataInicio,
                    DataFinal = Globais.dataFim
                };

                ProcessingMonitor.MonitoringQuestion(empresa, ProcessType.SAFX43, parametros);
            }

            return retorno;
        }

        public static RetornoOperacao BuracoDeNota(bool modeloHardcore, string referenciaBuracoNota, string? empresa = null)
        {
            if (modeloHardcore && (string.IsNullOrEmpty(referenciaBuracoNota) || referenciaBuracoNota.Length < 7))
                return new RetornoOperacao(false, "Preencha a referencia para o modo hardcore!");

            using OpenFileDialog openFileDialog = new()
            {
                Title = "Selecione um arquivo PDF",
                Filter = "Arquivos PDF (*.pdf)|*.pdf|Todos os arquivos (*.*)|*.*",
                InitialDirectory = Config.DiretorioPadraoEntrada
            };

            if (openFileDialog.ShowDialog() != DialogResult.OK)
                return new RetornoOperacao(false, "Cancelado pelo usuário");

            using var pdf = new PdfDocument(new PdfReader(openFileDialog.FileName));
            string allText = "";

            for (int i = 1; i <= pdf.GetNumberOfPages(); i++)
                allText += PdfTextExtractor.GetTextFromPage(pdf.GetPage(i));
            
            var regex = new Regex(@"\|\s*(\d{6,})\s*\|\s*(\d{6,})\s*\|", RegexOptions.Multiline);
            var pairs = new List<(int NumDocfis, int Proximo)>();
            int totalNotas = 0;

            foreach (Match match in regex.Matches(allText))
            {
                int numDocfis = int.Parse(match.Groups[1].Value);
                int proximo = int.Parse(match.Groups[2].Value);
                pairs.Add((numDocfis, proximo));
                totalNotas += proximo - 1 - numDocfis; //precisa do -1, confia em mim
            }

            F_buraco_nota buraco = new (ref pairs);
            var a = buraco.ShowDialog();
            if (a == DialogResult.Cancel)
                return new RetornoOperacao(false, "Cancelado pelo usuário");

            var buffer = new StringBuilder();

            foreach (var (inicio, fim) in pairs)
            {
                string n;

                if (modeloHardcore)
                {
                    if (fim - 1 > inicio + 1)
                        n = $"update capa_nf_sped_{referenciaBuracoNota} set datgrv = null where numdoc_fsc between {inicio + 1} and {fim - 1};";
                    
                    else if (inicio + 1 == fim - 1)
                        n = $"update capa_nf_sped_{referenciaBuracoNota} set datgrv = null where numdoc_fsc = {inicio};";
                 
                    else
                        continue;
                    
                    buffer.Append(n).Append('\n');
                }

                else
                {
                    // Garante que fim seja maior que inicio
                    if (fim <= inicio) continue;

                    for (int i = inicio + 1; i < fim; i++)
                        buffer.Append(i).Append(',');
                    
                }
            }

            List<string> resultado = buffer.ToString()
                    .Split(',', StringSplitOptions.RemoveEmptyEntries)
                    .Concat(buffer.ToString().Split(',', StringSplitOptions.RemoveEmptyEntries))
                    .Select(x => x.Trim())
                    .Distinct()
                    .ToList();


            RetornoOperacao retorno = new RetornoOperacao(true, $"Concluído! {totalNotas} notas encontradas no buraco.");

            if (Globais.gerarArquivo)
                CsvClass.WriteListToCsv(resultado, Globais.fracionarValores);
            
            else
                retorno = Util.DividirValoresAreaTransferencia(resultado, Globais.fracionarValores);


            if (!Globais.inspector)
            {
                var parametros = new DocumentoFiscalParametros
                {
                    Notas = Util.DividirValoresIn(string.Join(",", resultado), "TO_NUMBER(NUM_DOCFIS)", false),
                    DataInicial = Globais.dataInicio,
                    DataFinal = Globais.dataFim
                };

                ProcessingMonitor.MonitoringQuestion(empresa, ProcessType.SAFX42, parametros);
            }

            return retorno;

        }


        public static void PendenciaProcessamento(string tipoPendencia, string empresa, bool arq_temporario)
        {
            BancoDTO banco = Empresa.GetBancoMsa(empresa);
            if (banco is null) return;
                       
            string notas = "", query = "", condicao = "";
            switch (tipoPendencia)
            {
                case "Notas":
                case "Canceladas":
                    if(arq_temporario)
                        notas = CsvClass.CopiarNotas(0, Config.PathArquivoTemporario + "\\scriptTemporario.csv");
                    else
                        notas = CsvClass.CopiarNotas(0);

                    condicao = Util.DividirValoresIn(notas, "num_docfis", false);
                    query = Queries.pendentesSafx42 + " AND (" + condicao + ")";
                    break;
                case "Itens":
                    if (arq_temporario)
                        notas = CsvClass.CopiarNotas(0, Config.PathArquivoTemporario + "\\scriptTemporario.csv");
                    else
                        notas = CsvClass.CopiarNotas(0);

                    condicao = Util.DividirValoresIn(notas, "num_docfis", false);
                    query = Queries.pendentesSafx43 + " AND (" + condicao + ")";
                    break;
               
            }

            if (string.IsNullOrEmpty(notas))
            {
                MessageBox.Show("Nenhuma nota encontrada", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            DataTable pendentes = DataAccess.ExecuteQuery(Config.DatabaseUserMsa, Config.DatabasePasswordMsa, banco.database, banco.owner, query);

            MessageBox.Show($"{pendentes.Rows.Count} notas pendentes!", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }


        /*VOU TER QUE CONVERTER TUDO PARA WRITE STRING LIST DO CSV*/
        public static RetornoOperacao ImportarPessoaFisicaJuridica(bool gerarArquivo, bool fracionar, bool codFisJurCompleto, string? empresa = null, string? caminhoPdf = null, string? pathSaida = null)
        {
            if (string.IsNullOrWhiteSpace(caminhoPdf))
            {
                using OpenFileDialog openFileDialog = new()
                {
                    Title = "Selecione um arquivo PDF",
                    Filter = "Arquivos PDF (*.pdf)|*.pdf|Todos os arquivos (*.*)|*.*",
                    InitialDirectory = Config.DiretorioPadraoEntrada
                };

                if (openFileDialog.ShowDialog() != DialogResult.OK)
                    return new RetornoOperacao(false, "Cancelado pelo usuário"); 

                caminhoPdf = openFileDialog.FileName;
            }

            if (!File.Exists(caminhoPdf))
                return new RetornoOperacao(false, "O arquivo informado não foi encontrado."); 
            
            using var pdf = new PdfDocument(new PdfReader(caminhoPdf));

            string allText = "";

            for (int i = 1; i <= pdf.GetNumberOfPages(); i++)
                allText += PdfTextExtractor.GetTextFromPage(pdf.GetPage(i));
            

            // Captura valores que aparecem após "Conteúdo do Campo"
            // Exemplo: F1910005128733
            var regex = new Regex(@"F\d{13}", RegexOptions.Multiline);
            var matches = regex.Matches(allText);

            var valores = new List<string>();
            var valoresCompleto = new List<string>();

            foreach (Match match in matches)
            {
                string valor = match.Value;

                valoresCompleto.Add(valor);

                // O Parse remove os zeros à esquerda
                valores.Add(int.Parse(valor.Substring(valor.Length - 10)).ToString());
            }

            if(codFisJurCompleto)
                valores = valoresCompleto.Distinct().ToList();
            else
                valores = valores.Distinct().ToList();
            

            if (valores.Count == 0)
                return new RetornoOperacao(false, "Nenhum valor encontrado no PDF.");
            
            RetornoOperacao retorno = new RetornoOperacao(true, $"Concluído! {valores.Count} valores encontrados no PDF.");

            if (gerarArquivo)
            {
                CsvClass.WriteListToCsv(valores, fracionar, pathSaida);
            }
   
            else
            {
                if (codFisJurCompleto)
                    valores = valores.Select(s => $"'{s}'").ToList();

                retorno = Util.DividirValoresAreaTransferencia(valores, fracionar);
            }

            if (!Globais.inspector)
            {
                var parametros = new Safx04_2013Parametros
                {
                    Codigos = Util.DividirValoresIn(string.Join(",", valoresCompleto.Distinct()), "COD_FIS_JUR", true)
                };

                ProcessingMonitor.MonitoringQuestion(empresa, ProcessType.SAFX04, parametros);
            }

            return retorno;
        }

        public static RetornoOperacao ImportarProdutos(bool gerarArquivo, string? empresa = null, string? caminhoPdf = null, string? pathSaida = null)
        {
            if (string.IsNullOrEmpty(caminhoPdf))
            {
                using OpenFileDialog openFileDialog = new()
                {
                    Title = "Selecione um arquivo PDF",
                    Filter = "Arquivos PDF (*.pdf)|*.pdf|Todos os arquivos (*.*)|*.*",
                    InitialDirectory = Config.DiretorioPadraoEntrada
                };
                if (openFileDialog.ShowDialog() != DialogResult.OK)
                    return new RetornoOperacao(false, "Operação cancelada pelo usuário.");

                caminhoPdf = openFileDialog.FileName;
            }
            

            using var pdf = new PdfDocument(new PdfReader(caminhoPdf));

            string allText = "";
            for (int i = 1; i <= pdf.GetNumberOfPages(); i++)
                allText += PdfTextExtractor.GetTextFromPage(pdf.GetPage(i));
            
            // Captura valores que aparecem após "Conteúdo do Campo"
            // Exemplo: F1910005128733 F.T0003603
            var regex = new Regex(@"F.T\d{7}", RegexOptions.Multiline);
            var taxas = regex.Matches(allText);

            regex = new Regex(@"F.P\d{7}", RegexOptions.Multiline);
            var produtos = regex.Matches(allText);

            var listaTaxas = new List<string>();
            var listaProdutos = new List<string>();
            var listaCompleta = new List<string>();

            //Encontra as taxas no pdf
            foreach (Match match in taxas)
            {
                string valor = match.Value;

                if (valor.Length >= 7)
                {
                    listaTaxas.Add(int.Parse(valor.Substring(valor.Length - 7)).ToString()); //o parse é para remover os zeros à esquerda
                    listaCompleta.Add(valor);
                }
            }

            //Encontra os produtos no pdf
            foreach (Match match in produtos)
            {
                string valor = match.Value;

                if (valor.Length >= 7)
                {
                    listaProdutos.Add(int.Parse(valor.Substring(valor.Length - 7)).ToString()); //o parse é para remover os zeros à esquerda
                    listaCompleta.Add(valor);
                }
            }

            if (listaTaxas.Count == 0 && listaProdutos.Count == 0)
                return new RetornoOperacao(false, "Nenhum valor encontrado no PDF.");
            
            //Remove as duplicatas
            listaTaxas = listaTaxas.Distinct().ToList();
            listaProdutos = listaProdutos.Distinct().ToList();

            var buffer = new StringBuilder();

            if (listaTaxas.Count > 0)
            {
                foreach (var v in listaTaxas)
                    buffer.Append($"{v},");

                if(listaTaxas.Count == 1)
                    buffer.Remove(buffer.Length - 1, 1); //remove a ultima virgula
                else
                    buffer.Remove(buffer.Length - 3, 1); //remove a ultima virgula, -3 porque o appendline adiciona \n no final

                if(gerarArquivo)
                      CsvClass.WriteListToCsv(listaTaxas, false, pathSaida + "_taxas");
                else
                {
                    Clipboard.SetText(buffer.ToString());

                    MessageBox.Show($"{listaTaxas.Count} taxas copiadas para área de transferência.",
                        "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);

                }

            }

            //Monta sql dos produtos
            if (listaProdutos.Count > 0)
            {

                foreach (var v in listaProdutos)
                    buffer.Append($"{v},");

                if (listaProdutos.Count == 1)
                    buffer.Remove(buffer.Length - 1, 1); //remove a ultima virgula
                else
                    buffer.Remove(buffer.Length - 3, 1); //remove a ultima virgula, -3 porque o appendline adiciona \n no final

                if (gerarArquivo)
                    CsvClass.WriteListToCsv(listaProdutos, false, pathSaida + "_produtos");

                else
                {
                    Clipboard.SetText(buffer.ToString());

                    MessageBox.Show($"Finalizado! {listaProdutos.Count} produtos copiados para área de transferência.",
                        "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }

            if (!Globais.inspector)
            {
                var parametros = new Safx04_2013Parametros
                {
                    Codigos = Util.DividirValoresIn(string.Join(",", listaCompleta.Distinct()), "COD_PRODUTO", true)
                };

                ProcessingMonitor.MonitoringQuestion(empresa, ProcessType.SAFX2013, parametros);
            }


            return new RetornoOperacao(true, $"Concluído! {listaTaxas.Count} taxas e {listaProdutos.Count} produtos encontrados no PDF.");
        }


        public static RetornoOperacao GetDiferencaCanceladas(string ano, string mes, string empresa, bool mes_aberto, bool gerarArquivo, bool fracionar)
        {
            BancoDTO banco = Empresa.GetBancoFar(empresa);

            if (banco is null)
                return new RetornoOperacao(false, "Informe o banco de dados!");
            

            List<int> canceladasTax = CsvClass.CopiarNotasCanceladas(1);

            if (canceladasTax is null)
                return new RetornoOperacao(false, "Não foi possível obter as notas do arquivo.");
   

            string query = string.Empty;

            if (mes_aberto)
            {
                query = Queries.canceladasFarMesAberto;
            }
            else
            {
                query = string.Format(
                    Queries.canceladasFarMesFechado,
                    mes,
                    ano
                );
            }

            DataTable dataTableCanceladasFar = DataAccess.ExecuteQuery(Config.DatabaseUserFar, Config.DatabasePasswordFar, banco.database, banco.owner, query);

            if(dataTableCanceladasFar is null)
                return new RetornoOperacao(false, "Falha ao obter notas canceladas na base FAR");
            

            List<int> canceladasFar = dataTableCanceladasFar.AsEnumerable()
                .Select(r => r.Field<int>("NUMDOC_FSC"))
                .ToList();


            MessageBox.Show($"{canceladasFar.Count} canceladas recuperadas no SIFAR e {canceladasTax.Count} canceladas recuperadas no TAX. Diferença de {canceladasFar.Count - canceladasTax.Count} notas encontrada.",
                "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

            // Contagem das ocorrências de cada número nas duas listas
            var farCounts = canceladasFar.GroupBy(x => x)
                                         .ToDictionary(g => g.Key, g => g.Count());
            var taxCounts = canceladasTax.GroupBy(x => x)
                                         .ToDictionary(g => g.Key, g => g.Count());

            List<int> faltando = new();


            // Verifica se algum número está faltando ou aparece menos vezes
            foreach (var kvp in farCounts)
            {
                int numero = kvp.Key;
                int qtdFar = kvp.Value;
                int qtdTax = taxCounts.ContainsKey(numero) ? taxCounts[numero] : 0;

                if (qtdTax < qtdFar)
                {
                    // Adiciona o número tantas vezes quanto faltar
                    int faltam = qtdFar - qtdTax;
                    faltando.AddRange(Enumerable.Repeat(numero, faltam));
                }
            }

            RetornoOperacao retorno = new RetornoOperacao(true, $"Concluído! {faltando.Count} notas encontradas na diferença.");
            if (gerarArquivo)
                CsvClass.WriteIntListToCsv(faltando, fracionar);
            else
                retorno = Util.DividirValoresAreaTransferencia(faltando, fracionar);

            var parametros = new DocumentoFiscalParametros
            {
                Notas = Util.DividirValoresIn(string.Join(",", faltando), "TO_NUMBER(NUM_DOCFIS)", false),
                DataInicial = Globais.dataInicio,
                DataFinal = Globais.dataFim
            };

            ProcessingMonitor.MonitoringQuestion(empresa, ProcessType.SAFX42, parametros);

            return retorno;
        }

        public static async Task GetQuantidadeNotas(DateTime periodoIni, DateTime periodoFin, string empresa, bool mostrarNaTela, 
            bool arquivoTemporario, bool popularTabela, string local, bool incluidasHoje, bool mesAberto)
        {
            try
            {
                SaveFileDialog salvarDialog = new SaveFileDialog();

                int taskCount = 1;
                if (empresa == "TODAS") taskCount = 9;

                NotificationService.AtualizarStatusQtdNotas(
                            $"Iniciando consulta...",
                            1);

                int tarefasConcluidas = 0;

                var tasks = new List<Task<DataTable>>();

                for (int i = 0; i < taskCount; i++)
                {
                    BancoDTO banco = null;
                    string query = "", user = "", password = "";

                    if (taskCount > 1)
                        empresa = Empresa.ListaEmpresas[i].ToString();

                    if (local == "MSA")
                    {
                        banco = Empresa.GetBancoMsa(empresa);
                        user = Config.DatabaseUserMsa;
                        password = Config.DatabasePasswordMsa;

                        string filtroIncluidasHoje = "";
                        if (!incluidasHoje)
                        {
                            filtroIncluidasHoje = $"AND DTH_INCLUSAO < to_date('{DateTime.Now.ToString("dd/MM/yyyy")}', 'DD/MM/YYYY')";
                        }
                        string estabelecimentos = string.Join(",", Empresa.GetEstabelecimentos(empresa));
                        query = string.Format(Queries.qtdNotasMsa, Empresa.GetCodEmpresa(empresa), empresa, estabelecimentos, periodoIni.ToString("yyyyMMdd"), periodoFin.ToString("yyyyMMdd"), filtroIncluidasHoje);

                    }
                    else if (local == "SIFAR")
                    {
                        banco = Empresa.GetBancoFar(empresa);

                        if (mesAberto)
                            query = string.Format(Queries.qtdNotasFarMesAberto, periodoIni.ToString("dd/MM/yyyy"), periodoFin.ToString("dd/MM/yyyy"), empresa);
                        else
                            query = string.Format(Queries.qtdNotasFarMesFechado, periodoIni.Month.ToString("00"), periodoFin.Year, empresa);




                        user = Config.DatabaseUserFar;
                        password = Config.DatabasePasswordFar;
                    }

                    if (banco is null) return;

                    string serviceName = banco.database;
                    string session = banco.owner;

                    tasks.Add(Task.Run(() =>
                    {
                        DataTable resultado = DataAccess.ExecuteQuery(
                            user,
                            password,
                            serviceName,
                            session,
                            query);

                        int concluidas = Interlocked.Increment(ref tarefasConcluidas);
                        int progresso = concluidas * 100 / taskCount;

                        NotificationService.AtualizarStatusQtdNotas(
                            $"Consultando {concluidas}/{taskCount}",
                            progresso);

                        return resultado;
                    }));
                }

                DataTable qtd_notas = new();

                DataTable[] resultados = await Task.WhenAll(tasks);

                foreach (var tabela in resultados)
                {
                    if (tabela != null)
                        qtd_notas.Merge(tabela);
                }

                if (qtd_notas.Rows.Count == 0)
                {
                    MessageBox.Show("Falha ao consultar dados!", "Erro!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                if (mostrarNaTela)
                {
                    Util.MostrarDataTable(qtd_notas);
                }
                else if (popularTabela)
                {
                    Banco.AtualizarQtdSifar(qtd_notas, periodoIni.Year, periodoIni.Month);
                }
                else
                {
                    string filename;
                    if (!arquivoTemporario)
                        filename = salvarDialog.FileName;
                    else
                        filename = "C:\\Temp\\TaxZone\\qtd_notas.csv";

                    CsvClass.WriteDataTableToCsv(qtd_notas, filename);

                    var resposta = MessageBox.Show("Extração Finalizada! Deseja abrir o arquivo?", "Pronto!", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                    if (resposta == DialogResult.Yes)
                    {
                        Process.Start(new ProcessStartInfo(filename) { UseShellExecute = true });
                    }
                }
            }
            finally
            {
                //Encerra a notificação
                NotificationService.AtualizarStatusQtdNotas(
                            $"Finalizado",
                            100);
            }
            
        }

        public static async Task GetQuantidadeNotasFar(DateTime periodoIni, DateTime periodoFin, List<string> empresas, bool mostrarNaTela, string local, bool mesAberto, bool popularTabela, IProgress<Progresso>? progresso = null)
        {
            try
            {
                int taskCount = empresas.Count;

                progresso?.Report(new Progresso("Iniciando consulta", 1));

                int tarefasConcluidas = 0;

                var tasks = new List<Task<DataTable>>();

                foreach (string empresa in empresas)
                {
                    BancoDTO banco = null;
                    string query = "", user = "", password = "";


                    if (local == "MSA")
                    {
                        banco = Empresa.GetBancoMsa(empresa);
                        user = Config.DatabaseUserMsa;
                        password = Config.DatabasePasswordMsa;

                        string estabelecimentos = string.Join(",", Empresa.GetEstabelecimentos(empresa));
                        query = string.Format(Queries.qtdNotasMsa, Empresa.GetCodEmpresa(empresa), empresa, estabelecimentos, periodoIni.ToString("yyyyMMdd"), periodoFin.ToString("yyyyMMdd"), "");

                    }
                    else if (local == "SIFAR")
                    {
                        banco = Empresa.GetBancoFar(empresa);

                        if (mesAberto)
                            query = string.Format(Queries.qtdNotasFarMesAberto, periodoIni.ToString("dd/MM/yyyy"), periodoFin.ToString("dd/MM/yyyy"), empresa);
                        else
                            query = string.Format(Queries.qtdNotasFarMesFechado, periodoIni.Month.ToString("00"), periodoFin.Year, empresa);

                        user = Config.DatabaseUserFar;
                        password = Config.DatabasePasswordFar;
                    }

                    if (banco is null) return;

                    string serviceName = banco.database;
                    string session = banco.owner;

                    tasks.Add(Task.Run(() =>
                    {
                        DataTable resultado = DataAccess.ExecuteQuery(
                            user,
                            password,
                            serviceName,
                            session,
                            query);

                        int concluidas = Interlocked.Increment(ref tarefasConcluidas);
                        int porcentagem = concluidas * 100 / taskCount;

                        progresso?.Report(new Progresso($"Consultando {concluidas}/{taskCount}", porcentagem));

                        //Para mês aberto o sistema busca no MSA, mas lá não tem ICMS
                        if(mesAberto && popularTabela)
                        {
                            banco = Empresa.GetBancoFar(empresa);
                            query = string.Format(Queries.queryIcmsSifarMesAbertoPopulaTabela, periodoIni.ToString("dd/MM/yyyy"), periodoFin.ToString("dd/MM/yyyy"), empresa);
                            DataTable resultadoIcms = DataAccess.ExecuteQuery(
                                Config.DatabaseUserFar,
                                Config.DatabasePasswordFar,
                                banco.database,
                                banco.owner,
                                query);

                            resultado.Merge(resultadoIcms);
                        }



                        return resultado;
                    }));
                }

                DataTable qtd_notas = new();

                DataTable[] resultados = await Task.WhenAll(tasks);

                foreach (var tabela in resultados)
                {
                    if (tabela != null)
                        qtd_notas.Merge(tabela);
                }

                if (qtd_notas.Rows.Count == 0)
                {
                    MessageBox.Show("Falha ao consultar dados!", "Erro!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }


                if (mostrarNaTela)
                    Util.MostrarDataTable(qtd_notas);

                else if (popularTabela)
                    Banco.AtualizarQtdSifar(qtd_notas, periodoIni.Year, periodoIni.Month);

                else
                {
                    string filename = "C:\\Temp\\TaxZone\\qtd_notas.csv";

                    CsvClass.WriteDataTableToCsv(qtd_notas, filename);

                    var resposta = MessageBox.Show("Extração Finalizada! Deseja abrir o arquivo?", "Pronto!", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                    if (resposta == DialogResult.Yes)
                    {
                        Process.Start(new ProcessStartInfo(filename) { UseShellExecute = true });
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ocorreu um erro: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
    }
}
