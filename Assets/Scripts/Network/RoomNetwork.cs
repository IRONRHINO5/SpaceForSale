using System.Collections;
using System.Collections.Generic;
using System.IO;

using UnityEngine;
using UnityEngine.SceneManagement;

using Photon.Pun;
using Photon.Realtime;

public class RoomNetwork : MonoBehaviourPunCallbacks, IInRoomCallbacks
{
    [HideInInspector]
    public static RoomNetwork Instance;
    private PhotonView photonView;
    private CharacterSelectionCanvas selectionCanvas;

    bool first_sequence  = false,
         second_sequence = false,
         third_sequence  = false,
         fourth_sequence = false,
         fifth_sequence  = false;

    private string GameBoard
    {
        get;
        set;
    }

    // Use this for initialization
    private void Start()
    {
        GameBoard = "Prototype board";
        // Remove duplicate instances of this class
        RoomNetwork[] hoopla = (RoomNetwork[])FindObjectsOfType(typeof(RoomNetwork));
        if (hoopla.Length > 1)
        {
            for (int i = 1; i < hoopla.Length; i++)
            {
                Destroy(hoopla[i].gameObject);
            }
        }

        if (RoomNetwork.Instance == null)
        {
            RoomNetwork.Instance = this;
        }
        else
        {
            if (RoomNetwork.Instance != this)
            {
                //Destroy(RoomNetwork.Instance.gameObject);
                RoomNetwork.Instance = this;
            }
        }
    }

    private void Awake()
    {
        photonView = GetComponent<PhotonView>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            checkSequence(1);
        }
        else if (Input.GetKeyDown(KeyCode.U))
        {
            checkSequence(2);
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            checkSequence(3);
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            if (checkSequence(4))
            {
                GameObject.Find("PlayerNetwork").GetComponent<PlayerNetwork>().PlayerName = string.Empty;
                GameObject.Find("PlayerNetwork").GetComponent<PlayerNetwork>().PieceID = -1;

                GameObject.Find("Canvas").GetComponent<CharacterSelectionCanvas>().playerLeft(GameObject.Find("PlayerNetwork").GetComponent<PlayerNetwork>().PlayerID);

                Destroy(GameObject.Find("DDOL"));
                PhotonNetwork.LeaveRoom();
                PhotonNetwork.Disconnect();
                SceneManager.LoadScene("MainMenu");
            }
        }
    }

    int[] sequence = { 1, 2, 3, 4 };
    int currentButton = 0;

    bool checkSequence(int keyPressed)
    {
        //Key sequence pressed is correct thus far
        if (keyPressed == sequence[currentButton])
        {
            currentButton++;

            //return true when last button is pressed
            if (currentButton == 4)
            {

                //Important! Next call to checkKonami()
                //would result in ArrayIndexOutOfBoundsException otherwise
                currentButton = 0;

                return true;
            }
        }
        else
        {
            //Reset currentButton
            currentButton = 0;
        }

        return false;
    }

    #region Photon Callbacks

    public override void OnEnable()
    {
        base.OnEnable();
        PhotonNetwork.AddCallbackTarget(this);
        SceneManager.sceneLoaded += OnSceneFinishedLoading;
    }

    public override void OnDisable()
    {
        base.OnDisable();
        PhotonNetwork.RemoveCallbackTarget(this);
        SceneManager.sceneLoaded -= OnSceneFinishedLoading;
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("Joined room successfully, new playerID: " + (PhotonNetwork.PlayerList.Length - 1));
        PlayerNetwork.Instance.PlayerID = PhotonNetwork.PlayerList.Length - 1;
        print("Actual player ID: " + PlayerNetwork.Instance.PlayerID);

        //base.OnJoinedRoom();
        /*
        GameObject[] list = SceneManager.GetSceneByName("testselect").GetRootGameObjects();
        foreach (GameObject element in list)
        {
            if (element.name == "Canvas")
                element.GetComponent<CharacterSelectionCanvas>().UpdatePlayerList();
        }
        */
        //GameObject.Find("Canvas").GetComponent<CharacterSelectionCanvas>().UpdatePlayerList();
        // Leave the lobby, in order to rejoin it to refresh the list
        PhotonNetwork.LeaveLobby();
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.Log("Joined Room Failed: " + message);
    }

    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        GameObject.Find("Canvas").GetComponent<CharacterSelectionCanvas>().UpdatePlayerList();
    }

    //public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
    //{
    //    GameObject.Find("Canvas").GetComponent<CharacterSelectionCanvas>().UpdatePlayerList();
    //}

    #endregion

    public static void StartGame()
    {
        Debug.Log("Am I the master client: " + PhotonNetwork.IsMasterClient);
        if (PhotonNetwork.IsMasterClient)
        {
            Debug.Log("Starting game...");
            PhotonNetwork.LoadLevel("Prototype board");
        }
    }

    private void OnSceneFinishedLoading(Scene scene, LoadSceneMode mode)
    {
        print(scene.name);

        if (scene.name == GameBoard)
        {
            CreatePlayer();
        }
    }

    private void CreatePlayer()
    {
        // Temp info
        print("Instantiating " + PlayerNetwork.Instance.PlayerName + " playerID: " + PlayerNetwork.Instance.PlayerID);
        PlayerNetwork.Instance.GamePiece = PhotonNetwork.Instantiate(PlayerNetwork.Instance.PlayerName, GameObject.Find("Sun").transform.GetChild(PlayerNetwork.Instance.PlayerID).transform.position, Quaternion.Euler(0,180,0), 0);

    }
}
