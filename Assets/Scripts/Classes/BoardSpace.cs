using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardSpace : MonoBehaviour {

    public int spaceNumber;
    public string boardSpaceName;

    public int SpaceNumber
    {
        get
        {
            return spaceNumber;
        }

        set
        {
            if (value >= 0)
                spaceNumber = value;
            else
                spaceNumber = 9999;
        }
    }

    public string BoardSpaceName
    {
        get
        {
            return boardSpaceName;
        }

        set
        {
            if (value != "")
                boardSpaceName = value;
            else
                boardSpaceName = "Name not given";
        }
    }

    /*public BoardSpace()
    {
        SpaceNumber = 9999;
        BoardSpaceObject = GameObject.Find("Earth");
        BoardSpaceName = "boardSpaceName";
    }

    public BoardSpace(int number, string name, GameObject newObject)
    {
        SpaceNumber = number;
        BoardSpaceObject = newObject;
        BoardSpaceName = name;
    }*/

    public override string ToString()
    {
        return "Number:" + SpaceNumber +
               "\nName: " + BoardSpaceName;
    }

}
