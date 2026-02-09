using System.Data;
using System.IO.Compression;
using System.Text;
using System.Windows.Forms;

namespace TaxZone
{
    public class CsvClass
    {

        public static IEnumerable<string> LerArquivo(string path = "")
        {
            string caminho;

            if (string.IsNullOrEmpty(path))
            {
                using OpenFileDialog openFileDialog = new()
                {
                    Title = "Selecione um arquivo CSV ou ZIP",
                    Filter = "Arquivos CSV ou ZIP (*.csv;*.zip)|*.csv;*.zip|Todos os arquivos (*.*)|*.*",
                    InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\Downloads"
                };

                if (openFileDialog.ShowDialog() != DialogResult.OK)
                    return null;
                caminho = openFileDialog.FileName;
            }
            else
            {
                caminho = path;
            }


            IEnumerable<string> linhas;

            // CSV direto
            if (Path.GetExtension(caminho).Equals(".csv", StringComparison.OrdinalIgnoreCase))
            {
               return linhas = File.ReadLines(caminho);
            }
            // ZIP com CSV
            else
            {
                using ZipArchive zip = ZipFile.OpenRead(caminho);

                ZipArchiveEntry csv = zip.Entries
                    .First(e => e.FullName.EndsWith(".csv", StringComparison.OrdinalIgnoreCase));

                using StreamReader reader = new(csv.Open(), Encoding.UTF8);

                return linhas = reader.ReadToEnd().Split('\n');
            }
        }


        public static void CopiarNotasAreaTransferencia(int columnIndex, bool fracionar = true)
        {
            StringBuilder notasBuilder = new();
            int totalLinhas = 0;
            int linhasParciais = 0;
            bool cabecalho = true;

            try
            {
                IEnumerable<string> linhas = LerArquivo();

                if (linhas is null) return;

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

                    if (fracionar && notasBuilder.Length > 3950)
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

        public static string CopiarNotas(int columnIndex, string path = "")
        {
           
            StringBuilder notasBuilder = new();
            bool cabecalho = true;
            try
            {
                IEnumerable<string> linhas = LerArquivo(path);

                if (linhas is null) return string.Empty;
                
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

                    notasBuilder.Append(int.Parse(colunas[columnIndex].Trim()));
                    notasBuilder.Append(',');
                }
                if (notasBuilder.Length > 0)
                {
                    notasBuilder.Remove(notasBuilder.Length - 1, 1);
                }


                return notasBuilder.ToString();
                
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao ler o arquivo: " + ex.Message,
                    "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);

                return string.Empty;
            }
        }

        public static List<int> CopiarNotasCanceladas(int columnIndex)
        {

            IEnumerable<string> linhas = LerArquivo();

            if (linhas is null) return null; 
            
            List<int> notas = new ();
            int totalLinhas = 0;
            bool cabecalho = true;

            try
            {
                foreach (string linha in linhas)
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

        public static void WriteIntListToCsv(List<int> valores, bool fracionar, string filePath = null)
        {
            if (string.IsNullOrEmpty(filePath))
            {
                SaveFileDialog salvarDialog = new SaveFileDialog();

                salvarDialog.Title = "Salvar arquivo como...";
                salvarDialog.Filter = "Arquivo separado por vírgula (*.csv)|*.csv|Todos os arquivos (*.*)|*.*";
                salvarDialog.DefaultExt = "csv";
                salvarDialog.AddExtension = true;

                if (salvarDialog.ShowDialog() != DialogResult.OK) return;

                filePath = salvarDialog.FileName;
            }

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

            var sb = new StringBuilder();

            foreach (var v in valores)
            {
                sb.AppendLine(v.ToString());
            }

            File.WriteAllText(filePath, sb.ToString(), Encoding.UTF8);
            if(fracionar)
                SplitCsv(filePath, new FileInfo(filePath).Directory.FullName, 999);
        }

        public static void WriteListToCsv<T>(List<T> valores, bool fracionar, string filePath = null)
        {
            if (string.IsNullOrEmpty(filePath))
            {
                SaveFileDialog salvarDialog = new SaveFileDialog();

                salvarDialog.Title = "Salvar arquivo como...";
                salvarDialog.Filter = "Arquivo separado por vírgula (*.csv)|*.csv|Todos os arquivos (*.*)|*.*";
                salvarDialog.DefaultExt = "csv";
                salvarDialog.AddExtension = true;

                if (salvarDialog.ShowDialog() != DialogResult.OK) return;

                filePath = salvarDialog.FileName;
            }

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

            var sb = new StringBuilder();

            foreach (var v in valores)
            {
                sb.AppendLine(v.ToString());
            }

            File.WriteAllText(filePath, sb.ToString(), Encoding.UTF8);
            if (fracionar)
                SplitCsv(filePath, new FileInfo(filePath).Directory.FullName, 999);
        }



        private static void SplitCsv(string inputFile, string outputDir, int linesPerFile)
        {
            //Obtem o nome do arquivo sem a extensão
            string fileName = new FileInfo(inputFile).Name.Replace(new FileInfo(inputFile).Extension, "");

            //Mesmo encoding sql developer
            Encoding iso = Encoding.GetEncoding("iso-8859-1");

            using (var reader = new StreamReader(inputFile, iso))
            {
                int fileNumber = 1;
                int lineNumber = 0;

                List<string> currentLines = new List<string>();
                string header = null;

                while (!reader.EndOfStream)
                {
                    string line = reader.ReadLine();

                    // Primeira linha é o cabeçalho
                    if (lineNumber == 0 && fileNumber == 1)
                        header = line;

                    currentLines.Add(line);
                    lineNumber++;

                    if (lineNumber >= linesPerFile)
                    {
                        string outputFileName = Path.Combine(outputDir, $"{fileName}_PARTE_{fileNumber}.csv");

                        if (fileNumber == 1) WriteLinesToFile(outputFileName, header, currentLines, false);
                        else WriteLinesToFile(outputFileName, header, currentLines, false);
                        currentLines.Clear();
                        lineNumber = 0;
                        fileNumber++;
                    }
                }

                // Write remaining lines to the last file
                if (currentLines.Count > 0)
                {
                    string outputFileName = Path.Combine(outputDir, $"{fileName}_PARTE_{fileNumber}.csv");
                    WriteLinesToFile(outputFileName, header, currentLines, false);
                }
            }

            File.Delete(inputFile);


        }

        static void WriteLinesToFile(string fileName, string header, List<string> lines, bool writeHeader)
        {

            //Mesmo encoding sql developer
            Encoding iso = Encoding.GetEncoding("iso-8859-1");

            using var writer = new StreamWriter(fileName, true, iso);
            // Escreve o cabeçalho
            if (writeHeader) writer.WriteLine(header);

            foreach (var line in lines)
                writer.WriteLine(line);

        }

    }
}
