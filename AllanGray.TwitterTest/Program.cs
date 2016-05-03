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
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using AllanGray.TwitterTest.Model;
using AllanGray.TwitterTest.Interface;
using AllanGray.TwitterTest.Infrastructure;


namespace AllanGray.TwitterTest
{

	class MainClass
	{
		public static void Main (string[] args)
		{
			#region Sub Manager
			TwitterSubscriberManager ts = new TwitterSubscriberManager ();
			ts.LoadFromFile (args [0]);
			#endregion

			#region Tweet manager
			TwitterRepositoryStreamProvider r = new TwitterRepositoryStreamProvider (args [1]);
			#endregion

			#region Tweet Display
			TwitterDisplayProvider displayProvider = new TwitterDisplayProvider ();
			foreach (string user in ts.GetUsers()) {
				Console.WriteLine ("\n{0}\n", user);						
				displayProvider.DisplayTweets (r.GetTweets (ts.GetListOfFollowsByUser (user).ToArray ()));				
			}
			#endregion
		}
	}
}
