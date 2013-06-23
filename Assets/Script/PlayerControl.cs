using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(Rigidbody))]

public class PlayerControl : MonoBehaviour {
	
	static bool MyEnable = true;
	
	static string ColliderTag = "LEVEL";
	
	public enum MoveDirection{ Left , Right };
	static public MoveDirection move_dir = MoveDirection.Right;
	
	private Vector3 LocateNormal = Vector3.up ;
	
	static private bool IfInAir = true;
	private bool lastAir = true;
	
	static public GameObject player = null;
	
	static public float GravityAcceleration = -15f;
	
	//right velocity
	public float HorizontalMaxSpeed = 5f;
	public float HorizontalSpeed = 20f;
	public float JumpVelocityUp = 10f; 
	
	
	//hands
	public GameObject HandPrefab;
	static public float HandPushspeed = 15f;
	public bool IfThrownHand = false;
	public KeyCode ThrowHandKey = KeyCode.H;
	
	//public List<GameObject> hands;
	public GameObject DealHand ;
	
	//locate plantform
	public Platform LocatePlatForm;
	
	//Impulse from wall
	public float ImpulseFromWallFactor = 0.1f;
	
	int IfInAirCount = 0;
	
	//Revive Point
	static public Vector3 RevivePoint = Vector3.zero;
	
	//Organs
	public List<GameObject> LeftLegs = new List<GameObject>();
	public List<GameObject> RightLegs = new List<GameObject>();
	
	//Organs
	public GameObject LeftLegPrefab;
	public GameObject RightLegPrefab;
	public GameObject EyePrefab;
	public GameObject LeftHandPrefab;
	public GameObject RightHandPrefab;
		
	// Use this for initialization
	void Start () {
		if ( player == null )
			player = GameObject.FindGameObjectWithTag("Player");
		if ( player == null )
			Debug.Log("Can not find a player." );
		
		if ( player.rigidbody )
		{
			player.rigidbody.useGravity = false;
			player.rigidbody.isKinematic = false;
		}
		if ( HandPrefab == null )
		{
			Debug.Log("Can not find hand");
			enabled = false;
		}else{
			if ( HandPrefab.GetComponent<SingleHand>() == null )
			{
				Debug.Log("Can not find SingleHand in hand" );
				enabled = false;
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
		if ( MyEnable )
		{
		if ( ShouldGoRight() )
			GoRight();
		if ( ShouldGoLeft() )
			GoLeft();
		if ( ShouldJump() )
			Jump();
		if ( ShouldPushHand() )
			PushHand();
		SetVelocity();
		Smooth();
		JudgeIfInAir();
		}
	}
	
	bool ShouldGoRight(){
		if ( Input.GetKey( KeyCode.RightArrow ) )
			return true;
		
		return false;
	}
	
	bool ShouldGoLeft(){
		if ( Input.GetKey( KeyCode.LeftArrow ) )
			return true;
		
		return false;
		
	}
	
	bool ShouldJump(){
			
		if ( !Input.GetKey( KeyCode.Space ) && !Input.GetKey( KeyCode.Space ) )
			return false;
		if ( IfInAir )
			return false;
			
		return true;
	}
	
	bool ShouldPushHand(){
		if ( !Input.GetKeyDown( KeyCode.H ) )
			return false;
		
		return true;
	}
	
	void GoRight(){
		move_dir = MoveDirection.Right;
	}
	
	void GoLeft(){
		move_dir = MoveDirection.Left;
	}
	
	void Jump(){
		float jump_fator = 1.0f;
		if ( LocatePlatForm != null )
			jump_fator = LocatePlatForm.GetJumpFactor();
		Vector3 velocity = player.rigidbody.velocity;
		velocity.y = JumpVelocityUp;
		player.rigidbody.velocity = velocity;
		IfInAir = true;
	}
	
	
	void PushHand(){
		if ( DealHand == null )
			DealHand = Instantiate( HandPrefab ) as GameObject;
		if ( DealHand.GetComponent<SingleHand>().state == SingleHand.StateType.Null )
		{
			Vector3 dir = new Vector3( 0.5f , 1f , 0f );
			DealHand.GetComponent<SingleHand>().Push( dir  );
		}else if ( DealHand.GetComponent<SingleHand>().state == SingleHand.StateType.Adhere )
		{
			DealHand.GetComponent<SingleHand>().Recieve();
			DealHand = null;
		}
		
	}
	
	void OnCollisionEnter(Collision coll){
		foreach ( ContactPoint c in coll.contacts )
		{
			if ( c.otherCollider.tag == ColliderTag )
			{
				if ( Vector3.Dot ( c.normal , Vector3.up ) > 0.1f )
				{
					LocateNormal = c.normal;
					LocateNormal.z = 0;
					LocateNormal.Normalize();
					//IfInAir = false;
					LocatePlatForm =  c.otherCollider.GetComponent<Platform>() ;
				}
			}
		}
	}
	
	void OnCollisionStay( Collision coll ){
		foreach ( ContactPoint c in coll.contacts )
		{
			if ( c.otherCollider.tag == ColliderTag )
			{
				player.transform.position += c.normal * Vector3.Dot( c.normal , player.rigidbody.velocity ) * 0.07f;
			}
		}
			
	}
	
	void OnCollisionExit( Collision coll ){
		foreach ( ContactPoint c in coll.contacts )
		{
			if ( c.otherCollider.tag == ColliderTag )
			{
				//IfInAir = true;
			}
		}
	}
	
	void SetVelocity(){
		/*
			Vector3 force = Vector3.zero;
		
			if ( move_dir == MoveDirection.Left )
				force = Vector3.left * 1f;
			if ( move_dir == MoveDirection.Right )
				force = Vector3.right * 1f;
			player.rigidbody.AddForce( force );
		*/
		//set direction move
		//if ( !IfInAir ){
		Vector3 vel_move = Vector3.zero;
		Vector3 vel_diff = Vector3.zero;
		Vector3 vel_grav = Vector3.zero;
		// if player touch a colider in upper
			
		float velocity_factor = 1.0f;
		if ( LocatePlatForm != null )
				velocity_factor = LocatePlatForm.GetSpeedFactor();
			
		//for set the velocity directly
		if ( move_dir == MoveDirection.Left )
			vel_move= Vector3.Cross( LocateNormal , Vector3.back ) * HorizontalMaxSpeed * velocity_factor;
		else
			vel_move= Vector3.Cross( LocateNormal , Vector3.forward ) * HorizontalMaxSpeed * velocity_factor;
		//vel_diff = LocateNormal * 0.01f;
		
		if ( CheckIfInAir() ){
			vel_grav = player.rigidbody.velocity;
			vel_grav.x = vel_grav.z = 0f;
			vel_grav.y += GravityAcceleration * Time.deltaTime;
			
			
			if ( move_dir == MoveDirection.Left )
				vel_move= Vector3.left * ( HorizontalSpeed + GetOrganSpeed() ) * velocity_factor;
			else
				vel_move= Vector3.right * ( HorizontalSpeed + GetOrganSpeed() ) * velocity_factor;

		}else
		{
			
			if ( LocatePlatForm != null )
			if ( move_dir == MoveDirection.Left )
				vel_move= Vector3.Cross( LocateNormal , Vector3.back ) * ( HorizontalSpeed + LocatePlatForm.GetSpeed() + GetOrganSpeed() ) * velocity_factor;
			else
				vel_move= Vector3.Cross( LocateNormal , Vector3.forward ) * ( HorizontalSpeed - LocatePlatForm.GetSpeed() + GetOrganSpeed() ) * velocity_factor;

		}
		
		player.rigidbody.velocity = vel_move + vel_diff + vel_grav;
		
		//for force
		//if ( move_dir == MoveDirection.Right )
		//	player.rigidbody.AddForce( Vector3.right * 3f );
		//if ( move_dir == MoveDirection.Left )
		//	player.rigidbody.AddForce( Vector3.left * 3f );
		
		//player.rigidbody.AddForce( Vector3.up * GravityAcceleration , ForceMode.Acceleration );
		
		
		//limit the velocity
		//Vector3 velocity = player.rigidbody.velocity;
		//velocity.x = Mathf.Clamp( velocity.x , - HorizontalMaxSpeed * velocity_factor , HorizontalMaxSpeed * velocity_factor );
		//velocity.z = 0;
		//player.rigidbody.velocity = velocity;
		
		Vector3 position = player.transform.position;
		position.z = 0f;
		player.transform.position = position;
		
	}
	
	bool CheckIfInAir(){
		if ( IfInAir == false )
			return false;
		if ( IfInAirCount < 3 )
			return false;
		return true;
	}
	
	void Smooth(){
		if ( lastAir != IfInAir )
		{
			IfInAirCount = 0 ;
		}else
		{
			IfInAirCount++;
		}
		
		
		
	}
	
	void JudgeIfInAir(){
		Ray ray = new Ray( player.transform.position , - LocateNormal );
		RaycastHit[] hits = Physics.RaycastAll( ray );
		float distance = 100f;
		RaycastHit min_distan_hit;
		foreach( RaycastHit hit in hits )
		{
			if ( hit.distance < distance && hit.collider.gameObject.tag == "LEVEL" )
			{
				distance = hit.distance;
				min_distan_hit = hit;
			}
		}
		
		float radius = ((SphereCollider) player.GetComponent<Collider>()).radius * player.transform.localScale.y + 0.002f;
		if ( distance > radius )
			IfInAir = true;
		else
		{
			IfInAir = false;
			LocatePlatForm = min_distan_hit.collider.gameObject.GetComponent<Platform>();
		}
		
		
	}
	
	void OnGUI(){
		//GUILayout.Label( "IfInAir" + CheckIfInAir() );	
		//if ( GUILayout.Button( "Add Left Leg" ) )
		//{
		//	player.SendMessage( "OnAddOrgan" , "LeftLeg" , SendMessageOptions.DontRequireReceiver );
		//}
		
		if ( die_time > 0 )
		{
			Rect rect = new Rect( Screen.width / 2 -  468f / 2  , Screen.height / 2 - 237f / 2 , 468f , 237f );
			Texture pic = Resources.Load( "Death/Faint" + ( frame + 1 ) ) as Texture;
			
			Rect rect_black = new Rect( 0f , 0f , Screen.width , Screen.height );
			Texture black = Resources.Load( "Death/Black" ) as Texture;
			
			GUI.color  = new Color ( 1.0f , 1.0f , 1.0f , 0.7f );
			GUI.DrawTexture( rect_black , black );
			
			GUI.color  = new Color ( 1.0f , 1.0f , 1.0f , 1.0f );
			GUI.DrawTexture( rect , pic );
			if ( die_time > FrameDeltaTime )
			{
				frame = ( frame + 1 ) % 4 ;
				die_time = Time.deltaTime;
			}else{
				die_time += Time.deltaTime;	
			}
			if ( Input.GetKeyDown( KeyCode.Space ) )
			{
				player.transform.position = RevivePoint;
				IfInAir = true;
				die_time = -1f;
				MyEnable = true;
			}
		}
		
		
	}
	
	public float DeathTime = 3f;
	static private float die_time = -1f;
	public int frame = 0;
	static public float FrameDeltaTime = 0.2f;
	
	static public void OnDead(){
		MyEnable = false;
		die_time = 0.01f;
		
	}
	
	void OnAddOrgan( string OrganName ){
		GameObject organ = null;
		if ( OrganName == "LeftLegSelect")
		{
			organ = Instantiate( LeftLegPrefab ) as GameObject;
			organ.GetComponent<Organ>().Copy( LeftLegPrefab.GetComponent<Organ>() , LeftLegs.Count );
			LeftLegs.Add( organ );
		}else if ( OrganName == "RightLegSelect" )
		{
			organ = Instantiate( RightLegPrefab ) as GameObject;
			organ.GetComponent<Organ>().Copy( RightLegPrefab.GetComponent<Organ>() , RightLegs.Count );
			RightLegs.Add( organ );
		}
		organ.GetComponent<Organ>().WhenAdd();
		GameObject body = GameObject.Find("Body");
		if ( body != null )
			organ.transform.parent = body.transform;
	}
	
	float  GetOrganSpeed(){
		float speed = 0f;
		foreach( GameObject o in LeftLegs )
		{
			speed += o.GetComponent<Organ>().CallSpeed();
		}
		foreach( GameObject o in RightLegs )
		{
			speed += o.GetComponent<Organ>().CallSpeed();
		}
		return speed;
	}
	
	static public bool GetIfInAir(){
		return IfInAir;
	}
	
	public Vector3 GetSpeed(){
		
		float velocity_factor = 1.0f;
		if ( LocatePlatForm != null )
				velocity_factor = LocatePlatForm.GetSpeedFactor();
		
		Vector3 vel = Vector3.zero;
		if ( LocatePlatForm != null )
		if ( move_dir == MoveDirection.Left )
			vel = Vector3.Cross( LocateNormal , Vector3.back ) * ( HorizontalSpeed + LocatePlatForm.GetSpeed() + GetOrganSpeed() ) * velocity_factor;
		else
			vel = Vector3.Cross( LocateNormal , Vector3.forward ) * ( HorizontalSpeed - LocatePlatForm.GetSpeed() + GetOrganSpeed() ) * velocity_factor;
		return vel;
	}
	
	static public void OnLoseLeg(){
		GameObject leg = null;
		if ( Random.Range( 0 , 2 ) == 1 )
		{
			if ( player.GetComponent<PlayerControl>().LeftLegs.Count > 0 )
			{
				leg = player.GetComponent<PlayerControl>().LeftLegs[0];
				player.GetComponent<PlayerControl>().LeftLegs.RemoveAt( 0 ) ;
			}
			if ( leg == null )
			if ( player.GetComponent<PlayerControl>().RightLegs.Count > 0 )
			{
				leg = player.GetComponent<PlayerControl>().RightLegs[0];
				player.GetComponent<PlayerControl>().RightLegs.RemoveAt( 0 ) ;
			}
		}else{
			if ( player.GetComponent<PlayerControl>().RightLegs.Count > 0 )
			{
				leg = player.GetComponent<PlayerControl>().RightLegs[0];
				player.GetComponent<PlayerControl>().RightLegs.RemoveAt( 0 ) ;
			}
			if ( leg == null )
			if ( player.GetComponent<PlayerControl>().LeftLegs.Count > 0 )
			{
				leg = player.GetComponent<PlayerControl>().LeftLegs[0];
				player.GetComponent<PlayerControl>().LeftLegs.RemoveAt( 0 ) ;
			}
		}
		if ( leg == null )
			return ;
		leg.GetComponent<Organ>().IfFadeeOut = true;
		
	}
	
}
