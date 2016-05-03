using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AllanGray.Logic;
using AllanGray.Logic.Interfaces;
using AllanGray.Logic.Models;

namespace AllanGray.TwitterTest
{
    public class AllanGrayRunner
    {
        public readonly ITwitterDisplayProvider Renderer;
        public readonly ITwitterStreamProvider TweetProvider;
        public readonly ITwitterSubscriberManager SubscriberManager;
        
        public AllanGrayRunner(ITwitterSubscriberManager subscriberManager, ITwitterStreamProvider tweetProvider, ITwitterDisplayProvider displayProvider)
        {
            Renderer = displayProvider;
            TweetProvider = tweetProvider;
            SubscriberManager = subscriberManager;
        }
        public void Run()
        {                        
            foreach (string user in SubscriberManager.GetUsers())
            {
                Console.WriteLine("\n{0}\n", user);
                Renderer.DisplayTweets(TweetProvider.GetTweets(SubscriberManager.GetListOfFollowsByUser(user).ToArray()));
            }
        }
        
    }
}
