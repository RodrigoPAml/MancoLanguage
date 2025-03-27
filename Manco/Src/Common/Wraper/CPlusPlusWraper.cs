using System.Diagnostics;

namespace Manco.Common.Wrapper
{
    /// <summary>
    /// Wraper around G++ to compile and invoke programs
    /// Make sure g++ is installed and in the PATH
    /// </summary>
    public static class CPlusPlusWraper
    {
        public static CPlusPlusResult Compile(string content)
        {
            try
            {
                File.WriteAllText("program.cpp", content);

                var command = $"g++ -O2 program.cpp -o program";

                var processStartInfo = new ProcessStartInfo()
                {
                    FileName = "cmd.exe",
                    Arguments = $"/C {command}",
                    RedirectStandardOutput = true, 
                    RedirectStandardError = true, 
                    UseShellExecute = false,
                    CreateNoWindow = true
                };

                var process = Process.Start(processStartInfo);

                var output = process.StandardOutput.ReadToEnd();
                var errors = process.StandardError.ReadToEnd();

                process.WaitForExit();

                return new()
                {
                    Error = process.ExitCode != 0,
                    Output = output + errors
                };
            }
            catch (Exception ex)
            {
                return new()
                {
                    Error = true,
                    Output = ex.Message
                };
            }
        }

        public static CPlusPlusResult Execute()
        {
            try
            {
                var command = $"program.exe";

                var processStartInfo = new ProcessStartInfo()
                {
                    FileName = "cmd.exe",
                    Arguments = $"/C {command}",
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };

                var process = Process.Start(processStartInfo);

                var output = process.StandardOutput.ReadToEnd();
                var errors = process.StandardError.ReadToEnd();

                process.WaitForExit();

                return new()
                {
                    Error = process.ExitCode != 0,
                    Output = output + errors
                };
            }
            catch (Exception ex)
            {
                return new()
                {
                    Error = true,
                    Output = ex.Message
                };
            }
        }

        public class CPlusPlusResult
        {
            public string Output { get; set; }
            public bool Error { get; set; }
        }
    }
}
