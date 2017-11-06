using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class emit_ball : MonoBehaviour {

    public Rigidbody ball;
    public GameObject wall;
    public GameObject wall2;
    //private Vector3 w;
    // Use this for initialization
	void Start () {
        Vector3 w = new Vector3(-50 * Time.deltaTime, 10 * Time.deltaTime, -50 * Time.deltaTime);
        ball.AddForce(w * 50);
	}
	
	// Update is called once per frame
	void Update () {
        //Vector3 w = new Vector3(-50 * Time.deltaTime, 10 * Time.deltaTime, -50 * Time.deltaTime);
        //ball.AddForce(w * 20);
        if (Input.GetKey(KeyCode.Space)) {
            wall.SetActive(false);
            wall2.SetActive(false);
        }
	}
}
