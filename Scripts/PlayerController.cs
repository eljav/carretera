
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.SocialPlatforms;

public class PlayerController : MonoBehaviour
{
    private CharacterController controller;
    private Vector3 direction;
    private int currentLane = 1; // 0 left, 1 middle, 2 right
    public float forwardSpeed = 15;
    public float maxSpeed = 100;
    public float laneDistance = 3; // distance between lanes
    public float jumpForce = 10;
    public float downForce = -20;
    public GameObject speedText;

    public Transform groundCheck;
    public LayerMask groundLayer;

    public Animator animator;

    private bool easyMode = false;

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!PlayerManager.isGameStarted)
            return;
        if (forwardSpeed < maxSpeed)
            forwardSpeed += 0.3f * Time.deltaTime;
        speedText.GetComponent<TMPro.TextMeshProUGUI>().text = forwardSpeed.ToString("F1");

        direction.z = forwardSpeed;
        bool isGrounded = Physics.CheckSphere(groundCheck.position, 0.3f, groundLayer);
        if (isGrounded)
        {
            animator.SetBool("isJumping", false);
            //if (Input.GetKeyDown(KeyCode.UpArrow) || SwipeManager.swipeUp)
            //{
            //    Jump();
            //}
        }
        else
        {
            direction.y += downForce * Time.deltaTime;
        }

        // on which lane should player be
        //if (Input.GetKeyDown(KeyCode.RightArrow) || SwipeManager.swipeRight)
        //{
        //    animator.SetTrigger("GoingRight");
        //    currentLane++;
        //    if (currentLane == 3)
        //        currentLane = 2;
        //}
        //if (Input.GetKeyDown(KeyCode.LeftArrow) || SwipeManager.swipeLeft)
        //{
        //    animator.SetTrigger("GoingLeft");
        //    currentLane--;
        //    if (currentLane == -1)
        //        currentLane = 0;
        //}

        // on which lane should be in the future
        Vector3 targetPosition = transform.position.z * transform.forward + transform.position.y * transform.up;
        if (currentLane == 0)
        {
            targetPosition += Vector3.left * laneDistance;
        }
        else if (currentLane == 2)
        {
            targetPosition += Vector3.right * laneDistance;
        }
        // transform.position = Vector3.Lerp(transform.position, targetPosition, 80 * Time.deltaTime);
        // controller.center = controller.center;
        if (transform.position != targetPosition)
        {
            Vector3 diff = targetPosition - transform.position;
            Vector3 moveDir = diff.normalized * 25 * Time.deltaTime;
            if (moveDir.sqrMagnitude < diff.sqrMagnitude)
                controller.Move(moveDir);
            else
                controller.Move(diff);
        }

        controller.Move(direction * Time.deltaTime);
    }

    public void Jump()
    {
        bool isGrounded = Physics.CheckSphere(groundCheck.position, 0.3f, groundLayer);
        if (isGrounded)
        {
            direction.y = jumpForce;
            animator.SetBool("isJumping", true);
            animator.SetTrigger("Jumping");
        }
    }

    public void GoLeft()
    {
        animator.SetTrigger("GoingLeft");
        currentLane--;
        if (currentLane == -1)
            currentLane = 0;
    }

    public void GoRight()
    {
        animator.SetTrigger("GoingRight");
        currentLane++;
        if (currentLane == 3)
            currentLane = 2;
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (easyMode)
            return;
        if (hit.transform.tag == "Obstacle")
        {
            PlayerManager.gameOver = true;
            FindObjectOfType<AudioManager>().StopSound("Stage 1");
        }
    }

    public void toggleEasyMode()
    {
        easyMode = !easyMode;
    }
}
