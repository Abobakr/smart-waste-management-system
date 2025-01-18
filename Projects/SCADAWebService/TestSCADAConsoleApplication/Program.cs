using SCADADataContracts;
using SCADAWebServiceClientLibrary;
using System;
using System.Collections.Generic;

namespace SCADAWebServiceClientLibrary
{
    class Program
    {
        static void Main(string[] args)
        {

            Console.WriteLine("--------------------------------------------------");
            Console.WriteLine("Do you want to get unit item  long,lat ? Press Enter");
            Console.ReadLine();

            string terminalId = "4";
            int componentId = -1;
            int roleId = -1;
            double minValue = 50;
            double maxValue = 100;
            DateTime dateTime = new DateTime(2001, 11, 10); //DateTime.Now;
            bool isSubFilter = true;
            LocationType southWest = new LocationType (36,32);
            LocationType northEast = new LocationType (39,35);

            UnitItemFilter unitItemFilter = new UnitItemFilter(terminalId,componentId,roleId,minValue,maxValue,dateTime,isSubFilter,southWest,northEast);
            IList<UnitItemDAL> unitItems = SCADAClient.GetUnitItems(unitItemFilter);

            displayResults(unitItems);

            unitItemFilter.Id = "6";
            unitItems = SCADAClient.GetUnitItems(unitItemFilter);

            displayResults(unitItems);

            //IList<UnitItemDetails> itemDetails = SCADAClient.GetUnitItemDetails(5);
            //Console.WriteLine("****************** Unit Item Details ************");
            //foreach (UnitItemDetails item in itemDetails)
            //{

            //    Console.WriteLine("Name :" + item.Name);
            //    Console.WriteLine("Model No :" + item.ModelNo);
            //    Console.WriteLine("Role :" + item.RoleName);
            //    Console.WriteLine("LastValue :" + item.LastValue);
            //    Console.WriteLine("Date :" + item.Date);
            //}

            Console.ReadLine();


        }

        private static void displayResults(IList<UnitItemDAL> unitItems)
        {
            Console.WriteLine("Results : ...");
            foreach (UnitItemDAL item in unitItems)
            {
                Console.WriteLine("Lat = {0}, Long = {1}", item.Location.Latitude, item.Location.Longitude);
                Console.WriteLine("id = {0}, percentage = {1}", item.Id, item.LastValue);
                if (item.SubLastValues != null && item.SubLastValues.Length > 0)
                {
                    for (int i = 0; i < item.SubLastValues.Length; i++)
                    {
                        Console.WriteLine("value {0} = {1}", i, item.SubLastValues[i]);
                    }
                }
            }
        }
    }
}

