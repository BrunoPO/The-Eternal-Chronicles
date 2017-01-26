using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plataforma : MonoBehaviour {
	public Vector3 end;
	public float time = 1.0f;
	public bool relative = false;
	public bool evil = false;
	public float distBFRGetOut;

	protected Vector3 start;
	protected Vector3 speed;
	protected float travelTime = 0.0f;
	private bool temPassageiros=false;

	protected List<GameObject> ignoreList = new List<GameObject>();

	// Use this for initialization
	void Start ()
	{	
		start = transform.position;
		if (relative) end += start;

		speed = (end-start) / time;

		if (Application.isEditor)
			print ( start );
	}

	// Update is called once per frame
	void Update ()
	{
		Vector3 pos = transform.position;
		temPassageiros = false;
		foreach (Transform child in transform)
		{
			if (child.gameObject.CompareTag ("Alive") || child.gameObject.CompareTag ("Dead")) {
				temPassageiros = true;

				if(Mathf.Abs(transform.position.y-child.GetChild(0).transform.position.y)>=distBFRGetOut){
				print ("Out: " + child.gameObject);
				if( child.transform.parent != null )
					child.transform.SetParent ( null );
				}
			}
		}
		//if (travelTime <= time || travelTime <= 0) {
		if (temPassageiros && travelTime < time*0.8) {
			pos = transform.position + Time.deltaTime * speed;
			travelTime += Time.deltaTime;
		} else if(!temPassageiros && travelTime > 0){
			pos = transform.position + Time.deltaTime * -speed;
			travelTime -= Time.deltaTime;
		}
		transform.position = pos;
	}

	void OnCollisionEnter2D( Collision2D coll )
	{
		print ("In: " + coll.gameObject);

		// BUG: alguém consegue achá-lo? :D

		if (coll.gameObject.CompareTag ("Alive"))
		{
			coll.transform.SetParent (this.transform);
		}
	}

	//void OnTriggerExit2D( Collision2D coll ){}

	void OnDrawGizmos()
	{

	}

	void OnDrawGizmosSelected()
	{
		//Gizmos.matrix = Matrix4x4.identity;

		Vector3 size = gameObject.GetComponent<SpriteRenderer> ().bounds.extents;

		Gizmos.color = new Color(1, 1, 0, 0.5F);
		Gizmos.DrawLine (start, end);

		Gizmos.color = new Color(0, 0, 1, 0.4F);
		Gizmos.DrawCube ( end, size * 2.0f);
	}



}
