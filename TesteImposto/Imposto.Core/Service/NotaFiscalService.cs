using Imposto.Core.Data;
using Imposto.Core.Domain;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Imposto.Core.Service
{
    public class NotaFiscalService
    {
        public int GerarNotaFiscal(Domain.Pedido pedido)
        {
            NotaFiscal notaFiscal = new NotaFiscal();
           return notaFiscal.EmitirNotaFiscal(pedido);
        }

        public List<string> PopularEstadosOrigem()
        {
           
            List<string> listaEstadosOrigem = new List<string>();
            NotaFiscalRepository nfRepository = new NotaFiscalRepository();
            DataTable dtEstadosOrigem = nfRepository.PopularEstadosOrigem();

            foreach(DataRow row in dtEstadosOrigem.Rows)
            {
                listaEstadosOrigem.Add(row[0].ToString());
            }

            return listaEstadosOrigem;
        }

        public List<string> PopularEstadosDestino()
        {
            List<string> listaEstadosDestino = new List<string>();
            NotaFiscalRepository nfRepository = new NotaFiscalRepository();
            DataTable dtEstadosDestino = nfRepository.PopularEstadosDestino();

            foreach (DataRow row in dtEstadosDestino.Rows)
            {
                listaEstadosDestino.Add(row[0].ToString());
            }

            return listaEstadosDestino;
        }
    }
}
