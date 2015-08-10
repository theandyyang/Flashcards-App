﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

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
        static IList<Card> makeList()
        {
            
            //get card groups
            string[] fileEntries = Directory.GetDirectories(getUserDirectory());

            //get cards from each card group
            for (int i = 0; i < fileEntries.Length; i++)
            {
                int cardCount = System.IO.Directory.GetFiles(getUserDirectory()).Length;
            }
            return null;
        }

        //main
        static int Main(string[] args)
        {
            Console.WriteLine("Welcome to the Flashcards app.");
            //loop to allow for continuous usage
            while (true)
            {
                Console.Clear();
                //main menu
                Console.WriteLine("---------------------------------------------");
                Console.WriteLine("What would you like to do?");
                Console.WriteLine("\n\t1: Create card group");
                Console.WriteLine("\n\t2: Display card groups");
                Console.WriteLine("\n\t3: Delete card group");
                Console.WriteLine("\n\t4: Create card");
                Console.WriteLine("\n\t5: Read cards");
                Console.WriteLine("\n\t6: Delete card");
                Console.WriteLine("\n\t7: Exit program\n");
                Console.WriteLine("---------------------------------------------");
                //Console.WriteLine(getUserDirectory());

                //get user's command
                string choice = Console.ReadLine();

                //switch cases to lead to different functions
                switch (choice)
                {
                    case "1":
                        createCardGroup();
                        break;
                    case "2":
                        Console.Clear();
                        displayCardGroups(true);
                        break;
                    case "3":
                        deleteCardGroup();
                        break;
                    case "4":
                        createCard();
                        break;
                    case "5":
                        //readCards();
                        break;
                    case "6":
                        //deleteCard();
                        break;
                    case "7":
                        return 1;//terminate program
                    default:
                        Console.WriteLine("Invalid command. Please try again: ");
                        break;
                }

                //Console.ReadKey();

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
                //Console.WriteLine("The directory was created successfully at {0}.", Directory.GetCreationTime(path));

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

            // Process the list of files found in the directory. 
            string[] fileEntries = Directory.GetDirectories(targetDirectory);
            foreach (string fileName in fileEntries)
                ProcessFile(fileName);

            Console.WriteLine("---------------------------------------------");

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
        static void ProcessFile(string path)
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

            try
            {
                // Determine whether the directory exists. 
                if (Directory.Exists(path))
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
                }

            }
            catch (Exception e)
            {
                Console.WriteLine("The process failed: {0}", e.ToString());
            }
            finally { }

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

        /// <summary>
        /// Create a new text file (a card) containing
        /// two lines, which correspond to two sides of 
        /// a card.
        /// </summary>
        static void createCard()
        {
            Console.Clear();
            while (true)
            {
                Console.WriteLine("Which card group would you like to modify?");
                displayCardGroups(false);
                string path = getUserDirectory() + Console.ReadLine();
                if (Directory.Exists(path))
                {
                    string[] cardContent = { "", "" };

                    //ask user to populate two sides of the card
                    Console.Write("What would you like to write on side 1?");
                    cardContent[0] = Console.ReadLine();

                    Console.Write("What would you like to write on side 2?");
                    cardContent[1] = Console.ReadLine();

                    string textFileName = "1";
                    //create and write to new text file
                    System.IO.File.WriteAllLines(@path + "/" + textFileName + ".txt", cardContent);
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
        /// read its cards.
        /// </summary>
        static void readCards()
        {

        }

        /// <summary>
        /// Allows the user to select a card group and 
        /// delete a card from it.
        /// </summary>
        static void deleteCard()
        {
            //select card group to modify
            while (true)
            {
                Console.WriteLine("Which card group would you like to modify?");
                string folderName = Console.ReadLine();

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
            while (true)
            {
                
            }


        }

    }
}
