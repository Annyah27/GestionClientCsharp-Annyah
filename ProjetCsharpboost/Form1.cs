using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace ProjetCsharpboost
{
    public partial class Form1 : Form
    {
        private string connectionString = @"Data Source=(LocalDB)\v11.0;AttachDbFilename=c:\users\pisokelyy\documents\visual studio 2012\Projects\ProjetCsharpboost\ProjetCsharpboost\Database1.mdf;Integrated Security=True";
        private int clientIdSelectionne = -1;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView1.MultiSelect = false;
            dataGridView1.ReadOnly = true;
            AfficherClients();
            dataGridView1.CellClick += dataGridView1_CellClick;
        }


        private void AfficherClients()
        {
            string query = "SELECT Id, nom, prenom, adresse, phone FROM Client";
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                using (SqlDataAdapter adapter = new SqlDataAdapter(query, con))
                {
                    DataTable table = new DataTable();
                    adapter.Fill(table);
                    dataGridView1.DataSource = table;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur de chargement : " + ex.Message);
            }
        }


        private void ViderChamps()
        {
            txtNom.Clear();
            txtPrenom.Clear();
            txtAdresse.Clear();
            txtPhone.Clear();
            clientIdSelectionne = -1;
        }

        private void btn_Ajouter_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNom.Text) || string.IsNullOrWhiteSpace(txtPrenom.Text) ||
                string.IsNullOrWhiteSpace(txtAdresse.Text) || string.IsNullOrWhiteSpace(txtPhone.Text))
            {
                MessageBox.Show("Veuillez remplir tous les champs.");
                return;
            }

            string query = "INSERT INTO Client (nom, prenom, adresse, phone) VALUES (@nom, @prenom, @adresse, @phone)";

            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@nom", txtNom.Text.Trim());
                    cmd.Parameters.AddWithValue("@prenom", txtPrenom.Text.Trim());
                    cmd.Parameters.AddWithValue("@adresse", txtAdresse.Text.Trim());
                    cmd.Parameters.AddWithValue("@phone", txtPhone.Text.Trim());
                    con.Open();
                    cmd.ExecuteNonQuery();
                }
                MessageBox.Show("Client ajouté !");
                AfficherClients();
                ViderChamps();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur ajout : " + ex.Message);
            }
        }


        private void btn_Modifier_Click(object sender, EventArgs e)
        {
            if (clientIdSelectionne == -1)
            {
                MessageBox.Show("Veuillez sélectionner un client à modifier.");
                return;
            }

            if (string.IsNullOrWhiteSpace(txtNom.Text) || string.IsNullOrWhiteSpace(txtPrenom.Text) ||
                string.IsNullOrWhiteSpace(txtAdresse.Text) || string.IsNullOrWhiteSpace(txtPhone.Text))
            {
                MessageBox.Show("Tous les champs sont obligatoires.");
                return;
            }
            string query = "UPDATE Client SET nom = @nom, prenom = @prenom, adresse = @adresse, phone = @phone WHERE Id = @id";
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@nom", txtNom.Text.Trim());
                    cmd.Parameters.AddWithValue("@prenom", txtPrenom.Text.Trim());
                    cmd.Parameters.AddWithValue("@adresse", txtAdresse.Text.Trim());
                    cmd.Parameters.AddWithValue("@phone", txtPhone.Text.Trim());
                    cmd.Parameters.AddWithValue("@id", clientIdSelectionne);
                    con.Open();
                    cmd.ExecuteNonQuery();
                }
                MessageBox.Show("Client modifié avec succès !");
                AfficherClients();
                ViderChamps();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur modification : " + ex.Message);
            }
        }
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];
                if (row.Cells[0].Value != DBNull.Value && row.Cells[0].Value != null)
                    clientIdSelectionne = Convert.ToInt32(row.Cells[0].Value);

                txtNom.Text = row.Cells[1].Value != DBNull.Value && row.Cells[1].Value != null
                    ? row.Cells[1].Value.ToString() : "";

                txtPrenom.Text = row.Cells[2].Value != DBNull.Value && row.Cells[2].Value != null
                    ? row.Cells[2].Value.ToString() : "";

                txtAdresse.Text = row.Cells[3].Value != DBNull.Value && row.Cells[3].Value != null
                    ? row.Cells[3].Value.ToString() : "";

                txtPhone.Text = row.Cells[4].Value != DBNull.Value && row.Cells[4].Value != null
                    ? row.Cells[4].Value.ToString() : "";
            }
        }
        private void btn_Supprimer_Click(object sender, EventArgs e)
        {
            if (clientIdSelectionne == -1)
            {
                MessageBox.Show("Veuillez sélectionner un client à supprimer.");
                return;
            }
            DialogResult result = MessageBox.Show("Êtes-vous sûr de vouloir supprimer ce client ?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (result == DialogResult.Yes)
            {
                string query = "DELETE FROM Client WHERE Id = @id";

                try
                {
                    using (SqlConnection con = new SqlConnection(connectionString))
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@id", clientIdSelectionne);
                        con.Open();
                        cmd.ExecuteNonQuery();
                    }

                    MessageBox.Show("Client supprimé avec succès !");
                    AfficherClients();
                    ViderChamps();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erreur lors de la suppression : " + ex.Message);
                }
            }
        }

        private void btn_Annuler_Click(object sender, EventArgs e)
        {
            ViderChamps();
        }

    }
}
