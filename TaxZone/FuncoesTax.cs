using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas.Parser;
using System.Data;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace TaxZone
{
    public static class FuncoesTax
    {
        public static void DiferencaItens(bool gerarArquivo, bool fracionar)
        {
            string diferencaCapaItem = string.Empty;
            string notasSemItem = string.Empty;

            using (OpenFileDialog dialog = new OpenFileDialog())
            {
                dialog.Title = "Selecione os arquivos";
                dialog.Filter = "Arquivos CSV ou ZIP (*.csv;*.zip)|*.csv;*.zip|Todos os arquivos (*.*)|*.*";
                dialog.Multiselect = true;

              
                if (dialog.ShowDialog() != DialogResult.OK)
                    return;

                foreach (string arquivo in dialog.FileNames)
                {
                    string nome = Path.GetFileNameWithoutExtension(arquivo).ToUpper();

                    if (nome.Contains("DIFERENCA_CAPA_ITEM"))
                    {
                        diferencaCapaItem = CsvClass.CopiarNotas(1, arquivo);
                    }
                    else if (nome.Contains("NOTAS_SEM_ITEM"))
                    {
                        notasSemItem = CsvClass.CopiarNotas(2, arquivo);
                    }
                }
            }

            if (string.IsNullOrEmpty(diferencaCapaItem) && string.IsNullOrEmpty(notasSemItem))
            {
                MessageBox.Show("Nenhuma nota encontrada.");
                return;
            }

            List<int> resultado = diferencaCapaItem
                .Split(',', StringSplitOptions.RemoveEmptyEntries)
                .Concat(notasSemItem.Split(',', StringSplitOptions.RemoveEmptyEntries))
                .Select(x => x.Trim())
                .Select(int.Parse)
                .Distinct()
                .ToList();

            if (gerarArquivo)
            {
                CsvClass.WriteIntListToCsv(resultado, fracionar);
                MessageBox.Show("Concluído!", "Sucesso!");
            }
            else
            {
                Util.DividirValoresAreaTransferencia(resultado, fracionar);
            }
        }




        public static void BuracoDeNota(bool modeloHardcore, string referenciaBuracoNota)
        {
            if (modeloHardcore && (string.IsNullOrEmpty(referenciaBuracoNota) || referenciaBuracoNota.Length < 7))
                MessageBox.Show("Preencha a referencia para o modo hardcore!");

            using (OpenFileDialog openFileDialog = new()
            {
                Title = "Selecione um arquivo PDF",
                Filter = "Arquivos PDF (*.pdf)|*.pdf|Todos os arquivos (*.*)|*.*",
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\Downloads"
            })
            {
                if (openFileDialog.ShowDialog() != DialogResult.OK)
                    return;

                using var pdf = new PdfDocument(new PdfReader(openFileDialog.FileName));
                string allText = "";
                for (int i = 1; i <= pdf.GetNumberOfPages(); i++)
                {
                    allText += PdfTextExtractor.GetTextFromPage(pdf.GetPage(i));
                }

                var regex = new Regex(@"\|\s*(\d{6,})\s*\|\s*(\d{6,})\s*\|", RegexOptions.Multiline);

                var pairs = new List<(int NumDocfis, int Proximo)>();
                int totalNotas = 0;

                foreach (Match match in regex.Matches(allText))
                {
                    int numDocfis = int.Parse(match.Groups[1].Value);
                    int proximo = int.Parse(match.Groups[2].Value);
                    pairs.Add((numDocfis, proximo));
                    totalNotas += (proximo - 1) - numDocfis; //precisa do -1, confia em mim
                }

                F_buraco_nota buraco = new F_buraco_nota(ref pairs);
                var a = buraco.ShowDialog();
                if (a == DialogResult.Cancel) 
                    return;

                var buffer = new StringBuilder();
                int linhasParciais = 0;
                int linhasTotais = 0;
                foreach (var (inicio, fim) in pairs)
                {

                    string n;

                    if (modeloHardcore)
                    {
                        if (fim - 1 > inicio + 1)
                        {
                            n = $"update capa_nf_sped_{referenciaBuracoNota} set datgrv = null where numdoc_fsc between {inicio + 1} and {fim - 1};";
                        }
                        else if (inicio + 1 == fim - 1)
                        {
                            n = $"update capa_nf_sped_{referenciaBuracoNota} set datgrv = null where numdoc_fsc = {inicio};";
                        }
                        else
                        {
                            continue;
                        }
                        buffer.Append(n).Append('\n');
                        linhasParciais++;
                        linhasTotais++;
                    }

                    else
                    {
                        // Garante que fim seja maior que inicio
                        if (fim <= inicio) continue;

                        for (int i = inicio + 1; i < fim; i++)
                        {
                            buffer.Append(i).Append(',');
                            linhasParciais++;
                        }

                    }
                }

                List<string> resultado = buffer.ToString()
                        .Split(',', StringSplitOptions.RemoveEmptyEntries)
                        .Concat(buffer.ToString().Split(',', StringSplitOptions.RemoveEmptyEntries))
                        .Select(x => x.Trim())
                        .Distinct()
                        .ToList();


                if (Globais.gerarArquivo)
                {
                    CsvClass.WriteListToCsv(resultado, Globais.fracionarValores);
                    MessageBox.Show("Concluído!", "Sucesso!", MessageBoxButtons.OK);
                }
                else
                    Util.DividirValoresAreaTransferencia(resultado, Globais.fracionarValores);

       
                    

            }
        }



        public static void PendenciaProcessamento(string tipoPendencia, string empresa, bool arq_temporario)
        {
            Banco banco = Empresa.GetBancoMsa(empresa);
            if (banco is null) return;
                       
            string notas = "", query = "", condicao = "";
            switch (tipoPendencia)
            {
                case "Notas":
                case "Canceladas":
                    if(arq_temporario)
                        notas = CsvClass.CopiarNotas(0, Config.PathScriptTemporario);
                    else
                        notas = CsvClass.CopiarNotas(0);

                    condicao = Util.DividirValoresIn(notas, "num_docfis", false);
                    query = Queries.pendentesSafx42 + " AND (" + condicao + ")";
                    break;
                case "Itens":
                    if (arq_temporario)
                        notas = CsvClass.CopiarNotas(0, Config.PathScriptTemporario);
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

            DataTable pendentes = DataAccess.ExecuteQuery(ConfigManager.DatabaseUserMsa, ConfigManager.DatabasePasswordMsa, banco.database, banco.owner, query);

            MessageBox.Show($"{pendentes.Rows.Count} notas pendentes!", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }


        /*VOU TER QUE CONVERTER TUDO PARA WRITE STRING LIST DO CSV*/
        public static void ImportarPessoaFisicaJuridica(bool gerarArquivo, bool fracionar = true, bool codFisJurCompleto = false)
        {
            using (OpenFileDialog openFileDialog = new()
            {
                Title = "Selecione um arquivo PDF",
                Filter = "Arquivos PDF (*.pdf)|*.pdf|Todos os arquivos (*.*)|*.*",
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\Downloads"
            })
            {
                if (openFileDialog.ShowDialog() != DialogResult.OK)
                    return;

                using var pdf = new PdfDocument(new PdfReader(openFileDialog.FileName));

                string allText = "";
                for (int i = 1; i <= pdf.GetNumberOfPages(); i++)
                {
                    allText += PdfTextExtractor.GetTextFromPage(pdf.GetPage(i));
                }

                // Captura valores que aparecem após "Conteúdo do Campo"
                // Exemplo: F1910005128733
                var regex = new Regex(@"F\d{13}", RegexOptions.Multiline);
                var matches = regex.Matches(allText);

                var valores = new List<string>();


                foreach (Match match in matches)
                {
                    string valor = match.Value;
                    if (codFisJurCompleto)
                        valores.Add(valor);
                    else if (valor.Length >= 10)
                        valores.Add(int.Parse(valor.Substring(valor.Length - 10)).ToString()); //o parse é para remover os zeros à esquerda

                }

                valores = valores.Distinct().ToList();

                if (valores.Count == 0)
                {
                    MessageBox.Show("Nenhum valor encontrado no PDF.",
                        "Informação", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                if (gerarArquivo)
                {
                    CsvClass.WriteListToCsv(valores, fracionar);
                    MessageBox.Show("Concluído!", "Sucesso!", MessageBoxButtons.OK);
                }
                else
                {
                    if(codFisJurCompleto)
                        valores = valores.Select(s => $"'{s}'").ToList();
                    
                    Util.DividirValoresAreaTransferencia(valores, fracionar); 
                }

            }
        }

        public static void ImportarProdutos()
        {
            using (OpenFileDialog openFileDialog = new()
            {
                Title = "Selecione um arquivo PDF",
                Filter = "Arquivos PDF (*.pdf)|*.pdf|Todos os arquivos (*.*)|*.*",
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\Downloads"
            })
            {
                if (openFileDialog.ShowDialog() != DialogResult.OK)
                    return;

                using var pdf = new PdfDocument(new PdfReader(openFileDialog.FileName));

                string allText = "";
                for (int i = 1; i <= pdf.GetNumberOfPages(); i++)
                {
                    allText += PdfTextExtractor.GetTextFromPage(pdf.GetPage(i));
                }

                // Captura valores que aparecem após "Conteúdo do Campo"
                // Exemplo: F1910005128733 F.T0003603
                var regex = new Regex(@"F.T\d{7}", RegexOptions.Multiline);
                var taxas = regex.Matches(allText);

                regex = new Regex(@"F.P\d{7}", RegexOptions.Multiline);
                var produtos = regex.Matches(allText);

                var listaTaxas = new List<string>();
                var listaProdutos = new List<string>();

                //Encontra as taxas no pdf
                foreach (Match match in taxas)
                {
                    string valor = match.Value;

                    if (valor.Length >= 7)
                        listaTaxas.Add(int.Parse(valor.Substring(valor.Length - 7)).ToString()); //o parse é para remover os zeros à esquerda
                }

                //Encontra os produtos no pdf
                foreach (Match match in produtos)
                {
                    string valor = match.Value;

                    if (valor.Length >= 7)
                        listaProdutos.Add(int.Parse(valor.Substring(valor.Length - 7)).ToString()); //o parse é para remover os zeros à esquerda
                }

                if (listaTaxas.Count == 0 && listaProdutos.Count == 0)
                {
                    MessageBox.Show("Nenhum valor encontrado no PDF.",
                        "Informação", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                //Remove as duplicatas
                listaTaxas = listaTaxas.Distinct().ToList();
                listaProdutos = listaProdutos.Distinct().ToList();


                var buffer = new StringBuilder();

                //Monta sql das taxas
                if (listaTaxas.Count > 0) {

                    //buffer.AppendLine("update taxa set IND_SINCRONISMO_FISCAL = 'S' where codtaxa_tab in (");

                    foreach (var v in listaTaxas)
                    {
                        buffer.Append($"{v},");

                    }

                    buffer.Remove(buffer.Length - 3, 1); //remove a ultima virgula, -3 porque o appendline adiciona \n no final
                    //buffer.AppendLine(");");

                    Clipboard.SetText(buffer.ToString());

                    MessageBox.Show($"{listaTaxas.Count} taxas copiadas para área de transferência.",
                        "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);

                }
                
                //Monta sql dos produtos
                if (listaProdutos.Count > 0)
                {

                    //buffer.AppendLine("update TIPO_ITEM_CONTA set IND_SINCRONISMO_FISCAL = 'S' where COD_TIPO_ITEM in (");

                    foreach (var v in listaProdutos)
                    {
                      buffer.Append($"{v},");
                    }

                    buffer.Remove(buffer.Length - 3, 1); //remove a ultima virgula, -3 porque o appendline adiciona \n no final
                    // buffer.AppendLine(");");

                    Clipboard.SetText(buffer.ToString());

                    MessageBox.Show($"Finalizado! {listaProdutos.Count} produtos copiados para área de transferência.",
                        "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);

                }
                /*
                Clipboard.SetText(buffer.ToString());

                MessageBox.Show($"Finalizado! {listaTaxas.Count} taxas e {listaProdutos.Count} copiados para área de transferência.",
                    "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                */

            }
        }


        public static void GetDiferencaCanceladas(string ano, string mes, string empresa, bool mes_aberto, bool gerarArquivo, bool fracionar)
        {
            Banco banco = Empresa.GetBancoFar(empresa);

            if (banco is null)
            {
                MessageBox.Show("Informe o banco de dados!");
                return;
            }

            List<int> canceladasTax = CsvClass.CopiarNotasCanceladas(1);

            if (canceladasTax is null)
            {
                MessageBox.Show("Não foi possível obter as notas do arquivo.");
                return;
            }

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



            DataTable dataTableCanceladasFar = DataAccess.ExecuteQuery(ConfigManager.DatabaseUserFar, ConfigManager.DatabasePasswordFar, banco.database, banco.owner, query);

            if(dataTableCanceladasFar is null)
            {
                MessageBox.Show("Falha ao obter notas canceladas na base FAR", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

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


            if (gerarArquivo)
            {
                CsvClass.WriteIntListToCsv(faltando, fracionar);
                MessageBox.Show("Concluído!", "Sucesso!", MessageBoxButtons.OK);
            }
            else
                Util.DividirValoresAreaTransferencia(faltando, fracionar);

        }
    }
}
