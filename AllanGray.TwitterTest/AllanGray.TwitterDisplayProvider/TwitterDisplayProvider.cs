using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AllanGray.Logic.Interfaces;
using AllanGray.Logic.Models;
using log4net;
using System.IO;

namespace AllanGray.TwitterDisplayProvider
{
    public class TwitterDisplayProvider : ITwitterDisplayProvider
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType); 
        public void DisplayTweets(IEnumerable<Tweet> tweets)
        {            
            foreach (Tweet tweet in tweets)
            {
                try
                {
                    log.DebugFormat("Displaying tweet: {0}", tweet); //maybe you want to see which tweet might cause a failure?                    
                    Console.WriteLine("\t@{1}: {0}", tweet.Text, tweet.User); //now that we have the tweet, just display it to console as per requirement. 
                    //I could generalise further and write to any stream.
                }
                catch (Exception ex)
                {
                    log.ErrorFormat("Processing that last tweet caused an exception but we'll continue anyway. {0}",ex);
                    continue;
                }                
            }
        }
    }

}
