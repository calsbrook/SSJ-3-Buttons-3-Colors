using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using TMPro;


public class Player : MonoBehaviour
{

    [SerializeField] float runSpeed = 10f;
    [SerializeField] float jumpSpeed = 28f;
    [SerializeField] Vector2 startPosition;
    [SerializeField] TextMeshProUGUI lifeText;

    // State
    [SerializeField] public bool isAlive = true;
    [SerializeField] public int lifeCounterForLevel = 2;
    public int lives;

    // Components
    Rigidbody2D myRigidBody;
    Animator myAnimator;
    Collider2D myCollider2D;

    // Start is called before the first frame update
    void Start()
    {
        myRigidBody = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        myCollider2D = GetComponent<Collider2D>();
        lifeText.text = lives.ToString();
        lives = lifeCounterForLevel;
    }

    // Update is called once per frame
    void Update()
    {
        CheckIfDead();
        Run();
        Jump();
        FlipSprite();
    }

    private void Run()
    {
        float controlThrow = CrossPlatformInputManager.GetAxis("Horizontal"); // value is between -1 and 1
        Vector2 playerVelocity = new Vector2(controlThrow * runSpeed, myRigidBody.velocity.y);
        myRigidBody.velocity = playerVelocity;

        bool playerHasHorizontalSpeed = Mathf.Abs(myRigidBody.velocity.x) > Mathf.Epsilon;
        myAnimator.SetBool("Running", playerHasHorizontalSpeed);
    }

    private void Jump()
    {
        if (!myCollider2D.IsTouchingLayers(LayerMask.GetMask("Ground")) || Mathf.Round(myRigidBody.velocity.y) != 0) { return; }
        if (CrossPlatformInputManager.GetButtonDown("Jump"))
        {
            Vector2 jumpVelocityToAdd = new Vector2(0f, jumpSpeed);
            myRigidBody.velocity += jumpVelocityToAdd;
        }
    }
    private void FlipSprite()
    {
        bool playerHasHorizontalSpeed = Mathf.Abs(myRigidBody.velocity.x) > Mathf.Epsilon;
        if (playerHasHorizontalSpeed)
        {
            transform.localScale = new Vector2(Mathf.Sign(myRigidBody.velocity.x), 1f);
        }
    }

    private void CheckIfDead()
    {
        if (!isAlive)
        {
            lives -= 1;
            myRigidBody.velocity = new Vector2(0f, 0f);
            if (lives >= 0)
            {
                isAlive = true;
                GameObject body = GameObject.Find("Body");
                var NewBody = Instantiate(body, transform.position, body.transform.rotation);
                NewBody.gameObject.tag = "Bodies";
                transform.position = startPosition;
            }
            else
            {
                transform.position = startPosition;
                lives = lifeCounterForLevel;
                isAlive = true;
                GameObject[] bodies = GameObject.FindGameObjectsWithTag("Bodies");
                foreach (GameObject body in bodies)
                {
                    GameObject.Destroy(body);
                }
            }
            lifeText.text = lives.ToString();
        }
        else
        {
            return;
        }
    }
}
