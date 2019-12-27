using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
using System.Linq;
using UnityEngine.Networking;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;

public class GameFlow : MonoBehaviour, IPunObservable
{
    //c [SyncVar]
    //c   Vector3 realPosition = Vector3.zero;

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
    
    public int gateOneTurnsUnchanged,         // The amount of turns each gate has spent with an unchanged state
               gateTwoTurnsUnchanged,
               gateThreeTurnsUnchanged;
    
    public GameObject gate1,
                      gate2,
                      gate3;

    PhotonView PhotonView;

    GameData GameDataInstance;
    public static GameFlow GameFlowInstance;

	// Use this for initialization
	void Start ()
    {
        GameDataInstance = GameData.GameDataInstance;

        PhotonView = gameObject.GetComponent<PhotonView>();

        gateOneTurnsUnchanged = gateTwoTurnsUnchanged = gateThreeTurnsUnchanged = 0;

        gate1 = GameObject.Find("GateToLoop1");
        gate2 = GameObject.Find("GateToLoop2");
        gate3 = GameObject.Find("GateToLoop3");
    }

    private void Awake()
    {
        if (!GameFlowInstance)
        {
            GameFlowInstance = this;
        }
    }
    

    // Update is called once per frame
    /*void Update ()
    {
        if (!isLocalPlayer)
        {
            return;
        }
        
    }*/

    // Current location needs to be replaced with the player who is rolling. We can get the player's current location from its class.
    /// <summary>
    /// NEGATIVE: Each gate is only checked once so if all three are closed and the player rolls a value
    ///     high enough to pass a gate twice in the main loop the result will not be correct.
    ///     Edits will need to be made or one gate must remain open at all times to counter act this.
    /// </summary>
    /// <param name="diceRoll"></param>
    /// <param name="playerIndex"></param>
    
    public void GateSwitcher()
    {
        // Gate one switch
        // Random.Range is the Unity random number generator. The min is inclusive and the max is exclusive.
        if (Random.Range(GATE_RANGE_MIN, GATE_RANGE_MAX) <= gateOneTurnsUnchanged)
        {
            PhotonView.RPC("SwitchGate", RpcTarget.All, GATE_1_LIST_VALUE);

            RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };
            SendOptions sendOptions = new SendOptions { Reliability = true };

            if(GameDataInstance.boardSpaces[GATE_1_LIST_VALUE].GetComponent<Gate>().IsOpen)
            {
                PhotonNetwork.RaiseEvent(PhotonEvents.MESSAGE, "The gate to the Alpha Quadrant is now open.", raiseEventOptions, sendOptions);
            }
            else
            {
                PhotonNetwork.RaiseEvent(PhotonEvents.MESSAGE, "The gate to the Alpha Quadrant is now closed.", raiseEventOptions, sendOptions);
            }
        }
        else
        {
            PhotonView.RPC("UpdateTurnsUnchanged", RpcTarget.All, 1);
        }

        // Gate two switch
        if (Random.Range(GATE_RANGE_MIN, GATE_RANGE_MAX) <= gateTwoTurnsUnchanged)
        {
            PhotonView.RPC("SwitchGate", RpcTarget.All, GATE_2_LIST_VALUE);

            RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };
            SendOptions sendOptions = new SendOptions { Reliability = true };

            if (GameDataInstance.boardSpaces[GATE_2_LIST_VALUE].GetComponent<Gate>().IsOpen)
            {
                PhotonNetwork.RaiseEvent(PhotonEvents.MESSAGE, "The gate to the Beta Quadrant is now open.", raiseEventOptions, sendOptions);
            }
            else
            {
                PhotonNetwork.RaiseEvent(PhotonEvents.MESSAGE, "The gate to the Beta Quadrant is now closed.", raiseEventOptions, sendOptions);
            }
        }
        else
        {
            PhotonView.RPC("UpdateTurnsUnchanged", RpcTarget.All, 2);
        }

        // Gate three switch
        if (Random.Range(GATE_RANGE_MIN, GATE_RANGE_MAX) <= gateThreeTurnsUnchanged)
        {
            PhotonView.RPC("SwitchGate", RpcTarget.All, GATE_3_LIST_VALUE);

            RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };
            SendOptions sendOptions = new SendOptions { Reliability = true };

            if (GameDataInstance.boardSpaces[GATE_3_LIST_VALUE].GetComponent<Gate>().IsOpen)
            {
                PhotonNetwork.RaiseEvent(PhotonEvents.MESSAGE, "The gate to the Gamma Quadrant is now open.", raiseEventOptions, sendOptions);
            }
            else
            {
                PhotonNetwork.RaiseEvent(PhotonEvents.MESSAGE, "The gate to the Gamma Quadrant is now closed.", raiseEventOptions, sendOptions);
            }
        }
        else
        {
            PhotonView.RPC("UpdateTurnsUnchanged", RpcTarget.All, 3);
        }

        // If all gates are closed, open gate one
        if (!GameDataInstance.boardSpaces[GATE_1_LIST_VALUE].GetComponent<Gate>().IsOpen &&
            !GameDataInstance.boardSpaces[GATE_2_LIST_VALUE].GetComponent<Gate>().IsOpen &&
            !GameDataInstance.boardSpaces[GATE_3_LIST_VALUE].GetComponent<Gate>().IsOpen)
        {
            int longestNotOpen = int.MinValue;
            int gateLongest = GATE_1_LIST_VALUE;

            if (longestNotOpen < gateOneTurnsUnchanged)
            {
                longestNotOpen = gateOneTurnsUnchanged;
                gateLongest = GATE_1_LIST_VALUE;
            }
            if (longestNotOpen < gateTwoTurnsUnchanged)
            {
                longestNotOpen = gateTwoTurnsUnchanged;
                gateLongest = GATE_2_LIST_VALUE;
            }
            if (longestNotOpen < gateThreeTurnsUnchanged)
            {
                longestNotOpen = gateThreeTurnsUnchanged;
                gateLongest = GATE_3_LIST_VALUE;
            }
            

            PhotonView.RPC("SwitchGate", RpcTarget.All, gateLongest);

            RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };
            SendOptions sendOptions = new SendOptions { Reliability = true };
            PhotonNetwork.RaiseEvent(PhotonEvents.MESSAGE, "The gate to the Alpha Quadrant is now open.", raiseEventOptions, sendOptions);
        }
    }

    // DEPRECATED -- NO LONGER USED OR NEEDED
    public void DisplayBoardLocation(int playerIndex)
    {
        BoardSpace currentSpace = GameDataInstance.boardSpaces[GameDataInstance.boardSpaceOrder[PlayerNetwork.Instance.BoardPosition]].GetComponent<BoardSpace>();
        Property currentProperty;

        GameObject.Find("PlanetNameText").GetComponent<Text>().text = currentSpace.BoardSpaceName;

        if (currentSpace is Property) // Property space
        {
            currentProperty = (Property)currentSpace;

            if (currentProperty.IsOwned) // Property is owned, display owner and rent cost if applicable
            {
                GameObject.Find("OwnedByInfoText").GetComponentInChildren<Text>().text = currentProperty.OwnedBy.ToString();
                GameObject.Find("PropertyValueInfoText").GetComponentInChildren<Text>().text = currentProperty.PropertyValue.ToString();
                GameObject.Find("PropertyRentInfoText").GetComponentInChildren<Text>().text = currentProperty.PropertyRent[currentProperty.RentTier].ToString();
                
                GameObject.Find("FlavorText").GetComponentInChildren<Text>().text = currentProperty.PropertyFlavorText;
            }
            else // Property is not owned, would you like to buy it?
            {
                GameObject.Find("OwnedByInfoText").GetComponentInChildren<Text>().text = "Not owned, would you like to buy?";
                GameObject.Find("PropertyValueInfoText").GetComponentInChildren<Text>().text = currentProperty.PropertyValue.ToString();
                GameObject.Find("PropertyRentInfoText").GetComponentInChildren<Text>().text = currentProperty.PropertyRent[currentProperty.RentTier].ToString();
                GameObject.Find("FlavorText").GetComponentInChildren<Text>().text = currentProperty.PropertyFlavorText;
            }
        }
        /*
        else if (currentSpace is Chance) // Chance space
        {
            currentChance = (Chance)currentSpace;
            GameDataInstance.ChanceController.ChanceSelection(playerIndex, GameDataInstance.boardSpaceOrder[GameDataInstance.players[playerIndex].BoardPosition]);
        }*/
        else
        {
            // Already knows it is a BoardSpace, so no action is required
        }
        
    }

    public void UpdateRent(Property addedProperty) // increase the rent tier if a player ownes multiple properties in a set
    {
        List<Property> setList = new List<Property>();

        foreach (Property property in GameObject.Find("PlayerNetwork").GetComponent<PlayerNetwork>().MyProperties)
        {
            if (property.PropertySet == addedProperty.PropertySet)
            {
                setList.Add(property);
            }
        }

        foreach (Property property in setList)
        {
            property.RentTier = setList.Count - 1;
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        
    }

    [PunRPC]
    private void SwitchGate(int gateListValue)
    {
        GameDataInstance.boardSpaces[gateListValue].GetComponent<Gate>().IsOpen = !GameDataInstance.boardSpaces[gateListValue].GetComponent<Gate>().IsOpen;

        switch (gateListValue)
        {
            case GATE_1_LIST_VALUE:
                gate1.transform.GetChild(0).gameObject.SetActive(GameDataInstance.boardSpaces[gateListValue].GetComponent<Gate>().IsOpen);
                gate1.transform.GetChild(1).gameObject.SetActive(GameDataInstance.boardSpaces[gateListValue].GetComponent<Gate>().IsOpen);
                gate1.transform.GetChild(2).gameObject.SetActive(GameDataInstance.boardSpaces[gateListValue].GetComponent<Gate>().IsOpen);
                gate1.transform.GetChild(3).gameObject.SetActive(GameDataInstance.boardSpaces[gateListValue].GetComponent<Gate>().IsOpen);
                gateOneTurnsUnchanged = 0;
                break;
            case GATE_2_LIST_VALUE:
                gate2.transform.GetChild(0).gameObject.SetActive(GameDataInstance.boardSpaces[gateListValue].GetComponent<Gate>().IsOpen);
                gate2.transform.GetChild(1).gameObject.SetActive(GameDataInstance.boardSpaces[gateListValue].GetComponent<Gate>().IsOpen);
                gate2.transform.GetChild(2).gameObject.SetActive(GameDataInstance.boardSpaces[gateListValue].GetComponent<Gate>().IsOpen);
                gate2.transform.GetChild(3).gameObject.SetActive(GameDataInstance.boardSpaces[gateListValue].GetComponent<Gate>().IsOpen);
                gateTwoTurnsUnchanged = 0;
                break;
            case GATE_3_LIST_VALUE:
                gate3.transform.GetChild(0).gameObject.SetActive(GameDataInstance.boardSpaces[gateListValue].GetComponent<Gate>().IsOpen);
                gate3.transform.GetChild(1).gameObject.SetActive(GameDataInstance.boardSpaces[gateListValue].GetComponent<Gate>().IsOpen);
                gate3.transform.GetChild(2).gameObject.SetActive(GameDataInstance.boardSpaces[gateListValue].GetComponent<Gate>().IsOpen);
                gate3.transform.GetChild(3).gameObject.SetActive(GameDataInstance.boardSpaces[gateListValue].GetComponent<Gate>().IsOpen);
                gateThreeTurnsUnchanged = 0;
                break;
        }
    }

    [PunRPC]
    private void UpdateTurnsUnchanged(int gateNum)
    {
        switch (gateNum)
        {
            case 1:
                gateOneTurnsUnchanged++;
                break;
            case 2:
                gateTwoTurnsUnchanged++;
                break;
            case 3:
                gateThreeTurnsUnchanged++;
                break;
        }
    }

}