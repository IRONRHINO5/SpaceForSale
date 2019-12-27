using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;
using UnityEditor;

public class ChanceFunctions : MonoBehaviourPunCallbacks, IPunObservable, IOnEventCallback
{
    private int currentCard;

    IList<ChanceCard> chanceCards;

    public PhotonView PhotonView;

    public GameData GameDataInstance;

    private Photon.Realtime.Player[] players;

    [SerializeField]
    private GameObject chanceCardPrefab;

    [SerializeField]
    private Material GoldMaterial;

    private IEnumerator timer;

    private GameObject myChanceCard;

    PlayerNetwork player;

    int[] chanceCardNumbers;

    void Start()
    {
        players = PhotonNetwork.PlayerList;

        player = GameObject.Find("PlayerNetwork").GetComponent<PlayerNetwork>();

        chanceCards = new List<ChanceCard>();

        PhotonView = gameObject.GetComponent<PhotonView>();

        timer = StartCountdown(7);

#region Chance Card Initializations
        // Chance cards that add money
        chanceCards.Add(new ChanceCard("You sold some spare ship parts online. \n\nCollect 500 Credits.", 1, 500, ChanceCard1Function));
        chanceCards.Add(new ChanceCard("Your colony on Mars has been thriving lately. \n\nCollect 1000 Credits.", 2, 1000, ChanceCard2Function));
        chanceCards.Add(new ChanceCard("Your ship won first place in a race! \n(...in less than 12 parsecs!) \n\nCollect 2000 Credits for winning!", 3, 2000, ChanceCard3Function));
        chanceCards.Add(new ChanceCard("You harvest rare metals from a nearby asteroid field and sell them. \n\nCollect 1500 Credits.", 4, 1500, ChanceCard4Function));
        chanceCards.Add(new ChanceCard("You found the wreck of an old cargo vessel. \n\nCollect 2500 Credits.", 5, 2500, ChanceCard5Function));

        // Chance cards that subtract money
        chanceCards.Add(new ChanceCard("You were involved in a spaceship crash. \n\nPay 3000 Credits for damages.", 6, -3000, ChanceCard6Function));
        chanceCards.Add(new ChanceCard("You ran into some dangerous space pirates threating to destroy your ship. \n\nPay them off with 1500 Credits", 7, -1500, ChanceCard7Function));
        chanceCards.Add(new ChanceCard("You were flying too close to an asteroid belt and damaged your ship. \n\nPay 2000 Credits to fix damages.", 8, -2000, ChanceCard8Function));
        chanceCards.Add(new ChanceCard("You accidentally ejected your escape pod. \n\nPay 2500 Credits for a new one.", 9, -2500, ChanceCard9Function));
        chanceCards.Add(new ChanceCard("Your ship is in need of general repairs. \n\nPay 1000 Credits.", 10, -1000, ChanceCard10Function));
        chanceCards.Add(new ChanceCard("Planet taxes are due! \n\nPay 300 Credits for every planet that you own!", 11, 300, ChanceCard11Function));

        // Chance cards that affect board position
        chanceCards.Add(new ChanceCard("You found some extra fuel for a little boost! \n\nAdvance forward 3 spaces!", 12, 3, ChanceCard12Function));
        //chanceCards.Add(new ChanceCard("Your transporter seems to be malfunctioning. \n\nYour piece trades places with a random player.", 13, 0, ChanceCard13Function));
        chanceCards.Add(new ChanceCard("Mars is in dire need of some colonists! You have been drafted to take on the task! \n\nGo straight to Mars.", 13, 300, ChanceCard13Function));
        chanceCards.Add(new ChanceCard("Who says we can't land on the Sun? \n\nGo straight to the Sun collecting 2000 Credits.", 14, 0, ChanceCard14Function));
        chanceCards.Add(new ChanceCard("Your ship is suddenly pulled back by a nearby planet's gravity. \n\nGo backwards 3 spaces.", 15, -3, ChanceCard15Function));

        // Chance cards that affect owned properties
        chanceCards.Add(new ChanceCard("There's an ominous \"moon\" floating near your planet forcing your people to evacuate. \n\nLose your most recent planet.", 16, 300, ChanceCard16Function));
        //chanceCards.Add(new ChanceCard("You decided to conquer a planet at the same time they tried to conquer your planet. \n\nSwap your most recent planet purchase with another player's most recent planet purchase.", 17, 300, ChanceCard18Function));
        chanceCards.Add(new ChanceCard("With the snap of a finger, half of the universe is affected. \n\nEvery player loses their most recent planet purchase.", 17, 300, ChanceCard17Function));
        chanceCards.Add(new ChanceCard("Time's are tough and you need to raise the income for a little bit. \n\nThe next player to land on one of your planet pays double Credits.", 18, 300, ChanceCard18Function));

        // Chance cards that involve multiple players exchanging money
        chanceCards.Add(new ChanceCard("Galactic empire welfare has been passed by the planetary progressives. \n\nGive 100 credits to poorest player.", 19, 100, ChanceCard19Function));
        chanceCards.Add(new ChanceCard("Your company prepares to launch a car into orbit around Mars and sells tickets to the launch. \n\nCollect 500 Credits from all players.", 20, 500, ChanceCard20Function));
        chanceCards.Add(new ChanceCard("You feel bad for all the other players. \n\nPay each player 300 Credits", 21, 300, ChanceCard21Function));
        chanceCards.Add(new ChanceCard("Take advantage of the rich. \n\nCollect 750 Credits from all players that own 3 or more planets.", 22, 750, ChanceCard22Function));

        // Miscellaneous chance cards
        chanceCards.Add(new ChanceCard("Here is a big boost to get you closer to winning!\n\nAdd 500 to your bonus meter!", 23,  500, ChanceCard23Function)); 
        chanceCards.Add(new ChanceCard("Big Oof. This ain't it chief. \n\n Lose 250 points from your bonus meter!", 24, -250, ChanceCard24Function));  
        chanceCards.Add(new ChanceCard("Here is a little boost to get you closer to winning! \n\nAdd 100 to your bonus meter!", 25,  100,  ChanceCard25Function));  
        chanceCards.Add(new ChanceCard("Oof. That's not good. \n\n Lose 100 from your bonus meter!", 26, -100,  ChanceCard26Function));  
        chanceCards.Add(new ChanceCard("You found some extra fuel for next time! \n\nYour next roll will be doubled!", 27, 300, ChanceCard27Function)); 
        chanceCards.Add(new ChanceCard("You decided the current state of your piece doesn't quite meet your expectations. \n\nSpending half of your Credits decided to cover your piece in solid gold.", 28, 300, ChanceCard28Function)); // Make your piece gold, losing half your Credit
        #endregion

        chanceCardNumbers = new int[chanceCards.Count];

        // only one player needs to shuffle cards
        if(player.PlayerID == 0)
            chanceCards.Shuffle();

        for(int i = 0; i < chanceCards.Count; i++)
        {
            chanceCardNumbers[i] = chanceCards[i].ChanceCardID;
        }

        if (player.PlayerID == 0)
        {
            RaiseEventOptions raiseEventOptions1 = new RaiseEventOptions { Receivers = ReceiverGroup.All };
            SendOptions sendOptions1 = new SendOptions { Reliability = true };
            PhotonNetwork.RaiseEvent(PhotonEvents.SYNC_CARDS, chanceCardNumbers, raiseEventOptions1, sendOptions1);
        }

        currentCard = 0;
    }

    #region Chance Card Testing Status
    /*
       Chance 1  - Working
       Chance 2  - Working
       Chance 3  - Working
       Chance 4  - Working
       Chance 5  - Working
       Chance 6  - Working
       Chance 7  - Working
       Chance 8  - Working
       Chance 9  - Working
       Chance 10 - Working
       Chance 11 - Working
       Chance 12 - Working
       Chance 13 - Working
       Chance 14 - Working
       Chance 15 - Working
       Chance 16 - Working
       Chance 17 - Working
       Chance 18 - Working as far as we know. Needs tested.
       Chance 19 - Working
       Chance 20 - Working
       Chance 21 - Working
       Chance 22 - Working
       Chance 23 - Working
       Chance 24 - Working
       Chance 25 - Working
       Chance 26 - Working
       Chance 27 - Working
       Chance 28 - Working
    */
    #endregion

    public void ChanceSelection()
    {
        // Display the chance card on screen and initialize it
        myChanceCard = Instantiate(chanceCardPrefab, GameObject.Find("HUD").transform);
        myChanceCard.transform.GetChild(0).GetComponent<Text>().text = chanceCards[currentCard].FlavorText;
        myChanceCard.transform.GetChild(1).GetComponent<Button>().onClick.AddListener(btnDestroyCard);
        
        StartCoroutine("StartCountdown", 15);

    }

    public void btnDestroyCard()
    {
        RaiseEventOptions raiseEventOptions1 = new RaiseEventOptions { Receivers = ReceiverGroup.All };
        SendOptions sendOptions1 = new SendOptions { Reliability = true };
        PhotonNetwork.RaiseEvent(PhotonEvents.CHANCE_SELECT, player.PlayerID, raiseEventOptions1, sendOptions1);

        chanceCards[currentCard].function(player);

        StopCoroutine("StartCountdown");

        Destroy(GameObject.Find("ChanceCardPrefab(Clone)"));

        GameData.GameDataInstance.ActivePlayer++;
    }

    public void RunThroughCards(PlayerNetwork player)
    {
        foreach(ChanceCard card in chanceCards)
            card.function(player);
    }

    /*******************************************************************************
    *                     Start of the Chance Card Functions                       *
    *******************************************************************************/
#region Chance Card Functions
    // Gives a player 500 Credits
    public void ChanceCard1Function(PlayerNetwork player)
    {
        Debug.Log("In chance 1");
        ChangeMoney(player, 500);
        Debug.Log("In chance 1 : adding 500 to bank account. bank account: " + player.BankAccount);

        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };
        SendOptions sendOptions = new SendOptions { Reliability = true };
        PhotonNetwork.RaiseEvent(PhotonEvents.MESSAGE, player.PlayerName + " received 500 credits.", raiseEventOptions, sendOptions);
    }

    // Gives a player 1000 Credits
    public void ChanceCard2Function(PlayerNetwork player)
    {
        Debug.Log("In chance 2");
        ChangeMoney(player, 1000);
        Debug.Log("In chance 2 : adding 1000 to bank account. bank account: " + player.BankAccount);

        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };
        SendOptions sendOptions = new SendOptions { Reliability = true };
        PhotonNetwork.RaiseEvent(PhotonEvents.MESSAGE, player.PlayerName + " received 1,000 credits.", raiseEventOptions, sendOptions);
    }

    // Gives a player 2000 Credits
    public void ChanceCard3Function(PlayerNetwork player)
    {
        Debug.Log("In chance 3");
        ChangeMoney(player, 2000);
        Debug.Log("In chance 3 : adding 2000 to bank account. bank account: " + player.BankAccount);

        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };
        SendOptions sendOptions = new SendOptions { Reliability = true };
        PhotonNetwork.RaiseEvent(PhotonEvents.MESSAGE, player.PlayerName + " received 2,000 credits.", raiseEventOptions, sendOptions);
    }

    // Gives a player 1500 Credits
    public void ChanceCard4Function(PlayerNetwork player)
    {
        Debug.Log("In chance 4");
        ChangeMoney(player, 1500);
        Debug.Log("In chance 4 : adding 1500 to bank account. bank account: " + player.BankAccount);

        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };
        SendOptions sendOptions = new SendOptions { Reliability = true };
        PhotonNetwork.RaiseEvent(PhotonEvents.MESSAGE, player.PlayerName + " received 1,500 credits.", raiseEventOptions, sendOptions);
    }

    // Gives a player 2500 Credits
    public void ChanceCard5Function(PlayerNetwork player)
    {
        Debug.Log("In chance 5");
        ChangeMoney(player, 2500);
        Debug.Log("In chance 5 : adding 2500 to bank account. bank account: " + player.BankAccount);

        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };
        SendOptions sendOptions = new SendOptions { Reliability = true };
        PhotonNetwork.RaiseEvent(PhotonEvents.MESSAGE, player.PlayerName + " received 2,500 credits.", raiseEventOptions, sendOptions);
    }

    // Takes 3000 Credits from a player
    public void ChanceCard6Function(PlayerNetwork player)
    {
        Debug.Log("In chance 6");
        ChangeMoney(player, -3000);
        Debug.Log("In chance 6 : adding -3000 to bank account. bank account: " + player.BankAccount);

        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };
        SendOptions sendOptions = new SendOptions { Reliability = true };
        PhotonNetwork.RaiseEvent(PhotonEvents.MESSAGE, player.PlayerName + " lost 3,000 credits.", raiseEventOptions, sendOptions);
    }

    // Takes 1500 Credits from a player
    public void ChanceCard7Function(PlayerNetwork player)
    {
        Debug.Log("In chance 7");
        ChangeMoney(player, -1500);
        Debug.Log("In chance 7 : adding -1500 to bank account. bank account: " + player.BankAccount);

        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };
        SendOptions sendOptions = new SendOptions { Reliability = true };
        PhotonNetwork.RaiseEvent(PhotonEvents.MESSAGE, player.PlayerName + " lost 1,500 credits.", raiseEventOptions, sendOptions);
    }

    // Takes 2000 Credits from a player
    public void ChanceCard8Function(PlayerNetwork player)
    {
        Debug.Log("In chance 8");
        ChangeMoney(player, -2000);
        Debug.Log("In chance 8 : adding -2000 to bank account. bank account: " + player.BankAccount);

        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };
        SendOptions sendOptions = new SendOptions { Reliability = true };
        PhotonNetwork.RaiseEvent(PhotonEvents.MESSAGE, player.PlayerName + " lost 2,000 credits.", raiseEventOptions, sendOptions);
    }

    // Takes 2500 Credits from a player
    public void ChanceCard9Function(PlayerNetwork player)
    {
        Debug.Log("In chance 9");
        ChangeMoney(player, -2500);
        Debug.Log("In chance 9 : adding -2500 to bank account. bank account: " + player.BankAccount);

        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };
        SendOptions sendOptions = new SendOptions { Reliability = true };
        PhotonNetwork.RaiseEvent(PhotonEvents.MESSAGE, player.PlayerName + " lost 2,500 credits.", raiseEventOptions, sendOptions);
    }

    // Takes 1000 Credits from a player
    public void ChanceCard10Function(PlayerNetwork player)
    {
        Debug.Log("In chance 10");
        ChangeMoney(player, -1000);
        Debug.Log("In chance 10 : adding -1000 to bank account. bank account: " + player.BankAccount);

        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };
        SendOptions sendOptions = new SendOptions { Reliability = true };
        PhotonNetwork.RaiseEvent(PhotonEvents.MESSAGE, player.PlayerName + " lost 1,000 credits.", raiseEventOptions, sendOptions);
    }

    // Pay 300 Credits for every planet that you own
    public void ChanceCard11Function(PlayerNetwork player)
    {
        Debug.Log("In chance 11");

        List <Property> propertyList = player.MyProperties;
        int amountOwed = (propertyList.Count * -300); // Tally up -300 Credits for every planet owned by the calling player
        ChangeMoney(player, amountOwed);
        Debug.Log("In chance 11 : Pay 300 Credits for every planet that you own. bank account: " + player.BankAccount);

        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };
        SendOptions sendOptions = new SendOptions { Reliability = true };
        PhotonNetwork.RaiseEvent(PhotonEvents.MESSAGE, player.PlayerName + " lost " + amountOwed + " credits.", raiseEventOptions, sendOptions);
    }

    // Moves a piece forward 3 spaces
    public void ChanceCard12Function(PlayerNetwork player)
    {
        Debug.Log("In chance 12");
        Debug.Log("In chance 12: Move your piece forward 3 Spaces");
        int spaces = 3;

        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };
        SendOptions sendOptions = new SendOptions { Reliability = true };
        PhotonNetwork.RaiseEvent(PhotonEvents.ADD_SPACES, new int[] { player.PlayerID, spaces }, raiseEventOptions, sendOptions);
        PhotonNetwork.RaiseEvent(PhotonEvents.MESSAGE, player.PlayerName + " moved forward 3 spaces.", raiseEventOptions, sendOptions);

    }

    /* Swaps the pieces of a player and a random player
    public void ChanceCard13Function(PlayerNetwork player)
    {
        int randomPlayer;
        Debug.Log("In chance 13");

        Debug.Log("CHANCE 13 NEEDS FIXING");

        return;
        // Currently not working. Can't find the random player's board location for some reason

        // Get a random player that isn't the current player
        do
        {
            randomPlayer = Random.Range(0, PhotonNetwork.PlayerList.Length);
        } while (randomPlayer == player.PlayerID);

        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };
        SendOptions sendOptions = new SendOptions { Reliability = true };
        PhotonNetwork.RaiseEvent(PhotonEvents.SWAP_POSITIONS1, new int[] {player.PlayerID, randomPlayer}, raiseEventOptions, sendOptions);
        PhotonNetwork.RaiseEvent(PhotonEvents.SWAP_POSITIONS2, new int[] {player.PlayerID, randomPlayer}, raiseEventOptions, sendOptions);
    }
    */

    // Moves a piece to Mars
    public void ChanceCard13Function(PlayerNetwork player)
    {
        Debug.Log("In chance 13");
        Debug.Log("In chance 13: Moving your piece to Mars");
        int marsIndex = 18;

        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };
        SendOptions sendOptions = new SendOptions { Reliability = true };
        PhotonNetwork.RaiseEvent(PhotonEvents.NEW_POSITION, new int[] { player.PlayerID, marsIndex }, raiseEventOptions, sendOptions);
        PhotonNetwork.RaiseEvent(PhotonEvents.MESSAGE, player.PlayerName + " teleported to Mars.", raiseEventOptions, sendOptions);
    }

    // Moves a piece to the Sun
    public void ChanceCard14Function(PlayerNetwork player)
    {
        Debug.Log("In chance 14");
        Debug.Log("In chance 14: Moving your piece to the Sun");
        int sunIndex = 0;

        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };
        SendOptions sendOptions = new SendOptions { Reliability = true };
        PhotonNetwork.RaiseEvent(PhotonEvents.NEW_POSITION, new int[] { player.PlayerID, sunIndex }, raiseEventOptions, sendOptions);
        PhotonNetwork.RaiseEvent(PhotonEvents.MESSAGE, player.PlayerName + " teleported to the Sun.", raiseEventOptions, sendOptions);
    }

    // Moves a piece backwards 3 spaces (Works correctly, however the piece travels forwards and not backwards and it gives an error (See chance cards 12&14))
    public void ChanceCard15Function(PlayerNetwork player)
    {
        Debug.Log("In chance 15");
        Debug.Log("In chance 15: Moving your piece back 3 spaces");
        int spaces = -3;

        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };
        SendOptions sendOptions = new SendOptions { Reliability = true };
        PhotonNetwork.RaiseEvent(PhotonEvents.ADD_SPACES, new int[] {player.PlayerID, spaces }, raiseEventOptions, sendOptions);
        PhotonNetwork.RaiseEvent(PhotonEvents.MESSAGE, player.PlayerName + " moved backwards 3 spaces.", raiseEventOptions, sendOptions);
    }

    // Player loses his most recent planet
    public void ChanceCard16Function(PlayerNetwork player)
    {
        Debug.Log("In chance 16");
        Debug.Log("In chance 16: Calling player loses his most recent planet");

        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };
        SendOptions sendOptions = new SendOptions { Reliability = true };
        PhotonNetwork.RaiseEvent(PhotonEvents.LOSE_YOUR_PLANET, new int[] { player.PlayerID }, raiseEventOptions, sendOptions);
        PhotonNetwork.RaiseEvent(PhotonEvents.MESSAGE, player.PlayerName + " lost their most recent planet.", raiseEventOptions, sendOptions);
    }

    /* Swap your most recent planet purchase with another player's most recent player purchase
    public void ChanceCard18Function(PlayerNetwork player)
    {
        Debug.Log("In chance 18");
        Debug.Log("In chance 18: Swapping calling player's most recent planet with a random player's");
        int randomPlayer;

        // Get a random player that isn't the current player
        do
        {
            randomPlayer = Random.Range(0, PhotonNetwork.PlayerList.Length);
        } while (randomPlayer == player.PlayerID);

        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };
        SendOptions sendOptions = new SendOptions { Reliability = true };
        PhotonNetwork.RaiseEvent(PhotonEvents.SWAP_PLANETS1, new int[] { player.playerID, randomPlayer}, raiseEventOptions, sendOptions);

        

        RaiseEventOptions raiseEventOptions1 = new RaiseEventOptions { Receivers = ReceiverGroup.All };
        SendOptions sendOptions1 = new SendOptions { Reliability = true };
        PhotonNetwork.RaiseEvent(PhotonEvents.SWAP_PLANETS2, new int[] { player.playerID, randomPlayer }, raiseEventOptions1, sendOptions1);

        return;
    }
    */

    // Every player loses their most recent planet purchase
    public void ChanceCard17Function(PlayerNetwork myPlayer)
    {
        Debug.Log("In chance 17");
        Debug.Log("In chance 17: Every player loses their most recent planet.");

        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };
        SendOptions sendOptions = new SendOptions { Reliability = true };
        PhotonNetwork.RaiseEvent(PhotonEvents.LOSE_PLANETS, null, raiseEventOptions, sendOptions);

        PhotonNetwork.RaiseEvent(PhotonEvents.MESSAGE, "Every player lost their most recent planet.", raiseEventOptions, sendOptions);
    }

    // The next player to land on one of your planets pays you double
    public void ChanceCard18Function(PlayerNetwork player)
    {
        Debug.Log("In chance 18");
        foreach(Property planet in player.MyProperties)
        {
            planet.DoubleRent = true;
        }

        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };
        SendOptions sendOptions = new SendOptions { Reliability = true };
        PhotonNetwork.RaiseEvent(PhotonEvents.MESSAGE, "The next player to land on one of " + player.PlayerName + "'s planets owes them double.", raiseEventOptions, sendOptions);
    }

    // Takes 100 Credits from every player and gives it to the poorest player
    public void ChanceCard19Function(PlayerNetwork myPlayer)
    {
        Debug.Log("In chance 19");
        Debug.Log("In chance 19: Taking 100 Credits from every player and giving them to the poorest.");

        int poorPlayer = -1;
        int money = int.MaxValue;
        int charity = 0;
        int tax = 100;
        string poorPlayerName = "";

        foreach (Photon.Realtime.Player player in players)
        {

            // Determine who the poorest player is
            if ((int)player.CustomProperties["BankAccount"] < money)
            {
                poorPlayer = (int)player.CustomProperties["PieceID"];
                poorPlayerName = (string)player.CustomProperties["PlayerName"];
                money = (int)player.CustomProperties["BankAccount"];
            }

            // The charity is determined
            charity += tax;
        }

        // Raise the event that tells everyone to pay the tax and gives the charity to the poorest
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };
        SendOptions sendOptions = new SendOptions { Reliability = true };
        PhotonNetwork.RaiseEvent(PhotonEvents.TAKE_MONEY_FROM_OTHERS, new int[] {poorPlayer, charity, tax}, raiseEventOptions, sendOptions);
        PhotonNetwork.RaiseEvent(PhotonEvents.MESSAGE, poorPlayerName + " received " + tax + " credits from every other player. (Total:" + (charity - tax) + ")", raiseEventOptions, sendOptions);
    }

    // Takes 500 Credits from every player and gives them to the calling player
    public void ChanceCard20Function(PlayerNetwork myPlayer)
    {
        Debug.Log("In chance 20");

        int totalCredits = 0;
        int tax = 500;

        foreach (Photon.Realtime.Player player in players)
        {
            totalCredits += tax;
        }

        Debug.Log("In chance 20: Taking 500 Credits from every player and giving them to the calling player.");
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };
        SendOptions sendOptions = new SendOptions { Reliability = true };
        PhotonNetwork.RaiseEvent(PhotonEvents.TAKE_MONEY_FROM_OTHERS, new int[] { myPlayer.PlayerID, totalCredits, tax }, raiseEventOptions, sendOptions);
        PhotonNetwork.RaiseEvent(PhotonEvents.MESSAGE, player.PlayerName + " received " + tax + " credits from every other player. (Total:" + (totalCredits - tax) + ")" , raiseEventOptions, sendOptions);
    }

    // Give 300 Credits to each player and take away those credits from the calling player
    public void ChanceCard21Function(PlayerNetwork myPlayer)
    {
        Debug.Log("In chance 21");
        Debug.Log("In chance 21: Calling player gives 300 credits to everyone");

        int totalCredits = 0;
        int tax = 300;

        foreach (Photon.Realtime.Player player in players)
        {
            totalCredits += tax;
        }

        // Don't tax the player who landed on the spot
        totalCredits -= tax;

        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };
        SendOptions sendOptions = new SendOptions { Reliability = true };
        PhotonNetwork.RaiseEvent(PhotonEvents.GIVE_MONEY_TO_OTHERS, new int[] { myPlayer.PlayerID, totalCredits, tax }, raiseEventOptions, sendOptions);
        PhotonNetwork.RaiseEvent(PhotonEvents.MESSAGE, player.PlayerName + " paid each player " + tax + " credits. (Total:" + (totalCredits - tax) + ")", raiseEventOptions, sendOptions);
    }

    // Take 750 Credits from every player with 3 or more planets and give them to the calling player
    public void ChanceCard22Function(PlayerNetwork myPlayer)
    {
        Debug.Log("In chance 22");
        Debug.Log("In chance 22: Calling player gets 750 Credits from every player with 3+ Planets");

        int tax = 750;

        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };
        SendOptions sendOptions = new SendOptions { Reliability = true };
        PhotonNetwork.RaiseEvent(PhotonEvents.PROPERTY_TAX, new int[] { myPlayer.PlayerID, tax }, raiseEventOptions, sendOptions);
        PhotonNetwork.RaiseEvent(PhotonEvents.MESSAGE, player.PlayerName + " received 750 credits from every player with 3 or more planets.", raiseEventOptions, sendOptions);
    }

    // Add a big amount to the bonus meter
    public void ChanceCard23Function(PlayerNetwork player)
    {
        Debug.Log("In chance 23");
        changeBonusMeter(player, 500);
        Debug.Log("In chance 23: Adding 500 to Bonus meter: " + player.BonusMeter);

        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };
        SendOptions sendOptions = new SendOptions { Reliability = true };
        PhotonNetwork.RaiseEvent(PhotonEvents.MESSAGE, player.PlayerName + " received 500 meter points.", raiseEventOptions, sendOptions);
    }

    // Subtract a big amount to the bonus meter
    public void ChanceCard24Function(PlayerNetwork player)
    {
        Debug.Log("In chance 24");
        changeBonusMeter(player, -250);
        Debug.Log("In chance 24: Adding -250 to Bonus meter: " + player.BonusMeter);

        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };
        SendOptions sendOptions = new SendOptions { Reliability = true };
        PhotonNetwork.RaiseEvent(PhotonEvents.MESSAGE, player.PlayerName + " lost 250 meter points.", raiseEventOptions, sendOptions);

    }

    // Add a little to the bonus meter
    public void ChanceCard25Function(PlayerNetwork player)
    {
        Debug.Log("In chance 25");
        changeBonusMeter(player, 100);
        Debug.Log("In chance 25: Adding 50 to Bonus meter: " + player.BonusMeter);

        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };
        SendOptions sendOptions = new SendOptions { Reliability = true };
        PhotonNetwork.RaiseEvent(PhotonEvents.MESSAGE, player.PlayerName + " received 100 meter points.", raiseEventOptions, sendOptions);
    }

    // Subtract a little from the bonus meter
    public void ChanceCard26Function(PlayerNetwork player)
    {
        Debug.Log("In chance 26");
        changeBonusMeter(player, -100);
        Debug.Log("In chance 26: Adding -50 to Bonus meter: " + player.BonusMeter);

        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };
        SendOptions sendOptions = new SendOptions { Reliability = true };
        PhotonNetwork.RaiseEvent(PhotonEvents.MESSAGE, player.PlayerName + " lost 100 meter points.", raiseEventOptions, sendOptions);
    }

    // Double the value of the next roll
    public void ChanceCard27Function(PlayerNetwork player)
    {
        Debug.Log("In chance 27");
        player.DoubleRoll = true;
        Debug.Log("In Chance 27: making DoubleRoll = true. DoubleRoll:" + player.DoubleRoll);

        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };
        SendOptions sendOptions = new SendOptions { Reliability = true };
        PhotonNetwork.RaiseEvent(PhotonEvents.MESSAGE, "The next time " + player.PlayerName + " rolls their roll will be doubled.", raiseEventOptions, sendOptions);
    }
    // Make the calling player's piece turn gold
    public void ChanceCard28Function(PlayerNetwork player)
    {
        Debug.Log("In chance 28");

        // Don't allow free gold skin
        if(player.BankAccount > 0)
        {
            player.BankAccount /= 2;             
            player.GamePiece.transform.GetChild(0).GetComponent<Renderer>().material = GoldMaterial;
        }

        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };
        SendOptions sendOptions = new SendOptions { Reliability = true };
        PhotonNetwork.RaiseEvent(PhotonEvents.MESSAGE, player.PlayerName + " lost half of their credits.", raiseEventOptions, sendOptions);
        PhotonNetwork.RaiseEvent(PhotonEvents.MESSAGE, player.PlayerName + " is now gold.", raiseEventOptions, sendOptions);

    }
#endregion
    /*******************************************************************************
    *                      End of the Chance Card Functions                        *
    *******************************************************************************/

#region Utility Functions
    // Function to add/subtract $ from players bank account.
    public void ChangeMoney(PlayerNetwork player, int value)
    {
        player.BankAccount += value;
    }


    // Function to swap positions with another player
    // NEED TO FIX UNLESS BOTTOM IS ONE WE GO WITH
    public void SwapPositions(int player1Index, int player2Index)
    {
        int swap = 0;

        swap = (int)players[player1Index].CustomProperties["BoardPosition"];
        players[player1Index].CustomProperties["BoardPosition"] = (int)players[player2Index].CustomProperties["BoardPosition"];
        players[player2Index].CustomProperties["BoardPosition"] = swap;

        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };
        SendOptions sendOptions = new SendOptions { Reliability = true };
        PhotonNetwork.RaiseEvent(PhotonEvents.PROPERTY_TAX, null, raiseEventOptions, sendOptions);
    }

    // Function to add/subtract a player's piece a certain amount of board spaces.
    public void changeBoardPosition(PlayerNetwork player, int spaces)
    {
        player.BoardPosition += spaces;
        player.GetComponentInParent<Movement>().Teleport(GameDataInstance.boardSpaces[GameDataInstance.boardSpaceOrder[player.BoardPosition]].transform.GetChild(0).position);
    }

    // Function to transport a player to a specific board space.
    public void newBoardPosition(PlayerNetwork player, int planet)
    {
        player.BoardPosition = planet;
        player.GetComponentInParent<Movement>().Teleport(GameDataInstance.boardSpaces[GameDataInstance.boardSpaceOrder[player.BoardPosition]].transform.GetChild(0).position);
    }

    // Player piece loses a planet
    public void losePlanet(PlayerNetwork player, int propertyToLose)
    {
        List<Property> propertyList = player.MyProperties;

        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };
        SendOptions sendOptions = new SendOptions { Reliability = true };
        PhotonNetwork.RaiseEvent(PhotonEvents.LOSE_PLANETS, null, raiseEventOptions, sendOptions);
    }

    // Steal the most recent planet from another player
    public void stealPlanet(PlayerNetwork thief, PlayerNetwork victim)
    {
        List<Property> ThiefPlanets = thief.MyProperties;
        List<Property> VictimPlanets = victim.MyProperties;

        ThiefPlanets.Add(VictimPlanets[victim.MyProperties.Count - 1]);
        VictimPlanets.Remove(VictimPlanets[victim.MyProperties.Count - 1]);

        thief.MyProperties = ThiefPlanets;
        victim.MyProperties = VictimPlanets;

    }

    // Swap the most recent planets between two players
    public void swapPlanets(PlayerNetwork Player1, PlayerNetwork Player2)
    {
        //

        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };
        SendOptions sendOptions = new SendOptions { Reliability = true };
        PhotonNetwork.RaiseEvent(PhotonEvents.SWAP_PLANETS1, null, raiseEventOptions, sendOptions);


        List<Property> Player1Planets = Player1.MyProperties;
        List<Property> Player2Planets = Player2.MyProperties;
        List<Property> SwapPlanets    = Player1.MyProperties;

        Player1Planets.Remove(Player1Planets[Player1.MyProperties.Count - 1]); // Remove the most recent planet from player 1's list 
        Player1Planets.Add(Player2Planets[Player2.MyProperties.Count - 1]);    // Add Player 2's most recent planet to player 1's list

        Player2Planets.Remove(Player2Planets[Player2.MyProperties.Count - 1]); // Remove the most recent planet from player 2's list 
        Player2Planets.Add(SwapPlanets[Player1.MyProperties.Count - 1]);       // Add the temporary's(Player 1's) most recent planet to player 2's list

        // Copy the lists into the player's actual proprty list
        Player1.MyProperties = Player1Planets; // Player 1 Gets the list with Player 2's most recent planet on it
        Player2.MyProperties = Player2Planets; // Player 2 Gets the list with Player 1's most recent planet on it
    }

    public void changeBonusMeter(PlayerNetwork player, int value)
    {
        player.TotalMeterValue += value;
    }

    public float currCountdownValue = 20;
    public IEnumerator StartCountdown(float countdownValue = 10)
    {
        currCountdownValue = countdownValue;
        while (currCountdownValue > 0)
        {
            Debug.Log("Countdown: " + currCountdownValue);
            myChanceCard.transform.GetChild(1).GetComponent<Button>().transform.GetChild(0).GetComponent<Text>().text = "Accept (" + currCountdownValue + ")";
            yield return new WaitForSeconds(1.0f);
            currCountdownValue--;
        }

        // Do what you need to do once the timer ends
        btnDestroyCard();
    }

#endregion

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
    }
    #region Photon Callbacks

    public void OnEvent(EventData photonEvent)
    {
        switch (photonEvent.Code)
        {
            case PhotonEvents.CHANCE_SELECT:
                // Reshuffle if bottom of deck has been reached else take a card from the top
                if (currentCard < chanceCards.Count - 1)
                {
                    currentCard++;
                }
                else
                {
                    currentCard = 0;

                    if (player.PlayerID == 0)
                    {
                        chanceCards.Shuffle();
                        for (int i = 0; i < chanceCards.Count; i++)
                        {
                            chanceCardNumbers[i] = chanceCards[i].ChanceCardID;
                        }
                        RaiseEventOptions raiseEventOptions1 = new RaiseEventOptions { Receivers = ReceiverGroup.All };
                        SendOptions sendOptions1 = new SendOptions { Reliability = true };
                        PhotonNetwork.RaiseEvent(PhotonEvents.SYNC_CARDS, chanceCardNumbers, raiseEventOptions1, sendOptions1);
                    }
                }
                break;
            case PhotonEvents.SYNC_CARDS:
                IList<ChanceCard> newChanceCards = new List<ChanceCard>();

                for(int i = 0; i < chanceCards.Count; i++)
                {
                    newChanceCards.Add(chanceCards[((int[])photonEvent.CustomData)[i] - 1]);
                }

                chanceCards = newChanceCards;
                break;
        }
    }
    #endregion
}

static class MyExtensions
{
    public static void Shuffle<T>(this IList<T> list)
    {
        int n = list.Count;
        while (n > 0)
        {
            n--;
            int k = Random.Range(1, list.Count);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }
}
