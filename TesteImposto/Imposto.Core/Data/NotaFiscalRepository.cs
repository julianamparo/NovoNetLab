using Imposto.Core.Domain;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Imposto.Core.Data
{
    public class NotaFiscalRepository
    {
        private static string server = @"(local)\MSSQLSERVER02";
        private static string database = "teste";
        private static string user = "sa";
        private static string password = "burger";
        private SqlConnection Connection;

        private string ConnectionString = $"Server={server};Database={database};User Id={user};Password={password};";

        public NotaFiscalRepository()
        {
            //Conexao do banco de dados
            Connection = new SqlConnection(ConnectionString);
            Connection.Open();
        }

        public void InserirPedido(NotaFiscal notaFiscal)
        {
            //Chamar procedure que retorna os valores CFOP
            try
            {
                if (Connection.State != ConnectionState.Open)
                    Connection.Open();
                using (var cmd = Connection.CreateCommand())
                {
                    cmd.CommandText = "dbo.p_nota_fiscal";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@pId", notaFiscal.Id));
                    cmd.Parameters.Add(new SqlParameter("@pNumeroNotaFiscal", notaFiscal.NumeroNotaFiscal));
                    cmd.Parameters.Add(new SqlParameter("@pSerie", notaFiscal.Serie));
                    cmd.Parameters.Add(new SqlParameter("@pNomeCliente", notaFiscal.NomeCliente));
                    cmd.Parameters.Add(new SqlParameter("@pEstadoDestino", notaFiscal.EstadoDestino));
                    cmd.Parameters.Add(new SqlParameter("@pEstadoOrigem", notaFiscal.EstadoOrigem));

                    cmd.ExecuteScalar();

                    foreach (var item in notaFiscal.ItensDaNotaFiscal)
                    {
                        cmd.CommandText = "dbo.p_nota_fiscal_item";
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@pId", item.Id));
                        cmd.Parameters.Add(new SqlParameter("@pCfop", item.Cfop));
                        cmd.Parameters.Add(new SqlParameter("@pTipoIcms", item.TipoIcms));
                        cmd.Parameters.Add(new SqlParameter("@pBaseIcms", item.BaseIcms));
                        cmd.Parameters.Add(new SqlParameter("@pAliquotaIcms", item.AliquotaIcms));
                        cmd.Parameters.Add(new SqlParameter("@pTipoIcms", item.TipoIcms));
                        cmd.Parameters.Add(new SqlParameter("@pBaseIcms", item.BaseIcms));
                        cmd.Parameters.Add(new SqlParameter("@pAliquotaIcms", item.AliquotaIcms));
                        cmd.Parameters.Add(new SqlParameter("@@pValorIcms", item.ValorIcms));


                        cmd.Parameters.Add(new SqlParameter("@pNomeProduto", item.NomeProduto));
                        cmd.Parameters.Add(new SqlParameter("@pCodigoProduto", item.CodigoProduto));


                        cmd.ExecuteScalar();
                    }
                }
            }
            finally
            {
                if (Connection.State == ConnectionState.Open)
                    Connection.Close();
            }

        }

        public string BuscaDefinicaoCFOP(string estadoOrigem, string estadoDestino)
        {
            //Chamar function que retorna o valor de CFOP a partir dos Estados de origem e destino
            try
            {
                if (Connection.State != ConnectionState.Open)
                    Connection.Open();

                using (var cmd = Connection.CreateCommand())
                {
                    cmd.CommandText = "dbo.funcBuscarCfop";
                    cmd.CommandType = CommandType.StoredProcedure;

                    SqlParameter paramOrigem = new SqlParameter("@EstadoOrigem", estadoOrigem);
                    SqlParameter paramDestino = new SqlParameter("@EstadoDestino", estadoDestino);

                    cmd.Parameters.Add(paramOrigem);
                    cmd.Parameters.Add(paramDestino);

                    var CFOP = new SqlParameter("CFOP", SqlDbType.VarChar)
                    {
                        //Set this property as return value
                        Direction = ParameterDirection.ReturnValue
                    };

                    cmd.Parameters.Add(CFOP);
                    cmd.ExecuteScalar();

                    string retorno = CFOP.Value.ToString();
                    
                    return retorno;
                }
            }
            finally
            {
                if (Connection.State == ConnectionState.Open)
                    Connection.Close();
            }

        }

        public DataTable RetornarPedidos()
        {
            //Chamar procedure que retorna os valores CFOP
            try
            {
                if (Connection.State != ConnectionState.Open)
                    Connection.Open();
                using (var cmd = Connection.CreateCommand())
                {
                    cmd.CommandText = "dbo.procBuscaValorCFOP";
                    cmd.CommandType = CommandType.StoredProcedure;
                    SqlDataAdapter dataAdapter = new SqlDataAdapter(cmd);
                    DataTable dados = new DataTable();
                    dataAdapter.Fill(dados);
                    return dados;
                }
            }
            finally
            {
                if (Connection.State == ConnectionState.Open)
                    Connection.Close();
            }

        }
       
        public DataTable PopularEstadosOrigem()
        {
            //Chamar procedure que retorna os valores CFOP
            try
            {
                if (Connection.State != ConnectionState.Open)
                    Connection.Open();
                using (var cmd = Connection.CreateCommand())
                {
                    cmd.CommandText = "select distinct estadoOrigem from notafiscal";
                    
                    SqlDataAdapter dataAdapter = new SqlDataAdapter(cmd);
                    DataTable dados = new DataTable();
                    dataAdapter.Fill(dados);
                    return dados;
                }
            }
            finally
            {
                if (Connection.State == ConnectionState.Open)
                    Connection.Close();
            }

        }

        public DataTable PopularEstadosDestino()
        {
            //Chamar procedure que retorna os valores CFOP
            try
            {
                if (Connection.State != ConnectionState.Open)
                    Connection.Open();
                using (var cmd = Connection.CreateCommand())
                {
                    cmd.CommandText = "select distinct estadoDestino from notafiscal";
                    SqlDataAdapter dataAdapter = new SqlDataAdapter(cmd);
                    DataTable dados = new DataTable();
                    dataAdapter.Fill(dados);
                    return dados;
                }
            }
            finally
            {
                if (Connection.State == ConnectionState.Open)
                    Connection.Close();
            }

        }
    }
}
