//
//  Copyright 2016  marmite
//
//    Licensed under the Apache License, Version 2.0 (the "License");
//    you may not use this file except in compliance with the License.
//    You may obtain a copy of the License at
//
//        http://www.apache.org/licenses/LICENSE-2.0
//
//    Unless required by applicable law or agreed to in writing, software
//    distributed under the License is distributed on an "AS IS" BASIS,
//    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//    See the License for the specific language governing permissions and
//    limitations under the License.

namespace AllanGray.TwitterTest.Infrastructure
{

	using System;
	using AllanGray.TwitterTest.Infrastructure;
	using AllanGray.TwitterTest.Interface;
	using System.Collections;
	using System.Collections.Generic;
	using AllanGray.TwitterTest.Model;
	using System.IO;
	using System.Linq;


	/// <summary>
	/// In this implementation the tweet provider interface repository reads the tweets from a file. It never stores all the tweets in memory but reads them line by line yielding a result. Obviously it does this
	/// for every user you want to print tweets for so it might read the tweet file multiple times. 
	/// I assume that in production the volume of tweets would be too many to store in memory at once.
	/// I could have written a version of the implementation of the interface where all the tweets are stored in memory too. And if I did I would just have to create a new repository considering that the classes have been
	/// sufficiently loosely coupled/high cohesion that they communicate via interfaces only. ie. For such a change the DisplayProvider class wouldn't have to be updated too.
	/// </summary>
	public class TwitterRepositoryStreamProvider: ITwitterStreamProvider
	{
		string _fileName;

		public TwitterRepositoryStreamProvider (string fileName)
		{
			_fileName = fileName;
		}

		public IEnumerable<Tweet> GetTweets (params string[] users)
		{
			using (StreamReader sr = new StreamReader (_fileName)) {
				string user;
				string text;
				string line = sr.ReadLine ();
				while (!string.IsNullOrEmpty (line)) {
					user = line.Split ('>') [0];
					text = line.Split ('>') [1];
					//var intersect = text.Split ().Intersect (users.Select (x => "@" + x)); //in case someone sends you a tweet
					if (users.Contains (user))
						yield return new Tweet (){ Text = text.Trim (), User = user.Trim () };
					line = sr.ReadLine ();
				}					
			}
		}
	}
}