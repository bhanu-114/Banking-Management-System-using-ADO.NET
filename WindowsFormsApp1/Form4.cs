using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form4 : Form
    {
        public Form4()
        {
            InitializeComponent();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
           
        }

        private void button1_Click(object sender, EventArgs e)
        {
        
        }

        private void button3_Click(object sender, EventArgs e)
        {

        }

        private void Form4_Load(object sender, EventArgs e)
        {
            label1.Visible = false;
            button3.Visible = false;
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            label1.Text = "About Us\r\n\r\nWelcome to NATIONAL BANK\r\n\r\nAt NATIONAL BANK , we are committed to revolutionizing the banking experience by harnessing the power of technology to provide seamless, secure, and efficient financial services.\nOur Banking Management System is designed with both customers and administrators in mind, ensuring ease of use, reliability, and top-notch security.\r\n\r\nWho We Are\r\nWe are a team of passionate developers and financial experts dedicated to crafting innovative banking solutions that meet the needs of modern users.\nOur mission is to bridge the gap between traditional banking and the digital future by offering tools that empower both individuals and businesses to manage their finances effortlessly.\r\n\r\nWhat We Offer\r\nOur Banking Management System provides a comprehensive suite of features, including:\r\n\r\nSecure Account Management: Easily create, manage, and access bank accounts with robust security measures.\r\nCredit and Debit Operations: Handle transactions with precision, ensuring accuracy and transparency in every operation.\r\nCard Management: Generate and manage credit and debit cards, complete with real-time balance checks and transaction histories.\r\nUser-Friendly Interface: Navigate our system with ease, thanks to an intuitive interface designed for users of all technical levels.\r\nDetailed Reporting: Access detailed financial reports to stay on top of your financial health.\r\nOur Commitment\r\nAt NATIONAL BANK, we believe that banking should be simple, accessible, and above all, secure. We are committed to continuously improving our system to meet the evolving needs of our users,\nensuring that your financial management experience is second to none.\r\n\r\nThank you for choosing NATIONAL BANK. We look forward to serving you and helping you achieve your financial goals.\r\n\r\nYou can personalize this content with your project's name and any specific features or values your Banking Management System project emphasizes.";
            label1.Visible = true;
            button3.Visible = true;
        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            label1.Visible=false;
            button3.Visible=false;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Hide();
            Form1 form = new Form1();
            form.ShowDialog();
        }
    }
}
