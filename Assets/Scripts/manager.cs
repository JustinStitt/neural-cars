using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class manager : MonoBehaviour {

//	public bool alive = true;
//public bool start = false;
//	public pipeSpawner spawner;
	public GameObject carPrefab;

	public GameObject goalFab;
	public GameObject goalFab2;

	GameObject g1,g2,g3,g4;

	public Vector3 goal1;
	public Vector3 goal2;
	public Vector3 goal3;
	public Vector3 goal4;

	//	public GameObject generationText;
	private bool isTraining = false;
	private int populationSize = 50;
	private int generationNumber = 0;
	private int[] layers = new int[] { 5, 10, 10, 2 }; //5 inputs and 2 outputs
	private List<NeuralNetwork> nets;

	private List<Car> carList = null;

	public bool testCarDebug = false;

	GameObject testCar;
	public Material debugMat;

	public Text generationText;

	void Start()
	{
		Time.timeScale = 5.0f;

	}

	void Timer()
	{
		isTraining = false;
	}

	void Update()
	{
		


		if(!isTraining)
		{
			if(generationNumber == 0)
			{
				InitCarNeuralNetworks();
			}
			else
			{
				nets.Sort();
				for(int i = 0; i < populationSize / 2; i++)
				{
					nets[i] = new NeuralNetwork(nets[i + populationSize/2]);
					nets[i].Mutate();

					nets[i + (populationSize / 2)] = new NeuralNetwork(nets[i + populationSize/2]);
				}

				for(int i = 0; i < populationSize; i++)
				{
					nets[i].SetFitness(0f);
				}
			}




			generationNumber++;

			generationText.GetComponent<Text> ().text = "Generation: " + generationNumber;

			isTraining = true;
			//Invoke ("Timer", 15f);


			CreateCarBodies();
			CreateGoals();

		}



		if (!GameObject.Find ("car(Clone)")) {
			isTraining = false;
		}




	}//update


	void CreateCarBodies()
	{

		carList = new List<Car>();
		if(carList != null)
		{
			for(int i = 0; i < carList.Count; i++)
			{
				GameObject.Destroy(carList[i].gameObject);
			}
		}



		for(int i = 0; i < populationSize; i ++)
		{
			//spawn birds on the screen all starting at different areas..
			Car Carr = ((GameObject)Instantiate(carPrefab, new Vector3(UnityEngine.Random.Range(-5f,5f),/* UnityEngine.Random.Range(-1f, 2f) */0f, UnityEngine.Random.Range(-10f,-20f)),carPrefab.transform.rotation)).GetComponent<Car>();
			Carr.Init(nets[i]);
			carList.Add(Carr);
		}



	//	Instantiate(goalFab, goal1Pos, goalFab.transform.rotation);
	//	Instantiate(goalFab, goal2Pos, goalFab.transform.rotation);
	//	Instantiate(goalFab, goal3Pos, goalFab.transform.rotation);
	}
	void CreateGoals()
	{

		if(g1 != null || g2 != null || g3 != null || g4 != null)
		{
			foreach(GameObject g in GameObject.FindGameObjectsWithTag("goal"))
			{
				Destroy(g);
			}
		}

		g1 = Instantiate(goalFab, goal1,goalFab.transform.rotation);
		g2 = Instantiate(goalFab2, goal2, goalFab.transform.rotation);
		g3 = Instantiate(goalFab, goal3, goalFab.transform.rotation);
		g4 =Instantiate(goalFab2, goal4,goalFab.transform.rotation);


	}
	void InitCarNeuralNetworks()
	{

		if(populationSize % 2 != 0)
		{
			populationSize = 20;
		}

		nets = new List<NeuralNetwork>();

		for(int i = 0; i < populationSize; i++)
		{
			NeuralNetwork net = new NeuralNetwork(layers);
			net.Mutate();
			nets.Add(net);
		}//end initbirdneuralnetworks

	}//initbirdneuralnetworks



	void LateUpdate()
	{
		
	}
}//class
