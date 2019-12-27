using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceStation : MonoBehaviour
{
	[SerializeField]
	private float speed;

	private List<GameObject> StationChildren = new List<GameObject>();
	
	// Use this for initialization
	void Start ()
	{
		StationChildren.Add(this.transform.Find("Spindle.01").gameObject);
		StationChildren.Add(this.transform.Find("Spindle.02").gameObject);
		StationChildren.Add(this.transform.Find("Spindle.03").gameObject);
		StationChildren.Add(this.transform.Find("Spindle.04").gameObject);

		StationChildren.Add(this.transform.Find("Ring.01").gameObject);
		StationChildren.Add(this.transform.Find("Ring.02").gameObject);
		StationChildren.Add(this.transform.Find("Ring.03").gameObject);
		StationChildren.Add(this.transform.Find("Ring.04").gameObject);
	}
	
	// Update is called once per frame
	void Update ()
	{
		this.gameObject.transform.Rotate(new Vector3(0.0f, speed * Time.deltaTime, 0.0f), Space.World);
		foreach(GameObject item in StationChildren)
		{
			item.transform.Rotate(new Vector3(0.0f, 0.0f, (speed * 4.0f) * Time.deltaTime), Space.Self);
		}
	}
}
