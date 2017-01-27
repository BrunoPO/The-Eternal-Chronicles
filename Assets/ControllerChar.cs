﻿using System;
using UnityEngine;
using UnityStandardAssets._2D;

public class ControllerChar : MonoBehaviour {
	
	private PlayerAction Vertical;

	[SerializeField] public bool m_AirControl = false,Flying=false;
	[SerializeField] private int Timetowait=0;
	[SerializeField] public float m_MaxSpeed = 10f,m_JumpHeight,life;//Alter
	public bool Comment = false,animToRight,m_FacingRight = true,itsItem=false;
	private bool jumpIni=false,canMoveX = true,minion;
	private float gravityScale; //lastMove = 0,
	private int intecSprint = 2;
	private Animator m_Anim; 
	private Rigidbody2D m_Rigidbody2D;
	private LayerMask m_WhatIsGround,m_WhatIsPlat;
	private Transform m_GroundCheck,atack_Point_0,atack_Point_1;
	private Vector3 ini,fim;
	private float damage,k_GroundedRadius;
	private bool damaged=false;
	private int ID_Target,Efective,Efective_Aux,count_Without_move;//,Anim_Hash

	private Collider2D  on_Plat,platatual;

	private LayerMask myEnemy_layer;
	private float sprint_velo = 0;
	public bool[] comboTree=new bool[14];
	public bool[] Habilidades=new bool[4];//Dash,Pulo duplo,ataques em defesa
	public float[] damageTree=new float[14];
	[NonSerialized] public Collider2D m_lastPlat,on_Ground,m_Grounded;
	[NonSerialized] public int waitTime,altArvCombo=0,atk;
	public bool noAtacking=true,Gdamaged=false,defense,jump,sprint,canMoveY = true,PlusJump=false;
	[NonSerialized] public float move,m_JumpForce;
	[NonSerialized] public CircleCollider2D GroundCols;

	private void Start(){
		if (Flying) {
			Vertical = new VerticalFlyAction (this.gameObject);
		}else{
			Vertical = new JumpAction (this.gameObject);
		}
		if(Flying) intecSprint = 5 ;
		if (gameObject.GetComponents<AI> ().Length != 0)
			minion = GetComponent<AI> ().minion;
		else {
			minion = true;
			itsItem = true;
		}

		m_Anim = GetComponent<Animator>();
		m_Rigidbody2D = GetComponent<Rigidbody2D>();

		if (!itsItem) {
			m_GroundCheck = transform.Find ("GroundCheck");
			atack_Point_0 = transform.Find ("Point_Atack").Find ("0");
			atack_Point_1 = atack_Point_0.Find ("1");
			gravityScale = m_Rigidbody2D.gravityScale;

			m_WhatIsGround = Global.WhatIsGround | LayerMask.GetMask ("InteractiveItens");
			m_WhatIsPlat = Global.WhatIsPlat;
			GroundCols = GetComponent<CircleCollider2D> ();
			k_GroundedRadius = GroundCols.radius;

			m_JumpForce = (float) Mathf.Sqrt (Mathf.Abs( 2.075f * m_JumpHeight * Physics2D.gravity.y * m_Rigidbody2D.gravityScale))*m_Rigidbody2D.mass;

			if (Flying) {
				m_Rigidbody2D.gravityScale = 0f;
				GroundCols.isTrigger = true;
			}

			if (LayerMask.LayerToName (gameObject.layer) == "Character") {
				myEnemy_layer = LayerMask.GetMask ("Enemy") | LayerMask.GetMask ("InteractiveItens");
			}else 
				myEnemy_layer = LayerMask.GetMask("Character");

		}



	}
	//Maybe use FixedUpdate(faster) have a precision between execution
	private void Update(){	
		if ( transform.position.y <= -10)
			GameObject.Find("GM").GetComponent<Global>().Died (gameObject);
		else if (life <= 0)
			m_Anim.SetBool ("Death", true);

		Gdamaged = (damaged)?damaged:Gdamaged;//guarda se houve dano sempre tentando guardar o true;
		m_Anim.SetBool ("Damaged", damaged);
		damaged = false;


		if (itsItem) {
			Gdamaged = false;
			return;
		}

		if (jumpIni) {
			m_Anim.SetBool ("Jump_Bot", false);
			float pocertanim=m_Anim.GetCurrentAnimatorStateInfo (0).IsName ("Jumping 0")?m_Anim.GetCurrentAnimatorClipInfo (0).Length * m_Anim.GetCurrentAnimatorStateInfo (0).normalizedTime:0;
			if (pocertanim > 0.2) {
				m_Rigidbody2D.gravityScale = gravityScale;
				jumpIni = false;
				m_Anim.SetBool ("JumpIni", jumpIni);
				canMoveX = true;
				defense = false;
				jump = true;
				Vertical.perform (jump,defense);
			}
		}

		if (!noAtacking || Flying) 
			Calc_Efect ();

		if (!Flying) {
			collidindoComTerreno ();
		}

	}
	private void collidindoComTerreno(){
		m_Grounded = null;on_Ground = null;on_Plat=null;
		//abaixo é verificado se colidindo enquando intagivel se não torna tangivel(só é tangivel se não estiver batendo em nada)
		on_Ground = Physics2D.OverlapCircle (new Vector2 (m_GroundCheck.position.x, m_GroundCheck.position.y), k_GroundedRadius, m_WhatIsGround);
		on_Plat = Physics2D.OverlapCircle (new Vector2 (m_GroundCheck.position.x, m_GroundCheck.position.y), k_GroundedRadius, m_WhatIsPlat);
		m_Grounded = (on_Ground)?on_Ground:on_Plat;
		if (m_Grounded) m_lastPlat = m_Grounded;
		//print ("está no chão?" + m_Grounded);
		if (m_Rigidbody2D.velocity.y <= 0) {//estiver apenas cainda verificar se encostou no chão
			if (m_Grounded) {
				if (!GroundCols.isTrigger) {
					m_Rigidbody2D.velocity = new Vector2 (m_Rigidbody2D.velocity.x, 0);
					//m_Rigidbody2D.Sleep ();
				}
				//m_lastPlat = Physics2D.OverlapCircle (new Vector2 (m_GroundCheck.position.x, m_GroundCheck.position.y), k_GroundedRadius, m_WhatIsPlat);
				if (Habilidades [1]) {
					PlusJump = true;
				}
			} else if (GroundCols.isTrigger && m_Rigidbody2D.velocity.y < 3) {
				//m_WhatIsGround == UnityEngine.LayerMask.NameToLayer ("Platform")
				GroundCols.isTrigger = false;
			}
		}

		m_Anim.SetFloat ("vSpeed", m_Rigidbody2D.velocity.y);
		m_Anim.SetBool ("Ground", m_Grounded);
		if (count_Without_move >= 20) {
			m_Rigidbody2D.velocity = new Vector2 (0, m_Rigidbody2D.velocity.y);
			if (!minion) {
				m_Anim.SetBool ("Defense", false);
				m_Anim.SetBool ("Jump_Bot", false);
			}
		} else
			count_Without_move++;
		m_Anim.SetFloat ("Speed", Mathf.Abs (m_Rigidbody2D.velocity.x));
	}
	public void Calc_Efect(){
		ID_Target = transform.GetComponent<AI> ().ID_Target;
		//Anim_Hash = m_Anim.GetCurrentAnimatorStateInfo (0).shortNameHash;
		damage = 0;
		RaycastHit2D[] col1 = Physics2D.LinecastAll (transform.position, atack_Point_0.position, myEnemy_layer);
		RaycastHit2D[] col2 = Physics2D.LinecastAll (atack_Point_0.position, atack_Point_1.position, myEnemy_layer);
		int damage_posi = 0;

		if (!minion) {
			if (m_Anim.GetInteger ("Atk_3") != 0) {
				damage_posi = ((2*(2*(m_Anim.GetInteger ("Atk_1"))+m_Anim.GetInteger ("Atk_2")))+m_Anim.GetInteger ("Atk_3"))-1;
			} else if (m_Anim.GetInteger ("Atk_2") != 0) {
				damage_posi = (2 * (m_Anim.GetInteger ("Atk_1")) + m_Anim.GetInteger ("Atk_2")) - 1;
			} else {
				damage_posi = m_Anim.GetInteger ("Atk_1");
			}
		} else {
			damage_posi = damage_posi!=0?m_Anim.GetInteger ("Atk_1")-1:0;
		}



		if (Flying) {
			damage_posi = 0;
		}
		if(Comment) print ("Damage Posi:"+ damage_posi);
		if(Comment) print ("Damage inflict:"+ damageTree[damage_posi]);
		if (damage_posi >= 0) {
			damage = damageTree [damage_posi];
		}


		//damage = damageTree [galho - 1];
		Efective_Aux = 0;
		Efective=0;
		for (int i = (col1.Length > col2.Length) ? col1.Length : col2.Length; i >= 0; --i) {//for que verifica os 2 colisores e chamam funcao de dano em cada personagem
			if (i < col1.Length && col1 [i].transform.name != this.name)
				Efective_Aux = col1 [i].transform.GetComponent<CharController> ().Damaged (damage, ID_Target);
			Efective = (Efective_Aux > Efective) ? Efective_Aux : Efective;
			if (i < col2.Length && col2 [i].transform.name != this.name)
				Efective_Aux = col2 [i].transform.GetComponent<CharController> ().Damaged (damage, ID_Target);
			Efective = (Efective_Aux > Efective) ? Efective_Aux : Efective;
		}
		if(Efective != 0 && !minion && transform.GetComponent<AI> ().enabled)
			transform.GetComponent<AI> ().Golpe_Detec(Efective+(Gdamaged ? (m_Anim.GetBool ("Defense") ? 5 : 0) : 5));
		Debug.DrawLine (transform.position, atack_Point_0.position);
		Debug.DrawLine (atack_Point_0.position, atack_Point_1.position);
	}

	public int Damaged(float dano,int ID){//aqui é setado que foi recebido dano e possui um retorno 2 se acertou o inimigo certo 1 se ele estiver em defesa 0 se não formos o alvo.

		if (itsItem || minion || !m_Anim.GetCurrentAnimatorStateInfo (0).IsName("Defense")) {
			life -= dano;
			damaged = true;
			return (this.gameObject.GetInstanceID()==ID)?2:0;
		}else if(m_Anim.GetCurrentAnimatorStateInfo (0).IsName("Defense") && dano > 2) {
			life -= dano;
			damaged = true;
		} else {
			life -= dano/5;
		}

		//EfectGolpe [0] = 6;
		//EfectGolpe [2] = 6;
		return (this.gameObject.GetInstanceID()==ID)?((damaged)?2:1):0;
	}

	public void Move(float move, bool defense, bool jump,int atk,bool sprint){//procedimento acessado externamente que altera o corpo de acordo com as vars
		//print("Last Move"+lastMove);
		if (m_Anim.GetBool ("Death"))
			return;
		/*if (move != 0)
			lastMove = move;*/
		count_Without_move = 0;
		if (!Habilidades [0]) {
			sprint = false;
		}
		//if(Comment) print(move+" "+defense+" "+jump+" "+atk);
		//Anim_Hash = m_Anim.GetCurrentAnimatorStateInfo (0).shortNameHash;//guarda hash em int da animação atual para possiveis verificações futuras.
		if (waitTime < Timetowait) {//tempo de pausa entre ataques(evitar combos infinitos)
			atk = 0;
			waitTime++;
		}

		if (m_Anim.GetInteger ("Atk_1") != 0) {//caso iniciar ataque a var é avisada
			if (Flying) 
				sprint = true;
			noAtacking = false;
		}	

		if (noAtacking) {//zera vars que serão usadas futuramente
			Gdamaged = false;
			if (!minion) {
				m_Anim.SetInteger ("Atk_2", 0);
				m_Anim.SetInteger ("Atk_3", 0);
			}
		} else {
			//var inferior encontra em quantos porcerto de execução a animação atual está
			float pocertanim=!m_Anim.GetCurrentAnimatorStateInfo (0).IsName ("Idle")?m_Anim.GetCurrentAnimatorClipInfo (0).Length * m_Anim.GetCurrentAnimatorStateInfo (0).normalizedTime:0;
			pocertanim = (pocertanim >= 1) ? pocertanim - Mathf.Floor(pocertanim): pocertanim;//corrigi a var pode vir de 0 á 1 ou de 1 á 2
			if (pocertanim > 0.5f && altArvCombo==1) {//caso no meio da primeira animação de ataque então guarde valor do botão de ataque na var atk_2 
				if (atk != 0 && comboTree[(2*(m_Anim.GetInteger ("Atk_1"))+atk)-1]) //verifica se possui a continuação caso sim salve
					m_Anim.SetInteger ("Atk_2", atk);
				atk = 0;
			} else if (pocertanim > 0.5f && altArvCombo==2) {//se no meio do segundo ataque...mesma logica
				if (atk != 0 && comboTree[(2*(m_Anim.GetInteger ("Atk_1")+m_Anim.GetInteger ("Atk_2"))+atk)-1]) 
					m_Anim.SetInteger ("Atk_3", atk);
				atk = 0;
			}

		}
		if(atk!=0 && m_Anim.GetInteger ("Atk_1")==0 && comboTree[atk-1]){
			m_Anim.SetInteger ("Atk_1", atk);//seta ataque
		}
		if(!minion)
			m_Anim.SetBool ("Defense", defense);//seta defesa

		if ((m_Grounded || m_AirControl)) {//verifica se ele está no chão ou pode se mover no ar.
			move = (defense||atk != 0 ? move/1000: move);//caso esteja em modo de defesa ou atacando ele não pode se mover horinzontalmente
			HorizontalMove(move, sprint);
		}

		if (!minion){
			if (jump && defense) {
				Vertical.perform (jump,defense);
				//m_Anim.SetBool ("Jump_Bot", jump);
			} else if (!jumpIni && jump && !defense && (m_Grounded || (PlusJump && m_Rigidbody2D.velocity.y < 0))) {
				m_Rigidbody2D.velocity = new Vector2 (0, -0.5f);
				m_Rigidbody2D.gravityScale = 0;
				jumpIni = true;
				m_Anim.SetBool ("JumpIni", jumpIni);
				m_Anim.SetBool ("Jump_Bot", jump);//envia pulo para animação
				canMoveX=false;			
			}
		} else {
				Vertical.perform (jump,defense);
		}




	}
	public void VerticalMove(bool defense, bool jump){
		//if (!m_Grounded && m_Rigidbody2D.velocity.y >= 4) jump = false;
		if (Flying) {
			if (!jump && !defense) {
				m_Rigidbody2D.velocity = new Vector3 (m_Rigidbody2D.velocity.x, 0, 0);
			} else if (defense) {
				m_Rigidbody2D.velocity = new Vector3 (m_Rigidbody2D.velocity.x, -3f, 0);
			} else if (jump) {
				m_Rigidbody2D.velocity = new Vector3 (m_Rigidbody2D.velocity.x, 3f, 0);
			}
		} else if(canMoveY){
			if (m_Grounded && jump && defense) {//verifica se no chao,defesa e pulando (desativa trigger para cair)
				if (!on_Ground) {
					GroundCols.isTrigger = true;
					PlusJump = false;
				} else {
					jump = false;
					defense = false;
				}
			} else if ((m_Grounded || PlusJump) && jump) {//verifica se no chao, e pulando "envia" movimento para o corpo e desativa colisão
				if (!m_Grounded)
					PlusJump = false;
				GroundCols.isTrigger = true; 
				m_Rigidbody2D.AddForce(new Vector2(0,-1*m_Rigidbody2D.velocity.y+(m_JumpForce)),ForceMode2D.Impulse);//força adaptada a gravidade negativa
			} 
		}

	}
	private void HorizontalMove(float move,bool sprint){
		if (canMoveX) {
			float veloX = 0;
			if (sprint && sprint_velo == 0)
				sprint_velo = intecSprint * m_MaxSpeed;
			else if (sprint_velo > 0)
				sprint_velo = sprint_velo - 0.1f;
			else
				sprint_velo = 0;

			veloX = (move * m_MaxSpeed) + ((m_FacingRight) ? sprint_velo : -sprint_velo);	

			//float veloY = (m_Rigidbody2D.velocity.y > -8) ? m_Rigidbody2D.velocity.y : -9;//limita velocidade minina no eixo Y

			m_Rigidbody2D.velocity = new Vector2 (veloX, m_Rigidbody2D.velocity.y);
			m_Anim.SetFloat ("Speed", Mathf.Abs (m_Rigidbody2D.velocity.x));

			//if (Comment) print (animToRight);
			if (animToRight == true) {
				if (move < 0 && m_FacingRight)
					Flip ();
				else if (move > 0 && !m_FacingRight)
					Flip ();
			} else if (animToRight != true) {
				if (move < 0 && !m_FacingRight)
					Flip ();
				else if (move > 0 && m_FacingRight)
					Flip ();
			}
		}
	}


	//girar personagem no ambiente(sem suavização),as sprites são unidirecionais
	public void Flip(){//vira a escala (muda de lado) e altera a var responsavel
		//if(Comment) print("Virou de Lado");
		m_FacingRight = !m_FacingRight;
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}
}