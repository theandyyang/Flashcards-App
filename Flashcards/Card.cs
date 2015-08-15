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
        string id = "";
        string sideOne = "";
        string sideTwo = "";
        bool viewed = false;

        //populates a side of a card with a string
        public void setSide(int side, string content)
        {
            if (side == 1)
            {
                this.sideOne = content;
            }
            else
            {
                this.sideTwo = content;
            }

        }

        //set the 'viewed' variable to true 
        //so that it cannot be read twice in the same
        //session.
        public void isViewed()
        {
            this.viewed = true;
        }
        
    }

    
}
