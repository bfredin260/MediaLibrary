﻿using System.Text.RegularExpressions;
using NLog;

// See https://aka.ms/new-console-template for more information
string path = Directory.GetCurrentDirectory() + "//nlog.config";

// create instance of Logger
var logger = LogManager.LoadConfiguration(path).GetCurrentClassLogger();

Console.WriteLine();
logger.Info("Program started");

string scrubbedFile = FileScrubber.ScrubMovies("movies.csv");

Console.WriteLine();
logger.Info(scrubbedFile);

MovieFile movieFile = new MovieFile(scrubbedFile);

Console.WriteLine("\n\n   Media Library\n-------------------");

string userChoice;

do {
    userChoice = getUserChoice();

    if(userChoice != "") {

        if(userChoice == "1"){

            Movie movie = new Movie() {
                title = getMovieTitle(),
                genres = getMovieGenres(),
                director = getMovieDirector(),
                runningTime = getMovieRuntime()
            };

            movieFile.AddMovie(movie);

        } else if(userChoice == "2") {

            foreach(Movie movie in movieFile.Movies) {
                Console.WriteLine(movie.Display());
            }
        } else {

            string input = getUserInput("\n\n SEARCH:\n---------\nEnter title, or part of title:\n\n> ");

            foreach(Movie movie in movieFile.Movies) {
                if(movie.title.ToLower().Contains(input.ToLower())) {
                    Console.WriteLine("\n" + movie.Display());
                }
            }
        }
    }

} while (userChoice != "");

logger.Info("Program ended");


//  METHODS
// ---------
string getUserInput(string prompt) {
    Console.Write(prompt);

    return Console.ReadLine();
}

bool isValidChoice(string choice) {
    return choice == "1" || choice == "2" || choice == "3" || choice == "";
}

string getUserChoice() {
    string choice;
    bool valid;

    do {
        choice = getUserInput("\n\n CHOOSE:\n---------\n1) Add Movie\n2) Display All Movies\n3) Search for Movies\n\nEnter to quit\n\n> ");        

        if(choice != "") {
            choice = choice.ToCharArray()
                [0].ToString()
            ;
        }

        valid = isValidChoice(choice);

        Console.WriteLine();
        logger.Info("User choice: \"{0}\"", choice);

        Console.WriteLine();
        if (!valid) {
            logger.Error("Invalid input: \"{0}\"", choice);
        }
    } while (!valid);

    string selected = null;

    switch (choice) {
        case "1": 
            selected = "Add Movie";
            break;
        case "2":
            selected = "Display All Movies";
            break;
        case "3":
            selected = "Search for Movies";
            break;
        default:
            selected = "Quit Program";
            break;
    }
    logger.Info("Selected: {0}", selected);

    return choice;
}

string getMovieTitle() {
    string input;

    do {
        input = getUserInput("\n\n ADD MOVIE:\n------------\nEnter movie title\n\n> ");

        Console.WriteLine();
        if(input != "") {
            logger.Info("Added title \"{0}\" to movie", input); 
        } else {                
            logger.Error("Please input a value!");
        }
    } while(input == "");

    return input;
}

List<string> getMovieGenres() {
    string input = "";
    List<string> arr = new List<string>();

    // loops until input == "done"
    // uses i value for genre # in prompt.
    for(int i = 0; input != "done"; i++) {

        //loop if input is empty
        do {

            // gets genre string from user
            input = getUserInput(string.Format("\n\n ADD MOVIE:\n------------\nEnter genre #{0} (\"done\" to finish)\n\n> ", i + 1));

            Console.WriteLine();

            // if input is NOT empty OR "done"
            if(input != "" && input.ToLower() != "done") {
                // then add input to the list
                arr.Add(input);

                logger.Info("Added genre \"{0}\" to movie", input);
            
            // if input is "done"
            } else if(input.ToLower() == "done") {

                // then check if it is the first iteration
                if(i == 0) {

                    // first iteration means you need to input a genre
                    logger.Error("Please input AT LEAST one genre!");

                    // set input back to empty so that loop repeats
                    input = "";

                // if NOT the first iteration
                } else {

                    // then end loop
                    logger.Info("Finished adding genres");
                }
            // if input is empty
            } else if (input == "") {

                // alert user to enter a valid genre
                logger.Error("Please input a genre!");
            }
        } while (input == "");
    }

    return arr;
}

string getMovieDirector() {
    string input;

    do {
        input = getUserInput("\n\n ADD MOVIE:\n------------\nEnter movie director\n\n> ");

        Console.WriteLine();
        if(input != "") {
            logger.Info("Added director \"{0}\" to movie", input);
        } else {
            logger.Error("Please input a director");
        }
    } while (input == "");

    return input;
}

bool isValidRuntime(string input) {
    // returns true if input MATCHES regex (hh:mm:ss), returns false otherwise;
    return new Regex(@"^([01]?\d|2[0-3]):([0-5]?\d):([0-5]?\d)$")
                    .IsMatch(input)
    ;
}

TimeSpan getMovieRuntime() {
    string input;
    bool valid;
    int[] hms = new int[3];

    do {
        input = getUserInput("\n\n ADD MOVIE:\n------------\nEnter movie running time (h:m:s)\n\n> ");
        valid = isValidRuntime(input);

        Console.WriteLine();
        if(!valid) {
            logger.Error("Please enter a valid running time!");
        }
    } while (!valid);

    string[] hmsStrings = input.Split(":");

    for (int i = 0; i < hmsStrings.Length; i++) {
        hms[i] = int.Parse(hmsStrings[i]);
    }

    int h = hms[0];
    int m = hms[1];
    int s = hms[2];

    logger.Info("Added runtime of \"{0} hours, {1} minutes, {2} seconds\" to movie", h, m, s);

    return new TimeSpan(h, m, s);
}
