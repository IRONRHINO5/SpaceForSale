using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MouseClickBehavior : MonoBehaviour {

    // Pictures of the pieces
    public Sprite Rocket;
    public Sprite Blaster;
    public Sprite UFO;
    public Sprite Helmet;
    public Sprite Satellite;
    public Sprite Flag;
    public Sprite empty;

    // Piece models
    public GameObject RocketPrefab;
    public GameObject BlasterPrefab;
    public GameObject UFOPrefab;
    public GameObject HelmetPrefab;
    public GameObject SatellitePrefab;
    public GameObject FlagPrefab;
    public GameObject ZorroPrefab;


    public void clickrocket()
    {

        if (!RocketPrefab.activeSelf)
        {
            // Place rocket model on screen, remove any other models from screen
            RocketPrefab.SetActive   (true);
            BlasterPrefab.SetActive  (false);
            UFOPrefab.SetActive      (false);
            HelmetPrefab.SetActive   (false);
            SatellitePrefab.SetActive(false);
            FlagPrefab.SetActive     (false);
            ZorroPrefab.SetActive    (false);
        }
        else
        {
            RocketPrefab.SetActive(false);
        }
    }

    public void clickblaster()
    {

        if (!BlasterPrefab.activeSelf)
        {
            // Place blaster model on screen, remove any other models from screen
            BlasterPrefab.SetActive  (true);
            RocketPrefab.SetActive   (false);
            UFOPrefab.SetActive      (false);
            HelmetPrefab.SetActive   (false);
            SatellitePrefab.SetActive(false);
            FlagPrefab.SetActive     (false);
            ZorroPrefab.SetActive    (false);
        }
        else
        {
            BlasterPrefab.SetActive(false);
        }
    }

    public void clickufo()
    {

        if (!UFOPrefab.activeSelf)
        {
            // Place UFO model on screen, remove any other models from screen
            UFOPrefab.SetActive      (true);
            RocketPrefab.SetActive   (false);
            BlasterPrefab.SetActive  (false);
            HelmetPrefab.SetActive   (false);
            SatellitePrefab.SetActive(false);
            FlagPrefab.SetActive     (false);
            ZorroPrefab.SetActive    (false);
        }
        else
        {
            UFOPrefab.SetActive(false);
        }
    }

    public void clickhelmet()
    {
        if (!HelmetPrefab.activeSelf)
        {
            // Place helmet model on screen, remove any other models from screen
            HelmetPrefab.SetActive   (true);
            RocketPrefab.SetActive   (false);
            BlasterPrefab.SetActive  (false);
            UFOPrefab.SetActive      (false);
            SatellitePrefab.SetActive(false);
            FlagPrefab.SetActive     (false);
            ZorroPrefab.SetActive    (false);
        }
        else
        {
            HelmetPrefab.SetActive(false);
        }
    }

    public void clicksatellite()
    {
        if (!SatellitePrefab.activeSelf)
        {
            // Place helmet model on screen, remove any other models from screen
            HelmetPrefab.SetActive   (false);
            RocketPrefab.SetActive   (false);
            BlasterPrefab.SetActive  (false);
            UFOPrefab.SetActive      (false);
            SatellitePrefab.SetActive(true);
            FlagPrefab.SetActive     (false);
            ZorroPrefab.SetActive    (false);
        }
        else
        {
            SatellitePrefab.SetActive(false);
        }
    }

    public void clickflag()
    {
        if (!FlagPrefab.activeSelf)
        {
            // Place helmet model on screen, remove any other models from screen
            HelmetPrefab.SetActive   (false);
            RocketPrefab.SetActive   (false);
            BlasterPrefab.SetActive  (false);
            UFOPrefab.SetActive      (false);
            SatellitePrefab.SetActive(false);
            FlagPrefab.SetActive     (true);
            ZorroPrefab.SetActive    (false);
        }
        else
        {
            FlagPrefab.SetActive(false);
        }
    }

    public void clickzorro()
    {

        if (!ZorroPrefab.activeSelf)
        {
            // Place rocket model on screen, remove any other models from screen
            RocketPrefab.SetActive   (false);
            BlasterPrefab.SetActive  (false);
            UFOPrefab.SetActive      (false);
            HelmetPrefab.SetActive   (false);
            SatellitePrefab.SetActive(false);
            FlagPrefab.SetActive     (false);
            ZorroPrefab.SetActive    (true);
            GameObject.Find("pieceName").GetComponentInChildren<Text>().text = "Zorro";
            GameObject.Find("Piece Description").GetComponentInChildren<Text>().text = "The legendary outlaw.";
        }
        else
        {
            ZorroPrefab.SetActive(false);
        }
    }

    // Whenever your mouse leaves the game piece the descriptions revert back to the active piece
    public void exitPrefab()
    {
        if(RocketPrefab.activeSelf)
        {
            GameObject.Find("pieceName").GetComponentInChildren<Text>().text = "Rocket";
            GameObject.Find("Piece Description").GetComponentInChildren<Text>().text = "The humble Rocket, used to launch mankind into the heavens.";
        }

        else if (BlasterPrefab.activeSelf)
        {
            GameObject.Find("pieceName").GetComponentInChildren<Text>().text = "Raygun";
            GameObject.Find("Piece Description").GetComponentInChildren<Text>().text = "A less elegant weapon, from a less civilized age.";
        }

        else if (UFOPrefab.activeSelf)
        {
            GameObject.Find("pieceName").GetComponentInChildren<Text>().text = "UFO";
            GameObject.Find("Piece Description").GetComponentInChildren<Text>().text = "I'm not saying this is an alien ship, but it's definitely an alien ship.";
        }

        else if (HelmetPrefab.activeSelf)
        {
            GameObject.Find("pieceName").GetComponentInChildren<Text>().text = "Helmet";
            GameObject.Find("Piece Description").GetComponentInChildren<Text>().text = "The helmet is an essential device for any space explorer.";
        }
        else if(SatellitePrefab.activeSelf)
        {
            GameObject.Find("pieceName").GetComponentInChildren<Text>().text = "Satellite";
            GameObject.Find("Piece Description").GetComponentInChildren<Text>().text = "Bloop... Bloop... Bloop... Bloop...";
        }
        else if(FlagPrefab.activeSelf)
        {
            GameObject.Find("pieceName").GetComponentInChildren<Text>().text = "Flag";
            GameObject.Find("Piece Description").GetComponentInChildren<Text>().text = "The flag has been used to stake one's claim for millenia. Now with less waving!";
        }
        else if (ZorroPrefab.activeSelf)
        {
            GameObject.Find("pieceName").GetComponentInChildren<Text>().text = "Zorro";
            GameObject.Find("Piece Description").GetComponentInChildren<Text>().text = "The legendary outlaw.";
        }
        else
        {
            GameObject.Find("pieceName").GetComponentInChildren<Text>().text = "";
            GameObject.Find("Piece Description").GetComponentInChildren<Text>().text = "";
        }
    }




}
