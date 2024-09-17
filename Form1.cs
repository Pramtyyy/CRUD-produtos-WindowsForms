using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CRUD_produtos
{
    public partial class Form1 : Form
    {

        string connectionString = "Data Source=COMPUTADOR;Initial Catalog=CRUD;Integrated Security=True;Encrypt=False";

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // TODO: esta linha de código carrega dados na tabela 'cRUDDataSet.Produtos'. Você pode movê-la ou removê-la conforme necessário.
            this.produtosTableAdapter.Fill(this.cRUDDataSet.Produtos);
            CarregarProdutos();
        }



        private void CarregarProdutos()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM Produtos";
                SqlDataAdapter da = new SqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dgvProdutos.DataSource = dt;
            }

        }

        private void AdicionarProdutos()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "INSERT INTO Produtos (Nome, Preco, Quantidade) VALUES (@Nome, @Preco, @Quantidade)";
                SqlCommand cmd = new SqlCommand(query, conn);

                cmd.Parameters.AddWithValue("@Nome", txtNome.Text);
                cmd.Parameters.AddWithValue("@Preco", decimal.Parse(txtPreco.Text));
                cmd.Parameters.AddWithValue("@Quantidade", int.Parse(txtQuantidade.Text));

                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();

                CarregarProdutos();
            }
        }

        private void EditarProduto(int id)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "UPDATE Produtos SET Nome = @Nome, Preco = @Preco, Quantidade = @Quantidade WHERE Id = @Id";
                SqlCommand cmd = new SqlCommand(query, conn);

                cmd.Parameters.AddWithValue("@Id", id);
                cmd.Parameters.AddWithValue("@Nome", txtNome.Text);
                cmd.Parameters.AddWithValue("@Preco", decimal.Parse(txtPreco.Text));
                cmd.Parameters.AddWithValue("@Quantidade", int.Parse(txtQuantidade.Text));

                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();

                CarregarProdutos();
            }

        }

        private void ExcluirProduto(int id)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "DELETE FROM Produtos WHERE Id = @Id";
                SqlCommand cmd = new SqlCommand(query, conn);

                cmd.Parameters.AddWithValue("Id", id);

                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();

                CarregarProdutos();

                

            }

        }

        private void btnAdicionar_Click(object sender, EventArgs e)
        {
            AdicionarProdutos();
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            if(dgvProdutos.SelectedRows.Count > 0)
            {
                int id = Convert.ToInt32(dgvProdutos.SelectedRows[0].Cells[0].Value);
                
                EditarProduto(id);
            }
        }

        private void btnExcluir_Click(object sender, EventArgs e)
        {
            if(dgvProdutos.SelectedRows.Count > 0)
            {
                int id = Convert.ToInt32(dgvProdutos.SelectedRows[0].Cells[0].Value);

                ExcluirProduto(id);
            }
        }

        private void btnLimpar_Click(object sender, EventArgs e)
        {
            txtNome.Clear();
            txtPreco.Clear();
            txtQuantidade.Clear();
        }
    }
}
