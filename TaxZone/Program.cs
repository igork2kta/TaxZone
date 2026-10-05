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

            // Nome da variável de ambiente do TNS
            const string tnsVariable = "TNS_ADMIN";

            // Verifica se a variável de ambiente já existe no nível do usuário
            if (string.IsNullOrEmpty(Environment.GetEnvironmentVariable(tnsVariable, EnvironmentVariableTarget.User)))
            {
                MessageBox.Show("Variável de ambiente TNS_ADMIN não cadastrada, sua ausência pode causar falha para conectar no banco. Favor criar.",
                    "Atenção!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            Banco.CriarBanco();

            Banco.CriarAviso(
                "2.3.0 - Atualização Segurança",
                "Nova versão: Para sua segurança, agora suas senhas são salvas com criptografia. Insira novamente as suas senhas no menu Configurações > Credenciais."
            );

            //Limpar pasta de arquivos temporários
            // Garantir que a pasta de arquivos temporários exista
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