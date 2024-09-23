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
        private ErrorProvider errorProvider = new ErrorProvider();
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
            try
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
            catch (SqlException ex) {
                throw new
                Exception("Erro ao se conectar ao banco de dados: " + ex.Message); }
        }

        private void EditarProduto(int id)
        {
            try { 
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
            catch (SqlException ex)
            {
                throw new
                Exception("Erro ao se conectar ao banco de dados: " + ex.Message);
            }

        }

        private void ExcluirProduto(int id)
        {
            try { 
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
            catch (SqlException ex)
            {
                throw new
                Exception("Erro ao se conectar ao banco de dados: " + ex.Message);
            }
        }


        private void btnAdicionar_Click(object sender, EventArgs e)
        {


                if (string.IsNullOrWhiteSpace(txtNome.Text))
                {
                    errorProvider.SetError(txtNome, "Este campo e obrigatorio");
                }

                if (string.IsNullOrWhiteSpace(txtPreco.Text))
                {
                    errorProvider.SetError(txtPreco, "Este campo e obrigatorio");
                }

                if (string.IsNullOrWhiteSpace(txtQuantidade.Text))
                {
                    errorProvider.SetError(txtQuantidade, "Este campo e obrigatorio");
                }
            else { 
                errorProvider.SetError(txtNome, "");
                errorProvider.SetError(txtPreco, "");
                errorProvider.SetError(txtQuantidade, "");
            AdicionarProdutos();
            txtNome.Clear();
            txtPreco.Clear();
            txtQuantidade.Clear();
            }
        }


        private void btnEditar_Click(object sender, EventArgs e)
        {
            if(dgvProdutos.SelectedRows.Count > 0)
            {

                int id = Convert.ToInt32(dgvProdutos.SelectedRows[0].Cells[0].Value);
                
                EditarProduto(id);
                txtNome.Clear();
                txtPreco.Clear();
                txtQuantidade.Clear();
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

        private void txtNome_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void txtPreco_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(char.IsLetter(e.KeyChar)) { e.Handled = true; }
        }

        private void txtQuantidade_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsLetter(e.KeyChar)) { e.Handled = true; }
        }

        private void dgvProdutos_SelectionChanged(object sender, EventArgs e)
        {
            if(dgvProdutos.SelectedRows.Count > 0)
            {
                    var row = dgvProdutos.SelectedRows[0];
                    if (row.Cells[1].Value != null)
                    {
                        txtNome.Text = row.Cells[1].Value.ToString();
                    }

                    if (row.Cells[2].Value != null)
                    {
                        txtPreco.Text = row.Cells[2].Value.ToString();
                    }

                    if (row.Cells[3].Value != null)
                    {
                        txtQuantidade.Text = row.Cells[3].Value.ToString();
                    }
                
                txtNome.Text = dgvProdutos.SelectedRows[0].Cells[1].Value.ToString();
                txtPreco.Text = dgvProdutos.SelectedRows[0].Cells[2].Value.ToString();
                txtQuantidade.Text = dgvProdutos.SelectedRows[0].Cells[3].Value.ToString();
            }
        }
    }
}
