using UnityEngine;
using System.Collections;
using UnityStandardAssets._2D;

public abstract class PlayerAction
{
	protected GameObject gameObject;
	protected CharController playerScript;
	protected Rigidbody2D body;

	public PlayerAction ( GameObject go )
	{
		this.gameObject = go;

		body = go.GetComponent<Rigidbody2D> ();
		playerScript = go.GetComponent<CharController> ();
	}

	/**
	 *  True indica que a ação foi realizada com sucesso.
	 *  False indica que a ação não foi realizada.
	 */
	public abstract bool perform (bool jump,bool defense);
	public abstract bool perform (float move,bool sprint);
}