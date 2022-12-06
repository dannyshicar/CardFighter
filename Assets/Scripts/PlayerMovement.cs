

using UnityEngine;
using System.Collections;

// [RequireComponent(typeof(Rigidbody))]

public class PlayerMovement : MonoBehaviour {

    private CharacterController controller;
    private Transform player;
    private LineRenderer line; // whether the game
    private bool releasingSkill = false; // is paused, waiting for the player to release the skill
    private float verticalVelocity;
    public float gravity = 14.0f;
    public float jumpForce = 10.0f;
    public bool useAI;
    public Vector3 screenPosition;
    public Vector3 mousePos;
    public LayerMask mask;
    public PlayerHealthBar healthBar;
    public int maxHealth = 100;
    public int currentHealth;
    public int enemyCnt = 5;
    private int hitCnt = 0;
    private void Start() {
        useAI = false;
        controller = GetComponent<CharacterController>();
        player = GetComponent<Transform>();
        line = GetComponent<LineRenderer>();
        healthBar.setHealth(maxHealth);
        currentHealth = maxHealth;
    }

    public void takeDamage(int damage) {
        currentHealth -= damage;
        healthBar.setHealth(currentHealth);
    }

    private void Update() {
        
        Vector3 moveVector = Vector3.zero;

        // if (Input.GetKeyDown("p")) {
        //     takeDamage(20);
        // }
        // time flowing
        if(!releasingSkill && !PauseMenu.GameIsPaused){
            // if(Input.GetKeyDown("space")){
            //     releasingSkill = true;
            //     Time.timeScale = 0;
            // }
            //Jump 
            if(controller.isGrounded){
                if(Input.GetAxis("Jump") == 1) {
                    verticalVelocity = jumpForce;
                }
                else{
                    verticalVelocity = -gravity * Time.deltaTime;

                }
            }
            else{
                verticalVelocity -= gravity * Time.deltaTime;
            }
            
            //Move
            moveVector.x += Input.GetAxis("Horizontal") * 5.0f;
            moveVector.y += verticalVelocity;

            controller.Move(moveVector * Time.deltaTime);
        }

        // time stop
        if(releasingSkill && !PauseMenu.GameIsPaused){
            
            //get mouse position
            screenPosition = Input.mousePosition;
            mousePos = Camera.main.ScreenToWorldPoint(screenPosition);
            mousePos.z = 0;
            
            //set the line
            line.SetPositions(new Vector3[]{player.position, mousePos});

            //cast 
            if(Input.GetMouseButtonDown(0)){

                Debug.Log(mousePos);
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(player.position, mousePos - player.position, out hit, Vector3.Distance(player.position, mousePos), mask)){
                    Debug.Log(hit.transform.name);
                    if(hit.transform.GetComponent<Renderer>().material.color != Color.red){
                        hitCnt++;
                        hit.transform.GetComponent<Renderer>().material.color = Color.red;
                    }
                    if(hitCnt == enemyCnt){
                        FindObjectOfType<GameManager>().CompleteLevel();
                    }
                }
                line.SetPositions(new Vector3[]{Vector3.zero, Vector3.zero});

                //change stage back to time flowing
                Time.timeScale = 1;
                releasingSkill = false;
            }
        }
        
        //player dead
        if (player.position.y < -5f || currentHealth <= 0) {
            FindObjectOfType<GameManager>().CompleteLevel();
        }
        

    }

}