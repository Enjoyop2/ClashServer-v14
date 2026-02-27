using System.Collections.Generic;
using System.IO;

namespace ClashofClans.CsvConverter
{
    public class CsWriter
    {
        internal CsWriter(string name, IReadOnlyList<string> header, string[] types, string space)
        {
            using StreamWriter writer = new StreamWriter($"CS Output/{Uppercase(name)}.cs");
            writer.WriteLine($"using ClashofClans.Files.CsvHelpers;\nusing ClashofClans.Files.CsvReader;\n\nnamespace ClashofClans.Files.{space}");
            writer.WriteLine("{");
            writer.WriteLine($"    public class {Uppercase(name)} : Data");
            writer.WriteLine("    {");
            writer.WriteLine(
                $"        public {Uppercase(name)}(Row row, DataTable datatable) : base(row, datatable)");
            writer.WriteLine("        {");
            writer.WriteLine("            LoadData(this, GetType(), row);");
            writer.WriteLine("        }");
            writer.WriteLine();

            int count = header.Count;

            for (int index = 0; index < count; index++)
            {
                string type = types[index].ToLower() == "boolean" ? "bool" : types[index].ToLower();

                writer.WriteLine("        public " + type + " " + header[index].Replace(" ", string.Empty) + " { get; set; }");

                if (index < count - 1)
                    writer.WriteLine();
            }

            writer.WriteLine("    }");
            writer.WriteLine("}");
        }

        internal string Uppercase(string _string)
        {
            if (string.IsNullOrEmpty(_string))
                return string.Empty;

            string[] result = _string.Split('_');
            string newString = string.Empty;
            foreach (string s in result)
            {
                char[] _char = s.ToCharArray();
                _char[0] = char.ToUpper(_char[0]);

                newString += new string(_char);
            }

            return newString;
        }
    }
}