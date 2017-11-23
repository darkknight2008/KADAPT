using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorHoldOn : MonoBehaviour {
    Animator anim;
    public bool key_get;
    // Use this for initialization
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

    }
    void OnTriggerEnter(Collider other)
    {
        if (key_get == true)
        {
            anim.SetBool("isopen", true);
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (key_get == true)
        {
            anim.SetBool("isopen", false);
        }
    }
}
