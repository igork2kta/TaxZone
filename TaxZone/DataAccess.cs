using Oracle.ManagedDataAccess.Client;
using System.Data;

namespace TaxZone
{
    public static class DataAccess
    {

        const string alterSession = "ALTER SESSION SET CURRENT_SCHEMA = ";
        const string nls_date_format = "ALTER SESSION SET NLS_DATE_FORMAT = 'DD/MM/YYYY'";

        /// <summary>
        /// Exporta uma consulta para csv
        /// </summary>
        /// <param name="serviceName">Nome do serviço conforme definido no arquivo tnsnames.ora</param>
        /// <param name="session">Session (OWNER) no qual a query será executada</param>
        /// <param name="query">Query a ser executada</param>
        /// <param name="path">Path a salvar o arquivo</param>
        /// <returns>Sucesso ou falha da operação</returns>
        public static bool Export(string serviceName, string user, string password, string session, string path, string query)
        {

            GravaLog.Gravar("Iniciando exportação...\n" +
                $"Base: {serviceName}\n" +
                $"Session: {session}\n" +
                $"Usuário: {user}\n" +
                $"Senha: {password}\n" +
                $"Path: {path}\n");

            // Construir a string de conexão usando OracleConnectionStringBuilder
            OracleConnectionStringBuilder connectionStringBuilder = new OracleConnectionStringBuilder
            {
                DataSource = serviceName,
                UserID = user,
                Password = password
            };


            // Estabelecer a conexão com o banco de dados Oracle
            using (OracleConnection connection = new OracleConnection(connectionStringBuilder.ConnectionString))
            {
                try
                {
                    connection.Open();

                    //Altera o session e formato de dada antes de executar a consulta
                    using (OracleCommand command = new OracleCommand(alterSession + session, connection))
                    {
                        if (!string.IsNullOrEmpty(session))
                        {
                            command.CommandText = alterSession + session;
                            command.ExecuteNonQuery();
                        }
                        command.CommandText = nls_date_format;
                        command.ExecuteNonQuery();
                    }
                    
                    using (OracleCommand command = new OracleCommand(query, connection))
                    {

                        using (OracleDataReader reader = command.ExecuteReader())
                        {

                            DataTable dataTable = new DataTable();

                            dataTable.Load(reader);
                            GravaLog.Gravar($"Base: {serviceName}. Criando arquivo CSV.");
                            CsvClass.WriteDataTableToCsv(dataTable, path, query);

                            GravaLog.Gravar($"Base: {serviceName}. Extração finalizada.\n{dataTable.Rows.Count} Linhas extraídas.");

                            if (dataTable.Rows.Count == 0)
                            {
                                MessageBox.Show($"0 linhas extraídas.\nBanco: {serviceName}\nSession: {session}\nUsuario: {user}\nSenha: {password} ", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    GravaLog.Gravar($"Base: {serviceName}. \n {ex.Message}");
                    MessageBox.Show($"{ex.Message}\nBanco: {serviceName}\nSession: {session}\nUsuario: {user}\nSenha: {password} ", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }

            }
            return true;
        }


        /// <summary>
        /// Exporta uma consulta para csv
        /// </summary>
        /// <param name="serviceName">Nome do serviço conforme definido no arquivo tnsnames.ora</param>
        /// <param name="session">Session (OWNER) no qual a query será executada</param>
        /// <param name="query">Query a ser executada</param>
        /// <returns>Sucesso ou falha da operação</returns>
        public static DataTable ExecuteQuery(string user, string password, string serviceName, string session, string query)
        {


            GravaLog.Gravar("Iniciando exportação...\n" +
                $"Base: {serviceName}\n" +
                $"Session: {session}\n" +
                $"Usuário: {user}\n" +
                $"Senha: {password}\n");

            // Construir a string de conexão usando OracleConnectionStringBuilder
            OracleConnectionStringBuilder connectionStringBuilder = new OracleConnectionStringBuilder
            {
                DataSource = serviceName,
                UserID = user,
                Password = password
            };

            // Estabelecer a conexão com o banco de dados Oracle
            using (OracleConnection connection = new OracleConnection(connectionStringBuilder.ConnectionString))
            {
                try
                {
                    connection.Open();

                    //Altera o session e formato de dada antes de executar a consulta
                    using (OracleCommand command = new OracleCommand(alterSession + session, connection))
                    {
                        if (!string.IsNullOrEmpty(session))
                        {
                            command.CommandText = alterSession + session;
                            command.ExecuteNonQuery();
                        }
                        command.CommandText = nls_date_format;
                        command.ExecuteNonQuery();
                    }

                    using (OracleCommand command = new OracleCommand(query, connection))
                    {
                        using (OracleDataReader reader = command.ExecuteReader())
                        {

                            DataTable dataTable = new DataTable();

                            dataTable.Load(reader);
                            GravaLog.Gravar($"Base: {serviceName}. Criando arquivo CSV.");
                            //CsvClass.WriteDataTableToCsv(dataTable, path, query);

                            GravaLog.Gravar($"Base: {serviceName}. Extração finalizada.\n{dataTable.Rows.Count} Linhas extraídas.");

                            if (dataTable.Rows.Count == 0)
                            {
                                MessageBox.Show($"0 linhas extraídas.\nBanco: {serviceName}\nSession: {session}\nUsuario: {user}\nSenha: {password} ", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                            return dataTable;
                        }
                    }
                }
                catch (Exception ex)
                {
                    GravaLog.Gravar($"Base: {serviceName}. \n {ex.Message}");
                    MessageBox.Show($"{ex.Message}\nBanco: {serviceName}\nSession: {session}\nUsuario: {user}\nSenha: {password} ", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return null;
                }

            }
        }


        /// <summary>
        /// Exporta uma consulta para csv
        /// </summary>
        /// <param name="serviceName">Nome do serviço conforme definido no arquivo tnsnames.ora</param>
        /// <param name="session">Session (OWNER) no qual a query será executada</param>
        /// <param name="query">Query a ser executada</param>
        /// <returns>Sucesso ou falha da operação</returns>
        public static DataTable ExecuteQuery(string user, string password, string serviceName, string session, string query, OracleParameterCollection parameters)
        {
            GravaLog.Gravar("Iniciando exportação...\n" +
                $"Base: {serviceName}\n" +
                $"Session: {session}\n" +
                $"Usuário: {user}\n" +
                $"Senha: {password}\n");

            // Construir a string de conexão usando OracleConnectionStringBuilder
            OracleConnectionStringBuilder connectionStringBuilder = new OracleConnectionStringBuilder
            {
                DataSource = serviceName,
                UserID = user,
                Password = password
            };

            // Estabelecer a conexão com o banco de dados Oracle
            using (OracleConnection connection = new OracleConnection(connectionStringBuilder.ConnectionString))
            {
                try
                {
                    connection.Open();

                    //Altera o session e formato de dada antes de executar a consulta
                    using (OracleCommand command = new OracleCommand(alterSession + session, connection))
                    {
                        if (!string.IsNullOrEmpty(session))
                        {
                            command.CommandText = alterSession + session;
                            command.ExecuteNonQuery();
                        }
                        command.CommandText = nls_date_format;
                        command.Parameters.Add(parameters);
                        command.ExecuteNonQuery();
                    }

                    using (OracleCommand command = new OracleCommand(query, connection))
                    {
                        using (OracleDataReader reader = command.ExecuteReader())
                        {

                            DataTable dataTable = new DataTable();

                            dataTable.Load(reader);
                            GravaLog.Gravar($"Base: {serviceName}. Criando arquivo CSV.");
                            //CsvClass.WriteDataTableToCsv(dataTable, path, query);

                            GravaLog.Gravar($"Base: {serviceName}. Extração finalizada.\n{dataTable.Rows.Count} Linhas extraídas.");

                            if (dataTable.Rows.Count == 0)
                            {
                                MessageBox.Show($"0 linhas extraídas.\nBanco: {serviceName}\nSession: {session}\nUsuario: {user}\nSenha: {password} ", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                            return dataTable;
                        }
                    }
                }
                catch (Exception ex)
                {
                    GravaLog.Gravar($"Base: {serviceName}. \n {ex.Message}");
                    MessageBox.Show($"{ex.Message}\nBanco: {serviceName}\nSession: {session}\nUsuario: {user}\nSenha: {password} ", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return null;
                }

            }
        }
    }
}
