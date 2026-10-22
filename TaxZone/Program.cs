using TaxZone.Data;
using TaxZone.DTO;
using TaxZone.Infrastructure;

namespace TaxZone
{
    internal static class Program
    {
        
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {

            // Nome da vari�vel de ambiente do TNS
            const string tnsVariable = "TNS_ADMIN";

            // Verifica se a vari�vel de ambiente j� existe no n�vel do usu�rio
            if (string.IsNullOrEmpty(Environment.GetEnvironmentVariable(tnsVariable, EnvironmentVariableTarget.User)))
            {
                MessageBox.Show("Variável de ambiente TNS_ADMIN não cadastrada, sua ausência pode causar falha para conectar no banco. Favor criar.",
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
    }
}