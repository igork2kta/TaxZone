using System.Data;
using System.Data.SQLite;

namespace TaxZone.Data
{
    public static class Banco
    {
        private static readonly string connectionString =
    @"Data Source=J:\Igor Pinheiro\TaxZoneDatabase\taxzone.db;BusyTimeout=10000;";

        public static void CriarBanco()
        {
            using var conexao = new SQLiteConnection(connectionString);
            conexao.Open();

            string sql = @"
                CREATE TABLE IF NOT EXISTS comparativo_notas
                (
                    id INTEGER PRIMARY KEY AUTOINCREMENT,
                    ano INTEGER NOT NULL,
                    mes INTEGER NOT NULL,
                    empresa TEXT NOT NULL,
                    estabelecimento INTEGER NOT NULL,
                    tipo TEXT NOT NULL,
                    qtd_sifar DECIMAL(18,2) NOT NULL DEFAULT 0,
                    qtd_tax DECIMAL(18,2) NOT NULL DEFAULT 0,
                    status TEXT NOT NULL DEFAULT 'EM ANDAMENTO'
                );

                CREATE TABLE IF NOT EXISTS avisos
                (
                    id INTEGER PRIMARY KEY AUTOINCREMENT,
                    codigo TEXT NOT NULL UNIQUE,
                    mensagem TEXT NOT NULL
                );

                CREATE TABLE IF NOT EXISTS avisos_exibidos
                (
                    id INTEGER PRIMARY KEY AUTOINCREMENT,
                    aviso_id INTEGER NOT NULL,
                    usuario TEXT NOT NULL,
                    data_exibicao TEXT NOT NULL,

                    UNIQUE(aviso_id, usuario),
    
                    FOREIGN KEY(aviso_id) REFERENCES avisos(id)
                );

                    CREATE TABLE IF NOT EXISTS versoes_aplicacao
                    (
                        versao TEXT PRIMARY KEY NOT NULL,
                        mensagem TEXT NOT NULL,
                        data_publicacao TEXT NOT NULL
                    );
                ";

            using var cmd = new SQLiteCommand(sql, conexao);
            cmd.ExecuteNonQuery();
        }

        public static string? VerificarAtualizacaoDisponivel(Version versaoAtual)
        {
            using var con = Conexao();
            con.Open();

            Version? versaoMaisRecente = null;

            using (var cmd = new SQLiteCommand(
                "SELECT versao FROM versoes_aplicacao", con))
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    if (!Version.TryParse(reader["versao"]?.ToString(), out var versaoRegistrada))
                        continue;

                    if (versaoMaisRecente == null || versaoRegistrada > versaoMaisRecente)
                    {
                        versaoMaisRecente = versaoRegistrada;
                    }
                }
            }

            if (versaoMaisRecente == null || versaoAtual > versaoMaisRecente)
            {
                string versaoTexto = versaoAtual.ToString();
                string mensagem = $"Há uma atualização disponível para a versão {versaoTexto}.";
                string sql = @"
                    INSERT OR IGNORE INTO versoes_aplicacao
                    (
                        versao,
                        mensagem,
                        data_publicacao
                    )
                    VALUES
                    (
                        @versao,
                        @mensagem,
                        @data_publicacao
                    );";

                using var cmd = new SQLiteCommand(sql, con);
                cmd.Parameters.AddWithValue("@versao", versaoTexto);
                cmd.Parameters.AddWithValue("@mensagem", mensagem);
                cmd.Parameters.AddWithValue("@data_publicacao", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.ExecuteNonQuery();

                return null;
            }

            if (versaoMaisRecente > versaoAtual)
            {
                string versaoAtualTexto = versaoAtual.Build >= 0
                    ? versaoAtual.ToString(3)
                    : versaoAtual.ToString(2);
                string versaoNovaTexto = versaoMaisRecente.Build >= 0
                    ? versaoMaisRecente.ToString(3)
                    : versaoMaisRecente.ToString(2);

                return $"Há uma atualização disponível. Versão atual: {versaoAtualTexto}, versão nova: {versaoNovaTexto}.";
            }

            return null;
        }

        public static SQLiteConnection Conexao()
        {
            return new SQLiteConnection(connectionString);
        }


        public static void CriarAviso(string codigo, string mensagem)
        {
            using var con = Conexao();
            con.Open();

            string sql = @"
        INSERT OR IGNORE INTO avisos
        (
            codigo,
            mensagem
        )
        VALUES
        (
            @codigo,
            @mensagem
        );";

            using var cmd = new SQLiteCommand(sql, con);

            cmd.Parameters.AddWithValue("@codigo", codigo);
            cmd.Parameters.AddWithValue("@mensagem", mensagem);

            cmd.ExecuteNonQuery();
        }


        public static void InserirRegistro(int ano,
                                           int mes,
                                           string empresa,
                                           int estabelecimento,
                                           string tipo)
        {
            using var con = Conexao();
            con.Open();

            string sql = @"
                INSERT INTO comparativo_notas
                (
                    ano,
                    mes,
                    empresa,
                    estabelecimento,
                    tipo
                )
                VALUES
                (
                    @ano,
                    @mes,
                    @empresa,
                    @estabelecimento,
                    @tipo
                );";

            using var cmd = new SQLiteCommand(sql, con);

            cmd.Parameters.AddWithValue("@ano", ano);
            cmd.Parameters.AddWithValue("@mes", mes);
            cmd.Parameters.AddWithValue("@empresa", empresa);
            cmd.Parameters.AddWithValue("@estabelecimento", estabelecimento);
            cmd.Parameters.AddWithValue("@tipo", tipo);

            cmd.ExecuteNonQuery();
        }

        public static void AtualizarRegistro(int qtdSifar, int qtdTax, int id)
        {
            using var con = Conexao();
            con.Open();

            string sql = @"
                UPDATE comparativo_notas
                   SET qtd_sifar = @sifar,
                       qtd_tax   = @tax
                 WHERE id = @id";

            using var cmd = new SQLiteCommand(sql, con);

            cmd.Parameters.AddWithValue("@sifar", qtdSifar);
            cmd.Parameters.AddWithValue("@tax", qtdTax);
            cmd.Parameters.AddWithValue("@id", id);

            cmd.ExecuteNonQuery();
        }

        public static void AtualizarStatus(string status, int id)
        {
            using var con = Conexao();
            con.Open();

            string sql = @"
                UPDATE comparativo_notas
                   SET status = @status
                 WHERE id = @id";

            using var cmd = new SQLiteCommand(sql, con);

            cmd.Parameters.AddWithValue("@status", status);
            cmd.Parameters.AddWithValue("@id", id);

            cmd.ExecuteNonQuery();
        }

        public static DataTable Listar(int ano, int mes, List<string> empresas)
        {
            string empresasSql = string.Join(",", empresas.Select(e => $"'{e}'"));

            using var con = Conexao();
            con.Open();

            string sql = $@"
                SELECT ID, 
                        ANO AS ANO, 
                        MES AS MES,
                        EMPRESA AS EMPRESA, 
                        ESTABELECIMENTO AS ESTAB, 
                        TIPO AS TIPO, 
                        QTD_SIFAR AS QTD_SIFAR,
                        QTD_TAX AS QTD_TAX, 
                        STATUS AS STATUS
                  FROM comparativo_notas
                 WHERE ano = @ano
                   AND mes = @mes
                   AND empresa IN ({empresasSql});
                 ORDER BY empresa, estabelecimento, tipo";

            using var cmd = new SQLiteCommand(sql, con);

            cmd.Parameters.AddWithValue("@ano", ano);
            cmd.Parameters.AddWithValue("@mes", mes);

            using var da = new SQLiteDataAdapter(cmd);

            DataTable dt = new DataTable();
            da.Fill(dt);

            return dt;
        }

        public static void AtualizarQtdSifar(DataTable dadosSifar, int ano, int mes)
        {
            using var con = Conexao();
            con.Open();

            using var transaction = con.BeginTransaction();

            try
            {
                string sql = @"
            UPDATE comparativo_notas
               SET qtd_sifar = @qtd
             WHERE ano = @ano
               AND mes = @mes
               AND empresa = @empresa
               AND tipo = @tipo
               AND estabelecimento = @estabelecimento";

                foreach (DataRow row in dadosSifar.Rows)
                {
                    using var cmd = new SQLiteCommand(sql, con);

                    cmd.Parameters.AddWithValue("@qtd", row["TOTAL"]);
                    cmd.Parameters.AddWithValue("@ano", ano);
                    cmd.Parameters.AddWithValue("@mes", mes);
                    cmd.Parameters.AddWithValue("@empresa", row["EMPRESA"]);
                    cmd.Parameters.AddWithValue("@tipo", row["TIPO"]);
                    cmd.Parameters.AddWithValue("@estabelecimento", row["COD_ESTAB"]);

                    cmd.ExecuteNonQuery();
                }

                transaction.Commit();
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }

        public static void AtualizarQtdTax(int ano, int mes, string empresa, string tipo, int estabelecimento, decimal qtd_tax, SQLiteConnection con = null, SQLiteTransaction transaction = null)
        {
            bool criouConexao = false;
            bool criouTransacao = false;

            if (con == null)
            {
                con = Conexao();
                con.Open();
                criouConexao = true;
            }

            if (transaction == null)
            {
                transaction = con.BeginTransaction();
                criouTransacao = true;
            }

            try
            {
                string sql = @"
            UPDATE comparativo_notas
               SET qtd_tax = @qtd
             WHERE ano = @ano
               AND mes = @mes
               AND empresa = @empresa
               AND tipo = @tipo
               AND estabelecimento = @estabelecimento";

                using var cmd = new SQLiteCommand(sql, con, transaction);

                cmd.Parameters.AddWithValue("@qtd", qtd_tax);
                cmd.Parameters.AddWithValue("@ano", ano);
                cmd.Parameters.AddWithValue("@mes", mes);
                cmd.Parameters.AddWithValue("@empresa", empresa);
                cmd.Parameters.AddWithValue("@tipo", tipo);
                cmd.Parameters.AddWithValue("@estabelecimento", estabelecimento);

                cmd.ExecuteNonQuery();

                if (criouTransacao)
                    transaction.Commit();
            }
            catch
            {
                if (criouTransacao)
                    transaction.Rollback();

                throw;
            }
            finally
            {
                if (criouConexao)
                    con.Dispose();
            }
        }

        public static DataTable ListarAvisosNaoExibidos()
        {
            string usuario = $"{Environment.UserDomainName}\\{Environment.UserName}";

            using var con = Conexao();
            con.Open();

            string sql = @"
        SELECT a.id,
               a.codigo,
               a.mensagem
          FROM avisos a
         WHERE NOT EXISTS
         (
             SELECT 1
               FROM avisos_exibidos ae
              WHERE ae.aviso_id = a.id
                AND ae.usuario = @usuario
         )
         ORDER BY a.id;";

            using var cmd = new SQLiteCommand(sql, con);

            cmd.Parameters.AddWithValue("@usuario", usuario);

            using var da = new SQLiteDataAdapter(cmd);

            DataTable dt = new DataTable();
            da.Fill(dt);

            return dt;
        }

        public static void MarcarAvisoExibido(int avisoId)
        {
            string usuario = $"{Environment.UserDomainName}\\{Environment.UserName}";

            using var con = Conexao();
            con.Open();

            string sql = @"
        INSERT OR IGNORE INTO avisos_exibidos
        (
            aviso_id,
            usuario,
            data_exibicao
        )
        VALUES
        (
            @aviso_id,
            @usuario,
            @data_exibicao
        );";

            using var cmd = new SQLiteCommand(sql, con);

            cmd.Parameters.AddWithValue("@aviso_id", avisoId);
            cmd.Parameters.AddWithValue("@usuario", usuario);
            cmd.Parameters.AddWithValue(
                "@data_exibicao",
                DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

            cmd.ExecuteNonQuery();
        }
    }
}