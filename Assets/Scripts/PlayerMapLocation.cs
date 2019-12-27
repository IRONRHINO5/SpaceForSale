using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PlayerMapLocation : MonoBehaviour
{
    List<GameObject> coordinates;

    Photon.Realtime.Player[] players;

    [SerializeField]
    private List<GameObject> PieceSprites = new List<GameObject>();

    private List<GameObject> InstantiatedSprites;


    public int[] boardSpaceOrder = new int[51];

    [SerializeField]
    GameObject SunSprite,
               MercurySprite,
               VenusSprite,
               EarthSprite,
               MarsSprite,
               JupiterSprite,
               SaturnSprite,
               UranusSprite,
               NeptuneSprite,
               PlutoSprite,
               ZethurusSprite,
               KanusSprite,
               YoatheaSprite,
               YavisSprite,
               YigawaSprite,
               AgnonidesSprite,
               XilniweiSprite,
               ZeonSprite,
               Pingiri20Sprite,
               PongippeSprite,
               KenvionopeSprite,
               RameshanSprite,
               NutaniaSprite,
               VilmarsSprite,
               LliyiniovSprite,
               SaonysSprite,
               Zyke25Sprite,
               ZanzypsoSprite,
               ZunidesSprite,
               HuntariaSprite,
               Crion6QTSprite,
               PhorixB2Sprite,
               Cyke9ZUSprite,
               VorthYOZASprite,
               XonubosSprite,
               Denvion14MSprite,
               OchonoeSprite,
               NicrypsoSprite,
               XucindaSprite,
               Veshan69Sprite,
               WormholeToSun,
               StarWarsChance1Sprite,
               StarWarsChance2Sprite,
               StarWarsChance3Sprite,
               StarTrekChance1Sprite,
               StarTrekChance2Sprite,
               StarTrekChance3Sprite,
               HodgePodgeChance1Sprite,
               HodgePodgeChance2Sprite;

    // Use this for initialization
    void Start()
    {
        InstantiatedSprites = new List<GameObject>();

        coordinates = new List<GameObject>();

        // List of coordinates for all boardspaces
        SunSprite = GameObject.Find("SunSprite");
        MercurySprite = GameObject.Find("MercurySprite");
        VenusSprite = GameObject.Find("VenusSprite");
        EarthSprite = GameObject.Find("EarthSprite");
        MarsSprite = GameObject.Find("MarsSprite");
        JupiterSprite = GameObject.Find("JupiterSprite");
        SaturnSprite = GameObject.Find("SaturnSprite");
        UranusSprite = GameObject.Find("UranusSprite");
        NeptuneSprite = GameObject.Find("NeptuneSprite");
        PlutoSprite = GameObject.Find("PlutoSprite");

        ZethurusSprite = GameObject.Find("ZethurusSprite");
        KanusSprite = GameObject.Find("KanusSprite");
        YoatheaSprite = GameObject.Find("YoatheaSprite");
        YavisSprite = GameObject.Find("YavisSprite");
        YigawaSprite = GameObject.Find("YigawaSprite");
        AgnonidesSprite = GameObject.Find("AgnonidesSprite");
        XilniweiSprite = GameObject.Find("XilneiweiSprite");
        ZeonSprite = GameObject.Find("ZeonSprite");
        Pingiri20Sprite = GameObject.Find("PingiriSprite");
        PongippeSprite = GameObject.Find("PongippeSprite");

        KenvionopeSprite = GameObject.Find("KenvionopeSprite");
        RameshanSprite = GameObject.Find("RameshanSprite");
        NutaniaSprite = GameObject.Find("NutaniaSprite");
        VilmarsSprite = GameObject.Find("VilmarsSprite");
        LliyiniovSprite = GameObject.Find("LliyinovSprite");
        SaonysSprite = GameObject.Find("SaonysSprite");
        Zyke25Sprite = GameObject.Find("ZykeSprite");
        ZanzypsoSprite = GameObject.Find("ZanzypsoSprite");
        ZunidesSprite = GameObject.Find("ZunidesSprite");
        HuntariaSprite = GameObject.Find("HuntariaSprite");

        Crion6QTSprite = GameObject.Find("CrionSprite");
        PhorixB2Sprite = GameObject.Find("PhorixSprite");
        Cyke9ZUSprite = GameObject.Find("CykeSprite");
        VorthYOZASprite = GameObject.Find("VorthSprite");
        XonubosSprite = GameObject.Find("XonubosSprite");
        Denvion14MSprite = GameObject.Find("DenvionSprite");
        OchonoeSprite = GameObject.Find("OchonoeSprite");
        NicrypsoSprite = GameObject.Find("NicrypsoSprite");
        XucindaSprite = GameObject.Find("XucindaSprite");
        Veshan69Sprite = GameObject.Find("VeshanSprite");
        WormholeToSun = GameObject.Find("PortalSprite");

        StarWarsChance1Sprite = GameObject.Find("StarWarsChance1Sprite");
        StarWarsChance2Sprite = GameObject.Find("StarWarsChance2Sprite");
        StarWarsChance3Sprite = GameObject.Find("StarWarsChance3Sprite");
        StarTrekChance1Sprite = GameObject.Find("StarTrekChance1Sprite");
        StarTrekChance2Sprite = GameObject.Find("StarTrekChance2Sprite");
        StarTrekChance3Sprite = GameObject.Find("StarTrekChance3Sprite");
        HodgePodgeChance1Sprite = GameObject.Find("HodgePodgeChance1Sprite");
        HodgePodgeChance2Sprite = GameObject.Find("HodgePodgeChance2Sprite");

        coordinates.Add(SunSprite);
        coordinates.Add(MercurySprite);
        coordinates.Add(VenusSprite);
        coordinates.Add(EarthSprite);

        coordinates.Add(ZethurusSprite);
        coordinates.Add(KanusSprite);
        coordinates.Add(StarWarsChance1Sprite);
        coordinates.Add(YoatheaSprite);
        coordinates.Add(YavisSprite);
        coordinates.Add(StarWarsChance2Sprite);
        coordinates.Add(YigawaSprite);
        coordinates.Add(AgnonidesSprite);
        coordinates.Add(XilniweiSprite);
        coordinates.Add(ZeonSprite);
        coordinates.Add(StarWarsChance3Sprite);
        coordinates.Add(Pingiri20Sprite);
        coordinates.Add(PongippeSprite);

        coordinates.Add(MarsSprite);
        coordinates.Add(JupiterSprite);
        coordinates.Add(SaturnSprite);

        coordinates.Add(KenvionopeSprite);
        coordinates.Add(StarTrekChance1Sprite);
        coordinates.Add(RameshanSprite);
        coordinates.Add(NutaniaSprite);
        coordinates.Add(VilmarsSprite);
        coordinates.Add(LliyiniovSprite);
        coordinates.Add(SaonysSprite);
        coordinates.Add(StarTrekChance2Sprite);
        coordinates.Add(Zyke25Sprite);
        coordinates.Add(ZanzypsoSprite);
        coordinates.Add(ZunidesSprite);
        coordinates.Add(StarTrekChance3Sprite);
        coordinates.Add(HuntariaSprite);

        coordinates.Add(UranusSprite);
        coordinates.Add(NeptuneSprite);
        coordinates.Add(PlutoSprite);

        coordinates.Add(Crion6QTSprite);
        coordinates.Add(PhorixB2Sprite);
        coordinates.Add(Cyke9ZUSprite);
        coordinates.Add(HodgePodgeChance1Sprite);
        coordinates.Add(VorthYOZASprite);
        coordinates.Add(XonubosSprite);
        coordinates.Add(Denvion14MSprite);
        coordinates.Add(OchonoeSprite);
        coordinates.Add(HodgePodgeChance2Sprite);
        coordinates.Add(NicrypsoSprite);
        coordinates.Add(XucindaSprite);
        coordinates.Add(Veshan69Sprite);
        coordinates.Add(WormholeToSun);


        for (int i = 0, k = 0; i < 51; i++)
        {
            boardSpaceOrder[i] = k; // Count through the index
            k++;

            if (i == 17) // Gate 1...
            {
                boardSpaceOrder[i] = GameData.GATE_1_LIST_VALUE;
                k--;
            }
            else if (i == 34) // Gate 2...
            {
                boardSpaceOrder[i] = GameData.GATE_2_LIST_VALUE;
                k--;
            }
            // You don't have to account for the last gate since it doesn't loop back in on itself
        }

        players = PhotonNetwork.PlayerList;

        print("in pml: " + players.Length);
        foreach (Photon.Realtime.Player player in players)
        {
            InstantiatedSprites.Add(Instantiate(PieceSprites[(int)player.CustomProperties["PieceID"]], GameObject.Find("backImages").transform));
        }

        updatePlayerPosition();
    }



    // Update is called once per frame
    void Update () {
        updatePlayerPosition();

        if (Input.GetKeyDown(KeyCode.M))
        {
            if (GameObject.Find("Map(Clone)"))
            {
                closeMap();
            }
        }
        else if(Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameObject.Find("Map(Clone)"))
            {
                closeMap();
            }
        }
    }

    public void updatePlayerPosition()
    {
        foreach (Photon.Realtime.Player player in players)
        {
            InstantiatedSprites[(int)player.CustomProperties["PlayerID"]].transform.position = coordinates[boardSpaceOrder[(int)player.CustomProperties["BoardPosition"]]].transform.position;
        }
    }

    public void closeMap()
    {
        print("Closing map");
        Destroy(GameObject.Find("Map(Clone)"));
    }

}
