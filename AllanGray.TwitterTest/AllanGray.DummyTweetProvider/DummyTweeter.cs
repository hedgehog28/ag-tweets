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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AllanGray.Logic.Interfaces;
using AllanGray.Logic.Models;
using System.IO;

namespace AllanGray.DummyTweetProvider
{
	public class DummyTweeter:ITwitterStreamProvider
	{
		public DummyTweeter ()
		{

		}

		public IEnumerable<Tweet> GetTweets (params string[] users)
		{
			for (int i = 0; i < 125000; i++) {
				foreach (string user in users) {
					yield return new Tweet (){ User = user, Text = i.ToString ("0000000000") };
				}
			}
		}
	}
}

