using System.Xml.Schema;
using System;
using System.Numerics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class NPCarController : MonoBehaviour
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



	// Variables for ai
	private Vector3 targetPosition;
	public Transform Car;
	private float reachedTargetDistance = 7f;
	private float reverseDistance = 25f;

	//Variablen gegen Saltos
	public LayerMask layerMask;
	public Transform groundCheck;



	// Start is called before the first frame update
	void Start()
	{
		speed = 0;
		rotation = 0;
	}


	bool OnGround()
	{
		return Physics.CheckSphere(groundCheck.position, .5f, layerMask);
	}


	// Update is called once per frame
	void Update()
	{
		float forwardAmount = 1;
		float turnAmount = 0;
		float breakAmount = 0;
		targetPosition = Car.position;

		float distanceToTarget = Vector3.Distance(transform.position, targetPosition);
		if (distanceToTarget > reachedTargetDistance)
		{

			Vector3 dirToMovePosition = (targetPosition - transform.position).normalized;
			float dot = Vector3.Dot(transform.forward, dirToMovePosition);

			if (dot > 0)
			{
				forwardAmount = 1;
			}
			else
			{
				if (distanceToTarget > reverseDistance)
				{
					forwardAmount = 1f;
				}
				else
				{
					forwardAmount = -1;
				}

			}


			float angelToDir = Vector3.SignedAngle(transform.forward, dirToMovePosition, Vector3.up);

			if (angelToDir > 3)
			{
				turnAmount = 1;
			}
			else
			{
				if (angelToDir < -3)
				{
					turnAmount = -1;
				}
			}

		}
		else {
			breakAmount = 1;
		}	
		

		if (OnGround())
		{
			Drive(turnAmount, forwardAmount, breakAmount);
		}
	}

	private void Drive(float protation, float pforward, float pbreak)
	{
		float InRot = protation;
		float InSp = pforward;
		float InBr = pbreak;

		if (InRot == 0)
		{
			if (speed != 0)
			{
				rotation -= rotation * SteerStraighteningPower * Time.deltaTime;
			}
		}
		else
		{
			if ((rotation > 0 && InRot < 0) || (rotation < 0 && InRot > 0))
				rotation = 0;

			rotation += InRot;
		}

		rotation -= 0.1f * InRot * Time.deltaTime;

		if (pbreak > 0)
		{
			if (speed > 0)
			{
				speed -= BreakForce * Time.deltaTime;
				speed = Mathf.Clamp(speed, 0, MaxForwardSpeed);
			}
			else
			{
				speed += BreakForce * Time.deltaTime;
				speed = Mathf.Clamp(speed, MaxBackwardSpeed, 0);
			}

			if (Mathf.Abs(speed) < 2f)
				speed = 0;
		}
		else if (InSp == 0)
		{
			if (speed > 0)
			{
				speed -= speed * EngineBreakPower * Time.deltaTime;
				speed = Mathf.Clamp(speed, 0, MaxForwardSpeed);
			}
			else
			{
				speed -= speed * EngineBreakPower * Time.deltaTime;
				speed = Mathf.Clamp(speed, MaxBackwardSpeed, 0);
			}
		}
		else if (InSp > 0)
		{
			speed += Acceleration;
		}
		else if (InSp < 0)
		{
			speed -= Acceleration;
		}


		if (Mathf.Abs(speed) < 0.1f)
			speed = 0;

		if (Mathf.Abs(rotation) < 0.1f)
			rotation = 0;

		rotation = Mathf.Clamp(rotation, MaxLeftSteer, MaxRightSteer);
		speed = Mathf.Clamp(speed, MaxBackwardSpeed, MaxForwardSpeed);
		if (speed > 0)
		{
			transform.Translate(UnityEngine.Vector3.forward * Mathf.Sqrt(speed) * Time.deltaTime);
			transform.Rotate(0f, rotation * Time.deltaTime, 0f);
		}
		else if (speed < 0)
		{
			transform.Translate(UnityEngine.Vector3.back * Mathf.Sqrt(-speed) * Time.deltaTime);
			transform.Rotate(0f, -rotation * Time.deltaTime, 0f);
		}
	}
}