using System;
using CityRide.DriverConsoleApp.Client.Models;

namespace CityRide.DriverConsoleApp.Client.Responses
{
	public class RideRequestResponse
	{
		public int Id { get; set; }
		public Location From { get; set; }
		public Location To { get; set; }
		public int ClientId { get; set; }
		public decimal Price { get; set; }
	}
}

