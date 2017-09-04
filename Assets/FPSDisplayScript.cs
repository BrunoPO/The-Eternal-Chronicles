using System.Collections;
using UnityEngine;

public class FPSDisplayScript : MonoBehaviour {
	int lastPosi = 0;
	int[] frames = new int[6];
	string s ="";
	int timer = 0;

	float timeA;
	public int fps;
	public int lastFPS;
	public GUIStyle textStyle;
	// Use this for initialization
	void Start () {
		timeA = Time.timeSinceLevelLoad;
		DontDestroyOnLoad (this);
	}

	// Update is called once per frame
	void Update () {
		//Debug.Log(Time.timeSinceLevelLoad+" "+timeA);
		if(Time.timeSinceLevelLoad  - timeA <= 1)
		{
			fps++;
		}
		else
		{
			lastFPS = fps + 1;
			timeA = Time.timeSinceLevelLoad;
			if(timer<4){
				timer++;
			}else{
				timer = 0;
				s ="[";
				if(lastPosi<frames.Length)
					frames [lastPosi++] = lastFPS;
				for(int i=0;i<frames.Length;i++)
					s += frames[i]+",";
				s += "]";
				print (s);
			}

			fps = 0;
		}
	}
	void OnGUI()
	{
		GUI.Label(new Rect( 450,5, 30,30),""+lastFPS,textStyle);
	}
}
