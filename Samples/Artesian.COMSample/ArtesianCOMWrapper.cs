using Artesian.SDK.Dto;
using Artesian.SDK.Service;
using NodaTime;
using NodaTime.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

using Newtonsoft;
using System.Reflection;
using System.IO;
using System.Net;

namespace Artesian.COMSample
{
	[ComVisible(true)]
	[ClassInterface(ClassInterfaceType.AutoDual)]
	public class ArtesianCOMWrapperFactory : MarshalByRefObject
	{
		static ArtesianCOMWrapperFactory()
		{
			AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(_assemblyResolve);
			ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
		}

		[ComVisible(true)]
		public ArtesianCOMWrapper Create(string url, string apiKey)
		{
			return new ArtesianCOMWrapper(url, apiKey);
		}
		
		static Assembly _assemblyResolve(object sender, ResolveEventArgs args)
		{
			try
			{
				string[] Parts = args.Name.Split(',');
				string File = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\" + Parts[0].Trim() + ".dll";

				return Assembly.LoadFrom(File);
			}
			catch
			{
				// ignore load error 
			}

			Assembly assembly = Assembly.Load(args.Name);
			return assembly;
		}
	}

	[ComVisible(true)]
	[ClassInterface(ClassInterfaceType.AutoDual)]
	public class ArtesianCOMWrapper
	{
		private LocalDatePattern _pattern = LocalDatePattern.CreateWithInvariantCulture("yyyy-MM-dd");

		ArtesianServiceConfig _cfg;

		internal ArtesianCOMWrapper(string url, string apiKey)
		{
			_cfg = new ArtesianServiceConfig(new Uri(url), apiKey);
		}

		[ComVisible(true)]
		public ActualTimeSerieRow[] GetActuals(string csvMarketDataIds, string start, string end)
		{
			var startDate = ParseDateWithPattern(start);
			var endDate = ParseDateWithPattern(end);

			var marketDataIds = csvMarketDataIds.Split(';').Select(s => Int32.Parse(s)).ToArray();


			var queryservice = new QueryService(_cfg);


			var q = queryservice.CreateActual()
							.InTimezone("CET")
							.ForMarketData(marketDataIds)
							.InGranularity(Granularity.Day)
							.InAbsoluteDateRange(startDate, endDate)
							;

			var res = Task.Run(() => q.ExecuteAsync()).GetAwaiter().GetResult();

			var output = res.Select(s => s.ConvertToOutput()).ToArray();
			return output;
		}		

		private LocalDate ParseDateWithPattern(string input)
		{
			LocalDate cPeriod = default;

			if (_pattern.Parse(input).Success)
				cPeriod = _pattern.Parse(input).Value;
			else
				throw new Exception("DateTime Pattern Error");

			return cPeriod;
		}
	}

	[ComVisible(true)]
	[ClassInterface(ClassInterfaceType.AutoDual)]
	public class ActualTimeSerieRow
	{
		public string ProviderName { get; set; }
		public string CurveName { get; set; }
		public int TSID { get; set; }
		public string Time { get; set; }
		public double Value { get; set; }
		public string CompetenceStart { get; set; }
		public string CompetenceEnd { get; set; }
	}




}
