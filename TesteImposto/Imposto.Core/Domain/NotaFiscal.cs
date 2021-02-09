using Imposto.Core.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;

namespace Imposto.Core.Domain
{
    public class NotaFiscal
    {
        public int Id { get; set; }
        public int NumeroNotaFiscal { get; set; }
        public int Serie { get; set; }
        public string NomeCliente { get; set; }

        public string EstadoDestino { get; set; }
        public string EstadoOrigem { get; set; }

        public IEnumerable<NotaFiscalItem> ItensDaNotaFiscal { get; set; }

        public NotaFiscal()
        {
            ItensDaNotaFiscal = new List<NotaFiscalItem>();
        }

        public int EmitirNotaFiscal(Pedido pedido)
        {

            NotaFiscalRepository nfRepository = new NotaFiscalRepository();
            this.NumeroNotaFiscal = 0;
            this.Serie = new Random().Next(Int32.MaxValue);
            this.NomeCliente = pedido.NomeCliente;

            this.EstadoDestino = pedido.EstadoDestino;
            this.EstadoOrigem = pedido.EstadoOrigem;


            string cfop = nfRepository.BuscaDefinicaoCFOP(this.EstadoOrigem, this.EstadoDestino);

            if (cfop != "")
            {
                foreach (PedidoItem itemPedido in pedido.ItensDoPedido)
                {
                    NotaFiscalItem notaFiscalItem = new NotaFiscalItem();


                    if (this.EstadoDestino == this.EstadoOrigem)
                    {
                        notaFiscalItem.TipoIcms = "60";
                        notaFiscalItem.AliquotaIcms = 0.18;
                    }
                    else
                    {
                        notaFiscalItem.TipoIcms = "10";
                        notaFiscalItem.AliquotaIcms = 0.17;
                    }
                    if (notaFiscalItem.Cfop == "6.009")
                    {
                        notaFiscalItem.BaseIcms = itemPedido.ValorItemPedido * 0.90; //redução de base
                    }
                    else
                    {
                        notaFiscalItem.BaseIcms = itemPedido.ValorItemPedido;
                    }
                    notaFiscalItem.ValorIcms = notaFiscalItem.BaseIcms * notaFiscalItem.AliquotaIcms;

                    if (itemPedido.Brinde)
                    {
                        //Cálculo ICMS
                        notaFiscalItem.TipoIcms = "60";
                        notaFiscalItem.AliquotaIcms = 0.18;
                        notaFiscalItem.ValorIcms = notaFiscalItem.BaseIcms * notaFiscalItem.AliquotaIcms;

                        //Cálculo IPI
                        notaFiscalItem.BaseIpi = itemPedido.ValorItemPedido;
                        notaFiscalItem.AliquotaIpi = 0.1;
                    }
                    notaFiscalItem.ValorIpi = notaFiscalItem.BaseIpi * notaFiscalItem.AliquotaIpi;

                    notaFiscalItem.NomeProduto = itemPedido.NomeProduto;
                    notaFiscalItem.CodigoProduto = itemPedido.CodigoProduto;
                }

                nfRepository.InserirPedido(this);

                //operação bem sucedida
                return 0;
            }

            else
            {
                // Erro de Estado/Destino não possui definição de CFOP
                return 1;
            }

        }


    }
}
