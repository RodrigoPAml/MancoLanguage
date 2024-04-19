using AssemblerEmulator;
using Language.Common.Exceptions;
using Manco;
using System.Text;
using System.Text.RegularExpressions;
using Timer = System.Windows.Forms.Timer;

namespace GUI
{
    public partial class Form : System.Windows.Forms.Form
    {
        /// <summary>
        /// Timer for events for editor
        /// </summary>
        private Timer _timerBeautify = new Timer();

        /// <summary>
        /// Timer for code verification
        /// </summary>
        private Timer _timerCodeVerify = new Timer();

        /// <summary>
        /// Time when last typed
        /// </summary>
        private DateTime _lastTyped = DateTime.Now;

        /// <summary>
        /// Opened file
        /// </summary>
        private string _openFile = string.Empty;

        /// <summary>
        /// If text has changed
        /// </summary>
        private bool _hasChanged = false;

        /// <summary>
        /// If code hasError
        /// </summary>
        private bool _hasCodeError = false;

        /// <summary>
        /// To disable text highlighting
        /// </summary>
        private bool _disableHighlighting = false;

        /// <summary>
        /// Referencia para formulario
        /// </summary>
        private static Form _form = null;

        /// <summary>
        /// The manco language provider
        /// </summary>
        private MancoProvider _provider = new MancoProvider();

        public Form()
        {
            InitializeComponent();

            _form = this;
            WindowState = FormWindowState.Maximized;

            _timerBeautify = new Timer();
            _timerBeautify.Interval = 2000;
            _timerBeautify.Tick += this.Beautify;
            _timerBeautify.Start();

            _timerCodeVerify = new Timer();
            _timerCodeVerify.Interval = 200;
            _timerCodeVerify.Tick += this.VerifyCode;
            _timerCodeVerify.Start();
        }

        private void btnCompile_Click(object sender, EventArgs e)
        {
            if (_hasCodeError)
                return;

            textBoxGenerated.Text = "";

            try
            {
                listBoxOutput.Items.Clear();

                _provider.SetCode(string.Join('\n', codeTextBox.Text));

                DateTime now = DateTime.Now;
                textBoxGenerated.Text = string.Join('\n', _provider.Compile().Select(x => x).ToList());

                listBoxOutput.Items.Clear();
                listBoxOutput.Items.Add($"Code compiled in {(DateTime.Now - now).TotalMilliseconds} ms");

                RemoveHighlight();
                BeautifyGenerated();
            }
            catch (BaseException bex)
            {
                listBoxOutput.Items.Clear();
                listBoxOutput.Items.Add($"Compilation failed: {bex.Message}, Token: {bex.Token}, ErrorType: {bex.Code}");
            }
            catch (Exception ex)
            {
                listBoxOutput.Items.Clear();
                listBoxOutput.Items.Add($"Fatal error: {ex.Message}");
            }
        }

        private void ButtonRun_Click(object sender, EventArgs e)
        {
            if (_hasCodeError)
                return;

            DateTime now = DateTime.Now;

            listBoxOutput.Items.Clear();
            listBoxCodeOutput.Clear();
            _form.listBoxOutput.Items.Add("Executing program");

            Emulator emulator = new Emulator(null, null, OnSyscall);

            emulator.AddInstructions(textBoxGenerated.Text.Split('\n').ToList());
            
            try
            {
                while (emulator.ExecuteLine())
                { }

                _form.listBoxCodeOutput.AppendText(Environment.NewLine + "Program exited 0");
            }
            catch(Exception ex)
            {
                listBoxCodeOutput.AppendText(Environment.NewLine + $"Program exited 1: {ex.Message}");
            }

            listBoxOutput.Items.Add($"Code executed in {(DateTime.Now - now).TotalMilliseconds} ms");
        }

        /// <summary>
        /// When a syscall is made
        /// </summary>
        private static void OnSyscall(int code, byte[] value)
        {
            if (code == 1)
            {
                int integer = BitConverter.ToInt32(value);
                _form.listBoxCodeOutput.AppendText(integer.ToString());
            }
            else if (code == 2)
            {
                var fl = BitConverter.ToSingle(value);
                _form.listBoxCodeOutput.AppendText(fl.ToString());
            }
            else if (code == 3)
            {
                var fl = Encoding.ASCII.GetString(new byte[] { value[0] });
                fl = fl.Replace("\n", Environment.NewLine);

                _form.listBoxCodeOutput.AppendText(fl);
            }
        }

        /// <summary>
        /// Verify code
        /// </summary>
        private void VerifyCode(object? sender, EventArgs e)
        {
            if (!_hasChanged)
                return;
            
            if ((DateTime.Now - _lastTyped).TotalSeconds < 1)
                return;

            if(listBoxOutput.Items.Count == 0)
                RemoveHighlight();

            try
            {
                _provider.SetCode(string.Join('\n', codeTextBox.Text));
                _provider.Validate();
            }
            catch (BaseException bex)
            {
                listBoxOutput.Items.Clear();

                textBoxGenerated.Text = "";

                if (bex.Token != null)
                    HighlightLine(bex.Token.Line - 1, bex.Token.Start, bex.Token.End, Color.Red);

                listBoxOutput.Items.Add($"Message: {bex.Message}, Token: {bex.Token}, ErrorType: {bex.Code}");
                _hasCodeError = true;
            }
            catch (Exception ex)
            {
                listBoxOutput.Items.Clear();

                textBoxGenerated.Text = "";

                listBoxOutput.Items.Add($"Fatal error: {ex.Message}");
                _hasCodeError = true;
            }
        }

        /// <summary>
        /// Loads the code
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonLoad_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "All files (*.*)|*.*";
            openFileDialog.Title = "Open file";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                _openFile = openFileDialog.FileName;
                string fileContent = File.ReadAllText(_openFile);

                codeTextBox.Text = fileContent;
                codeTextBox.ResumeLayout();
                MessageBox.Show("Opened with success");

                _lastTyped = DateTime.Now.AddSeconds(-4);
                Beautify(null, null);
            }
        }

        /// <summary>
        /// Saves the code
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonSave_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "All files (*.*)|*.*";
            saveFileDialog.Title = "Open file";

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                string path = saveFileDialog.FileName;

                File.WriteAllText(path, codeTextBox.Text);
                MessageBox.Show("Saved with success");
            }
        }

        /// <summary>
        /// When the code changes 
        /// </summary>
        private void richTextBoxCode_TextChanged(object sender, EventArgs e)
        {
            _lastTyped = DateTime.Now;
            _hasChanged = true;

            if(_hasCodeError)
                listBoxOutput.Items.Clear();

            _hasCodeError = false;
        }

        #region ColorHighlight

        /// <summary>
        /// Put colors on syntax
        /// </summary>
        private void Beautify(object? sender, EventArgs e)
        {
            if (_disableHighlighting)
                return;

            if (!_hasChanged)
                return;

            if ((DateTime.Now - _lastTyped).TotalSeconds < 1)
                return;

            int originalCaretPosition = codeTextBox.SelectionStart;
            this.codeTextBox.Visible = false;
            this.codeTextBox.SuspendLayout();

            CheckKeyword(new Regex("\"[^\"]*\""), Color.Orange, codeTextBox);

            CheckKeyword("function", Color.DarkBlue, codeTextBox);
            CheckKeyword("end", Color.DarkBlue, codeTextBox);
            CheckKeyword("while", Color.DarkBlue, codeTextBox);
            CheckKeyword("or", Color.DarkBlue, codeTextBox);
            CheckKeyword("and", Color.DarkBlue, codeTextBox);
            CheckKeyword("break", Color.DarkBlue, codeTextBox);
            CheckKeyword("continue", Color.DarkBlue, codeTextBox);
            CheckKeyword("break", Color.DarkBlue, codeTextBox);
            CheckKeyword("if", Color.DarkBlue, codeTextBox);
            CheckKeyword("elif", Color.DarkBlue, codeTextBox);
            CheckKeyword("else", Color.DarkBlue, codeTextBox);

            CheckKeyword("integer", Color.DarkGreen, codeTextBox);
            CheckKeyword("decimal", Color.DarkGreen, codeTextBox);
            CheckKeyword("bool", Color.DarkGreen, codeTextBox);
            CheckKeyword("string", Color.DarkGreen, codeTextBox);
            CheckKeyword("integer&", Color.DarkGreen, codeTextBox);
            CheckKeyword("decimal&", Color.DarkGreen, codeTextBox);
            CheckKeyword("bool&", Color.DarkGreen, codeTextBox);
            CheckKeyword("print", Color.HotPink, codeTextBox);

            CheckKeyword(">=", Color.Gray, codeTextBox);
            CheckKeyword("<=", Color.Gray, codeTextBox);
            CheckKeyword(">", Color.Gray, codeTextBox);
            CheckKeyword("<", Color.Gray, codeTextBox);
            CheckKeyword("-", Color.Gray, codeTextBox);
            CheckKeyword("!=", Color.Gray, codeTextBox);
            CheckKeyword("==", Color.Gray, codeTextBox);
            CheckKeyword("=", Color.Gray, codeTextBox);
            CheckKeyword(":", Color.Gray, codeTextBox);
            CheckKeyword(new Regex("\\+"), Color.Gray, codeTextBox);
            CheckKeyword(new Regex("\\/"), Color.Gray, codeTextBox);
            CheckKeyword(new Regex("\\*"), Color.Gray, codeTextBox);
            CheckKeyword(new Regex("\\%"), Color.Gray, codeTextBox);
            CheckKeyword(new Regex("\\("), Color.Gray, codeTextBox);
            CheckKeyword(new Regex("\\)"), Color.Gray, codeTextBox);
            CheckKeyword(new Regex("\\."), Color.Gray, codeTextBox);
            CheckKeyword(new Regex("\\["), Color.Gray, codeTextBox);
            CheckKeyword(new Regex("\\]"), Color.Gray, codeTextBox);

            CheckKeyword(new Regex("^*--.*"), Color.Gray, codeTextBox);

            CheckKeyword(new Regex("true|false"), Color.HotPink, codeTextBox);
            CheckKeyword(new Regex("[0-9][0-9]*"), Color.DarkOliveGreen, codeTextBox);
            CheckKeyword(new Regex("\\d+\\.\\d+"), Color.DarkOliveGreen, codeTextBox);
            CheckKeyword(new Regex("\"[^\"]*\""), Color.Orange, codeTextBox);

            if (originalCaretPosition < codeTextBox.TextLength)
            {
                codeTextBox.SelectionStart = originalCaretPosition;
                codeTextBox.SelectionLength = 0;
            }

            this.codeTextBox.ResumeLayout();
            this.codeTextBox.Visible = true;
            this.codeTextBox.Focus();
            
            _hasChanged = false;
        }

        /// <summary>
        /// Change the color of a word based on regex in the text
        /// </summary>
        /// <param name="regex"></param>
        /// <param name="color"></param>
        private void CheckKeyword(Regex regex, Color color, RichTextBox textbox)
        {
            var matches = regex.Matches(textbox.Text).ToList();

            foreach (var match in matches)
            {
                int index = match.Index;
                int size = match.Length;
                int selectStart = textbox.SelectionStart;

                textbox.Select(index, size);
                textbox.SelectionColor = color;
                textbox.Select(selectStart, 0);
                textbox.SelectionColor = Color.Black;
            }
        }

        /// <summary>
        /// Insere destaque na linha
        /// </summary>
        /// <param name="line"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="color"></param>
        private void HighlightLine(int line, int start, int end, Color color)
        {
            if (_disableHighlighting)
                return;

            if (line < 0)
                return;

            this.codeTextBox.SuspendLayout();

            int originalSelectionStart = codeTextBox.SelectionStart;
            int originalSelectionLength = codeTextBox.SelectionLength;

            int lineStart = codeTextBox.GetFirstCharIndexFromLine(line);
            int lineEnd = lineStart + codeTextBox.Lines[line].Length;

            int selectionStart = Math.Max(lineStart + start, lineStart);
            int selectionEnd = Math.Min(lineStart + end, lineEnd);

            codeTextBox.SelectionStart = selectionStart;
            codeTextBox.SelectionLength = selectionEnd - selectionStart;
            codeTextBox.SelectionBackColor = color;

            codeTextBox.SelectionStart = originalSelectionStart;
            codeTextBox.SelectionLength = originalSelectionLength;

            this.codeTextBox.ResumeLayout();
        }

        /// <summary>
        /// Remove destaques do código
        /// </summary>
        private void RemoveHighlight()
        {
            if (_disableHighlighting)
                return;

            this.codeTextBox.SuspendLayout();

            int originalSelectionStart = codeTextBox.SelectionStart;
            int originalSelectionLength = codeTextBox.SelectionLength;

            codeTextBox.SelectionStart = 0;
            codeTextBox.SelectionLength = codeTextBox.Text.Length;

            codeTextBox.SelectionBackColor = Color.Transparent;
            codeTextBox.SelectionLength = 0;

            codeTextBox.SelectionStart = originalSelectionStart;
            codeTextBox.SelectionLength = originalSelectionLength;

            this.codeTextBox.ResumeLayout();
        }

        /// <summary>
        /// Change the color of a word in the text
        /// </summary>
        /// <param name="word"></param>
        /// <param name="color"></param>
        private void CheckKeyword(string word, Color color, RichTextBox textbox)
        {
            if (textbox.Text.Contains(word))
            {
                int index = -1;
                int selectStart = textbox.SelectionStart;

                while ((index = textbox.Text.IndexOf(word, (index + 1))) != -1)
                {
                    textbox.Select(index, word.Length);
                    textbox.SelectionColor = color;
                    textbox.Select(selectStart, 0);
                    textbox.SelectionColor = Color.Black;
                }
            }
        }

        /// <summary>
        /// Put colors on syntax
        /// </summary>
        private void BeautifyGenerated()
        {
            if (_disableHighlighting)
                return;

            this.textBoxGenerated.Visible = false;
            this.textBoxGenerated.SuspendLayout();

            CheckKeyword("add", Color.Green, textBoxGenerated);
            CheckKeyword("addf", Color.Green, textBoxGenerated);
            CheckKeyword("addi", Color.Green, textBoxGenerated);
            CheckKeyword("addfi", Color.Green, textBoxGenerated);
            CheckKeyword("sub", Color.Green, textBoxGenerated);
            CheckKeyword("subf", Color.Green, textBoxGenerated);
            CheckKeyword("mul", Color.Green, textBoxGenerated);
            CheckKeyword("mulf", Color.Green, textBoxGenerated);
            CheckKeyword("muli", Color.Green, textBoxGenerated);
            CheckKeyword("mulfi", Color.Green, textBoxGenerated);
            CheckKeyword("div", Color.Green, textBoxGenerated);
            CheckKeyword("divf", Color.Green, textBoxGenerated);

            CheckKeyword("or", Color.Green, textBoxGenerated);
            CheckKeyword("xor", Color.Green, textBoxGenerated);
            CheckKeyword("and", Color.Green, textBoxGenerated);
            CheckKeyword("not", Color.Green, textBoxGenerated);
            CheckKeyword("sfl", Color.Green, textBoxGenerated);
            CheckKeyword("sfr", Color.Green, textBoxGenerated);

            CheckKeyword("jr", Color.Green, textBoxGenerated);
            CheckKeyword("j", Color.Green, textBoxGenerated);
            CheckKeyword("beq", Color.Green, textBoxGenerated);
            CheckKeyword("bne", Color.Green, textBoxGenerated);
            CheckKeyword("jal", Color.Green, textBoxGenerated);

            CheckKeyword("se", Color.Green, textBoxGenerated);
            CheckKeyword("sne", Color.Green, textBoxGenerated);
            CheckKeyword("slt", Color.Green, textBoxGenerated);
            CheckKeyword("sgt", Color.Green, textBoxGenerated);
            CheckKeyword("slte", Color.Green, textBoxGenerated);
            CheckKeyword("sgte", Color.Green, textBoxGenerated);
            CheckKeyword("sltf", Color.Green, textBoxGenerated);
            CheckKeyword("sgtf", Color.Green, textBoxGenerated);
            CheckKeyword("sltof", Color.Green, textBoxGenerated);
            CheckKeyword("sgtef", Color.Green, textBoxGenerated);
            CheckKeyword("slt", Color.Green, textBoxGenerated);
            CheckKeyword("sgt", Color.Green, textBoxGenerated);

            CheckKeyword("lbr", Color.Green, textBoxGenerated);
            CheckKeyword("lcr", Color.Green, textBoxGenerated);
            CheckKeyword("lir", Color.Green, textBoxGenerated);
            CheckKeyword("lfr", Color.Green, textBoxGenerated);
            CheckKeyword("cfi", Color.Green, textBoxGenerated);
            CheckKeyword("cif", Color.Green, textBoxGenerated);

            CheckKeyword("lw", Color.Green, textBoxGenerated);
            CheckKeyword("sw", Color.Green, textBoxGenerated);
            CheckKeyword("sb", Color.Green, textBoxGenerated);
            CheckKeyword("lb", Color.Green, textBoxGenerated);
            CheckKeyword("move", Color.Green, textBoxGenerated);

            CheckKeyword("syscall", Color.HotPink, textBoxGenerated);

            foreach (var register in new Emulator(null, null, null).GetRegisters())
                CheckKeyword(register.Name, Color.Blue, textBoxGenerated);

            CheckKeyword(new Regex(".*--.*"), Color.DarkGreen, textBoxGenerated);
            CheckKeyword(":", Color.HotPink, textBoxGenerated);

            CheckKeyword(new Regex("\\s(\\d+)"), Color.Orange, textBoxGenerated);
            CheckKeyword(new Regex("\\s(\\d+\\.\\d+)"), Color.Orange, textBoxGenerated);

            CheckKeyword(new Regex("\\s(0x[0-9A-Fa-f]+)"), Color.HotPink, textBoxGenerated);
            CheckKeyword(new Regex("'(.)'"), Color.Brown, textBoxGenerated);

            this.textBoxGenerated.ResumeLayout();
            this.textBoxGenerated.Visible = true;
        }

        private void buttonDisableHighlight_Click(object sender, EventArgs e)
        {
            this._disableHighlighting = !this._disableHighlighting;

            if (this._disableHighlighting)
            {
                this.buttonDisableHighlight.Text = "Enable Highlight";
            }
            else
            {
                this.buttonDisableHighlight.Text = "Disable Highlight";
            }
        }

        #endregion
    }
}