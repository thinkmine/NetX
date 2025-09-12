#:package Thinkmine.NetX.Framework@1.0.2-preview

using static netx;

print(commandline.Length.ToString());
if (commandline.Length > 0)
    print($"Command Line: {commandline[0]}");

foreach (var item in environment_variables)
{
    print($"{item.Key}={item.Value}");
}


var passcode = prompt("What is your passcode");
if (Validate(passcode))
{
    exec("dir");
    exec("echo hello");
    pwd();
    //cd("../../..");
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


bool Validate(string passcode)
{
    if (passcode == "netx")
        return true;

    return false;
}