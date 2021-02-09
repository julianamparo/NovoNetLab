using Imposto.Core.Service;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Imposto.Core.Domain;
using Imposto.Core.Data;

namespace TesteImposto
{
    public partial class FormImposto : Form
    {
        private Pedido pedido = new Pedido();
        public NotaFiscalService notaFiscalService;
        public FormImposto()
        {
            InitializeComponent();
            dataGridViewPedidos.AutoGenerateColumns = true;
            dataGridViewPedidos.DataSource = GetTablePedidos();
            notaFiscalService = new NotaFiscalService();
            ResizeColumns();
        }

        private void ResizeColumns()
        {
            double mediaWidth = dataGridViewPedidos.Width / dataGridViewPedidos.Columns.GetColumnCount(DataGridViewElementStates.Visible);

            for (int i = dataGridViewPedidos.Columns.Count - 1; i >= 0; i--)
            {
                var coluna = dataGridViewPedidos.Columns[i];
                coluna.Width = Convert.ToInt32(mediaWidth);
            }
        }

        private object GetTablePedidos()
        {
            DataTable table = new DataTable("pedidos");
            table.Columns.Add(new DataColumn("Nome do produto", typeof(string)));
            table.Columns.Add(new DataColumn("Codigo do produto", typeof(string)));
            table.Columns.Add(new DataColumn("Valor", typeof(decimal)));
            table.Columns.Add(new DataColumn("Brinde", typeof(bool)));

            return table;
        }

        private void buttonGerarNotaFiscal_Click(object sender, EventArgs e)
        {
            NotaFiscalService service = new NotaFiscalService();
            pedido.EstadoOrigem = cmbEstadosOrigem.Text;
            pedido.EstadoDestino = cmbEstadosDestino.Text;
            pedido.NomeCliente = textBoxNomeCliente.Text;

            DataTable table = (DataTable)dataGridViewPedidos.DataSource;

            foreach (DataRow row in table.Rows)
            {
                pedido.ItensDoPedido.Add(
                    new PedidoItem()
                    {
                        Brinde = Convert.ToBoolean(row["Brinde"]),
                        CodigoProduto = row["Codigo do produto"].ToString(),
                        NomeProduto = row["Nome do produto"].ToString(),
                        ValorItemPedido = Convert.ToDouble(row["Valor"].ToString())
                    });
            }
            int retorno = service.GerarNotaFiscal(pedido);
            if (retorno == 0)
            {
                MessageBox.Show("Operação concluída com sucesso");
                LimparForm();
            }
            if (retorno == 1)
                MessageBox.Show("Estado Origem/Destino sem definição de CFOP");

        }

        private void FormImposto_Load(object sender, EventArgs e)
        {
            GetTablePedidos();
            cmbEstadosOrigem.Items.Clear();
            //Popular combobox de estados de origem
            List<string> estados = notaFiscalService.PopularEstadosOrigem().ToList();
            foreach (string estado in estados)
            {
                cmbEstadosOrigem.Items.Add(estado);
            }

            cmbEstadosDestino.Items.Clear();
            //Popular combobox de estados de destino
            List<string> estadosDestino = notaFiscalService.PopularEstadosDestino();
            foreach (string estado in estadosDestino)
            {
                cmbEstadosDestino.Items.Add(estado);
            }
        }

        public void LimparForm()
        {
            textBoxNomeCliente.Text = "";
            cmbEstadosOrigem.SelectedIndex = -1;
            cmbEstadosDestino.SelectedIndex = -1;
            dataGridViewPedidos.Rows.Clear();

        }

        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult resultado = MessageBox.Show("Deseja limpar todo o preenchimento do formulário?", "Aviso", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (resultado == DialogResult.Yes)
            {
                LimparForm();
            }
        }
    }
}
