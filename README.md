tldr; Find install instructions at the end of this file.
# Purpose of this project
This application is a coding test. It has the simple objective to display a list of tweets per user sourced from a text file. It has to be __production quality__.

# Things I considered in the design of the application

First, you must know that the content of this test and all of the source code submitted here is open source, as in fully searchable by anyone who knows even a little about how to use github. In that respect I was able to draw on many other peoples' attempt at implementing the same requirement. There are quite a number of different approaches that were adopted, each expressing the chosen language of implementation in its own unique way.

Ultimately a good design should be agnostic of the language it is implemented in. In reality though not many languages support the concepts of reflection, interfaces and are strongly-typed at the same time. So some of the attempts I studied took the limits of duck-typed languages such as `Python` and `Ruby` to their limits without allowing the developer to fully express her knowledge of `SOLID`, `Dependency Injection` and other constructs typically found in the `Enterprise Java` and `.NET` spaces.

Since the screening interview had a lot of questions about the latter concepts I determined that it would be prudent to select a language that allowed the expression of these ideas. At the same time I had to watch out for trying to over-engineer an approach to solving a simple business requirement. So I had to make a few __key assumptions__ which due to the nature of the test format could not be tested in the typical back and forth one finds in an actual production software construction environment.

# Key assumptions and design considerations
While I could have written a single method to fulfill the requirement of this test, I felt I had to marry this up with what the filter-interview probed me about. Therefore I felt some feature targets needed to be developed.
 - The main thing is to recognise that the domain of problem is spread across three distinct areas, namely providing a list of tweets, providing a graph of users/followers and displaying in both the proper sequence and filter that list of tweets. (Here I demonstrate segregating the domain into distinct areas of concern where each class imeplementing some interface is singularly responsible for that domain - the `S` in `SOLID` - single responsibility).
 - I defined three interfaces for these seperate components with the intention that all three classes should be able to communicate with each other only through the defined interfaces. (Here I demonstrate the `L.I.D` in `SOLID` - Liskov substition, Interface Segregation and Dependency inversion).
 - Typically when I see a requirement to read a list from a file I expect said file size to exceed the amount of available memory and therefore opted to read the file line by line. Therefore I designed the application to be able to work for a __20Gb__ source text file just as good as a __20kb__ file. (For this component at least).
 - At the same time the overall architecture of the application is written in such a way that component which provides the list of tweets which need to be displayed doesn't need to know where the tweets come from, and therefore the former component may just as well be written to source the tweets from a database or online.
 - I did this for all of the components. The modular design totally leaves open the possibility that the tweet renderer can output its source feed to an html formatted file as to an ansi-enabled screen without changing any other component. The component providing the social graph of followers/users may be sourced from the actual twitter API without regard to how the tweets are displayed nor sourced.
 
# Install instructions
My development environment was the following:
 - Visual Studio 2012 (Fully compatible and tested under Ubuntu 15.10 with Monodevelop / post-build event doesn't work - just copy the components into bin of TwitterTest application)
 - At least C# 3.5 (uses Linq ,Generic Collections, HashSet)
 - Nuget (to load log4net and Autofac and nunit)
 - In `.gitignore` file some standard ignores are present. So don't expect to find the executables on gitgub.
 - What's excluded is the contents of the `\packages` folder, so when the solutions loads for the first time be sure to allow nuget to download depedencies.
 
# Compilation instructions
 - Allow nuget to download the package dependencies.
 - Get the solution to compile first. There are a few post-build events which will trigger and copy the components to the application which serves as the solution entry point.
 
# Running instructions
 - The entry point is the project AllanGray.TwitterTestConsole
 - Compilation will trigger copy of the user.txt and tweet.txt files to the console application.
 - There is a command line configuration set in the IDE so that when you simply run the console application the input filenames will be read from command line.
 - Off the console application a `\log` file folder will be written. The default log-level is INFO. If you change it to DEBUG you will generate extremely verbose information which will be helpful in production fault-finding.  
 - In the `app.config` file of the TwitterTestConsole project there is a key called `infrastructure_assemblies`. In a Continuos Build + Integration environment the contents of this key would typically point to `Mock` versions of the components/services. I left the DummyTweetProvider project as an example. This component is capable of generating a huge number of tweets.
 - In keeping with the aim of planning for scale there is a python script added to let you generate a pretty big tweet file. In dev testing I generated a 300MB tweet file.
