using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Artesian.COMSample
{
	public class Redirector
	{
		public readonly IEnumerable<string> ExcludeList;
		public Redirector(IEnumerable<string> excludeList = null)
		{
			this.ExcludeList = excludeList ?? Enumerable.Empty<string>();
			this.EventHandler = new ResolveEventHandler(AssemblyResolve);
		}
		public readonly ResolveEventHandler EventHandler;
		protected Assembly AssemblyResolve(object sender, ResolveEventArgs resolveEventArgs)
		{
			Console.WriteLine("Attempting to resolve: " + resolveEventArgs.Name); // remove this after its verified to work
			foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
			{
				var pattern = "PublicKeyToken=(.*)$";
				var info = assembly.GetName();
				var included = System.Linq.Enumerable.Contains(ExcludeList, resolveEventArgs.Name.Split(',')[0], StringComparer.InvariantCultureIgnoreCase);
				if (included && resolveEventArgs.Name.StartsWith(info.Name, StringComparison.InvariantCultureIgnoreCase))
				{
					if (Regex.IsMatch(info.FullName, pattern))
					{
						var Matches = Regex.Matches(info.FullName, pattern);
						var publicKeyToken = Matches[0].Groups[1];
						if (resolveEventArgs.Name.EndsWith("PublicKeyToken=" + publicKeyToken, StringComparison.InvariantCultureIgnoreCase))
						{
							Console.WriteLine("Redirecting lib to: " + info.FullName); // remove this after its verified to work
							return assembly;
						}
					}
				}
			}
			return null;
		}
	}


	//[System.AppDomain]::CurrentDomain.add_AssemblyResolve($redirector.EventHandler)
}

