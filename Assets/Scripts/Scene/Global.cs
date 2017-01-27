using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using UnityStandardAssets._2D;

public class Global : MonoBehaviour {
	public LayerMask m_WhatIsGround,m_WhatIsPlayer,m_WhatIsPlat;
	public float LimiteX, LimiteY;
	public int waitForPath;
	public static bool[] fases = new bool[5];
	public static GameObject target;
	public float[] gravityScale = new float[2];
	public static LayerMask WhatIsGround,WhatIsPlayer,WhatIsPlat;
	public static float LimitY, LimitX;
	public bool Comment = false;
	private int counter = 0;
	private GameObject Char1,Char2;
	private CircleCollider2D tarCollider;
	private float h,currentslow;
	private bool defense, jump,atk1,atk2,sprint,bolean=true;
	private Vector3 Ini = new Vector3(0,0,0), Fim = new Vector3(0,0,0);
	//float[][] distan=new float[3][];
	//public GameObject[] Char1_List = new GameObject[5];
	//public GameObject[] Char2_List = new GameObject[5];
	public List<GameObject> Char1_List = new List<GameObject>();
	public List<GameObject> Char2_List = new List<GameObject>();
	public List<GameObject> Graveyard_List = new List<GameObject>();
	public float rot = 0;
	public static void LoadLevel(int i){
		SceneManager.LoadSceneAsync(i,LoadSceneMode.Additive);
		if(fases[i] != null)
			fases [i] = true;
	}
	private void Awake () {
		LimitX = LimiteX;
		LimitY = LimiteY;
		WhatIsGround = m_WhatIsGround;WhatIsPlayer = m_WhatIsPlayer;WhatIsPlat = m_WhatIsPlat;
		Char1 = GameObject.Find("Char1");
		Char2 = GameObject.Find("Char2");
		target = Char1;
		print ("Char Atual" + target);
	}
	private void Update () {
		if (!fases[0]) {
			//SceneManager.LoadScene(0,LoadSceneMode.Additive);
			fases[0]=true;
			LoadLevel(1);
			LoadLevel(2);
			print ("Carregou Fases");
		}
		//35 46
		if (target.transform.position.x > 35 && target.transform.position.x < 46)
			GameObject.Find("Main Camera").GetComponent<Camera2DFollow>().minimumHeight = 2.3f;
		else if (target.transform.position.x > 48 && target.transform.position.x < 64)
			GameObject.Find("Main Camera").GetComponent<Camera2DFollow>().minimumHeight = 3.7f;
		else 
			GameObject.Find("Main Camera").GetComponent<Camera2DFollow>().minimumHeight = 12.3f;
		
		if (target.transform.position.x > 42.5f && !fases [4]) {
			LoadLevel (4);
		}
		counter++;

		if(!bolean)
		Debug.DrawLine (Ini,Fim);
		//Fim = new Vector3 (100, 100, 100);
		/*rot += 200 * Time.deltaTime;
		rot = Time.time *20;
		RenderSettings.skybox.SetFloat("_Rotate",rot);
		RenderSettings.skybox.SetTextureOffset ("_Parallax",offset);*/
		if(Input.GetKey ("q")){
			if(Graveyard_List.Count > 0){
				GameObject resurect = Graveyard_List [0];
				resurect.GetComponent<CharController> ().life = 40;
				resurect.transform.position = new Vector3 (0, 3, 0);
				resurect.GetComponent<AI> ().ini ();
				resurect.SetActive (true);
				Graveyard_List.RemoveAt(0);
			}
		}
			if (Input.inputString == "h") {
				toggle_target (true);
			} else if (Input.inputString == "g") {
				toggle_target (false);
			}
			if (Input.inputString == "1") {
				SceneManager.LoadScene ("fase_0");
			} else if (Input.inputString == "2") {
				SceneManager.LoadScene ("scene_2_caverna_baixo");
			}else if (Input.inputString == "3") {
				SceneManager.LoadScene ("scene_3_caverna_direita");
			}else if (Input.inputString == "3") {
				SceneManager.LoadScene ("scene_4_caverna_direita_baixo");
			}
			if (target.transform.position.y < 0 || Input.inputString == "r") {//se o personagem cair no infinito ou apertar R será reiniciando a partida
				/*if(target.transform.position.x > 36 && target.transform.position.x<39)
					SceneManager.LoadScene ("scene_2_caverna_baixo");
				else
					SceneManager.LoadScene (SceneManager.GetActiveScene ().name);*/
				print("Ainda vou ver como reiniciar");
				
			}
			if (Input.GetKey ("p") && counter >= 10) {
				counter = 0;
				if (Time.timeScale != 0f)
					Time.timeScale = 0f;
				else
					Time.timeScale = 1.0f;
			}
			
			if (Input.GetKey ("\\"))
			if (Time.timeScale == 1.0)
				Time.timeScale = 0.3f;
			else
				Time.timeScale = 1.0f;
			if (Time.timeScale == 0.3)
				currentslow = Time.deltaTime;
			if (currentslow > 0) {
				currentslow = 0;
				Time.timeScale = 1.0f;
			}
		
		jump = Input.GetKeyDown("up");
		defense = Input.GetKey("down");
		atk1 = Input.GetKey("f");
		atk2 = Input.GetKey("t");
		sprint = Input.GetKey("left shift");
		h = Input.GetAxis ("Horizontal");
		if (h != 0 || defense || jump || ((atk1 || atk2) ? 2 : 0) != 0) {
			target.GetComponent<CharController> ().Move (h, defense, jump, (atk1) ? 1 : ((atk2) ? 2 : 0), sprint);
		}
		jump = false;

		if(Char1_List.Count != 0 || Char2_List.Count != 0)
			for(int i = 0;i<Graveyard_List.Count;i++){
			//print (Char1_List[0] == Graveyard_List[i]);
			Char1_List.Remove(Graveyard_List[i]);
			Char2_List.Remove(Graveyard_List[i]);
			}
	}


	public bool Died(GameObject who){
		//GameObject Send = Char2;
		if(Comment) print(Char1);
		if(Comment) print(Char2);
		if (Char1 != null && Char1.activeSelf) {
			if (Char1.GetInstanceID () == who.GetInstanceID ()) {
				Char1_List.Clear ();
			}else if(Char1.GetComponent<AI> ().enabled && Char1.GetComponent<AI> ().target_GO.GetInstanceID () == who.GetInstanceID ())
				Char1.GetComponent<AI> ().target_GO = null;
			
		}
		if (Char2 != null && Char2.activeSelf) {
			if (Char2.GetInstanceID () == who.GetInstanceID ()) {
				Char2_List.Clear ();
			}else if(Char2.GetComponent<AI> ().enabled && !Char2.GetComponent<AI> ().target_GO && Char2.GetComponent<AI> ().target_GO.GetInstanceID () ==  who.GetInstanceID ())
				Char2.GetComponent<AI> ().target_GO = null;

		}
		Graveyard_List.Add (who);
		Char1_List.Remove(who);
		Char2_List.Remove(who);
		who.GetComponent<AI>().target_GO = null;
		who.SetActive (false);
		return (Graveyard_List[Graveyard_List.Count-1] == who);
	}



	public GameObject chooseTarget(GameObject who){
		//GameObject Send = Char2;
		if (!(who.name == "Char1" || who.name == "Char2")) {
			if (Char2_List.Count < Char1_List.Count && Char2 != null && Char2.activeSelf) {
				Char2_List.Add (who);
				return Char2;
			} else if (Char1 != null && Char1.activeSelf) {
				Char1_List.Add (who);
				return Char1;
			} else {
				return null;
			}
		} else 
			if (who.name == "Char1" && Char1_List.Count > 0) 
				return Char1_List [0];
			else if(who.name == "Char2"&& Char2_List.Count > 0 )
				return Char2_List [0];
			
			if (who.name == "Char1" && Char2_List.Count > 0) 
				return Char2_List [0];
			else if(who.name == "Char2"&& Char1_List.Count > 0 )
				return Char1_List [0];
		return null;
	}


	void toggle_target(bool bool_AI){//Altera os personagens,alterando o foco da camera e ativando a ai e ativando o controle no outro personagem 
		target.GetComponent<SpriteRenderer> ().sortingOrder = 0;
		if(bool_AI)
			target.GetComponent<AI> ().enabled = true;
		
		if (Char1 != null && target == Char1 && Char2 != null && Char2.activeSelf) {
			target = Char2;
		} else if (Char1 != null && Char1.activeSelf) {
			target = Char1;
		}
		target.GetComponent<SpriteRenderer> ().sortingOrder = 1;
		target.GetComponent<AI> ().enabled = false;
		GameObject.Find("Main Camera").GetComponent<Camera2DFollow> ().Change ();
	}



	/*Pathfind é "criado" um linha q passa o Bot até o target e todo colisor q colidir com essa linha
	do tipo plataforma será utilizado como objeto para ser ultrapassado,tendo como base 2 pontos o centro e outro q será decidido
	de acordo com as regras para melhor rota podendo ser a ponta esquerda ou direita e assim é rotornado um vet de vet de int
	de 4 por 2 sendo 2 x e y*/
	public float[][] Pathfind(GameObject Bot,Vector3 Target){
		if(bolean){//Debug
			Ini = Bot.transform.position;
			Fim = Target;
			//if(Comment) print ("Pathfind do personagem para o alvo "+Ini+Fim);
			bolean = false;
		}

		bool cima = (Bot.transform.position.y<=Target.y);

		float m_JumpHeight = Bot.GetComponent<CharController> ().m_JumpHeight/2;
		//float m_JumpDist = Bot.GetComponent<CharController> ().m_MaxSpeed;
		float[][] vectorPaths=new float[4][];
		int j= 0;
		float dif = 0;
		RaycastHit2D[] colliders = Physics2D.LinecastAll(Bot.transform.position, Target, m_WhatIsPlat);

		for (int i = 0; i <= 3; i++) {
			vectorPaths [i] = new float[2];
			if(colliders.Length==0 || j>=colliders.Length){
				vectorPaths [i] [0] = float.NaN;
				vectorPaths [i] [1] = 1;
			}else
			if (cima) {
				int l = j;
				for (; dif < m_JumpHeight; ++l) {
					if (l >= colliders.Length)
						break;
					if (i != 0 && !float.IsNaN (vectorPaths [i - 1] [0]))
						dif = (colliders [l].collider.bounds.center.y > vectorPaths [i - 1] [1]) ? colliders [l].collider.bounds.center.y - vectorPaths [i - 1] [1] : vectorPaths [i - 1] [1] - colliders [l].collider.bounds.center.y;
					else if (i == 0)
						dif = (colliders [l].collider.bounds.center.y > Bot.transform.position.y) ? colliders [l].collider.bounds.center.y - Bot.transform.position.y : Bot.transform.position.y - colliders [l].collider.bounds.center.y;
				}
				j = l - 1;
					//|| j == colliders.Length
				if (dif >= m_JumpHeight ) {
						//if(Comment) print (i + " " + j);
					dif = 0;
					if (j > 0)
						j--;
					else if (dif < m_JumpHeight) {
						vectorPaths [i] [0] = float.NaN;
						vectorPaths [i] [1] = 1;
						continue;
					}
					if (i != -1) {
						vectorPaths [i] [0] = colliders [j].collider.bounds.center.x;
						vectorPaths [i] [1] = colliders [j].collider.bounds.center.y;
						j++;
					}
				} else {
					vectorPaths [i] [0] = float.NaN;
					vectorPaths [i] [1] = 1;
				}
			} else {
				//print (colliders [j].collider.bounds.center.x);
				vectorPaths [i] [0] = colliders [j].collider.bounds.center.x;
				vectorPaths [i] [1] = colliders [j].collider.bounds.center.y;
				i++;
				vectorPaths [i] = new float[2];
				if ((i == 0 && Bot.transform.position.x < colliders [j].collider.bounds.center.x + colliders [j].collider.bounds.extents.x) || (i != 0 && vectorPaths [i - 1] [0] < colliders [j].collider.bounds.center.x + colliders [j].collider.bounds.extents.x))
					vectorPaths [i] [0] = colliders [j].collider.bounds.center.x - colliders [j].collider.bounds.extents.x;
				else
					vectorPaths [i] [0] = colliders [j].collider.bounds.center.x + colliders [j].collider.bounds.extents.x;
				vectorPaths [i] [1] = colliders [j].collider.bounds.center.y;
				j++;
			}
		}

		/*if(Comment)
			for (int i = 0; i < vectorPaths.Length; i++) {
				print (i + " " + vectorPaths [i] [0] + " " + vectorPaths [i] [1] + " " + Bot.name);
			}*/
		return vectorPaths;
	}

}

