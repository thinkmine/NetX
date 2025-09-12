//#:package Thinkmine.NetX.Framework@1.0.4-preview

using static netx;

//List command line arguments
print("Command Line","red","white");
foreach (var item in commandline)
{
    print($"{item}");
}

//List environment variables
print("Environment Variables", "red", "white");
foreach (var item in environment_variables)
{
    print($"{item.Key}={item.Value}");
}