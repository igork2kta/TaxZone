using TaxZone.Data;
using TaxZone.DTO;
using TaxZone.Infrastructure;

namespace TaxZone
{
    internal static class Program
    {
        private const string TnsAdminVariable = "TNS_ADMIN";
        private const string TnsAdminDirectory = @"C:\oracle\ora121\network\admin";

        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            string? tnsAdminError = ConfigurarTnsAdmin();
            if (tnsAdminError != null)
            {
                MessageBox.Show(tnsAdminError,
                    "Atenção!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            Banco.CriarBanco();

            Version versaoAtual = typeof(Program).Assembly.GetName().Version ?? new Version(0, 0);
            string? mensagemAtualizacao = Banco.VerificarAtualizacaoDisponivel(versaoAtual);
            if (mensagemAtualizacao != null)
            {
                MessageBox.Show(
                    mensagemAtualizacao,
                    "Atualização disponível",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }

            Banco.CriarAviso(
                "2.4.0 - Melhoria ferramentas",
                "Nova versão: Nas ferramentas de ITENS, BURACO DE NOTA e DIFERENÇA CANCELADAS, agora há a opção de programar relatório automaticamente, bastando aguardar a finalização."
            );

            //Limpar pasta de arquivos temporários
            //Garantir que a pasta de arquivos temporários exista
            Directory.CreateDirectory(Config.PathArquivoTemporario);
            foreach (string arquivo in Directory.GetFiles(Config.PathArquivoTemporario))
                File.Delete(arquivo);

            
            Config.Load();
            

            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();
            if (args.Any(x => x.Equals("inspector", StringComparison.OrdinalIgnoreCase)))
            {
                Globais.inspector = true;
                Application.Run(new F_inspector());
            }
            else
            {
                Application.Run(new F_Main_V2());
            }
        }

        private static string? ConfigurarTnsAdmin()
        {
            if (!string.IsNullOrWhiteSpace(Environment.GetEnvironmentVariable(TnsAdminVariable)))
                return null;

            string? userValue = Environment.GetEnvironmentVariable(
                TnsAdminVariable,
                EnvironmentVariableTarget.User);
            string value = string.IsNullOrWhiteSpace(userValue)
                ? TnsAdminDirectory
                : userValue;

            if (string.IsNullOrWhiteSpace(userValue) && !Directory.Exists(TnsAdminDirectory))
            {
                return $"A pasta configurada para TNS_ADMIN não foi encontrada:\n{TnsAdminDirectory}";
            }

            try
            {
                if (string.IsNullOrWhiteSpace(userValue))
                {
                    Environment.SetEnvironmentVariable(
                        TnsAdminVariable,
                        value,
                        EnvironmentVariableTarget.User);
                }

                Environment.SetEnvironmentVariable(
                    TnsAdminVariable,
                    value,
                    EnvironmentVariableTarget.Process);
                return null;
            }
            catch (Exception ex) when (
                ex is UnauthorizedAccessException or
                System.Security.SecurityException or
                ArgumentException)
            {
                return $"Não foi possível cadastrar a variável de ambiente TNS_ADMIN:\n{ex.Message}";
            }
        }
    }
}