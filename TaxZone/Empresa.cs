
namespace TaxZone
{
    public static class Empresa
    {
        public static List<string> ListaEmpresas = [ "EMR", "ESE", "EPB", "ETO", "EMT", "EMS", "ESS", "ERO", "EAC" ];

        public static Banco GetBancoFar(string empresa)
        {
            switch (empresa)
            {
                case "EMR":
                    return new Banco("COM_MGR_ES", "CFLCL");
                case "ESE":
                    return new Banco("COM_SER_ES", "ENERGIPE");
                case "EPB":
                    return new Banco("COM_PAB_ES", "SAELPA");
                case "ETO":
                    return new Banco("COM_TOC_ES", "FARETO");
                case "EMT":
                    return new Banco("COM_MTN_ES", "FAREMT");
                case "EMS":
                    return new Banco("COM_MTS_ES", "FAREMS");
                case "ESS":
                    return new Banco("COM_SPR_ES", "FARESS");
                case "ERO":
                    return new Banco("COM_RON_ES", "FARERO");
                case "EAC":
                    return new Banco("COM_ACR_ES", "FAREAC");
                default:
                    return null;
            }
        }

        public static Banco GetBancoMsa(string empresa)
        {
            switch (empresa)
            {
                case "EMR":
                    return new Banco("MSA_CRP_PR", "MSFEMG");
                case "ESE":
                    return new Banco("MSA_CRP_PR", "MSFESE");
                case "EPB":
                    return new Banco("MSA_CRP_PR", "MSFEPB");
                case "ETO":
                    return new Banco("MSA_CRP_PR", "MSFETO");
                case "EMT":
                    return new Banco("MSA_CRP_PR", "MSFEMT");
                case "EMS":
                    return new Banco("MSA_CRP_PR", "MSFEMS");
                case "ESS":
                    return new Banco("MSA_CRP_PR", "MSFESS");
                case "ERO":
                    return new Banco("MSA_CRP_PR", "MSFERO");
                case "EAC":
                    return new Banco("MSA_CRP_PR", "MSFEAC");
                default:
                    return null;
            }
        }

        public static string GetEmpresaTax(string empresa)
        {
            switch (empresa)
            {
                case "EMR":
                    return "C14_001";
                case "ESE":
                    return "C14_003";
                case "EPB":
                    return "C14_002";
                case "ETO":
                    return "C14_009";
                case "EMT":
                    return "C14_004";
                case "EMS":
                    return "C14_005";
                case "ESS":
                    return "C14_007";
                case "ERO":
                    return "C14_006";
                case "EAC":
                    return "C14_008";
                default:
                    return null;
            }
        }

        public static string GetUsuarioTaxAutomation(string empresa)
        {
            switch (empresa)
            {
                case "EMR":
                    return "user_taxone_energisa";
                case "ESE":
                    return "user_taxone_energisasergipe";
                case "EPB":
                    return "user_taxone_energisaparaiba";
                case "ETO":
                    return "user_taxone_energisato";
                case "EMT":
                    return "user_taxone_energisamt";
                case "EMS":
                    return "user_taxone_energisams";
                case "ESS":
                    return "user_taxone_energsulsudeste";
                case "ERO":
                    return "user_taxone_energisaro";
                case "EAC":
                    return "user_taxone_energisaacre";
                default:
                    return null;
            }
        }

        public static int GetIndexFluxoTaxAutomation(string empresa)
        {
            switch (empresa)
            {
                case "EMR":
                    return 10;
                case "ESE":
                    return 8;
                case "EPB":
                    return 8;
                case "ETO":
                    return 10;
                case "EMT":
                    return 1;
                case "EMS":
                    return 8;
                case "ESS":
                    return 8;
                case "ERO":
                    return 8;
                case "EAC":
                    return 10;
                default:
                    throw new ArgumentException($"Empresa '{empresa}' não reconhecida para indexação do fluxo de automação fiscal.");
            }
        }

        public static List<int> GetEstabelecimentos(string empresa)
        {
            return empresa switch
            {
                "EMR" => [1, 78],
                "ESS" => [1, 70, 81],
                "EPB" => [1,28],
                _ => [1], //todo o resto so tem 1
            };
        }

        public static int GetCodEmpresa(string empresa)
        {
            return empresa switch
            {
                "EMR" => 1,
                "ESE" => 20,
                "EPB" => 27,
                "ETO" => 190,
                "EMT" => 191,
                "EMS" => 193,
                "ESS" => 218,
                "ERO" => 229,
                "EAC" => 226,
                _ => throw new ArgumentException($"Empresa '{empresa} desconhecida"),
            };
        }

    }
}
