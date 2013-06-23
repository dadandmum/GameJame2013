using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class OrganAdhere : MonoBehaviour {
	
	public GameObject PictureMesh = null;
	
	public float MaxOmiga = 30f;
	public float RotateTime = 2f;
	float max_omiga ;
	
	List<GameObject> organs = new List<GameObject>();
	List<float> initalTime = new List<float>();
	List<float> initalPosition = new List<float>();
	
	
	// Use this for initialization
	void Start () {
		if ( PictureMesh == null )
		{
			Debug.Log( "Can not find picture mesh." );
			enabled = false;
		}
		max_omiga = MaxOmiga / 180f * Mathf.PI;
	}
	
	// Update is called once per frame
	void Update () {
		if ( Input.GetKeyDown( KeyCode.A ) )
		{
			AddOrgan("Leg");
		}
		
		for( int i = 0 ; i < organs.Count ; ++ i )
		{
			UpdateOrgan( i );
		}
	}
	
	void UpdateOrgan( int i )
	{
		float theta = ( Time.time + initalTime[i] ) / RotateTime * Mathf.PI * 2 ;
		//theta = theta / Mathf.PI * 180f;
		float angle = MaxOmiga * Mathf.Sin( theta );
		
		organs[i].transform.rotation = Quaternion.AngleAxis( 180f , Vector3.forward )  * Quaternion.AngleAxis( angle , Vector3.forward );
	}
	
	void AddOrgan( string OrganName )
	{	
		GameObject organ = Instantiate( PictureMesh ) as GameObject;
		organ.renderer.sharedMaterial.mainTexture = Resources.Load( "Picture/Organ/" + OrganName ) as Texture;
		organ.transform.parent = transform;
		organ.transform.localPosition = Vector3.zero;
		//organ.renderer.sharedMaterial.mainTexture =;
			
		organs.Add( organ );
		//change color
		organ.GetComponent<Organ>().WhenAdd();
		
		initalTime.Add( Time.time );
		
	}
	
}
