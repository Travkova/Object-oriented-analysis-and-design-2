using System.Drawing;
using System.Windows.Forms;

namespace lr1_withoutPattern
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;

        // Элементы формы
        private System.Windows.Forms.GroupBox grpCreate;
        private System.Windows.Forms.Label lblDocName;
        private System.Windows.Forms.TextBox txtDocName;
        private System.Windows.Forms.Label lblDocText;
        private System.Windows.Forms.TextBox txtDocText;
        private System.Windows.Forms.Label lblPrinter;
        private System.Windows.Forms.ComboBox cmbPrinter;
        private System.Windows.Forms.Button btnAddToManager1;
        private System.Windows.Forms.Button btnAddToManager2;
        private System.Windows.Forms.Button btnShowProblem;
        private System.Windows.Forms.Button btnClearLog;

        private System.Windows.Forms.GroupBox grpManager1;
        private System.Windows.Forms.Label lblManager1Id;
        private System.Windows.Forms.Label lblManager1Queue;
        private System.Windows.Forms.ListView lvManager1Docs;
        private System.Windows.Forms.ColumnHeader colDoc1Name;
        private System.Windows.Forms.ColumnHeader colDoc1Status;
        private System.Windows.Forms.ColumnHeader colDoc1Printer;
        private System.Windows.Forms.ListView lvManager1Printers;
        private System.Windows.Forms.ColumnHeader colPrinter1Name;
        private System.Windows.Forms.ColumnHeader colPrinter1Status;
        private System.Windows.Forms.Button btnProcessManager1;
        private System.Windows.Forms.Button btnClearManager1;

        private System.Windows.Forms.GroupBox grpManager2;
        private System.Windows.Forms.Label lblManager2Id;
        private System.Windows.Forms.Label lblManager2Queue;
        private System.Windows.Forms.ListView lvManager2Docs;
        private System.Windows.Forms.ColumnHeader colDoc2Name;
        private System.Windows.Forms.ColumnHeader colDoc2Status;
        private System.Windows.Forms.ColumnHeader colDoc2Printer;
        private System.Windows.Forms.ListView lvManager2Printers;
        private System.Windows.Forms.ColumnHeader colPrinter2Name;
        private System.Windows.Forms.ColumnHeader colPrinter2Status;
        private System.Windows.Forms.Button btnProcessManager2;
        private System.Windows.Forms.Button btnClearManager2;

        private System.Windows.Forms.GroupBox grpLog;
        private System.Windows.Forms.ListBox lbLog;

        private void InitializeComponent()
        {
            this.grpCreate = new System.Windows.Forms.GroupBox();
            this.lblDocName = new System.Windows.Forms.Label();
            this.txtDocName = new System.Windows.Forms.TextBox();
            this.lblDocText = new System.Windows.Forms.Label();
            this.txtDocText = new System.Windows.Forms.TextBox();
            this.lblPrinter = new System.Windows.Forms.Label();
            this.cmbPrinter = new System.Windows.Forms.ComboBox();
            this.btnAddToManager1 = new System.Windows.Forms.Button();
            this.btnAddToManager2 = new System.Windows.Forms.Button();
            this.btnShowProblem = new System.Windows.Forms.Button();
            this.btnClearLog = new System.Windows.Forms.Button();
            this.grpManager1 = new System.Windows.Forms.GroupBox();
            this.lblManager1Id = new System.Windows.Forms.Label();
            this.lblManager1Queue = new System.Windows.Forms.Label();
            this.lvManager1Docs = new System.Windows.Forms.ListView();
            this.colDoc1Name = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colDoc1Status = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colDoc1Printer = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.lvManager1Printers = new System.Windows.Forms.ListView();
            this.colPrinter1Name = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colPrinter1Status = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.btnProcessManager1 = new System.Windows.Forms.Button();
            this.btnClearManager1 = new System.Windows.Forms.Button();
            this.grpManager2 = new System.Windows.Forms.GroupBox();
            this.lblManager2Id = new System.Windows.Forms.Label();
            this.lblManager2Queue = new System.Windows.Forms.Label();
            this.lvManager2Docs = new System.Windows.Forms.ListView();
            this.colDoc2Name = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colDoc2Status = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colDoc2Printer = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.lvManager2Printers = new System.Windows.Forms.ListView();
            this.colPrinter2Name = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colPrinter2Status = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.btnProcessManager2 = new System.Windows.Forms.Button();
            this.btnClearManager2 = new System.Windows.Forms.Button();
            this.grpLog = new System.Windows.Forms.GroupBox();
            this.lbLog = new System.Windows.Forms.ListBox();
            this.grpCreate.SuspendLayout();
            this.grpManager1.SuspendLayout();
            this.grpManager2.SuspendLayout();
            this.grpLog.SuspendLayout();
            this.SuspendLayout();
            // 
            // grpCreate
            // 
            this.grpCreate.Controls.Add(this.lblDocName);
            this.grpCreate.Controls.Add(this.txtDocName);
            this.grpCreate.Controls.Add(this.lblDocText);
            this.grpCreate.Controls.Add(this.txtDocText);
            this.grpCreate.Controls.Add(this.lblPrinter);
            this.grpCreate.Controls.Add(this.cmbPrinter);
            this.grpCreate.Controls.Add(this.btnAddToManager1);
            this.grpCreate.Controls.Add(this.btnAddToManager2);
            this.grpCreate.Controls.Add(this.btnShowProblem);
            this.grpCreate.Controls.Add(this.btnClearLog);
            this.grpCreate.Location = new System.Drawing.Point(14, 13);
            this.grpCreate.Name = "grpCreate";
            this.grpCreate.Size = new System.Drawing.Size(983, 160);
            this.grpCreate.TabIndex = 0;
            this.grpCreate.TabStop = false;
            this.grpCreate.Text = "Создание документа";
            // 
            // lblDocName
            // 
            this.lblDocName.Location = new System.Drawing.Point(17, 32);
            this.lblDocName.Name = "lblDocName";
            this.lblDocName.Size = new System.Drawing.Size(80, 25);
            this.lblDocName.TabIndex = 0;
            this.lblDocName.Text = "Название:";
            // 
            // txtDocName
            // 
            this.txtDocName.Location = new System.Drawing.Point(103, 29);
            this.txtDocName.Name = "txtDocName";
            this.txtDocName.Size = new System.Drawing.Size(228, 22);
            this.txtDocName.TabIndex = 0;
            // 
            // lblDocText
            // 
            this.lblDocText.Location = new System.Drawing.Point(343, 32);
            this.lblDocText.Name = "lblDocText";
            this.lblDocText.Size = new System.Drawing.Size(51, 21);
            this.lblDocText.TabIndex = 1;
            this.lblDocText.Text = "Текст:";
            // 
            // txtDocText
            // 
            this.txtDocText.Location = new System.Drawing.Point(400, 29);
            this.txtDocText.Name = "txtDocText";
            this.txtDocText.Size = new System.Drawing.Size(228, 22);
            this.txtDocText.TabIndex = 1;
            // 
            // lblPrinter
            // 
            this.lblPrinter.Location = new System.Drawing.Point(640, 32);
            this.lblPrinter.Name = "lblPrinter";
            this.lblPrinter.Size = new System.Drawing.Size(80, 25);
            this.lblPrinter.TabIndex = 2;
            this.lblPrinter.Text = "Принтер:";
            // 
            // cmbPrinter
            // 
            this.cmbPrinter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbPrinter.Location = new System.Drawing.Point(720, 29);
            this.cmbPrinter.Name = "cmbPrinter";
            this.cmbPrinter.Size = new System.Drawing.Size(171, 24);
            this.cmbPrinter.TabIndex = 2;
            // 
            // btnAddToManager1
            // 
            this.btnAddToManager1.BackColor = System.Drawing.Color.LightCoral;
            this.btnAddToManager1.Location = new System.Drawing.Point(17, 75);
            this.btnAddToManager1.Name = "btnAddToManager1";
            this.btnAddToManager1.Size = new System.Drawing.Size(149, 37);
            this.btnAddToManager1.TabIndex = 3;
            this.btnAddToManager1.Text = "+ в менеджер 1";
            this.btnAddToManager1.UseVisualStyleBackColor = false;
            this.btnAddToManager1.Click += new System.EventHandler(this.btnAddToManager1_Click);
            // 
            // btnAddToManager2
            // 
            this.btnAddToManager2.BackColor = System.Drawing.Color.LightBlue;
            this.btnAddToManager2.Location = new System.Drawing.Point(183, 75);
            this.btnAddToManager2.Name = "btnAddToManager2";
            this.btnAddToManager2.Size = new System.Drawing.Size(149, 37);
            this.btnAddToManager2.TabIndex = 4;
            this.btnAddToManager2.Text = "+ в менеджер 2";
            this.btnAddToManager2.UseVisualStyleBackColor = false;
            this.btnAddToManager2.Click += new System.EventHandler(this.btnAddToManager2_Click);
            // 
            // btnShowProblem
            // 
            this.btnShowProblem.Location = new System.Drawing.Point(400, 75);
            this.btnShowProblem.Name = "btnShowProblem";
            this.btnShowProblem.Size = new System.Drawing.Size(171, 37);
            this.btnShowProblem.TabIndex = 5;
            this.btnShowProblem.Text = "Показать проблему";
            this.btnShowProblem.Click += new System.EventHandler(this.btnShowProblem_Click);
            // 
            // btnClearLog
            // 
            this.btnClearLog.Location = new System.Drawing.Point(594, 75);
            this.btnClearLog.Name = "btnClearLog";
            this.btnClearLog.Size = new System.Drawing.Size(137, 37);
            this.btnClearLog.TabIndex = 6;
            this.btnClearLog.Text = "Очистить лог";
            this.btnClearLog.Click += new System.EventHandler(this.btnClearLog_Click);
            // 
            // grpManager1
            // 
            this.grpManager1.Controls.Add(this.lblManager1Id);
            this.grpManager1.Controls.Add(this.lblManager1Queue);
            this.grpManager1.Controls.Add(this.lvManager1Docs);
            this.grpManager1.Controls.Add(this.lvManager1Printers);
            this.grpManager1.Controls.Add(this.btnProcessManager1);
            this.grpManager1.Controls.Add(this.btnClearManager1);
            this.grpManager1.Location = new System.Drawing.Point(14, 181);
            this.grpManager1.Name = "grpManager1";
            this.grpManager1.Size = new System.Drawing.Size(480, 405);
            this.grpManager1.TabIndex = 1;
            this.grpManager1.TabStop = false;
            this.grpManager1.Text = "Менеджер 1";
            // 
            // lblManager1Id
            // 
            this.lblManager1Id.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold);
            this.lblManager1Id.ForeColor = System.Drawing.Color.Red;
            this.lblManager1Id.Location = new System.Drawing.Point(17, 27);
            this.lblManager1Id.Name = "lblManager1Id";
            this.lblManager1Id.Size = new System.Drawing.Size(171, 25);
            this.lblManager1Id.TabIndex = 0;
            this.lblManager1Id.Text = "ID: ---";
            // 
            // lblManager1Queue
            // 
            this.lblManager1Queue.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold);
            this.lblManager1Queue.Location = new System.Drawing.Point(320, 27);
            this.lblManager1Queue.Name = "lblManager1Queue";
            this.lblManager1Queue.Size = new System.Drawing.Size(114, 25);
            this.lblManager1Queue.TabIndex = 1;
            this.lblManager1Queue.Text = "Очередь: 0";
            // 
            // lvManager1Docs
            // 
            this.lvManager1Docs.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colDoc1Name,
            this.colDoc1Status,
            this.colDoc1Printer});
            this.lvManager1Docs.FullRowSelect = true;
            this.lvManager1Docs.GridLines = true;
            this.lvManager1Docs.HideSelection = false;
            this.lvManager1Docs.Location = new System.Drawing.Point(17, 59);
            this.lvManager1Docs.Name = "lvManager1Docs";
            this.lvManager1Docs.Size = new System.Drawing.Size(445, 160);
            this.lvManager1Docs.TabIndex = 2;
            this.lvManager1Docs.UseCompatibleStateImageBehavior = false;
            this.lvManager1Docs.View = System.Windows.Forms.View.Details;
            // 
            // colDoc1Name
            // 
            this.colDoc1Name.Text = "Документ";
            this.colDoc1Name.Width = 120;
            // 
            // colDoc1Status
            // 
            this.colDoc1Status.Text = "СтатусДокумента";
            this.colDoc1Status.Width = 130;
            // 
            // colDoc1Printer
            // 
            this.colDoc1Printer.Text = "НаПринтер";
            this.colDoc1Printer.Width = 110;
            // 
            // lvManager1Printers
            // 
            this.lvManager1Printers.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colPrinter1Name,
            this.colPrinter1Status});
            this.lvManager1Printers.FullRowSelect = true;
            this.lvManager1Printers.GridLines = true;
            this.lvManager1Printers.HideSelection = false;
            this.lvManager1Printers.Location = new System.Drawing.Point(17, 229);
            this.lvManager1Printers.Name = "lvManager1Printers";
            this.lvManager1Printers.Size = new System.Drawing.Size(445, 96);
            this.lvManager1Printers.TabIndex = 3;
            this.lvManager1Printers.UseCompatibleStateImageBehavior = false;
            this.lvManager1Printers.View = System.Windows.Forms.View.Details;
            // 
            // colPrinter1Name
            // 
            this.colPrinter1Name.Text = "Принтер";
            this.colPrinter1Name.Width = 180;
            // 
            // colPrinter1Status
            // 
            this.colPrinter1Status.Text = "СтатусПринтера";
            this.colPrinter1Status.Width = 180;
            // 
            // btnProcessManager1
            // 
            this.btnProcessManager1.Location = new System.Drawing.Point(17, 336);
            this.btnProcessManager1.Name = "btnProcessManager1";
            this.btnProcessManager1.Size = new System.Drawing.Size(137, 37);
            this.btnProcessManager1.TabIndex = 4;
            this.btnProcessManager1.Text = "Обработать";
            this.btnProcessManager1.Click += new System.EventHandler(this.btnProcessManager1_Click);
            // 
            // btnClearManager1
            // 
            this.btnClearManager1.Location = new System.Drawing.Point(171, 336);
            this.btnClearManager1.Name = "btnClearManager1";
            this.btnClearManager1.Size = new System.Drawing.Size(137, 37);
            this.btnClearManager1.TabIndex = 5;
            this.btnClearManager1.Text = "Очистить";
            this.btnClearManager1.Click += new System.EventHandler(this.btnClearManager1_Click);
            // 
            // grpManager2
            // 
            this.grpManager2.Controls.Add(this.lblManager2Id);
            this.grpManager2.Controls.Add(this.lblManager2Queue);
            this.grpManager2.Controls.Add(this.lvManager2Docs);
            this.grpManager2.Controls.Add(this.lvManager2Printers);
            this.grpManager2.Controls.Add(this.btnProcessManager2);
            this.grpManager2.Controls.Add(this.btnClearManager2);
            this.grpManager2.Location = new System.Drawing.Point(517, 181);
            this.grpManager2.Name = "grpManager2";
            this.grpManager2.Size = new System.Drawing.Size(480, 405);
            this.grpManager2.TabIndex = 2;
            this.grpManager2.TabStop = false;
            this.grpManager2.Text = "Менеджер 2";
            // 
            // lblManager2Id
            // 
            this.lblManager2Id.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold);
            this.lblManager2Id.ForeColor = System.Drawing.Color.Blue;
            this.lblManager2Id.Location = new System.Drawing.Point(17, 27);
            this.lblManager2Id.Name = "lblManager2Id";
            this.lblManager2Id.Size = new System.Drawing.Size(171, 25);
            this.lblManager2Id.TabIndex = 0;
            this.lblManager2Id.Text = "ID: ---";
            // 
            // lblManager2Queue
            // 
            this.lblManager2Queue.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold);
            this.lblManager2Queue.Location = new System.Drawing.Point(320, 27);
            this.lblManager2Queue.Name = "lblManager2Queue";
            this.lblManager2Queue.Size = new System.Drawing.Size(114, 25);
            this.lblManager2Queue.TabIndex = 1;
            this.lblManager2Queue.Text = "Очередь: 0";
            // 
            // lvManager2Docs
            // 
            this.lvManager2Docs.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colDoc2Name,
            this.colDoc2Status,
            this.colDoc2Printer});
            this.lvManager2Docs.FullRowSelect = true;
            this.lvManager2Docs.GridLines = true;
            this.lvManager2Docs.HideSelection = false;
            this.lvManager2Docs.Location = new System.Drawing.Point(17, 59);
            this.lvManager2Docs.Name = "lvManager2Docs";
            this.lvManager2Docs.Size = new System.Drawing.Size(445, 160);
            this.lvManager2Docs.TabIndex = 2;
            this.lvManager2Docs.UseCompatibleStateImageBehavior = false;
            this.lvManager2Docs.View = System.Windows.Forms.View.Details;
            // 
            // colDoc2Name
            // 
            this.colDoc2Name.Text = "Документ";
            this.colDoc2Name.Width = 120;
            // 
            // colDoc2Status
            // 
            this.colDoc2Status.Text = "СтатусДокумента";
            this.colDoc2Status.Width = 130;
            // 
            // colDoc2Printer
            // 
            this.colDoc2Printer.Text = "НаПринтер";
            this.colDoc2Printer.Width = 110;
            // 
            // lvManager2Printers
            // 
            this.lvManager2Printers.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colPrinter2Name,
            this.colPrinter2Status});
            this.lvManager2Printers.FullRowSelect = true;
            this.lvManager2Printers.GridLines = true;
            this.lvManager2Printers.HideSelection = false;
            this.lvManager2Printers.Location = new System.Drawing.Point(17, 229);
            this.lvManager2Printers.Name = "lvManager2Printers";
            this.lvManager2Printers.Size = new System.Drawing.Size(445, 96);
            this.lvManager2Printers.TabIndex = 3;
            this.lvManager2Printers.UseCompatibleStateImageBehavior = false;
            this.lvManager2Printers.View = System.Windows.Forms.View.Details;
            // 
            // colPrinter2Name
            // 
            this.colPrinter2Name.Text = "Принтер";
            this.colPrinter2Name.Width = 180;
            // 
            // colPrinter2Status
            // 
            this.colPrinter2Status.Text = "СтатусПринтера";
            this.colPrinter2Status.Width = 180;
            // 
            // btnProcessManager2
            // 
            this.btnProcessManager2.Location = new System.Drawing.Point(17, 336);
            this.btnProcessManager2.Name = "btnProcessManager2";
            this.btnProcessManager2.Size = new System.Drawing.Size(137, 37);
            this.btnProcessManager2.TabIndex = 4;
            this.btnProcessManager2.Text = "Обработать";
            this.btnProcessManager2.Click += new System.EventHandler(this.btnProcessManager2_Click);
            // 
            // btnClearManager2
            // 
            this.btnClearManager2.Location = new System.Drawing.Point(171, 336);
            this.btnClearManager2.Name = "btnClearManager2";
            this.btnClearManager2.Size = new System.Drawing.Size(137, 37);
            this.btnClearManager2.TabIndex = 5;
            this.btnClearManager2.Text = "Очистить";
            this.btnClearManager2.Click += new System.EventHandler(this.btnClearManager2_Click);
            // 
            // grpLog
            // 
            this.grpLog.Controls.Add(this.lbLog);
            this.grpLog.Location = new System.Drawing.Point(14, 597);
            this.grpLog.Name = "grpLog";
            this.grpLog.Size = new System.Drawing.Size(983, 160);
            this.grpLog.TabIndex = 3;
            this.grpLog.TabStop = false;
            this.grpLog.Text = "Лог";
            // 
            // lbLog
            // 
            this.lbLog.Font = new System.Drawing.Font("Consolas", 9F);
            this.lbLog.ItemHeight = 18;
            this.lbLog.Location = new System.Drawing.Point(11, 27);
            this.lbLog.Name = "lbLog";
            this.lbLog.Size = new System.Drawing.Size(959, 112);
            this.lbLog.TabIndex = 0;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1010, 769);
            this.Controls.Add(this.grpCreate);
            this.Controls.Add(this.grpManager1);
            this.Controls.Add(this.grpManager2);
            this.Controls.Add(this.grpLog);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Менеджер печати (без паттерна Singleton)";
            this.grpCreate.ResumeLayout(false);
            this.grpCreate.PerformLayout();
            this.grpManager1.ResumeLayout(false);
            this.grpManager2.ResumeLayout(false);
            this.grpLog.ResumeLayout(false);
            this.ResumeLayout(false);

        }
    }
}