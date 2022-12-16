// PlayerA (player)
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAAnimationAndMovementController : MonoBehaviour
{
    public bool isAI; // if this is true, then the player is controlled by AI
    public GameObject target; // PlayerB
    public Transform apple;

    public Context context;
    
    PlayerInput playerInput;
    CharacterController characterController;
    Animator animator;

    // variableds to store optimized setter/getter parameter IDs
    int isWalkingHash, isRunningHash, isJumpingHash, isSecondJumpingHash;

    Vector2 currentMovementInput;
    Vector3 currentMovement;
    Vector3 currentRunMovement;
    bool isMovementPressed;
    bool isRunPressed;
    float rotationFactorPerFrame = 15.0f;
    float walkSpeed = 2.0f;
    float runMultiplier = 2.0f;

    // gravity variables
    float groundedGravity = -.05f;
    float gravity = -9.8f;

    // jumping variables
    bool isJumpPressed = false;
    float initialJumpVelocity;
    float maxJumpHeight = 5.0f;
    float maxJumpTime = 0.75f;
    bool isJumping = false;
    bool isSecondJumping = false;
    bool isJumpAnimating = false;
    bool isSecondJumpAnimating = false;
    bool canSecondJump = false;
    bool hadSecondJump = false;
    


    [SerializeField] private List<GameObject> skillPrefabs;
    public List<Sprite> skillIcons;
    [SerializeField] SkillBar _skillBar;
    public List<SkillTemplate> skills;

    // set skill attributes
    // List<Skill> skillsBaseValue = new List<Skill>(){
    //     new Skill("Plasma", 0, 40, 2.0f, 2.0f, 2.0f, 1.0f, 2.0f, 20f),
    //     new Skill("Small", 1, 10, 1.5f, 1.5f, 1.5f, 1.0f, 2.0f, 10f),
    //     new Skill("Line", 1, 10, 1.5f, 1.5f, 1.5f, 1.0f, 2.0f, 10f)
    // };
    // skillsBaseValue.Add(new Skill("Plasma", 0, 40, 2.0f, 2.0f, 2.0f, 1.0f, 2.0f, 20f));
    // skillsBaseValue.Add(new Skill("Small", 1, 10, 1.5f, 1.5f, 1.5f, 1.0f, 2.0f, 10f));
    // Skill plasmaSkill = new Skill("Plasma", 0, 40, 2.0f, 2.0f, 2.0f, 1.0f, 2.0f, 20f);
    // Skill smallSkill = new Skill("Small", 1, 10, 1.5f, 1.5f, 1.5f, 1.0f, 2.0f, 10f);

    public List<int> skillPool = new List<int>(){1, 2, 3};


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
    

    Vector3 previewPosition;
    // public int maxHealth = 100;
    // public int currentHealth;
    private UnitHealth playerHealth = new UnitHealth(100, 100);
    private UnitEnergy playerEnergy = new UnitEnergy(0f, 100f, 50f);
    // public PlayerHealthBar healthBar;
    [SerializeField] HealthBar _healthBar;
    [SerializeField] EnergyBar _energyBar;
    public GameObject objectToThrow;
    // public float throwForce;
    // public float throwUpwardForce;
    Vector3 startPos, endPos;

    //Awake is called earlier than Start in Unity's event life cycle
    void Awake(){
        isAI = false;
        context = new Context();
        //initially set reference variables
        playerInput = new PlayerInput();
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        line = GetComponent<LineRenderer>();
        skills = GameObject.Find("PlayerASkill").GetComponent<PlayerASkill>().skills;
        
        isWalkingHash = Animator.StringToHash("isWalking");
        isRunningHash = Animator.StringToHash("isRunning");
        isJumpingHash = Animator.StringToHash("isJumping");
        isSecondJumpingHash = Animator.StringToHash("isSecondJumping");
        // Debug.Log("isWalkingHash " + isWalkingHash);
        // Debug.Log("isRunningHash" + isRunningHash);
        // Debug.Log("isJumpingHash" + isJumpingHash);
        // Debug.Log("isSecondJumpingHash" + isSecondJumpingHash);

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
        _healthBar.setMaxHealth(playerHealth.MaxHealth);
        _healthBar.setHealth(playerHealth.Health);
        _energyBar.setMaxEnergy(playerEnergy.MaxEnergy);
        _energyBar.setEnergy(playerEnergy.Energy);
        _skillBar.SetImage(skillIcons);
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
        // 沒有在跳 ＆ 按下跳躍鍵 & 在地面上 -> 跳躍
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
        // 放開跳躍鍵 ＆ 在跳躍動畫中 ＆ 在地面上 -> 可以再次跳
        else if (!isJumpPressed && isJumping && characterController.isGrounded){
            isJumping = false;
            isSecondJumping = false;
            hadSecondJump = false;
            Debug.Log("onGound");
        }
        // 放開跳躍鍵 ＆ 在跳月中 ＆還沒二段跳 -> 可以二段跳
        else if (!isJumpPressed && isJumping && !hadSecondJump){
            hadSecondJump = true;
            canSecondJump = true;
            Debug.Log("can second jump");
        }
        // 可以二段跳 ＆還沒有二段跳 ＆在跳躍中 ＆按下跳躍鍵
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
        currentMovement.z = -transform.position.z;
        currentRunMovement.x = currentMovementInput.x * walkSpeed * runMultiplier;
        currentRunMovement.z = -transform.position.z;
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
        if(!isMovementPressed && isWalking){
            animator.SetBool(isWalkingHash, false);
        }
        // run if movement and run pressed are true and not currentoy running
        if((isMovementPressed && isRunPressed) && !isRunning){
            animator.SetBool(isRunningHash, true);
        }
        if(!isRunPressed){
            animator.SetBool(isRunningHash, false);
        }
        // stop running if movement or run pressed are false and currently running
        if((!isMovementPressed && !isRunPressed)){
            animator.SetBool(isRunningHash, false);
            animator.SetBool(isWalkingHash, false);
        }
        // Debug.Log(isWalking + " " + isRunning);
        
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

    // void _handleCastSkillHelper(int skillIdx){
    //     // check and use energy
    //     if(!skillsBaseValue[skillIdx].isReady(playerEnergy.Energy)) return;
    //     useEnergy(skillsBaseValue[skillIdx].energyCost);

    //     GameObject skillPreviewPrefab = skillPreviewPrefabs[skillIdx];
    //     CastPositions[skillIdx] = (isAI) ? AIMousePos : mousePos;
    //     // previewPosition = CastPositions[skillIdx];
    //     // previewPosition.z = -50f;
    //     // GameObject obj = Instantiate(skillPreviewPrefab, previewPosition, skillPreviewPrefab.transform.rotation);
    //     // Destroy(obj, skillsBaseValue[skillIdx].skillDelay);
    //     // testSkill(skillIdx);
    //     _handleSkillHelper(skillIdx);
    // }
 
    // void _handleSkillHelper(int skillIdx){
    //     GameObject skillPrefab, obj;
    //     if(skillIdx == 1){
    //         // Debug.Log("drop");

    //         Vector3 skillPos = mousePos;
    //         skillPrefab = skillPrefabs[skillIdx];
    //         obj = Instantiate(skillPrefab, skillPos, skillPrefab.transform.rotation);
    //     }
    //     else if(skillIdx == 0){
    //         // Debug.Log("throw");

    //         Vector3 skillPos = transform.position;
    //         skillPos.y += 1f;
    //         skillPos.z = 0f;

    //         skillPrefab = skillPrefabs[skillIdx];
  
    //         obj = Instantiate(skillPrefab, skillPos, skillPrefab.transform.rotation);
    //     }
    //     else if(skillIdx == 2){

    //         Vector3 skillPos = transform.position;
    //         skillPos.y += 1f;
    //         skillPos.z = 0f;

    //         skillPrefab = skillPrefabs[skillIdx];
    //         obj = Instantiate(skillPrefab, skillPos, skillPrefab.transform.rotation);
    //         // startPos = transform.position;
    //         // startPos.y += 1f;
    //         // endPos = mousePos;
    //         // line.SetPositions(new Vector3[]{startPos, endPos});
    //         // // GameObject skillPreviewPrefab = skillPreviewPrefabs[2];
    //         // // previewPosition = mousePos;
    //         // // previewPosition.z = -50f;
    //         // // obj = Instantiate(skillPreviewPrefab, previewPosition, skillPreviewPrefab.transform.rotation);
    //         // // previewPosition = transform.position;
    //         // // previewPosition.z = -50f;
    //         // // GameObject obj2 = Instantiate(skillPreviewPrefab, previewPosition, skillPreviewPrefab.transform.rotation);
    //         // // Destroy(obj, skill3Delay);
    //         // // Destroy(obj2, skill3Delay);
    //         // Invoke("handleRayCast", skill3Delay);
    //     }
    //     else if(skillIdx == 3){
    //         Vector3 skillPos = transform.position;
    //         skillPos.y += 1f;
    //         skillPos.z = 0f;

    //         skillPrefab = skillPrefabs[skillIdx];
    //         obj = Instantiate(skillPrefab, skillPos, skillPrefab.transform.rotation);

    //     }
    // }

    void handleCastSkill(){
        if(isCast1Pressed){
            isCast1Pressed = false;
            GameObject.Find("PlayerASkill").GetComponent<PlayerASkill>().Cast(skillPool[0], playerEnergy.Energy);
            // _handleCastSkillHelper(0);
        }
        if(isCast2Pressed){
            isCast2Pressed = false;
            GameObject.Find("PlayerASkill").GetComponent<PlayerASkill>().Cast(skillPool[1], playerEnergy.Energy);
            // _handleCastSkillHelper(1);
        }
        if(isCast3Pressed){
            // isCasting = true;
            isCast3Pressed = false;
            GameObject.Find("PlayerASkill").GetComponent<PlayerASkill>().Cast(skillPool[2], playerEnergy.Energy);
            // _handleCastSkillHelper(2);
        }
        // if(isCasting){
        //     //set the line
        //     if(!PauseMenu.GameIsPaused){
        //         line.SetPositions(new Vector3[]{transform.position, mousePos});
        //     }
        //     if(isLeftClick){
        //         isCasting = false;
        //         isLeftClick = false;
        //         line.SetPositions(new Vector3[]{Vector3.zero, Vector3.zero});
        //         Cast3Position = mousePos;

        //         GameObject skillPreviewPrefab = skillPreviewPrefabs[2];
        //         previewPosition = mousePos;
        //         previewPosition.z = -50f;
        //         GameObject obj = Instantiate(skillPreviewPrefab, previewPosition, skillPreviewPrefab.transform.rotation);
        //         previewPosition = transform.position;
        //         previewPosition.z = -50f;
        //         GameObject obj2 = Instantiate(skillPreviewPrefab, previewPosition, skillPreviewPrefab.transform.rotation);
        //         Destroy(obj, skill3Delay);
        //         Destroy(obj2, skill3Delay);
        //         Invoke("handleRayCast", skill3Delay);
        //         // handleRayCast();
        //     }
        // }
    }

    void updateSkillIconCooldown()
    {
        for(int i = 0; i < 3; i++)
        {
            _skillBar.updateFillAmount(i, playerEnergy.Energy, skills[skillPool[i]].energyCost);
        }
    }
    // Update is called once per frame
    void Update()
    {
        updateSkillIconCooldown();
        //get mouse position
        // screenPosition = Input.mousePosition;
        // mousePos = Camera.main.ScreenToWorldPoint(screenPosition);
        // mousePos.z = 0;
        mousePos = context.GetMousePos();
        // Debug.Log("mousePos" + mousePos);
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
        // if (Input.GetKeyDown("p")) {
        //     takeDamage(20);
        // }
        //player dead
        if (transform.position.y < -20f || playerHealth.Health <= 0) {
            // FindObjectOfType<GameManager>().FailLevel();
            GameManager.Instance.FailLevel();
        }
        // Debug.Log(transform.position);
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

    public void heal(int amount){
        playerHealth.healUnit(amount);
        _healthBar.setHealth(playerHealth.Health);
    }
    
    public void useEnergy(float energyCost)
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
