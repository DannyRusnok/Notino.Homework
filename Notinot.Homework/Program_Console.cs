using System;
using System.Collections.Generic;
using System.IO;//usingy navíc
using System.Xml.Linq;
using Newtonsoft.Json;

namespace Notino.Homework //lze použít filescope namespace
{
    public class Document //definici tohoto pomocného DTO bych dal na konec fajlu nebo do vlastní třídy.
    {
        public string Title { get; set; }
        public string Text { get; set; }
    }

    class Program_Console
    {
        static void Main_Console(string[] args)
        {
            var sourceFileName = Path.Combine(
                Environment.CurrentDirectory, 
                "..\\..\\..\\Source Files\\Document1.xml"); // Cesta se dá zjednodušit na ..\\..\\Source Files\\Document1.xml
            
            //definování proměnných mám rád až na místě, kde dávají smysl. Posunul bych na řádek 53.
            var targetFileName = Path.Combine(
                Environment.CurrentDirectory,
                "..\\..\\..\\Target Files\\Document1.json"); // Cesta se dá zjednodušit na ..\\..\\Source Files\\Document1.json

            string input;
            try
            {
                //Tady taky vidím určitou nekonzistenci stylu psaní. Buď používám var anebo explicit types pro proměnné. Kombinovat mi to přijde nekonzistentní.
                /*using */FileStream sourceStream = File.Open(sourceFileName, FileMode.Open); //Je možné rovnou použít metodu OpenRead.
                /*using */var reader = new StreamReader(sourceStream);
                //Jake FileStream tak StreamReader jsou Disposable, takže je silně doporučeno použít klíčové slovo using pro automatické disposnutí spojení mezi souborem a programem.
                //string input = reader.ReadToEnd(); //Tato metoda obsahuje i asynchronní verzi, která je lepší k použití z možného důvodu zablokování hlavního vlákna.
                input = reader.ReadToEnd();
            }
            catch (Exception ex)
            {
                //Odchytávat exceptionu a následně předat její message a znovu vyhodit je blbost.
                //1. Buď můžeme exception rovnou vyhodit pomocí throw;
                //2. Pokud ji nechceme ošetřovat, tak nevidím důvod pro try-catch
                //3. Pokud ji chceme ošetřit, tak rozhodně jinak. Například logováním či výpisem friendly message do konzole pro uživatele a aplikace zavřít.
                throw new Exception(ex.Message);
            }

            //kód není kompilovatelný. Proměnná input je out of scope.
            //Při parsování je vysoká pravděpodobnost chyby. Ošetřil bych try-catch blokem a handloval bych vyjímku.            
            var xdoc = XDocument.Parse(input);

            //xdoc může být null, ošetřil bych ifem.
            var doc = new Document
            {
                //Element nemusí být nalezen, je nullable, ošetřil bych možnost, kdy toto nastane
                Title = xdoc.Root.Element("title").Value,
                //Element nemusí být nalezen, je nullable. Může být ošetřeno i testem, jestli se daný element v root nachází pomocí xdoc.Root.HasElements.
                Text = xdoc.Root.Element("text").Value
            };

            var serializedDoc = JsonConvert.SerializeObject(doc);

            //serializedDoc může být null. Metoda Open očekává non-nullable string podle null-reference types pojetí. Není ovšem "oattributována pomocí [NotNull]" atributem, takže možná kód nespadne
            //V případě, kdy cílový fajl už bude existovat, tak to spadne. Použil Bych FileMode.OpenOrCreate pokud má být business logika taková, že to máme zapsat opakovaně.
            /*using */var targetStream = File.Open(targetFileName, FileMode.Create, FileAccess.Write);
            /*using */var sw = new StreamWriter(targetStream);
            sw.Write(serializedDoc); //Tato metoda obsahuje i asynchronní verzi, která je lepší k použití z možného důvodu zablokování hlavního vlákna.


        }
    }
}