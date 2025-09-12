#:package Thinkmine.NetX.Framework@1.0.4-preview

using static netx;

print("Script customization");

pwd();

//copy the created nuget package to the root folder
copy("NetXFramework/bin/release/thinkmine.netx.framework.1.0.4-preview.nupkg", ".");