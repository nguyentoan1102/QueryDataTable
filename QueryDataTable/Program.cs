using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace QueryDataTable
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                DataTable dataTable = CreateTable();
                var result = (from row in dataTable.AsEnumerable()
                              where row.Field<string>("NAME") != "SAM"
                              select row).ToList();
                var result2 = dataTable.AsEnumerable().Where(dt => dt.Field<string>("NAME") != "SAM").ToList();
                var result3 = dataTable.Select().Where(dt => dt.Field<string>("NAME") != "SAM");
                foreach (DataRow o in result3)
                {
                    Console.WriteLine("\t" + o["SSN"] + "\t" + o["NAME"] + "\t" + o["ADDR"] + "\t" + o["AGE"]);
                }

                Console.WriteLine();
                var age = dataTable.Select().Where(dt => dt.Field<int>("AGE") > 60).Take(2);
                foreach (DataRow item in age)
                {
                    Console.WriteLine(item.ToString());
                }
                var tableAsEnumerable = dataTable.Select().ToList();
                foreach (var item in tableAsEnumerable)
                {
                    Console.WriteLine("\t" + item["SSN"] + "\t" + item["NAME"] + "\t" + item["ADDR"] + "\t" + item["AGE"]);
                }

                //var table = ConvertToDataTable<T>(tableAsEnumerable);
            }
            catch (Exception ex)
            {

                throw ex;
            }
            Console.ReadLine();
        }
        public static DataTable ConvertToDataTable<T>(IEnumerable<T> items)
        {
            var tb = new DataTable(typeof(T).Name);

            PropertyInfo[] props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (var prop in props)
            {
                tb.Columns.Add(prop.Name, prop.PropertyType);
            }

            foreach (var item in items)
            {
                var values = new object[props.Length];
                for (var i = 0; i < props.Length; i++)
                {
                    values[i] = props[i].GetValue(item, null);
                }

                tb.Rows.Add(values);
            }

            return tb;
        }

        static DataTable CreateTable()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("SSN", typeof(string));
            dt.Columns.Add("NAME", typeof(string));
            dt.Columns.Add("ADDR", typeof(string));
            dt.Columns.Add("AGE", typeof(int));
            DataColumn[] keys = new DataColumn[1];
            keys[0] = dt.Columns[0];
            dt.PrimaryKey = keys;
            dt.Rows.Add("203456876", "John", "12 Main Street, Newyork, NY", 15);
            dt.Rows.Add("203456877", "SAM", "13 Main Ct, Newyork, NY", 25);
            dt.Rows.Add("203456878", "Elan", "14 Main Street, Newyork, NY", 35);
            dt.Rows.Add("203456879", "Smith", "12 Main Street, Newyork, NY", 45);
            dt.Rows.Add("203456880", "SAM", "345 Main Ave, Dayton, OH", 55);
            dt.Rows.Add("203456881", "Sue", "32 Cranbrook Rd, Newyork, NY", 65);
            dt.Rows.Add("203456882", "Winston", "1208 Alex St, Newyork, NY", 65);
            dt.Rows.Add("203456883", "Mac", "126 Province Ave, Baltimore, NY", 85);
            dt.Rows.Add("203456884", "SAM", "126 Province Ave, Baltimore, NY", 95);
            return dt;
        }
    }
    public class Employee
    {
        public string SSN { get; set; }
        public string NAME { get; set; }
        public string ADDR { get; set; }
        public int AGE { get; set; }
    }
}
