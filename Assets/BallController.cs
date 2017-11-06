using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour {

    public bool isHold;
    public GameObject holdBy;

    private Vector3 difference;

	// Use this for initialization
	void Start () {
        isHold = false;
        difference = new Vector3(0.0f, 0.0f, 0.0f);
    }
	
	// Update is called once per frame
	void Update () {
        if (isHold)
        {
            transform.position = difference + holdBy.transform.Find("Daniel/UMA_Male_Rig/Global/Position/Hips/LowerBack/Spine/Spine1/RightShoulder/RightArm/RightForeArm/RightHand").transform.position;
            transform.rotation = holdBy.transform.Find("Daniel/UMA_Male_Rig/Global/Position/Hips/LowerBack/Spine/Spine1/RightShoulder/RightArm/RightForeArm/RightHand").transform.rotation;
        }
        else
        {
            difference = transform.position - holdBy.transform.Find("Daniel/UMA_Male_Rig/Global/Position/Hips/LowerBack/Spine/Spine1/RightShoulder/RightArm/RightForeArm/RightHand").transform.position;
        }
	}
}
