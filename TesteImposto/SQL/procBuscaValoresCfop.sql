USE teste;
GO

CREATE PROCEDURE procBuscaValorCFOP

AS
SELECT CFOP
	  ,SUM(BaseIcms) 'Valor Total Base de ICMS'
	  ,SUM(ValorIcms) 'Valor Total ICMS'
	  ,SUM(BaseIpi) 'Valor Base IPI'
	  ,SUM(ValorIpi) 'Valor Total IPI'
  FROM NotaFiscalItem N
 GROUP BY Cfop;

 GO

