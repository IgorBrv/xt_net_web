using Dependencies;

namespace PL.Webpages.models
{
	public static class StaticResolver
	{
		private static Resolver resolver = null;
		public static Resolver Get() => resolver ?? (resolver = new Resolver());
	}
}