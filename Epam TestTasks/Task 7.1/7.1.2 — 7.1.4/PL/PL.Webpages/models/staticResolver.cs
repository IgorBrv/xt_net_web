using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Dependencies;
using System.Web.WebPages;
using System.Dynamic;

namespace PL.Webpages.models
{
	public static class StaticResolver
	{
		private static Resolver resolver = null;

		public static Resolver Get(string spath)
		{
			if (resolver == null)
			{
				Path = spath;

				if (!System.IO.Directory.Exists(spath))
				{
					System.IO.Directory.CreateDirectory(spath);
				}

				resolver = new Resolver(spath);
			}
			return resolver;
		}

		public static string Path { get; private set; }
	}
}