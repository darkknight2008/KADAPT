using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class walls_input : MonoBehaviour
{
    public GameObject wall1;
    public GameObject wall2;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {   
            if (wall1.activeSelf==true)
            {
                  wall1.SetActive(false);
            }
           else
                wall1.SetActive(true);
            if (wall2.activeSelf == true)
            {
                wall2.SetActive(false);
            }
            else
                wall2.SetActive(true);
        }

    }
}
