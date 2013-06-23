using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Collider))]

public class LoseLeg : MonoBehaviour {
	
	// Use this for initialization
	void Start () {
		
	}
	
	void OnTriggerEnter( Collider col ){
		PlayerControl.player.SendMessage("OnLoseLeg" );
	}
	
}
