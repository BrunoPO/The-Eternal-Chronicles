﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactive_Itens : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	void  OnTriggerEnter2D(Collider2D collision) {
		if (collision.gameObject.layer == 10 || collision.gameObject.layer == 9) {
			collision.isTrigger = false;
		}
	}
	// Update is called once per frame
	void Update () {
		
	}
}