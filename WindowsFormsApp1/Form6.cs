﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form6 : Form
    {
        public Form6()
        {
            InitializeComponent();
        }

        private void label12_Click(object sender, EventArgs e) { }
        private void label11_Click(object sender, EventArgs e) { }

        private void button2_Click(object sender, EventArgs e)
        {
            if (ValidateDetails())
            {
                CompleteRegistration();
            }
        }

        private bool ValidateDetails()
        {
            if (string.IsNullOrWhiteSpace(textBox5.Text) || string.IsNullOrWhiteSpace(textBox4.Text) || string.IsNullOrWhiteSpace(textBox2.Text) ||
                string.IsNullOrWhiteSpace(textBox1.Text) || string.IsNullOrWhiteSpace(textBox3.Text) || string.IsNullOrWhiteSpace(comboBox1.Text) ||
                string.IsNullOrWhiteSpace(textBox7.Text) || string.IsNullOrWhiteSpace(textBox8.Text) || string.IsNullOrWhiteSpace(textBox9.Text) ||
                string.IsNullOrWhiteSpace(textBox10.Text) || string.IsNullOrWhiteSpace(textBox6.Text))
            {
                MessageBox.Show("Please fill all details.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (!Regex.IsMatch(textBox3.Text, @"^\d{10}$"))
            {
                MessageBox.Show("Invalid mobile number. It should be a 10-digit number.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (!Regex.IsMatch(textBox9.Text, @"^\d{6}$"))
            {
                MessageBox.Show("Invalid pin code. It should be a 6-digit number.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (!decimal.TryParse(textBox6.Text, out decimal amount) || amount < 500)
            {
                MessageBox.Show("Amount should be more than or equal to 500.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            return true;
        }

        private void CompleteRegistration()
        {
            Random ram = new Random();
            int accno = ram.Next(1000000000, 1999999999);

            // Bank code for IFSC (replace "NATB" with your bank's unique code)
            string bankCode = "NATB";
            string branchCode = GetBranchCode(comboBox1.SelectedItem.ToString()); // Branch code based on ComboBox selection
            string ifscCode = bankCode + "0" + branchCode; // Concatenating to form IFSC code

            string connectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\nalla\\Documents\\bankserver.mdf;Integrated Security=True;Connect Timeout=30";
            string query = "INSERT INTO coust (name, fname, mname, gender, mobile, branch, street, village, pin, state, amount, acc, ifsc) " +
                           "VALUES (@name, @fname, @mname, @gender, @mobile, @branch, @street, @village, @pin, @state, @amount, @acc, @ifsc)";

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@name", textBox5.Text);
                        cmd.Parameters.AddWithValue("@fname", textBox4.Text);
                        cmd.Parameters.AddWithValue("@mname", textBox2.Text);
                        cmd.Parameters.AddWithValue("@gender", textBox1.Text);

                        string mobile = textBox3.Text;
                        if (Regex.IsMatch(mobile, @"^\d{10}$"))
                        {
                            cmd.Parameters.AddWithValue("@mobile", mobile);
                        }
                        else
                        {
                            MessageBox.Show("Invalid mobile number. It should be a 10-digit number.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }

                        cmd.Parameters.AddWithValue("@branch", comboBox1.SelectedItem.ToString());
                        cmd.Parameters.AddWithValue("@street", textBox7.Text);
                        cmd.Parameters.AddWithValue("@village", textBox8.Text);

                        string pin = textBox9.Text;
                        if (Regex.IsMatch(pin, @"^\d{6}$"))
                        {
                            cmd.Parameters.AddWithValue("@pin", pin);
                        }
                        else
                        {
                            MessageBox.Show("Invalid pin code. It should be a 6-digit number.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }

                        cmd.Parameters.AddWithValue("@state", textBox10.Text);

                        if (decimal.TryParse(textBox6.Text, out decimal amount) && amount >= 500)
                        {
                            cmd.Parameters.AddWithValue("@amount", amount);
                        }
                        else
                        {
                            MessageBox.Show("Amount should be more than or equal to 500.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }

                        cmd.Parameters.AddWithValue("@acc", accno);
                        cmd.Parameters.AddWithValue("@ifsc", ifscCode); // Add the generated IFSC code

                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("YOUR ACCOUNT NUMBER IS " + accno + "\nIFSC Code: " + ifscCode, "Account Created", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            MessageBox.Show("Failed to create account. Please try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        private string GetBranchCode(string branchName)
        {
            // Dictionary to map branch names to codes
            Dictionary<string, string> branchCodes = new Dictionary<string, string>()
            {
                { "Guntur", "000001" },
                { "Vijayawada", "000002" },
                { "Vizag", "000003" },
                { "Nandigama", "000004" },
                { "Tenali", "000005" },
                { "Amaravathi", "000006" },
                { "Gannavaram", "000007" },
                { "Chennai", "000008" },
                { "Bengaluru", "000009" },
                { "Mumbai", "000010" }
            };

            // Return the branch code if found, otherwise return a default code
            if (branchCodes.ContainsKey(branchName))
            {
                return branchCodes[branchName];
            }

            // Default branch code if branch not found
            return "000000";
        }


        private void button1_Click(object sender, EventArgs e)
        {
            ClearForm();
        }

        private void ClearForm()
        {
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
            textBox4.Text = "";
            textBox5.Text = "";
            textBox6.Text = "";
            textBox7.Text = "";
            textBox8.Text = "";
            textBox9.Text = "";
            textBox10.Text = "";
            comboBox1.Text = "";
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Hide();
            Form1 f = new Form1();
            f.ShowDialog();
        }

        private void Form6_Load(object sender, EventArgs e)
        {
            // Any additional load actions can be placed here
        }
    }
}
