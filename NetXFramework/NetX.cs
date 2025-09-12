using Microsoft.VisualBasic;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Net.Mime;
using System.Xml;


public static class netx
{
    static ReadOnlyDictionary<string, string> _environment_variables;

    /// <summary>
    /// Gets a string based name/value dictionary of all the enviroment variables
    /// in scope.
    /// </summary>
    public static ReadOnlyDictionary<string, string> environment_variables
    {
        get
        {
            if (_environment_variables == null)
            {
                var vars = Environment.GetEnvironmentVariables();
                var dictionary = new Dictionary<string, string>();
                foreach (var key in vars.Keys)
                {
                    dictionary.Add((string)key, (string)vars[key]);
                }
                _environment_variables = new ReadOnlyDictionary<string, string>(dictionary);
            }
            return _environment_variables;
        }
    }


    #region [Console]
    /// <summary>
    /// Gets the current directory as a string value
    /// </summary>
    public static string current_directory
    {
        get
        {
            return Environment.CurrentDirectory;
        }
    }

    /// <summary>
    /// Gets the command line that was passed into the script when it was run
    /// </summary>
    public static string[] commandline
    {
        get
        {
            var list = new List<string>(Environment.GetCommandLineArgs());
            list.RemoveAt(0);
            return list.ToArray();
        }
    }

    /// <summary>
    /// Prompts the user to provide some input
    /// </summary>
    /// <param name="prompt">Message for the user</param>
    /// <returns>Input from the user</returns>
    public static string prompt(string prompt = "")
    {
        Console.BackgroundColor = ConsoleColor.Green;
        Console.ForegroundColor = ConsoleColor.White;
        if (!string.IsNullOrEmpty(prompt))
            Console.Write(prompt + ":");

        Console.ResetColor();
        Console.Write(" ");
        var response = Console.ReadLine();
        return response;
    }

    /// <summary>
    /// Lists the contents of the directory (like "ls" or "dir")
    /// </summary>
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

    /// <summary>
    /// Prints the current working directory
    /// </summary>
    public static void pwd()
    {
        Console.WriteLine(Environment.CurrentDirectory);
    }

    /// <summary>
    /// Changes the current working directory
    /// </summary>
    /// <param name="path">Relative path (from the current directory) to the new directory</param>
    public static void cd(string path)
    {
        var target_path = Path.Combine(Environment.CurrentDirectory, path);
        Environment.CurrentDirectory = target_path;
    }

    /// <summary>
    /// Prints text to the console (similar to echo)
    /// </summary>
    /// <param name="text">Text to print out</param>
    /// <param name="foreground">Foreground color of the text to print out</param>
    /// <param name="background">Background color of the text to print out</param>
    public static void print(string text, string foreground = null, string background = null)
    {
        if (!string.IsNullOrEmpty(foreground))
        {
            try
            {
                var foreground_color = (ConsoleColor)Enum.Parse(typeof(ConsoleColor), foreground, true);
                Console.ForegroundColor = foreground_color;
            }
            catch
            {
            }
        }

        if (!string.IsNullOrEmpty(background))
        {
            try
            {
                var background_color = (ConsoleColor)Enum.Parse(typeof(ConsoleColor), background, true);
                Console.BackgroundColor = background_color;
            }
            catch
            {
            }
        }

        Console.WriteLine(text);
        Console.ResetColor();
    }


    internal static void print_error(string text)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine(text);
        Console.ResetColor();
    }

    /// <summary>
    /// Runs an executable.  The file must be in the current folder or in the global 
    /// path
    /// </summary>
    /// <param name="executable_location">File name of the executable (can include path)</param>
    public static void run(string executable_location)
    {
        Process.Start(executable_location);
    }

    /// <summary>
    /// Executes a command in the console (as though it was run directly on the console itself)
    /// </summary>
    /// <param name="command">The command to run (like "dir" or "ls" or anything you could execute on the console)</param>
    public static void exec(string command)
    {
        string cmd = "cmd.exe";
        string args = $"$/c {command}";   // "/c" runs and then terminates

        ProcessStartInfo psi = new ProcessStartInfo
        {
            FileName = cmd,
            Arguments = args,
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
    #endregion

    #region [IO]


    /// <summary>
    /// Renames a file or folder
    /// </summary>
    /// <param name="old_name">Old name of the file or folder</param>
    /// <param name="new_name">New name of the file or folder</param>
    public static void rename(string old_name, string new_name)
    {
        copy(old_name, ".", new_name);
        delete(old_name);
    }

    /// <summary>
    /// Copies a file or folder.  If there is a file/folder with the same name in the destination it
    /// will be overriden
    /// </summary>
    /// <param name="source">Source file or folder</param>
    /// <param name="destination">Destination file or folder</param>
    /// <param name="copied_filename">New name of the file or folder at the destination</param>
    public static void copy(string source, string destination = null, string copied_filename = null)
    {
        bool overwrite = true;
        FileAttributes attr = File.GetAttributes(source);
        if (attr.HasFlag(FileAttributes.Directory))
        {
            if (Directory.Exists(destination))
            {
                destination = $"{destination}-copied-{Guid.NewGuid()}";
            }

            CopyDirectory(source, destination);
            void CopyDirectory(string sourceDir, string destinationDir)
            {
                // Create target directory if it doesn’t exist
                Directory.CreateDirectory(destinationDir);

                // Copy all files
                foreach (string filePath in Directory.GetFiles(sourceDir))
                {
                    string fileName = Path.GetFileName(filePath);
                    string destFile = Path.Combine(destinationDir, fileName);
                    File.Copy(filePath, destFile, overwrite: true);
                }

                // Copy all subdirectories recursively
                foreach (string subDir in Directory.GetDirectories(sourceDir))
                {
                    string subDirName = Path.GetFileName(subDir);
                    string destSubDir = Path.Combine(destinationDir, subDirName);
                    CopyDirectory(subDir, destSubDir);
                }
            }
        }
        else
        {
            if (!File.Exists(source))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                var full_path = Path.Combine(Environment.CurrentDirectory, source);
                var file_info = new FileInfo(full_path);

                var directory_name = file_info.Directory;

                Console.WriteLine($"File [{file_info.Name}] does not exist in [{directory_name}]");
                Console.ResetColor();
                return;
            }

            var target_folder = destination;
            if (string.IsNullOrEmpty(target_folder))
            {
                target_folder = Environment.CurrentDirectory;
            }

            var target_file = Path.GetFileName(source);
            if (!string.IsNullOrEmpty(copied_filename))
            {
                target_file = copied_filename;
            }
            var target_path = Path.Combine(target_folder, target_file);

            File.Copy(source, target_path, overwrite);
        }
    }

    /// <summary>
    /// Zips a folder
    /// </summary>
    /// <param name="target_folder">Folder to zip</param>
    /// <param name="output_file">Name of the output</param>
    public static void pack(string target_folder, string output_file)
    {
        if (File.Exists(output_file))
        {
            File.Delete(output_file);
        }

        ZipFile.CreateFromDirectory(target_folder, output_file);
    }

    /// <summary>
    /// Unzips a zip file into a folder
    /// </summary>
    /// <param name="zip_file">Name of the zip file</param>
    /// <param name="destination_folder">Destination folder where it will be unpacked</param>
    public static void unpack(string zip_file, string destination_folder)
    {
        ZipFile.ExtractToDirectory(zip_file, destination_folder);
    }

    /// <summary>
    /// Deletes a file or folder.  Folders are recursively deleted.
    /// </summary>
    /// <param name="folder_item_path">Path of the folder</param>
    public static void delete(string folder_item_path)
    {
        bool recursive = true;
        FileAttributes attr = File.GetAttributes(folder_item_path);
        if (attr.HasFlag(FileAttributes.Directory))
        {
            Directory.Delete(folder_item_path, recursive);
        }
        else
        {
            File.Delete(folder_item_path);
        }

    }
    #endregion

    #region [Services]
    /// <summary>
    /// Makes an HTTP GET call to the specified endpiont
    /// </summary>
    /// <param name="url">URL to call</param>
    /// <returns></returns>
    public static string http_get(string url)
    {
        string retval = "";
        HttpClient client = new HttpClient();
        Task.Run(async () =>
        {
            var ret = await client.GetAsync(url);
            if (ret.StatusCode == System.Net.HttpStatusCode.OK)
            {
                retval = await ret.Content.ReadAsStringAsync();
            }
            else
            {
                print_error($"HTTP GET call failed for URL [{url}]");
            }
        }).Wait();
        return retval;
    }

    /// <summary>
    /// Makes a HTTP POST call to the specified endpoint
    /// </summary>
    /// <param name="url">Url to call</param>
    /// <param name="json_object">Object that represents json object to be posted</param>
    /// <returns></returns>
    public static string http_post(string url, object json_object)
    {
        string retval = "";
        HttpClient client = new HttpClient();
        Task.Run(async () =>
        {
            var json_string = JsonConvert.SerializeObject(json_object);
            var content = new StringContent(json_string);
            var ret = await client.PostAsync(url, content);
            if (ret.StatusCode == System.Net.HttpStatusCode.OK)
            {
                retval = await ret.Content.ReadAsStringAsync();
            }
            else
            {
                print_error($"HTTP POST call failed for URL [{url}]");
            }
        }).Wait();
        return retval;
    }

    /// <summary>
    /// Converts a string to a json object
    /// </summary>
    /// <param name="json">json string to convert</param>
    /// <returns>A dynamic object representing the json form of the string</returns>
    public static dynamic json(string json)
    {
        var retval = JsonConvert.DeserializeObject<dynamic>(json);
        return retval;
    }

    /// <summary>
    /// Converts a string to an xml document
    /// </summary>
    /// <param name="xml">xml string to convert</param>
    /// <returns>An XmlDocument object</returns>
    public static XmlDocument xml(string xml)
    {
        var retval = new XmlDocument();
        retval.LoadXml(xml);
        return retval;
    }
    #endregion
}

