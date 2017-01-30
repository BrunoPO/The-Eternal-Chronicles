using UnityEngine;
using System.Collections;

public class VerticalFlyAction : PlayerAction 
{
	public VerticalFlyAction ( GameObject go )
		: base(go)
	{}
	public override bool  perform(float move,bool sprint){
		return true;
	}
	public override bool perform(bool jump,bool defense){
		if (!jump && !defense) {
				body.velocity = new Vector3 (body.velocity.x, 0, 0);
			} else if (defense) {
				body.velocity = new Vector3 (body.velocity.x, -3f, 0);
			} else if (jump) {
				body.velocity = new Vector3 (body.velocity.x, 3f, 0);
			}
		return true;
	}
}
