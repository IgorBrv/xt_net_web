using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Epam.DependencyResolver;

namespace Epam.ASPNet.PL.Models
{
	public static class StaticElements
	{
		private static Resolver resolver;
		
		public static Resolver GetResolver() => resolver ?? (resolver = new Resolver());
	}
}