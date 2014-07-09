MarkdownLog
===========

MarkdownLog is a .NET component for generating Markdown elements from common data structures. It can be used to provide diagnostic logs that are human readable in their raw form, yet capable of easily being transformed into HTML and other formats for reporting and documentation purposes. The binary is a lightweight (< 100KB), portable class library (PCL) DLL that requires no additional dependencies besides the .NET framework. 

[![Build status](https://ci.appveyor.com/api/projects/status/k15974djmygb9f13)](https://ci.appveyor.com/project/Wheelies/markdownlog)

Table example
-------------

    var data = new[]
    {
        new{Year = 1991, Album = "Out of Time", Songs=11, Rating = "* * * *"},
        new{Year = 1992, Album = "Automatic for the People", Songs=12, Rating = "* * * * *"},
        new{Year = 1994, Album = "Monster", Songs=12, Rating = "* * *"}
    };

    Console.Write(data.ToMarkdownTable().ToMarkdown());
    
    // Produces:
    //
    //     Year | Album                    | Songs | Rating   
    //     ----:| ------------------------ | -----:| --------- 
    //     1991 | Out of Time              |    11 | * * * *  
    //     1992 | Automatic for the People |    12 | * * * * *
    //     1994 | Monster                  |    12 | * * *    


Bar Chart example
----------------

    var worldCup = new Dictionary<string, int>
    {
        {"Brazil", 5},
        {"Italy", 4},
        {"Germany", 3},
        {"Argentina", 2},
        {"Uruguay", 2},
        {"France", 1},
        {"Spain", 1},
        {"England", 1}
    };

    Console.Write(worldCup.ToMarkdownBarChart().ToMarkdown());
    
    // Produces:
    //
    //    Brazil    |#####  5
    //    Italy     |####  4
    //    Germany   |###  3
    //    Argentina |##  2
    //    Uruguay   |##  2
    //    France    |#  1
    //    Spain     |#  1
    //    England   |#  1
    //              ------


Numbered list example 
---------------------

    var planets = new[] { "Mercury", "Venus", "Earth", "Mars", "Jupiter", "Saturn", "Uranus", "Neptune"};
    Console.Write(planets.ToMarkdownNumberedList().ToMarkdown());
    
    // Produces:
    //
    //    1. Mercury
    //    2. Venus
    //    3. Earth
    //    4. Mars
    //    5. Jupiter
    //    6. Saturn
    //    7. Uranus
    //    8. Neptune



The library was originally developed by BlackJet Software to output the results and observations of a fluent testing suite for an [iOS application](http://shoppingukapp.com/). The output of this suite included data structures that required manual inspection. The Markdown format was chosen because it can be produced and read without needing additional 3rd party libraries yet it is capable of representing rich data structures, such as lists, bar charts and tables, that can be easily converted into HTML for inclusion in professional quality reports.

MarkdownLog can produce all the standard Markdown features described in [John Gruber's original document](http://daringfireball.net/projects/markdown/); such as Headings, word-wrapped paragraphs, bulleted and numbered lists, blockquotes; in addition to GitHub flavoured tables, and representations of other data structures that are useful for outputting to diagnostic logs, such as bar charts and iOS style UIListView representations.

