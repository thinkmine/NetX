//#:package NetX@1.0.0
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


var v = http_get("http://www.google.com");
print(v);

var v2 = http_post("http://www.google.com", new
{
    Name = "John",
    Age = 33,
});
print(v2);

rename("helloworld.txt", "test.txt");









