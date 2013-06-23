using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Collider))]

public class organSelect : MonoBehaviour {
	
	public string OrganName = null ;

	// Use this for initialization
	void Start () {
		collider.isTrigger = true;
		if ( OrganName == null )
			OrganName = transform.gameObject.name;
	}
	
	void OnTriggerEnter( Collider col ){
		if ( col.gameObject.tag == "Player" )
		{
			PlayerControl.player.SendMessage( "OnAddOrgan" , OrganName );
			Destroy( this.gameObject );
		}
	}
}
