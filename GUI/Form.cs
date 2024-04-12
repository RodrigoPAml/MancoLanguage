using Language.Common.Exceptions;
using Manco;
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
        /// The manco language provider
        /// </summary>
        private MancoProvider _provider = new MancoProvider();

        public Form()
        {
            InitializeComponent();

            WindowState = FormWindowState.Maximized;

            _timerBeautify = new Timer();
            _timerBeautify.Interval = 3000;
            _timerBeautify.Tick += this.Beautify;
            _timerBeautify.Start();

            _timerCodeVerify = new Timer();
            _timerCodeVerify.Interval = 500;
            _timerCodeVerify.Tick += this.VerifyCode;
            _timerCodeVerify.Start();
        }

        private void ButtonRun_Click(object sender, EventArgs e)
        {
            // TODO
        }

        /// <summary>
        /// Verify code
        /// </summary>
        private void VerifyCode(object? sender, EventArgs e)
        {
            if (_hasCodeError)
                return;

            textBoxGenerated.Text = "";

            try
            {
                listBoxOutput.Items.Clear();

                _provider.SetCode(codeTextBox.Text);
                textBoxGenerated.Text = string.Join("\n", _provider.Compile().Select(x => x));
            }
            catch (BaseException bex)
            {
                if (bex.Token != null)
                    HighlightLine(bex.Token.Line-1, bex.Token.Start, bex.Token.End, Color.Red);

                listBoxOutput.Items.Add($"Message: {bex.Message}, Token: {bex.Token}, ErrorType: {bex.Code}");
                _hasCodeError = true;
            }
            catch (Exception ex)
            {
                listBoxOutput.Items.Add($"Erro Fatal: {ex.Message}");
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
            _hasChanged = true;

            if(_hasCodeError)
                RemoveHighlight();

            _hasCodeError = false;
        }

        /// <summary>
        /// Put colors on syntax
        /// </summary>
        private void Beautify(object? sender, EventArgs e)
        {
            if (!_hasChanged)
                return;

            this.codeTextBox.SuspendLayout();

            CheckKeyword(new Regex("\"[^\"]*\""), Color.Orange);

            CheckKeyword("function", Color.DarkBlue);
            CheckKeyword("end", Color.DarkBlue);
            CheckKeyword("while", Color.DarkBlue);
            CheckKeyword("OR", Color.DarkBlue);
            CheckKeyword("AND", Color.DarkBlue);
            CheckKeyword("break", Color.DarkBlue);
            CheckKeyword("continue", Color.DarkBlue);
            CheckKeyword("break", Color.DarkBlue);
            CheckKeyword("if", Color.DarkBlue);
            CheckKeyword("elif", Color.DarkBlue);
            CheckKeyword("else", Color.DarkBlue);

            CheckKeyword("integer", Color.DarkGreen);
            CheckKeyword("decimal", Color.DarkGreen);
            CheckKeyword("bool", Color.DarkGreen);
            CheckKeyword("string", Color.DarkGreen);
            CheckKeyword("integer&", Color.DarkGreen);
            CheckKeyword("decimal&", Color.DarkGreen);
            CheckKeyword("bool&", Color.DarkGreen);
            CheckKeyword("print", Color.HotPink);

            CheckKeyword(">=", Color.Gray);
            CheckKeyword("<=", Color.Gray);
            CheckKeyword(">", Color.Gray);
            CheckKeyword("<", Color.Gray);
            CheckKeyword("-", Color.Gray);
            CheckKeyword("!=", Color.Gray);
            CheckKeyword("==", Color.Gray);
            CheckKeyword("=", Color.Gray);
            CheckKeyword(":", Color.Gray);
            CheckKeyword(new Regex("\\+"), Color.Gray);
            CheckKeyword(new Regex("\\/"), Color.Gray);
            CheckKeyword(new Regex("\\*"), Color.Gray);
            CheckKeyword(new Regex("\\%"), Color.Gray);
            CheckKeyword(new Regex("\\("), Color.Gray);
            CheckKeyword(new Regex("\\)"), Color.Gray);
            CheckKeyword(new Regex("\\."), Color.Gray);
            CheckKeyword(new Regex("\\["), Color.Gray);
            CheckKeyword(new Regex("\\]"), Color.Gray);

            CheckKeyword(new Regex("^*--.*"), Color.Gray);

            CheckKeyword(new Regex("true|false"), Color.HotPink);
            CheckKeyword(new Regex("[0-9][0-9]*"), Color.DarkOliveGreen);
            CheckKeyword(new Regex("\\d+\\.\\d+"), Color.DarkOliveGreen);
            CheckKeyword(new Regex("\"[^\"]*\""), Color.Orange);

            this.codeTextBox.ResumeLayout();

            _hasChanged = false;
        }

        /// <summary>
        /// Change the color of a word based on regex in the text
        /// </summary>
        /// <param name="regex"></param>
        /// <param name="color"></param>
        private void CheckKeyword(Regex regex, Color color)
        {
            var matches = regex.Matches(this.codeTextBox.Text).ToList();

            foreach (var match in matches)
            {
                int index = match.Index;
                int size = match.Length;
                int selectStart = this.codeTextBox.SelectionStart;

                this.codeTextBox.Select(index, size);
                this.codeTextBox.SelectionColor = color;
                this.codeTextBox.Select(selectStart, 0);
                this.codeTextBox.SelectionColor = Color.Black;
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
        private void CheckKeyword(string word, Color color)
        {
            if (this.codeTextBox.Text.Contains(word))
            {
                int index = -1;
                int selectStart = this.codeTextBox.SelectionStart;

                while ((index = this.codeTextBox.Text.IndexOf(word, (index + 1))) != -1)
                {
                    this.codeTextBox.Select(index, word.Length);
                    this.codeTextBox.SelectionColor = color;
                    this.codeTextBox.Select(selectStart, 0);
                    this.codeTextBox.SelectionColor = Color.Black;
                }
            }
        }
    }
}