using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Vector3 playerVelocity;

    [Header("Walking")]
    [SerializeField] private float walkSpeed = 10f;

    [Header("Jump & Gravity")]
    [SerializeField] private float jumpHeight = 10f;
    [SerializeField] private float gravity = -10f;

    [Header("Input")]
    [SerializeField] private KeyCode jump = KeyCode.Space;
    [SerializeField] private KeyCode sprint = KeyCode.LeftShift;

    private CharacterController cc;
    private bool grounded = false;

    private CustomInstersectionScript intersection;

    void Start()
    {
        cc = GetComponent<CharacterController>();
        intersection = GetComponent<CustomInstersectionScript>();
    }

  
    // Update is called once per frame
    void Update()
    {
        Movement();
        GravityAndJump();
    }
    private void Movement()
    {
        float targetSpeed = Input.GetKey(sprint) ? walkSpeed * 2f : walkSpeed;

        Vector3 inputVector = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        Vector3 moveDirection = transform.right * inputVector.x + transform.forward * inputVector.z; 

        cc.Move(moveDirection * Time.deltaTime * targetSpeed);
    }

    private void GravityAndJump()
    {
        
        if (grounded && playerVelocity.y < 0f)
        {
            playerVelocity.y = -2f;
        }

        if (grounded && Input.GetKeyDown(jump))
        {
            AddUpwardForce(jumpHeight);
        }

        playerVelocity.y += gravity * Time.deltaTime;
        cc.Move(playerVelocity * Time.deltaTime);

        grounded = cc.isGrounded;
    }

    public void AddUpwardForce(float height)
    {
        playerVelocity.y = 0f;
        playerVelocity.y += Mathf.Sqrt(height * -2f * gravity);
    }


}
