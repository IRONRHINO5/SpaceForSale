using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using ExitGames.Client.Photon;

public sealed class GameData : MonoBehaviourPunCallbacks, IPunObservable, IOnEventCallback {

    public const int GATE_1_LIST_VALUE = 3,  // Planetary ID numbers
                     GATE_2_LIST_VALUE = 19;
    
    public int[] boardSpaceOrder = new int[51]; // Easier to explain in person, but in game planetary locations that account for double hit gates

    public List<GameObject> boardSpaces = new List<GameObject>();

    public ChanceFunctions ChanceController;

    public static GameData GameDataInstance;

    public PhotonView PhotonView;

    public static Photon.Realtime.Player[] players;
    
    public static List<PlayerNetwork> playerNetworks;
    
    public int activePlayer;
    public int ActivePlayer
    {
        get
        {
            return activePlayer;
        }
        set
        {
            RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };
            SendOptions sendOptions = new SendOptions { Reliability = true };
            PhotonNetwork.RaiseEvent(PhotonEvents.UPDATE_ACTIVE_PLAYER, null, raiseEventOptions, sendOptions);
            //PhotonView.RPC("updateActivePlayer", RpcTarget.All);
        }
    }


    private void Awake()
    {
        if(!GameDataInstance)
        {
            GameDataInstance = this;
        }

        players = PhotonNetwork.PlayerList;
    }

    private void Start()
    {
        ChanceController = gameObject.GetComponent<ChanceFunctions>();
        PhotonView = gameObject.GetComponent<PhotonView>();

        //int planetID = 0; // The id number of each planet

        boardSpaces.Add(GameObject.Find("Sun"));
        boardSpaces.Add(GameObject.Find("Mercury"));
        boardSpaces.Add(GameObject.Find("Venus"));
        boardSpaces.Add(GameObject.Find("Earth"));

        boardSpaces.Add(GameObject.Find("Zethurus"));
        boardSpaces.Add(GameObject.Find("Kanus"));
        boardSpaces.Add(GameObject.Find("StarWarsChance1"));
        boardSpaces.Add(GameObject.Find("Yoathea"));
        boardSpaces.Add(GameObject.Find("Yavis"));
        boardSpaces.Add(GameObject.Find("Yigawa"));
        boardSpaces.Add(GameObject.Find("StarWarsChance2"));
        boardSpaces.Add(GameObject.Find("Agnonides"));
        boardSpaces.Add(GameObject.Find("Xilniwei"));
        boardSpaces.Add(GameObject.Find("Zeon"));
        boardSpaces.Add(GameObject.Find("StarWarsChance3"));
        boardSpaces.Add(GameObject.Find("Pingiri 20"));
        boardSpaces.Add(GameObject.Find("Pongippe 17"));

        boardSpaces.Add(GameObject.Find("Mars"));
        boardSpaces.Add(GameObject.Find("Jupiter"));
        boardSpaces.Add(GameObject.Find("Saturn"));

        boardSpaces.Add(GameObject.Find("Kenvionope"));
        boardSpaces.Add(GameObject.Find("StarTrekChance1"));
        boardSpaces.Add(GameObject.Find("Rameshan"));
        boardSpaces.Add(GameObject.Find("Nutania"));
        boardSpaces.Add(GameObject.Find("Vilmars"));
        boardSpaces.Add(GameObject.Find("Lliyinov"));
        boardSpaces.Add(GameObject.Find("Saonys"));
        boardSpaces.Add(GameObject.Find("StarTrekChance2"));
        boardSpaces.Add(GameObject.Find("Zyke 25"));
        boardSpaces.Add(GameObject.Find("Zanzypso"));
        boardSpaces.Add(GameObject.Find("Zunides"));
        boardSpaces.Add(GameObject.Find("StarTrekChance3"));
        boardSpaces.Add(GameObject.Find("Huntaria"));

        boardSpaces.Add(GameObject.Find("Uranus"));
        boardSpaces.Add(GameObject.Find("Neptune"));
        boardSpaces.Add(GameObject.Find("Pluto"));

        boardSpaces.Add(GameObject.Find("Crion 6QT"));
        boardSpaces.Add(GameObject.Find("Phorix B2"));
        boardSpaces.Add(GameObject.Find("Cyke 9ZU"));
        boardSpaces.Add(GameObject.Find("HodgePodgeChance1"));
        boardSpaces.Add(GameObject.Find("Vorth YOZA"));
        boardSpaces.Add(GameObject.Find("Xonubos"));
        boardSpaces.Add(GameObject.Find("Denvion 14M"));
        boardSpaces.Add(GameObject.Find("Ochonoe"));
        boardSpaces.Add(GameObject.Find("HodgePodgeChance2"));
        boardSpaces.Add(GameObject.Find("Nicrypso"));
        boardSpaces.Add(GameObject.Find("Xucinda"));
        boardSpaces.Add(GameObject.Find("Veshan 69"));
        boardSpaces.Add(GameObject.Find("WormholeToSun"));

        // Determine the actual order of board spaces accounting for gates 1 and 2 being possibly landed on twice
        for (int i = 0, k = 0; i < 51; i++)
        {
            boardSpaceOrder[i] = k; // Count through the index
            k++;

            if (i == 17) // Gate 1...
            {
                boardSpaceOrder[i] = GATE_1_LIST_VALUE;
                k--;
            }
            else if (i == 34) // Gate 2...
            {
                boardSpaceOrder[i] = GATE_2_LIST_VALUE;
                k--;
            }
            // You don't have to account for the last gate since it doesn't loop back in on itself
        }
    }

    public void OnEvent(EventData photonEvent)
    {
        switch (photonEvent.Code)
        {
            case PhotonEvents.UPDATE_ACTIVE_PLAYER:
                print("turn change"); 
                activePlayer++;
                
                // We only want one person sending out this event at any time.
                if (GameObject.Find("PlayerNetwork").GetComponent<PlayerNetwork>().PlayerID == 0)
                {
                    RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };
                    SendOptions sendOptions = new SendOptions { Reliability = true };
                    PhotonNetwork.RaiseEvent(PhotonEvents.GAME_CHANGE_TURN, null, raiseEventOptions, sendOptions);
                }
                break;
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
    }

    [PunRPC]
    void updateActivePlayer()
    {
        activePlayer++;

        if (ActivePlayer % players.Length == GameObject.Find("PlayerNetwork").GetComponent<PlayerNetwork>().PlayerID)
        {
            GameObject.Find("btnLaunch").GetComponent<Button>().interactable = true;
        }
        else
        {
            GameObject.Find("btnLaunch").GetComponent<Button>().interactable = false;
        }
    }
}
