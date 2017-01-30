using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySoundEffect : StateMachineBehaviour {
	public AudioClip clip;
	public float audioVolume = 1.0F;

    // OnStateEnter is called before OnStateEnter is called on any state inside this state machine
	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		if(audioVolume > 1.0F || audioVolume < 0) audioVolume = 1.0F;
		AudioSource.PlayClipAtPoint(clip, animator.transform.position, audioVolume);
	}
}
