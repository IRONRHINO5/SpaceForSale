// TODO:
//     * Set property card panel color to property color

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;

public class HUD : MonoBehaviourPunCallbacks, IOnEventCallback
{
    // Constants
    private const int MAX_MESSAGE_HISTORY = 15;

    private const bool DEBUG = true;

    public List<GameObject> playerInfoPanels      = new List<GameObject>();
   // public List<GameObject> extraPlayerInfoPanels = new List<GameObject>();

    [SerializeField]
    private GameObject menuCanvas;

    [SerializeField]
    private GameObject playerInfoPrefab;

    [SerializeField]
    private GameObject MessagePrefab;

    [SerializeField]
    private GameObject championScreenPrefab;
    // [SerializeField]
    //private GameObject extraPlayerInfoPrefab;

    GameData GameDataInstance;
    private Button LaunchButton {get; set; }
    private Button MapButton { get; set; }
    private Button ViewButton { get; set; }

    [SerializeField]
    private GameObject Map;

    private Camera MainCamera;
    private Camera TopDownCamera;

    private GameObject PlayerInfoGroup {get; set;}
   // private GameObject ExtraPlayerInfoGroup {get; set;}

    private Photon.Realtime.Player[] players;
    PlayerNetwork player;

    Color originalColor;

#region Unity Callbacks

    ///<summary>Unity Callback: Used to initialize things that are not related to other scene object</summary>
    private void Start()
    {
        //originalColor = playerInfoPanels[0].GetComponent<Image>().color;
        GameDataInstance = GameObject.Find("GameData").GetComponent<GameData>();
        player = GameObject.Find("PlayerNetwork").GetComponent<PlayerNetwork>();
    }

    ///<summary>Unity Callback: Used to initialize things that are based on other scene objects</summary>
    private void Awake()
    {
        LaunchButton = GameObject.Find("btnLaunch").GetComponent<Button>();
        LaunchButton.onClick.AddListener(btnLaunchOnClick);
        if (GameObject.Find("PlayerNetwork").GetComponent<PlayerNetwork>().PlayerID == 0)
        {
            LaunchButton.interactable = true;
            //LaunchButton.GetComponent<Image>().color = Color.green;
        }
        else
        {
            LaunchButton.interactable = false;
            LaunchButton.gameObject.SetActive(false);
        }

        MapButton = GameObject.Find("MapViewButton").GetComponent<Button>();
        MapButton.onClick.AddListener(btnPullUpMap);
        
        PlayerInfoGroup = GameObject.Find("PlayerInfoGroup");
        players = PhotonNetwork.PlayerList;

        // Create and initialize the player info panels
        for(int i=0; i < players.Length; i++)
        {
            // Instantiate the prefab and set its parent to the PlayerInfoGroup object
            GameObject a = (GameObject)Instantiate(playerInfoPrefab, PlayerInfoGroup.transform);
            
            // Add the panel to the list
            playerInfoPanels.Add(a);
            
            // Initialize the player info
            a.transform.Find("txtPlayerName").GetComponent<Text>().text = players[i].NickName;
            a.transform.Find("txtMoney").GetComponent<Text>().text = "$" + (players[i].CustomProperties["BankAccount"]).ToString();
            a.transform.Find("ProgressBar").transform.Find("ProgressBarFill").GetComponent<Image>().fillAmount = 0.0f;
            a.transform.Find("ExtraPlayerInfo").gameObject.SetActive(false);

            if(i == 0)
            {
                originalColor = playerInfoPanels[0].GetComponent<Image>().color;
                a.GetComponent<Image>().color = Color.green;
            }
            else
            {
                a.GetComponent<Image>().color = originalColor;
            }
        }

        ViewButton = GameObject.Find("ViewButton").GetComponent<Button>();
        ViewButton.onClick.AddListener(btnChangeCamera);

        MainCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
        TopDownCamera = GameObject.Find("TopDownCamera").GetComponent<Camera>();
        TopDownCamera.enabled = false;
    }



    ///<summary>Unity Callback: Called every actual frame, not every calculated frame like Update().</summary>
    private void FixedUpdate()
    {
        
    }

    ///<summary>Unity Callback: Called every computed frame, not every actual frame like FixedUpdate().</summary>
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if (!GameObject.Find("Map(Clone)"))
            {
                if (menuCanvas.activeSelf)
                {
                    menuCanvas.SetActive(false);
                }
                else
                {
                    menuCanvas.SetActive(true);
                }
            }
        }
        else if(Input.GetKeyDown(KeyCode.M))
        {
            if(!GameObject.Find("Map(Clone)"))
            {
                btnPullUpMap();
            }
        }
        else if(Input.GetKeyDown(KeyCode.V))
        {
            btnChangeCamera();
        }
        else if(Input.GetKeyDown(KeyCode.H))
        {
            if(menuCanvas.activeSelf)
            {
                menuCanvas.SetActive(false);
            }
            else
            {
                menuCanvas.SetActive(true);
            }
        }
        //else if(Input.GetKeyDown(KeyCode.Space))
        //{
        //    if(LaunchButton.interactable == true)
        //        btnLaunchOnClick();
        //}
        //else if(Input.GetKeyDown(KeyCode.Space))
        //{
        //    player.TotalMeterValue += 2000;
        //}
        //else if(Input.GetKeyDown(KeyCode.Tab))
        //{
        //    GameDataInstance.ChanceController.ChanceSelection();
        //}

        // Temporary Test code
        /*
        if(DEBUG)
        {
            if(Input.GetKeyDown(KeyCode.KeypadPlus))
            {
                playerInfoPanels.Add((GameObject)Instantiate(playerInfoPrefab, PlayerInfoGroup.transform));
            }

            if(Input.GetKeyDown(KeyCode.KeypadMinus))
            {
                Destroy(playerInfoPanels[playerInfoPanels.Count - 1]);
                playerInfoPanels.RemoveAt(playerInfoPanels.Count - 1);
            }
        }*/
    }

#endregion

#region Photon Callbacks

    public void OnEvent(EventData photonEvent)
    {
        switch(photonEvent.Code)
        {
            case PhotonEvents.GAME_CHANGE_TURN:
                if (GameDataInstance.ActivePlayer % players.Length == player.GetComponent<PlayerNetwork>().PlayerID)
                {
                    LaunchButton.gameObject.SetActive(true);
                    LaunchButton.GetComponent<Button>().interactable = true;
                    playerInfoPanels[GameDataInstance.ActivePlayer % players.Length].GetComponent<Image>().color = Color.green; // set my panel to green
                    playerInfoPanels[(GameDataInstance.ActivePlayer - 1) % players.Length].GetComponent<Image>().color = originalColor; // reset previous player's panel
                    
                    print("in gct event");
                    StartCoroutine("LaunchCountdown", 15);
                }
                else
                {
                    LaunchButton.GetComponent<Button>().interactable = false;
                    LaunchButton.gameObject.SetActive(false);
                    playerInfoPanels[(GameDataInstance.ActivePlayer - 1) % players.Length].GetComponent<Image>().color = originalColor; // reset previous player's panel
                    playerInfoPanels[GameDataInstance.ActivePlayer % players.Length].GetComponent<Image>().color = Color.green; // set current player's panel to green
                    GameObject.Find("DiceText").GetComponent<Text>().text = "It is not your turn right now...";
                }
                break;
            case PhotonEvents.MONEY_CHANGED:
                playerInfoPanels[(int)photonEvent.CustomData].transform.GetChild(1).GetComponent<Text>().text = players[(int)photonEvent.CustomData].CustomProperties["BankAccount"].ToString();
                break;
            case PhotonEvents.METER_CHANGED:
                playerInfoPanels[(int)photonEvent.CustomData].transform.GetChild(2).transform.GetChild(0).GetComponent<Image>().fillAmount = (float)((float)((int)players[(int)photonEvent.CustomData].CustomProperties["TotalMeterValue"]) / (float)PlayerNetwork.WIN_CONDITION);
                break;
            case PhotonEvents.PLAYER_HAS_WON:
                // Instantiate the win screen
                GameObject championScreen = Instantiate(championScreenPrefab);
                Photon.Realtime.Player champion = null;

                // Disable all the pieces of the HUD
                for(int i = 0; i < gameObject.transform.childCount; i++)
                {
                    gameObject.transform.GetChild(i).gameObject.SetActive(false);
                }

                // Determine who to display as the winner
                foreach (Photon.Realtime.Player player in players)
                {
                    if((int)player.CustomProperties["TotalMeterValue"] >= PlayerNetwork.WIN_CONDITION)
                    {
                        champion = player;
                    }
                }
                championScreen.transform.GetChild(1).GetComponent<Text>().text = "The sun never sets on\n" + champion.NickName + "'s empire!";

                StartCoroutine(StartCountdown(20));

                // Setup the button
                championScreen.transform.GetChild(0).GetComponent<Button>().onClick.AddListener(btnGoBackToMainMenu);
                break;
            case PhotonEvents.MESSAGE:
                //AddMessage((string)photonEvent.CustomData);
                string message = (string)photonEvent.CustomData;
                GameObject.Find("Messages").transform.GetChild(0).GetComponent<ScrollRect>().verticalNormalizedPosition = 0.0f;
                GameObject newMessage = Instantiate(MessagePrefab, GameObject.Find("MessageScroller").transform);
                newMessage.GetComponentInChildren<Text>().text = message;

                Canvas.ForceUpdateCanvases();
                GameObject.Find("Messages Scroll View").GetComponent<ScrollRect>().verticalNormalizedPosition = 0.0f;
                break;
        }
    }
#endregion

#region Button OnClick Events
    
    private void btnLaunchOnClick()
    {
        // Roll Dice
        // In player controller
        if (GameDataInstance.ActivePlayer % players.Length == player.GetComponent<PlayerNetwork>().PlayerID)
        {
            print("launching off");
            StopCoroutine("LaunchCountdown");
            PlayerNetwork.Instance.PlayerController.RollDice();
            Destroy(GameObject.Find("Map(Clone)"));
            LaunchButton.GetComponent<Button>().interactable = false;
            LaunchButton.gameObject.SetActive(false);
        }
    }

    private void btnGoBackToMainMenu()
    {
        print("going home");

        StopCoroutine(StartCountdown());

        player.PlayerName = string.Empty;
        player.PieceID = -1;
        player.GamePiece = null;

        PhotonNetwork.LeaveRoom();
        PhotonNetwork.LeaveLobby();
        PhotonNetwork.Disconnect();
        PhotonNetwork.LoadLevel("MainMenu");

        Destroy(GameObject.Find("DDOL"));
    }

    public void btnPullUpMap()
    {
        Instantiate(Map, GameObject.Find("HUD").transform).transform.SetSiblingIndex(4);
    }

    public void btnChangeCamera()
    {
        if (MainCamera.enabled)
        {
            MainCamera.enabled = false;
            TopDownCamera.enabled = true;
        }
        else
        {
            MainCamera.enabled = true;
            TopDownCamera.enabled = false;
        }
    }

#endregion

    //public void AddMessage(string message)
    //{
    //    GameObject MessegesObj     = GameObject.Find("Messages");
    //    GameObject MessageScroller = GameObject.Find("MessageScroller");

    //    MessegesObj.transform.GetChild(0).GetComponent<ScrollRect>().verticalNormalizedPosition = 0.0f;
    //    GameObject newMessage = Instantiate(MessagePrefab, MessageScroller.transform);
    //    newMessage.GetComponentInChildren<Text>().text = message;

    //    // Keep the messages limited
    //    while(MessageScroller.transform.childCount > MAX_MESSAGE_HISTORY)
    //    {
    //        Destroy(MessageScroller.transform.GetChild(0).gameObject);
    //    }

    //    Canvas.ForceUpdateCanvases();
    //    MessegesObj.transform.GetChild(0).GetComponent<ScrollRect>().verticalNormalizedPosition = 0.0f;
    //}
    
    public float currCountdownValue = 20;
    public IEnumerator StartCountdown(float countdownValue = 10)
    {
        currCountdownValue = countdownValue;
        while (currCountdownValue > 0)
        {
            Debug.Log("Countdown: " + currCountdownValue);
            yield return new WaitForSeconds(1.0f);
            currCountdownValue--;
        }
        // Do what you need to do once the timer ends
        btnGoBackToMainMenu();
    }

    public IEnumerator LaunchCountdown(float countdownValue = 10)
    {
        Debug.Log(" banana");
        currCountdownValue = countdownValue;
        while (currCountdownValue > 0)
        {
            GameObject.Find("DiceText").GetComponent<Text>().text = "Launch in " + currCountdownValue + " ...";
            yield return new WaitForSeconds(1.0f);
            currCountdownValue--;
        }
        // Do what you need to do once the timer ends
        btnLaunchOnClick();
    }
}
