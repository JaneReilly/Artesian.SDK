using Artesian.SDK.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Artesian.COMSample
{
	public static class Ex
	{
		public static ActualTimeSerieRow ConvertToOutput(this TimeSerieRow.Actual input)
		{
			var res = new ActualTimeSerieRow()
			{
				CompetenceEnd = input.CompetenceEnd.ToString(),
				CompetenceStart = input.CompetenceStart.ToString(),
				CurveName = input.CurveName,
				ProviderName = input.ProviderName,
				Time = input.Time.ToString(),
				TSID = input.TSID,
				Value = input.Value ?? 0
			};

			return res;
		}


	}
}
