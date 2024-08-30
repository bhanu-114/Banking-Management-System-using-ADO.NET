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
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            string accountNumber = txtUsername.Text;
            string depositAmountText = textBox1.Text;

            if (string.IsNullOrWhiteSpace(accountNumber))
            {
                MessageBox.Show("Please enter the account number.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (string.IsNullOrWhiteSpace(depositAmountText) || !decimal.TryParse(depositAmountText, out decimal depositAmount) || depositAmount <= 0)
            {
                MessageBox.Show("Invalid deposit amount. Please enter a positive number.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            DepositAmount(accountNumber, depositAmount);
            //CalculateCreditScore(accountNumber);
        }

        private void DepositAmount(string accountNumber, decimal depositAmount)
        {
            string connectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\nalla\\Documents\\bankserver.mdf;Integrated Security=True;Connect Timeout=30";
            string query = "UPDATE coust SET amount = amount + @depositAmount WHERE acc = @acc";

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@depositAmount", depositAmount);
                        cmd.Parameters.AddWithValue("@acc", accountNumber);

                        int rowsAffected = cmd.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Deposit successful!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                            decimal newBalance = GetAccountBalance(accountNumber);
                            string branchName = GetBranchName(accountNumber);

                            string notificationMessage = $"Rs.{depositAmount} Credited to INB-{accountNumber.Substring(accountNumber.Length - 4)} AcBal:{newBalance} CLRBal:{newBalance} ({branchName}) on {DateTime.Now:dd-MM-yyyy HH:mm:ss}.INB.";
                            SaveNotification(accountNumber, notificationMessage);
                            SaveCreditDebitTransaction(accountNumber, "Credit", depositAmount);
                            int creditScore = CalculateCreditScore(accountNumber);
                            MessageBox.Show($"Updated Credit Score: {creditScore}", "Credit Score", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            MessageBox.Show("Account number not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        private void button4_Click(object sender, EventArgs e)
        {
            string accountNumber = textBox3.Text;
            string withdrawAmountText = textBox2.Text;

            if (string.IsNullOrWhiteSpace(accountNumber))
            {
                MessageBox.Show("Please enter the account number.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (string.IsNullOrWhiteSpace(withdrawAmountText) || !decimal.TryParse(withdrawAmountText, out decimal withdrawAmount) || withdrawAmount <= 0)
            {
                MessageBox.Show("Invalid withdrawal amount. Please enter a positive number.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            WithdrawAmount(accountNumber, withdrawAmount);
        }

        private void WithdrawAmount(string accountNumber, decimal withdrawAmount)
        {
            string connectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\nalla\\Documents\\bankserver.mdf;Integrated Security=True;Connect Timeout=30";
            string query = "UPDATE coust SET amount = amount - @withdrawAmount WHERE acc = @accountNumber AND amount >= @withdrawAmount";

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@withdrawAmount", withdrawAmount);
                        cmd.Parameters.AddWithValue("@accountNumber", accountNumber);

                        int rowsAffected = cmd.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Withdrawal successful!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                            decimal newBalance = GetAccountBalance(accountNumber);
                            string branchName = GetBranchName(accountNumber);

                            string notificationMessage = $"Rs.{withdrawAmount} Debited from INB-{accountNumber.Substring(accountNumber.Length - 4)} AcBal:{newBalance} CLRBal:{newBalance} ({branchName}) on {DateTime.Now:dd-MM-yyyy HH:mm:ss}.INB.";
                            SaveNotification(accountNumber, notificationMessage);
                            int creditScore = CalculateCreditScore(accountNumber);
                            MessageBox.Show($"Updated Credit Score: {creditScore}", "Credit Score", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            SaveCreditDebitTransaction(accountNumber, "Debit", withdrawAmount);
                        }
                        else
                        {
                            MessageBox.Show("Insufficient balance or account number not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        private decimal GetAccountBalance(string accountNumber)
        {
            string connectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\nalla\\Documents\\bankserver.mdf;Integrated Security=True;Connect Timeout=30";
            string query = "SELECT amount FROM coust WHERE acc = @acc";
            decimal balance = 0;

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@acc", accountNumber);
                        object result = cmd.ExecuteScalar();
                        if (result != null && result != DBNull.Value)
                        {
                            // Use Convert.ToDecimal to safely cast to decimal
                            balance = Convert.ToDecimal(result);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return balance;
        }

        private string GetBranchName(string accountNumber)
        {
            string connectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\nalla\\Documents\\bankserver.mdf;Integrated Security=True;Connect Timeout=30";
            string query = "SELECT branch FROM coust WHERE acc = @acc";
            string branchName = "";

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@acc", accountNumber);
                        object result = cmd.ExecuteScalar();
                        if (result != null && result != DBNull.Value)
                        {
                            branchName = result.ToString();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return branchName;
        }

        private void SaveNotification(string accountNumber, string message)
        {
            string connectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\nalla\\Documents\\bankserver.mdf;Integrated Security=True;Connect Timeout=30";
            string query = "INSERT INTO Notifications (accountNumber, message) VALUES (@acc, @message)";

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@acc", accountNumber);
                        cmd.Parameters.AddWithValue("@message", message);

                        cmd.ExecuteNonQuery();
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

        private void SaveCreditDebitTransaction(string accountNumber, string transactionType, decimal amount)
        {
            string connectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\nalla\\Documents\\bankserver.mdf;Integrated Security=True;Connect Timeout=30";
            string query = "INSERT INTO CreditDebit (accountNumber, transactionType, amount, timestamp) VALUES (@acc, @transactionType, @amount, @timestamp)";

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@acc", accountNumber);
                        cmd.Parameters.AddWithValue("@transactionType", transactionType);
                        cmd.Parameters.AddWithValue("@amount", amount);
                        cmd.Parameters.AddWithValue("@timestamp", DateTime.Now);
                        cmd.ExecuteNonQuery();
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
        private void button8_Click(object sender, EventArgs e)
        {
            panel5.Visible = true;// Show the panel for account number input
            panel2.Visible = false;
            panel3.Visible = false;
            panel4.Visible = false;
            panel6.Visible = false;
            panel7.Visible = false;
            panel9.Visible = false;
        }

        private void button9_Click(object sender, EventArgs e)
        {
            panel6.Visible = true;
            panel5.Visible = false;// Show the panel for account number input
            panel2.Visible = false;
            panel3.Visible = false;
            panel4.Visible = false;// Show the panel for account number input for debit card
            panel7.Visible = false;
            panel9.Visible=false;

        }
       
        private void NotifyManagerAboutCreditCardRequest(string accountNumber)
        {
            string connectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\nalla\\Documents\\bankserver.mdf;Integrated Security=True;Connect Timeout=30";
            string query = "INSERT INTO BankNotifications (Message, timestamp, accountNumber) VALUES (@message, @timestamp, @accountNumber)";

            string message = $"{accountNumber} has requested a credit card.";
            DateTime now = DateTime.Now;

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@message", message);
                        cmd.Parameters.AddWithValue("@timestamp", now);
                        cmd.Parameters.AddWithValue("@accountNumber", accountNumber);
                        cmd.ExecuteNonQuery();
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
        private bool CheckEligibilityForCreditCard(string accountNumber)
        {
                string connectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\nalla\\Documents\\bankserver.mdf;Integrated Security=True;Connect Timeout=30";
                string query = "SELECT SUM(amount) FROM creditdebit WHERE accountNumber = @acc AND MONTH(timestamp) = MONTH(GETDATE())";

                try
                {
                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        conn.Open();
                        using (SqlCommand cmd = new SqlCommand(query, conn))
                        {
                            cmd.Parameters.AddWithValue("@acc", accountNumber);

                            decimal totalCreditAmount = (decimal)cmd.ExecuteScalar();
                            return totalCreditAmount > 50000;
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            

        }
        private string GetUserName(string accountNumber)
        {
            string connectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\nalla\\Documents\\bankserver.mdf;Integrated Security=True;Connect Timeout=30";
            string query = "SELECT name FROM coust WHERE acc = @acc";
            string userName = "";

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@acc", accountNumber);
                        userName = (string)cmd.ExecuteScalar();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return userName;
        }

       

        private void button5_Click(object sender, EventArgs e)
        {
            // Add your button5 click event logic here, or leave empty if not needed.
        }
        private void Form2_Load(object sender, EventArgs e)
        {
            panel2.Visible = false;
            panel3.Visible = false;
            panel4.Visible = false;
            panel5.Visible = false;
            panel6.Visible = false;
            panel7.Visible = false;
            panel9.Visible = false;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            panel2.Visible = false;
            panel3.Visible = true;
            panel4.Visible = false;
            panel5.Visible = false;
            panel6.Visible = false;
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            panel2.Visible = true;
            panel3.Visible = false;
            panel4.Visible = false;
            panel5.Visible = false;
            panel6.Visible = false;
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            panel2.Visible = false;
            panel3.Visible = false;
            panel4.Visible = true;
            panel5.Visible = false;
            panel6.Visible = false;
        }

        private void button7_Click(object sender, EventArgs e)
        {
            this.Hide();
            Form5 form = new Form5();
            form.ShowDialog();
        }

        private void submitDebitCardButton_Click_1(object sender, EventArgs e)
        {
            string accountNumber = txtDebitCardAccountNumber.Text;

            if (string.IsNullOrWhiteSpace(accountNumber))
            {
                MessageBox.Show("Please enter the account number.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string connectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\nalla\\Documents\\bankserver.mdf;Integrated Security=True;Connect Timeout=30";
            string checkQuery = "SELECT cardNo, expDate, cvc FROM debitdetails WHERE accountNumber = @acc";
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    using (SqlCommand checkCmd = new SqlCommand(checkQuery, conn))
                    {
                        checkCmd.Parameters.AddWithValue("@acc", accountNumber);
                        using (SqlDataReader reader = checkCmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                // Debit card already exists, display it
                                string cardNumber = reader["cardNo"].ToString();
                                DateTime expiryDate = (DateTime)reader["expDate"];
                                string formattedCardNumber = FormatCardNumber(cardNumber);

                                label27.Text = formattedCardNumber;
                                label26.Text = expiryDate.ToString("yyyy-MM-dd");
                                label22.Text = reader["cvc"].ToString();
                                label24.Text = GetUserName(accountNumber);
                                panel7.Visible = true;
                                panel6.Visible = false;

                                MessageBox.Show("Debit card found and displayed.", "Card Found", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                            else
                            {
                                // Debit card does not exist, generate a new one
                                GenerateDebitCard(accountNumber);
                                MessageBox.Show("Debit card generated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void GenerateDebitCard(string accountNumber)
        {
            Random rnd = new Random();
            string debitCardNumber = rnd.Next(100000000, 999999999).ToString("D16");
            string cvc = rnd.Next(100, 999).ToString("D3");
            DateTime expiryDate = DateTime.Now.AddYears(5);
            string holderName = GetUserName(accountNumber);
            string connectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\nalla\\Documents\\bankserver.mdf;Integrated Security=True;Connect Timeout=30";
            string query = "INSERT INTO debitdetails (accountNumber, cardNo, cvc, expDate, holderName) VALUES (@acc, @cardNo, @cvc, @expDate, @holderName)";

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@acc", accountNumber);
                        cmd.Parameters.AddWithValue("@cardNo", debitCardNumber);
                        cmd.Parameters.AddWithValue("@cvc", cvc);
                        cmd.Parameters.AddWithValue("@expDate", expiryDate);
                        cmd.Parameters.AddWithValue("@holderName", holderName);
                        cmd.ExecuteNonQuery();
                    }
                }

                // Format and display the card details on panel7
                label27.Text = FormatCardNumber(debitCardNumber);
                label26.Text = expiryDate.ToString("yyyy-MM-dd");
                label22.Text = cvc;
                label24.Text = holderName;
                panel7.Visible = true;
                panel6.Visible = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private string FormatCardNumber(string cardNumber)
        {
            // Add spaces after every 4 digits
            for (int i = 4; i < cardNumber.Length; i += 5)
            {
                cardNumber = cardNumber.Insert(i, " ");
            }
            return cardNumber;
        }
        private void submitCreditCardButton_Click_1(object sender, EventArgs e)
        {
            string accountNumber = txtCreditCardAccountNumber.Text;

            if (string.IsNullOrWhiteSpace(accountNumber))
            {
                MessageBox.Show("Please enter the account number.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string connectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\nalla\\Documents\\bankserver.mdf;Integrated Security=True;Connect Timeout=30";
            string checkQuery = "SELECT cardNo, expDate, cvc FROM creditdetails WHERE accountNumber = @acc";

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    using (SqlCommand checkCmd = new SqlCommand(checkQuery, conn))
                    {
                        checkCmd.Parameters.AddWithValue("@acc", accountNumber);
                        using (SqlDataReader reader = checkCmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                // Card exists, display it
                                label15.Text = reader["cardNo"].ToString();
                                label16.Text = reader["expDate"].ToString(); // Corrected column name
                                label21.Text = reader["cvc"].ToString();
                                label19.Text = GetUserName(accountNumber); // Retrieve customer name
                                panel9.Visible = true;
                                panel5.Visible = false;
                            }
                            else
                            {
                                // Card does not exist, check eligibility
                                if (CheckEligibilityForCreditCard(accountNumber))
                                {
                                    // Notify the manager about the request
                                    NotifyManagerAboutCreditCardRequest(accountNumber);
                                    MessageBox.Show("Your request for a credit card has been sent to the manager. Please wait for approval.", "Request Sent", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    panel5.Visible = false;
                                }
                                else
                                {
                                    MessageBox.Show("Not eligible for a credit card.", "Eligibility Check", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void label16_Click(object sender, EventArgs e)
        {

        }

        private void button10_Click(object sender, EventArgs e)
        {
            this.Hide();
            Form7 form7 = new Form7();
            form7.ShowDialog();
        }
        private string _connectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\nalla\\Documents\\bankserver.mdf;Integrated Security=True;Connect Timeout=30";

        public int CalculateCreditScore(string accountNumber)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                con.Open();

                // 1. Payment History (35%)
                string paymentQuery = "SELECT COUNT(*) AS TotalPayments, " +
                                      "SUM(CASE WHEN Status = 'OnTime' THEN 1 ELSE 0 END) AS OnTimePayments, " +
                                      "SUM(CASE WHEN Status = 'Late' THEN 1 ELSE 0 END) AS LatePayments, " +
                                      "SUM(CASE WHEN Status = 'Missed' THEN 1 ELSE 0 END) AS MissedPayments " +
                                      "FROM PaymentHistory WHERE accountNumber = @accountNumber";
                SqlCommand paymentCmd = new SqlCommand(paymentQuery, con);
                paymentCmd.Parameters.AddWithValue("@accountNumber", accountNumber);

                SqlDataReader paymentReader = paymentCmd.ExecuteReader();
                paymentReader.Read();

                int totalPayments = paymentReader["TotalPayments"] != DBNull.Value ? Convert.ToInt32(paymentReader["TotalPayments"]) : 0;
                int onTimePayments = paymentReader["OnTimePayments"] != DBNull.Value ? Convert.ToInt32(paymentReader["OnTimePayments"]) : 0;
                int latePayments = paymentReader["LatePayments"] != DBNull.Value ? Convert.ToInt32(paymentReader["LatePayments"]) : 0;
                int missedPayments = paymentReader["MissedPayments"] != DBNull.Value ? Convert.ToInt32(paymentReader["MissedPayments"]) : 0;

                paymentReader.Close();

                decimal paymentHistoryScore = totalPayments > 0
                    ? ((onTimePayments * 35m) / totalPayments) - (latePayments * 10m + missedPayments * 20m)
                    : 0;

                // 2. Amounts Owed (30%)
                string creditUtilizationQuery = "SELECT SUM(CASE WHEN TransactionType = 'Debit' THEN Amount ELSE 0 END) AS TotalDebits, " +
                                                "SUM(CASE WHEN TransactionType = 'Credit' THEN Amount ELSE 0 END) AS TotalCredits " +
                                                "FROM creditdebit WHERE accountNumber = @accountNumber";
                SqlCommand creditUtilizationCmd = new SqlCommand(creditUtilizationQuery, con);
                creditUtilizationCmd.Parameters.AddWithValue("@accountNumber", accountNumber);

                SqlDataReader utilizationReader = creditUtilizationCmd.ExecuteReader();
                utilizationReader.Read();

                decimal totalDebits = utilizationReader["TotalDebits"] != DBNull.Value ? Convert.ToDecimal(utilizationReader["TotalDebits"]) : 0;
                decimal totalCredits = utilizationReader["TotalCredits"] != DBNull.Value ? Convert.ToDecimal(utilizationReader["TotalCredits"]) : 0;
                utilizationReader.Close();

                decimal creditUtilization = totalCredits > 0 ? totalDebits / totalCredits : 0;
                decimal amountsOwedScore = (1 - creditUtilization) * 30m;

                // 3. Length of Credit History (15%)
                string lengthQuery = "SELECT MIN(timestamp) AS FirstTransactionDate FROM creditdebit WHERE accountNumber = @accountNumber";
                SqlCommand lengthCmd = new SqlCommand(lengthQuery, con);
                lengthCmd.Parameters.AddWithValue("@accountNumber", accountNumber);

                DateTime? firstTransactionDate = lengthCmd.ExecuteScalar() as DateTime?;
                int creditHistoryLength = firstTransactionDate.HasValue ? (DateTime.Now - firstTransactionDate.Value).Days / 365 : 0;
                decimal creditHistoryScore = Math.Min(15m, creditHistoryLength * 1.5m);

                // 4. New Credit (10%)
                string newCreditQuery = "SELECT COUNT(*) FROM creditdebit WHERE accountNumber = @accountNumber AND timestamp >= @LastYear";
                SqlCommand newCreditCmd = new SqlCommand(newCreditQuery, con);
                newCreditCmd.Parameters.AddWithValue("@accountNumber", accountNumber);
                newCreditCmd.Parameters.AddWithValue("@LastYear", DateTime.Now.AddYears(-1));

                int newCreditCount = Convert.ToInt32(newCreditCmd.ExecuteScalar() ?? 0);
                decimal newCreditScore = 10m - Math.Min(10m, newCreditCount * 2m);

                // 5. Types of Credit in Use (10%)
                string loanCountQuery = "SELECT COUNT(*) FROM LoanApplications WHERE accountNumber = @accountNumber AND Status = 'Approved'";
                SqlCommand loanCountCmd = new SqlCommand(loanCountQuery, con);
                loanCountCmd.Parameters.AddWithValue("@accountNumber", accountNumber);

                int loanCount = Convert.ToInt32(loanCountCmd.ExecuteScalar() ?? 0);
                decimal typesOfCreditScore = Math.Min(10m, loanCount * 2m);

                // Combine all the scores
                int totalScore = (int)Math.Round(paymentHistoryScore + amountsOwedScore + creditHistoryScore + newCreditScore + typesOfCreditScore);
                totalScore = Math.Max(300, Math.Min(totalScore, 850)); // Normalize to standard credit score range

                // Update Credit Score in Database
                string insertScoreQuery = "IF EXISTS (SELECT 1 FROM CreditScoreHistory WHERE accountNumber = @accountNumber) " +
                                          "UPDATE CreditScoreHistory SET CreditScore = @CreditScore WHERE accountNumber = @accountNumber " +
                                          "ELSE INSERT INTO CreditScoreHistory (accountNumber, CreditScore) VALUES (@accountNumber, @CreditScore)";
                SqlCommand insertScoreCmd = new SqlCommand(insertScoreQuery, con);
                insertScoreCmd.Parameters.AddWithValue("@accountNumber", accountNumber);
                insertScoreCmd.Parameters.AddWithValue("@CreditScore", totalScore);
                insertScoreCmd.ExecuteNonQuery();

                return totalScore;
            }
        }
    }
}
