namespace TaxZone
{
    public static class Queries
    {
        public const string qtdNotasMsa = @"select '{2}' EMPRESA, COD_ESTAB, 'NOTAS', count(1) TOTAL 
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


        public const string qtdNotasFar = @"select '{2}' EMPRESA, 'NOTAS', count(1) TOTAL, codfil
                                            from CAPA_NF_SPED_{0}_{1} a
                                            where CODMDE_DOC = 66 
                                            group by codfil

                                            union all

                                            select '{2}' EMPRESA, 'ITENS', count(1) TOTAL, a.codfil
                                            from CAPA_NF_SPED_{0}_{1} a,
                                                 ITEM_NF_SPED_{0}_{1} b
                                            where   a.CODEMP = b.CODEMP
                                                    and a.CODFIL=b.CODFIL
                                                    and a.DATEMI=b.DATEMI
                                                    and a.IDTPSS=b.IDTPSS
                                                    and a.CODDTN=b.CODDTN
                                                    and a.NUMDOC_FSC=b.NUMDOC_FSC
                                                    and a.NUMser=b.NUMser
                                                    and a.CODMDE_DOC = 66 
                                                    group by a.codfil

                                            union all

                                            select '{2}' EMPRESA, 'CANCELADAS',count(1) TOTAL, codfil
                                            from CAPA_NF_SPED_{0}_{1} a
                                            where DATCAN is not null AND CODMDE_DOC = 66
                                            group by codfil

                                            order by 4, 2 desc";




        public const string queryIcmsSifar = @"SELECT  A.CODFIL, SUM(
                                            CASE 
                                                WHEN DATCAN IS NULL 
                                                THEN DECODE(INDADC_DCT, 'A', B.VLRICMS, B.VLRICMS * (-1)) 
                                                ELSE 0 
                                            END
                                        ) ICMS
                                FROM CAPA_NF_SPED_{0}_{1} A,
                                     ITEM_NF_SPED_{0}_{1} B
                                WHERE A.CODEMP = B.CODEMP
                                  AND A.CODFIL = B.CODFIL
                                  AND A.DATEMI = B.DATEMI
                                  AND A.IDTPSS = B.IDTPSS
                                  AND A.CODDTN = B.CODDTN
                                  AND A.NUMDOC_FSC = B.NUMDOC_FSC
                                  AND A.NUMSER = B.NUMSER
                                  AND A.CODMDE_DOC = '66'
                                GROUP BY A.CODFIL";


        public const string pendentesSafx43 = "select * from safx43 where num_docfis IN({0}) AND DTH_INCLUSAO IS NULL";
        public const string pendentesSafx42 = "select * from safx42 where num_docfis IN({0}) AND DTH_INCLUSAO IS NULL";

        public const string canceladasFar = "select NUMDOC_FSC from capa_nf_sped_{0}_{1} where datcan is not null";

    }
}
