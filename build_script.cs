#:package Thinkmine.NetX.Framework@1.0.4-preview

using static netx;

print("Script customization");

//check the current directory
pwd();

//copy the created nuget package to the root folder
copy("NetXFramework/bin/release/thinkmine.netx.framework.1.0.5-preview.nupkg", ".");

//see what was added to the folder
list();