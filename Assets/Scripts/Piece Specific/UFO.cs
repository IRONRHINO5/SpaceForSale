using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UFO : MonoBehaviour
{
    private float bottom;
    private float top;
    private float middle;
    private float vertical_shift_speed = 0.25f;
    //private float direction = 1.0f;
    private bool adjustment_add = false;
	// Use this for initialization
	void Start ()
    {
        this.transform.Rotate(5.0f, 0.0f, 0.0f, Space.World);
        middle = this.transform.position.y;
        bottom = middle - 0.125f;
        top = middle + 0.125f;
	}
	
	// Update is called once per frame
	void Update ()
    {
        this.transform.Rotate(0.0f, 200.0f * Time.deltaTime, 0.0f, Space.World);
        if(this.transform.position.y >= top || this.transform.position.y <= bottom)
        {
            vertical_shift_speed = -vertical_shift_speed;
        }

        //if (this.transform.position.y >= middle + 0.05 || this.transform.position.y <= middle - 0.05)
        //    adjustment_add = false;
        //if(adjustment_add)
        //{
        //    vertical_shift_speed += 0.01f;
        //}
        //else
        //{
        //    vertical_shift_speed -= 0.01f;
        //}
        this.transform.Translate(0.0f, vertical_shift_speed * Time.deltaTime, 0.0f, Space.World);
        //this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y + vertical_shift * Time.deltaTime, this.transform.position.z);
    }
}
