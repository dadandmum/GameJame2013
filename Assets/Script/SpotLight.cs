using UnityEngine;
using System.Collections;

public class SpotLight : MonoBehaviour {
	
	Vector3 StartPosition;
	
	// Use this for initialization
	void Start () {
			StartPosition = new Vector3( 0f , 0f , -8f );
	}
	
	// Update is called once per frame
	void Update () {
		if ( Animation.color_type != Animation.ColorType.Normal )
		{
			GetComponent<Light>().enabled = false;
			
		}else
		{
			transform.position = PlayerControl.player.transform.position + StartPosition;
			GetComponent<Light>().enabled = true;
			
		}
		
	}
}
