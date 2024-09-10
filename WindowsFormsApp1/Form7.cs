using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static WindowsFormsApp1.Form5;

namespace WindowsFormsApp1
{
    public partial class Form7 : Form
    {
        private string uploadedFilePath;
        private string connectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\nalla\\Documents\\bankserver.mdf;Integrated Security=True;Connect Timeout=30";
        public Form7()
        {
            InitializeComponent();

        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void Form7_Load(object sender, EventArgs e)
        {
            panel3.Visible = false;
            panel4.Visible = false;
            label8.Visible = false;
           
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbLoanType.SelectedIndex == 0)
            {
                label8.Text = "Proof of Identity: Passport, PAN Card, Aadhaar Card\r\nProof of Address: Utility Bill, Passport, Aadhaar Card, Voter ID\r\nIncome Proof: Salary slips (last 3-6 months), Form 16, Income Tax Returns (ITR) for the last 2 years\r\nBank Statements: Last 6 months' bank statement\r\nProperty Documents:\r\nSale agreement\r\nAllotment letter from the builder\r\nCopy of approved plan\r\nNOC from the builder/society\r\nEmployment Proof: Employment certificate/letter from the employer\r\nProcessing Fee Cheque: Required by most banks";
            }
            if (cmbLoanType.SelectedIndex == 1)
            {
                label8.Text = "Proof of Identity: Passport, Driver’s License, Voter ID, Aadhaar Card, PAN Card\r\nProof of Address: Utility Bill, Rental Agreement, Passport, Aadhaar Card\r\nIncome Proof: Salary slips (last 3-6 months), Income Tax Returns (ITR) for the last 2 years, Form 16\r\nBank Statements: Last 6 months' bank statement\r\nEmployment Proof: Employee ID card, Employment certificate/letter from the employer";
            }
            if (cmbLoanType.SelectedIndex == 2)
            {
                label8.Text = "Proof of Identity: PAN Card, Passport, Aadhaar Card\r\nProof of Address: Utility Bill, Rental Agreement, Passport, Aadhaar Card\r\nIncome Proof: Salary slips (last 3 months), Form 16, ITR for the last 2 years\r\nBank Statements: Last 6 months' bank statement\r\nVehicle Documents:\r\nProforma invoice of the vehicle to be purchased\r\nEmployment Proof: Employee ID card, Employment certificate/letter from the employer";
            }
            if (cmbLoanType.SelectedIndex == 3)
            {
                label8.Text = "Proof of Identity: Passport, PAN Card, Aadhaar Card\r\nProof of Address: Utility Bill, Rental Agreement, Passport, Aadhaar Card\r\nProof of Admission: Admission letter from the educational institution\r\nFee Structure: Schedule of fees from the institution\r\nIncome Proof (of parents/guardian): Salary slips (last 3-6 months), ITR for the last 2 years\r\nBank Statements: Last 6 months' bank statement of the co-applicant\r\nAcademic Records: Previous academic records, certificates, mark sheets";
            }
            if (cmbLoanType.SelectedIndex == 4)
            {
                label8.Text = "Proof of Identity: Passport, PAN Card, Aadhaar Card\r\nProof of Address: Utility Bill, Rental Agreement, Passport, Aadhaar Card\r\nOwnership Proof of Gold: No specific documents, but the gold items need to be verified for purity and value by the bank or financial institution";
            }
            if ((cmbLoanType.SelectedIndex == 5))
            {
                label8.Text = "Proof of Identity: PAN Card, Passport, Aadhaar Card of the business owner(s)\r\nProof of Address: Utility Bill, Rental Agreement, Passport, Aadhaar Card of the business\r\nBusiness Proof: Registration certificate, GST registration, Partnership deed, Memorandum of Association (MOA), Articles of Association (AOA) for companies\r\nFinancial Statements: Audited balance sheets, Profit & Loss statements for the last 2-3 years\r\nIncome Tax Returns (ITR): ITR of the business and the business owner(s) for the last 2-3 years\r\nBank Statements: Last 6 months' bank statement of the business\r\nCollateral Documents: If the loan is secured, documents related to the asset being pledged";
            }
            if ((cmbLoanType.SelectedIndex == 6))
            {
                label8.Text = "Proof of Identity:\r\nPassport\r\nPAN Card\r\nAadhaar Card\r\nProof of Address:\r\nUtility Bill\r\nRental Agreement\r\nPassport\r\nAadhaar Card\r\nProof of Land Ownership:\r\nLand Records (e.g., Sale Deed, Title Deed)\r\nRecord of Rights (ROR)\r\nMutation Certificate\r\nCrop/Project-Related Documents:\r\nCrop Plan or Project Report\r\nDetails of the Crop being cultivated (for crop loans)\r\nLand Utilization Certificate (if required)\r\nIncome Proof:\r\nBank Statements (last 6 months)\r\nAgricultural Income Proof (if applicable)\r\nOther Documents:\r\nPassport-sized Photographs\r\nLoan Application Form";
            }
        }

        private void btnApplyLoan_Click(object sender, EventArgs e)
        {
            panel3.Visible = true;
            label8.Visible = true;
            panel4.Visible = false;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            panel3.Visible = false;
            label8.Visible = false;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            txtApplicantName.Text = " ";
            txtAccountNumber.Text = " ";
            txtLoanAmount.Text = " ";
            txtCollateral.Text = " ";
            txtTenure.Text = " ";
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            string applicantName = txtApplicantName.Text;
            decimal accountNumber = Convert.ToDecimal(txtAccountNumber.Text);
            decimal loanAmount = Convert.ToDecimal(txtLoanAmount.Text);
            int tenure = Convert.ToInt32(txtTenure.Text);
            string collateral = txtCollateral.Text;
            string loanType = cmbLoanType.SelectedItem.ToString();

            // Get the interest rate based on loan type
            decimal interestRate = GetInterestRateByLoanType(loanType);

            byte[] documentData = null;

            // If the user has uploaded a document, read the file data
            if (!string.IsNullOrEmpty(uploadedFilePath))
            {
                documentData = File.ReadAllBytes(uploadedFilePath);
            }

            // Insert data into LoanApplications table
            string query = "INSERT INTO LoanApplications (ApplicantName, AccountNumber, Amount, InterestRate, Tenure, Collateral, LoanType, DocumentImage) " +
                           "VALUES (@ApplicantName, @AccountNumber, @Amount, @InterestRate, @Tenure, @Collateral, @LoanType, @DocumentImage)";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@ApplicantName", applicantName);
                command.Parameters.AddWithValue("@AccountNumber", accountNumber);
                command.Parameters.AddWithValue("@Amount", loanAmount);
                command.Parameters.AddWithValue("@InterestRate", interestRate);
                command.Parameters.AddWithValue("@Tenure", tenure);
                command.Parameters.AddWithValue("@Collateral", collateral);
                command.Parameters.AddWithValue("@LoanType", loanType);
                command.Parameters.AddWithValue("@DocumentImage", (object)documentData ?? DBNull.Value); // Check for null

                connection.Open();
                command.ExecuteNonQuery();
            }

            // Notify the manager by inserting a notification into the Banknotifications table
            string notificationMessage = $"Loan application for {loanType} has been submitted by {applicantName}.";
            string notificationQuery = "INSERT INTO Banknotifications (accountNumber, message) VALUES (@AccountNumber, @Message)";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(notificationQuery, connection);
                command.Parameters.AddWithValue("@AccountNumber", accountNumber);
                command.Parameters.AddWithValue("@Message", notificationMessage);

                connection.Open();
                command.ExecuteNonQuery();
            }

            MessageBox.Show("Loan application submitted successfully and the manager has been notified.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

            // Reset the panel and fields
            panel3.Enabled = false;
            panel3.Visible = false;
            label8.Visible = false;
        }

        private void UploadDocumentButton_Click_1(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "All Files (*.*)|*.*"; // Allow all file types
                openFileDialog.Title = "Select Document to Upload";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    // Get the file path and store it for later use
                    uploadedFilePath = openFileDialog.FileName;
                }
            }
        }
        private decimal GetInterestRateByLoanType(string loanType)
        {
            switch (loanType)
            {
                case "Home Loan":
                    return 6.5m;
                case "Personal Loan":
                    return 10.5m;
                case "Car Loan":
                    return 8.0m;
                case "Education Loan":
                    return 5.0m;
                case "Agricultural Loan":
                    return 4.5m;
                case "Business Loan":
                    return 9.0m;
                case "Gold Loan":
                    return 7.0m;
                default:
                    return 7.0m; // Default interest rate if loan type is unknown
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
  
        }

        private void button7_Click(object sender, EventArgs e)
        {
            this.Hide();
            Form2 form = new Form2();
            form.ShowDialog();
        }

        private void buttonNotifications_Click(object sender, EventArgs e)
        {
           
        }

        private void button3_Click(object sender, EventArgs e)
        {
            panel4.Visible = true;
            panel3.Visible = false;
            label8.Visible = false;
        }

        private void button8_Click_1(object sender, EventArgs e)
        {
            string accountNumber = textboxAccountNumber.Text;
            string loanType = combobox1.SelectedItem?.ToString(); // Get the selected loan type
            decimal enteredAmount;

            if (loanType == null)
            {
                MessageBox.Show("Please select a loan type.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!decimal.TryParse(textboxAmount.Text, out enteredAmount) || enteredAmount <= 0)
            {
                MessageBox.Show("Please enter a valid amount.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string connectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\nalla\\Documents\\bankserver.mdf;Integrated Security=True;Connect Timeout=30";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                try
                {
                    // Check loan details based on selected loan type and account number
                    string loanQuery = "SELECT Amount, Status, InterestRate, Paid FROM LoanApplications WHERE AccountNumber = @AccountNumber AND LoanType = @LoanType";
                    using (SqlCommand loanCommand = new SqlCommand(loanQuery, connection))
                    {
                        loanCommand.Parameters.AddWithValue("@AccountNumber", accountNumber);
                        loanCommand.Parameters.AddWithValue("@LoanType", loanType);

                        using (SqlDataReader reader = loanCommand.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                decimal loanAmount = (decimal)reader["Amount"];
                                string loanStatus = reader["Status"].ToString();
                                decimal interestRate = (decimal)reader["InterestRate"];
                                decimal paidAmount = reader["Paid"] != DBNull.Value ? (decimal)reader["Paid"] : 0;
                                reader.Close();

                                if (loanStatus == "Cleared")
                                {
                                    MessageBox.Show("Loan is cleared, no need to pay.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    return;
                                }

                                decimal yearlyPayment = loanAmount * interestRate / 100;
                                textboxAmount.Text = yearlyPayment.ToString("F2");

                                if (enteredAmount < yearlyPayment)
                                {
                                    MessageBox.Show($"You must pay at least {yearlyPayment:C} per year.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    return;
                                }

                                string balanceQuery = "SELECT CASE WHEN amount >= @EnteredAmount THEN 1 ELSE 0 END FROM coust WHERE acc = @AccountNumber";

                                using (SqlCommand balanceCommand = new SqlCommand(balanceQuery, connection))
                                {
                                    balanceCommand.Parameters.AddWithValue("@AccountNumber", accountNumber);
                                    balanceCommand.Parameters.AddWithValue("@EnteredAmount", enteredAmount);

                                    // Execute the query and check the result
                                    int sufficientFunds = (int)balanceCommand.ExecuteScalar();

                                    if (sufficientFunds == 0)
                                    {
                                        MessageBox.Show("Insufficient funds in the account.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                        return;
                                    }

                                    // Proceed with the payment if funds are sufficient
                                    string updateAccountQuery = "UPDATE coust SET amount = amount - @EnteredAmount WHERE acc = @AccountNumber";
                                    using (SqlCommand updateAccountCommand = new SqlCommand(updateAccountQuery, connection))
                                    {
                                        updateAccountCommand.Parameters.AddWithValue("@EnteredAmount", enteredAmount);
                                        updateAccountCommand.Parameters.AddWithValue("@AccountNumber", accountNumber);
                                        int rowsAffected = updateAccountCommand.ExecuteNonQuery();

                                        if (rowsAffected == 0)
                                        {
                                            MessageBox.Show("Account update failed.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                            return;
                                        }
                                    }

                                    // Update the Paid amount in the loan
                                    string updateLoanQuery = "UPDATE LoanApplications SET Paid = ISNULL(Paid, 0) + @EnteredAmount WHERE AccountNumber = @AccountNumber AND LoanType = @LoanType";
                                    using (SqlCommand updateLoanCommand = new SqlCommand(updateLoanQuery, connection))
                                    {
                                        updateLoanCommand.Parameters.AddWithValue("@EnteredAmount", enteredAmount);
                                        updateLoanCommand.Parameters.AddWithValue("@AccountNumber", accountNumber);
                                        updateLoanCommand.Parameters.AddWithValue("@LoanType", loanType);

                                        int rowsAffected = updateLoanCommand.ExecuteNonQuery();
                                        if (rowsAffected == 0)
                                        {
                                            MessageBox.Show("No rows were updated. Please check the account number and loan type.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                        }
                                        else
                                        {
                                            MessageBox.Show("Payment successful. Paid amount updated.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                        }
                                    }

                                    // Check if the loan is fully paid
                                    if (paidAmount + enteredAmount >= loanAmount)
                                    {
                                        string updateLoanStatusQuery = "UPDATE LoanApplications SET Status = 'Cleared' WHERE AccountNumber = @AccountNumber AND LoanType = @LoanType";
                                        using (SqlCommand updateLoanStatusCommand = new SqlCommand(updateLoanStatusQuery, connection))
                                        {
                                            updateLoanStatusCommand.Parameters.AddWithValue("@AccountNumber", accountNumber);
                                            updateLoanStatusCommand.Parameters.AddWithValue("@LoanType", loanType);
                                            updateLoanStatusCommand.ExecuteNonQuery();
                                        }

                                        MessageBox.Show("Loan fully repaid and status updated to 'Cleared'.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                                        // Clear all the fields after loan payment is completed
                                        textboxAccountNumber.Text = " ";
                                        textboxAmount.Text = " ";
                                        combobox1.Text = " ";
                                    }
                                    else
                                    {
                                        MessageBox.Show("Payment successful. Remaining loan balance has been updated.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    }
                                }
                            }
                            else
                            {
                                MessageBox.Show("Account or loan type not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void ClearFields()
        {
            textboxAccountNumber.Clear();
            combobox1.SelectedIndex = -1;
            textboxAmount.Clear();
        }



        private void button9_Click(object sender, EventArgs e)
        {
            string accountNumber = textboxAccountNumber.Text;

            if (string.IsNullOrWhiteSpace(accountNumber))
            {
                MessageBox.Show("Please enter a valid account number.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string connectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\nalla\\Documents\\bankserver.mdf;Integrated Security=True;Connect Timeout=30";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                // Query to get available loans for the provided account number
                string loanQuery = "SELECT LoanType FROM LoanApplications WHERE AccountNumber = @AccountNumber AND Status = 'Approved'";
                using (SqlCommand loanCommand = new SqlCommand(loanQuery, connection))
                {
                    loanCommand.Parameters.AddWithValue("@AccountNumber", accountNumber);

                    using (SqlDataReader reader = loanCommand.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            combobox1.Items.Clear(); // Clear previous items
                            while (reader.Read())
                            {
                                combobox1.Items.Add(reader["LoanType"].ToString());
                            }
                        }
                        else
                        {
                            MessageBox.Show("No approved loans found for this account.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                }
            }
        }

        private void combobox1_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            string accountNumber = textboxAccountNumber.Text;
            string loanType = combobox1.SelectedItem?.ToString(); // Get the selected loan type

            if (loanType == null)
            {
                MessageBox.Show("Please select a loan type.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string connectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\nalla\\Documents\\bankserver.mdf;Integrated Security=True;Connect Timeout=30";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                // Check loan details based on selected loan type and account number
                string loanQuery = "SELECT Amount, InterestRate FROM LoanApplications WHERE AccountNumber = @AccountNumber AND LoanType = @LoanType";
                using (SqlCommand loanCommand = new SqlCommand(loanQuery, connection))
                {
                    loanCommand.Parameters.AddWithValue("@AccountNumber", accountNumber);
                    loanCommand.Parameters.AddWithValue("@LoanType", loanType);

                    using (SqlDataReader reader = loanCommand.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            decimal loanAmount = (decimal)reader["Amount"];
                            decimal interestRate = (decimal)reader["InterestRate"];
                            reader.Close();

                            // Calculate the yearly payment
                            decimal yearlyPayment = loanAmount * interestRate / 100;
                            textboxAmount.Text = yearlyPayment.ToString("F2");
                        }
                        else
                        {
                            MessageBox.Show("Account or loan type not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            textboxAccountNumber.Text = " ";
            textboxAmount.Text = " ";
            combobox1.Text = " ";
        }

        private void button5_Click(object sender, EventArgs e)
        {
            panel4.Visible = false;
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
