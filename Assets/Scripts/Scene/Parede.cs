using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactive_Itens : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	void  OnTriggerStay2D(Collider2D collision) {
		float y = 0;
		if (collision.gameObject.layer == 10 || collision.gameObject.layer == 9) {
			//print ("Interation with coll");
			y = collision.attachedRigidbody.velocity.y;
			if (collision.offset.x > transform.position.x) {
				collision.attachedRigidbody.velocity = new Vector3 (0.01f, y, 0);
			} else {
				collision.attachedRigidbody.velocity = new Vector3 (-0.01f, y, 0);
			}
			collision.isTrigger = false;
		}
	}
	// Update is called once per frame
	void Update () {
		
	}
}
