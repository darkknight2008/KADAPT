using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class myCamera : MonoBehaviour {

    public GameObject camera;

    private float y_distance =  0f;
    private Vector3 move_position = Vector3.zero;

	// Use this for initialization
	void Start () {
        camera.transform.localPosition = new Vector3(0, 5f, 0);
    }
	// Update is called once per frame
    void Update()  
    {
        //scaling
        if (Input.GetAxis("Mouse ScrollWheel") != 0){
            y_distance = Input.GetAxis("Mouse ScrollWheel") * 10f;
            camera.transform.localPosition = camera.transform.position + camera.transform.forward * y_distance;
        }

        //movePosition
        if (Input.GetKey(KeyCode.W))
        {
            transform.Translate(new Vector3(0, 0, 5f * Time.deltaTime));
        } 
        if (Input.GetKey(KeyCode.S))
        {
            transform.Translate(new Vector3(0, 0, -1 * 5f * Time.deltaTime));
        }  
        if (Input.GetKey(KeyCode.A))
        {
            transform.Translate(new Vector3(-1 * 5f * Time.deltaTime, 0, 0));
        } 
        if (Input.GetKey(KeyCode.D))
        {
            transform.Translate(new Vector3(5f * Time.deltaTime, 0, 0));
        }

        //rotation
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            transform.Rotate(new Vector3(0, 0, 5f * Time.deltaTime));
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            transform.Rotate(new Vector3(0, 0, -5f * Time.deltaTime));
        }
        if (Input.GetKey(KeyCode.UpArrow))
        {
            transform.Rotate(new Vector3(-5f * Time.deltaTime, 0, 0));
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            transform.Rotate(new Vector3(5f * Time.deltaTime, 0, 0));
        }

	}
}
