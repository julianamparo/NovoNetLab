USE [Teste]
GO

/****** Object:  Table [dbo].[DiretorioConfig]    Script Date: 09/02/2021 01:51:36 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[DiretorioConfig](
	[Diretorio] [varchar](max) NULL,
	[DescricaoDiretorio] [varchar](max) NULL,
	[NomeDiretorio] [varchar](max) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO


