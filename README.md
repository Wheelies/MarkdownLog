[![Build status](https://ci.appveyor.com/api/projects/status/k15974djmygb9f13)](https://ci.appveyor.com/project/Wheelies/markdownlog)

Markdown is a great format for representing an application's internal data structures for debugging and diagnostic purposes. It is a flexible format that is readable in its raw form yet capable of being transformed into HTML for documentation or reporting.

MarkdownLog can produce all features described in John Gruber's [original spec](http://daringfireball.net/projects/markdown/) from common .NET types.

Tables
------

This is how a table is built from a List of objects:

    var data = new[]
    {
        new{Year = 1991, Album = "Out of Time", Songs=11, Rating = "* * * *"},
        new{Year = 1992, Album = "Automatic for the People", Songs=12, Rating = "* * * * *"},
        new{Year = 1994, Album = "Monster", Songs=12, Rating = "* * *"}
    };

    Console.Write(data.ToMarkdownTable());
    
    // Produces:
    //
    //     Year | Album                    | Songs | Rating   
    //     ----:| ------------------------ | -----:| --------- 
    //     1991 | Out of Time              |    11 | * * * *  
    //     1992 | Automatic for the People |    12 | * * * * *
    //     1994 | Monster                  |    12 | * * *    

Once passed through a GitHub-flavoured parser, you get a HTML table, complete with headings and alignments:

    Year | Album                    | Songs | Rating   
    ----:| ------------------------ | -----:| --------- 
    1991 | Out of Time              |    11 | * * * *  
    1992 | Automatic for the People |    12 | * * * * *
    1994 | Monster                  |    12 | * * *    

Lists
-----

A collection can be output as a numbered list:

    var planets = new[] { "Mercury", "Venus", "Earth", "Mars", "Jupiter", "Saturn", "Uranus", "Neptune"};
    Console.Write(planets.ToMarkdownNumberedList());
    
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

When passed through a Markdown parser, this becomes:

   1. Mercury
   2. Venus
   3. Earth
   4. Mars
   5. Jupiter
   6. Saturn
   7. Uranus
   8. Neptune

Alternatively, the list can have bullets:

    var beatles = new[] { "John", "Paul", "Ringo", "George" };
    Console.Write(beatles.ToMarkdownBulettedList());
	
	//Produces:
	//
    //   * John
    //   * Paul
    //   * Ringo
    //   * George

This is parsed to:

   * John
   * Paul
   * Ringo
   * George


Bar Chart example
----------------

A barchart can be produced from a collection of KeyValue or Tuple objects

    var worldCup = new Dictionary<string, int>
    {
        {"Brazil", 5},
        {"Italy", 4},
        {"Germany", 4},
        {"Argentina", 2},
        {"Uruguay", 2},
        {"France", 1},
        {"Spain", 1},
        {"England", 1}
    };

    Console.Write(worldCup.ToMarkdownBarChart());
    
    // Produces:
    //
    //    Brazil    |#####  5
    //    Italy     |####  4
    //    Germany   |####  4
    //    Argentina |##  2
    //    Uruguay   |##  2
    //    France    |#  1
    //    Spain     |#  1
    //    England   |#  1
    //              ------

Bar charts are not supported by standard Markdown. When a barchart is passed through a Markdown parser, it is rendered as a code block that retains its structure:

    Brazil    |#####  5
    Italy     |####  4
    Germany   |####  4
    Argentina |##  2
    Uruguay   |##  2
    France    |#  1
    Spain     |#  1
    England   |#  1
              ------

A bar chart can be produced from floating point and negative numbers and scaling can be applied as desired:


    const int valueCount = 20;
    var chart = new BarChart
    {
        ScaleAlways = true,
        MaximumChartWidth = 40,
        DataPoints = from i in Enumerable.Range(0, valueCount)
            let rad = (i * 2.0 * Math.PI) / valueCount
            select new BarChartDataPoint
            {
                CategoryName = string.Format("Cos({0:0.0})", rad),
                Value = Math.Cos(rad)
            }
    };

This produces:

    Cos(0.0)                     |####################  1
    Cos(0.3)                     |###################  0.95
    Cos(0.6)                     |################  0.81
    Cos(0.9)                     |############  0.59
    Cos(1.3)                     |######  0.31
    Cos(1.6)                     |  0
    Cos(1.9)               ######|  -0.31
    Cos(2.2)         ############|  -0.59
    Cos(2.5)     ################|  -0.81
    Cos(2.8)  ###################|  -0.95
    Cos(3.1) ####################|  -1
    Cos(3.5)  ###################|  -0.95
    Cos(3.8)     ################|  -0.81
    Cos(4.1)         ############|  -0.59
    Cos(4.4)               ######|  -0.31
    Cos(4.7)                     |  0
    Cos(5.0)                     |######  0.31
    Cos(5.3)                     |############  0.59
    Cos(5.7)                     |################  0.81
    Cos(6.0)                     |###################  0.95
             -----------------------------------------

Paragraphs
----------

Strings can be written as a word-wrapped paragraph:

    var text ="Lolita, light of my life, fire of my loins. My sin, my soul. Lo-lee-ta: the tip of the tongue taking a trip of three steps down the palate to tap, at three, on the teeth. Lo. Lee. Ta.";
    
    Console.Write(text.ToMarkdownParagraph());

    // Produces:
	//
	// Lolita, light of my life, fire of my loins. My sin, my soul. Lo-lee-ta: the tip 
    // of the tongue taking a trip of three steps down the palate to tap, at three, on 
    // the teeth. Lo. Lee. Ta.

After parsing, this becomes:

Lolita, light of my life, fire of my loins. My sin, my soul. Lo-lee-ta: the tip 
of the tongue taking a trip of three steps down the palate to tap, at three, on 
the teeth. Lo. Lee. Ta.

---

MarkdownLog was originally developed by [BlackJet Software](http://blackjetsoftware.com) to produce a report of performance test results for an [iOS application](http://shoppingukapp.com/). It is maintained by [Wheelies](https://github.com/Wheelies)