using Imposto.Core.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

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

                int sucesso = GerarXML(this);

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

        public int GerarXML(NotaFiscal notafiscal)
        {
            try
            {
                XmlTextWriter xml = new XmlTextWriter(@"C:\Users\julia\source\repos\NovoNetLab\TesteImposto\XML\NotaFiscal_" + notafiscal.Serie + ".xml", System.Text.Encoding.UTF8);
                xml.WriteStartDocument(true);
                xml.Formatting = Formatting.Indented;
                xml.Indentation = 2;
                xml.WriteStartElement("NotaFiscal");

                xml.WriteStartElement("NumeroNotaFiscal");
                xml.WriteString(notafiscal.NumeroNotaFiscal.ToString());
                xml.WriteEndElement();

                xml.WriteStartElement("Serie");
                xml.WriteString(notafiscal.Serie.ToString());
                xml.WriteEndElement();

                xml.WriteStartElement("NomeCliente");
                xml.WriteString(notafiscal.NomeCliente.ToString());
                xml.WriteEndElement();

                xml.WriteStartElement("EstadoOrigem");
                xml.WriteString(notafiscal.EstadoOrigem.ToString());
                xml.WriteEndElement();

                xml.WriteStartElement("EstadoDestino");
                xml.WriteString(notafiscal.EstadoDestino.ToString());
                xml.WriteEndElement();

                xml.WriteStartElement("NumeroNotaFiscal");
                xml.WriteString(notafiscal.NumeroNotaFiscal.ToString());
                xml.WriteEndElement();

                xml.WriteStartElement("ItensPedido");

                foreach (var item in notafiscal.ItensDaNotaFiscal)
                {
                    xml.WriteStartElement("IdNotaFiscal");
                    xml.WriteString(item.IdNotaFiscal.ToString());
                    xml.WriteEndElement();

                    xml.WriteStartElement("Cfop");
                    xml.WriteString(item.Cfop.ToString());
                    xml.WriteEndElement();

                    xml.WriteStartElement("TipoIcms");
                    xml.WriteString(item.TipoIcms.ToString());
                    xml.WriteEndElement();

                    xml.WriteStartElement("BaseIcms");
                    xml.WriteString(item.BaseIcms.ToString());
                    xml.WriteEndElement();

                    xml.WriteStartElement("AliquotaIcms");
                    xml.WriteString(item.AliquotaIcms.ToString());
                    xml.WriteEndElement();

                    xml.WriteStartElement("ValorIcms");
                    xml.WriteString(item.ValorIcms.ToString());
                    xml.WriteEndElement();

                    xml.WriteStartElement("NomeProduto");
                    xml.WriteString(item.NomeProduto.ToString());
                    xml.WriteEndElement();

                    xml.WriteStartElement("CodigoProduto");
                    xml.WriteString(item.CodigoProduto.ToString());
                    xml.WriteEndElement();

                    xml.WriteStartElement("BaseIpi");
                    xml.WriteString(item.BaseIpi.ToString());
                    xml.WriteEndElement();

                    xml.WriteStartElement("AliquotaIpi");
                    xml.WriteString(item.AliquotaIpi.ToString());
                    xml.WriteEndElement();

                    xml.WriteStartElement("ValorIpi");
                    xml.WriteString(item.ValorIpi.ToString());
                    xml.WriteEndElement();
                }


                xml.WriteEndElement();
                xml.WriteEndDocument();

                xml.Flush();
                xml.Close();

                return 0;
            }
            catch(Exception ex)
            {
                return 1;
            }
        }
    }
}
