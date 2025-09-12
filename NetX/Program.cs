using System.Diagnostics;

internal class Program
{
    private static void Main(string[] args)
    {
        string file_to_execute = args[0];
        string cmd = "cmd.exe";
        string arguments = $"$/c dotnet run {file_to_execute}";   // "/c" runs and then terminates

        ProcessStartInfo psi = new ProcessStartInfo
        {
            FileName = cmd,
            Arguments = arguments,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };

        using (Process process = new Process { StartInfo = psi })
        {
            process.Start();

            string output = process.StandardOutput.ReadToEnd();
            string error = process.StandardError.ReadToEnd();

            process.WaitForExit();

            print(output);



            if (!string.IsNullOrWhiteSpace(error))
            {
                print_error(error);
            }
        }
    }

    public static void print(string text)
    {
        Console.WriteLine(text);
    }

    public static void print_error(string text)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine(text);
        Console.ResetColor();
    }
}