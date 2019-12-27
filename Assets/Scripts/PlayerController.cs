using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Realtime;
using ExitGames.Client.Photon;
using Photon.Pun;
using System.Text;

public enum PlayerAction
{
    SHOOT,
    JUMP
}

public class PlayerController : MonoBehaviour {

    public const int GATE_1_LIST_VALUE = 3,  // Planetary ID numbers
                     GATE_2_LIST_VALUE = 19,
                     GATE_3_LIST_VALUE = 35,

                     GATE_INCLUSIVE_LOOP_LENGTH = 14, // Length of loops that loop back in on gate
                     GATE_EXCLUSIVE_LOOP_LENGTH = 13, // Length of loops that do not loop back in on gate

                     GATE_1_SEQUENCE_LOCATION = 3,    // Planetary locations on map accounting for possible double-hit gates
                     GATE_2_SEQUENCE_LOCATION = 20,
                     GATE_3_SEQUENCE_LOCATION = 37,

                     GATE_RANGE_MIN = 1,               // 
                     GATE_RANGE_MAX = 6;               //

    private int newLocation = 0;
    

    public delegate void PlayerInputCallback(PlayerAction action, float deg);
    public event PlayerInputCallback OnPlayerInput;
    bool isLocalPlayer;

    GameData GameDataInstance;
    GameFlow GameFlowInstance;
    PlayerNetwork Player;

    // Use this for initialization
    void Start()
    {
        //Instance = NetworkManager.Instance;
        //GameFlowInstance = GameObject.Find("GameData").GetComponent<GameFlow>();
        Player = GameObject.Find("PlayerNetwork").GetComponent<PlayerNetwork>();

        isLocalPlayer = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isLocalPlayer)
        {
            return;
        }

        

        //Shoot();
    }

    public void RollDice(int dice_override = -1)
    {
        GameDataInstance = GameObject.Find("GameData").GetComponent<GameData>();
        int diceRoll;
        
        // Allow for a manual override of the dice roll
        if(dice_override == -1)
            diceRoll = Random.Range(1, 7) + Random.Range(1, 7);
        else
            diceRoll = dice_override;

        // If the player has received a doubleRoll chance card this will double their roll and then get rid of it
        if (Player.DoubleRoll)
        {
            diceRoll += diceRoll;
            Player.DoubleRoll = false;
        }
        GameObject.Find("DiceText").GetComponent<Text>().text = diceRoll.ToString();

        movePlayer(diceRoll);
        
        print(Player.BoardPosition + " new board position");
    }

    public void movePlayer(int diceRoll)
    {
        bool passedGate1 = false;
        
        // Calculate new location assuming all gates are open
        newLocation = (Player.BoardPosition + diceRoll) % GameDataInstance.boardSpaceOrder.Length;
        
        // Check if player will pass the first gate and if closed add loop amount to account for skipping those properties
        if (newLocation > GATE_1_SEQUENCE_LOCATION && (Player.BoardPosition <= GATE_1_SEQUENCE_LOCATION || newLocation <= Player.BoardPosition))
        {
            if (!GameDataInstance.boardSpaces[GATE_1_LIST_VALUE].GetComponent<Gate>().IsOpen)
            {
                newLocation += GATE_INCLUSIVE_LOOP_LENGTH;
                passedGate1 = true;
            }
        }
        newLocation %= GameDataInstance.boardSpaceOrder.Length; // Accounts for player returning to the beginning of the board
        
        // Check if player will pass the second gate and if closed add loop amount to account for skipping those properties
        if (newLocation > GATE_2_SEQUENCE_LOCATION && (Player.BoardPosition <= GATE_2_SEQUENCE_LOCATION || newLocation <= Player.BoardPosition))
        {
            if (!GameDataInstance.boardSpaces[GATE_2_LIST_VALUE].GetComponent<Gate>().IsOpen)
            {
                newLocation += GATE_INCLUSIVE_LOOP_LENGTH;
            }
        }
        newLocation %= GameDataInstance.boardSpaceOrder.Length;
        
        // Check if player will pass the third gate and if closed add loop amount to account for skipping those properties
        if (newLocation > GATE_3_SEQUENCE_LOCATION && (Player.BoardPosition <= GATE_3_SEQUENCE_LOCATION || newLocation <= Player.BoardPosition))
        {
            if (!GameDataInstance.boardSpaces[GATE_3_LIST_VALUE].GetComponent<Gate>().IsOpen) //!Player.CmdCheckGates(GATE_3_LIST_VALUE)
            {
                newLocation += GATE_EXCLUSIVE_LOOP_LENGTH;
            }
        }
        newLocation %= GameDataInstance.boardSpaceOrder.Length;
        
        // Check if player will pass the first gate again and if closed add loop amount to account for skipping those properties
        if (newLocation > GATE_1_SEQUENCE_LOCATION && (Player.BoardPosition <= GATE_1_SEQUENCE_LOCATION || newLocation <= Player.BoardPosition))
        {
            if (!GameDataInstance.boardSpaces[GATE_1_LIST_VALUE].GetComponent<Gate>().IsOpen && !passedGate1)
            {
                newLocation += GATE_INCLUSIVE_LOOP_LENGTH;
            }
        }
        newLocation %= GameDataInstance.boardSpaceOrder.Length;

        // Update current location
        Player.BoardPosition = newLocation;

        // Perform action based on where you landed
        BoardSpace currentSpace = GameDataInstance.boardSpaces[GameDataInstance.boardSpaceOrder[Player.BoardPosition]].GetComponent<BoardSpace>();
        Chance currentChance;

        // If we can get a property or gate component, it is a property or gate so do property and gate stuff
        if (GameDataInstance.boardSpaces[GameDataInstance.boardSpaceOrder[Player.BoardPosition]].GetComponent<Property>()) 
        {
            Property currentProperty = GameDataInstance.boardSpaces[GameDataInstance.boardSpaceOrder[Player.BoardPosition]].GetComponent<Property>();

            //GameDataInstance.boardSpaces[GameDataInstance.boardSpaceOrder[Player.BoardPosition]].transform.GetChild(1).GetComponent<PlanetInfo>().CardUpdate();
            GameObject.Find("HUD").GetComponentInChildren<PlanetInfo>().CardUpdate();
            
        }
        else if(GameDataInstance.boardSpaces[GameDataInstance.boardSpaceOrder[Player.BoardPosition]].GetComponent<Gate>())
        {
            Gate currentGate = GameDataInstance.boardSpaces[GameDataInstance.boardSpaceOrder[Player.BoardPosition]].GetComponent<Gate>();

            //GameDataInstance.boardSpaces[GameDataInstance.boardSpaceOrder[Player.BoardPosition]].transform.GetChild(1).GetComponent<PlanetInfo>().CardUpdate();
            GameObject.Find("HUD").GetComponentInChildren<PlanetInfo>().CardUpdate();
        }
        // // If we can get a chance, it is a chance so do chance stuff
        else if (GameDataInstance.boardSpaces[GameDataInstance.boardSpaceOrder[Player.BoardPosition]].GetComponent<Chance>())
        {
            currentChance = GameDataInstance.boardSpaces[GameDataInstance.boardSpaceOrder[Player.BoardPosition]].GetComponent<Chance>();
            //GameDataInstance.ActivePlayer++;
            //GameDataInstance.ChanceController.ChanceSelection(Player);
        }
        else
        {
            //GameDataInstance.ActivePlayer++;
        }
    }

    public void BuyProperty()
    {
        Property availableProperty = GameDataInstance.boardSpaces[GameDataInstance.boardSpaceOrder[Player.BoardPosition]].GetComponent<Property>();

        Player.BankAccount -= availableProperty.PropertyValue;
        AddProperty(GameDataInstance.boardSpaces[GameDataInstance.boardSpaceOrder[Player.BoardPosition]].GetComponent<Property>());

        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };
        SendOptions sendOptions = new SendOptions { Reliability = true };
        PhotonNetwork.RaiseEvent(PhotonEvents.MESSAGE, Player.PlayerName + " bought the planet " + GameDataInstance.boardSpaces[GameDataInstance.boardSpaceOrder[Player.BoardPosition]].GetComponent<Property>().BoardSpaceName + " for " + GameDataInstance.boardSpaces[GameDataInstance.boardSpaceOrder[Player.BoardPosition]].GetComponent<Property>().PropertyValue + " credits.", raiseEventOptions, sendOptions);

        // Update the active player after buying
        GameDataInstance.ActivePlayer++;
    }

    public void AddProperty(Property property)
    {
        GameFlowInstance = GameObject.Find("GameData").GetComponent<GameFlow>();

        Player.MyProperties.Add(property);
        Player.TotalMeterValue += property.MeterValue;
        property.IsOwned = true;
        property.OwnedBy = Player.PlayerName;
        GameFlowInstance.UpdateRent(property);
    }

    public void PayRent(Property currentProperty) // not tested (can't be until we have multiplayer)
    {
        int doubleRent = currentProperty.DoubleRent ? 2 : 1;

        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };
        SendOptions sendOptions = new SendOptions { Reliability = true };

        // Pay rent if player has enough money, else give least valuable property
        if (Player.BankAccount - currentProperty.PropertyRent[currentProperty.RentTier] >= 0)
        {
            Player.BankAccount -= currentProperty.PropertyRent[currentProperty.RentTier] * doubleRent;

            PhotonNetwork.RaiseEvent(PhotonEvents.PAY_RENT, new object[] { currentProperty.OwnedBy, currentProperty.PropertyRent[currentProperty.RentTier] * doubleRent }, raiseEventOptions, sendOptions);
            PhotonNetwork.RaiseEvent(PhotonEvents.MESSAGE, Player.PlayerName + " landed on " + currentProperty.BoardSpaceName + " and paid " + currentProperty.OwnedBy + " " + (currentProperty.PropertyRent[currentProperty.RentTier] * doubleRent).ToString() + " credits.", raiseEventOptions, sendOptions);
        }
        else
        {
            int lowestValue = int.MaxValue;
            Property lowestProperty = null;

            // Determine the lowest property
            foreach(Property property in Player.MyProperties)
            {
                if(property.PropertyValue < lowestValue)
                {
                    lowestProperty = property;
                    lowestValue = lowestProperty.PropertyValue;
                }
            }

            if (lowestProperty != null)
            {
                Player.MyProperties.Remove(lowestProperty);
                Player.TotalMeterValue -= lowestProperty.MeterValue;

                PhotonNetwork.RaiseEvent(PhotonEvents.GIVE_PROPERTY, new object[] { currentProperty.OwnedBy, lowestProperty.SpaceNumber }, raiseEventOptions, sendOptions);
                PhotonNetwork.RaiseEvent(PhotonEvents.MESSAGE, Player.PlayerName + " is giving " + currentProperty.OwnedBy + " " + lowestProperty.BoardSpaceName + " for reparations.", raiseEventOptions, sendOptions);
            }
            else
            {
                PhotonNetwork.RaiseEvent(PhotonEvents.PAY_RENT, new object[] { currentProperty.OwnedBy, Player.BankAccount }, raiseEventOptions, sendOptions);
                PhotonNetwork.RaiseEvent(PhotonEvents.MESSAGE, Player.PlayerName + "is broke but landed on " + currentProperty.BoardSpaceName + " and paid " + currentProperty.OwnedBy + " " + (currentProperty.PropertyRent[currentProperty.RentTier] * doubleRent).ToString() + " credits.", raiseEventOptions, sendOptions);

                Player.BankAccount -= Player.BankAccount;
            }
        }
    }

    public void SetupLocalPlayer()
    {
        //add color to your player
        isLocalPlayer = true;
    }

    public void TurnStart()
    {
        //print("pc turn start");
        if (isLocalPlayer)
        {
            gameObject.transform.Find("LocalCanvas").transform.Find("DiceRoller").GetComponent<Button>().interactable = true;
            gameObject.transform.Find("LocalCanvas").transform.Find("BuyProperty").GetComponent<Button>().interactable = true;
        }

        Player.IsTurn = true;
    }

    public void TurnEnd()
    {
        print("pc turn end");
        if (isLocalPlayer)
        {
            gameObject.transform.Find("LocalCanvas").transform.Find("DiceRoller").GetComponent<Button>().interactable = false;
            gameObject.transform.Find("LocalCanvas").transform.Find("BuyProperty").GetComponent<Button>().interactable = false;
        }


        Player.IsTurn = false;
    }

    public override string ToString()
    {
        return "Controller";
    }
}