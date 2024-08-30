using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Controls.Primitives;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
   
    public partial class Form5 : Form
    {
        public Form5()
        {
            InitializeComponent();

        }
        
        private void button3_Click(object sender, EventArgs e)
        {
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
        }

        private void button2_Click(object sender, EventArgs e)
        {
           
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void label18_Click(object sender, EventArgs e)
        {

        }
        private void buttonNotifications_Click(object sender, EventArgs e)
        {
           
        }

        private void LoadNotifications(string accountNumber)
        {
            string connectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\nalla\\Documents\\bankserver.mdf;Integrated Security=True;Connect Timeout=30";
            string query = "SELECT message, timestamp FROM Notifications WHERE accountNumber = @acc ORDER BY timestamp DESC";

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@acc", accountNumber);

                        using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                        {
                            DataTable notificationsTable = new DataTable();
                            adapter.Fill(notificationsTable);

                            dataGridViewNotifications.DataSource = notificationsTable;
                            dataGridViewNotifications.Visible = true; // Show notifications DataGridView
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

        private void button2_Click_1(object sender, EventArgs e)
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
                                panel1.Visible = true;
                                panel2.Visible = true;
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

        private void button3_Click_1(object sender, EventArgs e)
        {
            this.Hide();
            Form1 form1 = new Form1();
            form1.ShowDialog();
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            textBox1.Text = " ";
        }

        private void buttonDownload_Click(object sender, EventArgs e)
        {

        }

        private void Form5_Load(object sender, EventArgs e)
        {
            panel1.Visible = false;
            panel2.Visible = true;
            panel3.Visible = false;
            dataGridViewNotifications.Visible = false; // Hide notifications DataGridView initially

        }

        private void buttonDownload_Click_1(object sender, EventArgs e)
        {

            if (panel1.Visible)
            {
                using (SaveFileDialog saveFileDialog = new SaveFileDialog())
                {
                    // Set the default file name and filter
                    saveFileDialog.FileName = "\"C:\\Users\\nalla\\OneDrive\\Desktop\\my details\"";
                    saveFileDialog.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
                    saveFileDialog.Title = "Save Account Details";

                    // Show the save file dialog
                    if (saveFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        try
                        {
                            // Get the selected file path
                            string filePath = saveFileDialog.FileName;

                            using (StreamWriter sw = new StreamWriter(filePath))
                            {
                                // Write headers
                                sw.WriteLine("Account Details");
                                sw.WriteLine("------------------------");

                                // Write data
                                sw.WriteLine($"Name: {label15.Text}");
                                sw.WriteLine($"Father's Name: {label16.Text}");
                                sw.WriteLine($"Mother's Name: {label17.Text}");
                                sw.WriteLine($"Gender: {label18.Text}");
                                sw.WriteLine($"Mobile: {label19.Text}");
                                sw.WriteLine($"Branch: {label20.Text}");
                                sw.WriteLine($"Street: {label21.Text}");
                                sw.WriteLine($"Village: {label22.Text}");
                                sw.WriteLine($"Pin Code: {label23.Text}");
                                sw.WriteLine($"State: {label24.Text}");
                                sw.WriteLine($"Balance: {label25.Text}");
                                sw.WriteLine($"Account Number: {label26.Text}");

                                MessageBox.Show("Account details have been saved to " + filePath, "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("An error occurred while saving the file: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Please retrieve the account details first.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            panel3.Visible = true;
            panel1.Visible = true;

            // Populate text boxes in panel3 with current details
            textBoxName.Text = label15.Text;
            textBoxFName.Text = label16.Text;
            textBoxMName.Text = label17.Text;
            textBox5.Text = label19.Text;
            textBox7.Text = label21.Text;
            textBox8.Text = label22.Text;
            textBox9.Text = label23.Text;
            textBox10.Text = label24.Text;
        }
        private void button7_Click(object sender, EventArgs e)
        {
            string accountNumber = label26.Text; // Account number is read-only and not editable
            string name = textBoxName.Text;
            string fname = textBoxFName.Text;
            string mname = textBoxMName.Text;
            string gender = textBox2.Text;
            string mobile= textBox5.Text;
            string street = textBox7.Text;
            string village = textBox8.Text;
            string pin = textBox9.Text;
            string state= textBox10.Text;
            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(fname) || string.IsNullOrWhiteSpace(mname))
            {
                MessageBox.Show("Please fill in all the fields.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string connectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\nalla\\Documents\\bankserver.mdf;Integrated Security=True;Connect Timeout=30";
            string updateQuery = "UPDATE coust SET name = @name, fname = @fname, mname = @mname, mobile = @mobile, street = @street, village = @village, pin = @pin, state = @state,gender = @gender WHERE acc = @acc";

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(updateQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@name", name);
                        cmd.Parameters.AddWithValue("@fname", fname);
                        cmd.Parameters.AddWithValue("@mname", mname);
                        cmd.Parameters.AddWithValue("@mobile", mobile);
                        cmd.Parameters.AddWithValue("@state", state);
                        cmd.Parameters.AddWithValue("@village", village);
                        cmd.Parameters.AddWithValue("@pin", pin);
                        cmd.Parameters.AddWithValue("@street", street);
                        cmd.Parameters.AddWithValue("@gender", gender);
                        cmd.Parameters.AddWithValue("@acc", accountNumber);
                        int rowsAffected = cmd.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Details updated successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            // Update labels with new details
                            label15.Text = name;
                            label16.Text = fname;
                            label17.Text = mname;
                            label18.Text = gender;
                            label19.Text = mobile;
                            label21.Text = street;
                            label22.Text = village;
                            label23.Text = pin;
                            label24.Text = state;
                            panel1.Visible = true;
                            panel3.Visible = false;
                        }
                        else
                        {
                            MessageBox.Show("Update failed. Account number not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
        private void label27_Click(object sender, EventArgs e)
        {

        }
        private void label35_Click(object sender, EventArgs e)
        {

        }
        private void label32_Click(object sender, EventArgs e)
        {

        }

        private void button6_Click(object sender, EventArgs e)
        {
            this.Hide();
            Form2 form2 = new Form2();
            form2.ShowDialog();
        }

        private void button7_Click_1(object sender, EventArgs e)
        {

        }

        private void buttonNotifications_Click_1(object sender, EventArgs e)
        {
            string accountNumber = textBox1.Text;

            if (string.IsNullOrWhiteSpace(accountNumber))
            {
                MessageBox.Show("Please enter the account number.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            LoadNotifications(accountNumber);
        }

        private void dataGridViewNotifications_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
