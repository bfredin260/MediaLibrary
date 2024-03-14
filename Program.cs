using NLog;

// See https://aka.ms/new-console-template for more information
string path = Directory.GetCurrentDirectory() + "//nlog.config";

// create instance of Logger
var logger = LogManager.LoadConfiguration(path).GetCurrentClassLogger();
Console.Clear();
logger.Info("Program started");

string userChoice;
do {
    do {
        Console.Write("\n CHOOSE:\n---------\n1) Add Movie\n2) Display All Movies\n\nEnter to quit\n\n> ");
        userChoice = getUserChoice();

        logger.Info("User choice: \"{0}\"", userChoice);        
    } while (!verifyUserChoice(userChoice) && userChoice != "");

} while (!verifyUserChoice(userChoice));

logger.Info("Program ended");

string getUserChoice() {
    try {
        userChoice = Console.ReadLine()
                                .ToCharArray()[0]
                                .ToString()
        ;
    } catch (IndexOutOfRangeException) {
        userChoice = "";
    }

    return userChoice;
}

bool verifyUserChoice(string input) {
    bool returnValue = false;

    if(input == "1" || input == "2" || input == "") {
        returnValue = true;
    } else {
        logger.Error("\nInvalid input: \"{0}\"\n", input);
    }

    return returnValue;
}