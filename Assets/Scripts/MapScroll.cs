using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapScroll : MonoBehaviour {

    // Private Variables
    private float scale;
    private float xPos;
    private float yPos;
    private float mapX;
    private float mapY;
    private Transform map;
    private int scrollNumber;
    float centerpointx;
    float centerpointy;

    // Use this for initialization
    void Start () {
        scale = transform.localScale.x;
        scrollNumber = 0;
        centerpointx = transform.position.x;
        centerpointy = transform.position.y;

    }
	
	// Update is called once per frame
	void Update () {
        //Scroll-In Check
        if (Input.GetAxis("Mouse ScrollWheel") > 0F)
        {
            scale = transform.localScale.x;
            
            if (scale <= 1.5F)
            {
                transform.localScale += new Vector3(0.1F, 0.1F, 0);
                scrollNumber++;
                //Debug.Log(scrollNumber);
            }
        }
        // Scroll-Out  Check
        else if (Input.GetAxis("Mouse ScrollWheel") < 0F)
        {
            scale = transform.localScale.x;
            
            if (scale >= 1.1F)
            {
                transform.localScale += new Vector3(-0.1F, -0.1F, 0);
                scrollNumber--;
                //Debug.Log(scrollNumber);
            } 
        }
        // Map Pos Move
        
        
        // Map Pos Check On Zoom. Checks if Pos is further than should be and compensates on zooming out.
        scale = transform.localScale.x;

        // Map Pos Check X
        if (scrollNumber == 0)
        {
            transform.localPosition = new Vector3(0, 0, 0);
        }
        if (scrollNumber >= 1)
        {
            float flyWidth = ((Screen.width * scale) - Screen.width) / 2.0F;
            float flyHeight = ((Screen.height * scale) - Screen.height) / 2.0F;
            Vector3 origpos = transform.position;
            transform.localPosition += new Vector3(Input.GetAxis("Horizontal") * -500.0F * Time.deltaTime, 0, 0);
            transform.localPosition += new Vector3(0, Input.GetAxis("Vertical") * -500.0F * Time.deltaTime, 0);
            //Debug.Log(transform.position.x);

            //old code
            #region
            /*
      
          
                        // Lock Position X
                        if (scrollNumber == 1 && transform.position.x > 414)
                        {
                            transform.position = new Vector3(414, transform.position.y, 0);
                        }
                        else if (scrollNumber == 1 && transform.position.x < 340)
                        {
                            transform.position = new Vector3(340, transform.position.y, 0);
                        }
                        else if (scrollNumber == 2 && transform.position.x > 456)
                        {
                            transform.position = new Vector3(456, transform.position.y, 0);
                        }
                        else if (scrollNumber == 2 && transform.position.x < 303)
                        {
                            transform.position = new Vector3(303, transform.position.y, 0);
                        }
                        else if (scrollNumber == 3 && transform.position.x > 493)
                        {
                            transform.position = new Vector3(493, transform.position.y, 0);
                        }
                        else if (scrollNumber == 3 && transform.position.x < 265)
                        {
                            transform.position = new Vector3(265, transform.position.y, 0);
                        }
                        else if (scrollNumber == 4 && transform.position.x > 529)
                        {
                            transform.position = new Vector3(529, transform.position.y, 0);
                        }
                        else if (scrollNumber == 4 && transform.position.x < 225)
                        {
                            transform.position = new Vector3(225, transform.position.y, 0);
                        }
                        else if (scrollNumber == 5 && transform.position.x > 567)
                        {
                            transform.position = new Vector3(567, transform.position.y, 0);
                        }
                        else if (scrollNumber == 5 && transform.position.x < 187)
                        {
                            transform.position = new Vector3(187, transform.position.y, 0);
                        }

                        // Lock Position Y
                        if (scrollNumber == 1 && transform.position.y < 192)
                        {
                            transform.position = new Vector3(transform.position.x, 192, 0);
                        }
                        else if (scrollNumber == 1 && transform.position.y > 237)
                        {
                            transform.position = new Vector3(transform.position.x, 237, 0);
                        }
                        if (scrollNumber == 2 && transform.position.y < 172)
                        {
                            transform.position = new Vector3(transform.position.x, 172, 0);
                        }
                        else if (scrollNumber == 2 && transform.position.y > 249)
                        {
                            transform.position = new Vector3(transform.position.x, 249, 0);
                        }
                        if (scrollNumber == 3 && transform.position.y < 150)
                        {
                            transform.position = new Vector3(transform.position.x, 150, 0);
                        }
                        else if (scrollNumber == 3 && transform.position.y > 270)
                        {
                            transform.position = new Vector3(transform.position.x, 270, 0);
                        }
                        if (scrollNumber == 4 && transform.position.y < 130)
                        {
                            transform.position = new Vector3(transform.position.x, 130, 0);
                        }
                        else if (scrollNumber == 4 && transform.position.y > 292)
                        {
                            transform.position = new Vector3(transform.position.x, 292, 0);
                        }
                        if (scrollNumber == 5 && transform.position.y < 109)
                        {
                            transform.position = new Vector3(transform.position.x, 109, 0);
                        }
                        else if (scrollNumber == 5 && transform.position.y > 311)
                        {
                            transform.position = new Vector3(transform.position.x, 311, 0);
                        }

                        */
            #endregion
            float prevLeft = (Screen.width * scale) - Screen.width;

            if (Mathf.Abs(centerpointx - transform.position.x) > flyWidth)
            {
                float x = centerpointx - transform.position.x;
                if ( x < 0)
                {
                    transform.position = new Vector3(transform.position.x + (centerpointx - transform.position.x + flyWidth), transform.position.y, 0);
                }
                else
                {
                    transform.position = new Vector3(transform.position.x + (centerpointx - transform.position.x - flyWidth), transform.position.y, 0);
                }
                
            }
            if (Mathf.Abs(centerpointy - transform.position.y) > flyHeight)
            {
                float y = centerpointy - transform.position.y;
                if (y < 0)
                {
                    transform.position = new Vector3(transform.position.x, transform.position.y + (centerpointy - transform.position.y + flyHeight), 0);
                }
                else
                {
                    transform.position = new Vector3(transform.position.x, transform.position.y + (centerpointy - transform.position.y - flyHeight), 0);
                }
            
            }
                

        }

    }
}
