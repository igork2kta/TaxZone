using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas.Parser;
using System.Data;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Text.RegularExpressions;

namespace TaxZone
{
    public class Util
    {
        public static void DividirValoresAreaTransferencia(List<int> lista, bool naoFracionar = false)
        {
            StringBuilder notasBuilder = new ();
            int linhasParciais = 0;
            int totalLinhas = 0;
            foreach (int linha in lista)
            {
                string valor = linha.ToString();

                // Se o texto acumulado já está grande, envia pro clipboard (somente se fracionamento estiver ativo)
                if (!naoFracionar && notasBuilder.Length > 3950)
                {
                    Clipboard.SetText(notasBuilder.ToString());
                    MessageBox.Show($"{linhasParciais} notas copiadas para a área de transferência.\n" +
                                    "Reprocese essas e clique em OK para continuar.",
                        "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    notasBuilder.Clear();
                    linhasParciais = 0;
                }

                // Adiciona vírgula **somente se já tiver algo antes**
                if (linhasParciais > 0)
                    notasBuilder.Append(',');

                notasBuilder.Append(valor);
                totalLinhas++;
                linhasParciais++;
            }

            // Copia o restante que ficou
            if (notasBuilder.Length > 0)
                Clipboard.SetText(notasBuilder.ToString());

            MessageBox.Show($"Finalizado! {totalLinhas} notas totais copiadas para a área de transferência.",
                "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        public static string IntListToString(List<int> list)
        {
            return string.Join(",", list);
        }

        public static void BuracoDeNota(bool modeloHardcore, string referenciaBuracoNota, bool naoFracionar = false)
        {
            if(modeloHardcore && (string.IsNullOrEmpty(referenciaBuracoNota) || referenciaBuracoNota.Length < 7))
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
                    totalNotas += proximo - numDocfis;
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

                        for (int i = inicio + 1; i < fim;i++)
                        {
                            buffer.Append(i).Append(',');
                            
                            if (!naoFracionar && buffer.Length >= 3950)
                            {
                                Clipboard.SetText(buffer.ToString().TrimEnd(','));
                                MessageBox.Show($"{linhasParciais} / {totalNotas} notas copiadas para a área de transferência.\n" +
                                                "Reprocese essas e clique em OK para continuar.",
                                    "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                                buffer.Clear();
                            }
                            linhasParciais++;
                        }
                        
                    }
                }
                
                // Copia o restante final, se existir
                if (buffer.Length > 0)
                {
                    Clipboard.SetText(buffer.ToString().TrimEnd(','));
                    MessageBox.Show($"Fim, {linhasParciais} / {totalNotas} notas copiadas para a área de transferência.",
                                "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }


            }
        }

        public static void ImportarPessoaFisicaJuridica(bool naoFracionar = false)
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

                    if (!naoFracionar && buffer.Length >= 1950)
                    {
                        Clipboard.SetText(buffer.ToString().TrimEnd(','));
                        MessageBox.Show($"{linhasParciais} / {valores.Count} UC's copiadas para a área de transferência.\n" +
                                        "Cole onde precisar e clique em OK para continuar.",
                            "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        buffer.Clear();
                    }
                    linhasParciais++;
                }

                // Copia o restante final, se existir
                if (buffer.Length > 0)
                {
                    Clipboard.SetText(buffer.ToString().TrimEnd(','));
                    MessageBox.Show($"{linhasParciais} / {valores.Count} UC's copiadas para a área de transferência. Finalizado!\n",
                                "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
        }


        public static string GerarSequencia(int inicio, int fim)
        {
            if (inicio >= fim)
                return string.Empty;

            string resultado = "";
            for (int i = inicio + 1; i < fim; i++)
            {
                resultado += i.ToString();
                if (i < fim - 1)
                    resultado += ",";
            }

            return resultado;
        }

        public static void MostrarDataTable(DataTable table)
        {
            if (table == null || table.Rows.Count == 0)
                return;

            int colCount = table.Columns.Count;
            int[] colWidths = new int[colCount];

            // Calcula a largura máxima de cada coluna
            for (int i = 0; i < colCount; i++)
            {
                colWidths[i] = table.Columns[i].ColumnName.Length;

                foreach (DataRow row in table.Rows)
                {
                    int length = row[i]?.ToString()?.Length ?? 0;
                    if (length > colWidths[i])
                        colWidths[i] = length;
                }
            }

            StringBuilder sb = new();

            // Cabeçalho
            for (int i = 0; i < colCount; i++)
            {
                sb.Append(table.Columns[i].ColumnName.PadRight(colWidths[i] + 2));
            }
            sb.AppendLine();

            // Linha separadora
            for (int i = 0; i < colCount; i++)
            {
                sb.Append(new string('-', colWidths[i])).Append("  ");
            }
            sb.AppendLine();

            // Linhas
            foreach (DataRow row in table.Rows)
            {
                for (int i = 0; i < colCount; i++)
                {
                    string value = row[i]?.ToString() ?? string.Empty;
                    sb.Append(value.PadRight(colWidths[i] + 2));
                }
                sb.AppendLine();
            }

            Form form = new()
            {
                Text = "Dados",
                Width = 300,
                Height = 300,
                StartPosition = FormStartPosition.CenterScreen
            };

            TextBox textBox = new()
            {
                Multiline = true,
                ReadOnly = true,
                Dock = DockStyle.Fill,
                ScrollBars = ScrollBars.Both,
                Font = new Font("Consolas", 10),
                WordWrap = false,
                Text = sb.ToString()
            };

            form.Controls.Add(textBox);

            // NÃO modal — mantém o form principal utilizável
            form.Show();
        }


    }
}
