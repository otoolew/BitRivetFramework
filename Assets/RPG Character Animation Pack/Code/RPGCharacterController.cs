using UnityEngine;
using System.Collections;

namespace RPGCharacterAnims{
	
	public enum Weapon{
		UNARMED = 0,
		TWOHANDSWORD = 1,
		TWOHANDSPEAR = 2,
		TWOHANDAXE = 3,
		TWOHANDBOW = 4,
		TWOHANDCROSSBOW = 5,
		STAFF = 6,
		ARMED = 7,
		RELAX = 8,
		RIFLE = 9,
		TWOHANDCLUB = 10,
		SHIELD = 11,
		ARMEDSHIELD = 12
	}
	
	public enum RPGCharacterState{
		DEFAULT,
		BLOCKING,
		STRAFING,
		CLIMBING,
		SWIMMING
	}

	[RequireComponent(typeof(Rigidbody))]
	[RequireComponent(typeof(UnityEngine.AI.NavMeshAgent))]
	public class RPGCharacterController : MonoBehaviour{
		
		#region Variables
		
		//Components.
		[HideInInspector]
		public UnityEngine.AI.NavMeshAgent navMeshAgent;
		[HideInInspector]
		public Rigidbody rb;
		public Animator animator;
		public GameObject target;
		[HideInInspector]
		public Vector3 targetDashDirection;
		CapsuleCollider capCollider;
		ParticleSystem FXSplash;
		public Camera sceneCamera;
		public RPGCharacterState rpgCharacterState = RPGCharacterState.DEFAULT;
		PerfectLookAt headLookController;

		//Movement.
		[HideInInspector]
		public bool crouch;
		[HideInInspector]
		public bool useMeshNav;
		[HideInInspector]
		public bool isMoving = false;
		[HideInInspector]
		public bool canMove = true;
		public float walkSpeed = 1.35f;
		float moveSpeed;
		public float runSpeed = 6f;
		public float sprintSpeed = 14f;
		public float slopeAmount = 0.5f;
		float rotationSpeed = 40f;
		Vector3 inputVec;
		Vector3 newVelocity;
		public bool onAllowableSlope;
		public bool isSprinting;
		
		//Jumping.
		public float gravity = -9.8f;
		[HideInInspector]
		public float gravityTemp = 0f;
		[HideInInspector]
		public bool canJump;
		bool isJumping = false;
		[HideInInspector]
		public bool isGrounded;
		public float jumpSpeed = 12;
		public float doublejumpSpeed = 12;
		bool doJump = false;
		bool doublejumping = true;
		[HideInInspector]
		public bool canDoubleJump = false;
		[HideInInspector]
		public bool isDoubleJumping = false;
		bool doublejumped = false;
		bool isFalling;
		bool startFall;
		float fallingVelocity = -1f;
		float fallTimer = 0f;
		public float fallDelay = 0.2f;
		float distanceToGround;
		
		//Air control.
		public float inAirSpeed = 8f;
		float maxVelocity = 2f;
		float minVelocity = -2f;
		
		//Rolling.
		public float rollSpeed = 8;
		bool isRolling = false;
		public float rollduration;

		//Weapon and Shield.
		public Weapon weapon = Weapon.RELAX;
		[HideInInspector]
		bool isSwitchingFinished = true;
		
		//Strafing/action.
		public bool hipShooting = false;
		[HideInInspector]
		public bool canAction = true;
		bool isStrafing = false;
		[HideInInspector]
		public bool isDead = false;
		[HideInInspector]
		public bool isBlocking = false;
		public float knockbackMultiplier = 1f;
		bool isKnockback;
		[HideInInspector]
		public bool isSitting = false;
		bool isAiming = false;
		[HideInInspector]
		public bool
		isClimbing = false;
		[HideInInspector]
		public bool
		isNearLadder = false;
		[HideInInspector]
		public bool isNearCliff = false;
		[HideInInspector]
		public GameObject ladder;
		[HideInInspector]
		public GameObject cliff;
		[HideInInspector]
		public bool isCasting;
		public int specialAttack = 0;
		public float aimHorizontal;
		public float aimVertical;
		public float bowPull;
		bool injured;
		public bool headLook = false;
		bool isHeadlook = false;
		public int numberOfConversationClips;
		int currentConversation;
		float idleTimer;
		float idleTrigger = 0f;
		
		//Swimming.
		public float inWaterSpeed = 8f;
		
		//Weapon Parameters.
		[HideInInspector]
		public int rightWeapon = 0;
		[HideInInspector]
		public int leftWeapon = 0;
		bool weaponSwitch;
		public bool instantWeaponSwitch;
		public bool dualSwitch;
		
		//Weapon Models.
		public GameObject twoHandAxe;
		public GameObject twoHandSword;
		public GameObject twoHandSpear;
		public GameObject twoHandBow;
		public GameObject twoHandCrossbow;
		public GameObject twoHandClub;
		public GameObject staff;
		public GameObject swordL;
		public GameObject swordR;
		public GameObject maceL;
		public GameObject maceR;
		public GameObject daggerL;
		public GameObject daggerR;
		public GameObject itemL;
		public GameObject itemR;
		public GameObject shield;
		public GameObject pistolL;
		public GameObject pistolR;
		public GameObject rifle;
		public GameObject spear;
		
		//Inputs.
		bool inputJump;
		bool inputLightHit;
		bool inputDeath;
		bool inputUnarmed;
		bool inputShield;
		bool inputAttackL;
		bool inputAttackR;
		bool inputCastL;
		bool inputCastR;
		float inputSwitchUpDown;
		float inputSwitchLeftRight;
		bool inputStrafe;
		float inputTargetBlock = 0;
		float inputDashVertical = 0;
		float inputDashHorizontal = 0;
		float inputHorizontal = 0;
		float inputVertical = 0;
		bool inputAiming;
		public float animationSpeed = 1;
		
		#endregion
		
		#region Initialization
		
		void Awake(){
			navMeshAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();
			rb = GetComponent<Rigidbody>();
			//Find the Animator component.
			animator = GetComponentInChildren<Animator>();
			if(animator == null){
				Debug.LogError("ERROR: There is no animator for character.");
				Destroy(this);
			}
			//Use MainCamera if no camera is selected.
			if(sceneCamera == null){
				sceneCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
				if(sceneCamera == null){
					Debug.LogError("ERROR: There is no camera in scene.");
					Destroy(this);
				}
			}
			//Find HeadLookController if applied.
			headLookController = GetComponent<PerfectLookAt>();
			capCollider = GetComponent<CapsuleCollider>();
			FXSplash = transform.GetChild(2).GetComponent<ParticleSystem>();
			StartCoroutine(_HideAllWeapons(false));
			//Set for starting Relax state.
			weapon = Weapon.RELAX;
			animator.SetInteger("Weapon", -1);
			animator.SetInteger("WeaponSwitch", -1);
			StartCoroutine(_ResetIdleTimer());
			isHeadlook = headLook;
		}
		
		/// <summary>
		/// Input abstraction for easier asset updates using outside control schemes.
		/// </summary>
		void Inputs(){
			inputJump = Input.GetButtonDown("Jump");
			inputLightHit = Input.GetButtonDown("LightHit");
			inputDeath = Input.GetButtonDown("Death");
			inputUnarmed = Input.GetButtonDown("Unarmed");
			inputShield = Input.GetButtonDown("Shield");
			inputAttackL = Input.GetButtonDown("AttackL");
			inputAttackR = Input.GetButtonDown("AttackR");
			inputCastL = Input.GetButtonDown("CastL");
			inputCastR = Input.GetButtonDown("CastR");
			inputSwitchUpDown = Input.GetAxisRaw("SwitchUpDown");
			inputSwitchLeftRight = Input.GetAxisRaw("SwitchLeftRight");
			inputStrafe = Input.GetKey(KeyCode.LeftShift);
			inputTargetBlock = Input.GetAxisRaw("TargetBlock");
			inputDashVertical = Input.GetAxisRaw("DashVertical");
			inputDashHorizontal = Input.GetAxisRaw("DashHorizontal");
			inputHorizontal = Input.GetAxisRaw("Horizontal");
			inputVertical = Input.GetAxisRaw("Vertical");
			inputAiming = Input.GetButtonDown("Aiming");
		}

		#endregion
		
		#region Updates

		void Update(){
			Inputs();
			UpdateAnimationSpeed();
			DirectionalAiming();
			RandomIdle();
			if(canMove && !isBlocking && !isDead && !useMeshNav){
				CameraRelativeInput();
			}
			else{
				inputVec = new Vector3(0, 0, 0);
			}
			if(inputJump){
				doJump = true;
			}
			else{
				doJump = false;
			}
			if(rpgCharacterState != RPGCharacterState.SWIMMING){
				Rolling();
				Jumping();
				Blocking();
			}
			if(inputLightHit && canAction && isGrounded && !isBlocking){
				GetHit();
			}
			if(inputDeath && canAction && isGrounded && !isBlocking){
				if(!isDead){
					StartCoroutine(_Death());
				}
				else{
					StartCoroutine(_Revive());
				}
			}
			if(inputUnarmed && canAction && isGrounded && !isBlocking && weapon != Weapon.UNARMED){
				StartCoroutine(_SwitchWeapon(0));
			}
			if(inputShield && canAction && isGrounded && !isBlocking && leftWeapon != 7){
				StartCoroutine(_SwitchWeapon(7));
			}
			if(inputAttackL && canAction && isGrounded && !isBlocking){
				Attack(1);
			}
			if(inputAttackL || inputAttackR && canAction && isGrounded && isBlocking){
				GetHit();
			}
			if(inputAttackR && canAction && isGrounded && !isBlocking){
				Attack(2);
			}
			if(inputCastL && canAction && isGrounded && !isBlocking){
				AttackKick(1);
			}
			if(inputCastL && canAction && isGrounded && isBlocking){
				StartCoroutine(_BlockBreak());
			}
			if(inputCastR && canAction && isGrounded && !isBlocking){
				AttackKick(2);
			}
			if(inputCastR && canAction && isGrounded && isBlocking){
				StartCoroutine(_BlockBreak());
			}
			if(inputSwitchUpDown < -0.1f && canAction && !isBlocking && isGrounded && isSwitchingFinished){  
				SwitchWeaponTwoHand(0);
			}
			else if(inputSwitchUpDown > 0.1f && canAction && !isBlocking && isGrounded && isSwitchingFinished){
				SwitchWeaponTwoHand(1);
			}
			if(inputSwitchLeftRight < -0.1f && canAction && !isBlocking && isGrounded && isSwitchingFinished){  
				SwitchWeaponLeftRight(0);
			}
			else if(inputSwitchLeftRight > 0.1f && canAction && !isBlocking && isGrounded && isSwitchingFinished){  
				SwitchWeaponLeftRight(1);
			}
			if(inputSwitchLeftRight == 0 && inputSwitchUpDown == 0){
				isSwitchingFinished = true;
			}
			//Strafing
			if(inputStrafe || inputTargetBlock > 0.1f && canAction && weapon != Weapon.RIFLE){
				if(weapon != Weapon.RELAX){
					animator.SetBool("Strafing", true);
				}
				isStrafing = true;
				if(inputCastL && canAction && isGrounded && !isBlocking){
					Cast(1, "attack");
				}
				if(inputCastR && canAction && isGrounded && !isBlocking){
					Cast(2, "attack");
				}
			}
			else{  
				isStrafing = false;
				animator.SetBool("Strafing", false);
			}
			//Aiming.
			if(Input.GetMouseButtonDown(0)){
				if(useMeshNav){
					RaycastHit hit;
					if(Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 100)){
						navMeshAgent.destination = hit.point;
					}
				}
				else if((weapon == Weapon.TWOHANDBOW || weapon ==  Weapon.TWOHANDCROSSBOW || weapon ==  Weapon.RIFLE) && isAiming){
					animator.SetInteger("Action", 1);
					if(weapon == Weapon.RIFLE && hipShooting == true){
						animator.SetInteger("Action", 2);
					}
					animator.SetTrigger("AttackTrigger");
					Debug.Log("Fire");
				}
			}
			if(Input.GetMouseButtonDown(2)){
				animator.SetTrigger("ReloadTrigger");
			}
			//Aiming switch.
			if(inputAiming){
				if(!isAiming){
					isAiming = true;
					animator.SetBool("Aiming", true);
				}
				else{
					isAiming = false;
					animator.SetBool("Aiming", false);
				}
			}
			//Climbing.
			if(rpgCharacterState == RPGCharacterState.CLIMBING && !isClimbing){
				if(inputVertical > 0.1f){
					animator.applyRootMotion = true;
					animator.SetInteger("Action", 1);
					animator.SetTrigger("ClimbLadderTrigger");
					isClimbing = true;
				}
				else if(inputVertical < -0.1f){
					animator.applyRootMotion = true;
					animator.SetInteger("Action", 2);
					animator.SetTrigger("ClimbLadderTrigger");
					isClimbing = true;
				}
			}
			if(rpgCharacterState == RPGCharacterState.CLIMBING && isClimbing){
				if(inputVertical == 0){
					isClimbing = false;
				}
			}
			//Slow time.
			if(Input.GetKeyDown(KeyCode.T)){
				if(Time.timeScale != 1){
					Time.timeScale = 1;
				}
				else{
					Time.timeScale = 0.0015f;
				}
			}
			//Pause.
			if(Input.GetKeyDown(KeyCode.P)){
				if(Time.timeScale != 1){
					Time.timeScale = 1;
				}
				else{
					Time.timeScale = 0f;
				}
			}
			//Injury.
			if(Input.GetKeyDown(KeyCode.I)){
				if(injured == false){
					injured = true;
					animator.SetBool("Injured", true);
				}
				else{
					injured = false;
					animator.SetBool("Injured", false);
				}
			}
			//Head look.
			if(Input.GetKeyDown(KeyCode.L)){
				if(headLook == false){
					headLook = true;
					isHeadlook = true;
				}
				else{
					headLook = false;
					isHeadlook = false;
				}
			}
		}
		
		void FixedUpdate(){
			if(rpgCharacterState != RPGCharacterState.SWIMMING){
				CheckForGrounded();
				//Apply Gravity.
				rb.AddForce(0, gravity, 0, ForceMode.Acceleration);
				//Check if can move.
				if(canMove && !isBlocking && rpgCharacterState != RPGCharacterState.CLIMBING){
					AirControl();
				}
				//Check if falling.
				if(rb.velocity.y < fallingVelocity && rpgCharacterState != RPGCharacterState.CLIMBING){
					isFalling = true;
					animator.SetInteger("Jumping", 2);
					canJump = false;
					rb.drag = 0f;
				}
				else{
					isFalling = false;
					if(!isJumping && !isFalling && isGrounded){
						rb.drag = 0;
						if(distanceToGround > 0.11f && distanceToGround < slopeAmount && inputVec == Vector3.zero){
							onAllowableSlope = true;
						}
						else{
							onAllowableSlope = false;
						}
					}
				}
			}
			else{
				WaterControl();
			}
			moveSpeed = UpdateMovement();
		}
		
		//Get velocity of rigid body and pass the value to the animator to control the animations.
		void LateUpdate(){
			//Get local velocity of charcter.
			float velocityXel = transform.InverseTransformDirection(rb.velocity).x;
			float velocityZel = transform.InverseTransformDirection(rb.velocity).z;
			//Update animator with movement values.
			animator.SetFloat("Velocity X", velocityXel / runSpeed);
			animator.SetFloat("Velocity Z", velocityZel / runSpeed);
			//If alive and can move, set animator.
			if(!isDead && canMove){
				if(moveSpeed > 0){
					animator.SetBool("Moving", true);
					isMoving = true;
				}
				else{
					animator.SetBool("Moving", false);
					isMoving = false;
				}
				if(isSprinting){
					animator.SetBool("Sprint", true);
				}
				else{
					animator.SetBool("Sprint", false);
				}
			}
			//If using Navmesh nagivation, update values.
			if(useMeshNav){
				if(navMeshAgent.velocity.sqrMagnitude > 0){
					animator.SetBool("Moving", true);
					animator.SetFloat("Velocity Z", navMeshAgent.velocity.magnitude);
				}
			}
			//Headlook.
			if(headLookController != null){
				if(canAction && isHeadlook == true && !isAiming){
					headLookController.m_Weight += 0.03f;
				}
				else{
					headLookController.m_Weight -= 0.03f;
				}
				if(headLookController.m_Weight > 1){
					headLookController.m_Weight = 1;
				}
				else if(headLookController.m_Weight < 0){
					headLookController.m_Weight = 0;
				}
			}
		}

		void UpdateAnimationSpeed(){
			animator.SetFloat("AnimationSpeed", animationSpeed);
		}
		
		#endregion
		
		#region Movement

		/// <summary>
		/// Movement based off camera facing.
		/// </summary>
		void CameraRelativeInput(){
			//Camera relative movement.
			Transform cameraTransform = sceneCamera.transform;
			//Forward vector relative to the camera along the x-z plane   
			Vector3 forward = cameraTransform.TransformDirection(Vector3.forward);
			forward.y = 0;
			forward = forward.normalized;
			//Right vector relative to the camera always orthogonal to the forward vector.
			Vector3 right = new Vector3(forward.z, 0, -forward.x);
			//Directional inputs.
			if(!isRolling && !isAiming){
				targetDashDirection = inputDashHorizontal * right + inputDashVertical * -forward;
			}
			inputVec = inputHorizontal * right + inputVertical * forward;
		}
		
		/// <summary>
		/// Applies velocity to rigidbody to move the character, and controls rotation if not aiming.
		/// </summary>
		/// <returns>The movement.</returns>
		float UpdateMovement(){
			Vector3 motion = inputVec;
			if(isGrounded && rpgCharacterState != RPGCharacterState.CLIMBING){
				//Reduce input for diagonal movement.
				if(motion.magnitude > 1){
					motion.Normalize();
				}
				if(canMove && !isBlocking && !useMeshNav){
					//Set speed by walking / running.
					if((isStrafing && !isAiming) || injured){
						newVelocity = motion * walkSpeed;
					}
					else if(isSprinting && (weapon == Weapon.UNARMED || weapon == Weapon.RELAX)){
						newVelocity = motion * sprintSpeed;
					}
					else{
						newVelocity = motion * runSpeed;
					}
					//If rolling use rolling speed and direction.
					if(isRolling){
						//Force the dash movement to 1.
						targetDashDirection.Normalize();
						newVelocity = rollSpeed * targetDashDirection;
					}
				}
			}
			else{
				if(rpgCharacterState != RPGCharacterState.SWIMMING){
					//Falling, use momentum.
					newVelocity = rb.velocity;
				}
				else{
					newVelocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
				}
			}
			if(isStrafing && weapon != Weapon.RELAX && !isSprinting){
				//Point at target.
				Quaternion targetRotation;
				Vector3 targetPos = target.transform.position;
				targetRotation = Quaternion.LookRotation(targetPos - new Vector3(transform.position.x, 0, transform.position.z));
				transform.eulerAngles = Vector3.up * Mathf.MoveTowardsAngle(transform.eulerAngles.y, targetRotation.eulerAngles.y, (rotationSpeed * Time.deltaTime) * rotationSpeed);
			}
			else if(isAiming && !isSprinting && weapon != Weapon.RELAX){
				Aiming();
			}
			else{
				if(canMove){
					RotateTowardsMovementDir();
				}
			}
			///If on slope, freeze movement.
			if(!onAllowableSlope){
				//Falling, use momentum.
				newVelocity.y = rb.velocity.y;
				rb.velocity = newVelocity;
			}
			else{
				rb.velocity = Vector3.zero;
			}
			//Return movement value for animator.
			return inputVec.magnitude;
		}
		
		/// <summary>
		/// Rotate character towards movement direction.
		/// </summary>
		void RotateTowardsMovementDir(){
//			Debug.Log("RotateTowardsMovementDir");
			if(inputVec != Vector3.zero){
				if(weapon != Weapon.RELAX){
					if(!isStrafing && !isAiming && !isRolling && !isBlocking && rpgCharacterState != RPGCharacterState.CLIMBING){
						transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(inputVec), Time.deltaTime * rotationSpeed);
					}
				}
				else{
					transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(inputVec), Time.deltaTime * rotationSpeed);
				}
			}
		}

		void Rolling(){
			if(!isRolling && isGrounded && !isAiming){
				if(Input.GetAxis("DashVertical") > 0.5f || Input.GetAxis("DashVertical") < -0.5f || Input.GetAxis("DashHorizontal") > 0.5f || Input.GetAxis("DashHorizontal") < -0.5f){
					StartCoroutine(_DirectionalRoll());
				}
			}
		}

		public IEnumerator _DirectionalRoll(){
			//Check which way the dash is pressed relative to the character facing.
			float angle = Vector3.Angle(targetDashDirection, -transform.forward);
			float sign = Mathf.Sign(Vector3.Dot(transform.up, Vector3.Cross(targetDashDirection, transform.forward)));
			//Angle in [-179,180].
			float signed_angle = angle * sign;
			//Angle in 0-360.
			float angle360 = (signed_angle + 180) % 360;
			//Deternime the animation to play based on the angle.
			if(angle360 > 315 || angle360 < 45){
				StartCoroutine(_Roll(1));
			}
			if(angle360 > 45 && angle360 < 135){
				StartCoroutine(_Roll(2));
			}
			if(angle360 > 135 && angle360 < 225){
				StartCoroutine(_Roll(3));
			}
			if(angle360 > 225 && angle360 < 315){
				StartCoroutine(_Roll(4));
			}
			yield return null;
		}

		/// <summary>
		/// Character Roll.
		/// </summary>
		/// <param name="1">Forward.</param>
		/// <param name="2">Right.</param>
		/// <param name="3">Backward.</param>
		/// <param name="4">Left.</param>
		public IEnumerator _Roll(int rollNumber){
			AnimatorDebug();
			if(weapon == Weapon.RELAX){
				weapon = Weapon.UNARMED;
				animator.SetInteger("Weapon", 0);
			}
			animator.SetInteger("Action", rollNumber);
			animator.SetTrigger("RollTrigger");
			isRolling = true;
			canAction = false;
			yield return new WaitForSeconds(rollduration);
			isRolling = false;
			canAction = true;
		}

		/// <summary>
		/// Dodge the specified direction.
		/// </summary>
		/// <param name="1">Left</param>
		/// <param name="2">Right</param>
		public IEnumerator _Dodge(int direction){
			if(weapon == Weapon.RELAX){
				weapon = Weapon.UNARMED;
				animator.SetInteger("Weapon", 0);
			}
			animator.SetInteger("Action", direction);
			animator.SetTrigger("DodgeTrigger");
			StartCoroutine(_Lock(true, true, true, 0, 0.55f));
			yield return null;
		}

		/// <summary>
		/// Climbing.
		/// </summary>
		/// <param name="Climb-Up">1</param>
		/// <param name="Climb-Down">2</param>
		/// <param name="Climb-Off-Top">3</param>
		/// <param name="Climb-Off-Bottom">4</param>
		/// <param name="Climb-On-Top">5</param>
		/// <param name="Climb-On-Bottom">6</param>
		public IEnumerator _Climbing(){
			gravityTemp = gravity;
			gravity = 0;
			rb.useGravity = false;
			animator.applyRootMotion = true;
			animator.SetInteger("Action", 6);
			animator.SetTrigger("ClimbLadderTrigger");
			//Get the direction of the ladder, and snap the character to the correct position and facing.
			Vector3 newVector = Vector3.Cross(ladder.transform.forward, ladder.transform.right);
			Debug.DrawRay(ladder.transform.position, newVector, Color.red, 2f);
			Vector3 newSpot = ladder.transform.position + (newVector.normalized * 0.71f);
			transform.position = new Vector3(newSpot.x, 0, newSpot.z);
			transform.rotation = Quaternion.Euler(transform.rotation.x, ladder.transform.rotation.eulerAngles.y, transform.rotation.z);
			canMove = false;
			yield return new WaitForSeconds(1f);
			rpgCharacterState = RPGCharacterState.CLIMBING;
		}

		public void EndClimbing(){
			rpgCharacterState = RPGCharacterState.DEFAULT;
			gravity = gravityTemp;
			rb.useGravity = true;
			animator.applyRootMotion = false;
			canMove = true;
			isClimbing = false;
		}

		//Keep character from moving.
		void LockMovement(){
			canMove = false;
			animator.SetBool("Moving", false);
			rb.velocity = Vector3.zero;
			rb.angularVelocity = Vector3.zero;
			inputVec = new Vector3(0, 0, 0);
			animator.applyRootMotion = true;
		}

		#endregion
		
		#region Aiming / Turning
		
		void Aiming(){
//			Debug.Log("Aiming");
			for(int i = 0; i < Input.GetJoystickNames().Length; i++){
				//If right joystick is moved, use that for facing.
				if(Mathf.Abs(inputDashHorizontal) > 0.1 || Mathf.Abs(inputDashVertical) < -0.1){
					Vector3 joyDirection = new Vector3(inputDashHorizontal, 0, -inputDashVertical);
					joyDirection = joyDirection.normalized;
					Quaternion joyRotation = Quaternion.LookRotation(joyDirection);
					transform.rotation = joyRotation;
				}
			}
			//No joysticks, use mouse aim.
			if(Input.GetJoystickNames().Length == 0){
//				Debug.Log("MouseAim");
				Plane characterPlane = new Plane(Vector3.up, transform.position);
				Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
				Vector3 mousePosition = new Vector3(0, 0, 0);
				float hitdist = 0.0f;
				if(characterPlane.Raycast(ray, out hitdist)){
					mousePosition = ray.GetPoint(hitdist);
				}
				mousePosition = new Vector3(mousePosition.x, transform.position.y, mousePosition.z);
				Vector3 relativePos = transform.position - mousePosition;
				Quaternion rotation = Quaternion.LookRotation(-relativePos);
				transform.rotation = rotation;
			}
		}
		
		/// <summary>
		/// Direcitonal aiming used by 2Handed Bow.
		/// </summary>
		void DirectionalAiming(){
			if(Input.GetKey(KeyCode.LeftArrow)){
				aimHorizontal -= 0.05f;
			}
			if(Input.GetKey(KeyCode.RightArrow)){
				aimHorizontal += 0.05f;
			}
			if(Input.GetKey(KeyCode.DownArrow)){
				aimVertical -= 0.05f;
			}
			if(Input.GetKey(KeyCode.UpArrow)){
				aimVertical += 0.05f;
			}
			if(aimHorizontal >= 1){
				aimHorizontal = 1;
			}
			if(aimHorizontal <= -1){
				aimHorizontal = -1;
			}
			if(aimVertical >= 1){
				aimVertical = 1;
			}
			if(aimVertical <= -1){
				aimVertical = -1;
			}
			if(Input.GetKey(KeyCode.B)){
				bowPull -= 0.05f;
			}
			if(Input.GetKey(KeyCode.N)){
				bowPull += 0.05f;
			}
			if(bowPull >= 1){
				bowPull = 1;
			}
			if(bowPull <= -1){
				bowPull = -1;
			}
			//Set the animator.
			animator.SetFloat("AimHorizontal", aimHorizontal);
			animator.SetFloat("AimVertical", aimVertical);
			animator.SetFloat("BowPull", bowPull);
		}
		
		//Turning.
		public IEnumerator _Turning(int direction){
			if(direction == 1){
				StartCoroutine(_Lock(true, true, true, 0, 0.55f));
				animator.SetTrigger("TurnLeftTrigger");
			}
			if(direction == 2){
				StartCoroutine(_Lock(true, true, true, 0, 0.55f));
				animator.SetTrigger("TurnRightTrigger");
			}
			yield return null;
		}
		
		#endregion
		
		#region Swimming

		/// <summary>
		/// Movement when in water volume.
		/// </summary>
		void WaterControl(){
			AscendDescend();
			Vector3 motion = inputVec;
			//Dampen vertical water movement.
			Vector3 dampenVertical = new Vector3(rb.velocity.x, (rb.velocity.y * 0.985f), rb.velocity.z);
			rb.velocity = dampenVertical;
			Vector3 waterDampen = new Vector3((rb.velocity.x * 0.98f), rb.velocity.y, (rb.velocity.z * 0.98f));
			//If swimming, don't dampen movement, and scale capsule collider.
			if(moveSpeed < 0.1f){
				rb.velocity = waterDampen;
				capCollider.radius = 0.5f;
			}
			else{
				capCollider.radius = 1.5f;
			}
			rb.velocity = waterDampen;
			//Clamp diagonal movement so its not faster.
			motion *= (Mathf.Abs(inputVec.x) == 1 && Mathf.Abs(inputVec.z) == 1) ? 0.7f : 1;
			rb.AddForce(motion * inWaterSpeed, ForceMode.Acceleration);
			//Limit the amount of velocity we can achieve to water speed.
			float velocityX = 0;
			float velocityZ = 0;
			if(rb.velocity.x > inWaterSpeed){
				velocityX = GetComponent<Rigidbody>().velocity.x - inWaterSpeed;
				if(velocityX < 0){
					velocityX = 0;
				}
				rb.AddForce(new Vector3(-velocityX, 0, 0), ForceMode.Acceleration);
			}
			if(rb.velocity.x < minVelocity){
				velocityX = rb.velocity.x - minVelocity;
				if(velocityX > 0){
					velocityX = 0;
				}
				rb.AddForce(new Vector3(-velocityX, 0, 0), ForceMode.Acceleration);
			}
			if(rb.velocity.z > inWaterSpeed){
				velocityZ = rb.velocity.z - maxVelocity;
				if(velocityZ < 0){
					velocityZ = 0;
				}
				rb.AddForce(new Vector3(0, 0, -velocityZ), ForceMode.Acceleration);
			}
			if(rb.velocity.z < minVelocity){
				velocityZ = rb.velocity.z - minVelocity;
				if(velocityZ > 0){
					velocityZ = 0;
				}
				rb.AddForce(new Vector3(0, 0, -velocityZ), ForceMode.Acceleration);
			}
		}

		/// <summary>
		/// Swim upwards while in a water volume.
		/// </summary>
		void AscendDescend(){
			if(doJump){
				//Swim down with left control.
				if(isStrafing){
					animator.SetBool("Strafing", true);
					animator.SetTrigger("JumpTrigger");
					rb.velocity -= inWaterSpeed * Vector3.up;
				}
				else{
					animator.SetTrigger("JumpTrigger");
					rb.velocity += inWaterSpeed * Vector3.up;
				}
			}
		}
		
		void OnTriggerEnter(Collider collide){
			//Entering a water volume.
			if(collide.gameObject.layer == 4){
				rpgCharacterState = RPGCharacterState.SWIMMING;
				canAction = false;
				rb.useGravity = false;
				animator.SetTrigger("SwimTrigger");
				animator.SetBool("Swimming", true);
				animator.SetInteger("Weapon", 0);
				weapon = Weapon.UNARMED;
				StartCoroutine(_WeaponVisibility(leftWeapon, false, true));
				animator.SetInteger("RightWeapon", 0);
				animator.SetInteger("LeftWeapon", 0);
				animator.SetInteger("LeftRight", 0);
				FXSplash.Emit(30);
			}
			else if(collide.transform.parent != null){
				if(collide.transform.parent.name.Contains("Ladder")){
					isNearLadder = true;
					ladder = collide.gameObject;
				}
			}
			else if(collide.transform.name.Contains("Cliff")){
				isNearCliff = true;
				cliff = collide.gameObject;
			}
		}
		
		void OnTriggerExit(Collider collide){
			//Leaving a water volume.
			if(collide.gameObject.layer == 4){
				rpgCharacterState = RPGCharacterState.DEFAULT;
				canAction = true;
				rb.useGravity = true;
				animator.SetInteger("Jumping", 2);
				animator.SetBool("Swimming", false);
				capCollider.radius = 0.5f;
			}
			//Leaving a ladder.
			else if(collide.transform.parent != null){
				if(collide.transform.parent.name.Contains("Ladder")){
					isNearLadder = false;
					ladder = null;
				}
			}
		}
		
		#endregion
		
		#region Jumping
		
		/// <summary>
		/// Checks if character is within a certain distance from the ground, and markes it IsGrounded.
		/// </summary>
		void CheckForGrounded(){
			RaycastHit hit;
			Vector3 offset = new Vector3(0, 0.1f, 0);
			if(Physics.Raycast((transform.position + offset), -Vector3.up, out hit, 100f)){
				distanceToGround = hit.distance;
				if(distanceToGround < slopeAmount){
					isGrounded = true;
					if(!isJumping){
						canJump = true;
					}
					startFall = false;
					doublejumped = false;
					canDoubleJump = false;
					isFalling = false;
					fallTimer = 0;
					if(!isJumping){
						animator.SetInteger("Jumping", 0);
					}
					//Exit climbing on ground.
					if(rpgCharacterState == RPGCharacterState.CLIMBING){
						animator.SetInteger("Action", 4);
						animator.SetTrigger("ClimbLadderTrigger");
						gravity = gravityTemp;
						rb.useGravity = true;
						rpgCharacterState = RPGCharacterState.DEFAULT;
						StartCoroutine(_Lock(true, true, true, 0f, 1f));
					}
				}
				else{
					fallTimer += 0.009f;
					if(fallTimer >= fallDelay){
						isGrounded = false;
					}
				}
			}
		}
		
		void Jumping(){
			if(isGrounded){
				if(canJump && doJump){
					StartCoroutine(_Jump());
				}
			}
			else{    
				canDoubleJump = true;
				canJump = false;
				if(isFalling){
					//Set the animation back to falling.
					animator.SetInteger("Jumping", 2);
					//Prevent from going into land animation while in air.
					if(!startFall){
						animator.SetTrigger("JumpTrigger");
						startFall = true;
					}
				}
				if(canDoubleJump && doublejumping && Input.GetButtonDown("Jump") && !doublejumped && isFalling){
					//Apply the current movement to launch velocity.
					rb.velocity += doublejumpSpeed * Vector3.up;
					animator.SetInteger("Jumping", 3);
					doublejumped = true;
				}
			}
		}
		
		public IEnumerator _Jump(){
			if(weapon == Weapon.RELAX){
				weapon = Weapon.UNARMED;
				animator.SetInteger("Weapon", 0);
			}
			isJumping = true;
			canJump = false;
			rb.drag = 0f;
			animator.SetInteger("Jumping", 1);
			animator.SetTrigger("JumpTrigger");
			//Apply the current movement to launch velocity.
			rb.velocity += jumpSpeed * Vector3.up;
			canJump = false;
			yield return new WaitForSeconds(0.5f);
			isJumping = false;
		}
		
		/// <summary>
		/// Controls movement of character while !isGrounded.
		/// </summary>
		void AirControl(){
			if(!isGrounded){
				Vector3 motion = inputVec;
				motion *= (Mathf.Abs(inputVec.x) == 1 && Mathf.Abs(inputVec.z) == 1) ? 0.7f : 1;
				rb.AddForce(motion * inAirSpeed, ForceMode.Acceleration);
				//Limit the amount of velocity in character.
				float velocityX = 0;
				float velocityZ = 0;
				if(rb.velocity.x > maxVelocity){
					velocityX = GetComponent<Rigidbody>().velocity.x - maxVelocity;
					if(velocityX < 0){
						velocityX = 0;
					}
					rb.AddForce(new Vector3(-velocityX, 0, 0), ForceMode.Acceleration);
				}
				if(rb.velocity.x < minVelocity){
					velocityX = rb.velocity.x - minVelocity;
					if(velocityX > 0){
						velocityX = 0;
					}
					rb.AddForce(new Vector3(-velocityX, 0, 0), ForceMode.Acceleration);
				}
				if(rb.velocity.z > maxVelocity){
					velocityZ = rb.velocity.z - maxVelocity;
					if(velocityZ < 0){
						velocityZ = 0;
					}
					rb.AddForce(new Vector3(0, 0, -velocityZ), ForceMode.Acceleration);
				}
				if(rb.velocity.z < minVelocity){
					velocityZ = rb.velocity.z - minVelocity;
					if(velocityZ > 0){
						velocityZ = 0;
					}
					rb.AddForce(new Vector3(0, 0, -velocityZ), ForceMode.Acceleration);
				}
			}
		}
		
		#endregion
		
		#region Combat
		
		//0 = No side
		//1 = Left
		//2 = Right
		//3 = Dual
		//weaponNumber 0 = Unarmed
		//weaponNumber 1 = 2H Sword
		//weaponNumber 2 = 2H Spear
		//weaponNumber 3 = 2H Axe
		//weaponNumber 4 = 2H Bow
		//weaponNumber 5 = 2H Crowwbow
		//weaponNumber 6 = 2H Staff
		//weaponNumber 7 = Shield
		//weaponNumber 8 = L Sword
		//weaponNumber 9 = R Sword
		//weaponNumber 10 = L Mace
		//weaponNumber 11 = R Mace
		//weaponNumber 12 = L Dagger
		//weaponNumber 13 = R Dagger
		//weaponNumber 14 = L Item
		//weaponNumber 15 = R Item
		//weaponNumber 16 = L Pistol
		//weaponNumber 17 = R Pistol
		//weaponNumber 18 = Rifle
		//weaponNumber 19 == Right Spear
		//weaponNumber 20 == 2H Club
		public void Attack(int attackSide){
			AnimatorDebug();
			int attackNumber = 0;
			if(canAction){
				//Ground attacks.
				if(isGrounded){
					//Stationary attack.
					if(!isMoving){
						if(weapon == Weapon.RELAX){
							weapon = Weapon.UNARMED;
							animator.SetInteger("Weapon", 0);
						}
						//Armed or Unarmed.
						if(weapon == Weapon.UNARMED || weapon == Weapon.ARMED || weapon == Weapon.ARMEDSHIELD){
							//If armed and moving, running attacks.
							if((weapon == Weapon.ARMED || weapon == Weapon.ARMEDSHIELD) && inputVec != Vector3.zero){
								if(attackSide == 1){
									animator.SetInteger("Attack", 1);
									animator.SetTrigger("AttackTrigger");
								}
								if(attackSide == 2){
									animator.SetInteger("Attack", 4);
									animator.SetTrigger("AttackTrigger");
								}
								if(attackSide == 3){
									animator.SetInteger("Attack", 1);
									animator.SetTrigger("AttackDualTrigger");
								}
							}
							//Not moving.
							else{
								int maxAttacks = 3;
								//Left attacks.
								if(attackSide == 1){
									animator.SetInteger("AttackSide", 1);
									//Left sword has 6 attacks.
									if(leftWeapon == 8){
										attackNumber = Random.Range(1, 8);
									}
									//Left item has 4 attacks.
									else if(leftWeapon == 14){
										attackNumber = Random.Range(1, 5);
									}
									else{
										attackNumber = Random.Range(1, maxAttacks + 1);
									}
								}
								//Right attacks.
								else if(attackSide == 2){
									animator.SetInteger("AttackSide", 2);
									//Right sword has 6 attacks.
									if(rightWeapon == 9){
										attackNumber = Random.Range(8, 15);
									}
									//Right item has 4 attacks.
									else if(rightWeapon == 15){
										attackNumber = Random.Range(5, 10);
									}
									//Right spear has 7 attacks.
									else if(rightWeapon == 19){
										attackNumber = Random.Range(1, 8);
									}
									else{
										attackNumber = Random.Range(4, maxAttacks + 4);
									}
								}
								//Dual attacks.
								else if(attackSide == 3){
									attackNumber = Random.Range(1, maxAttacks + 1);
								}
								//Set the Locks.
								if(attackSide != 3){
									if(leftWeapon == 12 || leftWeapon == 14 || rightWeapon == 13 || rightWeapon == 15 || rightWeapon == 19){
										StartCoroutine(_Lock(true, true, true, 0, 0.75f));
									}
									else{
										StartCoroutine(_Lock(true, true, true, 0, 0.7f));
									}
								}
								//Dual attacks.
								else{
									StartCoroutine(_Lock(true, true, true, 0, 0.75f));
								}
							}
						}
						//Shield or 2Handed Weapons.
						else if(weapon == Weapon.SHIELD){
							int maxAttacks = 1;
							attackNumber = Random.Range(1, maxAttacks);
							StartCoroutine(_Lock(true, true, true, 0, 1.1f));
						}
						else if(weapon == Weapon.TWOHANDSPEAR){
							int maxAttacks = 10;
							attackNumber = Random.Range(1, maxAttacks);
							StartCoroutine(_Lock(true, true, true, 0, 1.1f));
						}
						else if(weapon == Weapon.TWOHANDCLUB){
							int maxAttacks = 10;
							attackNumber = Random.Range(1, maxAttacks);
							StartCoroutine(_Lock(true, true, true, 0, 1.1f));
						}
						else if(weapon == Weapon.TWOHANDSWORD){
							int maxAttacks = 11;
							attackNumber = Random.Range(1, maxAttacks);
							StartCoroutine(_Lock(true, true, true, 0, 1.1f));
						}
						else if(weapon == Weapon.RIFLE){
							int maxAttacks = 3;
							attackNumber = Random.Range(1, maxAttacks);
							StartCoroutine(_Lock(true, true, true, 0, 1.1f));
						}
						else{
							int maxAttacks = 6;
							attackNumber = Random.Range(1, maxAttacks);
							if(weapon == Weapon.TWOHANDSWORD){
								StartCoroutine(_Lock(true, true, true, 0, 0.85f));
							}
							else if(weapon == Weapon.TWOHANDAXE){
								StartCoroutine(_Lock(true, true, true, 0, 1.5f));
							}
							else if(weapon == Weapon.STAFF){
								StartCoroutine(_Lock(true, true, true, 0, 1f));
							}
							else{
								StartCoroutine(_Lock(true, true, true, 0, 0.75f));
							}
						}
					}
					//Running attack.
					else{
						RunningAttack(attackSide);
						return;
					}
				}
				//Air attacks.
				else{
					AirAttack();
					return;
				}
			}
			//Trigger the animation.
			animator.SetInteger("Action", attackNumber);
			if(attackSide == 3){
				animator.SetTrigger("AttackDualTrigger");
			}
			else{
				animator.SetTrigger("AttackTrigger");
			}
		}

		void RunningAttack(int attackSide){
			Debug.Log("RunningAttack: " + attackSide);
			animator.SetInteger("Action", 1);
			animator.SetTrigger("AttackTrigger");
		}

		void AirAttack(){
			animator.SetInteger("Action", 1);
			animator.SetTrigger("AttackTrigger");
			StartCoroutine(_Jump());
		}
		
		public void AttackKick(int kickSide){
			if(isGrounded){
				if(weapon == Weapon.RELAX){
					weapon = Weapon.UNARMED;
					animator.SetInteger("Weapon", 0);
				}
				animator.SetInteger("Action", kickSide);
				animator.SetTrigger("AttackKickTrigger");
				StartCoroutine(_Lock(true, true, true, 0, 0.8f));
			}
		}
		
		public void Special(int special){
			if(weapon == Weapon.RELAX){
				weapon = Weapon.UNARMED;
				animator.SetInteger("Weapon", 0);
			}
			if(specialAttack == 0){
				specialAttack = special;
				animator.SetInteger("Action", special);
				animator.SetTrigger("SpecialAttackTrigger");
				StartCoroutine(_Lock(true, true, true, 0, 0.5f));
			}
			else{
				animator.SetTrigger("SpecialEndTrigger");
				StartCoroutine(_Lock(true, true, true, 0, 0.6f));
				UnLock(true, true);
				specialAttack = 0;
			}
		}
		
		/// <summary>
		/// Cast the specified attackSide and type. 
		///0 = No side
		///1 = Left
		///2 = Right
		///3 = Dual
		/// </summary>
		public void Cast(int attackSide, string type){
			if(weapon == Weapon.RELAX){
				weapon = Weapon.UNARMED;
				animator.SetInteger("Weapon", 0);
			}
			//Cancel current casting.
			if(attackSide == 0){
				animator.SetTrigger("CastEndTrigger");
				isCasting = false;
				canAction = true;
				StartCoroutine(_Lock(true, true, true, 0, 0.1f));
				return;
			}
			int maxAttacks = 3;
			//Set Left, Right, Dual for variable casts.
			if(attackSide == 4){
				if(leftWeapon == 0 && rightWeapon == 0){
					animator.SetInteger("LeftRight", 3);
				}
				else if(leftWeapon == 0){
					animator.SetInteger("LeftRight", 1);
				}
				else if(rightWeapon == 0){
					animator.SetInteger("LeftRight", 2);
				}
			}
			else{
				animator.SetInteger("LeftRight", attackSide);
			}
			//Cast Buffs, AOE, Summons.
			if(weapon == Weapon.UNARMED || weapon == Weapon.STAFF || weapon == Weapon.ARMED){
				//Buff1 = 1
				//Buff2 = 2
				//AOE1 = 3
				//AOE2 = 4
				//Summon1 = 5
				//Summon2 = 6
				maxAttacks = 2;
				int attackNumber = Random.Range(1, maxAttacks + 1);
				if(isGrounded){
					if(type == "buff"){
						animator.SetInteger("Action", attackNumber);
					}
					else if(type == "AOE"){
						animator.SetInteger("Action", attackNumber + 2);
					}
					else if(type == "summon"){
						animator.SetInteger("Action", attackNumber + 4);
					}
				}
			}
			//Trigger Cast if character is grounded.
			if(isGrounded){
				if(type == "attack"){
					animator.SetInteger("Action", Random.Range(1, maxAttacks + 1));
					animator.SetTrigger("AttackCastTrigger");
				}
				else{
					animator.SetTrigger("CastTrigger");
				}
				isCasting = true;
				canAction = false;
				StartCoroutine(_Lock(true, true, false, 0, 0.8f));
			}
			//Character is in air, do air Cast.
			else{
			}
		}
		
		public void Blocking(){
			if(Input.GetAxisRaw("TargetBlock") < -0.1f && canAction && isGrounded){
				if(!isBlocking){
					animator.SetTrigger("BlockTrigger");
				}
				isBlocking = true;
				animator.SetBool("Blocking", true);
				rb.velocity = Vector3.zero;
				rb.angularVelocity = Vector3.zero;
				inputVec = Vector3.zero;
			}
			else{
				isBlocking = false;
				animator.SetBool("Blocking", false);
			}
		}
		
		public void GetHit(){
			if(weapon == Weapon.RELAX){
				weapon = Weapon.UNARMED;
				animator.SetInteger("Weapon", 0);
			}
			if(weapon != Weapon.RIFLE || weapon != Weapon.TWOHANDCROSSBOW){
				int hits = 5;
				if(isBlocking){
					hits = 2;
				}
				int hitNumber = Random.Range(1, hits + 1);
				animator.SetInteger("Action", hitNumber);
				animator.SetTrigger("GetHitTrigger");
				StartCoroutine(_Lock(true, true, true, 0.1f, 0.4f));
				if(isBlocking){
					StartCoroutine(_Knockback(-transform.forward, 3, 3));
					return;
				}
				//Apply directional knockback force.
				if(hitNumber <= 1){
					StartCoroutine(_Knockback(-transform.forward, 8, 4));
				}
				else if(hitNumber == 2){
					StartCoroutine(_Knockback(transform.forward, 8, 4));
				}
				else if(hitNumber == 3){
					StartCoroutine(_Knockback(transform.right, 8, 4));
				}
				else if(hitNumber == 4){
					StartCoroutine(_Knockback(-transform.right, 8, 4));
				}
			}
		}
			
		IEnumerator _Knockback(Vector3 knockDirection, int knockBackAmount, int variableAmount){
			isKnockback = true;
			StartCoroutine(_KnockbackForce(knockDirection, knockBackAmount, variableAmount));
			yield return new WaitForSeconds(.1f);
			isKnockback = false;
		}
		
		IEnumerator _KnockbackForce(Vector3 knockDirection, int knockBackAmount, int variableAmount){
			while(isKnockback){
				rb.AddForce(knockDirection * ((knockBackAmount + Random.Range(-variableAmount, variableAmount)) * (knockbackMultiplier * 10)), ForceMode.Impulse);
				yield return null;
			}
		}
		
		public IEnumerator _Death(){
			animator.SetTrigger("Death1Trigger");
			StartCoroutine(_Lock(true, true, true, 0.1f, 1.5f));
			isDead = true;
			animator.SetBool("Moving", false);
			inputVec = new Vector3(0, 0, 0);
			yield return null;
		}
		
		public IEnumerator _Revive(){
			animator.SetTrigger("Revive1Trigger");
			StartCoroutine(_Lock(true, true, true, 0f, 1.45f));
			isDead = false;
			yield return null;
		}
		
		#endregion
		
		#region Weapons

		//weaponNumber -1 = Relax
		//weaponNumber 0 = Unarmed
		//weaponNumber 1 = 2H Sword
		//weaponNumber 2 = 2H Spear
		//weaponNumber 3 = 2H Axe
		//weaponNumber 4 = 2H Bow
		//weaponNumber 5 = 2H Crowwbow
		//weaponNumber 6 = 2H Staff
		//weaponNumber 7 = Shield
		//weaponNumber 8 = L Sword
		//weaponNumber 9 = R Sword
		//weaponNumber 10 = L Mace
		//weaponNumber 11 = R Mace
		//weaponNumber 12 = L Dagger
		//weaponNumber 13 = R Dagger
		//weaponNumber 14 = L Item
		//weaponNumber 15 = R Item
		//weaponNumber 16 = L Pistol
		//weaponNumber 17 = R Pistol
		//weaponNumber 18 = Rifle
		//weaponNumber 19 == Right Spear
		//weaponNumber 20 == 2H Club
		public IEnumerator _SwitchWeapon(int weaponNumber){
			//Debug.Log("Switch Weapon: " + weaponNumber);
			if(instantWeaponSwitch){
				StartCoroutine(_InstantWeaponSwitch(weaponNumber));
				yield break;
			}
			//If is Unarmed/Relax.
			if(IsNoWeapon(animator.GetInteger("Weapon"))){
				//Switch to Relax.
				if(weaponNumber == -1){
					StartCoroutine(_SheathWeapon(0, -1));
				}
				//Switch to Unarmed.
				else{
					StartCoroutine(_UnSheathWeapon(weaponNumber));
				}
			}
			//Character has 2handed weapon.
			else if(Is2HandedWeapon(animator.GetInteger("Weapon"))){
				StartCoroutine(_SheathWeapon(leftWeapon, weaponNumber));
				yield return new WaitForSeconds(1.1f);
				if(weaponNumber > 0){
					StartCoroutine(_UnSheathWeapon(weaponNumber));
				}
			}
			//Character has 1handed weapon(s).
			else if(Is1HandedWeapon(animator.GetInteger("Weapon"))){
				//Dual switching with dual wielding.
				if(dualSwitch && leftWeapon != 0 && rightWeapon != 0){
					StartCoroutine(_DualSheath(animator.GetInteger("Weapon"), weaponNumber));
					yield return new WaitForSeconds(1f);
					StartCoroutine(_UnSheathWeapon(weaponNumber));
					yield break;
				}
				//Character is switching to 2handed weapon or Unarmed or Relax, put put away all weapons.
				if(Is2HandedWeapon(weaponNumber) || IsNoWeapon(weaponNumber)){
					//Left hand has a weapon.
					if(leftWeapon != 0){
						StartCoroutine(_SheathWeapon(leftWeapon, weaponNumber));
						yield return new WaitForSeconds(1.05f);
					}
					//Right hand has a weapon.
					if(rightWeapon != 0){
						StartCoroutine(_SheathWeapon(rightWeapon, weaponNumber));
						yield return new WaitForSeconds(1.05f);
					}
					if(weaponNumber > 0){
						StartCoroutine(_UnSheathWeapon(weaponNumber));
					}
				}
				//Switching left weapon, put away left weapon if equipped.
				else if(IsLeftWeapon(weaponNumber)){
					if(leftWeapon > 0){
						StartCoroutine(_SheathWeapon(leftWeapon, weaponNumber));
						yield return new WaitForSeconds(1.05f);
					}
					StartCoroutine(_UnSheathWeapon(weaponNumber));
				}
				//Switching right weapon, put away right weapon if equipped
				else if(IsRightWeapon(weaponNumber)){
					if(leftWeapon > 0 && dualSwitch){
						StartCoroutine(_SheathWeapon(leftWeapon, weaponNumber));
						yield return new WaitForSeconds(1.05f);
						StartCoroutine(_DualUnSheath(weaponNumber));
						yield break;
					}
					if(rightWeapon > 0){
						StartCoroutine(_SheathWeapon(rightWeapon, weaponNumber));
						yield return new WaitForSeconds(1.05f);
					}
					StartCoroutine(_UnSheathWeapon(weaponNumber));
				}
			}
			yield return null;
		}


		public IEnumerator _UnSheathWeapon(int weaponNumber){
			//Debug.Log("UnsheathWeapon: " + weaponNumber);
			//Lock character while animation plays.
			if(!isMoving){
				StartCoroutine(_Lock(true, true, true, 0, 1f));
				yield return new WaitForSeconds(0.3f);
			}
			weaponSwitch = false;
			//Use Dual switch.
			if(dualSwitch){
				StartCoroutine(_DualUnSheath(weaponNumber));
				yield break;
			}
			//Switching to Unarmed from Relax.
			if(weaponNumber == 0){
				DoWeaponSwitch(-1, -1, -1, 0, false);
				yield return new WaitForSeconds(0.5f);
				SetAnimator(0, -2, 0, 0, 0);
			}
			//Switching to 2handed weapon.
			else if(Is2HandedWeapon(weaponNumber)){
				//Switching from 2handed weapon.
				if(Is2HandedWeapon(animator.GetInteger("Weapon"))){
					DoWeaponSwitch(0, weaponNumber, weaponNumber, -1, false);
					yield return new WaitForSeconds(0.5f);
					SetAnimator(weaponNumber, -2, animator.GetInteger("Weapon"), -1, -1);
				}
				else{
					DoWeaponSwitch(animator.GetInteger("Weapon"), weaponNumber, weaponNumber, -1, false);
					yield return new WaitForSeconds(0.5f);
					SetAnimator(weaponNumber, -2, weaponNumber, -1, -1);
				}
			}
			//Switching to 1handed weapons.
			else{
				//If switching from Unarmed or Relax.
				if(IsNoWeapon(animator.GetInteger("Weapon"))){
					animator.SetInteger("WeaponSwitch", 7);
				}
				//Left hand weapons.
				if(weaponNumber == 7 || weaponNumber == 8 || weaponNumber == 10 || weaponNumber == 12 || weaponNumber == 14 || weaponNumber == 16){
					//If not switching Shield.
					if(weaponNumber == 7){
						animator.SetBool("Shield", true);
					}
					DoWeaponSwitch(7, weaponNumber, animator.GetInteger("Weapon"), 1, false);
					yield return new WaitForSeconds(0.5f);
					SetAnimator(7, 7, weaponNumber, -1, 1);
				}
				//Right hand weapons.
				else if(weaponNumber == 9 || weaponNumber == 11 || weaponNumber == 13 || weaponNumber == 15 || weaponNumber == 17 || weaponNumber == 19){
					animator.SetBool("Shield", false);
					DoWeaponSwitch(7, weaponNumber, animator.GetInteger("Weapon"), 2, false);
					yield return new WaitForSeconds(0.5f);
					if(leftWeapon == 7){
						animator.SetBool("Shield", true);
					}
					SetAnimator(7, 7, -1, weaponNumber, 2);
				}
			}
			SetWeaponState(weaponNumber);
			yield return null;
		}

		public IEnumerator _SheathWeapon(int weaponNumber, int weaponTo){
			Debug.Log("Sheath Weapon: " + weaponNumber + " - Weapon To: " + weaponTo);
			//Lock character while animation plays.
			if(!isMoving){
				StartCoroutine(_Lock(true, true, true, 0, 1f));
			}
			//Reset for animation events.
			weaponSwitch = false;
			//Use Dual switch.
			if(dualSwitch){
				StartCoroutine(_DualSheath(weaponNumber, weaponTo));
				yield break;
			}
			//Set LeftRight hand for 1handed switching.
			if(IsLeftWeapon(weaponNumber)){
				animator.SetInteger("LeftRight", 1);
			}
			else if(IsRightWeapon(weaponNumber)){
				animator.SetInteger("LeftRight", 2);
			}
			//Switching to Unarmed or Relaxed.
			if(weaponTo < 1){
				//Have at least 1 weapon.
				if(rightWeapon != 0 || leftWeapon != 0){
					//Sheath 1handed weapon.
					if(Is1HandedWeapon(weaponNumber)){
						//If sheathing both weapons, go to Armed first.
						if(rightWeapon != 0 && leftWeapon != 0){
							DoWeaponSwitch(7, weaponNumber, 7, -1, true);
						}
						else{
							DoWeaponSwitch(weaponTo, weaponNumber, 7, -1, true);
						}
						yield return new WaitForSeconds(0.5f);
						if(IsLeftWeapon(weaponNumber)){
							animator.SetInteger("LeftWeapon", 0);
							SetAnimator(weaponTo, -2, 0, -1, -1);
						}
						else if(IsRightWeapon(weaponNumber)){
							animator.SetInteger("RightWeapon", 0);
							SetAnimator(weaponTo, -2, -1, 0, -1);
						}
						animator.SetBool("Shield", false);
					}
					//Sheath 2handed weapon.
					else if(Is2HandedWeapon(weaponNumber)){
						DoWeaponSwitch(weaponTo, weaponNumber, animator.GetInteger("Weapon"), -1, true);
						yield return new WaitForSeconds(0.5f);
						SetAnimator(weaponTo, -2, 0, 0, -1);
					}
				}
				//Unarmed, switching to Relax.
				else if(rightWeapon == 0 && leftWeapon == 0){
					DoWeaponSwitch(weaponTo, weaponNumber, animator.GetInteger("Weapon"), 0, true);
					yield return new WaitForSeconds(0.5f);
					SetAnimator(weaponTo, -2, 0, 0, -1);
				}
			}
			//Switching to 2handed weapon.
			else if(Is2HandedWeapon(weaponTo)){
				//Switching from 1handed weapons.
				if(animator.GetInteger("Weapon") == 7){
					//Dual weilding, switch to Armed if first switch.
					if(leftWeapon != 0 && rightWeapon != 0){
						DoWeaponSwitch(7, weaponNumber, 7, -1, true);
						if(IsLeftWeapon(weaponNumber)){
							SetAnimator(7, -2, 0, -1, -1);
						}
						else if(IsRightWeapon(weaponNumber)){
							SetAnimator(7, -2, -1, 0, -1);
						}
					}
					else{
						DoWeaponSwitch(0, weaponNumber, 7, -1, true);
						yield return new WaitForSeconds(0.5f);
						SetAnimator(0, -2, 0, 0, -1);
					}
				}
				//Switching from 2handed weapons.
				else{
					DoWeaponSwitch(0, weaponNumber, animator.GetInteger("Weapon"), -1, true);
					yield return new WaitForSeconds(0.5f);
					SetAnimator(weaponNumber, -2, weaponNumber, 0, -1);
				}
			}
			//Switching to 1handed weapons.
			else{
				//Switching from 2handed weapons, go to Unarmed before next switch.
				if(Is2HandedWeapon(animator.GetInteger("Weapon"))){
					DoWeaponSwitch(0, weaponNumber, animator.GetInteger("Weapon"), 0, true);
					yield return new WaitForSeconds(0.5f);
					SetAnimator(0, -2, 0, 0, 0);
				}
				//Switching from 1handed weapon(s), go to Armed before next switch.
				else if(Is1HandedWeapon(animator.GetInteger("Weapon"))){
					if(IsRightWeapon(weaponNumber)){
						animator.SetBool("Shield", false);
					}
					DoWeaponSwitch(7, weaponNumber, 7, -1, true);
					yield return new WaitForSeconds(0.1f);
					if(weaponNumber == 7){
						animator.SetBool("Shield", false);
					}
					if(IsLeftWeapon(weaponNumber)){
						SetAnimator(7, 7, 0, -1, 0);
					}
					else{
						SetAnimator(7, 7, -1, 0, 0);
					}
				}
			}
			SetWeaponState(weaponTo);
			yield return null;
		}

		IEnumerator _DualUnSheath(int weaponNumber){
			//Debug.Log("_DualUnSheath: " + weaponNumber);
			//Switching to Unarmed.
			if(weaponNumber == 0){
				DoWeaponSwitch(-1, -1, -1, -1, false);
				yield return new WaitForSeconds(0.5f);
				SetAnimator(0, -1, 0, 0, 0);
			}
			//Switching to 1handed weapons.
			else if(Is1HandedWeapon(weaponNumber)){
				//Only if both hands are empty.
				if(leftWeapon == 0 && rightWeapon == 0){
					//Switching to Shield.
					if(weaponNumber == 7){
						animator.SetBool("Shield", true);
						DoWeaponSwitch(7, weaponNumber, animator.GetInteger("Weapon"), 1, false);
						yield return new WaitForSeconds(0.5f);
						SetAnimator(7, -2, 7, 0, 1);
						yield break;
					}
					DoWeaponSwitch(7, weaponNumber, animator.GetInteger("Weapon"), 3, false);
					//Set alternate weapon for Left.
					if(IsRightWeapon(weaponNumber)){
						rightWeapon = weaponNumber;
						animator.SetInteger("RightWeapon", weaponNumber);
						leftWeapon = weaponNumber - 1;
						animator.SetInteger("LeftWeapon", weaponNumber - 1);
					}
					//Set alternate weapon for Right.
					else if(IsLeftWeapon(weaponNumber)){
						leftWeapon = weaponNumber;
						animator.SetInteger("LeftWeapon", weaponNumber);
						rightWeapon = weaponNumber + 1;
						animator.SetInteger("RightWeapon", weaponNumber + 1);
					}
					yield return new WaitForSeconds(0.5f);
					SetAnimator(7, -2, -1, -1, 3);
				}
				//Only 1 1handed weapon.
				else{
					DoWeaponSwitch(7, weaponNumber, 7, 1, false);
					yield return new WaitForSeconds(0.5f);
					SetAnimator(7, -2, 0, 0, 1);
				}
			}
			else if(Is2HandedWeapon(weaponNumber)){
				DoWeaponSwitch(0, weaponNumber, weaponNumber, -1, false);
				yield return new WaitForSeconds(0.5f);
				SetAnimator(weaponNumber, -1, weaponNumber, 0, 0);
			}
			yield return null;
		}

		IEnumerator _DualSheath(int weaponNumber, int weaponTo){
			//Debug.Log("_DualSheath: " + weaponNumber + " -  Weapon To: " + weaponTo);
			//If switching to Relax from Unarmed.
			if(weaponNumber == 0 && weaponTo == -1){
				DoWeaponSwitch(-1, -1, 0, -1, true);
				yield return new WaitForSeconds(0.5f);
				SetAnimator(-1, -1, 0, 0, 0);
			}
			//Sheath 2handed weapon.
			else if(Is2HandedWeapon(weaponNumber)){
				//Switching to Relax.
				if(weaponTo == -1){
					DoWeaponSwitch(weaponTo, weaponNumber, weaponNumber, 1, true);
				}
				else{
					DoWeaponSwitch(0, weaponNumber, weaponNumber, 1, true);
				}
				yield return new WaitForSeconds(0.5f);
				SetAnimator(weaponTo, -1, 0, 0, 0);
			}
			//Sheath 1handed weapon(s).
			else if(Is1HandedWeapon(weaponNumber)){
				//If has 2 1handed weapons.
				if(leftWeapon != 0 && rightWeapon != 0){
					//If swtiching to 2handed weapon, goto Unarmed.
					if(Is2HandedWeapon(weaponTo)){
						DoWeaponSwitch(0, weaponNumber, 7, 3, true);
						yield return new WaitForSeconds(0.5f);
						StartCoroutine(_HideAllWeapons(false));
						SetAnimator(0, -2, 0, 0, 0);
					}
					//Switching to other 1handed weapons.
					else if(Is1HandedWeapon(weaponTo)){
						DoWeaponSwitch(7, weaponNumber, 7, 3, true);
						yield return new WaitForSeconds(0.5f);
						StartCoroutine(_HideAllWeapons(false));
						SetAnimator(7, -2, 0, 0, 0);
					}
					//Switching to Unarmed/Relax.
					else if(IsNoWeapon(weaponTo)){
						DoWeaponSwitch(weaponTo, weaponNumber, 7, 3, true);
						yield return new WaitForSeconds(0.5f);
						StartCoroutine(_HideAllWeapons(false));
						SetAnimator(weaponTo, -2, 0, 0, 0);
					}
				}
				//Has 1 1handed weapon.
				else{
					DoWeaponSwitch(7, weaponNumber, 7, 3, true);
					yield return new WaitForSeconds(0.5f);
					SetAnimator(weaponTo, -2, 0, 0, 0);
				}
			}
			yield return null;
		}

		IEnumerator _InstantWeaponSwitch(int weaponNumber){
			animator.SetInteger("Weapon", -2);
			yield return new WaitForEndOfFrame();
			animator.SetTrigger("InstantSwitchTrigger");
			if(Is1HandedWeapon(weaponNumber)){
				animator.SetInteger("Weapon", 7);
			}
			else{
				animator.SetInteger("Weapon", weaponNumber);
			}
			StartCoroutine(_HideAllWeapons(false));
			StartCoroutine(_WeaponVisibility(weaponNumber, true, false));
			SetWeaponState(weaponNumber);
		}

		void DoWeaponSwitch(int weaponSwitch, int weaponVisibility, int weaponNumber, int leftRight, bool sheath){
			//Go to Null state.
			animator.SetInteger("Weapon", -2);
			//Wait for animator.
			while(animator.isActiveAndEnabled && animator.GetInteger("Weapon") != -2){
			}
			//Set weaponSwitch if applicable.
			if(weaponSwitch != -2){
				animator.SetInteger("WeaponSwitch", weaponSwitch);
			}
			animator.SetInteger("Weapon", weaponNumber);
			//Set leftRight if applicable.
			if(leftRight != -1){
				animator.SetInteger("LeftRight", leftRight);
			}
			AnimatorDebug();
			//Set animator trigger.
			if(sheath){
				animator.SetTrigger("WeaponSheathTrigger");
				if(dualSwitch){
					StartCoroutine(_WeaponVisibility(weaponVisibility, false, true));

				}
				else{
					StartCoroutine(_WeaponVisibility(weaponVisibility, false, false));
				}
			}
			else{
				animator.SetTrigger("WeaponUnsheathTrigger");
				if(dualSwitch){
					StartCoroutine(_WeaponVisibility(weaponVisibility, true, true));
				}
				else{
					StartCoroutine(_WeaponVisibility(weaponVisibility, true, false));
				}
			}
		}


		/// <summary>
		/// Controller weapon switching.
		/// </summary>
		void SwitchWeaponTwoHand(int upDown){
			if(instantWeaponSwitch){
				StartCoroutine(_HideAllWeapons(false));
			}
			isSwitchingFinished = false;
			int weaponSwitch = (int)weapon;
			if(upDown == 0){
				weaponSwitch--;
				if(weaponSwitch < 1 || weaponSwitch == 18 || weaponSwitch == 20){
					StartCoroutine(_SwitchWeapon(6));
				}
				else{
					StartCoroutine(_SwitchWeapon(weaponSwitch));
				}
			}
			if(upDown == 1){
				weaponSwitch++;
				if(weaponSwitch > 6 && weaponSwitch < 18){
					StartCoroutine(_SwitchWeapon(1));
				}
				else{
					StartCoroutine(_SwitchWeapon(weaponSwitch));
				}
			}
		}

		/// <summary>
		/// Controller weapon switching.
		/// </summary>
		void SwitchWeaponLeftRight(int leftRight){
			if(instantWeaponSwitch){
				StartCoroutine(_HideAllWeapons(false));
			}
			int weaponSwitch = 0;
			isSwitchingFinished = false;
			if(leftRight == 0){
				weaponSwitch = leftWeapon;
				if(weaponSwitch < 16 && weaponSwitch != 0 && leftWeapon != 7){
					weaponSwitch += 2;
				}
				else{
					weaponSwitch = 8;
				}
			}
			if(leftRight == 1){
				weaponSwitch = rightWeapon;
				if(weaponSwitch < 17 && weaponSwitch != 0){
					weaponSwitch += 2;
				}
				else{
					weaponSwitch = 9;
				}
			}
			StartCoroutine(_SwitchWeapon(weaponSwitch));
		}

		
		public void WeaponSwitch(){
			if(!weaponSwitch){
				weaponSwitch = true;
			}
		}


		public void SetSheathLocation(int location){
			animator.SetInteger("SheathLocation", location);
		}

		#endregion

		#region Actions

		/* List of Actions for Animator triggers.
		 * 0: Sit
		 * 1: Laydown
		 * 2: Pickup
		 * 3: Activate
		 * 4: Drink1
		 * 5: Bow1
		 * 6: Bow2
		 * 7: No
		 * 8: Yes
		 * 9: Boost1
		 * */
		
		/// <summary>
		/// Keep character from doing actions.
		/// </summary>
		void LockAction(){
			canAction = false;
			isHeadlook = false;
		}
		
		/// <summary>
		/// Let character move and act again.
		/// </summary>
		void UnLock(bool movement, bool actions){
			StartCoroutine(_ResetIdleTimer());
			if(movement){
				canMove = true;
				animator.applyRootMotion = false;
			}
			if(actions){
				canAction = true;
				if(headLook){
					isHeadlook = true;
				}
			}
		}
	
		public void Sit(){
			canAction = false;
			isSitting = true;
			canMove = false;
			animator.SetInteger("Action", 0);
			animator.SetTrigger("ActionTrigger");
		}

		public void Sleep(){
			canAction = false;
			isSitting = true;
			canMove = false;
			animator.SetInteger("Action", 1);
			animator.SetTrigger("ActionTrigger");	
		}
		public void Stand(){
			isSitting = false;
			//Sitting.
			if(animator.GetInteger("Action") == 0){
				Lock(true, true, true, 0f, 1f);
			}
			//Laying down.
			else if(animator.GetInteger("Action") == 1){
				Lock(true, true, true, 0f, 2f);
			}
			animator.SetTrigger("ActionTrigger");
		}

		public void Pickup(){
			animator.SetInteger("Action", 2);
			animator.SetTrigger("ActionTrigger");
			StartCoroutine(_Lock(true, true, true, 0, 1.4f));
		}
		
		public void Activate(){
			animator.SetInteger("Action", 3);
			animator.SetTrigger("ActionTrigger");
			StartCoroutine(_Lock(true, true, true, 0, 1.2f));
		}

		public void Drink(){
			animator.SetInteger("Action", 4);
			animator.SetTrigger("ActionTrigger");
			StartCoroutine(_Lock(true, true, true, 0, 1f));
		}

		public void Bow(){
			int numberOfBows = Random.Range(1, 3);
			animator.SetInteger("Action", numberOfBows + 4);
			animator.SetTrigger("ActionTrigger");
			StartCoroutine(_Lock(true, true, true, 0, 3f));
		}

		public void No(){
			animator.SetInteger("Action", 7);
			animator.SetTrigger("ActionTrigger");
		}

		public void Yes(){
			animator.SetInteger("Action", 8);
			animator.SetTrigger("ActionTrigger");
		}

		public void Boost(){
			animator.SetInteger("Action", 9);
			animator.SetTrigger("ActionTrigger");
			StartCoroutine(_Lock(true, true, true, 0, 1f));
		}

		#endregion
		
		#region Misc

		//Placeholder functions for Animation events.
		public void Hit(){
		}

		public void Shoot(){
		}

		public void FootR(){
		}

		public void FootL(){
		}

		public void Land(){
		}

		/// <summary>
		/// Plays random idle animation.  Currently only Alert1 animation.
		/// </summary>
		void RandomIdle(){
			if(!isMoving && weapon != Weapon.RELAX && !isAiming){
				idleTimer += 0.01f;
				if(idleTimer > idleTrigger){
					animator.SetInteger("Action", 1);
					animator.SetTrigger("IdleTrigger");
					StartCoroutine(_ResetIdleTimer());
					//TODO set anim times.
					Lock(true, true, true, 0, 1.25f);
				}
			}
		}

		IEnumerator _ResetIdleTimer(){
			idleTrigger = Random.Range(5f, 15f);
			idleTimer = 0;
			yield return new WaitForSeconds(1f);
			animator.ResetTrigger("IdleTrigger");
		}

		IEnumerator _HideAllWeapons(bool timed){
			if(timed){
				while(!weaponSwitch && !instantWeaponSwitch){
					yield return null;
				}
			}
			if(twoHandAxe != null){
				twoHandAxe.SetActive(false);
			}
			if(twoHandBow != null){
				twoHandBow.SetActive(false);
			}
			if(twoHandCrossbow != null){
				twoHandCrossbow.SetActive(false);
			}
			if(twoHandSpear != null){
				twoHandSpear.SetActive(false);
			}
			if(twoHandSword != null){
				twoHandSword.SetActive(false);
			}
			if(twoHandClub != null){
				twoHandClub.SetActive(false);
			}
			if(staff != null){
				staff.SetActive(false);
			}
			if(swordL != null){
				swordL.SetActive(false);
			}
			if(swordR != null){
				swordR.SetActive(false);
			}
			if(maceL != null){
				maceL.SetActive(false);
			}
			if(maceR != null){
				maceR.SetActive(false);
			}
			if(daggerL != null){
				daggerL.SetActive(false);
			}
			if(daggerR != null){
				daggerR.SetActive(false);
			}
			if(itemL != null){
				itemL.SetActive(false);
			}
			if(itemR != null){
				itemR.SetActive(false);
			}
			if(shield != null){
				shield.SetActive(false);
			}
			if(pistolL != null){
				pistolL.SetActive(false);
			}
			if(pistolR != null){
				pistolR.SetActive(false);
			}
			if(rifle != null){
				rifle.SetActive(false);
			}
			if(spear != null){
				spear.SetActive(false);
			}
		}

		/// <summary>
		/// Lock character movement and/or action, on a deley for a set time.
		/// </summary>
		/// <param name="lockMovement">If set to <c>true</c> lock movement.</param>
		/// <param name="lockAction">If set to <c>true</c> lock action.</param>
		/// <param name="timed">If set to <c>true</c> timed.</param>
		/// <param name="delayTime">Delay time.</param>
		/// <param name="lockTime">Lock time.</param>
		public void Lock(bool lockMovement, bool lockAction, bool timed, float delayTime, float lockTime){
			StartCoroutine(_Lock(lockMovement, lockAction, timed, delayTime, lockTime));
		}
		
		//Timed -1 = infinite, 0 = no, 1 = yes.
		IEnumerator _Lock(bool lockMovement, bool lockAction, bool timed, float delayTime, float lockTime){
			yield return new WaitForSeconds(delayTime);
			if(lockMovement){
				LockMovement();
			}
			if(lockAction){
				LockAction();
			}
			if(timed){
				yield return new WaitForSeconds(lockTime);
				UnLock(lockMovement, lockAction);
			}
		}
	
		/// <summary>
		/// Sets the animator state.
		/// </summary>
		/// <param name="weapon">Weapon.</param>
		/// <param name="weaponSwitch">Weapon switch.</param>
		/// <param name="Lweapon">Lweapon.</param>
		/// <param name="Rweapon">Rweapon.</param>
		/// <param name="weaponSide">Weapon side.</param>
		void SetAnimator(int weapon, int weaponSwitch, int Lweapon, int Rweapon, int weaponSide){
			Debug.Log("SETANIMATOR: Weapon:" + weapon + " Weaponswitch:" + weaponSwitch + " Lweapon:" + Lweapon + " Rweapon:" + Rweapon + " Weaponside:" + weaponSide);
			//Set Weapon if applicable.
			if(weapon != -2){
				animator.SetInteger("Weapon", weapon);
			}
			//Set WeaponSwitch if applicable.
			if(weaponSwitch != -2){
				animator.SetInteger("WeaponSwitch", weaponSwitch);
			}
			//Set left weapon if applicable.
			if(Lweapon != -1){
				leftWeapon = Lweapon;
				animator.SetInteger("LeftWeapon", Lweapon);
				//Set Shield.
				if(Lweapon == 7){
					animator.SetBool("Shield", true);
				}
				else{
					animator.SetBool("Shield", false);
				}
			}
			//Set right weapon if applicable.
			if(Rweapon != -1){
				rightWeapon = Rweapon;
				animator.SetInteger("RightWeapon", Rweapon);
			}
			//Set weapon side if applicable.
			if(weaponSide != -1){
				animator.SetInteger("LeftRight", weaponSide);
			}
			SetWeaponState(weapon);
		}
		
		void SetWeaponState(int weaponNumber){
			if(weaponNumber == -1){
				weapon = Weapon.RELAX;
			}
			else if(weaponNumber == 0){
				weapon = Weapon.UNARMED;
			}
			else if(weaponNumber == 1){
				weapon = Weapon.TWOHANDSWORD;
			}
			else if(weaponNumber == 2){
				weapon = Weapon.TWOHANDSPEAR;
			}
			else if(weaponNumber == 3){
				weapon = Weapon.TWOHANDAXE;
			}
			else if(weaponNumber == 4){
				weapon = Weapon.TWOHANDBOW;
			}
			else if(weaponNumber == 5){
				weapon = Weapon.TWOHANDCROSSBOW;
			}
			else if(weaponNumber == 6){
				weapon = Weapon.STAFF;
			}
			else if(Is1HandedWeapon(weaponNumber)){
				weapon = Weapon.ARMED;
				if(animator.GetInteger("LeftWeapon") == 7){
					if(animator.GetInteger("RightWeapon") != 0){
						weapon = Weapon.ARMEDSHIELD;
					}
					else{
						weapon = Weapon.SHIELD;
					}
				}
			}
			else if(weaponNumber == 18){
				weapon = Weapon.RIFLE;
			}
			else if(weaponNumber == 20){
				weapon = Weapon.TWOHANDCLUB;
			}
		}
		
		public IEnumerator _WeaponVisibility(int weaponNumber, bool visible, bool dual){
			//Debug.Log("WeaponVisiblity: " + weaponNumber + "  Visible: " + visible);
			while(!weaponSwitch && !instantWeaponSwitch){
				yield return null;
			}
			weaponSwitch = false;
			if(weaponNumber == 1){
				twoHandSword.SetActive(visible);
			}
			else if(weaponNumber == 2){
				twoHandSpear.SetActive(visible);
			}
			else if(weaponNumber == 3){
				twoHandAxe.SetActive(visible);
			}
			else if(weaponNumber == 4){
				twoHandBow.SetActive(visible);
			}
			else if(weaponNumber == 5){
				twoHandCrossbow.SetActive(visible);
			}
			else if(weaponNumber == 6){
				staff.SetActive(visible);
			}
			else if(weaponNumber == 7){
				shield.SetActive(visible);
			}
			else if(weaponNumber == 8){
				swordL.SetActive(visible);
				if(dual){
					swordR.SetActive(visible);
				}
			}
			else if(weaponNumber == 9){
				swordR.SetActive(visible);
				if(dual){
					swordL.SetActive(visible);
				}
			}
			else if(weaponNumber == 10){
				maceL.SetActive(visible);
				if(dual){
					maceR.SetActive(visible);
				}
			}
			else if(weaponNumber == 11){
				maceR.SetActive(visible);
				if(dual){
					maceL.SetActive(visible);
				}
			}
			else if(weaponNumber == 12){
				daggerL.SetActive(visible);
				if(dual){
					daggerR.SetActive(visible);
				}
			}
			else if(weaponNumber == 13){
				daggerR.SetActive(visible);
				if(dual){
					daggerL.SetActive(visible);
				}
			}
			else if(weaponNumber == 14){
				itemL.SetActive(visible);
				if(dual){
					itemR.SetActive(visible);
				}
			}
			else if(weaponNumber == 15){
				itemR.SetActive(visible);
				if(dual){
					itemL.SetActive(visible);
				}
			}
			else if(weaponNumber == 16){
				pistolL.SetActive(visible);
				if(dual){
					pistolR.SetActive(visible);
				}
			}
			else if(weaponNumber == 17){
				pistolR.SetActive(visible);
				if(dual){
					pistolL.SetActive(visible);
				}
			}
			else if(weaponNumber == 18){
				rifle.SetActive(visible);
			}
			else if(weaponNumber == 19){
				spear.SetActive(visible);
			}
			else if(weaponNumber == 20){
				twoHandClub.SetActive(visible);
			}
			yield return null;
		}
		
		bool IsNoWeapon(int weaponNumber){
			if(weaponNumber < 1){
				return true;
			}
			else{
				return false;
			}
		}
		
		bool IsLeftWeapon(int weaponNumber){
			if((weaponNumber == 7 || weaponNumber == 8 || weaponNumber == 10 || weaponNumber == 12 || weaponNumber == 14 || weaponNumber == 16)){
				return true;
			}
			else{
				return false;
			}
		}
		
		bool IsRightWeapon(int weaponNumber){
			if((weaponNumber == 9 || weaponNumber == 11 || weaponNumber == 13 || weaponNumber == 15 || weaponNumber == 17 || weaponNumber == 19)){
				return true;
			}
			else{
				return false;
			}
		}
		
		bool Is2HandedWeapon(int weaponNumber){
			if((weaponNumber > 0 && weaponNumber < 7) || weaponNumber == 18 || weaponNumber == 20){
				return true;
			}
			else{
				return false;
			}
		}
		
		bool Is1HandedWeapon(int weaponNumber){
			if((weaponNumber > 6 && weaponNumber < 18) || weaponNumber == 19){
				return true;
			}
			else{
				return false;
			}
		}
		
		public IEnumerator _BlockBreak(){
			animator.applyRootMotion = true;
			animator.SetTrigger("BlockBreakTrigger");
			yield return new WaitForSeconds(1f);
			animator.applyRootMotion = false;
		}
		
		public void StartConversation(){
			currentConversation = Random.Range(1, numberOfConversationClips + 1);
			animator.SetInteger("Talking", Random.Range(1, currentConversation));
			StartCoroutine(_PlayConversationClip());
			canAction = false;
			canMove = false;
		}
		
		public void StopConversation(){
			currentConversation = 0;
			animator.SetInteger("Talking", 0);
			StopCoroutine("_PlayConversationClip");
		}
		
		/// <summary>
		/// Plays a random conversation animation.
		/// </summary>
		/// <returns>The conversation clip.</returns>
		IEnumerator _PlayConversationClip(){
			if(currentConversation != 0){
				yield return new WaitForSeconds(2f);
				animator.SetInteger("Talking", Random.Range(1, numberOfConversationClips + 1));
				StartCoroutine(_PlayConversationClip());
			}
			else{
				currentConversation = 0;
				animator.SetInteger("Talking", 0);
				canAction = true;
				canMove = true;
			}
		}
		
		void AnimatorDebug(){
			Debug.Log("ANIMATOR SETTINGS---------------------------");
			Debug.Log("Moving: " + animator.GetBool("Moving"));
			Debug.Log("Strafing: " + animator.GetBool("Strafing"));
			Debug.Log("Aiming: " + animator.GetBool("Aiming"));
			Debug.Log("Stunned: " + animator.GetBool("Stunned"));
			Debug.Log("Shield: " + animator.GetBool("Shield"));
			Debug.Log("Swimming: " + animator.GetBool("Swimming"));
			Debug.Log("Blocking: " + animator.GetBool("Blocking"));
			Debug.Log("Injured: " + animator.GetBool("Injured"));
			Debug.Log("Weapon: " + animator.GetInteger("Weapon"));
			Debug.Log("WeaponSwitch: " + animator.GetInteger("WeaponSwitch"));
			Debug.Log("LeftRight: " + animator.GetInteger("LeftRight"));
			Debug.Log("LeftWeapon: " + animator.GetInteger("LeftWeapon"));
			Debug.Log("RightWeapon: " + animator.GetInteger("RightWeapon"));
			Debug.Log("AttackSide: " + animator.GetInteger("AttackSide"));
			Debug.Log("Jumping: " + animator.GetInteger("Jumping"));
			Debug.Log("Action: " + animator.GetInteger("Action"));
			Debug.Log("SheathLocation: " + animator.GetInteger("SheathLocation"));
			Debug.Log("Talking: " + animator.GetInteger("Talking"));
			Debug.Log("Velocity X: " + animator.GetFloat("Velocity X"));
			Debug.Log("Velocity Z: " + animator.GetFloat("Velocity Z"));
			Debug.Log("AimHorizontal: " + animator.GetFloat("AimHorizontal"));
			Debug.Log("AimVertical: " + animator.GetFloat("AimVertical"));
			Debug.Log("BowPull: " + animator.GetFloat("BowPull"));
			Debug.Log("Charge: " + animator.GetFloat("Charge"));
		}
		
		#endregion
		
	}
}