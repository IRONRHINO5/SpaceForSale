using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
public class ExtraInfoMouseEvents : MonoBehaviour {
    public HUD gameHud;
    GameData GameDataInstance;

    // Use this for initialization
    void Start ()
    {
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    public void OnMouseOver()
    {
        gameHud = GameObject.Find("HUD").GetComponent<HUD>();

        gameObject.transform.Find("ExtraPlayerInfo").gameObject.SetActive(true);
        PanelUpdate(gameObject.transform.GetChild(0).GetComponent<Text>().text);

    }

    public void OnMouseExit()
    {

        gameHud = GameObject.Find("HUD").GetComponent<HUD>();

        gameObject.transform.Find("ExtraPlayerInfo").gameObject.SetActive(false);

    }

    private void PanelUpdate(string name)
    {
        Photon.Realtime.Player[] players = PhotonNetwork.PlayerList;
        GameDataInstance = GameObject.Find("GameData").GetComponent<GameData>();

        gameObject.transform.Find("ExtraPlayerInfo").transform.Find("PlanetOwnedText").GetComponent<Text>().text = string.Empty;

        foreach (Photon.Realtime.Player player in players)
        {
            foreach(GameObject property in GameDataInstance.boardSpaces)
            {
                //print(property);
                if (property.GetComponent<Property>())
                {
                    if (property.GetComponent<Property>().OwnedBy == (string)(player.NickName) && property.GetComponent<Property>().OwnedBy == name)
                    {
                        gameObject.transform.Find("ExtraPlayerInfo").transform.Find("PlanetOwnedText").GetComponent<Text>().text = gameObject.transform.Find("ExtraPlayerInfo").transform.Find("PlanetOwnedText").GetComponent<Text>().text + property.GetComponent<Property>().BoardSpaceName + "\n";
                    }
                }
                else if (property.GetComponent<Gate>())
                {
                    if (property.GetComponent<Gate>().OwnedBy == (string)(player.NickName) && property.GetComponent<Gate>().OwnedBy == name)
                    {
                        gameObject.transform.Find("PlanetOwnedtext").GetComponent<Text>().text = property.GetComponent<Property>().name + "\n";
                    }
                }
            }

        }
    }


}
