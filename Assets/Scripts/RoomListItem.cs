/********************************************************************************/
/* Class: RoomListItem                                                          */
/*                                                                              */
/* Author: Team Mansa Musa                                                      */
/*                                                                              */
/* Date: 2018-2019                                                              */
/*                                                                              */
/* Description: This class manages the button prefab for the room listing.      */
/*                                                                              */
/* Notes:                                                                       */
/********************************************************************************/

using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class RoomListItem : MonoBehaviour
{
	public string RoomName {get; private set;}
	public RoomInfo roomInfo {get; private set;}

	[SerializeField]
	private GameObject _lobbyNetwork;
	private GameObject m_LobbyNetwork { get{return _lobbyNetwork;}}

	public bool Updated {get; set;}

#region Unity Callbacks

	/// <summary>Unity Callback: Called after Awake(). Used to initialize variables that require accessing other objects. DO NOT CALL!</summary>
	private void Start()
	{
		_lobbyNetwork = GameObject.Find("LobbyNetwork").gameObject;
		GetComponent<Button>().onClick.AddListener(() => m_LobbyNetwork.GetComponent<LobbyNetwork>().btnJoinRoomClick(RoomName)); // This notation allows to pass a function that requires a parameter. NOTE THE PARANTHESES!
	}
	
	/// <summary>Unity Callback: Called first, and before the game starts. Used to initialize this object's variables independent on other objects. DO NOT CALL!</summary>
	private void Awake()
	{
		
	}

	/// <summary>Unity Callback: Called when the object is destroyed via Destroy(GameObject). DO NOT CALL!</summary>
	private void OnDestroy()
	{
		Button button = GetComponent<Button>();
		button.onClick.RemoveAllListeners();
	}

#endregion

	public void SetRoomNameText(RoomInfo room)
	{
		roomInfo = room;
		RoomName = roomInfo.Name;
		// Set the text box's text to display: Room No. 1234   (2/4)
		this.GetComponentInChildren<Text>().text = "Room No. " + RoomName + "    (" + room.PlayerCount + "/" + room.MaxPlayers + ")";
	}
}
