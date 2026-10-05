using TaxZone.DTO;

namespace TaxZone.Services
{
    public class StatusTask : IDisposable
    {
        private readonly StatusStrip _statusStrip;
        private bool _disposed;

        public ToolStripStatusLabel Label { get; }
        public ToolStripProgressBar ProgressBar { get; }

        public StatusTask(StatusStrip statusStrip, string texto)
        {
            _statusStrip = statusStrip;

            Label = new ToolStripStatusLabel(texto);

            ProgressBar = new ToolStripProgressBar
            {
                Minimum = 0,
                Maximum = 100,
                Width = 120
            };

            _statusStrip.Items.Add(Label);
            _statusStrip.Items.Add(ProgressBar);
        }

        public void Atualizar(Progresso p)
        {
            if (_disposed)
                return;

            Label.Text = p.Mensagem;
            ProgressBar.Value = p.Valor;
        }

        public void Dispose()
        {
            _disposed = true;

            _statusStrip.Items.Remove(Label);
            _statusStrip.Items.Remove(ProgressBar);

            Label.Dispose();
            ProgressBar.Dispose();
        }
    }
}
