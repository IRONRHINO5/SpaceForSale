/********************************************************************************/
/* Class: MenuAVController                                                      */
/*                                                                              */
/* Author: Team Mansa Musa                                                      */
/*                                                                              */
/* Date: 2018-2019                                                              */
/*                                                                              */
/* Description: This class manages the Video background and Music               */
/*     through the menus until the actual game starts.                          */
/********************************************************************************/

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class MenuAVController : MonoBehaviour
{
	void Update ()
    {
		// If there is no camera assigned to the player, assign the main camera
		if(this.GetComponent<VideoPlayer>().targetCamera == null)
        {
            this.GetComponent<VideoPlayer>().targetCamera = Camera.current;
        }

		// When the game switches to the main board scene, destroy this
        if(SceneManager.GetActiveScene().name == "Prototype board")
        {
            Destroy(this.gameObject);
        }
	}
}
