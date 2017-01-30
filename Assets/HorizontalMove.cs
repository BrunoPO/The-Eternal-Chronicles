using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HorizontalMove : PlayerAction {
	float m_MaxSpeed;
	bool m_FacingRight,animToRight;
	public HorizontalMove ( GameObject go )
		: base(go)
	{
		m_MaxSpeed = playerScript.m_MaxSpeed;
	}
	public override bool perform (bool move, bool sprint){
		return true;
	}
	public override bool perform(float move,bool sprint){
		if (playerScript.canMoveX) {
			float veloX = 0;
			float sprint_velo = 0;
			if (sprint && playerScript.sprint_velo == 0){
				sprint_velo = playerScript.intecSprint * playerScript.m_MaxSpeed;
			}
			else if (playerScript.sprint_velo > 0){
				sprint_velo = playerScript.sprint_velo - 0.1f;
				//print("teste");
			}				
			else
				sprint_velo = 0;

			veloX = (move * m_MaxSpeed) + ((!playerScript.m_FacingRight) ? sprint_velo : -sprint_velo);	

			body.velocity = new Vector2 (veloX, body.velocity.y);

			//if (Comment) print (animToRight);
			m_FacingRight = playerScript.m_FacingRight;
			animToRight = playerScript.animToRight;
			if (animToRight == true) {
				if (move < 0 && m_FacingRight)
					playerScript.Flip ();
				else if (move > 0 && !m_FacingRight)
					playerScript.Flip ();
			} else if (animToRight != true) {
				if (move < 0 && !m_FacingRight)
					playerScript.Flip ();
				else if (move > 0 && m_FacingRight)
					playerScript.Flip ();
			}
		}
		return true;
	}
}
