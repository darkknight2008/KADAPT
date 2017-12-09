using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorHoldOn : MonoBehaviour
{
    public GameObject door, hero;
    public bool key_get;

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        Animator door_anim = door.GetComponent<Animator>();
        if (key_get == true)
        {
            if (Vector3.Distance(hero.transform.position, door.transform.position) < 10)
            {
                door_anim.SetBool("isopen", true);
            }
            if (Vector3.Distance(hero.transform.position, door.transform.position) > 16)
            {
                door_anim.SetBool("isopen", false);
            }
        }
    }


}
