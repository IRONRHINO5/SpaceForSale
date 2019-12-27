using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class Property : BoardSpace, IPunObservable {

    // We need to add rent tiers
    // The default values are always 9999


    //comment go here
    public int propertyValue,
                   rentTier,
                   meterValue;
    public string propertyFlavorText,
                   propertySet;
    
    protected PhotonView PhotonView;

    public int[] PropertyRent;

    public bool doubleRent;
    public bool DoubleRent
    {
        get
        {
            return doubleRent;
        }
        set
        {
            PhotonView.RPC("updateDoubleRent", RpcTarget.All, value);
        }
    }
    

    public int PropertyValue
    {
        get
        {
            return propertyValue;
        }

        set
        {
            if (value >= 0)
                propertyValue = value;
            else
                propertyValue = 9999;
        }
    }

    public int RentTier
    {
        get
        {
            return rentTier;
        }

        set
        {
            PhotonView.RPC("updateRentTier", RpcTarget.All, value);
        }
    }

    public int MeterValue
    {
        get
        {
            return meterValue;
        }

        set
        {
            if (value >= 0)
                meterValue = value;
            else
                meterValue = 9999;
        }
    }

    public string PropertyFlavorText
    {
        get
        {
            return propertyFlavorText;
        }

        set
        {
            if (value != "")
                propertyFlavorText = value;
            else
                propertyFlavorText = "Flavor text not given";
        }
    }

    public string PropertySet
    {
        get
        {
            return propertySet;
        }

        set
        {
            if (value != "")
                propertySet = value;
            else
                propertyFlavorText = "Property set was not given";
        }
    }

    private bool isOwned;
    public bool IsOwned
    {
        get
        {
            return isOwned;
        }
        set
        {
            print(PhotonView);
            PhotonView.RPC("updateIsOwned", RpcTarget.All, value);
        }
    }

    private string ownedBy;
    public string OwnedBy
    {
        get
        {
            return ownedBy;
        }
        set
        {
            PhotonView.RPC("updateOwnedBy", RpcTarget.All, value);
        }
    }

    private void Start()
    {
        rentTier = 0;
        isOwned = false;
        ownedBy = null;
        PhotonView = gameObject.GetComponent<PhotonView>();
    }

    public override string ToString()
    {
        return "Number: " + SpaceNumber +
               "\nValue: " + PropertyValue +
               "\nRent: " + PropertyRent +
               "\nName: " + BoardSpaceName +
               "\nOwned by: " + OwnedBy;
    }


    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {

    }
    

    public void updateEverything(int tier, bool owned, PlayerNetwork owner)
    {
        RentTier = tier;
        IsOwned = owned;
        OwnedBy = owner.PlayerName;
    }

    [PunRPC]
    public void updateRentTier(int tier)
    {
        rentTier = tier;
    }

    [PunRPC]
    public void updateIsOwned(bool owned)
    {
        isOwned = owned;
    }

    [PunRPC]
    public void updateOwnedBy(string owner)
    {
        ownedBy = owner;
    }

    [PunRPC]
    public void updateDoubleRent(bool doubled)
    {
        doubleRent = doubled;
    }
}
