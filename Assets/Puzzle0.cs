using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puzzle0 : MonoBehaviour {
	public bool zerou = true,Comment;
	public int[] botao = new int[3]; 
	public int hit = 0;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Comment) print ("Botões "+botao [0]+","+botao [1]+","+botao [2]);
		if (zerou) {
			botao[0]=0;botao[1]=0;botao[2]=0;
			zerou = false;
			hit = 0;
		}
		if(botao [0] != 0 && botao [1] != 0 && botao [2] != 0){
			if (botao [0] == 1 && botao [1] == 2 && botao [2] == 3) {
				if (Comment) print ("Só felicidade");
			}else{
				if (Comment) print ("Deu Ruim");
				zerou = true;
			}
		}
	}
}
