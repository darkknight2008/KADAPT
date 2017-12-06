using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordController : MonoBehaviour
{
    Animator anim;

    public bool isHold;
    public GameObject holdBy;
    public bool kill;

    public float killStart;
    public float killEnd;

    private float count;

    // Use this for initialization
    void Start()
    {
        isHold = true;
        anim = holdBy.GetComponent<Animator>();

        kill = false;
        count = 0.0f;

        //transform.position = holdBy.transform.Find("Daniel/UMA_Male_Rig/Global/Position/Hips/LowerBack/Spine/Spine1/RightShoulder/RightArm/RightForeArm/RightHand/SNAP_WalletHold").transform.position;
        //transform.rotation = holdBy.transform.Find("Daniel/UMA_Male_Rig/Global/Position/Hips/LowerBack/Spine/Spine1/RightShoulder/RightArm/RightForeArm/RightHand/SNAP_WalletHold").transform.rotation;

        transform.position = holdBy.transform.Find("Hero/Hips/Spine/Spine1/Spine2/RightShoulder/RightArm/RightForeArm/RightHand/Sword").transform.position;
        transform.rotation = holdBy.transform.Find("Hero/Hips/Spine/Spine1/Spine2/RightShoulder/RightArm/RightForeArm/RightHand/Sword").transform.rotation;

        transform.parent = holdBy.transform.Find("Hero/Hips/Spine/Spine1/Spine2/RightShoulder/RightArm/RightForeArm/RightHand").transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (isHold)
        {
            anim.SetBool("Sword", true);
            if (Input.GetKey(KeyCode.Space))
            {
                anim.SetBool("Slash", true);

                holdBy.GetComponent<PlayerController2>().moveable = false;
                count = 0.0f;
            }
        }
        else
        {
            anim.SetBool("Sword", false);
        }

        count += Time.deltaTime;
        if (count >= 1.4f)
        {
            holdBy.GetComponent<PlayerController2>().moveable = true;
            anim.SetBool("Slash", false);
        }
        if (count >= killStart && count <= killEnd)
        {
            kill = true;
        }
        else
        {
            kill = false;
        }
    }
}
