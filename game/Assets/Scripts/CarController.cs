using System.Xml.Schema;
using System;
using System.Numerics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
	private float speed;
	private float rotation;
	public float MaxLeftSteer = -45;
	public float MaxRightSteer = 45;
	public float MaxForwardSpeed = 1024;
	public float MaxBackwardSpeed = -16;
	public float EngineBreakPower = 2f;
	public float Acceleration = 1;
	public float BreakForce = 10;
	public float SteerStraighteningPower = 2f;
	public float AbruptBreak =2;

	//Variablen gegen Saltos
	public LayerMask layerMask;
	public Transform groundCheck;


	// Start is called before the first frame update
	void Start()
	{
		speed = 0;
		rotation = 0;
	}

	// Update is called once per frame
	void FixedUpdate()
	{
		if (OnGround())
		{
			Drive();
		}
	}

	bool OnGround()
	{
		return Physics.CheckSphere(groundCheck.position, 1f, layerMask);
	}

	void Drive()
	{
		float InRot = Input.GetAxis("Horizontal");
		float InSp = Input.GetAxis("Vertical");

		// Steering
		if(InRot==0)
		{
			if(speed!=0)
			{
				rotation -= rotation*SteerStraighteningPower*Time.deltaTime;
			}
		}
		else
		{
			if((rotation>0 && InRot<0)||(rotation<0 && InRot>0))
				rotation=0;

			rotation += InRot;
		}

		rotation -= InRot*Time.deltaTime;

		if(Input.GetAxis("Jump")>0) //Breaking
		{ 
			if(speed>0)
			{
				speed -= BreakForce*Time.deltaTime;
				speed = Mathf.Clamp(speed, 0, MaxForwardSpeed);
			}
			else
			{
				speed += BreakForce*Time.deltaTime;
				speed = Mathf.Clamp(speed, MaxBackwardSpeed, 0);
			}

			if(Mathf.Abs(speed)<AbruptBreak)
				speed = 0;
		}
		else if(InSp == 0) //Engine Breaking
		{
			if(speed>0)
			{
				speed -= speed*EngineBreakPower*Time.deltaTime;
				speed = Mathf.Clamp(speed, 0, MaxForwardSpeed);
			}
			else
			{
				speed -= speed*EngineBreakPower*Time.deltaTime;
				speed = Mathf.Clamp(speed, MaxBackwardSpeed, 0);
			}
		}
		else if(InSp>0) // Acceleration
		{
			speed += Acceleration;
		}
		else if(InSp<0)
		{
			speed -= Acceleration;
		}


		if(Mathf.Abs(speed)<0.1f) // Abrupt break
			speed=0;

		if(Mathf.Abs(rotation)<0.1f) // Abrupt straightening
			rotation=0;

		// Max values
		rotation = Mathf.Clamp(rotation, MaxLeftSteer, MaxRightSteer);
		speed = Mathf.Clamp(speed, MaxBackwardSpeed, MaxForwardSpeed);
		if(speed>0) //Moving and rotating the car
		{
			transform.Translate(UnityEngine.Vector3.forward * Mathf.Sqrt(speed) * Time.deltaTime);
			transform.Rotate(0f,rotation*Time.deltaTime,0f);
		}
		else if (speed<0)
		{
			transform.Translate(UnityEngine.Vector3.back * Mathf.Sqrt(-speed) * Time.deltaTime);
			transform.Rotate(0f,-rotation*Time.deltaTime,0f);
		}
	}
}
