/********************************************************************************/
/* Class: PlayerNetwork                                                         */
/*                                                                              */
/* Author: Team Mansa Musa                                                      */
/*                                                                              */
/* Date: 2018-2019                                                              */

using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using System;

public class PlayerNetwork : MonoBehaviourPunCallbacks, IPunObservable, IOnEventCallback
{
    #region constants
    /// <summary>The starting money for each player</summary>
    public const int STARTING_CASH = 10000 - PassGo.PASS_GO_MONEY;

	/// <summary>The meter amount to win</summary>
    public const int WIN_CONDITION = 2000;
#endregion

#region Player attributes (Player properties)
    /// <summary>The name of the player is given when they select a game piece and begin a game. The player's name is that game piece, i.e. UFO or Spaceship.</summary>
    private string playerName = string.Empty;

	/// <summary>Property: The name of the player is given when they select a game piece and begin a game.</summary>
	public string PlayerName
    {
        get
        {
            return playerName;
        }
        set
        {
            playerName = value;
            PhotonNetwork.LocalPlayer.NickName = PlayerName;
        }
    }
	
	/// <summary>The player's Heads Up Display, or HUD.</summary>
	private HUD playerHUD;

    /// <summary>The player controller connected to this player</summary>
    public PlayerController PlayerController;

    /// <summary>Is the player ready to start the game</summary>
    public bool Ready
    {
        get;
        set;
    }

    /// <summary>Enabled to allow the player's next roll of the dice to be doubled</summary>
    private bool doubleRoll;
    public bool DoubleRoll
    {
        get
        {
            return doubleRoll;
        }
        set
        {
            doubleRoll = value;
        }
    }

    /// <summary>Enabled to allow the next time this player is owed money they are owed double</summary>
    private bool doubleRent;
    public bool DoubleRent
    {
        get
        {
            return doubleRent;
        }
        set
        {
            doubleRent = value;

            if (!hashtable.ContainsKey("DoubleRent"))
                hashtable.Add("DoubleRent", DoubleRent);

            hashtable["DoubleRent"] = DoubleRent;
            UpdateStats();
        }
    }

    /// <summary>Enabled to allow the player a second roll</summary>
    public bool rollAgain;
    public bool RollAgain
    {
        get
        {
            return rollAgain;
        }
        set
        {
            rollAgain = value;
        }
    }

    /// <summary>The value to add to the meter for bonuses from chance cards</summary>
    public int  bonusMeter;
    public int BonusMeter
    {
        get
        {
            return bonusMeter;
        }
        set
        {
            bonusMeter += value;
        }
    }

    /// <summary>The unique player ID</summary>
    public int playerID;
    public int PlayerID
    {
        get
        {
            return playerID;
        }
        set
        {
            playerID = value;

            if (!hashtable.ContainsKey("PlayerID"))
                hashtable.Add("PlayerID", PlayerID);

            hashtable["PlayerID"] = PlayerID;
        }
    }

    /// <summary>This is the current position of the player</summary>
    private int boardPosition;
    public int BoardPosition
    {
        get
        {
            return boardPosition;
        }
        set
        {
            boardPosition = value;

            if(!hashtable.ContainsKey("BoardPosition"))
                hashtable.Add("BoardPosition", BoardPosition);
            else
            {
                GamePiece.GetComponent<Movement>().Move(GameData.GameDataInstance.boardSpaces[GameData.GameDataInstance.boardSpaceOrder[BoardPosition]].transform.GetChild(0).position);
            }

            hashtable["BoardPosition"] = BoardPosition;
            UpdateStats();
        }
    }

	/// <summary>Player's bank account. Please use the property instead.</summary>
	private int bankAccount;

	/// <summary>Public accessor of the player's money amount. Use this instead of m_bankAccount</summary>
	public int BankAccount
	{
		get
        {
            return bankAccount;
        }
		set
		{
            // Doesn't let the player have negative money
			if (value >= 0)
                bankAccount = value;
            else
                bankAccount = 0;

            // If the key hasn't been added to the hash table, add it (first time initialization)
            if (!hashtable.ContainsKey("BankAccount"))
                hashtable.Add("BankAccount", BankAccount);

            // Update the hashtable
            hashtable["BankAccount"] = BankAccount;
            UpdateStats();

            // Raise the event to update the player HUD money value
            RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };
            SendOptions sendOptions = new SendOptions { Reliability = true };
            PhotonNetwork.RaiseEvent(PhotonEvents.MONEY_CHANGED, PlayerID, raiseEventOptions, sendOptions);
        }
	}

    private int pieceID;
	public int PieceID
    {
        get
        {
            return pieceID;
        }
        set
        {
            pieceID = value;

            if (!hashtable.ContainsKey("PieceID"))
                hashtable.Add("PieceID", PieceID);

            hashtable["PieceID"] = PieceID;

            UpdateStats();
        }
    }

	/// <summary>Player's meter value. Please use the property TotalMeterValue instead</summary>
	private int totalMeterValue;
	/// <summary>Public accessor of the player's meter amount.false Use this instead of totalMeterAmount</summary>
	public int TotalMeterValue
	{
		get { return totalMeterValue; }
		set
		{
			if (value >= 0)
				totalMeterValue = value;
            
            if (!hashtable.ContainsKey("TotalMeterValue"))
                hashtable.Add("TotalMeterValue", TotalMeterValue);

            hashtable["TotalMeterValue"] = TotalMeterValue;
            UpdateStats();

            print("current totalmetervalue: " + totalMeterValue);

            // Raise the event to update the player HUD meter value
            RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };
            SendOptions sendOptions = new SendOptions { Reliability = true };
            PhotonNetwork.RaiseEvent(PhotonEvents.METER_CHANGED, PlayerID, raiseEventOptions, sendOptions);

            if (TotalMeterValue >= WIN_CONDITION)
            {
                PhotonNetwork.RaiseEvent(PhotonEvents.PLAYER_HAS_WON, PlayerID, raiseEventOptions, sendOptions);
            }
        }
    }

    /// <summary>Boolean that lets the player know when it's their turn.</summary>
    /// We may turn back to property once turn system is working.
    private bool isTurn;
    public bool IsTurn
    {
        get
        {
            return isTurn;
        }
        set
        {
            isTurn = value;

            if (!hashtable.ContainsKey("IsTurn"))
                hashtable.Add("IsTurn", IsTurn);

            hashtable["IsTurn"] = IsTurn;
        }
    }

    /// <summary>The prefab that the user selects as their game piece.</summary>
    public GameObject GamePiece
    {
        get;
        set;
    }

    public List<Property> MyProperties
    {
        get;
        set;
    }
    

#endregion

#region Photon Networking stuff
    /// <summary>Custom properies associated with the player. These are synced across the network.</summary>
    private Hashtable hashtable = new Hashtable();

    /// <summary>Who knows what this is for...</summary>
    private PhotonView photonView;

    /// <summary>The purpose for this is unknown at the moment.</summary>
    public static PlayerNetwork Instance;

    /// <summary>The purpose for this is unknown at the moment.</summary>
    public Property player1MostRecent;

    /// <summary>The purpose for this is unknown at the moment.</summary>
    public Property player2MostRecent;

    /// <summary>The purpose for this is unknown at the moment.</summary>
    public int randomPlayerBoardPosition = 2;

    public int localPlayerBoardPosition;
    
    public void OnEvent(EventData photonEvent)
    {
        switch (photonEvent.Code)
        {
            case PhotonEvents.TAKE_MONEY_FROM_OTHERS:

                print("Bank Account: " + BankAccount);

                // Everyone pays the tax no matter what
                BankAccount -= ((int[])(photonEvent.CustomData))[2];

                // If this player is the poorest player, he gets the charity
                if (((int[])(photonEvent.CustomData))[0] == PlayerID)
                    BankAccount += ((int[])(photonEvent.CustomData))[1];
                break;

            case PhotonEvents.GIVE_MONEY_TO_OTHERS:
                // If the player landed on the chance that gives money to others, subtract the total of all the taxes from bank account
                if (((int[])(photonEvent.CustomData))[0] == PlayerID)
                    BankAccount -= ((int[])(photonEvent.CustomData))[1];
                // Else, add the tax to the bank account
                else
                    BankAccount += ((int[])(photonEvent.CustomData))[2];
                break;

            case PhotonEvents.COLLECT_FROM_WEALTHY:
                // If you are the player who landed on the chance, collect your money from each individual person
                if(((int[])(photonEvent.CustomData))[0] == PlayerID)
                    BankAccount += ((int[])(photonEvent.CustomData))[1];
                break;

            case PhotonEvents.PROPERTY_TAX:
                // If you own more than three properties, pay the person who landed on the chance card
                // Subtract the tax from your bank account
                // Raise the event to send the money to the player who landed on the chance
                if (MyProperties.Count >= 3)
                {
                    BankAccount -= ((int[])(photonEvent.CustomData))[1];

                    RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };
                    SendOptions sendOptions = new SendOptions { Reliability = true };
                    PhotonNetwork.RaiseEvent(PhotonEvents.COLLECT_FROM_WEALTHY, photonEvent.CustomData, raiseEventOptions, sendOptions);
                }
                break;

            case PhotonEvents.SWAP_POSITIONS1:
                if (((int[])photonEvent.CustomData)[0] == PlayerID)
                {
                    localPlayerBoardPosition = this.BoardPosition;
                }

                if (((int[])photonEvent.CustomData)[1] == PlayerID)
                {
                    randomPlayerBoardPosition = this.BoardPosition;
                }

                break;

            case PhotonEvents.SWAP_POSITIONS2:
                GameData GameDataInstance = GameObject.Find("GameData").GetComponent<GameData>();

                print("SWAP2");

                // If you are the local player
                if (((int[])photonEvent.CustomData)[1] == PlayerID)
                {
                    boardPosition = localPlayerBoardPosition;
                    GamePiece.GetComponent<Movement>().Teleport(GameDataInstance.boardSpaces[GameDataInstance.boardSpaceOrder[this.BoardPosition]].transform.GetChild(0).position);
                }

                // If you are the random player
                if (((int[])photonEvent.CustomData)[0] == PlayerID)
                {
                    boardPosition = localPlayerBoardPosition;
                    GamePiece.GetComponent<Movement>().Teleport(GameDataInstance.boardSpaces[GameDataInstance.boardSpaceOrder[this.BoardPosition]].transform.GetChild(0).position);
                }

                //                GamePiece.GetComponent<Movement>().Teleport(GameDataInstance.boardSpaces[GameDataInstance.boardSpaceOrder[this.BoardPosition]].transform.GetChild(0).position);

                break;

            case PhotonEvents.ADD_SPACES:
                // Add # amount of board spaces to the calling players board position

                if(((int[])photonEvent.CustomData)[0] == PlayerID)
                {
                    GameData GameDataInstance1 = GameObject.Find("GameData").GetComponent<GameData>();
                    boardPosition += ((int[])(photonEvent.CustomData))[1];
                    GamePiece.GetComponent<Movement>().Teleport(GameDataInstance1.boardSpaces[GameDataInstance1.boardSpaceOrder[this.BoardPosition]].transform.GetChild(0).position);
                    hashtable["BoardPosition"] = BoardPosition;
                    UpdateStats();
                }
                break;

            case PhotonEvents.NEW_POSITION:
                // Calling player is teleported to a specific board position

                if (((int[])(photonEvent.CustomData))[0] == PlayerID)
                {
                    GameData GameDataInstance2 = GameObject.Find("GameData").GetComponent<GameData>();
                    boardPosition = ((int[])(photonEvent.CustomData))[1];
                    GamePiece.GetComponent<Movement>().Teleport(GameDataInstance2.boardSpaces[GameDataInstance2.boardSpaceOrder[this.BoardPosition]].transform.GetChild(0).position);
                    hashtable["BoardPosition"] = BoardPosition;
                    UpdateStats();
                }
                break;

            case PhotonEvents.SWAP_PLANETS1:
                // Stores the most recent planet owned by each player into a variable
                if(((int[])(photonEvent.CustomData))[0] == PlayerID)
                {
                    player1MostRecent = MyProperties[MyProperties.Count - 1];
                }

                if (((int[])(photonEvent.CustomData))[1] == PlayerID)
                {
                    player2MostRecent = MyProperties[MyProperties.Count - 1];
                }

                break;

            case PhotonEvents.SWAP_PLANETS2:

                // Stores a temporary copy so we can grab who owns it.
                Property tempForOwnedBy = player1MostRecent;

                // Uses the stored planets to swap the pieces
                if(((int[])(photonEvent.CustomData))[0] == PlayerID)
                {
                    MyProperties.Add(player2MostRecent);
                    player1MostRecent.OwnedBy = player2MostRecent.OwnedBy;
                    GameFlow.GameFlowInstance.UpdateRent(player1MostRecent);
                }

                if (((int[])(photonEvent.CustomData))[1] == PlayerID)
                {
                    MyProperties.Add(player1MostRecent);
                    player2MostRecent.OwnedBy = tempForOwnedBy.OwnedBy;
                    GameFlow.GameFlowInstance.UpdateRent(player2MostRecent);
                }
                break;

            case PhotonEvents.LOSE_PLANETS:
                if (MyProperties.Count > 0)
                {
                    foreach(Property test in MyProperties)
                    {
                        print(test);
                    }
                    print(MyProperties);
                    print(MyProperties.Count - 1);
                    Property removedProperty = MyProperties[MyProperties.Count - 1];
                    removedProperty.IsOwned = false;
                    removedProperty.OwnedBy = null;
                    GameFlow.GameFlowInstance.UpdateRent(removedProperty);
                    print(removedProperty.ToString());

                    MyProperties.Remove(removedProperty);
                    
                    TotalMeterValue -= removedProperty.meterValue;
                }
                break;

            case PhotonEvents.LOSE_YOUR_PLANET:
                if(((int[])(photonEvent.CustomData))[1] == PlayerID)
                {
                    Property removedProperty = MyProperties[MyProperties.Count - 1];
                    removedProperty.IsOwned = false;
                    removedProperty.OwnedBy = null;
                    GameFlow.GameFlowInstance.UpdateRent(removedProperty);
                    
                    MyProperties.Remove(removedProperty);
                    totalMeterValue -= removedProperty.meterValue;
                }
                break;

            case PhotonEvents.PAY_RENT:
                if (Convert.ToString(((object[])(photonEvent.CustomData))[0]) == PlayerName)
                {
                    BankAccount += (Convert.ToInt32(((object[])(photonEvent.CustomData))[1]));
                    print("paying rent: " + ((object[])(photonEvent.CustomData))[1]);

                    foreach(Property property in MyProperties)
                    {
                        property.DoubleRent = false;
                    }
                }
                break;

            case PhotonEvents.GIVE_PROPERTY:
                if (Convert.ToString(((object[])(photonEvent.CustomData))[0]) == PlayerName)
                {
                    Property property = GameObject.Find("GameData").GetComponent<GameData>().boardSpaces[(Convert.ToInt32(((object[])(photonEvent.CustomData))[1]))].GetComponent<Property>();

                    PlayerController.AddProperty(property);
                }
                break;
        }
    }
#endregion

#region Unity Callbacks
    private void Awake()
    {
        if (!Instance)
        {
            Instance = this;
        }

        photonView = GetComponent<PhotonView>();
        Init();
    }

    private void Start()
	{
        // Remove duplicate instances of this class
        PlayerNetwork[] allInstances = (PlayerNetwork[])FindObjectsOfType(typeof(PlayerNetwork));
        if (allInstances.Length > 1)
        {
            for (int i = 1; i < allInstances.Length; i++)
            {
                Destroy(allInstances[i].gameObject);
            }
        }

        PlayerController = GetComponent<PlayerController>();

        MyProperties = new List<Property>();
	}
#endregion
	
	/// <summary>Initialize the player information, DO NOT CALL.</summary>
	private void Init()
	{
        IsTurn = false;
        TotalMeterValue = 0;
        BankAccount = STARTING_CASH;
        BoardPosition = 0;
        BonusMeter = 0;
        RollAgain = false;
        DoubleRent = false;
        DoubleRoll = false;
        Ready = false;
        PieceID = -1;

        UpdateStats();
        PhotonNetwork.AuthValues = new AuthenticationValues(userId: UnityEngine.Random.Range(0, 10000).ToString());
	}

	/// <summary>Update the player's stats to the server. Call before changing turns.</summary>
	public void UpdateStats()
	{
		PhotonNetwork.LocalPlayer.SetCustomProperties(hashtable);
	}

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        throw new System.NotImplementedException();
    }
}
