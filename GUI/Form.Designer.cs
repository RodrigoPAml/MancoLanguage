namespace GUI
{
    partial class Form
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form));
            tableLayoutPanel1 = new TableLayoutPanel();
            tableLayoutPanel2 = new TableLayoutPanel();
            btnCompile = new Button();
            buttonSave = new Button();
            buttonLoad = new Button();
            ButtonRun = new Button();
            buttonDisableHighlight = new Button();
            comboBoxTransformer = new ComboBox();
            codeTextBox = new RichTextBox();
            tableLayoutPanel4 = new TableLayoutPanel();
            listBoxOutput = new ListBox();
            listBoxCodeOutput = new TextBox();
            tableLayoutPanel3 = new TableLayoutPanel();
            tabControlTranspilado = new TabControl();
            tabPage = new TabPage();
            textBoxGenerated = new RichTextBox();
            tabPage2 = new TabPage();
            textBoxTokens = new RichTextBox();
            tableLayoutPanel1.SuspendLayout();
            tableLayoutPanel2.SuspendLayout();
            tableLayoutPanel4.SuspendLayout();
            tableLayoutPanel3.SuspendLayout();
            tabControlTranspilado.SuspendLayout();
            tabPage.SuspendLayout();
            tabPage2.SuspendLayout();
            SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            tableLayoutPanel1.ColumnCount = 1;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.Controls.Add(tableLayoutPanel2, 0, 0);
            tableLayoutPanel1.Controls.Add(codeTextBox, 0, 1);
            tableLayoutPanel1.Controls.Add(tableLayoutPanel4, 0, 2);
            tableLayoutPanel1.Location = new Point(3, 3);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 3;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 35F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 73.84615F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 26.15385F));
            tableLayoutPanel1.Size = new Size(576, 549);
            tableLayoutPanel1.TabIndex = 0;
            // 
            // tableLayoutPanel2
            // 
            tableLayoutPanel2.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            tableLayoutPanel2.ColumnCount = 6;
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle());
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle());
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle());
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle());
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle());
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle());
            tableLayoutPanel2.Controls.Add(btnCompile, 0, 0);
            tableLayoutPanel2.Controls.Add(buttonSave, 0, 0);
            tableLayoutPanel2.Controls.Add(buttonLoad, 0, 0);
            tableLayoutPanel2.Controls.Add(ButtonRun, 3, 0);
            tableLayoutPanel2.Controls.Add(buttonDisableHighlight, 4, 0);
            tableLayoutPanel2.Controls.Add(comboBoxTransformer, 5, 0);
            tableLayoutPanel2.Location = new Point(3, 3);
            tableLayoutPanel2.Name = "tableLayoutPanel2";
            tableLayoutPanel2.RowCount = 1;
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel2.Size = new Size(570, 29);
            tableLayoutPanel2.TabIndex = 0;
            // 
            // btnCompile
            // 
            btnCompile.Location = new Point(165, 3);
            btnCompile.Name = "btnCompile";
            btnCompile.Size = new Size(75, 23);
            btnCompile.TabIndex = 4;
            btnCompile.Text = "Compile";
            btnCompile.UseVisualStyleBackColor = true;
            btnCompile.Click += btnCompile_Click;
            // 
            // buttonSave
            // 
            buttonSave.Location = new Point(84, 3);
            buttonSave.Name = "buttonSave";
            buttonSave.Size = new Size(75, 23);
            buttonSave.TabIndex = 3;
            buttonSave.Text = "Save";
            buttonSave.UseVisualStyleBackColor = true;
            buttonSave.Click += buttonSave_Click;
            // 
            // buttonLoad
            // 
            buttonLoad.Location = new Point(3, 3);
            buttonLoad.Name = "buttonLoad";
            buttonLoad.Size = new Size(75, 23);
            buttonLoad.TabIndex = 0;
            buttonLoad.Text = "Load";
            buttonLoad.UseVisualStyleBackColor = true;
            buttonLoad.Click += buttonLoad_Click;
            // 
            // ButtonRun
            // 
            ButtonRun.Location = new Point(246, 3);
            ButtonRun.Name = "ButtonRun";
            ButtonRun.Size = new Size(51, 23);
            ButtonRun.TabIndex = 1;
            ButtonRun.Text = "Run";
            ButtonRun.UseVisualStyleBackColor = true;
            ButtonRun.Click += ButtonRun_Click;
            // 
            // buttonDisableHighlight
            // 
            buttonDisableHighlight.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            buttonDisableHighlight.Location = new Point(303, 3);
            buttonDisableHighlight.Name = "buttonDisableHighlight";
            buttonDisableHighlight.Size = new Size(117, 23);
            buttonDisableHighlight.TabIndex = 5;
            buttonDisableHighlight.Text = "Disable Highlight";
            buttonDisableHighlight.UseVisualStyleBackColor = true;
            buttonDisableHighlight.Click += buttonDisableHighlight_Click;
            // 
            // comboBoxTransformer
            // 
            comboBoxTransformer.FormattingEnabled = true;
            comboBoxTransformer.Items.AddRange(new object[] { "Mips Compiler", "C++ Transpiler" });
            comboBoxTransformer.Location = new Point(426, 3);
            comboBoxTransformer.Name = "comboBoxTransformer";
            comboBoxTransformer.Size = new Size(121, 23);
            comboBoxTransformer.TabIndex = 6;
            comboBoxTransformer.SelectedIndexChanged += comboBoxTransformer_SelectedIndexChanged;
            // 
            // codeTextBox
            // 
            codeTextBox.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            codeTextBox.Font = new Font("Segoe UI", 18F, FontStyle.Regular, GraphicsUnit.Point);
            codeTextBox.Location = new Point(3, 38);
            codeTextBox.Name = "codeTextBox";
            codeTextBox.Size = new Size(570, 373);
            codeTextBox.TabIndex = 3;
            codeTextBox.Text = "";
            codeTextBox.TextChanged += richTextBoxCode_TextChanged;
            // 
            // tableLayoutPanel4
            // 
            tableLayoutPanel4.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            tableLayoutPanel4.ColumnCount = 2;
            tableLayoutPanel4.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 66.13844F));
            tableLayoutPanel4.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.86156F));
            tableLayoutPanel4.Controls.Add(listBoxOutput, 0, 0);
            tableLayoutPanel4.Controls.Add(listBoxCodeOutput, 1, 0);
            tableLayoutPanel4.Location = new Point(3, 417);
            tableLayoutPanel4.Name = "tableLayoutPanel4";
            tableLayoutPanel4.RowCount = 1;
            tableLayoutPanel4.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel4.Size = new Size(570, 129);
            tableLayoutPanel4.TabIndex = 4;
            // 
            // listBoxOutput
            // 
            listBoxOutput.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            listBoxOutput.Font = new Font("Segoe UI", 15.75F, FontStyle.Regular, GraphicsUnit.Point);
            listBoxOutput.FormattingEnabled = true;
            listBoxOutput.HorizontalScrollbar = true;
            listBoxOutput.ItemHeight = 30;
            listBoxOutput.Location = new Point(3, 3);
            listBoxOutput.Name = "listBoxOutput";
            listBoxOutput.Size = new Size(370, 94);
            listBoxOutput.TabIndex = 2;
            // 
            // listBoxCodeOutput
            // 
            listBoxCodeOutput.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            listBoxCodeOutput.Font = new Font("Segoe UI", 15.75F, FontStyle.Regular, GraphicsUnit.Point);
            listBoxCodeOutput.Location = new Point(379, 3);
            listBoxCodeOutput.Multiline = true;
            listBoxCodeOutput.Name = "listBoxCodeOutput";
            listBoxCodeOutput.ScrollBars = ScrollBars.Both;
            listBoxCodeOutput.Size = new Size(188, 123);
            listBoxCodeOutput.TabIndex = 3;
            // 
            // tableLayoutPanel3
            // 
            tableLayoutPanel3.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            tableLayoutPanel3.ColumnCount = 2;
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 60.19737F));
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 39.80264F));
            tableLayoutPanel3.Controls.Add(tableLayoutPanel1, 0, 0);
            tableLayoutPanel3.Controls.Add(tabControlTranspilado, 1, 0);
            tableLayoutPanel3.Location = new Point(12, 12);
            tableLayoutPanel3.Name = "tableLayoutPanel3";
            tableLayoutPanel3.RowCount = 1;
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel3.Size = new Size(968, 555);
            tableLayoutPanel3.TabIndex = 1;
            // 
            // tabControlTranspilado
            // 
            tabControlTranspilado.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            tabControlTranspilado.Controls.Add(tabPage);
            tabControlTranspilado.Controls.Add(tabPage2);
            tabControlTranspilado.Location = new Point(585, 3);
            tabControlTranspilado.Name = "tabControlTranspilado";
            tabControlTranspilado.SelectedIndex = 0;
            tabControlTranspilado.Size = new Size(380, 549);
            tabControlTranspilado.TabIndex = 1;
            // 
            // tabPage
            // 
            tabPage.Controls.Add(textBoxGenerated);
            tabPage.Location = new Point(4, 24);
            tabPage.Name = "tabPage";
            tabPage.Padding = new Padding(3);
            tabPage.Size = new Size(372, 521);
            tabPage.TabIndex = 0;
            tabPage.Text = "Compiled";
            tabPage.UseVisualStyleBackColor = true;
            // 
            // textBoxGenerated
            // 
            textBoxGenerated.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            textBoxGenerated.Location = new Point(3, 3);
            textBoxGenerated.Name = "textBoxGenerated";
            textBoxGenerated.ReadOnly = true;
            textBoxGenerated.Size = new Size(363, 512);
            textBoxGenerated.TabIndex = 0;
            textBoxGenerated.Text = "";
            // 
            // tabPage2
            // 
            tabPage2.Controls.Add(textBoxTokens);
            tabPage2.Location = new Point(4, 24);
            tabPage2.Name = "tabPage2";
            tabPage2.Padding = new Padding(3);
            tabPage2.Size = new Size(372, 521);
            tabPage2.TabIndex = 1;
            tabPage2.Text = "Tokens";
            tabPage2.UseVisualStyleBackColor = true;
            // 
            // textBoxTokens
            // 
            textBoxTokens.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            textBoxTokens.Location = new Point(6, 6);
            textBoxTokens.Name = "textBoxTokens";
            textBoxTokens.ReadOnly = true;
            textBoxTokens.Size = new Size(360, 512);
            textBoxTokens.TabIndex = 0;
            textBoxTokens.Text = "";
            textBoxTokens.WordWrap = false;
            // 
            // Form
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(992, 579);
            Controls.Add(tableLayoutPanel3);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "Form";
            Text = "Manco Playground";
            tableLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel2.ResumeLayout(false);
            tableLayoutPanel4.ResumeLayout(false);
            tableLayoutPanel4.PerformLayout();
            tableLayoutPanel3.ResumeLayout(false);
            tabControlTranspilado.ResumeLayout(false);
            tabPage.ResumeLayout(false);
            tabPage2.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private TableLayoutPanel tableLayoutPanel1;
        private TableLayoutPanel tableLayoutPanel2;
        private Button buttonLoad;
        private Button ButtonRun;
        private ListBox listBoxOutput;
        private RichTextBox codeTextBox;
        private TableLayoutPanel tableLayoutPanel3;
        private Button buttonSave;
        private Button btnCompile;
        private TableLayoutPanel tableLayoutPanel4;
        private TextBox listBoxCodeOutput;
        private Button buttonDisableHighlight;
        private TabControl tabControlTranspilado;
        private TabPage tabPage;
        private TabPage tabPage2;
        private RichTextBox textBoxGenerated;
        private RichTextBox textBoxTokens;
        private ComboBox comboBoxTransformer;
    }
}