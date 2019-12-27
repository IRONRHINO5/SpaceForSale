using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChanceCard {

    private string flavorText;
    private int chanceCardID,
                cardSpecificVal;

    public delegate void ChanceFunction(PlayerNetwork player = null);
    public ChanceFunction function;

    public int ChanceCardID
    {
        get
        {
            return chanceCardID;
        }
        set
        {
            chanceCardID = value;
        }
    }

    public int CardSpecificVal
    {
        get
        {
            return cardSpecificVal;
        }
        set
        {
            cardSpecificVal = value;
        }
    }

    public string FlavorText
    {
        get
        {
            return flavorText;
        }
        set
        {
            flavorText = value;
        }
    }

    public ChanceCard()
    {
        FlavorText = "Not provided";
        ChanceCardID = 9999;
        function = null;
        CardSpecificVal = 9999;
    }

    public ChanceCard(string uniqueFlavorText, int functionID, int specificValue, ChanceFunction myFunction)
    {
        FlavorText = uniqueFlavorText;
        ChanceCardID = functionID;
        function = myFunction;
        CardSpecificVal = specificValue;
    }
    //myChanceFunction();
}
