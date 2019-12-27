using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MouseOverBehavior : MonoBehaviour {

    
    public void PointerEnterRocket() // Display various information when the mouse hovers over the rocket
    {
        // Display name of piece
        GameObject.Find("pieceName").GetComponentInChildren<Text>().text = "Rocket";

        // Display description of piece
        GameObject.Find("Piece Description").GetComponentInChildren<Text>().text = "The humble Rocket, used to launch mankind into the heavens.";

    }

    public void PointerEnterBlaster() // Display various information when the mouse hovers over the blaster
    {
        // Display name of piece
        GameObject.Find("pieceName").GetComponentInChildren<Text>().text = "Raygun";

        // Display description of piece
        GameObject.Find("Piece Description").GetComponentInChildren<Text>().text = "A less elegant weapon, from a less civilized age.";

    }

    public void PointerEnterUFO() // Display various information when the mouse hovers over the UFO
    {
        // Display name of piece
        GameObject.Find("pieceName").GetComponentInChildren<Text>().text = "UFO";

        // Display description of piece
        GameObject.Find("Piece Description").GetComponentInChildren<Text>().text = "I'm not saying this is an alien ship, but it's definitely an alien ship.";

    }

    public void PointerEnterHelmet() // Display various information when the mouse hovers over the helmet
    {
        // Display name of piece
        GameObject.Find("pieceName").GetComponentInChildren<Text>().text = "Helmet";

        // Display description of piece
        GameObject.Find("Piece Description").GetComponentInChildren<Text>().text = "The helmet is an essential device for any space explorer.";

    }

    public void PointerEnterSatellite() // Display various information when the mouse hovers over the helmet
    {
        // Display name of piece
        GameObject.Find("pieceName").GetComponentInChildren<Text>().text = "Satellite";

        // Display description of piece
        GameObject.Find("Piece Description").GetComponentInChildren<Text>().text = "Bloop... Bloop... Bloop... Bloop...";

    }

    public void PointerEnterFlag() // Display various information when the mouse hovers over the helmet
    {
        // Display name of piece
        GameObject.Find("pieceName").GetComponentInChildren<Text>().text = "Flag";

        // Display description of piece
        GameObject.Find("Piece Description").GetComponentInChildren<Text>().text = "The flag has been used to stake one's claim for millenia. Now with less waving!";

    }

    public void PointerEnterGate()
    {
        GameObject.Find("HelpfulText").GetComponentInChildren<Text>().text = "When you move through the galaxy, you may notice some strange hoops.\nThese are gates, " +
            "which allow you to move from one star sytem to another in an instant, and will either be open(filled with energy) or closed(empty), " +
            "which is randomly determined at the start of each of your turns.\nWhen a gate is closed, you will not pass through it, " +
            "but if it is open you will fly straight through it into a new star system!";
    }

    public void PointerEnterGeneral()
    {
        GameObject.Find("HelpfulText").GetComponent<Text>().text = "> To move your piece, click \"Launch\".\n" + 
                                                                   "> To see a topdown view of the game,\n" + 
                                                                   "   click \"View\". You can use the arrow keys\n" + 
                                                                   "   to move the view.\n" +
                                                                   "> You can see a list of what planets a\n" + 
                                                                   "   player owns by hovering over the name\n" +
                                                                   "   of that player.\n" + 
                                                                   "> When you buy planets they fill your\n" + 
                                                                   "   victory meter. When it's full, you win.";
    }

    public void PointerEnterCredits()
    {
        GameObject.Find("HelpfulText").GetComponent<Text>().text = "\t\t\t\tDeveloped by:\n\t\t\tRyan Shrewsbury\n\t\t\t Andrew Vedder\n\t\t\t   Ethan Snyder\n" +
            "\t\t\t   Nick Caroselli\n\t\t\t     Craig Hafer\n\t\t\t     Kyle Ebert\n\n\t\t\t\t  Music By:\n\t\t\t     Josh Kohr\n\n\t\t\t\t    Artists:\nLeah Bayatola   AJ Childers";
     }

    public void PointerEnterHowTo()
    {
        GameObject.Find("HelpfulText").GetComponent<Text>().text = "When you land on a planet, you will see an information card.\n" +
                                                                   "From Top to bottom, the card has the following info:\n" +
                                                                   "> The name of the planet\n" + 
                                                                   "> Who owns the planet, if anyone\n" +
                                                                   "> What the rent is.\n" + 
                                                                   "> How much victory meter it is worth.\n" +
                                                                   "> A brief description of the planet.\n" +
                                                                   "> A button to pass on buying the planet,\n   and a button to buy it.";
    }



    public void ClearHelpfulText()
    {
        GameObject.Find("HelpfulText").GetComponent<Text>().text = " ";
    }

    public void PointerExit() // Clear information of pieces when mouse leaves piece.
    {
        GameObject.Find("pieceName").GetComponentInChildren<Text>().text = "Name";
        GameObject.Find("Piece Description").GetComponentInChildren<Text>().text = "Description";
    }


}
