using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AllanGray.Logic.Interfaces;
using System.IO;
using log4net;
namespace AllanGray.TwitterSubscriberManager
{
    public class TwitterSubscriberManager : ITwitterSubscriberManager
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType); 

        //i didn't want to use a specialist graph structure for this so i just implemented an adjacency matrix using a dictionary of a list.
        Dictionary<string, HashSet<string>> _followsGraph = new Dictionary<string, HashSet<string>>(); 
        Dictionary<string, HashSet<string>> _followedByGraph = new Dictionary<string, HashSet<string>>();
        //I use a hashset for superfast lookups because I'm assuming a potential scaling problem
        //also i assume that at a minimum the userlist can fit into available memory.

        //just for internal use
        HashSet<string> _userList = new HashSet<string>();
        string _fileName;
        

        public TwitterSubscriberManager()
        {
            Initialize();
        }
        private void Initialize()
        {
            var args = Environment.GetCommandLineArgs();
            log.DebugFormat("Command line count is {0}", args.Length);
            if (args.Length == 3)
            {
                _fileName = args[1];                
                //there are some race conditions that might arise if you check for file existance, READ privilege etc. just throw the exception and get over it.
                LoadFromFile(_fileName);
            }
            else
            {
                throw new ArgumentException("This component requires that you pass 2 parameters to the command line. Eg [program name] <user.txt file> <tweets.txt file>");
            }            
        }
        public IEnumerable<string> GetUsers()
        {
            return _userList.OrderBy(s => s);
        }

        public void LoadFromFile(string fileName)
        {                       
            using (TextReader reader = File.OpenText(fileName))
            {                        
                string user;
                string followers;
                string line = reader.ReadLine();
                while (!string.IsNullOrEmpty(line))
                {
                    try
                    {
                        log.DebugFormat("Read input line: {0}", line); //maybe you want to see on which line it fails, can always tone down/up the verbosity in config.
                        line = line.Replace(" follows ", "|");
                        user = line.Split('|')[0];
                        Follow(user, user);
                        followers = line.Split('|')[1];
                        if (!_userList.Contains(user))
                            _userList.Add(user.Trim());

                        foreach (string follower in followers.Split(','))
                        {
                            Follow(follower, user);
                            if (!_userList.Contains(follower.Trim()))
                                _userList.Add(follower.Trim());
                        }
                        line = reader.ReadLine();
                    }
                    catch (Exception ex)
                    {
                        log.ErrorFormat("Processing that last line caused an exception but we'll continue anyway. {0}", ex);
                        continue;
                    }                                        
                }
            }//stream is automatically closed
        }

        public void Follow(string follower, string user)
        {
            if (string.IsNullOrEmpty(follower))
                throw new ArgumentException("follower cannot be null or empty.");

            if (string.IsNullOrEmpty(user))
                throw new ArgumentException("user cannot be null or empty.");

            follower = follower.Trim();
            user = user.Trim();

            if (!_followsGraph.ContainsKey(user))
            {
                _followsGraph.Add(user, new HashSet<string>());
            }

            if (!_followsGraph[user].Contains(follower))
                _followsGraph[user].Add(follower); //OK

            if (!_followedByGraph.ContainsKey(user))
            {
                _followedByGraph.Add(user, new HashSet<string>());
            }
            if (!_followedByGraph[user].Contains(follower))
                _followedByGraph[user].Add(follower);
        }

        public HashSet<string> GetListOfFollowsByUser(string user)
        {
            if (_followsGraph.ContainsKey(user))
                return _followsGraph[user];
            return new HashSet<string>();

        }        
        public HashSet<string> GetFollowersOfUser(string user)
        {
            if (_followedByGraph.ContainsKey(user))
                return _followedByGraph[user];
            return new HashSet<string>();
        }

    }

}
