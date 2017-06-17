using UnityEngine;
using System.Collections;
using UnityStandardAssets._2D;

public class JumpAction : PlayerAction
{
	private CircleCollider2D GroundCols;
	bool m_Grounded, on_Ground,PlusJump;
	float m_JumpForce;
	private bool jump, defense;
	public JumpAction ( GameObject go )
		: base(go)
	{
		m_JumpForce = playerScript.m_JumpForce;
	}


	public override bool  perform(float move,bool sprint){
		return true;
	}
	public override bool  perform(bool jump,bool defense)
	{
		m_Grounded = playerScript.m_Grounded;
		on_Ground = playerScript.on_Ground;
		PlusJump = playerScript.PlusJump;
		//Debug.Log(m_Grounded + "," + on_Ground + "," + m_JumpForce + "," + PlusJump + ",");

		if(playerScript.canMoveY){
			if (m_Grounded && jump && defense) {//verifica se no chao,defesa e pulando (desativa trigger para cair)
				if (!on_Ground) {
					playerScript.GroundCols.isTrigger = true;
					playerScript.PlusJump = false;
				} else {
					jump = false;
					defense = false;
				}
			} else if ((m_Grounded || PlusJump) && jump) {//verifica se no chao, e pulando "envia" movimento para o corpo e desativa colisão
				if (!m_Grounded)
					playerScript.PlusJump = false;
				playerScript.GroundCols.isTrigger = true; 
				body.AddForce(new Vector2(0,-1*body.velocity.y+(m_JumpForce)),ForceMode2D.Impulse);//força adaptada a gravidade negativa
			} 
		}



		return true;
	}
}
