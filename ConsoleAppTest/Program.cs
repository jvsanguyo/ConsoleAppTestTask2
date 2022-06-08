using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;

namespace ConsoleAppTest
{
    class Program
    {
        static List<Country> GetCountries()
        {
            var list = new List<Country>();
            list.Add(new Country
            {
                Id = 1,
                Name = "Philippines",
                ShortName = "PH",
            });
            list.Add(new Country
            {
                Id = 2,
                Name = "United Arab Emirates",
                ShortName = "UAE",
            });
            list.Add(new Country
            {
                Id = 3,
                Name = "Kingdom of Saudi Arabia",
                ShortName = "KSA",
            });
            return list;
        }
        static void Main(string[] args)
        {
            DataTable dataTable = ConvertToDataTable(GetCountries());
            var lines = new List<string>();

            string[] columnNames = dataTable.Columns
                .Cast<DataColumn>()
                .Select(column => column.ColumnName)
                .ToArray();

            var header = string.Join(",", columnNames.Select(name => $"\"{name}\""));
            lines.Add(header);

            var valueLines = dataTable.AsEnumerable()
                .Select(row => string.Join(",", row.ItemArray.Select(val => $"\"{val}\"")));

            lines.AddRange(valueLines);

            File.WriteAllLines("excel.csv", lines);
            Console.Write("File save in current working directory, press enter to exit");
            Console.ReadLine();
        }
        static DataTable ConvertToDataTable<T>(IList<T> data)
        {
            PropertyDescriptorCollection properties =
               TypeDescriptor.GetProperties(typeof(T));
            DataTable table = new DataTable();
            foreach (PropertyDescriptor prop in properties)
                table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
            foreach (T item in data)
            {
                DataRow row = table.NewRow();
                foreach (PropertyDescriptor prop in properties)
                    row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
                table.Rows.Add(row);
            }
            return table;
        }

    }
}
