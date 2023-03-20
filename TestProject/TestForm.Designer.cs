
namespace TestProject
{
    partial class TestForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btRunFull = new System.Windows.Forms.Button();
            this.btRunUsers = new System.Windows.Forms.Button();
            this.btTestSearch = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btFtpApiByTimer = new System.Windows.Forms.Button();
            this.btFtpUpdate = new System.Windows.Forms.Button();
            this.btApiUpdate = new System.Windows.Forms.Button();
            this.btStartUserByTimer = new System.Windows.Forms.Button();
            this.txtUsername = new System.Windows.Forms.TextBox();
            this.btRunSpecificUser = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btUpdateDeliveryDates = new System.Windows.Forms.Button();
            this.btCloseasWon = new System.Windows.Forms.Button();
            this.txtOppName = new System.Windows.Forms.TextBox();
            this.btCreateOpportunity = new System.Windows.Forms.Button();
            this.btUpdateOwner = new System.Windows.Forms.Button();
            this.btUnpaidInvoices = new System.Windows.Forms.Button();
            this.btIsInCRM = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.btTestBO = new System.Windows.Forms.Button();
            this.btTestSocket = new System.Windows.Forms.Button();
            this.btTestCurrency = new System.Windows.Forms.Button();
            this.btCheckIsInCRM = new System.Windows.Forms.Button();
            this.btGetName = new System.Windows.Forms.Button();
            this.btTestSeach = new System.Windows.Forms.Button();
            this.btStopFull = new System.Windows.Forms.Button();
            this.btStartHost = new System.Windows.Forms.Button();
            this.btTestPunchOUT = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // btRunFull
            // 
            this.btRunFull.Location = new System.Drawing.Point(6, 19);
            this.btRunFull.Name = "btRunFull";
            this.btRunFull.Size = new System.Drawing.Size(138, 23);
            this.btRunFull.TabIndex = 0;
            this.btRunFull.Text = "Run full";
            this.btRunFull.UseVisualStyleBackColor = true;
            this.btRunFull.Click += new System.EventHandler(this.btRunFull_Click);
            // 
            // btRunUsers
            // 
            this.btRunUsers.Location = new System.Drawing.Point(6, 19);
            this.btRunUsers.Name = "btRunUsers";
            this.btRunUsers.Size = new System.Drawing.Size(138, 23);
            this.btRunUsers.TabIndex = 1;
            this.btRunUsers.Text = "Run user check";
            this.btRunUsers.UseVisualStyleBackColor = true;
            this.btRunUsers.Click += new System.EventHandler(this.btRunUsers_Click);
            // 
            // btTestSearch
            // 
            this.btTestSearch.Location = new System.Drawing.Point(6, 48);
            this.btTestSearch.Name = "btTestSearch";
            this.btTestSearch.Size = new System.Drawing.Size(138, 23);
            this.btTestSearch.TabIndex = 2;
            this.btTestSearch.Text = "Test CRM search";
            this.btTestSearch.UseVisualStyleBackColor = true;
            this.btTestSearch.Click += new System.EventHandler(this.btTestSearch_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btFtpApiByTimer);
            this.groupBox1.Controls.Add(this.btFtpUpdate);
            this.groupBox1.Controls.Add(this.btApiUpdate);
            this.groupBox1.Controls.Add(this.btStartUserByTimer);
            this.groupBox1.Controls.Add(this.txtUsername);
            this.groupBox1.Controls.Add(this.btRunSpecificUser);
            this.groupBox1.Controls.Add(this.btRunUsers);
            this.groupBox1.Location = new System.Drawing.Point(177, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(156, 310);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Users";
            // 
            // btFtpApiByTimer
            // 
            this.btFtpApiByTimer.Location = new System.Drawing.Point(6, 195);
            this.btFtpApiByTimer.Name = "btFtpApiByTimer";
            this.btFtpApiByTimer.Size = new System.Drawing.Size(138, 23);
            this.btFtpApiByTimer.TabIndex = 6;
            this.btFtpApiByTimer.Text = "By api+ftp";
            this.btFtpApiByTimer.UseVisualStyleBackColor = true;
            this.btFtpApiByTimer.Click += new System.EventHandler(this.btFtpApiByTimer_Click);
            // 
            // btFtpUpdate
            // 
            this.btFtpUpdate.Location = new System.Drawing.Point(6, 166);
            this.btFtpUpdate.Name = "btFtpUpdate";
            this.btFtpUpdate.Size = new System.Drawing.Size(138, 23);
            this.btFtpUpdate.TabIndex = 5;
            this.btFtpUpdate.Text = "Run FTP update";
            this.btFtpUpdate.UseVisualStyleBackColor = true;
            this.btFtpUpdate.Click += new System.EventHandler(this.btFtpUpdate_Click);
            // 
            // btApiUpdate
            // 
            this.btApiUpdate.Location = new System.Drawing.Point(6, 137);
            this.btApiUpdate.Name = "btApiUpdate";
            this.btApiUpdate.Size = new System.Drawing.Size(138, 23);
            this.btApiUpdate.TabIndex = 5;
            this.btApiUpdate.Text = "Run API update";
            this.btApiUpdate.UseVisualStyleBackColor = true;
            this.btApiUpdate.Click += new System.EventHandler(this.btFtpApiUpdate_Click);
            // 
            // btStartUserByTimer
            // 
            this.btStartUserByTimer.Location = new System.Drawing.Point(6, 106);
            this.btStartUserByTimer.Name = "btStartUserByTimer";
            this.btStartUserByTimer.Size = new System.Drawing.Size(138, 23);
            this.btStartUserByTimer.TabIndex = 4;
            this.btStartUserByTimer.Text = "Start by timer";
            this.btStartUserByTimer.UseVisualStyleBackColor = true;
            this.btStartUserByTimer.Click += new System.EventHandler(this.btStartUserByTimer_Click);
            // 
            // txtUsername
            // 
            this.txtUsername.Location = new System.Drawing.Point(6, 80);
            this.txtUsername.Name = "txtUsername";
            this.txtUsername.Size = new System.Drawing.Size(138, 20);
            this.txtUsername.TabIndex = 3;
            // 
            // btRunSpecificUser
            // 
            this.btRunSpecificUser.Enabled = false;
            this.btRunSpecificUser.Location = new System.Drawing.Point(6, 48);
            this.btRunSpecificUser.Name = "btRunSpecificUser";
            this.btRunSpecificUser.Size = new System.Drawing.Size(138, 23);
            this.btRunSpecificUser.TabIndex = 2;
            this.btRunSpecificUser.Text = "Run specific user";
            this.btRunSpecificUser.UseVisualStyleBackColor = true;
            this.btRunSpecificUser.Click += new System.EventHandler(this.btRunSpecificUser_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btTestPunchOUT);
            this.groupBox2.Controls.Add(this.btUpdateDeliveryDates);
            this.groupBox2.Controls.Add(this.btCloseasWon);
            this.groupBox2.Controls.Add(this.txtOppName);
            this.groupBox2.Controls.Add(this.btCreateOpportunity);
            this.groupBox2.Controls.Add(this.btUpdateOwner);
            this.groupBox2.Controls.Add(this.btUnpaidInvoices);
            this.groupBox2.Controls.Add(this.btIsInCRM);
            this.groupBox2.Location = new System.Drawing.Point(339, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(184, 310);
            this.groupBox2.TabIndex = 4;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Orders/Opportunities";
            // 
            // btUpdateDeliveryDates
            // 
            this.btUpdateDeliveryDates.Location = new System.Drawing.Point(6, 195);
            this.btUpdateDeliveryDates.Name = "btUpdateDeliveryDates";
            this.btUpdateDeliveryDates.Size = new System.Drawing.Size(172, 23);
            this.btUpdateDeliveryDates.TabIndex = 6;
            this.btUpdateDeliveryDates.Text = "Update delivery dates";
            this.btUpdateDeliveryDates.UseVisualStyleBackColor = true;
            this.btUpdateDeliveryDates.Click += new System.EventHandler(this.btUpdateDeliveryDates_Click);
            // 
            // btCloseasWon
            // 
            this.btCloseasWon.Location = new System.Drawing.Point(6, 166);
            this.btCloseasWon.Name = "btCloseasWon";
            this.btCloseasWon.Size = new System.Drawing.Size(172, 23);
            this.btCloseasWon.TabIndex = 5;
            this.btCloseasWon.Text = "Close as won";
            this.btCloseasWon.UseVisualStyleBackColor = true;
            this.btCloseasWon.Click += new System.EventHandler(this.btCloseasWon_Click);
            // 
            // txtOppName
            // 
            this.txtOppName.Location = new System.Drawing.Point(6, 135);
            this.txtOppName.Name = "txtOppName";
            this.txtOppName.Size = new System.Drawing.Size(172, 20);
            this.txtOppName.TabIndex = 4;
            // 
            // btCreateOpportunity
            // 
            this.btCreateOpportunity.Location = new System.Drawing.Point(6, 106);
            this.btCreateOpportunity.Name = "btCreateOpportunity";
            this.btCreateOpportunity.Size = new System.Drawing.Size(172, 23);
            this.btCreateOpportunity.TabIndex = 3;
            this.btCreateOpportunity.Text = "Create opportunity (!!!)";
            this.btCreateOpportunity.UseVisualStyleBackColor = true;
            this.btCreateOpportunity.Click += new System.EventHandler(this.btCreateOpportunity_Click);
            // 
            // btUpdateOwner
            // 
            this.btUpdateOwner.Location = new System.Drawing.Point(6, 77);
            this.btUpdateOwner.Name = "btUpdateOwner";
            this.btUpdateOwner.Size = new System.Drawing.Size(172, 23);
            this.btUpdateOwner.TabIndex = 2;
            this.btUpdateOwner.Text = "Update owners";
            this.btUpdateOwner.UseVisualStyleBackColor = true;
            this.btUpdateOwner.Click += new System.EventHandler(this.btUpdateOwner_Click);
            // 
            // btUnpaidInvoices
            // 
            this.btUnpaidInvoices.Location = new System.Drawing.Point(6, 48);
            this.btUnpaidInvoices.Name = "btUnpaidInvoices";
            this.btUnpaidInvoices.Size = new System.Drawing.Size(172, 23);
            this.btUnpaidInvoices.TabIndex = 1;
            this.btUnpaidInvoices.Text = "Unpaid invoices";
            this.btUnpaidInvoices.UseVisualStyleBackColor = true;
            this.btUnpaidInvoices.Click += new System.EventHandler(this.btUnpaidInvoices_Click);
            // 
            // btIsInCRM
            // 
            this.btIsInCRM.Location = new System.Drawing.Point(6, 19);
            this.btIsInCRM.Name = "btIsInCRM";
            this.btIsInCRM.Size = new System.Drawing.Size(172, 23);
            this.btIsInCRM.TabIndex = 0;
            this.btIsInCRM.Text = "Update IsInCRM field";
            this.btIsInCRM.UseVisualStyleBackColor = true;
            this.btIsInCRM.Click += new System.EventHandler(this.btIsInCRM_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.btTestBO);
            this.groupBox3.Controls.Add(this.btTestSocket);
            this.groupBox3.Controls.Add(this.btTestCurrency);
            this.groupBox3.Controls.Add(this.btCheckIsInCRM);
            this.groupBox3.Controls.Add(this.btGetName);
            this.groupBox3.Controls.Add(this.btTestSeach);
            this.groupBox3.Controls.Add(this.btStopFull);
            this.groupBox3.Controls.Add(this.btStartHost);
            this.groupBox3.Controls.Add(this.btRunFull);
            this.groupBox3.Controls.Add(this.btTestSearch);
            this.groupBox3.Location = new System.Drawing.Point(12, 12);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(159, 310);
            this.groupBox3.TabIndex = 5;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Common";
            // 
            // btTestBO
            // 
            this.btTestBO.Location = new System.Drawing.Point(6, 281);
            this.btTestBO.Name = "btTestBO";
            this.btTestBO.Size = new System.Drawing.Size(138, 23);
            this.btTestBO.TabIndex = 7;
            this.btTestBO.Text = "Test BO integration";
            this.btTestBO.UseVisualStyleBackColor = true;
            this.btTestBO.Click += new System.EventHandler(this.btTestBO_Click);
            // 
            // btTestSocket
            // 
            this.btTestSocket.Location = new System.Drawing.Point(7, 253);
            this.btTestSocket.Name = "btTestSocket";
            this.btTestSocket.Size = new System.Drawing.Size(137, 23);
            this.btTestSocket.TabIndex = 9;
            this.btTestSocket.Text = "Test local socket";
            this.btTestSocket.UseVisualStyleBackColor = true;
            this.btTestSocket.Click += new System.EventHandler(this.btTestSocket_Click);
            // 
            // btTestCurrency
            // 
            this.btTestCurrency.Location = new System.Drawing.Point(7, 195);
            this.btTestCurrency.Name = "btTestCurrency";
            this.btTestCurrency.Size = new System.Drawing.Size(137, 23);
            this.btTestCurrency.TabIndex = 8;
            this.btTestCurrency.Text = "Test Currency";
            this.btTestCurrency.UseVisualStyleBackColor = true;
            this.btTestCurrency.Click += new System.EventHandler(this.btTestCurrency_Click);
            // 
            // btCheckIsInCRM
            // 
            this.btCheckIsInCRM.Location = new System.Drawing.Point(7, 166);
            this.btCheckIsInCRM.Name = "btCheckIsInCRM";
            this.btCheckIsInCRM.Size = new System.Drawing.Size(137, 23);
            this.btCheckIsInCRM.TabIndex = 7;
            this.btCheckIsInCRM.Text = "Check all year IsInCrm";
            this.btCheckIsInCRM.UseVisualStyleBackColor = true;
            this.btCheckIsInCRM.Click += new System.EventHandler(this.btCheckIsInCRM_Click);
            // 
            // btGetName
            // 
            this.btGetName.Location = new System.Drawing.Point(7, 224);
            this.btGetName.Name = "btGetName";
            this.btGetName.Size = new System.Drawing.Size(137, 23);
            this.btGetName.TabIndex = 6;
            this.btGetName.Text = "Get local machine name";
            this.btGetName.UseVisualStyleBackColor = true;
            this.btGetName.Click += new System.EventHandler(this.btGetName_Click);
            // 
            // btTestSeach
            // 
            this.btTestSeach.Location = new System.Drawing.Point(7, 105);
            this.btTestSeach.Name = "btTestSeach";
            this.btTestSeach.Size = new System.Drawing.Size(137, 23);
            this.btTestSeach.TabIndex = 5;
            this.btTestSeach.Text = "Test Search";
            this.btTestSeach.UseVisualStyleBackColor = true;
            this.btTestSeach.Click += new System.EventHandler(this.btTestSeach_Click);
            // 
            // btStopFull
            // 
            this.btStopFull.Location = new System.Drawing.Point(6, 137);
            this.btStopFull.Name = "btStopFull";
            this.btStopFull.Size = new System.Drawing.Size(138, 23);
            this.btStopFull.TabIndex = 4;
            this.btStopFull.Text = "Stop full";
            this.btStopFull.UseVisualStyleBackColor = true;
            this.btStopFull.Click += new System.EventHandler(this.btStopFull_Click);
            // 
            // btStartHost
            // 
            this.btStartHost.Location = new System.Drawing.Point(6, 77);
            this.btStartHost.Name = "btStartHost";
            this.btStartHost.Size = new System.Drawing.Size(138, 23);
            this.btStartHost.TabIndex = 3;
            this.btStartHost.Text = "Start host";
            this.btStartHost.UseVisualStyleBackColor = true;
            this.btStartHost.Click += new System.EventHandler(this.btStartHost_Click);
            // 
            // btTestPunchOUT
            // 
            this.btTestPunchOUT.Location = new System.Drawing.Point(6, 224);
            this.btTestPunchOUT.Name = "btTestPunchOUT";
            this.btTestPunchOUT.Size = new System.Drawing.Size(172, 23);
            this.btTestPunchOUT.TabIndex = 7;
            this.btTestPunchOUT.Text = "Test punchout";
            this.btTestPunchOUT.UseVisualStyleBackColor = true;
            this.btTestPunchOUT.Click += new System.EventHandler(this.btTestPunchOUT_Click);
            // 
            // TestForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(531, 334);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Name = "TestForm";
            this.Text = "Test Form";
            this.Load += new System.EventHandler(this.TestForm_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btRunFull;
        private System.Windows.Forms.Button btRunUsers;
        private System.Windows.Forms.Button btTestSearch;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button btIsInCRM;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button btStartHost;
        private System.Windows.Forms.Button btUnpaidInvoices;
        private System.Windows.Forms.Button btUpdateOwner;
        private System.Windows.Forms.Button btStopFull;
        private System.Windows.Forms.TextBox txtUsername;
        private System.Windows.Forms.Button btRunSpecificUser;
        private System.Windows.Forms.Button btStartUserByTimer;
        private System.Windows.Forms.TextBox txtOppName;
        private System.Windows.Forms.Button btCreateOpportunity;
        private System.Windows.Forms.Button btTestSeach;
        private System.Windows.Forms.Button btApiUpdate;
        private System.Windows.Forms.Button btFtpUpdate;
        private System.Windows.Forms.Button btGetName;
        private System.Windows.Forms.Button btFtpApiByTimer;
        private System.Windows.Forms.Button btCheckIsInCRM;
        private System.Windows.Forms.Button btCloseasWon;
        private System.Windows.Forms.Button btTestCurrency;
        private System.Windows.Forms.Button btTestSocket;
        private System.Windows.Forms.Button btUpdateDeliveryDates;
        private System.Windows.Forms.Button btTestBO;
        private System.Windows.Forms.Button btTestPunchOUT;
    }
}

