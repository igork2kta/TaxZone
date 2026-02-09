using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas.Parser;
using System.Data;
using System.Text;
using System.Text.RegularExpressions;

namespace TaxZone
{
    public static class FuncoesTax
    {
        public static void DiferencaItens(bool gerarArquivo, bool fracionar)
        {
            DialogResult resposta = MessageBox.Show("Selecionar arquivo DIFERENÇA CAPA-ITEM?", "Perguntar", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            string diferencaCapaItem = string.Empty;
            string notasSemItem = string.Empty;

            if (resposta == DialogResult.Yes)
                diferencaCapaItem = CsvClass.CopiarNotas(1);  //No arquivo Diferenca_Capa_Item as notas ficam na coluna 2

            resposta = MessageBox.Show("Selecionar arquivo NOTAS SEM ITEM?", "Perguntar", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (resposta == DialogResult.Yes)
                notasSemItem = CsvClass.CopiarNotas(2);  //No arquivo Notas_sem_item as notas ficam na coluna 3

            if (string.IsNullOrEmpty(diferencaCapaItem) && string.IsNullOrEmpty(diferencaCapaItem))
            {
                MessageBox.Show("Nenhuma nota encontrada.", "Perguntar", MessageBoxButtons.OK);
                return;
            }
                

            List<int> resultado = diferencaCapaItem
                .Split(',', StringSplitOptions.RemoveEmptyEntries)
                .Concat(notasSemItem.Split(',', StringSplitOptions.RemoveEmptyEntries))
                .Select(x => x.Trim())
                .Select(int.Parse)   // converte para int
                .Distinct()
                .ToList();


            if (gerarArquivo)
            {
                CsvClass.WriteIntListToCsv(resultado, fracionar);
                MessageBox.Show("Concluído!", "Sucesso!", MessageBoxButtons.OK);
            }
            else
                Util.DividirValoresAreaTransferencia(resultado, fracionar);
        }


        public static void BuracoDeNota(bool modeloHardcore, string referenciaBuracoNota, bool gerarArquivo, bool fracionar = true)
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
                            /*
                            if (fracionar && buffer.Length >= 3950)
                            {
                                Clipboard.SetText(buffer.ToString().TrimEnd(','));
                                MessageBox.Show($"{linhasParciais} / {totalNotas} notas copiadas para a área de transferência.\n" +
                                                "Reprocese essas e clique em OK para continuar.",
                                    "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                                buffer.Clear();
                            }*/
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


                if (gerarArquivo)
                {
                    CsvClass.WriteListToCsv(resultado, fracionar);
                    MessageBox.Show("Concluído!", "Sucesso!", MessageBoxButtons.OK);
                }
                else
                    Util.DividirValoresAreaTransferencia(resultado, fracionar);

                /*
                // Copia o restante final, se existir
                if (buffer.Length > 0)
                {
                    Clipboard.SetText(buffer.ToString().TrimEnd(','));
                    MessageBox.Show($"FIM, {linhasParciais} / {totalNotas} notas copiadas para a área de transferência.",
                                "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }*/

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
                case "Items":
                    if (arq_temporario)
                        notas = CsvClass.CopiarNotas(0, Config.PathScriptTemporario);
                    else
                        notas = CsvClass.CopiarNotas(1);

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
        public static void ImportarPessoaFisicaJuridica(bool gerarArquivo, bool fracionar = true)
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

                var valores = new List<int>();
                int linhasParciais = 0;

                foreach (Match match in matches)
                {
                    string valor = match.Value;
                    if (valor.Length >= 10)
                        valores.Add(int.Parse(valor.Substring(valor.Length - 10)));

                }

                valores = valores.Distinct().ToList();

                if (valores.Count == 0)
                {
                    MessageBox.Show("Nenhum valor encontrado no PDF.",
                        "Informação", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                var buffer = new StringBuilder();
                foreach (var v in valores)
                {
                    buffer.Append(v).Append(',');

                    /*
                    if (fracionar && buffer.Length >= 1950)
                    {
                        Clipboard.SetText(buffer.ToString().TrimEnd(','));
                        MessageBox.Show($"{linhasParciais} / {valores.Count} UC's copiadas para a área de transferência.\n" +
                                        "Cole onde precisar e clique em OK para continuar.",
                            "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        buffer.Clear();
                    }*/
                    linhasParciais++;
                }

                List<string> lista = buffer.ToString()
                  .Split(',')
                  .ToList();

                if (gerarArquivo)
                {
                    CsvClass.WriteListToCsv(lista, fracionar);
                    MessageBox.Show("Concluído!", "Sucesso!", MessageBoxButtons.OK);
                }
                else
                    Util.DividirValoresAreaTransferencia(lista, fracionar);

                /*
                // Copia o restante final, se existir
                if (buffer.Length > 0)
                {
                    Clipboard.SetText(buffer.ToString().TrimEnd(','));
                    MessageBox.Show($"{linhasParciais} / {valores.Count} UC's copiadas para a área de transferência. Finalizado!\n",
                                "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }*/
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

            List<int> canceladasTax = CsvClass.CopiarNotasCanceladas(3);

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

            /*
            // Para verificar as notas a mais que estão no tax
            foreach (var kvp in taxCounts)
            {
                int numero = kvp.Key;
                int qtdTax = kvp.Value;
                int qtdFar = farCounts.ContainsKey(numero) ? farCounts[numero] : 0;

                if (qtdFar < qtdTax)
                {
                    // Adiciona o número tantas vezes quanto faltar
                    int faltam = qtdTax - qtdFar;
                    faltando.AddRange(Enumerable.Repeat(numero, faltam));
                }
            }*/

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
