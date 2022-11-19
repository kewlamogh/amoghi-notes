using System.IO;
using System.Collections.Generic;
using System.Windows.Controls;

namespace amoghi_notes
{
    public class Persistance
    {
        public static string[] GetAll()
        {
            string path = @"Persistor.txt";

            if (!File.Exists(path))
            {
                using FileStream fs = File.Create(path);
            }

            path = @"Recover.txt";

            if (!File.Exists(path))
            {
                using FileStream fs = File.Create(path);
            }

            return File.ReadAllText(@".\Persistor.txt").Split("\n");
        }
        public static void Write(dynamic x, string file = "Persistor.txt")
        {
            File.WriteAllLines(@$"{file}", x);
        }
        public static void Save(ItemCollection o)
        {
            List<string> props = new();
            foreach (object i in o) props.Add(i.ToString().Replace("\n", "<br>").Trim());
            Write(props);
        }
    }
}