namespace TaxZone
{
    public static class Queries
    {
        public static string qtdNotasMsa = @"select '{2}' EMPRESA, COD_ESTAB, 'NOTAS', count(1) TOTAL 
                                from (  SELECT COD_ESTAB, TO_DATE(dat_fiscal, 'YYYYMMDD') DAT_FISCAL, IND_FIS_JUR, COD_FIS_JUR, NUM_DOCFIS
                                        FROM safx42
                                        group by COD_ESTAB, DAT_FISCAL, IND_FIS_JUR, COD_FIS_JUR, NUM_DOCFIS
                                        )
                                where dat_fiscal between '{0}' and '{1}'
                                group by COD_ESTAB

                                union all

                                select '{2}'  EMPRESA, COD_ESTAB,'ITENS', count(1) TOTAL 
                                from (  select COD_ESTAB, TO_DATE(dat_fiscal, 'YYYYMMDD') DAT_FISCAL, IND_FIS_JUR, COD_FIS_JUR, NUM_DOCFIS, NUM_ITEM
                                        from safx43 
                                        group by COD_ESTAB, DAT_FISCAL, IND_FIS_JUR, COD_FIS_JUR, NUM_DOCFIS, NUM_ITEM
                                ) 
                                where dat_fiscal between '{0}' and '{1}'
                                group by COD_ESTAB

                                union all

                                select '{2}' EMPRESA, COD_ESTAB, 'CANCELADAS', count(1) TOTAL 
                                from (  select COD_ESTAB,  TO_DATE(dat_fiscal, 'YYYYMMDD') DAT_FISCAL, IND_FIS_JUR, COD_FIS_JUR, NUM_DOCFIS
                                        from safx42 where situacao = 'S'
                                        group by COD_ESTAB, DAT_FISCAL, IND_FIS_JUR, COD_FIS_JUR, NUM_DOCFIS
                                ) 
                                where dat_fiscal between '{0}' and '{1}'
                                group by COD_ESTAB";
    }
}
