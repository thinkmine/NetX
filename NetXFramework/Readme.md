# NetX

## Release Notes Version Preview 1

This is a __preview__ version of NetX focused on exploring functionality, usage scenarios,
and feasibility.  While all commands have been tested, all error conditions have not been encountered.

#### 1.0.1-preview Changes
- Added ```commandline```
- Added ```environment_variable```
- Added ```prompt(<text>)```


## Why netx

With the release of .NET 10 comes a powerful new feature to C#, __dotnet run file__.
This capability in the new .NET build toolchain lets us run C# files directly from the ```dotnet run``` command.
This means that we can easily execute functionality, using C#, in a manner very similar to writing
scripts.  To find out more about this feature here:
https://devblogs.microsoft.com/dotnet/announcing-dotnet-run-app/  

Of course there are still a number of differences, as C# even though extremely powerful is quite verbose
in comparison to the needs of a script.  This is obviously because it is designed primarily to be a language for
building applications.

### How it works
With the netx nuget and additional toolkit we make it easy for you to execute the most
common script tasks by exposing those capabilities to you in a manner that can be easily
leveraged for automation and general purpose scripting.  netx offers the best of both worlds as you can
seamlessly switch between top-down scripts and more sophisticated coding all in the same document.

Here is what a sample netx script looks like.  Assuming this file was called __dostuff.cs__ you can run this code and execute this functionality
```dotnet run dostuff.cs```  

```
//#:package NetX@1.0.2-preview
using static netx;

exec("dir");
exec("echo hello");
pwd();
cd("../../..");
rename("test.txt", "helloworld.txt");
list();
copy("bin", "test");
list();
pwd();
copy("program.cs", "c:/temp", true, "hello.cs");
pack("c:/temp", "temp.zip");
list();
unpack("temp.zip", "destination");
delete("temp.zip");
delete("destination");
delete("test");
```

Using __netx__ you can add high level concepts like loops, function calls, referencing libraries,
and much more to your scripting experience.  We can, for instance, apply some conditional logic to our
script from above as follows:


```
var passcode = prompt("What is your passcode");
if (passcode == "netx")
{
    exec("dir");
    exec("echo hello");
    pwd();
    cd("../../..");
    rename("test.txt", "helloworld.txt");
    list();
    copy("bin", "test");
    list();
    pwd();
    copy("program.cs", "c:/temp", "hello.cs");
    pack("c:/temp", "temp.zip");
    list();
    unpack("temp.zip", "destination");
    delete("temp.zip");
    delete("destination");
    delete("test");


    var v = http_get("http://www.google.com");
    print(v);

    var v2 = http_post("http://www.google.com", new
    {
        Name = "John",
        Age = 33,
    });
    print(v2);

    rename("helloworld.txt", "test.txt");
}
else
{
    print("You can't do this", "white", "red");
}
```

Now when the script runs the user is prompted to enter a passcode
that we then check to see it's validity. Based on that we either execute 
the normal path or let the user know it can't be done.

### Accessing command line arguments
To access command line arguments use the ```commandline``` global object.  This object
returns a string array of all the arguments passed into the program at run time.  In the
example below we use this object to detect whether any command line arguments
have been sent in and print out the first argument if true.

```
print(commandline.Length.ToString());
if (commandline.Length > 0)
    print($"Command Line: {commandline[0]}");
```

### Accessing environment variables

To access environment variables use the ```environment_variables``` global object.  This 
object returns a string based name value dictionary of all global list of 
environment variables in scope during the exection of the program.

```
foreach (var item in environment_variables)
{
    print($"{item.Key}={item.Value}");
}
```


### Calling Functions
We can even take it a bit further with the ability to make function calls.

```
var passcode = prompt("What is your passcode");
if (Validate(passcode))
{
    //normal flow
}
else
{
    print("You can't do this", "white", "red");
}


bool Validate(string passcode)
{
    if(passcode == "netx")
    return true;

    return false;
}
```


## Basic commands

netx currently has 14 commands you can perform with it.  The table below provides instructions
on what they are and how to use them.

| Command | Sample | Description |
|---------|--------|-----|
|pwd|```pwd()```|Prints the current working directory to the console.
|cd|```cd(<folder name>)```|Changes the current working directory to the one specified in the argument|
|list|```list()```|Lists all contents in the current folder
|copy|```copy(<source>,<destination>)```|Copies a file or folder from source to destination
|delete|```delete```|Deletes a file or folder
|pack|```pack(<folder>,<zip file>)```|Zips a folder
|unpack|```unpack(<file>,<folder>)```|Unzips a zip file to a folder
|print|```print(<text>)```|Prints the specified text to the console
|run|```run(<executable>)```|Runs an executable.  The file must be in the current working folder or in the global path
|exec|```exec(<command>)```|Executes a console command as if it was typed in and run directly in the console.  This can be used do "ls", "dir", etc.
|http_get|```http_get(<url>)```|Makes an HTTP GET call to the endpoint provided
|http_post|```http_post(<url>,{object})```|Makes an HTTP POS call to the endpoint provided, passing in an object as the payload.  The object will be converted to json before being sent.
|json|```json(<text>)```|Converts a string to a json object.
|xml|```xml(<text>)```|Converts a string to an xml document.
|commandline|```commandline```|Gets the command line that was passed into the script when it was run
|environment_variables|```environment_variables```|Gets a string based name/value dictionary of all the enviroment variables in scope.
|current_directory|```current_directory```|Gets the current directory as a string value


## Advanced commands

Documentation coming soon.
