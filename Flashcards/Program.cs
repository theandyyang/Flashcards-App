using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

/// <summary>
/// A program that saves and reads flashcards for
/// the user. 
/// </summary>
namespace Flashcards
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to the Flashcards app.");
            Console.WriteLine("---------------------------------------------");
            Console.WriteLine("What would you like to do?");
            Console.WriteLine("\n\t1: Create card group");
            Console.WriteLine("\n\t2: Create card");
            Console.WriteLine("\n\t3: Read cards");
            //Console.WriteLine("\n\t");
            //Console.WriteLine("\n\tCreate card");
            string choice = Console.ReadLine();
            if (choice.Equals("1")){
                createCardGroup();
            }
            Console.ReadKey();

        }

        static void createCard()
        {

        }

        static void createCardGroup()
        {
            Console.WriteLine("What would you like the new flashcard group to be called?");
            string folderName = Console.ReadLine();
            string path = @"C:\Users\Ryan\Documents\visual studio 2015\Projects\FlashcardsSolution\Flashcards\cards\" + folderName;

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
                Console.WriteLine("The directory was created successfully at {0}.", Directory.GetCreationTime(path));

            }
            catch (Exception e)
            {
                Console.WriteLine("The process failed: {0}", e.ToString());
            }
            finally { }
        }
    }
}
