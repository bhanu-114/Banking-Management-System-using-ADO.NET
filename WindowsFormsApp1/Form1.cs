using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            panel1.Visible = false;
            panel2.Visible = false;
            panel3.Visible = false;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(comboBox1.SelectedIndex == 0)
            {
                panel1.Visible=true;
                panel2.Visible=false;
                panel3.Visible=false;
            }
            else if(comboBox1.SelectedIndex == 1)
            {
                panel1.Visible=false;
                panel2.Visible=true;
                panel3.Visible=false;
            }
            else if (comboBox1.SelectedIndex == 2)
            {
                panel1.Visible=false;
                panel2.Visible=false;
                panel3.Visible=true;
            }
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if(checkBox2.Checked == true)
            {
                textBox3.PasswordChar = '\0';
                checkBox2.Text = "Hide Password";
            }
            else
            {
                textBox3.PasswordChar = '*';
                checkBox2.Text = "Show Password";
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked == true)
            {
                txtPassword.PasswordChar = '\0';
                checkBox1.Text = "Hide Password";
            }
            else
            {
                txtPassword.PasswordChar = '*';
                checkBox1.Text = "Show Password";
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            this.Hide();
            mregistraton mregistraton = new mregistraton();
            mregistraton.ShowDialog();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text; // Replace with your actual textbox name
            string password = txtPassword.Text; // Replace with your actual textbox name

            // Ensure that username and password are not empty
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Please enter both username and password.", "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Connection string to your database
            string connectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\nalla\\Documents\\bankserver.mdf;Integrated Security=True;Connect Timeout=30";

            // SQL query to check if the user exists in the database
            string query = "SELECT COUNT(*) FROM managerl WHERE Username = @Username AND Password = @Password";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    // Open the connection
                    connection.Open();

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        // Add parameters to prevent SQL injection
                        command.Parameters.AddWithValue("@Username", username);
                        command.Parameters.AddWithValue("@Password", password);

                        // Execute the query and get the count of matching rows
                        int userCount = (int)command.ExecuteScalar();

                        // Check if user exists
                        if (userCount > 0)
                        {
                            // Hide the current form and show the next form
                            this.Hide();
                            Form3 nextForm = new Form3(); // Replace with your actual next form
                            nextForm.ShowDialog();
                        }
                        else
                        {
                            // Show an error message if credentials are invalid
                            MessageBox.Show("Invalid credentials. Please try again.", "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Handle any potential errors
                    MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string username =textBox4.Text; // Replace with your actual textbox name
            string password = textBox3.Text; // Replace with your actual textbox name
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Please enter both username and password.", "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            // Connection string to your database
            string connectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\nalla\\Documents\\bankserver.mdf;Integrated Security=True;Connect Timeout=30";

            // SQL query to check if the user exists in the database
            string query = "SELECT COUNT(*) FROM employee WHERE Username = @Username AND Password = @Password";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    // Open the connection
                    connection.Open();

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        // Add parameters to prevent SQL injection
                        command.Parameters.AddWithValue("@Username", username);
                        command.Parameters.AddWithValue("@Password", password);

                        // Execute the query
                        int userCount = (int)command.ExecuteScalar();

                        // Check if user exists
                        if (userCount > 0)
                        {
                            // Hide the current form and show the next form
                            this.Hide();
                            Form8 f = new Form8(); // Replace with your next form
                            f.ShowDialog();
                        }
                        else
                        {
                            // Show an error message if credentials are invalid
                            MessageBox.Show("Invalid credentials. Please try again.", "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Handle any potential errors
                    MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Form6 f = new Form6();
            f.ShowDialog();
            this.Hide();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Hide();
            Form5 f = new Form5();
            f.ShowDialog();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            this.Hide();
            employereg employereg = new employereg();
            employereg.ShowDialog();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            this.Hide();
            Form4 f = new Form4();
            f.ShowDialog();
        }
    }
    
}
