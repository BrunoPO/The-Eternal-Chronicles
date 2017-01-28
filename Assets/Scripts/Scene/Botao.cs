using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Botao : MonoBehaviour {
	public int posi = 0;
	private bool damaged = false;
	// Update is called once per frame
	private void Start(){
	}
	void Update () {
		if (GameObject.Find ("GM").GetComponent<Puzzle0> ().zerou) {
			GetComponent<CharController> ().Gdamaged = false;
			damaged = false;
		}
		GetComponent<Animator>().SetBool ("Damaged", damaged); 
		if(GetComponent<CharController>().Gdamaged){
			if (!damaged) {
				GameObject.Find ("GM").GetComponent<Puzzle0> ().botao [posi-1] = ++GameObject.Find ("GM").GetComponent<Puzzle0> ().hit;
			}
			damaged = true;
		}
	}
}
