using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using MySql.Data.MySqlClient;
using RateValue;



namespace RateValute
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Write("Input host: ");
            string host = Console.ReadLine();
            Console.Write("Input database: ");
            string dataBase = Console.ReadLine();
            Console.Write("Input user: ");
            string user = Console.ReadLine();
            Console.Write("Input password: ");
            string pass = Console.ReadLine();
            Console.Write("Input valute: ");
            string valute = Console.ReadLine();
            Console.Write("Input year: ");
            int year = Convert.ToInt32(Console.ReadLine());
            Console.Write("Input month: ");
            int month = Convert.ToInt32(Console.ReadLine());
            Console.Write("Input day: ");
            int day = Convert.ToInt32(Console.ReadLine());


            DateTime date = new DateTime(year, month, day);
            Valute val = new Valute(valute, date);
            Db db = new Db(valute, date.ToString("yyyy-MM-dd"), val.ReturnParametrs(),host,dataBase,user,pass);
            db.SaveToDb();
        }
    }
}

        