
namespace TaxZone
{
    public static class Empresa
    {
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

    }
}
