using UnityEngine;
using System.Collections;
using UnityStandardAssets._2D;

public class Dano : StateMachineBehaviour {
	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		if (!animator.GetComponent<AI> ().minion) {
			animator.GetComponent<CharController> ().noAtacking = true;
			animator.GetComponent<AI> ().round = 0;
			animator.SetInteger ("Atk_2", 0);
			animator.SetInteger ("Atk_3", 0);
		}
		animator.SetInteger ("Atk_1", 0);
	}
}
