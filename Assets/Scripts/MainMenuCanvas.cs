using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class MainMenuCanvas : MonoBehaviour
{
	private Button btnStart;

	// Use this for initialization
	void Start ()
	{
		btnStart   = GameObject.Find("Start").GetComponent<Button>();
		btnStart.onClick.AddListener(btnStartOnClick);
	}
	
	private void btnStartOnClick()
	{
		PhotonNetwork.LoadLevel("GameLobby");
	}
}
