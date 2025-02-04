Console.WriteLine("Hello, World!");

foreach (var env in Environment.GetEnvironmentVariables().Keys)
{
    Console.WriteLine(env);
}