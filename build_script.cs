#:package Thinkmine.NetX.Framework@1.0.6-preview

using static netx;

print("Update everything to nuget version");

var nuget_version = commandline[0];
print($"Your value is {nuget_version}");



//replace_in_file("NetXFramework/Thinkmine.NetX.Framework.csproj", "@@NEW_VERSION", nuget_version);