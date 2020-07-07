using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace RateValute
{
    public class Valute
    {
        public DateTime Date { get; set; }
        public string ValuteName { get; set; }
        XDocument xdoc;
        IEnumerable<XElement> itemsHead;
        IEnumerable<XElement> itemsRate;
        IEnumerable<XElement> itemsChild;
        int count;
        Queue<string> parametrs;
        string returnValue;
        string pathURI = "http://www.cbr.ru/scripts/XML_daily.asp?date_req=";

        public Valute(string valute, DateTime date)
        {
            Date = date;
            ValuteName = valute;
            parametrs = new Queue<string>();

            try
            {
                xdoc = XDocument.Load(pathURI + Date.ToString("dd.MM.yyyy"));
                Console.WriteLine("Load from {0} success\n", pathURI + Date.ToString("dd.MM.yyyy"));
                Console.WriteLine("Date unloading: {0}. Chooche valute: {1}\n", Date.ToString("dd.MM.yyyy"), ValuteName);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
        public Queue<string> ReturnParametrs()
        {
            itemsHead = from t in xdoc.Root.Elements()
                        from x in t.Elements()
                        from z in t.Elements()
                        where x.Value == ValuteName && z.Name == "Value"
                        select t;

            foreach (XElement i in itemsHead)
            {
                parametrs.Enqueue(i.Attribute("ID").Value);
                count++;
            }

            itemsChild = from t in xdoc.Root.Elements()
                         from x in t.Elements()
                         from z in t.Elements()
                         from c in t.Elements()
                         where x.Value == ValuteName && z.Name == "Value"
                         select c;

            foreach (XElement i in itemsChild)
            {
                parametrs.Enqueue(i.Value);
                count++;
            }
            Console.WriteLine("\n\t\tVALUTE\n___________________________________");
            foreach (string i in parametrs)
                Console.WriteLine(i);
            Console.WriteLine("___________________________________");
            return parametrs;
        }
        public string ReturnKurs()
        {
            try
            {
                xdoc = XDocument.Load(pathURI + Date.ToString("dd.MM.yyyy"));
                Console.WriteLine("Load from {0} success\n", pathURI + Date.ToString("dd.MM.yyyy"));
                Console.WriteLine("Date unloading: {0}. Chooche valute: {1}\n", Date.ToString("dd.MM.yyyy"), ValuteName);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            itemsRate = from t in xdoc.Root.Elements()
                        from x in t.Elements()
                        from z in t.Elements()
                        where x.Value == ValuteName && z.Name == "Value"
                        select z;

            foreach (XElement i in itemsRate)
            {
                Console.WriteLine($"Rate - {i.Value}");
                returnValue = i.Value;
            }
            return returnValue;
        }
    }
}
