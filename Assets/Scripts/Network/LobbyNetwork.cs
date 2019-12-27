/********************************************************************************/
/* Class: LobbyNetwork                                                          */
/*                                                                              */
/* Author: Team Mansa Musa                                                      */
/*                                                                              */
/* Date: 2018-2019                                                              */
/*                                                                              */
/* Description: This class manages the Lobby and its UI. It connects the player */
/*     to the Photon Network, manages the room list, and connects the player to */
/*     a room, whether new or existing.                                         */
/*                                                                              */
/* Notes:                                                                       */
/*     * This uses Photon PUN v2, not PUN v1 (aka Classic)                      */
/*     * As of the writing of this class, there is no way to ask the server for */
/*       the room list manually. The only way to do so is edit the server code  */
/*       and compile. There is a "hack" and it's to leave and rejoin the lobby. */
/********************************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class LobbyNetwork : MonoBehaviourPunCallbacks
{
	// Canvas Buttons
	private Button btnCreateRoom;
    private Button btnRandomJoin;
	private Button btnReturn;
	private Button btnRefresh;

	private GameObject txtServerConnect;

	private const int ROOM_TTL = 0;
	private const int MAX_ROOM_TTL = 60000;

	// Other Canvas Objects
	private GameObject RoomListGroup; // The area where the room buttons are put (grandchild of the Scroll view)

	// Room List Item prefab
	[SerializeField]
	private GameObject _roomListingPrefab;
	private GameObject RoomListingPrefab
	{
		get { return _roomListingPrefab;}
	}

	// List of all the room buttons
	private List<RoomListItem> _roomListingButtons = new List<RoomListItem>();
	private List<RoomListItem> RoomListingButtons {get{return _roomListingButtons;}}

#region Unity Callbacks

	/// <summary>Unity Callback: Used for initialization, DO NOT CALL!</summary>
	private void Start ()
	{
		// Clean up extra DDOL's and offer your first born to the unknowns of Photon
		DDOL[] hoopla = (DDOL[])FindObjectsOfType(typeof(DDOL));
		if(hoopla.Length > 1)
		{
			for(int i = 1; i < hoopla.Length; i++)
			{
				if(hoopla[i].gameObject.transform.childCount == 0)
					Destroy(hoopla[i].gameObject);
			}
		}

		txtServerConnect = GameObject.Find("txtServerConnect");

		// Find the UI buttons and assign them to the variables
		// and assign their OnClick() events
		btnCreateRoom = GameObject.Find("btnCreate").GetComponent<Button>();
		btnCreateRoom.onClick.AddListener(btnCreateRoomOnClick);

        btnRandomJoin = GameObject.Find("btnRandomJoin").GetComponent<Button>();
        btnRandomJoin.onClick.AddListener(btnRandomJoinOnClick);

		btnReturn = GameObject.Find("btnReturn").GetComponent<Button>();
		btnReturn.onClick.AddListener(btnReturnOnClick);

		btnRefresh = GameObject.Find("btnRefresh").GetComponent<Button>();
		btnRefresh.onClick.AddListener(btnRefreshOnClick);

		// Find the Scroll view's content element:
		RoomListGroup = GameObject.Find("RoomListContent");

		// Disable buttons until connected to server
		btnRefresh.interactable    = false;
		btnRandomJoin.interactable = false;
		btnCreateRoom.interactable = false;

		// If not connected to Photon, connect using the PhotonServerSettings asset
		if(!PhotonNetwork.IsConnected)
		{
			Debug.Log("Connecting to network...");
			PhotonNetwork.ConnectUsingSettings();
		}
	}

#endregion

#region Photon Callbacks

	/// <summary>Photon Callback: Called for when the client is connected to the master server, DO NOT CALL!</summary>
	public override void OnConnectedToMaster()
	{
		Debug.Log("Connection to server successful!");
		PhotonNetwork.JoinLobby(TypedLobby.Default);
	}

	/// <summary>Photon Callback: Called when the client is connected to a lobby, DO NOT CALL!</summary>
	public override void OnJoinedLobby()
	{
		Debug.Log("Joined Lobby!");
		txtServerConnect.SetActive(false);
		btnRefresh.interactable    = true;
		btnRandomJoin.interactable = true;
		btnCreateRoom.interactable = true;
	}

	/// <summary>Photon Callback: Called when a room cannot be created. The message variable is helpful. DO NOT CALL!</summary>
	public override void OnCreateRoomFailed(short code, string message)
	{
		Debug.LogError("Create room failed: " + message);
	}

	/// <summary>Photon Callback: Called when a room is successfully created. The player is automatically joined to that room. DO NOT CALL!</summary>
	public override void OnCreatedRoom()
	{
		print("Room created successfully");
		// Send player to selection screen
		// SceneManager.LoadScene("testselect");
		PhotonNetwork.LoadLevel("testselect");
	}

	/// <summary>PUN Callback when entering lobby. Cannot request room list for whatever reason!?!?</summary>
	public override void OnRoomListUpdate(List<RoomInfo> roomList)
	{
		print("Updating Room List");
		print("Room list size: " + roomList.Count);
		foreach(RoomInfo room in roomList)
		{
			int button_index = RoomListingButtons.FindIndex(x => x.RoomName == room.Name);
			if(!room.IsOpen || !room.IsVisible)
			{
				if(button_index != -1)
				{
					RemoveRoom(room, button_index);
				}
			}
			else if(room.RemovedFromList)
			{
				// Room is either full or hidden & the room button exists
				if(button_index != -1)
				{
					RemoveRoom(room, button_index);
				}
			}
			else
			{
				// Room is to be displayed
				if(button_index == -1)
				{
					// Create new room
					AddRoom(room);
				}
				else
				{
					// Update Room
					RoomListItem roomItem = RoomListingButtons[button_index];
					roomItem.SetRoomNameText(room);
					roomItem.Updated = true;
				}
			}
		}

		// Search for and remove old buttons:
		foreach(RoomListItem room in RoomListingButtons)
		{
			if(room.Updated)
				room.Updated = false;
			else
				RemoveRoom(room);
		}
	}

#endregion

#region UI Button OnClick() Functions

	/// <summary>Button Event: Creates new room with a random number as the name.</summary>
	public void btnCreateRoomOnClick()
	{
		RoomOptions options = new RoomOptions();
		options.MaxPlayers = 6;
		options.EmptyRoomTtl = ROOM_TTL;
		string temp_name = Random.Range(1000, 9999).ToString();

		if(PhotonNetwork.CreateRoom(temp_name, options))
		{
			print("Create room successfully sent");
		}
		else
		{
			print("Create room failed to send");
		}
	}

    public void btnRandomJoinOnClick()
    {
		if(RoomListingButtons.Count == 0)
		{
			// Add a room
			btnCreateRoomOnClick();
		}
		else
		{
			// Join an open room
        	btnJoinRoomClick(RoomListingButtons[0].RoomName);
		}
    }

	/// <summary>Button Event: Join the selected room from the list</summary>
	public void btnJoinRoomClick(string roomName)
	{
		PhotonNetwork.JoinRoom(roomName);
		print("Joining: " + roomName);
		SceneManager.LoadScene("TestSelect");
	}

	/// <summary>Button Event: Disconnect from Photon and go back to main screen</summary>
	public void btnReturnOnClick()
	{
		Destroy(GameObject.Find("DDOL"));
		PhotonNetwork.LeaveLobby();
		PhotonNetwork.Disconnect();
		SceneManager.LoadScene("MainMenu");
	}

	/// <summary>Button Event: Hack to refresh the room list</summary>
	public void btnRefreshOnClick()
	{
		PhotonNetwork.LeaveLobby();
		PhotonNetwork.JoinLobby();
	}

#endregion

#region Room List Management Functions

	/// <summary>Add a new room to the list of rooms</summary>
	private void AddRoom(RoomInfo room)
	{
		Debug.Log("Adding room# " + room.Name);
		GameObject RoomListObj = Instantiate(RoomListingPrefab);           // Instantiate a new list object
		RoomListObj.transform.SetParent(RoomListGroup.transform, false);   // Set ^'s parent to the Room List Group Canvas object
		RoomListItem roomItem = RoomListObj.GetComponent<RoomListItem>();  // Get the RoomListItem script from the button
		roomItem.Updated = true;                                           // House keeping: set the updated property so it doesn't get deleted later
		roomItem.SetRoomNameText(room);                                    // Send the Room's name to the button
		RoomListingButtons.Add(roomItem);                                  // Add the button to the list
	}

	/// <summary>Remove a room from the list of rooms</summary>
	private void RemoveRoom(RoomInfo room, int button_index)
	{
		// Remove Button:
		Debug.Log("Removing room #" + room.Name);
		GameObject roomObj = RoomListingButtons[button_index].gameObject; // Get the button object
		RoomListingButtons.Remove(RoomListingButtons[button_index]);      // Remove the room from the local list
		Destroy(roomObj);                                                 // Destroy the instantiated object
	}

	/// <summary>Remove a room from the list of rooms</summary>
	private void RemoveRoom(RoomListItem room)
	{
		RoomListingButtons.Remove(room); // Remove the room from the local list
		Destroy(room.gameObject);        // Destroy the instantiated object
	}

#endregion
}
