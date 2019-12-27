using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class PlanetInfo : MonoBehaviourPun
{
	private bool visible = false;
	[HideInInspector]
	public int activePlayer;

	private Text       txtPlanetName;
	private Text       txtOwner;
	private Text       txtRate;
	private Text       txtMeter;
	private Text       txtDescription;
	private Button     btnBuy;
	private Button     btnSkip;

    private IEnumerator timer;

    private GameObject current_planet;

    // Init the card
    void Start ()
	{
		txtPlanetName  = (Text)this.gameObject.transform.GetChild(0).gameObject.GetComponent<Text>();
		txtOwner       = (Text)this.gameObject.transform.GetChild(1).gameObject.GetComponent<Text>();
		txtRate        = (Text)this.gameObject.transform.GetChild(2).gameObject.GetComponent<Text>();
		txtMeter       = (Text)this.gameObject.transform.GetChild(3).gameObject.GetComponent<Text>();
		txtDescription = (Text)this.gameObject.transform.GetChild(4).gameObject.GetComponent<Text>();
		btnBuy         = (Button)this.gameObject.transform.GetChild(5).gameObject.GetComponent<Button>();
		btnSkip        = (Button)this.gameObject.transform.GetChild(6).gameObject.GetComponent<Button>();

        timer = StartCountdown(10);

		btnBuy.onClick.AddListener(btnBuyClicked);
		btnSkip.onClick.AddListener(btnSkipClicked);
	}

	// Update the card info
	public void CardUpdate()
	{
		current_planet = GameData.GameDataInstance.boardSpaces[GameData.GameDataInstance.boardSpaceOrder[PlayerNetwork.Instance.BoardPosition]];

		txtPlanetName.text = current_planet.GetComponent<Property>().BoardSpaceName;
		txtRate.text = "Rate: $" + current_planet.GetComponent<Property>().PropertyRent[current_planet.GetComponent<Property>().RentTier];
		txtDescription.text = current_planet.GetComponent<Property>().PropertyFlavorText;
		txtMeter.text = "Meter Value: " + current_planet.GetComponent<Property>().MeterValue.ToString();

		if(current_planet.GetComponent<Property>().IsOwned)
		{
			txtOwner.text = "Owner: " + current_planet.GetComponent<Property>().OwnedBy;
			btnBuy.GetComponentInChildren<Text>().text = "Owned";
			btnBuy.interactable = false;
            btnSkip.transform.GetChild(0).GetComponent<Text>().text = "OK";
		}
		else
		{
			txtOwner.text = "None";
			btnBuy.GetComponentInChildren<Text>().text = "Buy $" + current_planet.GetComponent<Property>().PropertyValue.ToString();
			btnBuy.interactable = true;
            btnSkip.transform.GetChild(0).GetComponent<Text>().text = "Skip";
            if (current_planet.GetComponent<Property>().PropertyValue > GameObject.Find("PlayerNetwork").GetComponent<PlayerNetwork>().BankAccount)
            {
                btnBuy.interactable = false;
            }
		}

		// Set the card color:
		switch(current_planet.GetComponent<Property>().propertySet)
		{
			case "RED":                                                   //  R    G    B    A
				this.GetComponent<Image>().color               = new Color32(255,   0,   0, 255);
				btnBuy.gameObject.GetComponent<Image>().color  = new Color32(  8, 103, 136, 255);
				btnSkip.gameObject.GetComponent<Image>().color = new Color32(  8, 103, 136, 255);
				break;
			case "LIGHT_BLUE":
				this.GetComponent<Image>().color               = new Color32(  0, 228, 255, 255);
				btnBuy.gameObject.GetComponent<Image>().color  = new Color32( 12, 186, 186, 255);
				btnSkip.gameObject.GetComponent<Image>().color = new Color32( 12, 186, 186, 255);
				break;
			case "GREEN":
				this.GetComponent<Image>().color               = new Color32(  0, 255,   0, 255);
				btnBuy.gameObject.GetComponent<Image>().color  = new Color32( 19, 138,  54, 255);
				btnSkip.gameObject.GetComponent<Image>().color = new Color32( 19, 138,  54, 255);
				break;
			case "YELLOW":
				this.GetComponent<Image>().color               = new Color32(255, 255,   0, 255);
				btnBuy.gameObject.GetComponent<Image>().color  = new Color32( 51, 124, 160, 255);
				btnSkip.gameObject.GetComponent<Image>().color = new Color32( 51, 124, 160, 255);
				break;
			case "ORANGE":
				this.GetComponent<Image>().color               = new Color32(255, 112,   0, 255);
				btnBuy.gameObject.GetComponent<Image>().color  = new Color32( 35, 100, 170, 255);
				btnSkip.gameObject.GetComponent<Image>().color = new Color32( 35, 100, 170, 255);
				break;
			case "PINK":
				this.GetComponent<Image>().color               = new Color32(255, 61, 231, 255);
				btnBuy.gameObject.GetComponent<Image>().color  = new Color32(255,   0, 255, 255);
				btnSkip.gameObject.GetComponent<Image>().color = new Color32(255,   0, 255, 255);
				break;
			case "BROWN":
				this.GetComponent<Image>().color               = new Color32( 99,  45,   2, 255);
				btnBuy.gameObject.GetComponent<Image>().color  = new Color32(153,  88,  42, 255);
				btnSkip.gameObject.GetComponent<Image>().color = new Color32(153,  88,  42, 255);
				break;
			case "GRAY":
				this.GetComponent<Image>().color               = new Color32(114, 114, 114, 255);
				btnBuy.gameObject.GetComponent<Image>().color  = new Color32(132, 169, 192, 255);
				btnSkip.gameObject.GetComponent<Image>().color = new Color32(132, 169, 192, 255);
				break;
			case "WHITE":
				this.GetComponent<Image>().color               = new Color32(255, 255, 255, 255);
				btnBuy.gameObject.GetComponent<Image>().color  = new Color32(162, 153, 158, 255);
				btnSkip.gameObject.GetComponent<Image>().color = new Color32(162, 153, 158, 255);
				break;
			case "PURPLE":
				this.GetComponent<Image>().color               = new Color32(215,   0, 255, 255);
				btnBuy.gameObject.GetComponent<Image>().color  = new Color32(  2, 169, 234, 255);
				btnSkip.gameObject.GetComponent<Image>().color = new Color32(  2, 169, 234, 255);
				break;
			case "TEAL":
				this.GetComponent<Image>().color               = new Color32(  0, 128, 128, 255);
				btnBuy.gameObject.GetComponent<Image>().color  = new Color32(  0, 165, 207, 255);
				btnSkip.gameObject.GetComponent<Image>().color = new Color32(  0, 165, 207, 255);
				break;
			case "BLUE":
				this.GetComponent<Image>().color               = new Color32(  0,   0, 255, 255);
				btnBuy.gameObject.GetComponent<Image>().color  = new Color32(245, 184,  65, 255);
				btnSkip.gameObject.GetComponent<Image>().color = new Color32(245, 184,  65, 255);
				break;
			case "GOLD":
				this.GetComponent<Image>().color               = new Color32(236, 192,   0, 255);
				btnBuy.gameObject.GetComponent<Image>().color  = new Color32(  4,  67, 137, 255);
				btnSkip.gameObject.GetComponent<Image>().color = new Color32(  4,  67, 137, 255);
				break;
			default:
				this.GetComponent<Image>().color               = new Color32(  0, 128, 128, 255);
				btnBuy.gameObject.GetComponent<Image>().color  = new Color32(  0, 165, 207, 255);
				btnSkip.gameObject.GetComponent<Image>().color = new Color32(  0, 165, 207, 255);
				break;
		}
	}

	// Show the card
	public void show()
	{
		CardUpdate();
		if(!visible)
		{
			gameObject.GetComponent<Animation>().Play("Panel_Open");
			visible = true;
		}
        StartCoroutine("StartCountdown", 20);
	}

	// Hide the card
	public void hide()
	{
		if(visible)
		{
			gameObject.GetComponent<Animation>().Play("Panel_Close");
			visible = false;
		}
	}

	public void btnBuyClicked()
	{
        // Call buy property
        GameObject.Find("PlayerNetwork").GetComponent<PlayerController>().BuyProperty();

        // 4. Update Card
		CardUpdate();
        StopCoroutine("StartCountdown");
		hide();
    }

	public void btnSkipClicked()
	{
        if (current_planet.GetComponent<Property>().IsOwned && current_planet.GetComponent<Property>().OwnedBy != GameObject.Find("PlayerNetwork").GetComponent<PlayerNetwork>().PlayerName)
        {
            GameObject.Find("PlayerNetwork").GetComponent<PlayerController>().PayRent(current_planet.GetComponent<Property>());
        }

		// End turn
		hide();
        GameObject.Find("GameData").GetComponent<GameData>().ActivePlayer++;
        StopCoroutine("StartCountdown");
    }

	// Useless function
	public override string ToString()
	{
		return "Why are you accessing this function?";
	}

    public float currCountdownValue = 20;
    public IEnumerator StartCountdown(float countdownValue = 10)
    {
        currCountdownValue = countdownValue;
        while (currCountdownValue > 0)
        {
            if (current_planet.GetComponent<Property>().IsOwned)
            {
                btnSkip.transform.GetChild(0).GetComponent<Text>().text = "OK (" + currCountdownValue + ")";
            }
            else
            {
                btnSkip.transform.GetChild(0).GetComponent<Text>().text = "Skip (" + currCountdownValue + ")";
            }
            yield return new WaitForSeconds(1.0f);
            currCountdownValue--;
        }
        // Do what you need to do once the timer ends
        btnSkipClicked();
    }
}
