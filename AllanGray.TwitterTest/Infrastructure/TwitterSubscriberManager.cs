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
	using System.IO;
	using AllanGray.TwitterTest.Interface;
	using System.Collections.Generic;
	using System.Linq;

	public class TwitterSubscriberManager:ITwitterSubscriberManager
	{
		Dictionary<string, HashSet<string>> _followsGraph = new Dictionary<string, HashSet<string>> ();
		Dictionary<string, HashSet<string>> _followedByGraph = new Dictionary<string, HashSet<string>> ();
		//just for internal use
		HashSet<string> _userList = new HashSet<string> ();
		//I use a hashset for superfast lookups because I'm assuming a potential scaling problem

		public TwitterSubscriberManager ()
		{

		}

		public IEnumerable<string> GetUsers ()
		{
			return _userList.OrderBy (s => s);
		}

		public void LoadFromFile (string fileName)
		{
			using (StreamReader sr = new StreamReader (fileName)) {
				string user;
				string followers;
				string line = sr.ReadLine ();
				while (!string.IsNullOrEmpty (line)) {
					line = line.Replace (" follows ", "|");
					user = line.Split ('|') [0];
					Follow (user, user);
					followers = line.Split ('|') [1];
					if (!_userList.Contains (user))
						_userList.Add (user.Trim ());

					foreach (string follower in followers.Split(',')) {
						Follow (follower, user);
						if (!_userList.Contains (follower.Trim ()))
							_userList.Add (follower.Trim ());
					}
					line = sr.ReadLine ();
				}
			}//stream is automatically closed
		}

		public void Follow (string follower, string user)
		{
			if (string.IsNullOrEmpty (follower))
				throw new ArgumentException ("follower cannot be null or empty.");

			if (string.IsNullOrEmpty (user))
				throw new ArgumentException ("user cannot be null or empty.");

			follower = follower.Trim ();
			user = user.Trim ();

			if (!_followsGraph.ContainsKey (user)) {
				_followsGraph.Add (user, new HashSet<string> ());
			}

			if (!_followsGraph [user].Contains (follower))
				_followsGraph [user].Add (follower); //OK

			if (!_followedByGraph.ContainsKey (user)) {
				_followedByGraph.Add (user, new HashSet<string> ());
			}
			if (!_followedByGraph [user].Contains (follower))
				_followedByGraph [user].Add (follower);
		}

		public HashSet<string> GetListOfFollowsByUser (string user)
		{
			if (_followsGraph.ContainsKey (user))
				return _followsGraph [user];
			return new HashSet<string> ();

		}

		public HashSet<string> GetFollowersOfUser (string user)
		{
			if (_followedByGraph.ContainsKey (user))
				return _followedByGraph [user];
			return new HashSet<string> ();
		}

	}
}

