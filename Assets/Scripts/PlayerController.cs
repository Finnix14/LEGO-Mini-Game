using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class PlayerController : MonoBehaviour
{
    private CharacterController characterController;
    private Vector3 direction;
    public float forwardSpeed;
    public float maxSpeed;

    private int desiredLane = 1; // 0 left, 1 middle, 2 right
    public float laneDistance = 4;//the distance between lanes
 
    public float jumpForce;
    public float gravity = -20f;

    public Animator anim;
    public float playerScore;

    public AudioSource jump;
    public AudioSource swipe;
    void Start()
    {
        characterController = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();

    }

    void Update()
    {
        //increasing speed
        direction.z = forwardSpeed;
        if(forwardSpeed < maxSpeed)
            forwardSpeed += 0.15f * Time.deltaTime;


        direction.y += gravity * Time.deltaTime;
        if (characterController.isGrounded)
        {
  
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Jump();
            }
          
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            anim.SetTrigger("Roll");
        }


        if (Input.GetKeyDown(KeyCode.D))
        {
            desiredLane++;
            if (desiredLane == 3)
                desiredLane = 2;
            swipe.Play();
        }

      

        if (Input.GetKeyDown(KeyCode.A))
        {
            desiredLane--;
            if (desiredLane == -1)
                desiredLane = 0;
            swipe.Play();
        }
      

        Vector3 targetPosition = transform.position.z * transform.forward + transform.position.y * transform.up;

        if(desiredLane == 0)
        {
            targetPosition += Vector3.left * laneDistance;

        }else if(desiredLane == 2)
        {
            targetPosition += Vector3.right * laneDistance;
        }

        // transform.position = Vector3.Lerp(transform.position, targetPosition, 80 * Time.fixedDeltaTime);

        if (transform.position == targetPosition)
            return;
        Vector3 diff = targetPosition - transform.position;
        Vector3 moveDir = diff.normalized * 40 * Time.deltaTime;
        if (moveDir.sqrMagnitude < diff.sqrMagnitude)
            characterController.Move(moveDir);
        else
            characterController.Move(diff);
    }

    private void FixedUpdate()
    {
        characterController.Move(direction * Time.fixedDeltaTime);

    }

    private void Jump()
    {
        direction.y = jumpForce;
        anim.SetTrigger("Jump");
        jump.Play();
    }
}
