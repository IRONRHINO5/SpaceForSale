using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;

public class CharacterSelectionCanvas : MonoBehaviourPunCallbacks, IPunObservable, IOnEventCallback, IInRoomCallbacks
{
	// Constants
	private const int MIN_PLAYERS = 3;

	/// <summary>The player's singleton</summary>
	private PlayerNetwork player;

	// Game Piece Buttons
	private Button btnRocket;
	private Button btnRayGun;
	private Button btnUfo;
	private Button btnHelmet;
	private Button btnSatellite;
	private Button btnFlag;

	private Button btnLeave;
    private Button btnReady;

	private GameObject PlayerList;
	private Dictionary<int, Photon.Realtime.Player> NetworkPlayers;

	private InputField txtEgg;
	private bool bEgg = true;
	private const string EGG_TEXT = "UUDDLRLRBA"; // The Konami Code

    private Text   txtRoomNumber;

    private bool[] playersReady = new bool[6];

	[SerializeField]
	private GameObject PlayerSelectionPrefab;

	private List<GameObject> PlayerListPrefabs = new List<GameObject>();

	[SerializeField]
	private List<Sprite> GamePieceSprites;

    int playerLeaveNum = -1;

	private enum GamePiecePrefabNames
	{
		Rocket, RayGun, Ufo, Helmet, Satellite, Flag, Egg
	};

	private List<PlayerNetwork> players;

#region Unity Callbacks

	// Use this for initialization
	void Start ()
	{
		PhotonNetwork.AutomaticallySyncScene = true;
		players     = new List<PlayerNetwork>();
		
		// Find and set the player
		player      = GameObject.Find("PlayerNetwork").GetComponent<PlayerNetwork>();

		// Add the player to the current player list
		players.Add(player);

		// Find and set the game piece buttons
		btnRocket     = GameObject.Find("btnRocket").GetComponent<Button>();
		btnRayGun     = GameObject.Find("btnRayGun").GetComponent<Button>();
		btnUfo        = GameObject.Find("btnUfo").GetComponent<Button>();
		btnHelmet     = GameObject.Find("btnHelmet").GetComponent<Button>();
		btnSatellite  = GameObject.Find("btnSatellite").GetComponent<Button>();
		btnFlag       = GameObject.Find("btnFlag").GetComponent<Button>();
		btnLeave      = GameObject.Find("btnLeave").GetComponent<Button>();
        btnReady      = GameObject.Find("btnReady").GetComponent<Button>();
        txtRoomNumber = GameObject.Find("RoomNumber").GetComponent<Text>();
		PlayerList    = GameObject.Find("PlayerList");
        txtEgg        = GameObject.Find("txtEgg").GetComponent<InputField>();

		txtEgg.gameObject.SetActive(false);
		bEgg = !bEgg;

		// Set the button OnClick() events
		btnRocket.onClick.AddListener(btnRocketOnClick);
		btnRayGun.onClick.AddListener(btnRayGunOnClick);
		btnUfo.onClick.AddListener(btnUfoOnClick);
		btnHelmet.onClick.AddListener(btnHelmetOnClick);
		btnSatellite.onClick.AddListener(btnSatelliteOnClick);
		btnFlag.onClick.AddListener(btnFlagOnClick);

		btnLeave.onClick.AddListener(btnLeaveOnClick);
        btnReady.onClick.AddListener(btnReadyOnClick);

		btnReady.interactable = false;
	}

	private void Awake()
	{
        for (int i = 0; i < playersReady.Length; i++)
        {
            playersReady[i] = false;
        }
	}

	private void Update()
	{
		if(Input.GetKeyDown(KeyCode.BackQuote))
		{
			bEgg = !bEgg;
			txtEgg.gameObject.SetActive(bEgg);
		}

		if(bEgg)
		{
			if(Input.GetKeyDown(KeyCode.UpArrow))
				txtEgg.text += "U";
			if(Input.GetKeyDown(KeyCode.DownArrow))
				txtEgg.text += "D";
			if(Input.GetKeyDown(KeyCode.LeftArrow))
				txtEgg.text += "L";
			if(Input.GetKeyDown(KeyCode.RightArrow))
				txtEgg.text += "R";
			if(Input.GetKeyDown(KeyCode.A))
				txtEgg.text += "A";
			if(Input.GetKeyDown(KeyCode.B))
				txtEgg.text += "B";
			if(Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
			{
				if(txtEgg.text == EGG_TEXT)
				{
					// Secret Stuff
					player.PlayerName = "Zorro";
					player.PieceID = (int)GamePiecePrefabNames.Egg;
					btnReady.interactable = true;
                    GameObject.Find("MouseClick").GetComponent<MouseClickBehavior>().clickzorro();
					CallToUpdateList();
                    bEgg = !bEgg;
                    txtEgg.gameObject.SetActive(bEgg);
                }
				txtEgg.text = "";
			}
		}
	}

#endregion

#region UI Button OnClick() Functions
	
	public void btnRocketOnClick()
	{
        player.PlayerName = "Rocket";
        player.PieceID = (int)GamePiecePrefabNames.Rocket;
		btnReady.interactable = true;
        CallToUpdateList();
	}

	public void btnRayGunOnClick()
	{
        player.PlayerName = "Raygun";
        player.PieceID = (int)GamePiecePrefabNames.RayGun;
		btnReady.interactable = true;
        CallToUpdateList();
	}

	public void btnUfoOnClick()
	{
		player.PlayerName = "UFO";
        player.PieceID = (int)GamePiecePrefabNames.Ufo;
		btnReady.interactable = true;
        CallToUpdateList();
	}

	public void btnHelmetOnClick()
	{
        player.PlayerName = "Helmet";
        player.PieceID = (int)GamePiecePrefabNames.Helmet;
		btnReady.interactable = true;
        CallToUpdateList();
	}

	public void btnSatelliteOnClick()
	{
        player.PlayerName = "Satellite";
        player.PieceID = (int)GamePiecePrefabNames.Satellite;
		btnReady.interactable = true;
        CallToUpdateList();
	}

	public void btnFlagOnClick()
	{
		player.PlayerName = "Flag";
        player.PieceID = (int)GamePiecePrefabNames.Flag;
		btnReady.interactable = true;
        CallToUpdateList();
	}

	private void btnLeaveOnClick()
	{
        foreach (KeyValuePair<int, Photon.Realtime.Player> entry in NetworkPlayers)
        {
            if (entry.Value.ActorNumber == PhotonNetwork.LocalPlayer.ActorNumber)
            {
                playerLeaveNum = entry.Key - 1;
                break;
            }
        }

        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.Others };
        SendOptions sendOptions = new SendOptions { Reliability = true };
        PhotonNetwork.RaiseEvent(PhotonEvents.SELECTION_PLAYER_LEFT, playerLeaveNum, raiseEventOptions, sendOptions);

        player.PlayerName = string.Empty;
        player.PieceID = -1;

        PhotonNetwork.LeaveRoom();
		PhotonNetwork.LoadLevel("GameLobby");
	}

	private void btnReadyOnClick()
	{
    	CallReady();

    	btnRocket.interactable = false; 
    	btnRayGun.interactable = false; 
    	btnUfo.interactable = false;
    	btnHelmet.interactable = false;
    	btnSatellite.interactable = false;
    	btnFlag.interactable = false;
        btnLeave.interactable = false;
    }
#endregion

#region Photon Callbacks
    
	public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) {}

	public override void OnJoinedRoom()
	{
		Debug.Log(PhotonNetwork.CurrentRoom);
        Debug.Log(PhotonNetwork.CurrentRoom.Name);
        txtRoomNumber.text = "Room: " + PhotonNetwork.CurrentRoom.Name;

		NetworkPlayers = PhotonNetwork.CurrentRoom.Players;

		// Init the player list
		for(int i = 0; i < NetworkPlayers.Count; i++)
		{
			PlayerListPrefabs.Add(Instantiate(PlayerSelectionPrefab, PlayerList.transform));
		}
		UpdatePlayerList();
	}

	///<summary>Photon Callback: Called whenever a new player enters a room.</summary>
	public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
	{
        Debug.Log("player joining");
		PlayerListPrefabs.Add(Instantiate(PlayerSelectionPrefab, PlayerList.transform));
		// Only allow the master client to alter the room properties
		if(PhotonNetwork.IsMasterClient)
		{
			// Close the room off if full
			if(PhotonNetwork.CurrentRoom.PlayerCount == PhotonNetwork.CurrentRoom.MaxPlayers)
			{
				Debug.Log("Closing off full room");
				PhotonNetwork.CurrentRoom.IsOpen = false;
                PhotonNetwork.CurrentRoom.IsVisible = false;
				PhotonNetwork.CurrentRoom.RemovedFromList = true;
			}
		}

		UpdatePlayerList();
	}

    public void playerLeft(int ID)
    {
        if (ID != -1)
        {
            Destroy(PlayerList.transform.GetChild(ID).gameObject);
            PlayerListPrefabs.RemoveAt(ID);

            // Only allow the master client to alter the room properties
            if (PhotonNetwork.IsMasterClient)
            {
                // Open up the room if previously full
                if (PhotonNetwork.CurrentRoom.PlayerCount == PhotonNetwork.CurrentRoom.MaxPlayers - 1)
                {
                    PhotonNetwork.CurrentRoom.IsOpen = true;
                    PhotonNetwork.CurrentRoom.IsVisible = true;
                    PhotonNetwork.CurrentRoom.RemovedFromList = false;
                }
            }

            UpdatePlayerList();
        }
        else
        {
            Debug.LogError("Error removing player from room.");
        }

        NetworkPlayers = PhotonNetwork.CurrentRoom.Players;
    }

    public void OnEvent(EventData photonEvent)
    {
        switch(photonEvent.Code)
		{
			case PhotonEvents.SELECTION_PLAYER_CHANGE:
				UpdatePlayerList();
				break;
			case PhotonEvents.SELECTION_PLAYER_READY:
                // Show the person who readied up
                print(PlayerListPrefabs.Count);
                GameObject rowItem = PlayerListPrefabs[(int)(photonEvent.CustomData)];
                rowItem.transform.GetComponent<Image>().color = Color.green;

                // Change the player who readied up to ready
                Debug.Log((int)(photonEvent.CustomData));
				playersReady[(int)(photonEvent.CustomData)] = true;

                // Loop checking if everyone is ready, returning if no one is ready
                for (int i = 0; i < PhotonNetwork.CurrentRoom.PlayerCount; i++)
                {
                    if (PhotonNetwork.CurrentRoom.PlayerCount >= MIN_PLAYERS)
                    {
                        if (!playersReady[i])
                            return;
                    }
                    else
                    {
                        GameObject.Find("playerMin").GetComponent<Text>().text = "Three players are required to play";
                        return;
                    }
                    GameObject.Find("playerMin").GetComponent<Text>().text = string.Empty;
                }

				if(PhotonNetwork.IsMasterClient)
				{
                	PhotonNetwork.CurrentRoom.IsOpen = false;
                	PhotonNetwork.CurrentRoom.IsVisible = false;
					PhotonNetwork.CurrentRoom.RemovedFromList = true;
				}
                RoomNetwork.StartGame();
				break;
            case PhotonEvents.SELECTION_PLAYER_LEFT:
                playerLeft((int)(photonEvent.CustomData));
                UpdatePlayerList();
                break;
        }
    }

#endregion
	
	private void CallToUpdateList()
	{
		RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All};
		SendOptions sendOptions = new SendOptions {Reliability = true};
		PhotonNetwork.RaiseEvent(PhotonEvents.SELECTION_PLAYER_CHANGE, null, raiseEventOptions, sendOptions);
		Debug.Log("Event #1 Sent :D");
	}

	private void CallReady()
	{
		RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All};
		SendOptions sendOptions = new SendOptions {Reliability = true};
		PhotonNetwork.RaiseEvent(PhotonEvents.SELECTION_PLAYER_READY, GameObject.Find("PlayerNetwork").GetComponent<PlayerNetwork>().PlayerID, raiseEventOptions, sendOptions);
        btnReady.GetComponent<Image>().color = Color.green;
        Debug.Log("Event #2 Sent :D");
	}

	public void UpdatePlayerList()
	{
		// Dictionary<int, Player(Photon's class)>
		NetworkPlayers = PhotonNetwork.CurrentRoom.Players;
        Photon.Realtime.Player[] playerList = PhotonNetwork.PlayerList;
		//PrintDictionary();
		
		ResetUI();

		foreach(GameObject rowItem in PlayerListPrefabs)
		{
			int pieceIndex;

			// Because an exception is thrown:
			try
			{
				// This IS an integer, whether C# likes it or not!
				pieceIndex = (int)(playerList[PlayerListPrefabs.IndexOf(rowItem)].CustomProperties["PieceID"]);
			}
			catch(InvalidCastException e)
			{
				Debug.LogError("Error! Cannot cast Piece ID: " + e.Message);
				Debug.LogWarning("Setting pieceIndex to -1");
				pieceIndex = -1;
			}

            if (pieceIndex >= 0)
            {
                rowItem.transform.GetChild(0).GetComponent<Image>().sprite = GamePieceSprites[pieceIndex];

                // The if checks that the player isn't readied up
                if (rowItem.transform.GetComponent<Image>().color != Color.green)
                {
                    rowItem.transform.GetComponent<Image>().color = Color.white; // make the player name and piece choice visible 
                }
            }

			// Disable buttons for taken game pieces
            switch (pieceIndex)
			{
				case (int)GamePiecePrefabNames.Rocket:
					btnRocket.enabled    = false;
					break;
				case (int)GamePiecePrefabNames.RayGun:
					btnRayGun.enabled    = false;
					break;
				case (int)GamePiecePrefabNames.Ufo:
					btnUfo.enabled       = false;
					break;
				case (int)GamePiecePrefabNames.Helmet:
					btnHelmet.enabled    = false;
					break;
				case (int)GamePiecePrefabNames.Flag:
					btnFlag.enabled      = false;
					break;
				case (int)GamePiecePrefabNames.Satellite:
					btnSatellite.enabled = false;
					break;
				default:
					break;
			}

			rowItem.transform.GetChild(1).GetComponent<Text>().text = playerList[PlayerListPrefabs.IndexOf(rowItem)].NickName;
		}
	}

	private void ResetUI()
	{
		btnRocket.enabled    = true;
		btnRayGun.enabled    = true;
		btnUfo.enabled       = true;
		btnHelmet.enabled    = true;
		btnFlag.enabled      = true;
		btnSatellite.enabled = true;
	}
}
