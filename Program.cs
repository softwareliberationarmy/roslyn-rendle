// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");

var demo = new RoslynDemo();
//await demo.Start("E:\\git\\steris\\spmi\\SPMi.sln");
await demo.Start("C:\\git\\work\\spmi\\SPMi.sln");

Console.WriteLine("*** DONE ***");
Console.ReadLine();


//for more info, see https://joshvarty.com/learn-roslyn-now/
//original talk from NDC Copenhagen 2022: https://youtu.be/V4zqk4-LL1M
