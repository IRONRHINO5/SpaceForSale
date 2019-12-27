using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassGo : MonoBehaviour {

    // Use this for initialization

    public const int PASS_GO_MONEY = 500;
    
	void Start () {
        var trigger = GameObject.Find("Sun").GetComponent<SphereCollider>();
	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == GameObject.Find("PlayerNetwork").GetComponent<PlayerNetwork>().GamePiece)
        {
            GameObject.Find("PlayerNetwork").GetComponent<PlayerNetwork>().BankAccount += PASS_GO_MONEY;
            RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };
            SendOptions sendOptions = new SendOptions { Reliability = true };
            PhotonNetwork.RaiseEvent(PhotonEvents.MESSAGE, GameObject.Find("PlayerNetwork").GetComponent<PlayerNetwork>().PlayerName + " passed go and collected 500 credits.", raiseEventOptions, sendOptions);
        }
    }
}
