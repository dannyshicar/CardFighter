// PlayerB (boss)
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerBAnimationAndMovementController : MonoBehaviour
{
    public bool isAI; // if true, the player is controlled by AI
    public GameObject target; // PlayerA
    
    PlayerInput playerInput;
    CharacterController characterController;
    Animator animator;

    // variableds to store optimized setter/getter parameter IDs
    int isWalkingHash;
    int isRunningHash;
    int isJumpingHash;
    int isSecondJumpingHash;

    Vector2 currentMovementInput;
    Vector3 currentMovement;
    Vector3 currentRunMovement;
    bool isMovementPressed;
    bool isRunPressed;
    float rotationFactorPerFrame = 15.0f;
    float walkSpeed = 1.0f;
    float runMultiplier = 1.5f;

    // gravity variables
    float groundedGravity = -.05f;
    float gravity = -9.8f;

    // jumping variables
    bool isJumpPressed = false;
    float initialJumpVelocity;
    float maxJumpHeight = 6.0f;
    float maxJumpTime = 0.75f;
    bool isJumping = false;
    bool isSecondJumping = false;
    bool isJumpAnimating = false;
    bool isSecondJumpAnimating = false;
    bool canSecondJump = false;
    bool hadSecondJump = false;


    [SerializeField] private List<GameObject> skillPrefabs;
    [SerializeField] private List<GameObject> skillPreviewPrefabs;

    // set skill attributes
    List<Skill> skillsBaseValue = new List<Skill>(){
        new Skill("Plasma", 0, 0, 2.0f, 2.0f, 2.0f, 1.0f, 2.0f, 20f),
        new Skill("Small", 1, 0, 1.5f, 1.5f, 1.5f, 1.0f, 2.0f, 10f)
    };


    bool isCast1Pressed = false;
    bool isCast2Pressed = false;
    bool isCast3Pressed = false;
    bool isLeftClick = false;
    bool isCasting = false;
    Vector2 currentCastInput;

    public Vector3 screenPosition;
    public Vector3 mousePos;
    public Vector3 AIMousePos; // for AI controller
    private LineRenderer line;
    public LayerMask mask;
    public int enemyCnt = 5;
    private int hitCnt = 0;
    Vector3 Cast1Position, Cast2Position, Cast3Position;
    List<Vector3> CastPositions = new List<Vector3>(){
        new Vector3(0, 0, 0),
        new Vector3(0, 0, 0),
        new Vector3(0, 0, 0)
    };

    float skill1Delay = 0.5f;
    float skill2Delay = 0.5f;
    float skill3Delay = 0.5f;
    Vector3 previewPosition;
    // public int maxHealth = 100;
    // public int currentHealth;
    private UnitHealth playerHealth = new UnitHealth(100, 100);
    private UnitEnergy playerEnergy = new UnitEnergy(100f, 100f, 10f);
    // public PlayerHealthBar healthBar;
    [SerializeField] HealthBar _healthBar;
    [SerializeField] EnergyBar _energyBar;

    //Awake is called earlier than Start in Unity's event life cycle
    void Awake(){
        isAI = true;
        //initially set reference variables
        playerInput = new PlayerInput();
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        line = GetComponent<LineRenderer>();
        
        isWalkingHash = Animator.StringToHash("isWalking");
        isRunningHash = Animator.StringToHash("isRunning");
        isJumpingHash = Animator.StringToHash("isJumping");
        isSecondJumpingHash = Animator.StringToHash("isSecondJumping");

        playerInput.CharacterControls.PlayerBMove.started += onMovementInput;
        playerInput.CharacterControls.PlayerBMove.canceled += onMovementInput;
        playerInput.CharacterControls.PlayerBMove.performed += onMovementInput;
        playerInput.CharacterControls.PlayerBRun.started += onRun;
        playerInput.CharacterControls.PlayerBRun.canceled += onRun;
        playerInput.CharacterControls.PlayerBJump.started += onJump;
        playerInput.CharacterControls.PlayerBJump.canceled += onJump;
        playerInput.CharacterControls.PlayerBCast1.started += onCast1;
        playerInput.CharacterControls.PlayerBCast1.canceled += onCast1;
        playerInput.CharacterControls.PlayerBCast2.started += onCast2;
        playerInput.CharacterControls.PlayerBCast2.canceled += onCast2;
        playerInput.CharacterControls.PlayerBCast3.started += onCast3;
        playerInput.CharacterControls.PlayerBCast3.canceled += onCast3;
        playerInput.CharacterControls.LeftClick.started += onLeftClick;
        playerInput.CharacterControls.LeftClick.canceled += onLeftClick;

        setupJumpVariables();
        _healthBar.setMaxHealth(playerHealth.Health);
        _energyBar.setMaxEnergy(playerEnergy.Energy);
        InvokeRepeating("regenEnergy", 0f, 1f);
        if(isAI){
            InvokeRepeating("AIControl", 0f, 1f);
        }

    }
    
    void setupJumpVariables(){

        float timeToApex = maxJumpTime / 2;
        gravity = (-2 * maxJumpHeight) / Mathf.Pow(timeToApex, 2);
        initialJumpVelocity = (2 * maxJumpHeight) / timeToApex;
        // initialJumpVelocity = 200f;
    }

    void handleJump(){
        //沒在跳＆在地上＆按下跳躍 -> 跳起來
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
        // 跳完回到地上＆放開跳躍鍵 -> reset 且可再次跳躍
        else if (isJumping && characterController.isGrounded){
            isJumping = false;
            isSecondJumping = false;
            hadSecondJump = false;

        }
        // 沒有按著跳躍件鍵＆在跳的途中＆還沒有跳第二次 -> 進入可以跳第二次的stage
        else if (!isJumpPressed && isJumping && !hadSecondJump){
            hadSecondJump = true;
            canSecondJump = true;
            Debug.Log("can second jump");
        }
        // 在可以跳第二次的stage & 還沒按下第二次跳躍 ＆ 還在跳躍飛行在空中的時候 ＆ 按下跳躍鍵 -> 第二次跳躍！
        else if (canSecondJump && !isSecondJumping && isJumping && isJumpPressed){
            Debug.Log("second Jump!");
            canSecondJump = false;
            isSecondJumping = true;
            animator.SetBool(isSecondJumpingHash, true);
            currentMovement.y = initialJumpVelocity * .5f;
            currentRunMovement.y = initialJumpVelocity * .5f;

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
                animator.SetBool(isSecondJumpingHash, false);
                isJumpAnimating = false;
                canSecondJump = false;
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

    void _handleCastSkillHelper(int skillIdx){
        // check and use energy
        if(!skillsBaseValue[skillIdx].isReady(playerEnergy.Energy)) return;
        useEnergy(skillsBaseValue[skillIdx].energyCost);

        GameObject skillPreviewPrefab = skillPreviewPrefabs[skillIdx];
        CastPositions[skillIdx] = (isAI) ? AIMousePos : mousePos;
        // previewPosition = CastPositions[skillIdx];
        // previewPosition.z = -50f;
        // GameObject obj = Instantiate(skillPreviewPrefab, previewPosition, skillPreviewPrefab.transform.rotation);
        // Destroy(obj, skillsBaseValue[skillIdx].skillDelay);
        // testSkill(skillIdx);
        _handleSkillHelper(skillIdx);
    }
    void _handleSkillHelper(int skillIdx){
        GameObject skillPrefab, obj;
        if(skillIdx == 1){
            // Debug.Log("drop");

            Vector3 skillPos = AIMousePos;
            skillPrefab = skillPrefabs[skillIdx];
            obj = Instantiate(skillPrefab, skillPos, skillPrefab.transform.rotation);
        }
        else if(skillIdx == 0){
            // Debug.Log("throw");

            Vector3 skillPos = transform.position;
            skillPos.y += 1f;
            skillPos.z = 0f;

            skillPrefab = skillPrefabs[skillIdx];
  
            obj = Instantiate(skillPrefab, skillPos, skillPrefab.transform.rotation);
        }
    }

    void handleCastSkill(){
        if(isCast1Pressed){
            isCast1Pressed = false;
            _handleCastSkillHelper(0);
        }
        if(isCast2Pressed){
            isCast2Pressed = false;
            _handleCastSkillHelper(1);
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

                GameObject skillPreviewPrefab = skillPreviewPrefabs[2];
                previewPosition = mousePos;
                previewPosition.z = -50f;
                GameObject obj = Instantiate(skillPreviewPrefab, previewPosition, skillPreviewPrefab.transform.rotation);
                previewPosition = transform.position;
                previewPosition.z = -50f;
                GameObject obj2 = Instantiate(skillPreviewPrefab, previewPosition, skillPreviewPrefab.transform.rotation);
                Destroy(obj, skill3Delay);
                Destroy(obj2, skill3Delay);
                Invoke("handleRayCast", skill3Delay);
                // handleRayCast();
            }
        }
    }
    void handleSkill1(){
        // if(!plasmaSkill.isReady(playerEnergy.Energy)) return;
        // useEnergy(plasmaSkill.energyCost);
        // GameObject skillPrefab;
        // GameObject obj;
        // skillPrefab = skillPrefabs[plasmaSkill.skillPrefabIdx];
        // // Debug.Log(skillPrefab.transform.rotation);
        // obj = Instantiate(skillPrefab, Cast1Position, skillPrefab.transform.rotation);
        // if(plasmaSkill.InSkillRange(Cast1Position, target.transform.position)){
        //     target.GetComponent<PlayerAAnimationAndMovementController>().takeDamage(plasmaSkill.baseDamage);
        // }
        // Destroy(obj, 2);
    }
    void handleSkill2(){
        // if(!smallSkill.isReady(playerEnergy.Energy)) return;
        // useEnergy(smallSkill.energyCost);
        // GameObject skillPrefab;
        // GameObject obj;
        // skillPrefab = skillPrefabs[smallSkill.skillPrefabIdx];
        // // Debug.Log(skillPrefab.transform.rotation);
        // obj = Instantiate(skillPrefab, Cast2Position, skillPrefab.transform.rotation);
        // if(smallSkill.InSkillRange(Cast2Position, target.transform.position)){
        //     target.GetComponent<PlayerAAnimationAndMovementController>().takeDamage(smallSkill.baseDamage);
        // }
        // Destroy(obj, 2);
    }
    void handleRayCast(){
        // Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        // Debug.Log("player :" + Cast3Position);
        // Vector3 skill3StartPoint = transform.position;
        // skill3StartPoint.x += 1;
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

    void AIControl()
    {
        if(Time.timeScale == 0) return; // don't control when game is paused
        // control movement
        float speed = walkSpeed;
        int direction;
        isMovementPressed = true;
        if(Mathf.Abs(target.transform.position.x - transform.position.x) > 3f){
            speed *= runMultiplier;
            isRunPressed = true;
        }
        else{
            isRunPressed = false;
        }
        direction = (target.transform.position.x >= transform.position.x) ? 1 : -1;
        currentMovement.x = direction * speed;
        currentRunMovement.x = direction * speed;

        if(target.transform.position.y - transform.position.y > 1f){
            isJumpPressed = true;
        }
        else{
            isJumpPressed = false;
        }

        // control skill
        // use skill or not
        bool useSkill = Random.Range(0f, 1f) > 0.5f;
        if(useSkill){
            // which skill to use
            int skillIdx = Random.Range(0, skillsBaseValue.Count);
            if(skillsBaseValue[skillIdx].isReady(playerEnergy.Energy)){
                // use skill
                isCast1Pressed = (skillIdx == 0);
                isCast2Pressed = (skillIdx == 1);
                AIMousePos = target.transform.position;
                AIMousePos.y += 1f;
            }
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
            // Debug.Log("move " + currentMovement);
        }
        if (isRunPressed){
            characterController.Move(currentRunMovement * Time.deltaTime);
        }
        else{
            characterController.Move(currentMovement * Time.deltaTime);
        }
        handleGravity();
        handleJump();
        if (Input.GetKeyDown("p")) {
            takeDamage(20);
        }
        //player dead
        if (transform.position.y < -20f || playerHealth.Health <= 0) {
            Debug.Log("over");
            // FindObjectOfType<GameManager>().CompleteLevel();
            GameManager.Instance.CompleteLevel();
        }
    }

    void OnEnable(){
        // enable the character controls action map
        playerInput.CharacterControls.Enable();
    }

    void OnDisable(){
        // disable the character controls action map
        playerInput.CharacterControls.Disable();
    }
    public void takeDamage(int damage) {
        playerHealth.dmgUnit(damage);
        _healthBar.setHealth(playerHealth.Health);
    }

    private void useEnergy(float energyCost)
    {
        playerEnergy.useEnergy(energyCost);
        _energyBar.setEnergy(playerEnergy.Energy);
    }

    private void regenEnergy()
    {
        playerEnergy.regenEnergy();
        _energyBar.setEnergy(playerEnergy.Energy);
    }
}
