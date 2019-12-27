using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class Gate : Property, IPunObservable {

    private bool isOpen;

    public bool IsOpen
    {
        get
        {
            return isOpen;
        }
        set
        {
            PhotonView.RPC("updateIsOpen", RpcTarget.All, value);
        }
    }

    private void Start()
    {
        isOpen = true;
        PhotonView = GetComponent<PhotonView>();
    }

    /*public Gate() : base()
    {
        IsOpen = true;
    }

    public Gate(int number, int value, int rent1, int rent2, int rent3, string name, string flavorText, string propSet, GameObject newPlanet, bool status) : base(number, value, rent1, rent2, rent3, name, flavorText, propSet, newPlanet)
    {
        IsOpen = status;
    }

    public override string ToString()
    {
        return "Name: " + BoardSpaceName +
                "\nStatus" + IsOpen;
    }*/

    [PunRPC]
    private void updateIsOpen(bool open)
    {
        isOpen = open;
    }
}
