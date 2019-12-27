using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetSpin : MonoBehaviour
{
	public float PlanetSpeed;
  public GameObject clouds;
	private float CloudSpeed;
  private Behaviour halo;
    // Use this for initialization
  void Start ()
	{
		CloudSpeed = PlanetSpeed * 2;
  	//halo = (Behaviour)this.GetComponent("Halo");
  }

	// Update is called once per frame
	void Update ()
	{
        this.transform.Rotate(0.0f, PlanetSpeed * Time.deltaTime, 0.0f, Space.World);
	}
}
/* Planet Names
 1. Zethurus
 2.






 */
