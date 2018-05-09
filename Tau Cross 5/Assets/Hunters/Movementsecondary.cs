using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movementsecondary : MonoBehaviour 
{

	public float maxSpeed = 7.5f;
	public bool modesec = false;
	float rotSpeed = 180f;


    // = = = // = = = Start up and Cycle Functions = = = // = = = //


	// Use this for initialization
	void Start () {
	modesec = false;
	}

	// Activator Switch, Activates all the AI To Run Auto for the rest of the time being
	void HermitPurple(){

				Vector3 diff = Vector3.zero - transform.position;
        		diff.Normalize();
 
       			float rot_z = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
        		transform.rotation = Quaternion.Euler(0f, 0f, rot_z - 90);


		Quaternion rot = transform.rotation;

		float z = rot.eulerAngles.z;

		z += 38f;

		rot = Quaternion.Euler( 0, 0, z );

		transform.rotation = rot;

		maxSpeed = 8;

	}


    // = = = // = = = Auto Run Actions = = = // = = = //


	// Update is called once per tick (use for Movement)
	void FixedUpdate (){

		Quaternion rot = transform.rotation;

		transform.rotation = rot;

		Vector3 pos = transform.position;

		Vector3 velocity = new Vector3(0, 1 * maxSpeed * Time.deltaTime, 0);

		pos += rot * velocity;

		transform.position = pos;

	}

	void OnTriggerEnter2D(Collider2D other)
	{
		//Debug.Log ("X");

     if (other.gameObject.name.Contains("Anti")) // When touching the outer layering object called the ANTI Dome, then turn around to face the middle with a random Speed change and random Slight Tilt Rotation from there
        {

        		Vector3 diff = Vector3.zero - transform.position;
        		diff.Normalize();
 
       			float rot_z = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
        		transform.rotation = Quaternion.Euler(0f, 0f, rot_z - 90);


        Quaternion rot = transform.rotation;

		float z = rot.eulerAngles.z;

		if (modesec == true)
		{z += Random.Range(48f,45f);
			modesec = false;}
		else
		if (modesec == false)
		{z += Random.Range(12f,9f);
			modesec = true;}

		rot = Quaternion.Euler( 0, 0, z );

		transform.rotation = rot;

		maxSpeed = Random.Range(7.75f, 8.75f);

			//Debug.Log ("Z");

        }
	}	


	// = = = // = = = RESTART FUNCTIONS AND RECIVER = = = // = = = //


	public void Erase() // Reciver for the CONTROLLER
    
    {modesec = false;KingCrimson();}

	void  KingCrimson () {	// Overall Restart System

				{GameObject.Find("XpawnE").transform.position = new Vector3(0,-26,-1);}
				{GameObject.Find("XpawnF").transform.position = new Vector3(26,0,-1);}
				{GameObject.Find("XpawnG").transform.position = new Vector3(-26,0,-1);}
				{GameObject.Find("XpawnH").transform.position = new Vector3(0,26,-1);}

		   	HermitPurple();

	}


}


	// = = = // = = = Extra Code = = = // = = = //


	// If ONE wishes to control the Pawns on thier own, they may plugin this code to interact with the AI
	/*void Update () {

		//ROTATE

		/*Quaternion rot = transform.rotation;

		float z = rot.eulerAngles.z;

		z += Input.GetAxis("Horizontal") * rotSpeed * Time.deltaTime;

		rot = Quaternion.Euler( 0, 0, z );

		transform.rotation = rot;


		//MOVEMENT

		Vector3 pos = transform.position;

		Vector3 velocity = new Vector3(0, Input.GetAxis("Vertical") * maxSpeed * Time.deltaTime, 0);

		pos += rot * velocity;

		transform.position = pos;*/



