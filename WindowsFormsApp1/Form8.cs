using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form8 : Form
    {
        public Form8()
        {
            InitializeComponent();
        }

       private void pictureBox2_Click(object sender, EventArgs e)
        {
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string accountNumber = textBox1.Text;

            if (string.IsNullOrWhiteSpace(accountNumber))
            {
                MessageBox.Show("Please enter the account number.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!Regex.IsMatch(accountNumber, @"^\d{10}$"))
            {
                MessageBox.Show("Invalid account number. It should be a 10-digit number.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            RetrieveAndDisplayData(accountNumber);
        }

        private void RetrieveAndDisplayData(string accountNumber)
        {
            string connectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\nalla\\Documents\\bankserver.mdf;Integrated Security=True;Connect Timeout=30";
            string query = "SELECT name, fname, mname, gender, mobile, branch, street, village, pin, state, amount, acc FROM coust WHERE acc = @acc";

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@acc", accountNumber);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                // Assuming you have 24 labels: label1, label2, ..., label24
                                // Update the labels with the retrieved data
                                label15.Text = reader["name"].ToString();
                                label16.Text = reader["fname"].ToString();
                                label17.Text = reader["mname"].ToString();
                                label18.Text = reader["gender"].ToString();
                                label19.Text = reader["mobile"].ToString();
                                label20.Text = reader["branch"].ToString();
                                label21.Text = reader["street"].ToString();
                                label22.Text = reader["village"].ToString();
                                label23.Text = reader["pin"].ToString();
                                label24.Text = reader["state"].ToString();
                                label25.Text = reader["amount"].ToString();
                                label26.Text = reader["acc"].ToString();

                                panel2.Visible = true;
                                panel1.Visible = true;
                            }
                            else
                            {
                                MessageBox.Show("Account number not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show("An SQL error occurred: " + ex.Message, "SQL Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    

        private void Form8_Load(object sender, EventArgs e)
        {
            panel2.Visible = false;
            panel1.Visible = true;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            this.Hide();
            Form1 form1 = new Form1();
            form1.ShowDialog();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            textBox1.Text = " ";
        }
    }
}
