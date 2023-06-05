using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AirTek_Assignment
{
    public enum Days
    {
        Monday, Tuesday
    }

    public enum AirportCodes
    {
        YYZ, YUL, YVR, YYC
    }

    internal class Program
    {

        const int FlightCount = 6;
        
        static void Main(string[] args)
        {
            Flight[] flights = new Flight[FlightCount];
            
            setSchedule(flights);
            prepareOrder(readJsonFile(), flights);

        }


        static Dictionary<string, string> readJsonFile()
        {
            Dictionary<string, string> assignmentOrders = new Dictionary<string, string>();
            string path = Path.Combine(Environment.CurrentDirectory, @"Data\", "coding-assigment-orders.json");
            using (StreamReader r = new StreamReader(path))
            {
                
                string jsonText = r.ReadToEnd();
                var parsedJsonText = JObject.Parse(jsonText);
                
                foreach(var obj in parsedJsonText.Children())
                {
                    var orderNo  = obj.Path;
                    var destination = ((JProperty)obj).Value;
                    if(!assignmentOrders.ContainsKey(orderNo))
                    {
                        assignmentOrders[orderNo] = destination["destination"].ToString();
                    }
                }
            }
            return assignmentOrders;

        }

        static void printSchedule(Flight[] flights)
        {
            foreach(var flight in flights)
            {
                Console.WriteLine("Flight: {0}, departure: {1}, arrival: {2}, day:{3}", flight.FlightNumber, flight.Departure, flight.Destination, (int)flight.DaysOfOperation + 1);
            }
            Console.ReadLine();
        }

        static void printSchedule(OrderShipment[] orders)
        {
            for (int i = 0; i < orders.Length; i++)
                {
                    if (orders[i] == null)
                        break;

                    if (orders[i].OrderName == "order-x")
                        Console.WriteLine("Order: {0}, flightNumber: {1}", orders[i].OrderName, orders[i].FlightNumber);
                    else
                        Console.WriteLine("Order: {0}, flightNumber: {1}, departure: {2}, arrival: {3}, day: {4}", orders[i].OrderName, orders[i].FlightNumber, orders[i].Departure, orders[i].Destination, (int)orders[i].DaysOfOperation + 1);
                }
            Console.ReadLine();
        }

        static void setSchedule(Flight[] flights)
        {
            for(int i= 0; i < flights.Length; i++)
            {
                flights[i] = new Flight();
                switch (i)
                {
                    case 0:
                        {
                            flights[i].DaysOfOperation = Days.Monday;
                            flights[i].Destination = AirportCodes.YYZ.ToString();
                            flights[i].FlightNumber = "1";
                            break;
                        }
                    case 1:
                        {
                            flights[i].DaysOfOperation = Days.Monday;
                            flights[i].Destination = AirportCodes.YYC.ToString();
                            flights[i].FlightNumber = "2";
                            break;
                        }
                    case 2:
                        {
                            flights[i].DaysOfOperation = Days.Monday;
                            flights[i].Destination = AirportCodes.YVR.ToString();
                            flights[i].FlightNumber = "3";
                            break;
                        }
                    case 3:
                        {
                            flights[i].DaysOfOperation = Days.Tuesday;
                            flights[i].Destination = AirportCodes.YYZ.ToString();
                            flights[i].FlightNumber = "4";
                            break;
                        }
                    case 4:
                        {
                            flights[i].DaysOfOperation = Days.Tuesday;
                            flights[i].Destination = AirportCodes.YYC.ToString();
                            flights[i].FlightNumber = "5";
                            break;
                        }
                    case 5:
                        {
                            flights[i].DaysOfOperation = Days.Tuesday;
                            flights[i].Destination = AirportCodes.YVR.ToString();
                            flights[i].FlightNumber = "6";
                            break;
                        }
                }
            }
            printSchedule(flights);
        }
        static void prepareOrder(Dictionary<string, string> assignmentOrders, Flight[] flights)
        {
            int j = 0;
            OrderShipment[] orderShipmentsList = new OrderShipment[100];
            foreach (KeyValuePair<string, string> assignmentOrder in assignmentOrders)
            {
                var i = Enumerable.Range(0, flights.Length).Where(d => flights[d].Destination == assignmentOrder.Value).ToList();
                
                if (i.Count == 0)
                {
                    orderShipmentsList[j] = new OrderShipment();
                    orderShipmentsList[j].OrderName = "order-x";
                    orderShipmentsList[j].FlightNumber = "not scheduled";
                }

                for (int t = 0; t < i.Count; t++)
                {
                    int index = i[t];
                    if (flights[index].Capacity > 0)
                    {
                        index = i[t];
                    }
                    else
                    {
                        index = i[t + 1];
                    }

                    orderShipmentsList[j] = new OrderShipment();

                    orderShipmentsList[j].OrderName = assignmentOrder.Key;
                    orderShipmentsList[j].FlightNumber = flights[index].FlightNumber;
                    orderShipmentsList[j].Departure = flights[index].Departure;
                    orderShipmentsList[j].Destination = flights[index].Destination;
                    orderShipmentsList[j].DaysOfOperation = flights[index].DaysOfOperation;
                    flights[index].Capacity--;
                    break;
                }
                
                j++;
            }
            printSchedule(orderShipmentsList);
        }
    }

    class Flight
    {
        const int FlightCapacity = 20;
        public string FlightNumber { get; set; }
        public string Destination { get; set; }
        public string Departure { get; set; } = AirportCodes.YUL.ToString();
        public Days DaysOfOperation { get; set; }
        public int Capacity { get; set; } = FlightCapacity;

    }

    class OrderShipment : Flight
    {
        public string OrderName { get; set; } 

    }
}
