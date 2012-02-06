using System;
using System.Linq;
using System.IO;
using System.Xml.Linq;

namespace ReplaceKeyValue
{
    class Program
    {
        static void Main(string[] args)
        {
            if ((args == null) || args.Length < 3 || (args.Length % 2 == 0))
            {
                Console.WriteLine("Wrong number of arguments");
                Console.WriteLine("Usage: StringReplace C:\\Text.txt OldValue NewValue ");
                return;
            }

            var fileName = args[0];
            XElement contents;
            try
            {
                using (StreamReader streamReader = File.OpenText(fileName))
                {
                    contents = XElement.Load(streamReader);
                }

                using (StreamWriter streamWriter = File.CreateText(fileName))
                {
                    for (int i = 1; i < args.Length; i += 2)
                    {
                        var node = contents.Element("appSettings")
                            .Elements("add").Where(el => el.Attribute("key").Value == args[i]).FirstOrDefault();
                        node.Attribute("value").SetValue(args[i + 1]);

                        Console.WriteLine(String.Format("Replacing {0} with {1}", args[i], args[i + 1]));
                    }
                    streamWriter.Write(contents);
                }
            }
            catch (FileNotFoundException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
