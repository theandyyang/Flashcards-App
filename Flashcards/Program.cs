using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;
/// <summary>
/// Program Name: Flashcards App (temporary name)
/// Program Description: A program that saves and reads 
/// flashcards for the user. 
/// Authors: Ryan Samarajeewa, Andy Yang
/// </summary>
namespace Flashcards
{
    class Program
    {

        /// <summary>
        /// Get the directory of the program and return 
        /// a string containing the "\cards" directory.
        /// </summary>
        /// <returns></returns>
        static string getUserDirectory()
        {
            string path = Directory.GetCurrentDirectory();
            if (path.Contains("bin\\Debug"))
            {
                path = path.Replace("bin\\Debug", "cards\\");
            }
            return path;

        }

        /// <summary>
        /// Make an IList of Card objects to allow for 
        /// traversing through each card group.
        /// </summary>
        /// <returns></returns>
        static IList<Card>[] makeList()
        {
            //get card group paths and store it in a string array
            string[] folders = Directory.GetDirectories(getUserDirectory());

            //number of card groups the user has
            int numGroups = folders.Length;
            
            /** FOR TESTING
            foreach(string folder in folders)
            {
                Console.WriteLine(folder);
            }
            */

            //Create an array of ILists. The size of the array depends on the number of card groups the user has.
            IList<Card>[] iListArray = new IList<Card>[numGroups];
            
            for(int i = 0; i<iListArray.Length; i++)
            {
                var type = Type.GetType(typeof(List<Card>).AssemblyQualifiedName);
                var list = (IList<Card>)Activator.CreateInstance(type);
                iListArray[i] = list;
            }
            //get cards from each card group
            for (int i = 0; i < numGroups; i++)
            {
                
                string[] fileEntries = System.IO.Directory.GetFiles(folders[i]);//.txt file paths of current card group
                int numCards = fileEntries.Length;//number of cards in the folder

                /** FOR TESTING
                foreach (string file in fileEntries)
                {
                    Console.WriteLine(file);
                }
                */

                for (int j = 0; j < numCards; j++)
                {
                    Card tempCard = makeCardObject(fileEntries[j]);//make a card from the current file
                    tempCard.belongsTo = folders[i];

                    try
                    {
                        iListArray[i].Add(tempCard);//add the card to the iList array's appropriate index
                    }catch(NullReferenceException e)
                    {
                        Console.WriteLine("Exception when adding card to IList");
                    }
                    
                }

            }
            return iListArray;

        }

        /// <summary>
        /// Reads a .txt file, populates
        /// a Card object using its content 
        /// and returns it.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        static Card makeCardObject(string path)
        {
            Card newCard = new Card();
            int counter = 1;
            string line;

            // Read the file and add each line to the Card.
            System.IO.StreamReader file = new System.IO.StreamReader(path);
            while ((line = file.ReadLine()) != null)
            {
                newCard.writeToSide(counter, line);
                counter++;
            }

            file.Close();
            return newCard;
        }

        //main
        static int Main(string[] args)
        {
     
            IList<Card>[] cardsCollection = makeList();//Make the IList array containing the user's card collection

            //loop to allow for continuous usage
            while (true)
            {
                Console.Clear();
                //main menu
                Console.WriteLine("-----Flashcards App LINKED LIST SOLUTION-----\n");
                Console.WriteLine("What would you like to do?");
                Console.WriteLine("\n\t1: Create card group");
                Console.WriteLine("\n\t2: Display card groups");
                Console.WriteLine("\n\t3: Delete card group");
                Console.WriteLine("\n\t4: Create card");
                Console.WriteLine("\n\t5: Read cards");
                Console.WriteLine("\n\t6: Delete card");
                Console.WriteLine("\n\t7: Exit program\n");
                Console.WriteLine("---------------------------------------------");

                //get user's command
                string choice = Console.ReadLine();

                //switch cases to lead to different functions
                switch (choice)
                {

                    case "1":
                        createCardGroup();
                        cardsCollection = makeList();
                        break;
                    case "2":
                        Console.Clear();
                        displayCardGroups(true);
                        break;
                    case "3":
                        deleteCardGroup();
                        cardsCollection = makeList();
                        break;
                    case "4":
                        createCard();
                        break;
                    case "5":
                        readCards(cardsCollection);
                        cardsCollection = makeList();
                        break;
                    case "6":
                        deleteCard();
                        break;
                    case "7":
                        return 1;//terminate program
                    default:
                        Console.WriteLine("Invalid command. Please try again: ");
                        break;

                }

            }

        }

        /// <summary>
        /// Creates a new card group within /cards.
        /// </summary>
        static void createCardGroup()
        {
            Console.WriteLine("What would you like the new flashcard group to be called?");
            string folderName = Console.ReadLine();
            string path = getUserDirectory() + folderName;

            try
            {
                // Determine whether the directory exists. 
                if (Directory.Exists(path))
                {
                    Console.WriteLine("That path exists already.");
                    return;
                }

                // Try to create the directory.
                DirectoryInfo di = Directory.CreateDirectory(path);

            }
            catch (Exception e)
            {
                Console.WriteLine("The process failed: {0}", e.ToString());
            }
            finally { }

            //inform user of successful card group creation
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("The card group ");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write(folderName);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write(" was created succesfully. Press any key to continue.");
            Console.ResetColor();
            Console.ReadKey();
        }

        /// <summary>
       /// Displays the directories("card groups") within /cards.
       /// </summary>
        static void displayCardGroups(bool calledFromMain)
        {
            Console.WriteLine("--------------Your card groups---------------");
            string targetDirectory = getUserDirectory();

            // Process the list of folders found in the directory. 
            string[] folders = Directory.GetDirectories(targetDirectory);
            foreach (string folder in folders)
                ProcessFolder(folder);

            Console.WriteLine("---------------------------------------------");

            //if the function was called from main, allow the user to return to the main menu
            if (calledFromMain)
            {
                Console.WriteLine("Press any key to continue.");
                Console.ReadKey();
            }

            
        }

        /// <summary>
        /// Method for formatting and printing 
        /// the card group names (used in displayCardGroups()).
        /// </summary>
        /// <param name="path"></param>
        static void ProcessFolder(string path)
        {
            path = path.Replace(getUserDirectory(), "");
            Console.WriteLine(" - {0}", path);
        }
        
        /// <summary>
        /// Deletes a specified directory within /cards.
        /// This will also delete all text files (cards) within the 
        /// card group.
        /// </summary>
        static void deleteCardGroup()
        {
            Console.Clear();
            Console.WriteLine("Which card group would you like to delete?");
            displayCardGroups(false);
            string folderName = Console.ReadLine();
            string path = getUserDirectory() + folderName;
            bool deleted = true;

            try
            {
                // Determine whether the directory exists. 
                if (Directory.Exists(path) && !String.IsNullOrWhiteSpace(folderName))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write("WARNING: All cards within the group will be lost. ");
                    Console.ResetColor();
                    Console.Write("Are you sure you want to delete this card group? (y/n)");
                    string choice = Console.ReadLine();
                    if(choice.Equals("y")||choice.Equals("Y")){
                        Directory.Delete(path, true);
                    }
                    else
                    {
                        Console.WriteLine("No card groups were deleted.");
                        return;
                    }
                         
                }
                else
                {
                    Console.WriteLine("The specified card group does not exist.");
                    deleted = false;
                }

            }
            catch (Exception e)
            {
                Console.WriteLine("The process failed: {0}", e.ToString());
            }
            finally { }

            if (deleted)
            {
                //inform user about successful card group deletion
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write("The card group ");
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write(folderName);
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write(" was deleted succesfully. Press any key to continue.");
                Console.ResetColor();
                Console.ReadKey();
            }
            else
            {
                Console.WriteLine("Nothing was deleted. Press any key to contiune.");
                Console.ReadKey();
            }
            
        }

        /// <summary>
        /// Create a new text file (a card) containing
        /// two lines, which correspond to two sides of 
        /// a card.
        /// </summary>
        static void createCard()
        {
            int counter = 1;
            Console.Clear();
            while (true)
            {
                Console.WriteLine("Which card group would you like to modify?");
                displayCardGroups(false);
                string input = Console.ReadLine();
                string path = getUserDirectory() + input;
                if (Directory.Exists(path)&& !String.IsNullOrWhiteSpace(input))
                {
                    string[] cardContent = { "", "" };

                    //ask user to populate two sides of the card
                    Console.Write("What would you like to write on side 1?");
                    cardContent[0] = Console.ReadLine();

                    Console.Write("What would you like to write on side 2?");
                    cardContent[1] = Console.ReadLine();

                    //finds an integer that is not already used by another file
                    while (File.Exists(path + "\\" + counter + ".txt"))
                    {
                        counter++;
                    }

                    //create and write to new text file
                    System.IO.File.WriteAllLines(@path + "/" + counter + ".txt", cardContent);
                    break;
                }
                else
                {
                    Console.WriteLine("The specified card group does not exist.");
                }
            }

            //inform user about successful card group deletion
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("The card was created successcully. Press any key to continue.");
            Console.ResetColor();
            Console.ReadKey();
        }

        /// <summary>
        /// Allows the user to select a card group and
        /// read its cards. Also used for testing of makeList().
        /// </summary>
        static void readCards(IList<Card>[] collection)
        {
            string folderName;//name of the card group that the user wants to read
            string cardGroupPath;//the full path of a group in the cards/ directory

            //asks user to select a card group
            while (true)
            {
                Console.WriteLine("Which card group would you like to read?");
                displayCardGroups(false);
                folderName = Console.ReadLine();
                cardGroupPath = getUserDirectory() + folderName;

                //check if specified directory exists and contains at least one file. 
                if (Directory.Exists(cardGroupPath)&&!IsDirectoryEmpty(cardGroupPath)&& !String.IsNullOrWhiteSpace(folderName))
                {
                    break;
                }
                else
                {
                    Console.WriteLine("The specified card group does not exist or is empty. Please specify a valid card group.");
                }
   
             }

            int counter = 0;
            //find the correct IList in the collections array
            while (counter < collection.Length-1)
            {
                if (collection[counter].Count == 0)
                {
                    //Console.WriteLine("The list at index " + counter + " is empty.");
                }
                else
                {
                    //Console.WriteLine("The list at index " + counter + " is NOT empty.");
                    IList<Card> temp = collection[counter];
                    if (temp[0].belongsTo.Equals(cardGroupPath))
                    {
                        Console.WriteLine("Found the matching card group.");
                        break;
                    }
                }

                counter++;
            }

            IList<Card> toRead = collection[counter];//the card group to be read
            int numCards = toRead.Count;//number of cards in the group
            Random rand = new Random();//random number generator

            int read = 0;//number of cards that have been read
            while(read < numCards)
            {
                Card card = toRead[rand.Next(0, numCards)];
                if (card.viewed == false)
                {
                    Console.WriteLine(card.sideOne);
                    Console.ReadKey(true);
                    Console.WriteLine(card.sideTwo);
                    Console.WriteLine();
                    card.setToViewed();
                    read++;
                }
           
            }

            //notify user that the card group has been read
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("You have finished reading this card group. Press any key to return to the main menu.");
            Console.ResetColor();
            Console.ReadKey();

        }

        /// <summary>
        /// Allows the user to select a card group and 
        /// delete a card from it.
        /// </summary>
        static void deleteCard()
        {
            string folderName;
            string deleteCard;
            //select card group to modify
            while (true)
            {
                Console.WriteLine("Which card group would you like to modify?");
                displayCardGroups(false);
                folderName = Console.ReadLine();

                if (Directory.Exists(getUserDirectory()+folderName))
                {
                    break;
                }
                else
                {
                    Console.WriteLine("The specified card group does not exist. Please try again.");
                }
            }

            //select and delete card
            displayCards(folderName);
            Console.WriteLine("Which card would you like to delete?");                  
            while (true)
            {
                deleteCard = Console.ReadLine();
                if (File.Exists(getUserDirectory() + folderName + "\\" + deleteCard))
                {
                    break;
                }
                else
                {
                    Console.WriteLine("The specified card does not exist. Please try again.");
                }
            }

            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("WARNING: All cards within the group will be lost. ");
            Console.ResetColor();
            Console.WriteLine("Are you sure you want to delete this card group? (y/n)");
            string choice = Console.ReadLine();
            try
            {
                if (choice.Equals("y") || choice.Equals("Y"))
                {
                    File.Delete(getUserDirectory() + folderName + "\\" + deleteCard);
                }
                else
                {
                    Console.WriteLine("No cards were deleted.");
                    return;
                }
            }
            catch(Exception e)
            {
                Console.WriteLine("The process failed: {0}", e.ToString());
            }


            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("The card ");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write(deleteCard);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write(" was deleted succesfully. Press any key to continue.");
            Console.ResetColor();
            Console.ReadKey();



        }
        
        ///<summary>
        ///displays files ("cards") with in \cards\group
        ///</summary>
        static void displayCards(string group)
        {
            Console.Clear();
            Console.WriteLine("-------------- " + group + " ---------------");
            string targetDirectory = getUserDirectory();
            string[] fileEntries = Directory.GetFiles(targetDirectory+group + "\\");
            foreach (string fileName in fileEntries)
                ProcessFiles(fileName, group);

            Console.WriteLine("---------------------------------------------");
            
        }

        /// <summary>
        /// Method for formatting and printing 
        /// the card names (used in displayCards).
        /// </summary>
        static void ProcessFiles(string path, string cardGroup)
        {
            path = path.Replace(getUserDirectory() + cardGroup + "\\", "");
            Console.WriteLine(" - {0}", path);
        }

        /// <summary>
        /// Check if the given directory contains any folders or files.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static bool IsDirectoryEmpty(string path)
        {
            return !Directory.EnumerateFileSystemEntries(path).Any();
        }

    }
}
