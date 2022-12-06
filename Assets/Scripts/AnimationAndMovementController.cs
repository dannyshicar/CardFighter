using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class AnimationAndMovementController : MonoBehaviour
{
    PlayerInput playerInput;
    CharacterController characterController;
    Animator animator;

    // variableds to store optimized setter/getter parameter IDs
    int isWalkingHash;
    int isRunningHash;
    int isJumpingHash;

    Vector2 currentMovementInput;
    Vector3 currentMovement;
    Vector3 currentRunMovement;
    bool isMovementPressed;
    bool isRunPressed;
    float rotationFactorPerFrame = 15.0f;
    float walkSpeed = 2.0f;
    float runMultiplier = 3.0f;

    // gravity variables
    float groundedGravity = -.05f;
    float gravity = -9.8f;

    // jumping variables
    bool isJumpPressed = false;
    float initialJumpVelocity;
    float maxJumpHeight = 6.0f;
    float maxJumpTime = 0.75f;
    bool isJumping = false;
    bool isJumpAnimating = false;


    [SerializeField] private List<GameObject> skillPrefabs;
    [SerializeField] private List<GameObject> skillPreviewPrefabs;


    bool isCast1Pressed = false;
    bool isCast2Pressed = false;
    bool isCast3Pressed = false;
    bool isLeftClick = false;
    bool isCasting = false;
    Vector2 currentCastInput;

    public Vector3 screenPosition;
    public Vector3 mousePos;
    private LineRenderer line;
    public LayerMask mask;
    public int enemyCnt = 5;
    private int hitCnt = 0;
    Vector3 Cast1Position, Cast2Position, Cast3Position;
    float skill1Delay = 0.5f;
    float skill2Delay = 0.5f;
    float skill3Delay = 0.5f;
    Vector3 previewPosition;

    //Awake is called earlier than Start in Unity's event life cycle
    void Awake(){

        //initially set reference variables
        playerInput = new PlayerInput();
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        line = GetComponent<LineRenderer>();
        
        isWalkingHash = Animator.StringToHash("isWalking");
        isRunningHash = Animator.StringToHash("isRunning");
        isJumpingHash = Animator.StringToHash("isJumping");

        playerInput.CharacterControls.PlayerAMove.started += onMovementInput;
        playerInput.CharacterControls.PlayerAMove.canceled += onMovementInput;
        playerInput.CharacterControls.PlayerAMove.performed += onMovementInput;
        playerInput.CharacterControls.PlayerARun.started += onRun;
        playerInput.CharacterControls.PlayerARun.canceled += onRun;
        playerInput.CharacterControls.PlayerAJump.started += onJump;
        playerInput.CharacterControls.PlayerAJump.canceled += onJump;
        playerInput.CharacterControls.PlayerACast1.started += onCast1;
        playerInput.CharacterControls.PlayerACast1.canceled += onCast1;
        playerInput.CharacterControls.PlayerACast2.started += onCast2;
        playerInput.CharacterControls.PlayerACast2.canceled += onCast2;
        playerInput.CharacterControls.PlayerACast3.started += onCast3;
        playerInput.CharacterControls.PlayerACast3.canceled += onCast3;
        playerInput.CharacterControls.LeftClick.started += onLeftClick;
        playerInput.CharacterControls.LeftClick.canceled += onLeftClick;

        setupJumpVariables();
    }
    
    void setupJumpVariables(){

        float timeToApex = maxJumpTime / 2;
        gravity = (-2 * maxJumpHeight) / Mathf.Pow(timeToApex, 2);
        initialJumpVelocity = (2 * maxJumpHeight) / timeToApex;
        // initialJumpVelocity = 200f;
    }

    void handleJump(){
        if (!isJumping && characterController.isGrounded && isJumpPressed){
            animator.SetBool(isJumpingHash, true);
            isJumpAnimating = true;
            isJumping = true;
            // float previousYVelocity = currentMovement.y;
            // float newYVelocity = currentMovement.y + initialJumpVelocity;
            // float nextYVelocity = (previousYVelocity + newYVelocity) * .5f;
            currentMovement.y = initialJumpVelocity * .5f;
            currentRunMovement.y = initialJumpVelocity * .5f;
        }
        else if (!isJumpPressed && isJumping && characterController.isGrounded){
            isJumping = false;
        }
    }
    void onCast1(InputAction.CallbackContext context){
        isCast1Pressed = context.ReadValueAsButton();
    }
    void onCast2(InputAction.CallbackContext context){
        isCast2Pressed = context.ReadValueAsButton();
    }
    void onCast3(InputAction.CallbackContext context){
        isCast3Pressed = context.ReadValueAsButton();
    }
    void onLeftClick(InputAction.CallbackContext context){
        isLeftClick = context.ReadValueAsButton();
    }
    void onJump(InputAction.CallbackContext context){
        isJumpPressed = context.ReadValueAsButton();
        // Debug.Log(isJumpPressed);
    }
    void onRun (InputAction.CallbackContext context){
        isRunPressed = context.ReadValueAsButton();
    }

    void handleRotation(){
        Vector3 positionToLookAt;
        // the change in position our character should p[oint to
        positionToLookAt.x = currentMovement.x;
        positionToLookAt.y = 0.0f;
        positionToLookAt.z = currentMovement.z;
        // the current rotation of our character
        Quaternion currentRotation = transform.rotation;

        if (isMovementPressed){
            // creates a new rotation based on where the player is currently pressing
            Quaternion targetRotation = Quaternion.LookRotation(positionToLookAt);
            transform.rotation = Quaternion.Slerp(currentRotation, targetRotation, rotationFactorPerFrame * Time.deltaTime);
        }
    }

    void onMovementInput(InputAction.CallbackContext context){

        currentMovementInput = context.ReadValue<Vector2>();
        currentMovement.x = currentMovementInput.x * walkSpeed;
        // currentMovement.z = currentMovementInput.y;
        currentRunMovement.x = currentMovementInput.x * walkSpeed * runMultiplier;
        // currentRunMovement.z = currentMovementInput.y * runMultiplier;
        isMovementPressed = currentMovementInput.x != 0;
    }
    void handleAnimation(){

        //get parameter values from animator
        bool isWalking = animator.GetBool(isWalkingHash);
        bool isRunning = animator.GetBool(isRunningHash);

        // start walking if movement pressed is true and not already walking
        if(isMovementPressed && !isWalking){
            animator.SetBool(isWalkingHash, true);
        }
        // stop walking if isMovementPressed is false and not already walking
        else if(!isMovementPressed && isWalking){
            animator.SetBool(isWalkingHash, false);
        }
        // run if movement and run pressed are true and not currentoy running
        if((isMovementPressed && isRunPressed) && !isRunning){
            animator.SetBool(isRunningHash, true);
        }
        // stop running if movement or run pressed are false and currently running
        else if((!isMovementPressed || !isRunPressed) && isRunning){
            animator.SetBool(isRunningHash, false);
        }
    }

    void handleGravity(){

        bool isFalling =  currentMovement.y <= 0.0f || !isJumpPressed;
        float fallMultiplier = 2.0f;
        // apply proper gravity depending on if the character is grounded or not
        if (characterController.isGrounded){
            if (isJumpAnimating){
                animator.SetBool(isJumpingHash, false);
                isJumpAnimating = false;
            }
            currentMovement.y = groundedGravity;
            currentMovement.y = groundedGravity;
        }
        else if (isFalling){
            float previousYVelocity = currentMovement.y;
            float newYVelocity = currentMovement.y + (gravity * fallMultiplier * Time.deltaTime);
            float nextYVelocity = Mathf.Max((previousYVelocity + newYVelocity) * .5f, -20.0f);
            currentMovement.y = nextYVelocity;
            currentRunMovement.y = nextYVelocity;
        }
        else {
            float previousYVelocity = currentMovement.y;
            float newYVelocity = currentMovement.y + (gravity * Time.deltaTime);
            float nextYVelocity = (previousYVelocity + newYVelocity) * .5f;
            currentMovement.y = nextYVelocity;
            currentRunMovement.y = nextYVelocity;
        }
    }

    void handleCastSkill(){
        
        GameObject skillPreviewPrefab;
        GameObject obj, obj2;
        if(isCast1Pressed){
            Cast1Position = mousePos;
            isCast1Pressed = false;
            skillPreviewPrefab = skillPreviewPrefabs[0];
            previewPosition = mousePos;
            previewPosition.z = -50f;
            obj = Instantiate(skillPreviewPrefab, previewPosition, skillPreviewPrefab.transform.rotation);
            Destroy(obj, skill1Delay);
            Invoke("handleSkill1", skill1Delay);
        }
        if(isCast2Pressed){
            Cast2Position = mousePos;
            isCast2Pressed = false;
            skillPreviewPrefab = skillPreviewPrefabs[1];
            previewPosition = mousePos;
            previewPosition.z = -50f;
            obj = Instantiate(skillPreviewPrefab, previewPosition, skillPreviewPrefab.transform.rotation);
            Destroy(obj, skill2Delay);
            Invoke("handleSkill2", skill2Delay);
        }
        if(isCast3Pressed){
            isCasting = true;
        }
        if(isCasting){
            //set the line
            if(!PauseMenu.GameIsPaused){
                line.SetPositions(new Vector3[]{transform.position, mousePos});
            }
            if(isLeftClick){
                isCasting = false;
                isLeftClick = false;
                line.SetPositions(new Vector3[]{Vector3.zero, Vector3.zero});
                Cast3Position = mousePos;

                skillPreviewPrefab = skillPreviewPrefabs[2];
                previewPosition = mousePos;
                previewPosition.z = -50f;
                obj = Instantiate(skillPreviewPrefab, previewPosition, skillPreviewPrefab.transform.rotation);
                previewPosition = transform.position;
                previewPosition.z = -50f;
                obj2 = Instantiate(skillPreviewPrefab, previewPosition, skillPreviewPrefab.transform.rotation);
                Destroy(obj, skill3Delay);
                Destroy(obj2, skill3Delay);
                Invoke("handleRayCast", skill3Delay);
                // handleRayCast();
            }
        }
    }
    void handleSkill1(){
        GameObject skillPrefab;
        GameObject obj;
        skillPrefab = skillPrefabs[0];
        Debug.Log(skillPrefab.transform.rotation);
        obj = Instantiate(skillPrefab, Cast1Position, skillPrefab.transform.rotation);
        Destroy(obj, 2);
    }
    void handleSkill2(){
        GameObject skillPrefab;
        GameObject obj;
        skillPrefab = skillPrefabs[1];
        Debug.Log(skillPrefab.transform.rotation);
        obj = Instantiate(skillPrefab, Cast2Position, skillPrefab.transform.rotation);
        Destroy(obj, 2);
    }
    void handleRayCast(){
        // Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        Debug.Log("player :" + Cast3Position);
        if (Physics.Raycast(transform.position, Cast3Position - transform.position, out hit, Vector3.Distance(transform.position, Cast3Position), mask)){
            Debug.Log(hit.transform.name);
            hit.transform.GetComponent<PlayerMovement>().takeDamage(10);
            // if(hit.transform.GetComponent<Renderer>().material.color != Color.red){
            //     hitCnt++;
            //     hit.transform.GetComponent<Renderer>().material.color = Color.red;
            // }
            // if(hitCnt == enemyCnt){
            //     FindObjectOfType<GameManager>().CompleteLevel();
            // }
        }
    }
    // Update is called once per frame
    void Update()
    {
        //get mouse position
        screenPosition = Input.mousePosition;
        mousePos = Camera.main.ScreenToWorldPoint(screenPosition);
        mousePos.z = 0;
        handleCastSkill();
        handleRotation();
        handleAnimation();

        if (isJumping){
            Debug.Log("move " + currentMovement);
        }
        if (isRunPressed){
            characterController.Move(currentRunMovement * Time.deltaTime);
        }
        else{
            characterController.Move(currentMovement * Time.deltaTime);
        }
        handleGravity();
        handleJump();
    }

    void OnEnable(){
        // enable the character controls action map
        playerInput.CharacterControls.Enable();
    }

    void OnDisable(){
        // disable the character controls action map
        playerInput.CharacterControls.Disable();
    }
}
