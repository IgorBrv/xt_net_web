using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Epam.DependencyResolver;

namespace Epam.ASPNet.PL
{
	public static class StaticElements
	{
		private static Resolver resolver;
		private static CommonLogger logger;
		
		public static CommonLogger GetLogger() => logger ?? (logger = new CommonLogger());
		public static Resolver GetResolver() => resolver ?? (resolver = new Resolver(GetLogger()));
	}
}