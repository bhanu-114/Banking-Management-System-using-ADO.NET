using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class employereg : Form
    {
        public employereg()
        {
            InitializeComponent();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text; // Replace with your actual textbox name
            string password = txtPassword.Text; // Replace with your actual textbox name
            string confirmPassword = txtConfirmPassword.Text; // Replace with your actual textbox name
            string fullName = txtFullName.Text; // Replace with your actual textbox name
            string email = txtEmail.Text; // Replace with your actual textbox name
            string phone = txtPhone.Text; // Replace with your actual textbox name

            // Check if passwords match
            if (password != confirmPassword)
            {
                MessageBox.Show("Passwords do not match!", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Validate email format using regular expressions
            if (!IsValidEmail(email))
            {
                MessageBox.Show("Invalid email format!", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Validate phone number format using regular expressions
            if (!IsValidPhoneNumber(phone))
            {
                MessageBox.Show("Invalid phone number format! Phone number should be 10 digits.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Connection string to your database
            string connectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\nalla\\Documents\\bankserver.mdf;Integrated Security=True;Connect Timeout=30";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                // Start a transaction
                SqlTransaction transaction = connection.BeginTransaction();

                try
                {
                    // Insert into ManagerLogin
                    string loginQuery = "INSERT INTO employee (Username, Password) VALUES (@Username, @Password)";
                    using (SqlCommand loginCommand = new SqlCommand(loginQuery, connection, transaction))
                    {
                        loginCommand.Parameters.AddWithValue("@Username", username); // Set username parameter
                        loginCommand.Parameters.AddWithValue("@Password", password); // Set password parameter
                        loginCommand.ExecuteNonQuery();
                    }

                    // Insert into ManagerRegistration
                    string registrationQuery = "INSERT INTO employeer (Username, FullName, Email, Phone) VALUES (@Username, @FullName, @Email, @Phone)";
                    using (SqlCommand registrationCommand = new SqlCommand(registrationQuery, connection, transaction))
                    {
                        registrationCommand.Parameters.AddWithValue("@Username", username); // Set username parameter
                        registrationCommand.Parameters.AddWithValue("@FullName", fullName); // Set full name parameter
                        registrationCommand.Parameters.AddWithValue("@Email", email); // Set email parameter
                        registrationCommand.Parameters.AddWithValue("@Phone", phone); // Set phone parameter
                        registrationCommand.ExecuteNonQuery();
                    }

                    // Commit the transaction
                    transaction.Commit();
                    MessageBox.Show("Registration successful!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    // Rollback the transaction if an error occurs
                    transaction.Rollback();
                    MessageBox.Show("Registration failed: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        // Method to validate email format
        private bool IsValidEmail(string email)
        {
            // Regular expression pattern for valid email format
            string emailPattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            return Regex.IsMatch(email, emailPattern);
        }

        // Method to validate phone number format
        private bool IsValidPhoneNumber(string phone)
        {
            // Regular expression pattern for valid phone number format (10 digits)
            string phonePattern = @"^\d{10}$";
            return Regex.IsMatch(phone, phonePattern);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();
            Form1 form = new Form1();
            form.ShowDialog();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            txtUsername.Text = "";
            txtPassword.Text = "";
            txtConfirmPassword.Text = "";
            txtFullName.Text = "";
            txtPhone.Text = "";
            txtEmail.Text = "";
        }
    }
}
