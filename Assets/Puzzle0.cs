using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puzzle0 : MonoBehaviour {
	public bool zerou = true,Comment,trap1=false;
	public int[] botao = new int[3]; 
	public int hit = 0,counter=0,counterFrames=0;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		//if (Comment) print ("Botões "+botao [0]+","+botao [1]+","+botao [2]);
		if (zerou) {
			if (++counter > 3) {
				counter = 0;
				botao[0]=0;botao[1]=0;botao[2]=0;
				trap1 = false;
				zerou = false;
				hit = 0;
				counterFrames = 0;
			}
		}
		if (botao [0] != 0 && botao [1] != 0 && botao [2] != 0) {
			if (botao [0] == 1 && botao [1] == 2 && botao [2] == 3) {
				if (Comment)
					print ("Só felicidade");
			} else {
				if (Comment)
					print ("Deu Ruim");
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
