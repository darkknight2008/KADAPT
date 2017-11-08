using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour
{

    public bool isHold;
    public GameObject holdBy;

    private Vector3 difference;

    // Use this for initialization
    void Start()
    {
        isHold = false;
        holdBy = null;
        difference = new Vector3(0.0f, 0.0f, 0.0f);
    }

    // Update is called once per frame
    void Update()
    {
        if (isHold)
        {
            //transform.position = holdBy.transform.Find("Daniel/UMA_Male_Rig/Global/Position/Hips/LowerBack/Spine/Spine1/RightShoulder/RightArm/RightForeArm/RightHand").transform.rotation * difference + holdBy.transform.Find("Daniel/UMA_Male_Rig/Global/Position/Hips/LowerBack/Spine/Spine1/RightShoulder/RightArm/RightForeArm/RightHand").transform.position;
            //transform.rotation = holdBy.transform.Find("Daniel/UMA_Male_Rig/Global/Position/Hips/LowerBack/Spine/Spine1/RightShoulder/RightArm/RightForeArm/RightHand").transform.rotation;
            transform.position = holdBy.transform.Find("Daniel/UMA_Male_Rig/Global/Position/Hips/LowerBack/Spine/Spine1/RightShoulder/RightArm/RightForeArm/RightHand/SNAP_FormHold").transform.position;
        }
        else if (holdBy)
        {
            //difference = Quaternion.Inverse(holdBy.transform.Find("Daniel/UMA_Male_Rig/Global/Position/Hips/LowerBack/Spine/Spine1/RightShoulder/RightArm/RightForeArm/RightHand").transform.rotation) * (transform.position - holdBy.transform.Find("Daniel/UMA_Male_Rig/Global/Position/Hips/LowerBack/Spine/Spine1/RightShoulder/RightArm/RightForeArm/RightHand").transform.position);
        }
    }
}