using System;
using System.IO;

namespace TaxZone
{
    public static class GravaLog
    {


        public static void Gravar(string texto, string strNomeArquivo = "")
        {
                TaskGravar(texto, strNomeArquivo);

        }

        public static void TaskGravar(string strMensagem, string strNomeArquivo)
        {
            try
            {

                string nomeArquivo = AppDomain.CurrentDomain.BaseDirectory + "\\Logs\\" + strNomeArquivo + DateTime.Now.ToString("dd-MM-yy") + ".txt";

                if (!Directory.Exists(AppDomain.CurrentDomain.BaseDirectory + "\\Logs"))
                    Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory + "\\Logs");


                if (!File.Exists(nomeArquivo))
                {
                    FileStream arquivo = File.Create(nomeArquivo);
                    arquivo.Close();
                }
                using StreamWriter w = File.AppendText(nomeArquivo);
                AppendLog(strMensagem, w);

            }
            catch (Exception)
            {

            }
        }
        private static void AppendLog(string logMensagem, TextWriter txtWriter)
        {
            try
            {
                txtWriter.WriteLine("------------------" + DateTime.Now.ToString() + "------------------------------");
                txtWriter.WriteLine("\n" + logMensagem + "\n");
            }
            catch
            {
                throw;
            }
        }


    }
}
