using UnityEngine;
using System.Collections;
using RPGCharacterAnims;

public class GUIControls : MonoBehaviour{
	RPGCharacterController rpgCharacterController;
	[HideInInspector]
	public bool blockGui;
	float charge = 0f;
	bool useHips;
	bool useDual;
	bool useCrouch;
	bool useSprint;
	bool useInstant;
	bool hipsToggle;
	bool dualToggle;
	bool blockToggle;
	bool instantToggle;
	bool crouchToggle;
	bool sprintToggle;
	public bool useNavAgent;

	void Start(){
		rpgCharacterController = GetComponent<RPGCharacterController>();
	}

	public void EndClimbing(){
		rpgCharacterController.rpgCharacterState = RPGCharacterState.DEFAULT;
		rpgCharacterController.gravity = rpgCharacterController.gravityTemp;
		rpgCharacterController.rb.useGravity = true;
		rpgCharacterController.animator.applyRootMotion = false;
		rpgCharacterController.canMove = true;
		rpgCharacterController.isClimbing = false;
	}

	void OnGUI(){
		//Set blocking in controller.
		if(blockGui){
			rpgCharacterController.isBlocking = true;
		}
		else{
			rpgCharacterController.isBlocking = false;
		}
		if(!rpgCharacterController.isDead){
			//Use NavMesh.
			if(!blockGui){
				useNavAgent = GUI.Toggle(new Rect(500, 15, 100, 30), useNavAgent, "Use NavAgent");
				if(useNavAgent){
					rpgCharacterController.useMeshNav = true;
					rpgCharacterController.navMeshAgent.enabled = true;
				}
				else{
					rpgCharacterController.useMeshNav = false;
					rpgCharacterController.navMeshAgent.enabled = false;
				}
				useCrouch = GUI.Toggle(new Rect(510, 95, 100, 30), useCrouch, "Crouch");
				if(useCrouch){
					rpgCharacterController.crouch = true;
					rpgCharacterController.animator.SetBool("Crouch", true);
				}
				else{
					rpgCharacterController.crouch = false;
					rpgCharacterController.animator.SetBool("Crouch", false);
				}
				useSprint = GUI.Toggle(new Rect(510, 115, 100, 30), useSprint, "Sprint");
				if(useSprint){
					rpgCharacterController.isSprinting = true;
				}
				else{
					rpgCharacterController.isSprinting = false;
				}
			}
			//Charging.
			if(!blockGui){
				GUI.Button(new Rect(500, 55, 100, 30), "Charge");
				charge = GUI.HorizontalSlider(new Rect(500, 45, 100, 30), charge, 0.0F, 1f);
				rpgCharacterController.animator.SetFloat("Charge", charge);
			}
			//Stop Casting.
			if(rpgCharacterController.isCasting){
				if(GUI.Button(new Rect(25, 330, 100, 30), "Stop Casting")){
					rpgCharacterController.Cast(0, "attack");
				}
			}
			//Actions.
			if(rpgCharacterController.canAction){
				if(rpgCharacterController.isGrounded){
					//if character is not blocking.
					blockGui = GUI.Toggle(new Rect(25, 215, 100, 30), blockGui, "Block");
					if(blockGui){
						rpgCharacterController.isBlocking = true;
						rpgCharacterController.animator.SetBool("Blocking", true);
						if(blockToggle == false){
							rpgCharacterController.animator.SetTrigger("BlockTrigger");
							blockToggle = true;
						}
					}
					else{
						rpgCharacterController.isBlocking = false;
						rpgCharacterController.animator.SetBool("Blocking", false);
						blockToggle = false;
					}
					//Blocking.
					if(blockGui){
						if(GUI.Button(new Rect(30, 240, 100, 30), "Get Hit")){
							rpgCharacterController.GetHit();
						}
						if(GUI.Button(new Rect(30, 270, 100, 30), "Block Break")){
							StartCoroutine(rpgCharacterController._BlockBreak());
						}
					}
					//Not Blocking.
					else if(!rpgCharacterController.isBlocking){
						//Rolling.
						if(GUI.Button(new Rect(25, 15, 100, 30), "Roll Forward")){
							rpgCharacterController.targetDashDirection = transform.forward;
							StartCoroutine(rpgCharacterController._Roll(1));
						}
						if(GUI.Button(new Rect(130, 15, 100, 30), "Roll Backward")){
							rpgCharacterController.targetDashDirection = -transform.forward;
							StartCoroutine(rpgCharacterController._Roll(3));
						}
						if(GUI.Button(new Rect(25, 45, 100, 30), "Roll Left")){
							rpgCharacterController.targetDashDirection = -transform.right;
							StartCoroutine(rpgCharacterController._Roll(4));
						}
						if(GUI.Button(new Rect(130, 45, 100, 30), "Roll Right")){
							rpgCharacterController.targetDashDirection = transform.right;
							StartCoroutine(rpgCharacterController._Roll(2));
						}
						//Dodging.
						if(GUI.Button(new Rect(235, 15, 100, 30), "Dodge Left")){
							StartCoroutine(rpgCharacterController._Dodge(1));
						}
						if(GUI.Button(new Rect(235, 45, 100, 30), "Dodge Right")){
							StartCoroutine(rpgCharacterController._Dodge(2));
						}
						//Turning.
						if(rpgCharacterController.weapon != Weapon.RELAX || rpgCharacterController.weapon != Weapon.ARMED || rpgCharacterController.weapon != Weapon.ARMEDSHIELD){
							if(GUI.Button(new Rect(340, 15, 100, 30), "Turn Left")){
								StartCoroutine(rpgCharacterController._Turning(1));
							}
							if(GUI.Button(new Rect(340, 45, 100, 30), "Turn Right")){
								StartCoroutine(rpgCharacterController._Turning(2));
							}
						}
						//Boost - Victory
						if(rpgCharacterController.weapon != Weapon.RELAX){
							if(GUI.Button(new Rect(480, 650, 100, 30), "Boost")){
								rpgCharacterController.Boost();
							}
						}
						//ATTACK LEFT.
						if(rpgCharacterController.weapon == Weapon.SHIELD || rpgCharacterController.weapon != Weapon.ARMED || (rpgCharacterController.weapon == Weapon.ARMED && rpgCharacterController.leftWeapon != 0) && rpgCharacterController.leftWeapon != 7){
							if(rpgCharacterController.weapon != Weapon.RIFLE){
								if(GUI.Button(new Rect(25, 85, 100, 30), "Attack L")){
									rpgCharacterController.Attack(1);
								}
							}
						}
						//ATTACK RIGHT.
						if(rpgCharacterController.weapon == Weapon.RIFLE || rpgCharacterController.weapon != Weapon.ARMED || (rpgCharacterController.weapon == Weapon.ARMED && rpgCharacterController.animator.GetInteger("RightWeapon") != 0) || rpgCharacterController.weapon == Weapon.ARMEDSHIELD){
							if(rpgCharacterController.weapon != Weapon.SHIELD){
								if(GUI.Button(new Rect(130, 85, 100, 30), "Attack R")){
									rpgCharacterController.Attack(2);
								}
							}
						}
						//ATTACK DUAL.
						if(rpgCharacterController.leftWeapon > 7 && rpgCharacterController.animator.GetInteger("RightWeapon") > 7 && rpgCharacterController.leftWeapon != 14){
							if(rpgCharacterController.animator.GetInteger("RightWeapon") != 15){
								if((rpgCharacterController.leftWeapon != 16 && rpgCharacterController.animator.GetInteger("RightWeapon") != 17)){
									if(GUI.Button(new Rect(235, 85, 100, 30), "Attack Dual")){
										rpgCharacterController.Attack(3);
									}
								}
								else if((rpgCharacterController.leftWeapon == 16 && rpgCharacterController.animator.GetInteger("RightWeapon") == 17)){
									if(GUI.Button(new Rect(235, 85, 100, 30), "Attack Dual")){
										rpgCharacterController.Attack(3);
									}
								}
							}
						}
						//Special Attack.
						if(rpgCharacterController.weapon != Weapon.RELAX && rpgCharacterController.isGrounded){
							if(rpgCharacterController.weapon == Weapon.TWOHANDSWORD || rpgCharacterController.weapon == Weapon.TWOHANDAXE || rpgCharacterController.weapon == Weapon.TWOHANDCLUB || rpgCharacterController.weapon == Weapon.TWOHANDSPEAR || rpgCharacterController.weapon == Weapon.STAFF){
								if(GUI.Button(new Rect(235, 85, 100, 30), "Special Attack1")){
									rpgCharacterController.Special(1);
								}
							}
							else if(rpgCharacterController.weapon == Weapon.ARMED){
								if((rpgCharacterController.leftWeapon == 8 || rpgCharacterController.leftWeapon == 10) && (rpgCharacterController.rightWeapon == 9 || rpgCharacterController.rightWeapon == 11)){
									if(GUI.Button(new Rect(340, 85, 100, 30), "Special Attack1")){
										rpgCharacterController.Special(1);
									}
								}
							}
						}
						//Kicking.
						if(GUI.Button(new Rect(25, 115, 100, 30), "Left Kick")){
							rpgCharacterController.AttackKick(1);
						}
						if(GUI.Button(new Rect(25, 145, 100, 30), "Left Kick2")){
							rpgCharacterController.AttackKick(2);
						}
						if(GUI.Button(new Rect(130, 115, 100, 30), "Right Kick")){
							rpgCharacterController.AttackKick(3);
						}
						if(GUI.Button(new Rect(130, 145, 100, 30), "Right Kick2")){
							rpgCharacterController.AttackKick(4);
						}
						if(GUI.Button(new Rect(30, 240, 100, 30), "Get Hit")){
							rpgCharacterController.GetHit();
						}
						//Weapon Switching.
						if(!rpgCharacterController.isMoving){
							if(rpgCharacterController.weapon != Weapon.RELAX){
								if(GUI.Button(new Rect(1115, 265, 100, 30), "Relax")){
									StartCoroutine(rpgCharacterController._SwitchWeapon(-1));
								}
							}
							if(rpgCharacterController.weapon != Weapon.UNARMED){
								if(GUI.Button(new Rect(1115, 310, 100, 30), "Unarmed")){
									StartCoroutine(rpgCharacterController._SwitchWeapon(0));
									rpgCharacterController.canAction = true;
								}
							}
							if(rpgCharacterController.weapon != Weapon.TWOHANDSWORD){
								if(GUI.Button(new Rect(1115, 340, 100, 30), "2 Hand Sword")){
									StartCoroutine(rpgCharacterController._SwitchWeapon(1));
								}
							}
							if(rpgCharacterController.weapon != Weapon.TWOHANDCLUB){
								if(GUI.Button(new Rect(1000, 340, 100, 30), "2 Hand Club")){
									StartCoroutine(rpgCharacterController._SwitchWeapon(20));
								}
							}
							if(rpgCharacterController.weapon != Weapon.TWOHANDSPEAR){
								if(GUI.Button(new Rect(1115, 370, 100, 30), "2 Hand Spear")){
									StartCoroutine(rpgCharacterController._SwitchWeapon(2));
								}
							}
							if(rpgCharacterController.weapon != Weapon.TWOHANDAXE){
								if(GUI.Button(new Rect(1115, 400, 100, 30), "2 Hand Axe")){
									StartCoroutine(rpgCharacterController._SwitchWeapon(3));
								}
							}
							if(rpgCharacterController.weapon != Weapon.TWOHANDBOW){
								if(GUI.Button(new Rect(1115, 430, 100, 30), "2 Hand Bow")){
									StartCoroutine(rpgCharacterController._SwitchWeapon(4));
								}
							}
							if(rpgCharacterController.weapon != Weapon.TWOHANDCROSSBOW){
								if(GUI.Button(new Rect(1115, 460, 100, 30), "Crossbow")){
									StartCoroutine(rpgCharacterController._SwitchWeapon(5));
								}
							}
							if(rpgCharacterController.weapon != Weapon.RIFLE){
								if(GUI.Button(new Rect(1000, 460, 100, 30), "Rifle")){
									StartCoroutine(rpgCharacterController._SwitchWeapon(18));
								}
							}
							if(rpgCharacterController.weapon != Weapon.STAFF){
								if(GUI.Button(new Rect(1115, 490, 100, 30), "Staff")){
									StartCoroutine(rpgCharacterController._SwitchWeapon(6));
								}
							}
							if(rpgCharacterController.leftWeapon != 7){
								if(GUI.Button(new Rect(1115, 685, 100, 30), "Shield")){
									StartCoroutine(rpgCharacterController._SwitchWeapon(7));
								}
							}
							if(rpgCharacterController.leftWeapon != 8){
								if(GUI.Button(new Rect(1065, 530, 100, 30), "Left Sword")){
									StartCoroutine(rpgCharacterController._SwitchWeapon(8));
								}
							}
							if(rpgCharacterController.animator.GetInteger("RightWeapon") != 9){
								if(GUI.Button(new Rect(1165, 530, 100, 30), "Right Sword")){
									StartCoroutine(rpgCharacterController._SwitchWeapon(9));
								}
							}
							if(rpgCharacterController.leftWeapon != 10){
								if(GUI.Button(new Rect(1065, 560, 100, 30), "Left Mace")){
									StartCoroutine(rpgCharacterController._SwitchWeapon(10));
								}
							}
							if(rpgCharacterController.animator.GetInteger("RightWeapon") != 11){
								if(GUI.Button(new Rect(1165, 560, 100, 30), "Right Mace")){
									StartCoroutine(rpgCharacterController._SwitchWeapon(11));
								}
							}
							if(rpgCharacterController.leftWeapon != 12){
								if(GUI.Button(new Rect(1065, 590, 100, 30), "Left Dagger")){
									StartCoroutine(rpgCharacterController._SwitchWeapon(12));
								}
							}
							if(rpgCharacterController.rightWeapon != 13){
								if(GUI.Button(new Rect(1165, 590, 100, 30), "Right Dagger")){
									StartCoroutine(rpgCharacterController._SwitchWeapon(13));
								}
							}
							if(rpgCharacterController.leftWeapon != 14){
								if(GUI.Button(new Rect(1065, 620, 100, 30), "Left Item")){
									StartCoroutine(rpgCharacterController._SwitchWeapon(14));
								}
							}
							if(rpgCharacterController.rightWeapon != 15){
								if(GUI.Button(new Rect(1165, 620, 100, 30), "Right Item")){
									StartCoroutine(rpgCharacterController._SwitchWeapon(15));
								}
							}
							if(rpgCharacterController.leftWeapon != 16){
								if(GUI.Button(new Rect(1065, 650, 100, 30), "Left Pistol")){
									StartCoroutine(rpgCharacterController._SwitchWeapon(16));
								}
							}
							if(rpgCharacterController.rightWeapon != 17){
								if(GUI.Button(new Rect(1165, 650, 100, 30), "Right Pistol")){
									StartCoroutine(rpgCharacterController._SwitchWeapon(17));
								}
							}
							if(rpgCharacterController.animator.GetInteger("RightWeapon") != 19){
								if(GUI.Button(new Rect(1000, 370, 100, 30), "1 Hand Spear")){
									StartCoroutine(rpgCharacterController._SwitchWeapon(19));
								}
							}
							//Sheath/Unsheath Hips.
							useHips = GUI.Toggle(new Rect(1025, 260, 100, 30), useHips, "Hips");
							if(useHips){
								if(hipsToggle == false){
									rpgCharacterController.animator.SetInteger("SheathLocation", 1);
									hipsToggle = true;
								}
							}
							else{
								rpgCharacterController.animator.SetInteger("SheathLocation", 0);
								hipsToggle = false;
							}
							//Sheath/Unsheath Dual.
							useDual = GUI.Toggle(new Rect(1025, 285, 100, 30), useDual, "Dual");
							if(useDual){
								if(dualToggle == false){
									rpgCharacterController.dualSwitch = true;
									dualToggle = true;
								}
							}
							else{
								dualToggle = false;
								rpgCharacterController.dualSwitch = false;
							}
							//Instant weapon toggle.
							useInstant = GUI.Toggle(new Rect(1025, 310, 100, 30), useInstant, "Instant");
							if(useInstant){
								if(instantToggle == false){
									rpgCharacterController.instantWeaponSwitch = true;
									instantToggle = true;
								}
							}
							else{
								instantToggle = false;
								rpgCharacterController.instantWeaponSwitch = false;
							}
						}
					}
				}
				//Jump / Double Jump.
				if((rpgCharacterController.canJump || rpgCharacterController.canDoubleJump) && !blockGui && rpgCharacterController.canAction){
					if(rpgCharacterController.isGrounded){
						if(GUI.Button(new Rect(25, 175, 100, 30), "Jump")){
							if(rpgCharacterController.canJump){
								StartCoroutine(rpgCharacterController._Jump());
							}
						}
					}
					else if(rpgCharacterController.rpgCharacterState != RPGCharacterState.CLIMBING){
						if(GUI.Button(new Rect(25, 165, 100, 30), "Double Jump")){
							if(rpgCharacterController.canDoubleJump && !rpgCharacterController.isDoubleJumping){
								StartCoroutine(rpgCharacterController._Jump());
							}
						}
					}
				}
				//Death Pickup Activate.
				if(!blockGui && !rpgCharacterController.isBlocking && rpgCharacterController.isGrounded && rpgCharacterController.canAction){
					if(GUI.Button(new Rect(30, 270, 100, 30), "Death")){
						StartCoroutine(rpgCharacterController._Death());
					}
					if(rpgCharacterController.weapon != Weapon.ARMED){
						if(GUI.Button(new Rect(130, 175, 100, 30), "Pickup")){
							rpgCharacterController.Pickup();
						}
						if(GUI.Button(new Rect(235, 175, 100, 30), "Activate")){
							rpgCharacterController.Activate();
						}
					}
					else if(rpgCharacterController.weapon == Weapon.ARMED){
						if(rpgCharacterController.leftWeapon != 0 && rpgCharacterController.animator.GetInteger("RightWeapon") != 0){
						}
						else{
							if(GUI.Button(new Rect(130, 165, 100, 30), "Pickup")){
								rpgCharacterController.Pickup();
							}
							if(GUI.Button(new Rect(235, 165, 100, 30), "Activate")){
								rpgCharacterController.Activate();
							}
						}
					}
				}
				//Casting Armed and Staff.
				if((rpgCharacterController.weapon == Weapon.ARMED || rpgCharacterController.weapon == Weapon.STAFF || rpgCharacterController.weapon == Weapon.UNARMED) && !blockGui && rpgCharacterController.isGrounded){
					if(GUI.Button(new Rect(25, 330, 100, 30), "Cast Atk Left")){
						if(!rpgCharacterController.isCasting){
							rpgCharacterController.Cast(1, "attack");
						}
						else{
							rpgCharacterController.Cast(0, "attack");
						}
					}
					if(rpgCharacterController.weapon != Weapon.STAFF){
						if(GUI.Button(new Rect(130, 330, 100, 30), "Cast Atk Right")){
							if(!rpgCharacterController.isCasting){
								rpgCharacterController.Cast(2, "attack");
							}
							else{
								rpgCharacterController.Cast(0, "attack");
							}
						}
						if(rpgCharacterController.leftWeapon == 0 && rpgCharacterController.animator.GetInteger("RightWeapon") == 0){
							if(GUI.Button(new Rect(80, 365, 100, 30), "Cast Atk Dual")){
								if(!rpgCharacterController.isCasting){
									rpgCharacterController.Cast(3, "attack");
								}
								else{
									rpgCharacterController.Cast(0, "attack");
								}
							}
						}
					}
					if(GUI.Button(new Rect(25, 425, 100, 30), "Cast AOE")){
						if(!rpgCharacterController.isCasting){
							rpgCharacterController.Cast(4, "AOE");
						}
						else{
							rpgCharacterController.Cast(0, "AOE");
						}
					}
					if(GUI.Button(new Rect(25, 400, 100, 30), "Cast Buff")){
						if(!rpgCharacterController.isCasting){
							rpgCharacterController.Cast(4, "buff");
						}
						else{
							rpgCharacterController.Cast(0, "buff");
						}
					}
					if(GUI.Button(new Rect(25, 450, 100, 30), "Cast Summon")){
						if(!rpgCharacterController.isCasting){
							rpgCharacterController.Cast(4, "summon");
						}
						else{
							rpgCharacterController.Cast(0, "summon");
						}
					}
				}
			}
			//Idle Actions.
			//Sit = 0
			//Laydown = 1
			//Pickup = 2
			//Activate = 3
			//Drink = 4
			if(rpgCharacterController.weapon == Weapon.RELAX && !rpgCharacterController.isMoving){
				if(!rpgCharacterController.isSitting ){
					if(GUI.Button(new Rect(900, 680, 100, 30), "Sit")){
						rpgCharacterController.Sit();
					}
					if(GUI.Button(new Rect(795, 680, 100, 30), "Sleep")){
						rpgCharacterController.Sleep();
					}
					if(GUI.Button(new Rect(900, 650, 100, 30), "Drink")){
						rpgCharacterController.Drink();
					}
					if(GUI.Button(new Rect(795, 650, 100, 30), "Bow")){
						rpgCharacterController.Bow();
					}
					if(GUI.Button(new Rect(690, 680, 100, 30), "Start Talking")){
						rpgCharacterController.StartConversation();
					}
					if(GUI.Button(new Rect(585, 680, 100, 30), "Stop Talking")){
						rpgCharacterController.StopConversation();
					}
					if(GUI.Button(new Rect(690, 650, 100, 30), "Yes")){
						rpgCharacterController.Yes();
					}
					if(GUI.Button(new Rect(585, 650, 100, 30), "No")){
						rpgCharacterController.No();
					}
				}
				if(rpgCharacterController.isSitting){
					if(GUI.Button(new Rect(795, 680, 100, 30), "Stand")){
						rpgCharacterController.Stand();
					}
				}
			}
			//Special Attack End.
			if(rpgCharacterController.weapon != Weapon.RELAX && !rpgCharacterController.canAction){
				if(rpgCharacterController.weapon == Weapon.TWOHANDSWORD || rpgCharacterController.weapon == Weapon.TWOHANDAXE || rpgCharacterController.weapon == Weapon.TWOHANDCLUB || rpgCharacterController.weapon == Weapon.TWOHANDSPEAR || rpgCharacterController.weapon == Weapon.STAFF){
					if(GUI.Button(new Rect(235, 85, 100, 30), "End Special")){
						rpgCharacterController.Special(1);
					}
				}
			}
			//Climbing.
			if(!blockGui && !rpgCharacterController.isBlocking && rpgCharacterController.isGrounded && rpgCharacterController.rpgCharacterState != RPGCharacterState.CLIMBING && rpgCharacterController.isNearLadder){
				if(GUI.Button(new Rect(130, 200, 100, 30), "Climb")){
					StartCoroutine(rpgCharacterController._Climbing());
				}
			}
			/// <param name="Climb-Up">1</param>
			/// <param name="Climb-Down">2</param>
			/// <param name="Climb-Off-Top">3</param>
			/// <param name="Climb-Off-Bottom">4</param>
			/// <param name="Climb-On-Top">5</param>
			/// <param name="Climb-On-Bottom">6</param>
			if(rpgCharacterController.rpgCharacterState == RPGCharacterState.CLIMBING){
				if(GUI.Button(new Rect(130, 200, 100, 30), "Climb Off Top")){
					rpgCharacterController.animator.applyRootMotion = true;
					rpgCharacterController.animator.SetInteger("Action", 3);
					rpgCharacterController.animator.SetTrigger("ClimbLadderTrigger");
					Invoke("EndClimbing", 2.6f);
				}
				if(GUI.Button(new Rect(130, 235, 100, 30), "Climb Up")){
					rpgCharacterController.animator.applyRootMotion = true;
					rpgCharacterController.animator.SetInteger("Action", 1);
					rpgCharacterController.animator.SetTrigger("ClimbLadderTrigger");
				}
				if(GUI.Button(new Rect(130, 270, 100, 30), "Climb Down")){
					rpgCharacterController.animator.applyRootMotion = true;
					rpgCharacterController.animator.SetInteger("Action", 2);
					rpgCharacterController.animator.SetTrigger("ClimbLadderTrigger");
				}
			}
		}
		//Revive.
		if(rpgCharacterController.isDead){
			if(GUI.Button(new Rect(30, 270, 100, 30), "Revive")){
				StartCoroutine(rpgCharacterController._Revive());
			}
		}
	}
}