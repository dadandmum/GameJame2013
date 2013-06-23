using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Renderer))]

public class Organ : MonoBehaviour {
	
	
	public bool IfRotate = true;
	public float MaxOmiga = 5f;
	public float MaxRotateTime = 2f;
	public float RotateTime = 2f;
	public float DiffAngle = 180f;
	public float DiffAngleAdd = 3f;
	public Vector3 LocalPosition = Vector3.zero;
	float max_omiga ;
	public float InitialTime = 0f;
	public float InitInitialTime = 0f;
	
	public float SpeedIncrease = 2f;
	public bool IfChangeColor = false;
	public Animation.ColorType ChangeColorTo = Animation.ColorType.Normal;
	private float scale;
	
	private float inner_time = 0f;
	
	// Use this for initialization
	void Start () {
		max_omiga = MaxOmiga / 180f * Mathf.PI;
		InitInitialTime = InitialTime + Random.Range( -0.1f,0.1f);
	}
	
	// Update is called once per frame
	void Update () {
		transform.localScale = PlayerControl.player.transform.localScale;
		Vector3 s = transform.localScale;
		s.z = 0.01f;
		transform.localScale = s;
		float k = 0;
		if ( PlayerControl.player )
			k = 0.1f * PlayerControl.player.GetComponent<PlayerControl>().GetSpeed().magnitude;
		
		
		RotateTime = MaxRotateTime / (1f + k);
		InitialTime = InitInitialTime / ( 1f + k );
		UpdateOrgan( );
	}
	
	void UpdateOrgan( )
	{
		transform.position = PlayerControl.player.transform.position + LocalPosition * transform.localScale.x;
		Debug.Log("Inner Time" + inner_time );
		if ( IfRotate )
		{
			if ( PlayerControl.GetIfInAir() && AtJumping() )
				return;
			
			inner_time += Time.deltaTime;
			float theta = ( inner_time + InitialTime ) / RotateTime * Mathf.PI * 2 ;
			float angle = MaxOmiga * Mathf.Sin( theta );
			transform.rotation = Quaternion.AngleAxis( DiffAngle , Vector3.forward )  * Quaternion.AngleAxis( angle , Vector3.forward );
		}
	}
	
	public bool AtJumping()
	{	
		float t = inner_time;
		while ( t > 1 ) 
			t -= 1.0f;
		if ( t > RotateTime / 4 - 0.02f && t < RotateTime / 4 +0.02f )
			return true;
		
		return false;
	}
	
	public float CallSpeed(){
		return SpeedIncrease;		
	}
	
	public void WhenAdd(){
		//if ( IfChangeColor )
		//{
		//	Animation.SetColorStyle( ChangeColorTo );
		//}
		if ( IfChangeColor )
		{
			Animation.AddEye();
		}
	}
	
	public void Copy( Organ o , int i ){
			
		IfRotate = o.IfRotate;
		MaxOmiga = o.MaxOmiga;
		RotateTime = o.RotateTime;
		DiffAngle = o.DiffAngle + DiffAngleAdd * i ;
		LocalPosition = o.LocalPosition;
		LocalPosition.z += i * 0.01f;
		
		SpeedIncrease = o.SpeedIncrease;
		IfChangeColor = o.IfChangeColor;
		ChangeColorTo = o.ChangeColorTo;
	
	}
	
	
}
