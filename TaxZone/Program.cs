namespace TaxZone
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {

            // Nome da variável de ambiente do TNS
            const string tnsVariable = "TNS_ADMIN";

            // Verifica se a variável de ambiente já existe no nível do usuário
            if (string.IsNullOrEmpty(Environment.GetEnvironmentVariable(tnsVariable, EnvironmentVariableTarget.User)))
            {
                MessageBox.Show("Variável de ambiente TNS_ADMIN não cadastrada, sua ausência pode causar falha para conectar no banco. Favor criar.",
                    "Atenção!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }


                // To customize application configuration such as set high DPI settings or default font,
                // see https://aka.ms/applicationconfiguration.
                ApplicationConfiguration.Initialize();
            Application.Run(new Form1());
        }
    }
}