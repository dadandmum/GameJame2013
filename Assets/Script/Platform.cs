using UnityEngine;
using System.Collections;

public class Platform : MonoBehaviour {
	
	public float SpeedFactor = 1.0f;
	public float JumpFactor = 1.0f;
	public float Speed = 0f;
	
	public bool IsMoveable = false;
	public float MoveDistanceMax = 10f;
	public float MoveDistanceMin = -10f;
	public float MoveSpeed = 5f;
	float tem_speed;
	Vector3 original_position;
	
	// Use this for initialization
	void Start () {
		original_position = transform.position;
		tem_speed = MoveSpeed;
	}
	
	// Update is called once per frame
	void Update () {
		if ( IsMoveable )
			Move();
	}
	
	public void Move(){
		float tem_x = transform.position.x;
		float ori_x = original_position.x;
		if ( tem_x > ori_x + MoveDistanceMax )
		{
			tem_speed = - Mathf.Abs( MoveSpeed );
		}
		if ( tem_x < ori_x + MoveDistanceMin )
		{
			tem_speed = + Mathf.Abs( MoveSpeed );
		}
		tem_x += tem_speed * Time.deltaTime;
		Vector3 pos = new Vector3( tem_x , original_position.y , original_position.z );
		transform.position = pos;
	}
	
	public float GetSpeedFactor(){
		return SpeedFactor;
	}
	public float GetJumpFactor(){
		return JumpFactor;
	}
	public float GetSpeed(){
		return Speed;
	}
}
