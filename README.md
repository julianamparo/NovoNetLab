# NovoNetLab

Geração de Nota Fiscal

Para executar o programa corretamente, é necessário seguir os seguintes passos:

Ir para a pasta SQL
- Executar o script Adicionar_Campos_NotaFiscalItem (para add campos de Ipi à tabela)
- Executar scripts tabelaDiretorioConfig e tabelaDefinicaoCFOP
- Executar Popular_tb_DiretorioConfig, alterando o diretório para o caminho onde devem ser gerados os arquivos XML
- Executar Popular_tb_DefinicaoCfop
- Executar as Stored Procedures/Functions:
    procBuscaValoresCfop (Solicitada no exercício 4)
    P_NOTA_FISCAL
    P_NOTA_FISCAL_ITEM
    funcBuscarCfop
    procAtualizarNumerosIpi(executar apenas 1 vez, pra popular os 3 campos novos da notaFiscalItem)
    

Com a base preparada para execução, rode o executável, preencha as informações solicitadas (O combinação de estado origem + destino deve estar cadastrada na tabela de Definição CFOP).
Ao fim da execução, o arquivo XML será gerado na pasta definida na tabela.
    
