CREATE OR ALTER FUNCTION funcBuscarCfop (
  
      @EstadoOrigem varchar(2),
	  @EstadoDestino varchar(2)
   
) RETURNS varchar(5)

AS 
BEGIN
      DECLARE @CFOP VARCHAR(5)

	  SET @CFOP = (SELECT DISTINCT Cfop
                     FROM DefinicaoCfop
		            WHERE EstadoOrigem = @EstadoOrigem
		              AND EstadoDestino = @EstadoDestino)


		  RETURN @CFOP



END



SELECT funcBuscarCfop('SP', 'RO')