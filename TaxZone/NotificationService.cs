using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaxZone
{
    public static class NotificationService
    {
        public static event Action<string, int>? QtdNotasProgressChanged;
        public static event Action<string, int>? StatusTaxChanged;

        public static void AtualizarStatusQtdNotas(string mensagem, int progresso)
        {
            progresso = Math.Max(0, Math.Min(100, progresso));

            QtdNotasProgressChanged?.Invoke(mensagem, progresso);
        }

        public static void AtualizarStatusTax(string mensagem, int progresso)
        {
            progresso = Math.Max(0, Math.Min(100, progresso));

            StatusTaxChanged?.Invoke(mensagem, progresso);
        }

        public static void Clear()
        {
            QtdNotasProgressChanged?.Invoke("", 0);
        }
    }
}
