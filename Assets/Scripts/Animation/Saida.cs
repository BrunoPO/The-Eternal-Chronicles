using UnityEngine;
using System.Collections;
using UnityStandardAssets._2D;

public class Saida : StateMachineBehaviour {
	public bool continuaCombo = false;
	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		if(!animator.GetComponent<AI> ().minion && continuaCombo && (animator.GetInteger ("Atk_2") == 0 || animator.GetInteger ("Atk_3") == 0)){
			//Debug.Log ("On Enter"+animator.gameObject.name);
			animator.SetInteger ("Atk_1", 0);animator.SetInteger ("Atk_2", 0);animator.SetInteger ("Atk_3", 0);
			animator.GetComponent<CharController> ().noAtacking = true;
			animator.GetComponent<CharController> ().altArvCombo = 0;
			animator.GetComponent<CharController> ().waitTime = 0;
		}
	}
	override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		//if (!stateInfo.IsName ("mid")||(stateInfo.IsName ("mid") && animator.GetInteger ("Atk_3") == 0)) {
		//Zera se estiver saindo ou se for inimigo minion
		if (!continuaCombo){
			if (!animator.GetComponent<AI> ().minion) {
				animator.GetComponent<CharController> ().altArvCombo = 0;
				animator.GetComponent<CharController> ().noAtacking = true;
				animator.GetComponent<AI> ().round = 0;
				animator.SetInteger ("Atk_2", 0);
				animator.SetInteger ("Atk_3", 0);
			}
			animator.SetInteger ("Atk_1", 0);
			animator.GetComponent<CharController> ().waitTime = 0;
		}
	}
}
