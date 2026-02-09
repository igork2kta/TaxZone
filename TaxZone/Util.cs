using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas.Parser;
using System;
using System.Data;
using System.Text;
using System.Text.RegularExpressions;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace TaxZone
{
    public class Util
    {
        public static void DividirValoresAreaTransferencia<T>(List<T> lista, bool fracionar = true)
        {
            StringBuilder notasBuilder = new ();
            int linhasParciais = 0;
            foreach (T linha in lista)
            {
                string valor = linha.ToString();

                // Se o texto acumulado já está grande, envia pro clipboard (somente se fracionamento estiver ativo)
                if (fracionar && notasBuilder.Length > 3950)
                {
                    Clipboard.SetText(notasBuilder.ToString());
                    MessageBox.Show($"{linhasParciais} / {lista.Count} notas copiadas para a área de transferência.\n" +
                                    "Reprocese essas e clique em OK para continuar.",
                        "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    notasBuilder.Clear();
                }

                // Adiciona vírgula **somente se já tiver algo antes**
                if (linhasParciais > 0)
                    notasBuilder.Append(',');

                notasBuilder.Append(valor);
                linhasParciais++;
            }

            // Copia o restante que ficou
            if (notasBuilder.Length > 0)
                Clipboard.SetText(notasBuilder.ToString());

            MessageBox.Show($"Finalizado! {lista.Count} notas totais copiadas para a área de transferência.",
                "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        public static string IntListToString(List<int> list)
        {
            return string.Join(",", list);
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

        
        public static string DividirValoresIn(string valores, string coluna, bool aspas)
        {
            string[] split = valores.Split(",");

            for (int i = 0; i < split.Length; i++)
            {
                //Aspas
                if (aspas) split[i] = "'" + split[i] + "'";

                //Vírgula
                if (i != split.Length - 1 && !string.IsNullOrEmpty(split[i + 1]))
                {
                    //Impede de colocar virgula na linha 1000 de cada IN
                    if ((i + 1) % 1000 != 0) split[i] += ","; ;
                }

                //Primeira linha
                if (i == 0) split[i] = $"{coluna} IN ( {split[i]}";

                //A cada 1000 linhas
                else if ((i + 1) % 1000 == 0) split[i] += $") OR {coluna} IN (";

                //Fecha o parênteses na ultima linha
                if (i == split.Length - 1)
                    split[i] += ")";

            }
            
            return string.Join("", split);
        }

    }
}
