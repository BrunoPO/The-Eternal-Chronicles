using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayLongSoundEffect : StateMachineBehaviour {

	public AudioClip clip;
	private AudioSource sound_effect;

	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		Debug.Log("OnStateEnter");
		sound_effect = animator.GetComponent<AudioSource>();
		sound_effect.clip = clip;
		sound_effect.Play();
		//sound_effect.PlayClipAtPoint(clip, animator.transform.position, 0.3F);
	}

	// OnStateExit is called when a transition ends and the state machine finishes evaluating this state
	override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		//Debug.Log("OnStateExit: " + layerIndex);
		sound_effect.Stop();
	}
}
