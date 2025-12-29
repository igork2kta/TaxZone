using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaxZone
{
    public class CsvClass
    {

        public static void CopiarNotasAreaTransferencia(int columnIndex)
        {
            using OpenFileDialog openFileDialog = new()
            {
                Title = "Selecione um arquivo CSV ou ZIP",
                Filter = "Arquivos CSV ou ZIP (*.csv;*.zip)|*.csv;*.zip|Todos os arquivos (*.*)|*.*",
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\Downloads"
            };

            if (openFileDialog.ShowDialog() != DialogResult.OK)
                return;

            StringBuilder notasBuilder = new();
            int totalLinhas = 0;
            int linhasParciais = 0;
            bool cabecalho = true;

            try
            {
                IEnumerable<string> linhas;

                string caminho = openFileDialog.FileName;

                // CSV direto
                if (Path.GetExtension(caminho).Equals(".csv", StringComparison.OrdinalIgnoreCase))
                {
                    linhas = File.ReadLines(caminho);
                }
                // ZIP com CSV
                else
                {
                    using ZipArchive zip = ZipFile.OpenRead(caminho);

                    ZipArchiveEntry csv = zip.Entries
                        .First(e => e.FullName.EndsWith(".csv", StringComparison.OrdinalIgnoreCase));

                    using StreamReader reader = new(csv.Open(), Encoding.UTF8);

                    linhas = reader.ReadToEnd().Split('\n');
                }

                foreach (string linha in linhas)
                {
                    if (cabecalho)
                    {
                        cabecalho = false;
                        continue;
                    }

                    string[] colunas = linha.Split(';');

                    if (colunas.Length <= columnIndex)
                        continue;

                    if (notasBuilder.Length > 3950)
                    {
                        Clipboard.SetText(notasBuilder.ToString());
                        MessageBox.Show(
                            $"{linhasParciais} notas copiadas.\nClique OK para continuar.",
                            "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

                        notasBuilder.Clear();
                        linhasParciais = 0;
                    }

                    if (linhasParciais > 0)
                        notasBuilder.Append(',');

                    notasBuilder.Append(int.Parse(colunas[columnIndex].Trim()));
                    totalLinhas++;
                    linhasParciais++;
                }

                if (notasBuilder.Length > 0)
                    Clipboard.SetText(notasBuilder.ToString());

                MessageBox.Show(
                    $"Finalizado! {totalLinhas} notas copiadas.",
                    "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao ler o arquivo: " + ex.Message,
                    "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        public static List<int> CopiarNotas(int columnIndex)
        {
            using (OpenFileDialog openFileDialog = new ()
            {
                Title = "Selecione um arquivo CSV",
                Filter = "Arquivos CSV (*.csv)|*.csv|Todos os arquivos (*.*)|*.*",
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\Downloads"

            })
            {
                if (openFileDialog.ShowDialog() != DialogResult.OK)
                    return null;

                string caminhoArquivo = openFileDialog.FileName;
                List<int> notas = new ();
                int totalLinhas = 0;
                bool cabecalho = true;

                try
                {
                    foreach (string linha in File.ReadLines(caminhoArquivo))
                    {
                        // Pula o cabeçalho
                        if (cabecalho)
                        {
                            cabecalho = false;
                            continue;
                        }

                        string[] colunas = linha.Split(';');

                        if (colunas.Length > columnIndex)
                        {
                            string valor = colunas[columnIndex].Trim().Replace("=", "").Replace("\"", "").Replace("\\", "");
                            if (int.TryParse(valor, out int numero))
                            {
                                notas.Add(numero);
                                totalLinhas++;
                            }
                        }
                    }

                    return notas;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erro ao ler o arquivo: " + ex.Message,
                        "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return null;
                }
            }
        }

        public static void WriteDataTableToCsv(DataTable dataTable, string filePath, string query = null)
        {
            // Verifica se o caminho do arquivo está vazio
            if (string.IsNullOrEmpty(filePath)) return;

            //Tratamento se o diretorio existe ou não
            if (!Directory.Exists(new FileInfo(filePath).Directory.FullName))
                Directory.CreateDirectory(new FileInfo(filePath).Directory.FullName);

            // Verifica se a extensão .csv foi fornecida, se não, adiciona
            if (string.IsNullOrEmpty(Path.GetExtension(filePath))) filePath += ".csv";

            // Se o arquivo já existir, pergunta se deseja substituir
            if (File.Exists(filePath))
            {
                var response = MessageBox.Show($"Arquivo {filePath} já existe! Deseja substituir?", "Atenção!", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (response == DialogResult.No) return;
                else File.Delete(filePath);
            }

            // Define o encoding como ISO-8859-1
            Encoding iso = Encoding.GetEncoding("iso-8859-1");

            using (StreamWriter streamWriter = new StreamWriter(filePath, false, iso))
            {
                // Escrever cabeçalho (nomes das colunas)
                StringBuilder sb = new StringBuilder();
                foreach (DataColumn column in dataTable.Columns)
                {
                    sb.Append($"\"{column.ColumnName}\";");
                }
                streamWriter.WriteLine(sb.ToString().TrimEnd(';')); // Remover o último ponto e vírgula

                // Escrever os dados das linhas
                foreach (DataRow row in dataTable.Rows)
                {
                    sb.Clear();
                    foreach (var item in row.ItemArray)
                    {
                        sb.Append(string.IsNullOrEmpty(item.ToString()) ? ";" : $"\"{item}\";");
                    }
                    streamWriter.WriteLine(sb.ToString().TrimEnd(';')); // Remover o último ponto e vírgula
                }

                // Se houver uma query, adiciona no final do arquivo
                if (!string.IsNullOrEmpty(query))
                    streamWriter.WriteLine($"\"{query}\";");

            }
        }
    }
}
