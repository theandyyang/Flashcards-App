using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// Program Name: Card.cs
/// Program Description: A class that 
/// acts as one flashcard. It stores 
/// strings for two sides of the card. 
/// Authors: Ryan Samarajeewa, Andy Yang
/// </summary>
namespace Flashcards
{
    class Card
    {
        public string id, side1, side2;
        public bool viewed = false;


        public Card(string id, string side1, string side2)
        {
            this.id = id;
            this.side1 = side1;
            this.side2 = side2;
            viewed = false;
        }

          
    }

    
}
