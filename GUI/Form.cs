using System.Text;
using Manco;
using AssemblerSimulator;
using Manco.Common.Enums;
using Manco.Common.Wrapper;
using Manco.Common.Exceptions;
using Timer = System.Windows.Forms.Timer;
using WinForm = System.Windows.Forms.Form;

namespace GUI
{
    public partial class Form : WinForm
    {
        /// <summary>
        /// Timer para eventos do editor para verificar código e highlight visual
        /// </summary>
        private Timer _timerVerify = new Timer();

        /// <summary>
        /// Guarda ultima vez que algo foi digitado
        /// </summary>
        private DateTime _lastTyped = DateTime.Now;

        /// <summary>
        /// Arquivo que foi aberto
        /// </summary>
        private string _openFile = string.Empty;

        /// <summary>
        /// If text has changed
        /// </summary>
        private bool _hasChanged = false;

        /// <summary>
        /// Se o código possui erros
        /// </summary>
        private bool _hasCodeError = false;

        /// <summary>
        /// Para desabilitar text highlighting
        /// </summary>
        private bool _disableHighlighting = false;

        /// <summary>
        /// Referencia para formulario
        /// </summary>
        private static Form _form = null;

        /// <summary>
        /// Provedor da linguagem manco
        /// </summary>
        private MancoProvider _provider = new MancoProvider();

        public Form()
        {
            InitializeComponent();

            _form = this;
            WindowState = FormWindowState.Maximized;

            _timerVerify = new Timer();
            _timerVerify.Interval = 2000;
            _timerVerify.Tick += this.VerifyAndHighligth;
            _timerVerify.Start();

            comboBoxTransformer.SelectedIndex = 0;
        }

        /// <summary>
        /// Botão para evento de compilação
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCompile_Click(object sender, EventArgs e)
        {
            textBoxGenerated.Text = string.Empty;
            this.codeTextBox.TextChanged -= this.richTextBoxCode_TextChanged!;

            try
            {
                // Limpa saída
                listBoxOutput.Items.Clear();

                // Insere código
                _provider.SetCode(string.Join('\n', codeTextBox.Text));

                var now = DateTime.Now;
                var type = (TransformerType)comboBoxTransformer.SelectedIndex;

                // Compila/transpila código
                var lines = _provider
                        .Transform(type)
                        .Select(x => x)
                        .ToList();

                textBoxGenerated.Text = string.Join('\n', lines);
                listBoxOutput.Items.Clear();

                if (type == TransformerType.TranspiledCPlusPlus)
                {
                    var result = CPlusPlusWraper.Compile(textBoxGenerated.Text);

                    if(!string.IsNullOrEmpty(result.Output))
                        listBoxOutput.Items.Add($"g++ compilation output: {result.Output}");

                    listBoxOutput.Items.Add($"Code compiled with g++ in {(DateTime.Now - now).TotalMilliseconds} ms");
                }
                else
                {
                    listBoxOutput.Items.Add($"Code compiled with mips in {(DateTime.Now - now).TotalMilliseconds} ms");
                }

                RemoveHighlight();
                BeautifyGenerated();

                _hasCodeError = false;
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
            finally
            {
                this.codeTextBox.TextChanged += this.richTextBoxCode_TextChanged!;
            }
        }

        /// <summary>
        /// Botão para execução do código
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonRun_Click(object sender, EventArgs e)
        {
            if (_hasCodeError)
                return;


            if(string.IsNullOrEmpty(textBoxGenerated.Text))
            {
                MessageBox.Show("Code to execute is empty");
                return;
            }

            listBoxOutput.Items.Clear();
            listBoxCodeOutput.Clear();

            _form.listBoxOutput.Items.Add("Executing program");

            var type = (TransformerType)comboBoxTransformer.SelectedIndex;
            var now = DateTime.Now;
            var elapsed = 0.0;

            if (type == TransformerType.TranspiledCPlusPlus)
            {
                var result = CPlusPlusWraper.Execute();
                elapsed = (DateTime.Now - now).TotalMilliseconds;

                _form.listBoxCodeOutput.AppendText(result.Output);
            }
            else
            {
                var emulator = new Emulator(null, null, OnSyscall);
                emulator.AddInstructions(textBoxGenerated.Text.Split('\n').ToList());
                emulator.PostergateCallbacks = true;

                try
                {
                    emulator.ExecuteAll();
                    elapsed = (DateTime.Now - now).TotalMilliseconds;

                    emulator.InvokeCallbacks();
                    _form.listBoxCodeOutput.AppendText(Environment.NewLine + "Program exited 0");
                }
                catch (Exception ex)
                {
                    elapsed = (DateTime.Now - now).TotalMilliseconds;
                    listBoxCodeOutput.AppendText(Environment.NewLine + $"Program exited 1: {ex.Message}");
                }
            }

            listBoxOutput.Items.Add($"Executed in {elapsed} ms (aprox)");
        }

        /// <summary>
        /// Syscalls sendo inseridas na UI
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
        /// Verifica se código esta com erro e atualiza UI
        /// </summary>
        private void VerifyCode()
        {
            try
            {
                _provider.SetCode(string.Join('\n', codeTextBox.Text));
                _provider.Validate();

                RemoveHighlight();
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
        /// Carrega código
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
                _hasChanged = true;

                VerifyAndHighligth();
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

            PutTokens();
        }

        /// <summary>
        /// Put tokens in the textbox
        /// </summary>
        private void PutTokens()
        {
            try
            {
                _provider.SetCode(string.Join('\n', codeTextBox.Text));
                textBoxTokens.Text = _provider.GetTokensToString();
            }
            catch
            {
                textBoxTokens.Text = string.Empty;
            }
        }

        /// <summary>
        /// Muda o tipo de transformação (mips ou c++)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void comboBoxTransformer_SelectedIndexChanged(object sender, EventArgs e)
        {
            textBoxGenerated.Text = string.Empty;
        }
    }
}