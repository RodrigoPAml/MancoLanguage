using AssemblerSimulator;
using System.Text.RegularExpressions;
using Manco.Common.Enums;

namespace GUI
{
    partial class Form
    {
        // Funções relacioandas a colorização da UI abaixo
        #region ColorHighlight

        /// <summary>
        /// Faz verificação de erros do código visualmente e highlight de sintaxe
        /// </summary>
        private void VerifyAndHighligth(object sender = null, EventArgs e = null)
        {
            if (_disableHighlighting)
                return;

            if (!_hasChanged)
                return;

            if ((DateTime.Now - _lastTyped).TotalSeconds < 1)
                return;

            _hasChanged = false;

            this.codeTextBox.TextChanged -= this.richTextBoxCode_TextChanged!;

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

            this.VerifyCode();
            this.codeTextBox.TextChanged += this.richTextBoxCode_TextChanged!;
        }

        /// <summary>
        /// Muda a cor do texto baseado no regex
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
        /// Faz syntax highlighting do assembly
        /// </summary>
        private void BeautifyGenerated()
        {
            if (_disableHighlighting)
                return;

            this.textBoxGenerated.Visible = false;
            this.textBoxGenerated.SuspendLayout();

            var type = (TransformerType)comboBoxTransformer.SelectedIndex;

            if (type == TransformerType.CompiledMIPS)
                MipsBeautify();
            else
                CppBeautify();

            this.textBoxGenerated.ResumeLayout();
            this.textBoxGenerated.Visible = true;
        }

        private void MipsBeautify()
        {
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
        }

        private void CppBeautify()
        {
            // C++ Keywords
            CheckKeyword("int", Color.Green, textBoxGenerated);
            CheckKeyword("float", Color.Green, textBoxGenerated);
            CheckKeyword("bool", Color.Green, textBoxGenerated);
            CheckKeyword("void", Color.Green, textBoxGenerated);
            CheckKeyword("if", Color.Green, textBoxGenerated);
            CheckKeyword("else", Color.Green, textBoxGenerated);
            CheckKeyword("while", Color.Green, textBoxGenerated);
            CheckKeyword("break", Color.Green, textBoxGenerated);
            CheckKeyword("continue", Color.Green, textBoxGenerated);
            CheckKeyword("return", Color.Green, textBoxGenerated);
            CheckKeyword("false", Color.Blue, textBoxGenerated);
            CheckKeyword("true", Color.Blue, textBoxGenerated);

            // C++ Operators
            CheckKeyword("\\+", Color.Green, textBoxGenerated); // Addition operator
            CheckKeyword("-", Color.Green, textBoxGenerated); // Subtraction operator
            CheckKeyword("\\*", Color.Green, textBoxGenerated); // Multiplication operator
            CheckKeyword("/", Color.Green, textBoxGenerated); // Division operator
            CheckKeyword("%", Color.Green, textBoxGenerated); // Modulo operator
            CheckKeyword("=", Color.Green, textBoxGenerated); // Assignment operator
            CheckKeyword("==", Color.Green, textBoxGenerated); // Equality operator
            CheckKeyword("!=", Color.Green, textBoxGenerated); // Inequality operator
            CheckKeyword("<", Color.Green, textBoxGenerated); // Less than operator
            CheckKeyword(">", Color.Green, textBoxGenerated); // Greater than operator
            CheckKeyword("<=", Color.Green, textBoxGenerated); // Less than or equal operator
            CheckKeyword(">=", Color.Green, textBoxGenerated); // Greater than or equal operator
            CheckKeyword("&&", Color.Green, textBoxGenerated); // Logical AND
            CheckKeyword("\\|\\|", Color.Green, textBoxGenerated); // Logical OR
            CheckKeyword("!", Color.Green, textBoxGenerated); // Logical NOT
            CheckKeyword("&", Color.Green, textBoxGenerated); // Address-of operator
            CheckKeyword("\\*", Color.Green, textBoxGenerated); // Pointer dereference operator

            // C++ Literals
            CheckKeyword(new Regex("\\s(\\d+)"), Color.Orange, textBoxGenerated); // Integers
            CheckKeyword(new Regex("\\s(\\d+\\.\\d+)"), Color.Orange, textBoxGenerated); // Floating point numbers
            CheckKeyword(new Regex("\\s(0x[0-9A-Fa-f]+)"), Color.HotPink, textBoxGenerated); // Hexadecimal literals
            CheckKeyword(new Regex("\"[^\"]*\""), Color.Brown, textBoxGenerated); // String literals
            CheckKeyword(new Regex("'[^\']'"), Color.Brown, textBoxGenerated); // Character literals

            // C++ Comments
            CheckKeyword(new Regex("//.*"), Color.Gray, textBoxGenerated); // Single-line comments
            CheckKeyword(new Regex("/\\*.*?\\*/", RegexOptions.Singleline), Color.Gray, textBoxGenerated); // Multi-line comments

            // C++ Preprocessor Directives
            CheckKeyword("#include", Color.HotPink, textBoxGenerated);
        }

        /// <summary>
        /// Disabilita syntax highlighting
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonDisableHighlight_Click(object sender, EventArgs e)
        {
            _disableHighlighting = !_disableHighlighting;

            if (_disableHighlighting)
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