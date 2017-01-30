using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySoundEffect : StateMachineBehaviour {
	public AudioClip clip;

    // OnStateEnter is called before OnStateEnter is called on any state inside this state machine
	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		AudioSource.PlayClipAtPoint(clip, animator.transform.position, 1.0F);
	}
}
