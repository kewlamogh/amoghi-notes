using System.IO;
using System.Collections.Generic;
using System.Windows.Controls;

public class Persistance
{
    public static string[] GetAll()
    {
        return File.ReadAllText(@"C:\Users\amogh\source\repos\amoghi-notes\Persistor.txt").Split("\n");
    }
    public static void Write(dynamic x, string file = "Persistor.txt")
    {
        File.WriteAllLines(@$"C:\Users\amogh\source\repos\amoghi-notes\{file}", x);
    }
    public static void Save(ItemCollection o)
    {
        List<string> props = new();
        foreach (object i in o) props.Add(i.ToString().Replace("\n", "<br>").Trim());
        Write(props);
    }
}