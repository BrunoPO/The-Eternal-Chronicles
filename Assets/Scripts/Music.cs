//Controla a musica de fundo

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Music : MonoBehaviour {


	public AudioSource audio;
	private bool isMusicMuted;
	// Use this for initialization
	void Start () {
		audio = gameObject.GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown("m")){
			isMusicMuted = !isMusicMuted;
			audio.mute = isMusicMuted;
		}
	}
}
