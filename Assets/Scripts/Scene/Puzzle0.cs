using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Puzzle0 : MonoBehaviour {
	public bool zerou = true,Comment,trap1=false,fim=false;
	public int[] botao = new int[3]; 
	public int hit = 0,counter=0,counterFrames=0;
	public GameObject ChaoFalso;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (fim ) {
			GameObject.Find ("Main Camera").transform.position = new Vector3 (54.6f,3.72f,-0.3f);
		}
		if (Comment) print ("Botões "+botao [0]+","+botao [1]+","+botao [2]);
		if (zerou) {
			if (++counter > 30) {
				counter = 0;
				botao[0]=0;botao[1]=0;botao[2]=0;
				trap1 = false;
				zerou = false;
				fim = false;
				hit = 0;
				counterFrames = 0;
			}
		}
		if (botao [0] != 0 && botao [1] != 0 && botao [2] != 0) {
			if (botao [0] == 1 && botao [1] == 2 && botao [2] == 3) {
				fim = true;
				if (Comment)
					print ("Puzzle Concluido");
			} else {
				if (Comment)
					print ("Errou a ordem dos botões");
				zerou = true;
			}
		} else if(botao [0] != 0 || botao [1] != 0 || botao [2] != 0){
			counterFrames++;
		}
		if(counterFrames > 1800){
			print ("Puzzle Timeout");
			zerou = true;
		}
	}
}
