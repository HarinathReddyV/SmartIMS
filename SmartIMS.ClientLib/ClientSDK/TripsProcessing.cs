using System;
using System.Collections.Generic;
using System.Linq;
using SmartIMS.ClientLib.Constants;
using SmartIMS.ClientLib.Extensions;
using SmartIMS.ClientLib.Models;

namespace SmartIMS.ClientLib.ClientSDK
{
    public class TripsProcessing : IDisposable
    {
        List<Drivers> DriversList {get;set;}
        List<Trips> TripsList { get; set; }

        public TripsProcessing()
        {
            DriversList = new List<Drivers>();
            TripsList = new List<Trips>();
        }

        /// <summary>
        /// Process the Data line(Command) and Insert in to appropriate List 
        /// </summary>
        /// <param name="Command"></param>
        public void ProcessCommandLine(string Command)
        {
            if (Command.IsNullString())
                return;
            var data = Command.Split(CharConstants.Data_delimited);

            switch (data[0].Trim().ToLower())
            {
                case StringConstants.DriverCommand:
                    ValidateAndAddDriver(data);
                    break;
                case StringConstants.TripCommand:
                    ValidateAndAddTrip(data);
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Get the consolidated Outup of the input file(commands)
        /// </summary>
        /// <returns></returns>
        public List<TotalTrips> GetTotalTripsList()
        {
            //Consolidating All Trips 
            var result =  TripsList.GroupBy(t => t.Driver, StringComparer.CurrentCultureIgnoreCase)
                                       .Select(t => new TotalTrips
                                       {
                                           Driver = t.Key,
                                           TotalMiles = (int) t.Sum(ta => ta.Miles),
                                           AvgSpeed = (int) t.Average(tb => tb.Speed)
                                       }).OrderByDescending(tc => tc.TotalMiles).ToList();


            //Joining Driver list and Trips List (for 0 Miles)
            return (from a in DriversList
                        join b in result on a.Driver.ToLower() equals b.Driver.ToLower() into x
                        from c in x.DefaultIfEmpty()
                        select new TotalTrips { Driver = a.Driver, TotalMiles = c?.TotalMiles ?? 0, AvgSpeed = c?.TotalMiles ?? 0 }).ToList();
        }

        private void ValidateAndAddDriver(string[] value)
        {
            if (value.Length < 2)
                return;
            if (DriversList.Any(x => x.Driver.ToLower()==value[1].ToLower()))
                return;
            DriversList.Add(new Drivers { Driver = value[1] });
        }

        private void ValidateAndAddTrip(string[] value)
        {
            if (value.Length < 5)
                return;

            if (!DriversList.Any(x => x.Driver.ToLower() == value[1].ToLower()))
                return;

            if (value[2].IsNotValidTime())
                return;

            if (value[3].IsNotValidTime())
                return;

            decimal speed = (value[4].ConvertToDecimal() / (value[3].ConvertTimein100() - value[2].ConvertTimein100()));

            if (speed < 5 || speed > 100)
                return;
            
            TripsList.Add(new Trips { Driver = value[1], Miles = value[4].ConvertToDecimal(), Speed=speed }); ;
        }

        public void Dispose()
        {
            DriversList = new List<Drivers>();
            TripsList = new List<Trips>();
        }
    }
}
