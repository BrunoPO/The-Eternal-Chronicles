using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaoFalso : MonoBehaviour {
	GameObject Enemy;
	public int counter=0;
	// Use this for initialization
	void Start () {
		Enemy = GameObject.Find("Ativador");
	}
	
	// Update is called once per frame
	void Update () {
		if (GameObject.Find ("GM").GetComponent<Puzzle0> ().fim) {
			//Enemy = Instantiate(Resources.Load<GameObject>("Enemy_0")); 
			Enemy.transform.position = transform.position;
			Enemy.transform.SetParent (this.transform);
			counter++;
		}
		if (counter >= 24) {
			counter = 0;
			GameObject.Find ("GM").GetComponent<Puzzle0> ().zerou = true;
		}
	}
}
