namespace TaxZone.Infrastructure
{
    public static class Logger
    {
        private static readonly string PastaLogs =
            Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs");

        private static readonly object _lock = new();

        public static void Log(string empresa, string mensagem)
        {
            try
            {
                Directory.CreateDirectory(PastaLogs);

                string nomeArquivo = $"{empresa}.log";
                string caminhoArquivo = Path.Combine(PastaLogs, nomeArquivo);

                string linha =
                    $"{DateTime.Now:dd/MM/yyyy HH:mm:ss}\t{mensagem}";

                lock (_lock)
                {
                    File.AppendAllText(
                        caminhoArquivo,
                        linha + Environment.NewLine
                    );
                }
            }
            catch
            {
                // Falha no log não deve interromper a aplicação
            }
        }
    }
}
