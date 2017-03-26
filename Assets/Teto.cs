using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teto : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	void  OnTriggerStay2D(Collider2D collision) {
		float x = 0;
		if (collision.gameObject.layer == 10 || collision.gameObject.layer == 9) {
			//print ("Interation with coll");
			x = collision.attachedRigidbody.velocity.x;
			if (collision.offset.y > transform.position.y) {
				collision.attachedRigidbody.velocity = new Vector3 (x, 0.01f, 0);
			} else {
				collision.attachedRigidbody.velocity = new Vector3 (x, -0.01f, 0);
			}
			collision.isTrigger = false;
		}
	}
	// Update is called once per frame
	void Update () {
		
	}
}
