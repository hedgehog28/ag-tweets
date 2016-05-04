using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using NUnit;
using AllanGray.Technology;
using System.Reflection;
using AllanGray.Logic.Models;
using AllanGray.Logic.Interfaces;
using AllanGray.TwitterTestConsole;

namespace AllanGray.UnitTests
{
	[TestFixture]
	public class AppTests
	{

		public AppTests()
		{

		}
		[Test]
		public void TestLocatorPatternEndToEnd()
		{
			Environment.SetEnvironmentVariable("tweetfile","tweet.txt");
			Environment.SetEnvironmentVariable("userfile","user.txt");
			var rendererAssembly = Assembly.LoadFrom("AllanGray.TwitterDisplayProvider.dll");
			var subscriberManagerAssembly = Assembly.LoadFrom("AllanGray.TwitterSubscriberManager.dll");
			var tweetProviderAssembly = Assembly.LoadFrom("AllanGray.TwitterTweetProvider.dll");
			RuntimeContext.Configure(rendererAssembly,subscriberManagerAssembly, tweetProviderAssembly);
			var displayProvider = RuntimeContext.Resolve<ITwitterDisplayProvider>();
			var subscriberManager = RuntimeContext.Resolve<ITwitterSubscriberManager>();
			var tweetProvider = RuntimeContext.Resolve<ITwitterStreamProvider>();
			AllanGrayRunner runner = new AllanGrayRunner(subscriberManager, tweetProvider, displayProvider);
			runner.Run();
		}
	}
}
