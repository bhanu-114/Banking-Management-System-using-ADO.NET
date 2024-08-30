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
using System.Windows.Controls;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace WindowsFormsApp1
{
    public partial class Form3 : Form
    {
        private string username;
        private string profilePicPath;

        public Form3()
        {
            InitializeComponent();
          
        }
        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            textBox1.Text = "";
        }

        private void button3_Click(object sender, EventArgs e)
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
            dataGridViewNotifications.Visible = false;
            button8.Visible = false;
            button6.Visible = true;
            button7.Visible = true;
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
    

        private void button4_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp",
                Title = "Select a Profile Picture"
            };

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                profilePicPath = openFileDialog.FileName;
                pictureBox1.Image = System.Drawing.Image.FromFile(profilePicPath);

                // Save the updated profile picture path to the database
                SaveProfilePicture(profilePicPath);
            }
       }
        private void SaveProfilePicture(string imagePath)
        {
            // Save the image path to local storage
            string destinationPath = Path.Combine(Application.StartupPath, "ProfilePictures", Path.GetFileName(imagePath));
            Directory.CreateDirectory(Path.GetDirectoryName(destinationPath));
            File.Copy(imagePath, destinationPath, true);

            // Save the image path to the database
            SaveImagePathToDatabase(destinationPath);
        }

        private void SaveImagePathToDatabase(string imagePath)
        {
            string connectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\nalla\\Documents\\bankserver.mdf;Integrated Security=True;Connect Timeout=30"; // Replace with your actual connection string
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "UPDATE managerr SET ProfilePicPath = @ProfilePicPath WHERE Username = @Username";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ProfilePicPath", imagePath);
                    command.Parameters.AddWithValue("@Username", username);
                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
        }
        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void Form3_Load(object sender, EventArgs e)
        {
            panel2.Visible = false;
            panel1.Visible = true;
            panel3.Visible = false;
            panel4.Visible = false;
            button3.Visible = false;
            btnViewDocuments.Visible = false;
            button6.Visible = false;
            button7.Visible = false;
            button9.Visible = false;
            button10.Visible = false;
            button11.Visible = false;
            button14.Visible = false;
            webBrowser1.Visible = false;
            pictureBoxDocument.Visible = false;
            dataGridView1.Visible = false;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            this.Hide();
            Form1 form1 = new Form1();
            form1.ShowDialog();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            // This is the Approval button for a credit card on the manager login
            string accountNumber = textBox1.Text; // Assuming you get the account number from a textbox or other input

            if (string.IsNullOrWhiteSpace(accountNumber))
            {
                MessageBox.Show("Please enter the account number.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (GenerateCreditCard(accountNumber))
            {
                NotifyUserAboutApproval(accountNumber);
                MessageBox.Show("Credit card request approved generate your card by submitting details", "Approval", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Error occurred while generating the credit card.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool GenerateCreditCard(string accountNumber)
        {
            // Generate credit card details
            Random rnd = new Random();
            string creditCardNumber = rnd.Next(100000000, 999999999).ToString("D16");
            string cvc = rnd.Next(100, 999).ToString("D3");
            DateTime expiryDate = DateTime.Now.AddYears(5);
            string holderName = GetUserName(accountNumber); // Method to retrieve the customer's name

            if (string.IsNullOrEmpty(holderName))
            {
                MessageBox.Show("Account number not found or customer name is missing.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            string connectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\nalla\\Documents\\bankserver.mdf;Integrated Security=True;Connect Timeout=30";
            string query = "INSERT INTO creditdetails (accountNumber, cardNo, cvc, expDate, holderName) VALUES (@acc, @cardNo, @cvc, @expDate, @holderName)";

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@acc", accountNumber);
                        cmd.Parameters.AddWithValue("@cardNo", creditCardNumber);
                        cmd.Parameters.AddWithValue("@cvc", cvc);
                        cmd.Parameters.AddWithValue("@expDate", expiryDate.ToString("yyyy-MM-dd"));
                        cmd.Parameters.AddWithValue("@holderName", holderName);
                        cmd.ExecuteNonQuery();
                    }
                }

                // Display the card number and CVC in a PictureBox or Label (Optional)
                // DisplayCardImage(creditCardNumber, pictureBox1); // Assuming pictureBox1 is where you display the card
                // DisplayCVC(cvc, labelCVC); // Assuming labelCVC is where you display the CVC (optional)

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }


        private void NotifyUserAboutApproval(string accountNumber)
        {
            string connectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\nalla\\Documents\\bankserver.mdf;Integrated Security=True;Connect Timeout=30";
            string query = "INSERT INTO Notifications (accountNumber, message) VALUES (@acc, @message)";

            string message = $"Your request for a credit card has been approved. Click the link to view your card.";

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

        private void buttonNotifications_Click(object sender, EventArgs e)
        {
            string accountNumber = textBox1.Text;

            if (string.IsNullOrWhiteSpace(accountNumber))
            {
                MessageBox.Show("Please enter the account number.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            LoadNotifications(accountNumber);
            dataGridViewNotifications.Visible = true;
            dataGridView1.Visible = false;
            button8.Visible = true;
        }
        private void LoadNotifications(string accountNumber)
        {
            string connectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\nalla\\Documents\\bankserver.mdf;Integrated Security=True;Connect Timeout=30";
            string query = "SELECT message, timestamp FROM Banknotifications WHERE accountNumber = @acc ORDER BY timestamp DESC";

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

        private void button8_Click(object sender, EventArgs e)
        {
            dataGridViewNotifications.Visible=false;
            button8.Visible=false;
            btnViewDocuments.Visible=false;
            dataGridView1.Visible=false;
        }

        private void button7_Click(object sender, EventArgs e)
        {
            if (!decimal.TryParse(textBox1.Text, out decimal accountNumber))
            {
                MessageBox.Show("Please enter a valid account number.", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            string connectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\nalla\\Documents\\bankserver.mdf;Integrated Security=True;Connect Timeout=30";
            // SQL query to select the loan applications based on the account number
            string query = "SELECT LoanID, LoanType, ApplicantName, Amount, InterestRate, Tenure, Collateral, Status, ApprovedDate " +
                           "FROM LoanApplications WHERE AccountNumber = @AccountNumber";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@AccountNumber", accountNumber);

                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataTable dataTable = new DataTable();

                try
                {
                    connection.Open();
                    adapter.Fill(dataTable);

                    if (dataTable.Rows.Count > 0)
                    {
                        // Bind the DataTable to the DataGridView
                        dataGridView1.DataSource = dataTable;
                        dataGridView1.Visible = true;
                        dataGridViewNotifications.Visible = false; // Show notifications DataGridView
                        button8.Visible = true;
                        button14.Visible=true;
                        btnViewDocuments.Visible = true;
                    }
                    else
                    {
                        MessageBox.Show("No loan applications found for the entered account number.", "No Data", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        dataGridView1.DataSource = null;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred while retrieving loan applications: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnViewDocuments_Click(object sender, EventArgs e)
        {
            string connectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\nalla\\Documents\\bankserver.mdf;Integrated Security=True;Connect Timeout=30";

            if (selectedLoanID <= 0)
            {
                MessageBox.Show("No loan selected.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string query = "SELECT DocumentImage, AccountNumber, Status FROM LoanApplications WHERE LoanID = @LoanID";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@LoanID", selectedLoanID);

                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    byte[] documentImage = reader["DocumentImage"] as byte[];
                    decimal accountNumber = (decimal)reader["AccountNumber"];
                    string loanStatus = reader["Status"].ToString();
                    reader.Close();

                    if (loanStatus == "Approved")
                    {
                        button9.Visible = false;  // Approve button
                        button10.Visible = false;// Reject button
                        button11.Visible = true;   
                    }
                    else if (loanStatus == "Rejected")
                    {
                        MessageBox.Show("This loan application is rejected.", "Action Canceled", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    if (documentImage != null && documentImage.Length > 0)
                    {
                        try
                        {
                            if (IsPdf(documentImage) || IsWordDocument(documentImage) || IsExcelDocument(documentImage) || IsPowerPointDocument(documentImage))
                            {
                                string tempFilePath = Path.Combine(Path.GetTempPath(), "tempDocument");
                                if (IsPdf(documentImage))
                                {
                                    tempFilePath += ".pdf";
                                }
                                else if (IsWordDocument(documentImage))
                                {
                                    tempFilePath += ".docx";
                                }
                                else if (IsExcelDocument(documentImage))
                                {
                                    tempFilePath += ".xlsx";
                                }
                                else if (IsPowerPointDocument(documentImage))
                                {
                                    tempFilePath += ".pptx";
                                }

                                File.WriteAllBytes(tempFilePath, documentImage);
                                webBrowser1.Navigate(tempFilePath);

                                webBrowser1.Visible = true;
                                pictureBoxDocument.Visible = false;

                                if (loanStatus == "Pending")
                                {
                                    button10.Visible = true;
                                    button11.Visible = true;
                                    button9.Visible = true;
                                }
                            }
                            else
                            {
                                string errorMessage = "Unsupported file type. Please upload a document in one of the supported formats: PDF, Word, Excel, or PowerPoint.";
                                MessageBox.Show(errorMessage, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                                LogNotification(connection, accountNumber, errorMessage);
                                SetLoanStatusAsRejected(connection, selectedLoanID);
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("An error occurred while displaying the document.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            Console.WriteLine($"Error: {ex.Message}");
                        }
                    }
                    else
                    {
                        string errorMessage = "No document found. Please provide the documents related to your loan.";
                        MessageBox.Show(errorMessage, "No Document", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        LogNotification(connection, accountNumber, errorMessage);
                        SetLoanStatusAsRejected(connection, selectedLoanID);
                    }
                }
                else
                {
                    MessageBox.Show("Loan not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }


        // Helper method to check if the document is a PDF
        private bool IsPdf(byte[] documentData)
        {
            return documentData != null && documentData.Length > 4 &&
                   documentData[0] == 0x25 &&
                   documentData[1] == 0x50 &&
                   documentData[2] == 0x44 &&
                   documentData[3] == 0x46; // "%PDF"
        }

        // Helper method to check if the document is a Word document (DOCX)
        private bool IsWordDocument(byte[] documentData)
        {
            return documentData != null && documentData.Length > 4 &&
                   documentData[0] == 0x50 &&
                   documentData[1] == 0x4B &&
                   documentData[2] == 0x03 &&
                   documentData[3] == 0x04; // ZIP file header (DOCX is a zipped file)
        }

        // Helper method to check if the document is an Excel document (XLSX)
        private bool IsExcelDocument(byte[] documentData)
        {
            return IsWordDocument(documentData); // Same ZIP file header as DOCX
        }

        // Helper method to check if the document is a PowerPoint document (PPTX)
        private bool IsPowerPointDocument(byte[] documentData)
        {
            return IsWordDocument(documentData); // Same ZIP file header as DOCX
        }

        // Helper method to check if the document is an image
        private bool IsImage(byte[] documentData)
        {
            try
            {
                using (MemoryStream ms = new MemoryStream(documentData))
                {
                    System.Drawing.Image img = System.Drawing.Image.FromStream(ms);
                    return true;
                }
            }
            catch (ArgumentException)
            {
                return false;
            }
        }

        // Helper method to display an image in the PictureBox
        private void DisplayImage(byte[] documentData)
        {
            using (MemoryStream ms = new MemoryStream(documentData))
            {
                pictureBoxDocument.Image = System.Drawing.Image.FromStream(ms);
                webBrowser1.Visible = false;
                button10.Visible = true;
                button11.Visible = true;
                pictureBoxDocument.Visible = true;
            }
        }

        private int selectedLoanID;
        private void button11_Click(object sender, EventArgs e)
        {
            webBrowser1.Visible = false;
            button9.Visible=false;
            button10.Visible=false;
            button11.Visible=false;
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0) // Ensure that the clicked row index is valid
            {
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];

                // Assuming "LoanID" is the name of the column containing the LoanID
                selectedLoanID = Convert.ToInt32(row.Cells["LoanID"].Value);
            }
        }
        private void LogNotification(SqlConnection connection, decimal accountNumber, string message)
        {
            string insertNotificationQuery = "INSERT INTO Notifications (accountNumber, message) VALUES (@accountNumber, @message)";

            using (SqlCommand cmd = new SqlCommand(insertNotificationQuery, connection))
            {
                cmd.Parameters.AddWithValue("@accountNumber", accountNumber);
                cmd.Parameters.AddWithValue("@message", message);
                cmd.ExecuteNonQuery();
            }
        }

        private void SetLoanStatusAsRejected(SqlConnection connection, int loanID)
        {
            string updateLoanStatusQuery = "UPDATE LoanApplications SET LoanStatus = 'Rejected' WHERE LoanID = @LoanID";

            using (SqlCommand cmd = new SqlCommand(updateLoanStatusQuery, connection))
            {
                cmd.Parameters.AddWithValue("@LoanID", loanID);
                cmd.ExecuteNonQuery();
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Are you sure you want to approve this loan?", "Loan Approval", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);

            if (dialogResult == DialogResult.OK)
            {
                string connectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\nalla\\Documents\\bankserver.mdf;Integrated Security=True;Connect Timeout=30";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // Get the loan details
                    string loanQuery = "SELECT Amount, AccountNumber FROM LoanApplications WHERE LoanID = @LoanID";
                    using (SqlCommand loanCommand = new SqlCommand(loanQuery, connection))
                    {
                        loanCommand.Parameters.AddWithValue("@LoanID", selectedLoanID);
                        SqlDataReader loanReader = loanCommand.ExecuteReader();

                        if (loanReader.Read())
                        {
                            decimal loanAmount = (decimal)loanReader["Amount"];
                            decimal accountNumber = (decimal)loanReader["AccountNumber"];
                            loanReader.Close();

                            // Approve the loan, update the LoanApplications table with status and approval date
                            string updateLoanStatusQuery = "UPDATE LoanApplications SET Status = 'Approved', ApprovalDate = @ApprovalDate WHERE LoanID = @LoanID";
                            using (SqlCommand updateLoanStatusCommand = new SqlCommand(updateLoanStatusQuery, connection))
                            {
                                updateLoanStatusCommand.Parameters.AddWithValue("@ApprovalDate", DateTime.Now);
                                updateLoanStatusCommand.Parameters.AddWithValue("@LoanID", selectedLoanID);
                                updateLoanStatusCommand.ExecuteNonQuery();
                            }

                            // Add the loan amount to the user's account in the coust table
                            string updateAccountAmountQuery = "UPDATE coust SET amount = amount + @LoanAmount WHERE acc = @AccountNumber";
                            using (SqlCommand updateAccountAmountCommand = new SqlCommand(updateAccountAmountQuery, connection))
                            {
                                updateAccountAmountCommand.Parameters.AddWithValue("@LoanAmount", loanAmount);
                                updateAccountAmountCommand.Parameters.AddWithValue("@AccountNumber", accountNumber);
                                updateAccountAmountCommand.ExecuteNonQuery();
                            }

                            // Calculate the new balance (Assuming balance is updated correctly in coust table)
                            string newBalanceQuery = "SELECT amount FROM coust WHERE acc = @AccountNumber";
                            decimal newBalance;
                            using (SqlCommand newBalanceCommand = new SqlCommand(newBalanceQuery, connection))
                            {
                                newBalanceCommand.Parameters.AddWithValue("@AccountNumber", accountNumber);
                                newBalance = (decimal)newBalanceCommand.ExecuteScalar();
                            }

                            // Retrieve branch name using the GetBranchName method
                            string branchName = GetBranchName(accountNumber.ToString());

                            // Prepare the notification messages
                            string loanApprovalMessage = $"Your loan for Rs.{loanAmount} has been approved and credited to your account.";
                            string notificationMessage = $"Rs.{loanAmount} Debited from INB-{accountNumber.ToString().Substring(accountNumber.ToString().Length - 4)} AcBal:{newBalance} CLRBal:{newBalance} ({branchName}) on {DateTime.Now:dd-MM-yyyy HH:mm:ss}.INB.";

                            // Insert the approval notification into the Notifications table
                            string insertNotificationQuery = "INSERT INTO Notifications (accountNumber, message) VALUES (@AccountNumber, @Message)";
                            using (SqlCommand insertNotificationCommand = new SqlCommand(insertNotificationQuery, connection))
                            {
                                insertNotificationCommand.Parameters.AddWithValue("@AccountNumber", accountNumber);
                                insertNotificationCommand.Parameters.AddWithValue("@Message", loanApprovalMessage);
                                insertNotificationCommand.ExecuteNonQuery();
                            }

                            // Insert the debit notification into the Notifications table
                            using (SqlCommand insertNotificationCommand = new SqlCommand(insertNotificationQuery, connection))
                            {
                                insertNotificationCommand.Parameters.AddWithValue("@AccountNumber", accountNumber);
                                insertNotificationCommand.Parameters.AddWithValue("@Message", notificationMessage);
                                insertNotificationCommand.ExecuteNonQuery();
                            }

                            MessageBox.Show("Loan approved, amount credited to the user's account, and notifications sent.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            MessageBox.Show("Loan not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            else if (dialogResult == DialogResult.Cancel)
            {
                // Close the message box if Cancel is clicked
                MessageBox.Show("Loan approval cancelled.", "Cancelled", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
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

        private void button10_Click(object sender, EventArgs e)
        {
            panel3.Visible = true;
        }
        private int GetAccountNumberByLoanID(int loanID)
        {
            string connectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\nalla\\Documents\\bankserver.mdf;Integrated Security=True;Connect Timeout=30";
            string query = "SELECT accountNumber FROM LoanApplications WHERE LoanID = @LoanID";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@LoanID", loanID);

                connection.Open();
                object result = command.ExecuteScalar();

                if (result != null)
                {
                    return Convert.ToInt32(result);
                }
                else
                {
                    throw new Exception("Account number not found for the selected loan.");
                }
            }
        }
        private void button12_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are you sure you want to reject the loan?", "Confirm Rejection", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);

            if (result == DialogResult.OK)
            {
                int accountNumber = GetAccountNumberByLoanID(selectedLoanID);
                // Update the loan status to 'Rejected'
                string connectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\nalla\\Documents\\bankserver.mdf;Integrated Security=True;Connect Timeout=30";
                string updateQuery = "UPDATE LoanApplications SET Status = 'Rejected' WHERE LoanID = @LoanID";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlCommand command = new SqlCommand(updateQuery, connection);
                    command.Parameters.AddWithValue("@LoanID", selectedLoanID);

                    connection.Open();
                    command.ExecuteNonQuery();
                }

                // Insert the rejection message into the Notifications table
                string rejectionMessage = textBoxRejectionReason.Text; // Assuming this is the text box where the manager writes the rejection reason
                string insertQuery = "INSERT INTO Notifications (accountNumber, message) VALUES (@AccountNumber, @Message)";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlCommand command = new SqlCommand(insertQuery, connection);
                    command.Parameters.AddWithValue("@AccountNumber", accountNumber); // Make sure you have the correct account number
                    command.Parameters.AddWithValue("@Message", rejectionMessage);

                    connection.Open();
                    command.ExecuteNonQuery();
                }

                MessageBox.Show("Loan has been rejected and the notification has been sent.", "Loan Rejected", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else if (result == DialogResult.Cancel)
            {
                // Close the message box without further action
            }
        }

        private void button13_Click(object sender, EventArgs e)
        {
            panel3.Visible = false;
        }

        private void button14_Click(object sender, EventArgs e)
        {

            string connectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\nalla\\Documents\\bankserver.mdf;Integrated Security=True;Connect Timeout=30";

            // First, get the account number associated with the selected loan ID
            string accountNumberQuery = "SELECT AccountNumber FROM LoanApplications WHERE LoanID = @LoanID";
            string creditScoreQuery = "SELECT TOP 1 CreditScore FROM CreditScoreHistory WHERE accountNumber = @accountNumber ORDER BY CalculatedDate DESC";

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // Get the account number
                    string accountNumber = "";
                    using (SqlCommand accountNumberCommand = new SqlCommand(accountNumberQuery, connection))
                    {
                        accountNumberCommand.Parameters.AddWithValue("@LoanID", selectedLoanID);
                        object result = accountNumberCommand.ExecuteScalar();
                        if (result != null && result != DBNull.Value)
                        {
                            accountNumber = result.ToString();
                        }
                        else
                        {
                            MessageBox.Show("Loan ID not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }

                    // Get the latest credit score
                    using (SqlCommand creditScoreCommand = new SqlCommand(creditScoreQuery, connection))
                    {
                        creditScoreCommand.Parameters.AddWithValue("@accountNumber", accountNumber);
                        object result = creditScoreCommand.ExecuteScalar();
                        if (result != null && result != DBNull.Value)
                        {
                            panel4.Visible = true;
                            int creditScore = Convert.ToInt32(result);
                            label30.Text = creditScore.ToString();
                        }
                        else
                        {
                            label30.Text = "Credit Score: Not Available";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button15_Click(object sender, EventArgs e)
        {
            panel4.Visible = false;
        }
    }
}
