using System.Diagnostics;

public static class netx
{
    public static string current_directory
    {
        get
        {
            return Environment.CurrentDirectory;
        }
    }

    public static void pwd()
    {
        Console.WriteLine(Environment.CurrentDirectory);
    }

    public static void cd(string path)
    {
        var target_path = Path.Combine(Environment.CurrentDirectory, path);
        Environment.CurrentDirectory = target_path;
    }


    public static void print(string text)
    {
        Console.WriteLine(text);
    }

    public static void run(string executable_location)
    {
        Process.Start(executable_location);
    }

    public static void list()
    {
        Console.WriteLine($"");
        var d = Directory.GetCurrentDirectory();
        DirectoryInfo di = new DirectoryInfo(d);
        foreach (var item in di.GetDirectories())
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(item.Name);
            Console.ResetColor();
        }
        foreach (var item in di.GetFiles())
        {
            Console.WriteLine($"./{item.Name}");
        }
        Console.WriteLine($"");
    }

    public static void copy(string in_file, string folder = "", bool overwrite = false)
    {
        if (!File.Exists(in_file))
        {
            Console.ForegroundColor = ConsoleColor.Red;
            var full_path = Path.Combine(Environment.CurrentDirectory, in_file);
            var file_info = new FileInfo(full_path);

            var directory_name = file_info.Directory;

            Console.WriteLine($"File [{file_info.Name}] does not exist in [{directory_name}]");
            Console.ResetColor();
            return;
        }

        var target_folder = folder;
        if (string.IsNullOrEmpty(target_folder))
        {
            target_folder = Environment.CurrentDirectory;
        }

        var target_file = Path.GetFileName(in_file);
        var target_path = Path.Combine(target_folder, target_file);

        File.Copy(in_file, target_path, overwrite);
    }
}

