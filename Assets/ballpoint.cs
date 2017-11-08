using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ballpoint : MonoBehaviour {
    public Transform bp;
    public GameObject ball;
	// Use this for initialization
	void Start () {
        transform.position = ball.transform.position;
	}
	
	// Update is called once per frame
	void Update () {
        //transform.position = ball.transform.position + new Vector3(1f,0,0);
        transform.position = ball.transform.position;
    }
}
