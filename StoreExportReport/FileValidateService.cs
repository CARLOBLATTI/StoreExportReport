using System;
using System.Text.RegularExpressions;

namespace StoreExportReport
{
    //10 colonne separate da ;
    //Prima riga nome negozio
    //Ultima riga numero righe prodotto
    //guardare documento per specifiche controlli campi
    internal class FileValidateService
    {
        //Validazione estensione file
        public Boolean validateFileExtension(String extension)
        {
            Boolean result = false;
            String lowerExtension = extension.ToLower();
            if (extension.ToLower().Equals(".csv"))
            {
                result = true;
            }
            return result;
        }

        //validazione prima riga del file che deve contenere solo il nome del negozio
        public Boolean validateFirstLine(String firstLine)
        {
            Boolean result = true;
            var firstLineSplitted = firstLine.Split(';');
            if (firstLineSplitted.Length > 1)
            {
                Console.WriteLine("Fisrt line must contains only shop's name");
                Console.WriteLine($"First line: {firstLine}");
                result = false;
            }
            return result;
        }
        //validazione ultima riga che deve contenere il numero di righe prodotti 
        public Boolean validateLastLine(String lastLine)
        {
            Boolean result = true;
            var firstLineSplitted = lastLine.Split(';');
            if (firstLineSplitted.Length > 1)
            {
                Console.WriteLine("Last line must contains only the tickets number");
                Console.WriteLine($"Last line: {lastLine}");
                result = false;
                return result;
            }
            result = validateInteger(lastLine);
            return result;
        }
        //validazione delle righe prodotti e dei campi che contengono
        public Boolean validateGenericLine(String genericLine, int lineNumber, Company company)
        {
            Boolean result = true;
            var genericLineSplitted = genericLine.Split(';');
            if (genericLineSplitted.Length != 10)
            {
                Console.WriteLine("The line " + lineNumber + " contains a wrong number of columns");
                Console.WriteLine($"Line: {genericLine}");
                result = false;
                return result;
            }

            //1 id società
            if (!validateInteger(genericLineSplitted[0]) || !genericLineSplitted[0].Equals(company.CompanyId))
            {
                Console.WriteLine("Wrong data format for society's id");
                result = false;

            }
            //2 id negozio
            if (!validateInteger(genericLineSplitted[1]) || !validateShopId(genericLineSplitted[1], company.CompanyShopsId))
            {
                Console.WriteLine("Wrong data format for shop's id");
                result = false;

            }
            //3 id cassa
            if (!validateInteger(genericLineSplitted[2]))
            {
                Console.WriteLine("Wrong data format for cash register's id");
                result = false;

            }
            //4 id scontrino
            if (!validateInteger(genericLineSplitted[3]))
            {
                Console.WriteLine("Wrong data format for ticket's id");
                result = false;

            }

            DateTime dateTemp, timeTemp;
            var formats = new[] { "yyyy/mm/dd", "HH:mm" };
            String dateTime = genericLineSplitted[4] + " " + genericLineSplitted[5];
            //5 data
            if (DateTime.TryParseExact(genericLineSplitted[4], formats, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out dateTemp))
            {
                Console.WriteLine("Valid date format");
                Console.WriteLine(dateTemp);
            }
            else
            {
                Console.WriteLine(dateTemp);
                Console.WriteLine("Wrong date format");
                result = false;
            }
            //6 ora
            if (DateTime.TryParseExact(genericLineSplitted[5], formats, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out timeTemp))
            {
                Console.WriteLine("Valid time format");
                Console.WriteLine(timeTemp);
            }
            else
            {
                Console.WriteLine(timeTemp);
                Console.WriteLine("Wrong time format");
            }

            //7 prodotto
            if (genericLineSplitted[6] == null || !genericLineSplitted[6].Contains("-"))
            {
                Console.WriteLine("Wrong data format for products categories and description");
                result = false;

            }

            //8 prezzo
            if (!validatePrice(genericLineSplitted[7]))
            {
                Console.WriteLine("Wrong data format for price");
                result = false;

            }

            //9 numero pezzi
            if (!validateInteger(genericLineSplitted[8]))
            {
                Console.WriteLine("Wrong data format for products number");
                result = false;

            }
            //10 codice iva
            if (!validateInteger(genericLineSplitted[9]))
            {
                Console.WriteLine("Wrong data format for iva code");
                result = false;

            }
            return result;
        }

        private Boolean validateInteger(String textNumber)
        {
            Boolean result = true;
            try
            {
                int intNumber = Int32.Parse(textNumber);
                Console.WriteLine("The value " + textNumber + " is a valid number");
            }
            catch (FormatException)
            {
                Console.WriteLine($"Unable to parse '{textNumber}'");
                result = false;
            }
            return result;
        }
        //validazione dello shop id rispetto a quello del file di configurazione
        private Boolean validateShopId(String shopId, String[] shopsId)
        {
            Boolean result = true;
            result = Array.Exists(shopsId, element => element == shopId);
            if (!result)
            {
                Console.WriteLine("Shop Id non presente per questa società");
            }
            return result;
        }

        //validazione del campo prezzo utilizzando regex
        private Boolean validatePrice(String price)
        {
            Boolean result = Regex.Match(price, @"^[0-9]{0,6}(\.[0-9]{2})?$").Success;
            Console.WriteLine("Price: " + Regex.Match(price, @"^[0-9]{0,6}(\.[0-9]{2})?$").Value);
            return result;
        }
    }
}
