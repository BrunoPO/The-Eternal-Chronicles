using UnityEngine;
using System.Collections;
namespace UnityStandardAssets._2D{public class Nvl_Arv : StateMachineBehaviour {
	public int altArv =0 ;
	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
			
			animator.GetComponent<CharController> ().altArvCombo = (animator.GetComponent<CharController> ().altArvCombo<altArv)?altArv:animator.GetComponent<CharController> ().altArvCombo;
	}
}
}
