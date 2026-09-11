
using System.ComponentModel;
using System.Data;
using TaxZone.Data;
using TaxZone.DTO;
using TaxZone.Infrastructure;
namespace TaxZone.Services
{

    public enum ProcessType
    {
        SAFX04,
        SAFX42,
        SAFX43,
        SAFX2013
    }

    public enum ValidationStatus
    {
        NaoIniciado,
        Validando,
        Concluido,
        Erro
    }

    public class ValidationItem
    {
        public string BtnAcao {get;set;} = "Iniciar";
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Empresa { get; set; }
        public ProcessType Processo { get; set; }
        public ValidationStatus Status { get; set; }
        public CancellationTokenSource CancellationTokenSource { get; set; }
        public object Parametros { get; set; }
    }

    public static class ProcessingMonitor
    {
        private static readonly NotifyIcon _notifyIcon = new()
        {
            Icon = SystemIcons.Information,
            Visible = true
        };

        public static BindingList<ValidationItem> Processos { get; }
            = new();

        public static void Adicionar(ValidationItem processo)
        {
            processo.Status = ValidationStatus.NaoIniciado;

            Processos.Add(processo);
        }

        public static void Remover(ValidationItem processo)
        {
            processo.CancellationTokenSource?.Cancel();

            Processos.Remove(processo);
        }

        public static void MonitoringQuestion(string? empresa, ProcessType processType, object parametros)
        {

            var resposta = MessageBox.Show("Deseja monitorar essa execução?", "Pergunta", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (resposta == DialogResult.Yes)
            {
                if (string.IsNullOrEmpty(empresa))
                {
                    MessageBox.Show("Selecione pelo uma empresa para executar a função de monitoramento.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                Adicionar(
                        new ValidationItem
                        {
                            Empresa = empresa,
                            Processo = processType,
                            Parametros = parametros
                        });
            }
        }

        public static async Task IniciarValidacao(ValidationItem processo)
        {
            processo.CancellationTokenSource = new CancellationTokenSource();

            processo.Status = ValidationStatus.Validando;

            try
            {
                while (!processo.CancellationTokenSource.Token.IsCancellationRequested && processo.Status != ValidationStatus.Concluido)
                {
                    int pendentes = ConsultarPendentes(processo);

                    if (pendentes == 0)
                    {
                        processo.BtnAcao = "Remover";
                        processo.Status = ValidationStatus.Concluido;

                        if (Globais.jobAutomatico)
                        {
                            TaxApiResponse resposta = await ApiTax.ProgramarJob(processo.Empresa, null, null);

                            if (!resposta.Success)
                                _notifyIcon.ShowBalloonTip(
                                   7000,
                                   "Concluído",
                                   $"Processamento {processo.Processo} para {processo.Empresa} concluído com sucesso!\n Erro ao programar JOB automático: {resposta.Message}.",
                                   ToolTipIcon.Info);
                            
                            else
                                _notifyIcon.ShowBalloonTip(
                                       7000,
                                       "Concluído",
                                       $"Processamento {processo.Processo} para {processo.Empresa} concluído com sucesso!\n JOB automático programado com sucesso.",
                                       ToolTipIcon.Info);

                        }
                        else
                        {
                            _notifyIcon.ShowBalloonTip(
                                   7000,
                                   "Concluído",
                                   $"Processamento {processo.Processo} para {processo.Empresa} concluído com sucesso!\n JOB automático desativado.",
                                   ToolTipIcon.Info);

                        }
                    }

                    await Task.Delay(
                        TimeSpan.FromSeconds(10),
                        processo.CancellationTokenSource.Token);
                }
            }
            catch
            {
                processo.Status = ValidationStatus.Erro;
            }
        }

        private static int ConsultarPendentes(ValidationItem processo)
        {
            string sql;

            if(processo.Processo == ProcessType.SAFX2013)
            {
                sql = @"SELECT COUNT(1) FROM(
                        SELECT IND_SINCRONISMO_FISCAL
                        FROM TIPO_ITEM_CONTA 
                        WHERE IND_SINCRONISMO_FISCAL = 'S'

                        UNION ALL

                        SELECT IND_SINCRONISMO_FISCAL
                        FROM TAXA
                        WHERE IND_SINCRONISMO_FISCAL = 'S')";

                var bancoFar = Empresa.GetBancoFar(processo.Empresa);

                DataTable dtfar = DataAccess.ExecuteQuery(
                    Config.DatabaseUserFar,
                    Config.DatabasePasswordFar,
                    bancoFar.database,
                    bancoFar.owner,
                    sql);

                if(Convert.ToInt32(dtfar.Rows[0][0]) > 0) return Convert.ToInt32(dtfar.Rows[0][0]);
            }

            sql = MontarQuery(processo);

            var banco = Empresa.GetBancoMsa(processo.Empresa);

            DataTable dt = DataAccess.ExecuteQuery(
                Config.DatabaseUserMsa,
                Config.DatabasePasswordMsa,
                banco.database,
                banco.owner,
                sql);

            return Convert.ToInt32(dt.Rows[0][0]);
        }

        private static string MontarQuery(ValidationItem processo)
        {
            switch (processo.Processo)
            {
                case ProcessType.SAFX04:

                    var p04 = (Safx04_2013Parametros)processo.Parametros;

                    return string.Format(
                        Queries.pendentesSafx04,
                        p04.Codigos);

                case ProcessType.SAFX2013:

                    var p2013 = (Safx04_2013Parametros)processo.Parametros;

                    return string.Format(
                        Queries.pendentesSafx2013,
                        string.Join(",", p2013.Codigos));

                case ProcessType.SAFX42:

                    var p42 = (DocumentoFiscalParametros)processo.Parametros;

                    return string.Format(
                        Queries.pendentesSafx42,
                        processo.Empresa,
                        string.Join(",", Empresa.GetEstabelecimentos(processo.Empresa)),
                        string.Join(",", p42.Notas),
                        p42.DataInicial.ToString("dd/MM/yyyy"),
                        p42.DataFinal.ToString("dd/MM/yyyy"));

                case ProcessType.SAFX43:

                    var p43 = (DocumentoFiscalParametros)processo.Parametros;

                    return string.Format(
                        Queries.pendentesSafx43,
                        processo.Empresa,
                        string.Join(",", Empresa.GetEstabelecimentos(processo.Empresa)),
                        string.Join(",", p43.Notas),
                        p43.DataInicial.ToString("dd/MM/yyyy"),
                        p43.DataFinal.ToString("dd/MM/yyyy"));

                default:
                    throw new Exception("Processo não suportado");
            }
        }

    }


}
