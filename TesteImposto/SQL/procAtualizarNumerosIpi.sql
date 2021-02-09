USE Teste;  
GO  

CREATE OR ALTER PROCEDURE procAtualizarNumerosIPI

AS   

    UPDATE N 
	   SET N.BaseIpi = N2.BaseIcms
	      ,N.AliquotaIpi = 0
		  ,N.ValorIpi = N.BaseIpi * N.AliquotaIpi
	  FROM NotaFiscalItem as N
	  INNER JOIN NotaFiscalItem as N2
	  ON N.Id = N2.Id;

  UPDATE N 
	   SET N.BaseIpi = N2.BaseIcms
	      ,N.AliquotaIpi = 10
		  ,N.ValorIpi = N.BaseIpi * N.AliquotaIpi
	  FROM NotaFiscalItem as N
	  INNER JOIN NotaFiscalItem as N2
	  ON N.Id = N2.Id
	  WHERE N2.AliquotaIcms =18;

GO

exec AtualizarNumerosIPI;
