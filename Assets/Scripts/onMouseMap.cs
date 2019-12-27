using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class onMouseMap : MonoBehaviour {

    Property hoveredProperty;
    Gate hoveredGate;
    GameObject PropertyCard;

	// Use this for initialization
	void Start () {
        PropertyCard = GameObject.Find("MapProperty");
    }

    public void onMercuryEnter()
    {
        hoveredProperty = GameObject.Find("Mercury").GetComponent<Property>();
        displayProperty(hoveredProperty);
    }
    public void onVenusEnter()
    {
        hoveredProperty = GameObject.Find("Venus").GetComponent<Property>();
        displayProperty(hoveredProperty);
    }
    public void onZethurusEnter()
    {
        hoveredProperty = GameObject.Find("Zethurus").GetComponent<Property>();
        displayProperty(hoveredProperty);
    }
    public void onKanusEnter()
    {
        hoveredProperty = GameObject.Find("Kanus").GetComponent<Property>();
        displayProperty(hoveredProperty);
    }
    public void onYoatheaEnter()
    {
        hoveredProperty = GameObject.Find("Yoathea").GetComponent<Property>();
        displayProperty(hoveredProperty);
    }
    public void onYavisEnter()
    {
        hoveredProperty = GameObject.Find("Yavis").GetComponent<Property>();
        displayProperty(hoveredProperty);
    }
    public void onYigawaEnter()
    {
        hoveredProperty = GameObject.Find("Yigawa").GetComponent<Property>();
        displayProperty(hoveredProperty);
    }
    public void onAgnonidesEnter()
    {
        hoveredProperty = GameObject.Find("Agnonides").GetComponent<Property>();
        displayProperty(hoveredProperty);
    }
    public void onXilniweiEnter()
    {
        hoveredProperty = GameObject.Find("Xilniwei").GetComponent<Property>();
        displayProperty(hoveredProperty);
    }
    public void onZeonEnter()
    {
        hoveredProperty = GameObject.Find("Zeon").GetComponent<Property>();
        displayProperty(hoveredProperty);
    }
    public void onPingiri20Enter()
    {
        hoveredProperty = GameObject.Find("Pingiri 20").GetComponent<Property>();
        displayProperty(hoveredProperty);
    }
    public void onPongippeEnter()
    {
        hoveredProperty = GameObject.Find("Pongippe 17").GetComponent<Property>();
        displayProperty(hoveredProperty);
    }
    public void onMarsEnter()
    {
        hoveredProperty = GameObject.Find("Mars").GetComponent<Property>();
        displayProperty(hoveredProperty);
    }
    public void onJupiterEnter()
    {
        hoveredProperty = GameObject.Find("Jupiter").GetComponent<Property>();
        displayProperty(hoveredProperty);
    }
    public void onKenvionopeEnter()
    {
        hoveredProperty = GameObject.Find("Kenvionope").GetComponent<Property>();
        displayProperty(hoveredProperty);
    }
    public void onRameshanEnter()
    {
        hoveredProperty = GameObject.Find("Rameshan").GetComponent<Property>();
        displayProperty(hoveredProperty);
    }
    public void onNutaniaEnter()
    {
        hoveredProperty = GameObject.Find("Nutania").GetComponent<Property>();
        displayProperty(hoveredProperty);
    }
    public void onVilmarsEnter()
    {
        hoveredProperty = GameObject.Find("Vilmars").GetComponent<Property>();
        displayProperty(hoveredProperty);
    }
    public void onLliyinovEnter()
    {
        hoveredProperty = GameObject.Find("Lliyinov").GetComponent<Property>();
        displayProperty(hoveredProperty);
    }
    public void onSaonysEnter()
    {
        hoveredProperty = GameObject.Find("Saonys").GetComponent<Property>();
        displayProperty(hoveredProperty);
    }
    public void onZyke25Enter()
    {
        hoveredProperty = GameObject.Find("Zyke 25").GetComponent<Property>();
        displayProperty(hoveredProperty);
    }
    public void onZanzypsoEnter()
    {
        hoveredProperty = GameObject.Find("Zanzypso").GetComponent<Property>();
        displayProperty(hoveredProperty);
    }
    public void onZunidesEnter()
    {
        hoveredProperty = GameObject.Find("Zunides").GetComponent<Property>();
        displayProperty(hoveredProperty);
    }
    public void onHuntariaEnter()
    {
        Debug.Log("Ladies and Gentleman, we got it.");
        hoveredProperty = GameObject.Find("Huntaria").GetComponent<Property>();
        displayProperty(hoveredProperty);
    }
    public void onUranusEnter()
    {
        hoveredProperty = GameObject.Find("Uranus").GetComponent<Property>();
        displayProperty(hoveredProperty);
    }
    public void onNeptuneEnter()
    {
        hoveredProperty = GameObject.Find("Neptune").GetComponent<Property>();
        displayProperty(hoveredProperty);
    }
    public void onPlutoEnter()
    {
        hoveredProperty = GameObject.Find("Pluto").GetComponent<Property>();
        displayProperty(hoveredProperty);
    }
    public void onCrion6qtEnter()
    {
        hoveredProperty = GameObject.Find("Crion 6QT").GetComponent<Property>();
        displayProperty(hoveredProperty);
    }
    public void onPhorixb2Enter()
    {
        hoveredProperty = GameObject.Find("Phorix B2").GetComponent<Property>();
        displayProperty(hoveredProperty);
    }
    public void onCyke9zuEnter()
    {
        hoveredProperty = GameObject.Find("Cyke 9ZU").GetComponent<Property>();
        displayProperty(hoveredProperty);
    }
    public void onVorthYozaEnter()
    {
        hoveredProperty = GameObject.Find("Vorth YOZA").GetComponent<Property>();
        displayProperty(hoveredProperty);
    }
    public void onXonubosEnter()
    {
        hoveredProperty = GameObject.Find("Xonubos").GetComponent<Property>();
        displayProperty(hoveredProperty);
    }
    public void onDenvion14mEnter()
    {
        hoveredProperty = GameObject.Find("Denvion 14M").GetComponent<Property>();
        displayProperty(hoveredProperty);
    }
    public void onOchonoeEnter()
    {
        hoveredProperty = GameObject.Find("Ochonoe").GetComponent<Property>();
        displayProperty(hoveredProperty);
    }
    public void onNicrypsoEnter()
    {
        hoveredProperty = GameObject.Find("Nicrypso").GetComponent<Property>();
        displayProperty(hoveredProperty);
    }
    public void onXucindaEnter()
    {
        hoveredProperty = GameObject.Find("Xucinda").GetComponent<Property>();
        displayProperty(hoveredProperty);
    }
    public void onVeshanEnter()
    {
        hoveredProperty = GameObject.Find("Veshan 69").GetComponent<Property>();
        displayProperty(hoveredProperty);
    }
    public void onEarthEnter()
    {
        hoveredProperty = GameObject.Find("Earth").GetComponent<Property>();
        displayProperty(hoveredProperty);
    }
    public void onSaturnEnter()
    {
        hoveredProperty = GameObject.Find("Saturn").GetComponent<Property>();
        displayProperty(hoveredProperty);
    }

    public void onMercuryExit()
    {
        hoveredProperty = GameObject.Find("Mercury").GetComponent<Property>();
        hideProperty(hoveredProperty);
    }
    public void onVenusExit()
    {
        hoveredProperty = GameObject.Find("Venus").GetComponent<Property>();
        hideProperty(hoveredProperty);
    }
    public void onZethurusExit()
    {
        hoveredProperty = GameObject.Find("Zethurus").GetComponent<Property>();
        hideProperty(hoveredProperty);
    }
    public void onKanusExit()
    {
        hoveredProperty = GameObject.Find("Kanus").GetComponent<Property>();
        hideProperty(hoveredProperty);
    }
    public void onYoatheaExit()
    {
        hoveredProperty = GameObject.Find("Yoathea").GetComponent<Property>();
        hideProperty(hoveredProperty);
    }
    public void onYavisExit()
    {
        hoveredProperty = GameObject.Find("Yavis").GetComponent<Property>();
        hideProperty(hoveredProperty);
    }
    public void onYigawaExit()
    {
        hoveredProperty = GameObject.Find("Yigawa").GetComponent<Property>();
        hideProperty(hoveredProperty);
    }
    public void onAgnonidesExit()
    {
        hoveredProperty = GameObject.Find("Agnonides").GetComponent<Property>();
        hideProperty(hoveredProperty);
    }
    public void onXilniweiExit()
    {
        hoveredProperty = GameObject.Find("Xilniwei").GetComponent<Property>();
        hideProperty(hoveredProperty);
    }
    public void onZeonExit()
    {
        hoveredProperty = GameObject.Find("Zeon").GetComponent<Property>();
        hideProperty(hoveredProperty);
    }
    public void onPingiri20Exit()
    {
        hoveredProperty = GameObject.Find("Pingiri 20").GetComponent<Property>();
        hideProperty(hoveredProperty);
    }
    public void onPongippeExit()
    {
        hoveredProperty = GameObject.Find("Pongippe 17").GetComponent<Property>();
        hideProperty(hoveredProperty);
    }
    public void onMarsExit()
    {
        hoveredProperty = GameObject.Find("Mars").GetComponent<Property>();
        hideProperty(hoveredProperty);
    }
    public void onJupiterExit()
    {
        hoveredProperty = GameObject.Find("Jupiter").GetComponent<Property>();
        hideProperty(hoveredProperty);
    }
    public void onKenvionopeExit()
    {
        hoveredProperty = GameObject.Find("Kenvionope").GetComponent<Property>();
        hideProperty(hoveredProperty);
    }
    public void onRameshanExit()
    {
        hoveredProperty = GameObject.Find("Rameshan").GetComponent<Property>();
        hideProperty(hoveredProperty);
    }
    public void onNutaniaExit()
    {
        hoveredProperty = GameObject.Find("Nutania").GetComponent<Property>();
        hideProperty(hoveredProperty);
    }
    public void onVilmarsExit()
    {
        hoveredProperty = GameObject.Find("Vilmars").GetComponent<Property>();
        hideProperty(hoveredProperty);
    }
    public void onLliyinovExit()
    {
        hoveredProperty = GameObject.Find("Lliyinov").GetComponent<Property>();
        hideProperty(hoveredProperty);
    }
    public void onSaonysExit()
    {
        hoveredProperty = GameObject.Find("Saonys").GetComponent<Property>();
        hideProperty(hoveredProperty);
    }
    public void onZyke25Exit()
    {
        hoveredProperty = GameObject.Find("Zyke 25").GetComponent<Property>();
        hideProperty(hoveredProperty);
    }
    public void onZanzypsoExit()
    {
        hoveredProperty = GameObject.Find("Zanzypso").GetComponent<Property>();
        hideProperty(hoveredProperty);
    }
    public void onZunidesExit()
    {
        hoveredProperty = GameObject.Find("Zunides").GetComponent<Property>();
        hideProperty(hoveredProperty);
    }
    public void onHuntariaExit()
    {
        Debug.Log("Ladies and Gentleman, we got it.");
        hoveredProperty = GameObject.Find("Huntaria").GetComponent<Property>();
        hideProperty(hoveredProperty);
    }
    public void onUranusExit()
    {
        hoveredProperty = GameObject.Find("Uranus").GetComponent<Property>();
        hideProperty(hoveredProperty);
    }
    public void onNeptuneExit()
    {
        hoveredProperty = GameObject.Find("Neptune").GetComponent<Property>();
        hideProperty(hoveredProperty);
    }
    public void onPlutoExit()
    {
        hoveredProperty = GameObject.Find("Pluto").GetComponent<Property>();
        hideProperty(hoveredProperty);
    }
    public void onCrion6qtExit()
    {
        hoveredProperty = GameObject.Find("Crion 6QT").GetComponent<Property>();
        hideProperty(hoveredProperty);
    }
    public void onPhorixb2Exit()
    {
        hoveredProperty = GameObject.Find("Phorix B2").GetComponent<Property>();
        hideProperty(hoveredProperty);
    }
    public void onCyke9zuExit()
    {
        hoveredProperty = GameObject.Find("Cyke 9ZU").GetComponent<Property>();
        hideProperty(hoveredProperty);
    }
    public void onVorthYozaExit()
    {
        hoveredProperty = GameObject.Find("Vorth YOZA").GetComponent<Property>();
        hideProperty(hoveredProperty);
    }
    public void onXonubosExit()
    {
        hoveredProperty = GameObject.Find("Xonubos").GetComponent<Property>();
        hideProperty(hoveredProperty);
    }
    public void onDenvion14mExit()
    {
        hoveredProperty = GameObject.Find("Denvion 14M").GetComponent<Property>();
        hideProperty(hoveredProperty);
    }
    public void onOchonoeExit()
    {
        hoveredProperty = GameObject.Find("Ochonoe").GetComponent<Property>();
        hideProperty(hoveredProperty);
    }
    public void onNicrypsoExit()
    {
        hoveredProperty = GameObject.Find("Nicrypso").GetComponent<Property>();
        hideProperty(hoveredProperty);
    }
    public void onXucindaExit()
    {
        hoveredProperty = GameObject.Find("Xucinda").GetComponent<Property>();
        hideProperty(hoveredProperty);
    }
    public void onVeshanExit()
    {
        hoveredProperty = GameObject.Find("Veshan 69").GetComponent<Property>();
        hideProperty(hoveredProperty);
    }
    public void onEarthExit()
    {
        hoveredProperty = GameObject.Find("Earth").GetComponent<Property>();
        hideProperty(hoveredProperty);
    }
    public void onSaturnExit()
    {
        hoveredProperty = GameObject.Find("Saturn").GetComponent<Property>();
        hideProperty(hoveredProperty);
    }



    public void displayProperty(Property hoveredProperty)
    {
        PropertyCard.GetComponent<Animation>().Play("Panel_Open");
        GameObject.Find("txtPlanetNameMap").GetComponent<Text>().text = hoveredProperty.BoardSpaceName;
        GameObject.Find("txtOwnerMap").GetComponent<Text>().text = "Owner: " + hoveredProperty.OwnedBy;
        GameObject.Find("txtRateMap").GetComponent<Text>().text = "Rate: $" + hoveredProperty.PropertyRent[hoveredProperty.rentTier].ToString();
        GameObject.Find("txtMeterMap").GetComponent<Text>().text = "Meter Value: " + hoveredProperty.meterValue.ToString();
        GameObject.Find("txtDescriptionMap").GetComponent<Text>().text = hoveredProperty.propertyFlavorText;
        GameObject.Find("hoverEffect").GetComponent<AudioSource>().Play(0);
    }
    public void hideProperty(Property hoveredProperty)
    {
        PropertyCard.GetComponent<Animation>().Play("Panel_Close");
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
