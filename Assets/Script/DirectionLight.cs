using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Light))]

public class DirectionLight : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if ( Animation.color_type == Animation.ColorType.Normal )
		{
			GetComponent<Light>().enabled = false;
			
		}else
		{
			GetComponent<Light>().enabled = true;
			
		}
	}
}
