using System;

namespace Homework1
{
	class Program
	{
		static void Main(string[] args)
		{
			string[] destinations = args[0].Split(",");

			int hoursPassed = 0;

			int initialNumberOfPackages = destinations.Length;
			int packagesAtFactory = initialNumberOfPackages;
			int packagesAtPort = 0;
			int packagesDelivered = 0;
			int currentFactoryPackageIndex = 0;

			int hoursToA = 4;
			int hoursToPort = 1;
			int hoursToB = 5;

			var truck1 = new Carrier();
			var truck2 = new Carrier();
			var ferry = new Carrier { DeliveryPoint = "A" };

			while (packagesDelivered < initialNumberOfPackages)
			{
				// Unloading ferry:

				if (ferry.HasArrived)
				{
					ferry.IsReturning = true;
					ferry.HoursToArrival = hoursToA;
					packagesDelivered++;
				}

				// Unloading truck 1:

				if (truck1.HasArrived)
				{
					truck1.IsReturning = true;

					if (truck1.DeliveryPoint == "Port")
					{
						truck1.HoursToArrival = hoursToPort;
						packagesAtPort++;
					}
					else
					{
						truck1.HoursToArrival = hoursToB;
						packagesDelivered++;
					}

				}

				// Unloading truck 2:

				if (truck2.HasArrived)
				{
					truck2.IsReturning = true;

					if (truck2.DeliveryPoint == "Port")
					{
						truck2.HoursToArrival = hoursToPort;
						packagesAtPort++;
					}
					else
					{
						truck2.HoursToArrival = hoursToB;
						packagesDelivered++;
					}

				}

				// Loading truck 1:

				if (packagesAtFactory > 0 && truck1.IsReadyToLoad)
				{
					var deliveryPoint = destinations[currentFactoryPackageIndex++] == "A" ? "Port" : "B";
					truck1.LoadForDestination(deliveryPoint, deliveryPoint == "Port" ? hoursToPort : hoursToB);
					packagesAtFactory--;
				}

				// Loading truck 2:

				if (packagesAtFactory > 0 && truck2.IsReadyToLoad)
				{
					var deliveryPoint = destinations[currentFactoryPackageIndex++] == "A" ? "Port" : "B";
					truck2.LoadForDestination(deliveryPoint, deliveryPoint == "Port" ? hoursToPort : hoursToB);
					packagesAtFactory--;
				}

				// Loading ferry:

				if (packagesAtPort > 0 && ferry.IsReadyToLoad)
				{
					ferry.LoadForDestination("A", hoursToA);
					packagesAtPort--;
				}

				// Passing time:

				if (packagesDelivered < initialNumberOfPackages)
				{
					truck1.Move();
					truck2.Move();
					ferry.Move();

					hoursPassed++;
				}
			}

			Console.WriteLine($"{string.Join(",", destinations)} -> {hoursPassed}");

		}
	}

	public class Carrier
	{
		public string DeliveryPoint = "";
		public bool IsReturning = true;
		public int HoursToArrival = 0;

		public bool HasArrived => !IsReturning && HoursToArrival == 0;
		public bool IsReadyToLoad => IsReturning && HoursToArrival == 0;

		public void LoadForDestination(string destination, int timeToArrival)
		{
			DeliveryPoint = destination;
			HoursToArrival = timeToArrival;
			IsReturning = false;
		}

		public void Move()
		{
			if (HoursToArrival > 0)
			{
				HoursToArrival--;
			}
		}
	}

}
