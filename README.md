Banking Management System

ADO.NET-Based Windows Forms Application

Table of Contents

Overview

Features

Technology Stack

Database Structure

Setup Instructions

Usage

Screenshots

Future Enhancements

Overview

The Banking Management System is an integrated desktop-based application developed using ADO.NET integrated with Windows Forms. It allows customer account management, loans, transactions, and notifications within the banking institution. This project emulates a real-life banking system for creating accounts, applying for loans, managing credit scores, and transferring funds.


Features

Customer Registration: Create a new customer with account validation, which also verifies a mobile number and a PIN code. Loan Management: Provide a facility to customers for applying for loans and to employees for approving loans, considering the interest rate according to the type of loan the customer has selected.

Credit Score Calculation: Automatically calculate the credit score of a customer based on his payment history and financial reputation, and display it. 

Transaction Management: Debit and credit amount in a customer's account. Notification: Notify the customer about loan approval or rejection and other bank-related activities via SMS or email.
Upload of Documents: The customer can upload and view his/her documents, as in PDF or image formats, regarding loan process.

Account Deletion: Customer accounts should be deleted if he/she has repaid the loan and has no outstanding transactions.

Technology Stack

Backend: ADO.NET-for database interactions

Frontend: Windows Forms (C#)

Database: SQL Server (LocalDB)

Database Structure

Tables:

coust: This table stores customer details related to an account number, personal information, and amount.

LoanApplications: It maintains loan-related details such as amount, rate of interest, status, type, and paid.

CreditScoreHistory: It logs credit scores for every account developed from financial activities.

Notifications: Stores notifications of loan approvals and rejections, among other activities.

BankNotifications: Stores bank messages and system-wide notifications.

Sample Table Structure:
sql

Copy code

    CREATE TABLE [dbo].[coust] (

    [name]    NVARCHAR (50) NOT NULL,
    
    [fname]   NVARCHAR (50) NULL,
    
    [mname]   NVARCHAR (50) NULL,
    
    [gender]  NVARCHAR (50) NULL,
    
    [mobile]  NVARCHAR (50) NULL,
    
    [street]  NVARCHAR (50) NULL,

    [village] NVARCHAR (50) NULL,
    
    [pin]     NVARCHAR (50) NULL,
    
    [state]   NVARCHAR (50) NULL,
    
    [branch]  NVARCHAR (50) NULL,
    
    [amount]  INT           NULL,
    
    [acc]     NUMERIC (18)  NOT NULL PRIMARY KEY,

    [ifsc]    NVARCHAR(11)  NULL
);

Setup Instructions

Clone the repository:

bash
Copy code
git clone https://github.com/yourusername/banking-management-system.git
Open the project:

Open the solution file (.sln) in Visual Studio.
Configure the database:

The project uses SQL Server LocalDB. Ensure that the connection string in the App.config is correct:
xml
Copy code
<connectionStrings>
  <add name="BankDBConnection"
connectionString="Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\path\to\your\database\bankserver.mdf;Integrated Security=True;Connect Timeout=30"
        providerName="System.Data.SqlClient" />
    </connectionStrings>

Run Application:

Build and run the application from within Visual Studio. The application should now load in your browser of choice.

Database Setup:

You may want to initialize database tables with sample data by using the SQL scripts located in the /scripts folder or create the tables manually in SQL Server Management Studio (SSMS). Usage
Customer Registration: Reach the customer registration panel and add new customers. Ensure that all the validations, including those of mobile numbers and PINs, pass upon submission. 
Loan Application: Depending on the availability of loan types and amount, customers may apply for loans. Loan Approval will be done by the bank manager himself/herself. 
Credit and Debit Operations: Transact funds into an account through crediting and debiting. The balance amount keeps updating in real time.
Notifications: System generated notifications regarding various account-related activities viewed in this area.
Screenshots
<!-- Add application screenshots -->
Add screenshots of the main form, registration form, loan application process, notification panel, etc
Future Enhancements
Online Banking Integration: Extend the system to manage online banking features such as online transfers, bill payments, etc
Reports & Analytics: Include reporting capability to generate customers and loan reports
Multi-user Roles: Provide different access levels such as Admin, Manager, Customer to log in and look at different areas.
