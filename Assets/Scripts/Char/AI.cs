using System;
using UnityEngine;
using System.Collections;
using UnityStandardAssets._2D;

public class AI : MonoBehaviour {
	[SerializeField] private bool enemy=true;
	public bool minion = false,Comment=false;

	private bool iniAI=false;
	private int waitforPath = 0;
	private float LimitX, LimitY, Dist;
	private int VeloX,veloXOld,CountBFPath=0;
	private bool jump, defense;
	private float[][] Path;
	private int posiPath = 0,altArvCombo;
	private Vector2 Boxsize;
	private bool boolX = false,boolY= false;
	private float[] lastTarget= new float[2],target = new float[2];
	private float difX,difY,m_JumpHeight,m_JumpDist;
	private Vector3 pointAdap;
	private bool animToRight = false;
	private int timedefence = 0;
	private Rigidbody2D m_Rigidbody2D;
	public GameObject target_GO;
	private bool finded=false;
	[NonSerialized] public int Atk=0,round=0,ID_Target;
	[NonSerialized] public static int[] Golpe=new int[3],EfectGolpe=new int[3];
	[NonSerialized] public static int[] n_Golpe=new int[3],n_EfectGolpe=new int[3];
	private bool reloadTarget = false;

	private Animator m_Anim;
	private bool noAtacking,zerado=false,Flying;
	private Collider2D lastPlat;
	void Start() {
		m_Rigidbody2D = GetComponent<Rigidbody2D>(); 
		animToRight = GetComponent<CharController> ().animToRight;
		Flying = GetComponent<CharController> ().Flying;

		waitforPath = GameObject.Find ("GM").GetComponent<Global> ().waitForPath;
		waitforPath = UnityEngine.Random.Range(waitforPath,2*waitforPath);
		m_Anim = GetComponent<Animator>();
		m_JumpHeight = GetComponent<CharController> ().m_JumpHeight;
		m_JumpDist = GetComponent<CharController> ().m_MaxSpeed;
		Dist = (this.GetComponent<CircleCollider2D> ().offset.y -  this.GetComponent<CircleCollider2D> ().radius) * this.GetComponent<Transform> ().localScale.y;
		altArvCombo = transform.GetComponent<CharController> ().altArvCombo;
		Boxsize.x = GetComponent<BoxCollider2D> ().size.x * transform.localScale.x;
		Boxsize.y = GetComponent<BoxCollider2D> ().size.y * transform.localScale.y;
		ini ();
	}
	public void ini(){
		GetComponent<AI> ().enabled = (GetComponent<Transform> ().name != Global.target.name);
		iniAI = false;
		//Possibilidade de não ser utilizado essa var LimitX ou LimitY(consumo de men por duplicação)
		LimitX = (Flying)?2:1 * Global.LimitX;
		LimitY = (Flying)?0.2f:1 * Global.LimitY;
		n_Golpe [0] = 1;
		n_EfectGolpe [0] = 6;
		n_Golpe [1] = 1;
		n_EfectGolpe [1] = 6;
		n_Golpe [2] = 1;
		n_EfectGolpe [2] = 6;
		Path = new float[4][];
		//if(Comment) print (Path.Length);
		Path [0] = new float[2];
		//if(Comment) print (Path.Length + " "+ Path[0].Length);
		Path [0] [0] = float.NaN;
		posiPath = Path.Length+1;
		boolX=false;
		boolY=false;
	}

	void Update () {//É calculado a distancia entre o personagem atual e seu alvo
		//print ("Bools" + boolX + boolY);

		lastPlat = GetComponent<CharController>().m_lastPlat; 
		if(target_GO != null) Debug.DrawLine (gameObject.transform.position,new Vector2(target [0],target [1]));
		if(null != Path [0] && this.name == "Char2")//Debug
			for (int i = 0; null != Path [i+1] && i<Path.Length-2; i++)
				Debug.DrawLine (new Vector2(Path[i][0],Path[i][1]),new Vector2(Path[i+1][0],Path[i+1][1]));

		if (!iniAI) {
			if (Vector2.Distance (Global.target.transform.position, transform.position) < 2)
				iniAI = true;
			return;
		}

		chooseTarget();

		if (target_GO == null )
			return;
		
		noAtacking = transform.GetComponent<CharController> ().noAtacking;
		difX = (transform.position.x > target_GO.transform.position.x) ? transform.position.x - target_GO.transform.position.x : target_GO.transform.position.x - transform.position.x;
		difY = (transform.position.y > target_GO.transform.position.y) ? transform.position.y - target_GO.transform.position.y : target_GO.transform.position.y - transform.position.y;

		if (((!enemy && target_GO.name != Global.target.name) || enemy) && difX < m_JumpDist / 8 && difY < m_JumpDist / 8 ) {
			atack();
		}
		//vá para o proximo ponto
		if (boolX && boolY && (target [1] != target_GO.transform.position.y + Dist && target [0] != target_GO.transform.position.x)) {
				posiPath++;
				lastTarget = target;
				boolX = false;
				boolY = false;
		}else {
			chooseYourPath ();
		}

		if(  defense || jump || (Atk+VeloX) != 0 )
			GetComponent<CharController> ().Move (VeloX, defense, jump,Atk,false);
		Atk = 0;

	}

	void chooseTarget(){
		if ( target_GO == null || !target_GO.activeSelf || (!enemy && (target_GO.name == "Char1" || target_GO.name == "Char2"))) {
			target_GO = GameObject.Find ("GM").GetComponent<Global> ().chooseTarget (transform.gameObject);
			if (target_GO == null) {
				//if(Comment) print (this.gameObject.name);
				if (this.gameObject.name == "Char1")
					target_GO = GameObject.Find ("Char2");
				else if (this.gameObject.name == "Char2")
					target_GO = GameObject.Find ("Char1");
			}
			if (target_GO != null)
				ID_Target = target_GO.GetInstanceID ();
		}
	}

	public void atack(){
		//if(Comment) print ("Golpe " + Golpe [0] + Golpe [1] + Golpe [2] + " Efetividade " + EfectGolpe [0] + EfectGolpe [1] + EfectGolpe [2]);

		if (noAtacking && !zerado) {
			bool m_FacingRight = GetComponent<CharController> ().m_FacingRight;
			bool direita = (transform.position.x > target_GO.transform.position.x);
			if (animToRight == true) {
				if (!direita && !m_FacingRight)
					GetComponent<CharController> ().Flip ();
				else if (direita && m_FacingRight)
					GetComponent<CharController> ().Flip ();
			} else if (animToRight != true) {
				//print ("Tentou Girar"+direita+" "+m_FacingRight);
				if (!direita && m_FacingRight)
					GetComponent<CharController> ().Flip ();
				else if (direita && !m_FacingRight)
					GetComponent<CharController> ().Flip ();
			}
			for(int i=0;i<3;i++){
				Golpe[i] = n_Golpe[i];
				EfectGolpe[i] = n_EfectGolpe[i];
				n_Golpe [i] = 0;
				n_EfectGolpe [i] = 0;
			}
			zerado = true;

		}
		if (noAtacking) {//Se não estiver atacando poderá se tentar locomoverse 
			boolX = false;
			boolY = false;
			round = 0;
		} else if (altArvCombo == 1 && GetComponent<Animator> ().GetInteger ("Atk_2") == 0) { 
			round = 1;
			//zerado = false;
		} else if (altArvCombo == 2 && GetComponent<Animator> ().GetInteger ("Atk_3") == 0) {
			round = 2;
			//zerado = false;
		} else if (!noAtacking) {//se estiver atacando então inicie round 0
			round = 0;
			zerado = false;
		}

		if (minion) {
			Atk = 1;
		} else if (EfectGolpe [round] > 5 && timedefence<60) {
			if(Comment) print (timedefence+" "+Golpe [round]);
			if (Golpe [round] == 0) {//quando sai do modo de defesa
				Atk = 1;
				timedefence++;
			}else if (EfectGolpe [round] == 7) {
				Atk = Golpe [round];
			} else {
				Atk = (Golpe [round] == 1) ? 2 : 1;
			}
		} else {
			if (EfectGolpe [round] == 0 || timedefence>60){
				Atk = 1;
				timedefence=0;
			}else{
				defense = true;
				timedefence++;
			}
		}
	}

	void moviment(){
		float posiLimit,dif=0;
		if (!float.IsNaN (target [0])) {//Ande em direção ao alvo
			//print("Há um alvo");
			if(Flying && VeloX != 0) veloXOld = VeloX;
			if (this.transform.position.x < target [0] - LimitX) {
				VeloX = 1;
			} else if (this.transform.position.x > target [0] + LimitX) {
				VeloX = -1;
			} else {
				VeloX = 0;
				boolX = true;
			}
			if(Flying && VeloX == 0) VeloX = veloXOld;


			//Suba e/ou desça em direção ao alvo
			if (this.GetComponent<Animator> ().GetFloat ("vSpeed") == 0) {
				if (this.transform.position.y + Dist > target [1] + LimitY) {
					jump = (Flying) ? false : true;
					defense = true;
				} else if (this.transform.position.y + Dist < target [1] - ((Flying) ? 0f : LimitY)) {
					jump = true;
					defense = false;
				} else {
					jump = false;
					defense = false;
					if (!boolY)
						boolY = true;
				}

				if (!boolY && !Flying) {
					//verificando viabilidade de acesso
					//Collider2D lastPlat = GetComponent<CharController>().m_lastPlat; 
					float distXPlats = (target [0] > transform.position.x) ? target [0] - transform.position.x : transform.position.x - target [0];
					//float distYPlats = (target [1] > transform.position.y) ? target [1] - transform.position.y : transform.position.y - target [1];
					float percent = (distXPlats / m_JumpDist) * m_JumpHeight;
					if (jump && defense && percent > 0.4f) {
						defense = false;
					}
					//if(Comment) print ("Defesa?" + defense);
				}

			} else {
				jump = false;
				defense = false;
			}
			if (!Flying && VeloX != 0 && !(jump || !m_Anim.GetBool("Ground"))) {
				if(Comment) print ("Trying to move");
				if (lastPlat != null) {
					if (VeloX > 0) {
						posiLimit = lastPlat.bounds.max.x;
						dif = posiLimit - transform.position.x;
					} else {
						posiLimit = lastPlat.bounds.min.x;
						dif = transform.position.x - posiLimit;
					}
				}
				if (dif <= 1) {
					VeloX = 0;
				}
			}
		} else {
			VeloX = 0;
			jump = false;
			boolX = true;
			boolY = true;
		}
		if (Flying && VeloX == 0) {
			VeloX = veloXOld;
		}
	}

	void chooseYourPath(){
		//if(Comment) print("Escolhendo destino");
		target [0] = float.NaN;
		target [1] = float.NaN;
		if (Flying) {
			target [0] = target_GO.transform.position.x;
			target [1] = target_GO.transform.position.y - Boxsize.y;
		} else if (difX > m_JumpDist / 8 || difY > m_JumpHeight / 5) {
			
			if (posiPath < 0)
				posiPath = 0;
			else if (posiPath > Path.Length)
				posiPath = Path.Length;
			//print ("Escolhendo caminho"+posiPath);

			if (posiPath < 0 && !(posiPath < Path.Length && float.IsNaN (Path [0] [0]))) {//Voltou a posição demais pegue sua ultima posição
				if (Comment)
					print ("Voltando impedido no curso normal" + posiPath);
				target = lastTarget;
			} else if (CountBFPath <= 10 && (difX < m_JumpDist / 3 && difY < m_JumpHeight / 3 || posiPath < Path.Length - 1 && null != Path [posiPath] && float.IsNaN (Path [posiPath] [0]))) {//Está proximo o suficiente para ir só
				CountBFPath++;
				if (Comment)
					print ("(Caminho incompleto) Proximo ");
				Path = new float[4][];
				Path [0] = new float[2];
				Path [0] [0] = float.NaN;
				//Path [0] [1] = 0;
				posiPath = -1;
				//posiPath = -1;//em toda rodada é incremetado um para o valor ficar zero é preciso q valor esteja -1(começar da posição 0 do Path)
				finded = true;
			} else if ((posiPath >= Path.Length) || finded) {//O caminho chegou ao fim,mas o inimigo não foi encontrado então recalculo
				if (Comment)
					print ("Recalculo para chegar em: " + target_GO.name);
				//if(CountBFPath>=waitforPath){
				finded = false;
				posiPath = -1;
				boolX = false;
				boolY = false;
				Dist = (this.GetComponent<CircleCollider2D> ().offset.y - this.GetComponent<CircleCollider2D> ().radius) * this.GetComponent<Transform> ().localScale.y / 2;
				pointAdap = target_GO.transform.position;
				pointAdap.y = pointAdap.y + Dist;
				CountBFPath = 0;

				Path = GameObject.Find ("GM").GetComponent<Global> ().Pathfind (this.gameObject, pointAdap);
				/*}else{
					CountBFPath++;
				}*/

			} else if (posiPath < Path.Length - 1 && !float.IsNaN (Path [posiPath] [0])) {//ainda não chegou siga o caminho
				//if(Comment) print ("Tentando se aproximar através do caminho");
				if (Comment)
					print ("Apenas seguindo");
				finded = false;
				target = Path [posiPath];
			}

			if (finded) {
				posiPath = -1;
				boolX = false;
				boolY = false;
				//float distanc = (transform.position.y > target_GO.transform.position.y + Dist) ? transform.position.y - target_GO.transform.position.y + Dist : target_GO.transform.position.y + Dist - transform.position.y;

				if (Flying || (difY < m_JumpHeight || target_GO.transform.position.y < transform.position.y)) {
					if (Comment)
						print ("Taking him");
					target [0] = target_GO.transform.position.x;
					target [1] = target_GO.transform.position.y + Dist;
				} else {
					if (Comment)
						print ("Can't taking him" + difX + " " + m_JumpHeight);
					target [0] = float.NaN;
				}
			}

			//try to do not sucide
			if (!minion) {
				//this.GetComponent<Animator> ().GetBool ("Ground") == false && this.GetComponent<Animator> ().GetFloat ("vSpeed") < 0
				if (target [0] != float.NaN) {
					bool perigo = false;
					float DistancX = (transform.position.x > target [0]) ? transform.position.x - target [0] : target [0] - transform.position.x;
					float DistancY = (transform.position.y > target [1]) ? transform.position.y - target [1] : target [1] - transform.position.y;
					float DistLastPlat = 0;
					if(target_GO.GetComponent<CharController> ().Flying){
						Collider2D lastPlat = this.GetComponent<CharController> ().m_lastPlat; 
						if (lastPlat != null) {
							float hLastPlat = lastPlat.bounds.max.y;
							DistLastPlat = (transform.position.y > hLastPlat) ? transform.position.y - hLastPlat : hLastPlat - transform.position.y;
							perigo = (this.GetComponent<CharController> ().m_MaxSpeed < DistancX) && ((m_Rigidbody2D.velocity.y < 0 && DistancY > hLastPlat));
						} else
							perigo = false;

					}else
						perigo = (this.GetComponent<CharController> ().m_MaxSpeed < DistancX) || ((transform.position.y < target [1] && DistancY > m_JumpHeight));

					if (perigo) {//|| Global.target.transform.position.y >= this.transform.position.y
						if(Comment) print ("Tentou Salvar");
						reloadTarget = true;
						lastTarget = target;
						if (lastPlat != null) {
							target [0] = lastPlat.bounds.center.x;
							target [1] = lastPlat.bounds.center.y;
						}
					}
			

			
				}
			}
		}
		//if(Comment) print("Destino escolhido"+target[0]+" "+target[1]);
		moviment ();
		if (reloadTarget) {
			reloadTarget = false;
			target = lastTarget;
		}
	}
		
	public void Golpe_Detec(int Efective){
		if (!noAtacking) {//se estiver atacando então o dano(qual animação=valor padrão para dano) e efetividade do ataque são calculados
			//Até o fim dessa funcao será identificado em q estágio dos ataques o personagem está e será guardado o golpe e quanto o ataque atual(animação) é efetiva.


			altArvCombo = transform.GetComponent<CharController> ().altArvCombo;
			int val=m_Anim.GetInteger ("Atk_1");//guarda valor de atk_1 
			//val != 0 && m_Anim.GetInteger ("Atk_2") == 0
			if (altArvCombo == 1) {
				n_Golpe [0] = val;
				//val = 0;
				if (n_EfectGolpe [0] == 0)
					n_EfectGolpe [0] = Efective;
				else if (Efective != 0)
					n_EfectGolpe [0] = ((n_EfectGolpe [0] > Efective) ? Efective : n_EfectGolpe [0]);
			} else if (altArvCombo == 2) {
				val = m_Anim.GetInteger ("Atk_2");
				if (val != 0)
					n_Golpe [1] = val;
				//val = 0;
				if (n_EfectGolpe [1] == 0)
					n_EfectGolpe [1] = Efective;
				else if (Efective != 0)
					n_EfectGolpe [1] = ((n_EfectGolpe [1] > Efective) ? Efective : n_EfectGolpe [1]);
			} else if (altArvCombo == 3) {
				val = m_Anim.GetInteger ("Atk_3");
				if (val != 0)
					n_Golpe [2] = val;
				if (n_EfectGolpe [2] == 0)
					n_EfectGolpe [2] = Efective;
				else if (Efective != 0)
					n_EfectGolpe [2] = ((n_EfectGolpe [2] > Efective) ? Efective : n_EfectGolpe [2]) ;
			}
			transform.GetComponent<CharController> ().Gdamaged = false;
		}
	}
}

