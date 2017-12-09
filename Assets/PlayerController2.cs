using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController2 : MonoBehaviour
{
    Animator anim;

    public bool moveable;

    public float speed;
    public float backspeed;
    public float angleSpeed;
    public float jumpHeight;
    public float extraPower;

    private float velocity;
    private float angleVelocity;
    private float distToGround;
    private bool isOnGround;
    private Vector3 deltaP;

    // Use this for initialization
    void Start()
    {
        moveable = true;
        anim = gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (moveable)
        {
            float rotate = Input.GetAxis("Horizontal");
            float move = Input.GetAxis("Vertical");

            Vector3 direction = transform.rotation * new Vector3(0.0f, 0.0f, 1.0f);

            // Move by Position
            velocity = (move >= 0) ? speed : backspeed;
            velocity *= move;
            velocity = (Input.GetKey(KeyCode.Z)) ? velocity * extraPower : velocity;
            transform.position += velocity * Time.deltaTime * direction;
            angleVelocity = (move >= 0) ? -angleSpeed : angleSpeed;
            angleVelocity *= rotate;
            transform.rotation = Quaternion.Euler(new Vector3(0.0f, angleVelocity * Time.deltaTime, 0.0f)) * transform.rotation;

            anim.SetFloat("Speed", velocity);
            //anim.SetFloat("Direction", Mathf.Sign(angleVelocity));
            anim.SetFloat("AngularSpeed", 0.5f * angleVelocity);
            anim.SetFloat("Direction", 0.0f);
        }
    }
}
