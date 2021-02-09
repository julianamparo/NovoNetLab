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

                    using (var cmdItem = Connection.CreateCommand())
                    {
                        foreach (var item in notaFiscal.ItensDaNotaFiscal)
                        {
                            cmdItem.CommandText = "dbo.p_nota_fiscal_item";
                            cmdItem.CommandType = CommandType.StoredProcedure;

                            cmdItem.Parameters.Add(new SqlParameter("@pId", item.Id));
                            cmdItem.Parameters.Add(new SqlParameter("@pIdNotaFiscal", item.IdNotaFiscal));
                            cmdItem.Parameters.Add(new SqlParameter("@pCfop", item.Cfop));
                            cmdItem.Parameters.Add(new SqlParameter("@pTipoIcms", item.TipoIcms));
                            cmdItem.Parameters.Add(new SqlParameter("@pBaseIcms", item.BaseIcms));
                            cmdItem.Parameters.Add(new SqlParameter("@pAliquotaIcms", item.AliquotaIcms));
                            cmdItem.Parameters.Add(new SqlParameter("@pValorIcms", item.TipoIcms));
                            cmdItem.Parameters.Add(new SqlParameter("@pNomeProduto", item.NomeProduto));
                            cmdItem.Parameters.Add(new SqlParameter("@pCodigoProduto", item.CodigoProduto));
                            cmdItem.Parameters.Add(new SqlParameter("@pBaseIpi", item.BaseIpi));
                            cmdItem.Parameters.Add(new SqlParameter("@pAliquotaIpi", item.AliquotaIpi));
                            cmdItem.Parameters.Add(new SqlParameter("@pValorIpi", item.ValorIpi));

                            cmdItem.ExecuteScalar();

                        }
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
                    cmd.CommandText = "select distinct estadoOrigem from definicaocfop";

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
                    cmd.CommandText = "select distinct estadoDestino from definicaocfop";
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

        public int RetornarMaxNumeroNota()
        {
            int numeroNotaFiscal = 0;
            //Chamar procedure que retorna os valores CFOP
            try
            {


                if (Connection.State != ConnectionState.Open)
                    Connection.Open();
                using (var cmd = Connection.CreateCommand())
                {
                    cmd.CommandText = "select max(numeronotafiscal) from notafiscal";
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        numeroNotaFiscal = reader.GetInt32(0);
                    }


                }
            }
            finally
            {
                if (Connection.State == ConnectionState.Open)
                    Connection.Close();
            }
            return numeroNotaFiscal;
        }

        public string BuscarDiretorioXML()
        {
            string diretorio = "";
            //Chamar procedure que retorna os valores CFOP
            try
            {
                if (Connection.State != ConnectionState.Open)
                    Connection.Open();
                using (var cmd = Connection.CreateCommand())
                {
                    cmd.CommandText = "select diretorio from diretorioconfig where nomediretorio = 'XML'";
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        diretorio = reader.GetString(0);
                    }
                }
            }
            finally
            {
                if (Connection.State == ConnectionState.Open)
                    Connection.Close();
            }
            return diretorio;
        }


    }
}
