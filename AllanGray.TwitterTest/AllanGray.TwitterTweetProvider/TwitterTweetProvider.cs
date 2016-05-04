using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AllanGray.Logic.Interfaces;
using AllanGray.Logic.Models;
using System.IO;
using log4net;

namespace AllanGray.TwitterTweetProvider
{
	/// <summary>
	/// In this implementation the tweet provider interface repository reads the tweets from a file. It never stores all the tweets in memory but reads them line by line yielding a result. Obviously it does this
	/// for every user you want to print tweets for so it might read the tweet file multiple times. 
	/// I assume that in production the volume of tweets would be too many to store in memory at once.
	/// I could have written a version of the implementation of the interface where all the tweets are stored in memory too. And if I did I would just have to create a new repository considering that the classes have been
	/// sufficiently loosely coupled/high cohesion that they communicate via interfaces only. ie. For such a change the DisplayProvider class wouldn't have to be updated too.
	/// </summary>
	public class TwitterStreamProvider : ITwitterStreamProvider
	{
		private static readonly ILog log = LogManager.GetLogger (System.Reflection.MethodBase.GetCurrentMethod ().DeclaringType);
        
		string _fileName;

		public TwitterStreamProvider ()
		{			
			Initialize ();
		}

		public void Initialize ()
		{
			//ok a lit bit of copy and paste here.
			var args = Environment.GetCommandLineArgs ();
			log.DebugFormat ("Command line count is {0}", args.Length);
			if (args.Length == 3) {
				_fileName = args [2];
				//there are some race conditions that might arise if you check for file existance, READ privilege etc. just throw the exception and get over it.                
			} else {
				var tweetFile = Environment.GetEnvironmentVariable("tweetfile");
				if (!string.IsNullOrEmpty(tweetFile)) //just so that the unit tests will work
				{
					_fileName = tweetFile;
					return;
				}
				throw new ArgumentException ("This component requires that you pass 2 parameters to the command line. Eg [program name] <user.txt file> <tweets.txt file>");
			}            
		}

		public IEnumerable<Tweet> GetTweets (params string[] users)
		{
			using (TextReader sr = File.OpenText (_fileName)) {
				string user;
				string text;
				string line = sr.ReadLine ();                
				while (!string.IsNullOrEmpty (line)) {
					log.DebugFormat ("Read input line: {0}", line); //maybe you want to see on which line it fails, can always tone down/up the verbosity in config.
					if (!line.Contains('>'))
						log.Error("No delimiter found, continuing with next line.");
					user = line.Split ('>') [0].Trim();
					text = line.Split ('>') [1].Trim();
					if (string.IsNullOrEmpty(text) || string.IsNullOrEmpty(user))
					{
						log.Error("That last line was unusable, continuing anyway.");
						continue;
					}
					if (text.Length > 140)
					{					
						log.Error("Tweet text longer than 140 characters. Trimming instead.");
						text = text.Substring(0,140);
					}
					//var intersect = text.Split ().Intersect (users.Select (x => "@" + x)); //in case someone sends you a tweet
					if (users.Contains (user))
						yield return new Tweet () { Text = text, User = user };
					line = sr.ReadLine ();					                                
				}                                
			}
		}
	}

}
