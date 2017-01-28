using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Espinho : MonoBehaviour {
	Rigidbody2D m_Rigidbody2D ;
	public int milsBfGetDown = 0;
	public int counter=0;
	// Use this for initialization
	void Start () {
		m_Rigidbody2D = GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update () {
		if (GameObject.Find ("GM").GetComponent<Puzzle0> ().zerou) {
			counter = 0;
		}else if (GameObject.Find ("GM").GetComponent<Puzzle0> ().trap1) {
			if (counter <= milsBfGetDown) {
				counter++;
			} else {
				m_Rigidbody2D.gravityScale = 3;
			}
		}
	}
}
