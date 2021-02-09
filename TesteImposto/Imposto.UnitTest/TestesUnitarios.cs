using Imposto.Core.Domain;
using System;

namespace Imposto.UnitTest
{
    public class TestesUnitarios
    {
        public void GerarXMLTest()
        {
            NotaFiscal notaFiscal = new NotaFiscal();

            notaFiscal.Id = 99999;
            notaFiscal.NumeroNotaFiscal = 999999;
            notaFiscal.NomeCliente = "Teste Unitario";
            notaFiscal.Serie = 999999;
            notaFiscal.EstadoOrigem = "SP";
            notaFiscal.EstadoDestino = "RO";

            NotaFiscalItem nfItem = new NotaFiscalItem();
            nfItem.IdNotaFiscal = notaFiscal.NumeroNotaFiscal;
            nfItem.NomeProduto = "Produto Teste";
            nfItem.CodigoProduto = "123";
            nfItem.Cfop = "6.006";
            nfItem.TipoIcms = "60";
            nfItem.BaseIcms = 312;
            nfItem.AliquotaIcms = 0.18;
            nfItem.ValorIcms = 56.16;
            nfItem.BaseIpi = 312;
            nfItem.AliquotaIpi = 0.1;
            nfItem.ValorIpi = 31.2;

            notaFiscal.GerarXML(notaFiscal);


        }
    }
}
