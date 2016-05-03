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
using AllanGray.Technology;
using AllanGray.Logic.Interfaces;
using AllanGray.Logic.Models;
using Autofac;
using Autofac.Core;
using log4net;

namespace AllanGray.TwitterTest
{

	class MainClass
	{
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType); 
		public static void Main (string[] args)
		{			         
            /*
             *   Here I show several technologies in action namely Depedency Injection, in a broader sense Inversion of Control, 
             *  SOLID principles, and Domain Driven Design.
             * 
             *  Display Provider (It can also be called a Service or Component. I call it that a matter of personal choice -- easier to conceptualise it that way.)
             *  Its role is to accept a list of tweets and render it on some output device.
             *  Tweet Provider
             *  Its role is to output a list of tweets from some source
             *  Subscriber Manager
             *  Its role is to keep track of which user follows who and to provide methods of querying that information.               
             *  
             *  This application uses DI so that these different components are truly modular. Because the components are bound to an interface, the Providers are free
             *  to implement the interfaces in whatever way the business requirement will be met. This also opens up the modularity to be exploited by testing frameworks by
             *  allowing us to inject test-Providers and any combination thereof into the runtime context. Here the runtime context is 'set' by using the Autofac utility.
             *  
             *  Note that the design also allows us for example to change the way the Provider works and get the application to output the tweet list from console to say an html stream without
             *  having to recompile the whole thing, simple drop-in component and configuration update. So in other words for the three components I have identified we have 
             *  a plugin type architecture which will be very useful to debug the application in-flight.
             * 
            */
            
            try
            {
                RuntimeContext.Configure("infrastructure_assemblies"); //start of the service locator pattern                                    
                var displayProvider = RuntimeContext.Resolve<ITwitterDisplayProvider>(); //just a console output implementation
                var tweetStreamProvider = RuntimeContext.Resolve<ITwitterStreamProvider>(); //source the input tweets from a file
                var subscriberManager = RuntimeContext.Resolve<ITwitterSubscriberManager>(); //source the follower/follows graph from a file
                AllanGrayRunner run = new AllanGrayRunner(subscriberManager, tweetStreamProvider, displayProvider);
                run.Run();
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw;
            }
            Console.ReadKey();
		}
	}
}
