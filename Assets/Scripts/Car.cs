using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Car : MonoBehaviour {

	public float absDist;

	public float force;
	public float rotation;
	public float testRot;
	public float wDis,eDis,nDis,nwDis,neDis;

	public float pDist, nDist;
	public Vector3 startPos;

	public float[] inputs = new float[5];
	private NeuralNetwork net;
	private Rigidbody2D rBody;

	public manager man;

	public Text fitnessText;


	public Ray eastRay, westRay, northRay, northWestRay, northEastRay;
	// Use this for initialization
	void Start () {

		startPos = transform.position;
		fitnessText = GameObject.Find("Text (1)").GetComponent<Text>();

	}
	
	void Update()
	{
		
		fitnessText.text = "Fitness: " + net.GetFitness();
	}

	void FixedUpdate()
	{
		


		RaycastHit hit;
		eastRay = new Ray (transform.position, -transform.forward);
		//Debug.DrawRay(transform.position, -transform.forward, Color.red);
		westRay = new Ray (transform.position, transform.forward);

		Quaternion spreadAngle = Quaternion.AngleAxis(45, new Vector3(0, 1, 0));

		Quaternion spreadAngle2 = Quaternion.AngleAxis(135, new Vector3(0, 1, 0));

		Vector3 noAngle = transform.forward;
		Vector3 rotatedVector = spreadAngle * noAngle;
		Vector3 rotatedVector2 = spreadAngle2 * noAngle;


		northRay = new Ray (transform.position, transform.right);
		northWestRay = new Ray (transform.position, rotatedVector);
		northEastRay = new Ray (transform.position, rotatedVector2);

		Debug.DrawRay(transform.position, -transform.forward, Color.red);//west
		Debug.DrawRay(transform.position, transform.forward, Color.green);//east
		Debug.DrawRay(transform.position, transform.right, Color.blue);//north
		Debug.DrawRay(transform.position, rotatedVector, Color.black);//ne
		Debug.DrawRay(transform.position, rotatedVector2, Color.cyan);//nw



		inputs = new float[5];


		if (Physics.Raycast (eastRay, out hit))
			inputs [0] = hit.distance;
		if (Physics.Raycast (westRay, out hit))
			inputs [1]= hit.distance;
		if (Physics.Raycast (northRay, out hit))
			inputs [2]= hit.distance;
		if (Physics.Raycast (northWestRay, out hit))
			inputs [3]= hit.distance;
		if (Physics.Raycast (northEastRay, out hit))
			inputs [4]= hit.distance;


		float[] output = net.feedForward(inputs);

		driveForward();
		if (output [0] >= 0f) 
			turnRight ();
		if (output [1] >= 0f)
			turnLeft ();

			//if()
			//{
				
			//	net.AddFitness(10f);
			//}
		absDist = Mathf.Abs(inputs[0] - inputs[1]);

		//Debug.Log(inputs[3]);
		//abs a-b < 2
	}

	public void Init(NeuralNetwork net)
	{
		//initialized = true;
		this.net = net;
	}

	void OnTriggerEnter(Collider col)
	{
		if (col.tag == ("wall")) {
			Destroy (gameObject);
			net.AddFitness (0f );
		}
		if(col.tag == ("goal"))
		{
			Destroy(col.gameObject);
			//net.AddFitness(absDist - inputs[2]);
		}
	}


	void driveForward ()
	{
		gameObject.GetComponent<Rigidbody>().velocity += transform.right * force;
		net.AddFitness(1f);
	}//new Vector3(force, 0f, 0f);}
		


	void turnLeft(){transform.Rotate(0f,-rotation,0f);}

	void turnRight(){transform.Rotate(0f,rotation,0f);}


	void OnDestroy(){
		
	}


}


