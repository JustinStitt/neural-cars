using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class NeuralNetwork : IComparable<NeuralNetwork> {


	public Car car;

	private int[] layers;
	//private float[] input  = new float[2];
	private float[][] neurons;
	private float[][][] weights;

	public float fitness;

	public NeuralNetwork(int[] layers)
	{
		//constructs layers in the NN
		this.layers = new int[layers.Length];
		for (int i = 0; i < layers.Length; i++) 
		{
			this.layers[i] = layers[i];
		}//end for loop


		//these two methods initialize the neurons in the layers and the inter-layer connections between them
		InitNeurons();
		InitWeights();
		// - - -

	}//end constructor NeuralNetwork
	public NeuralNetwork()
	{
	}
	private void InitNeurons() //generates neuron matrix
	{
		//starts by generating a list of neurons , and then converts the list into an array
		List<float[]> neuronsList = new List<float[]>();//list of float arrays (number of neurons in that array)

		// - - - - - - - iterates through the layers in the NN and creates a new float array of neurons in each layer - - - - - - - 

		for(int i = 0; i < layers.Length; i++)//traverse all layers
		{
			neuronsList.Add (new float[layers [i]]);//add layer to neuron list
		}//end for loop

		neurons = neuronsList.ToArray(); // list conversion to an array

	}//end method InitNeurons()

	private void InitWeights()//generates weight array
	{
		//each layer needs it's own weight matrix for the neurons
		List<float[][]> weightsList = new List<float[][]>();


		/*  1. runs through all the neurons in this current layer (loop i)  
	2. creates a neuron weight (loop j) which is all the connections to the current neuron 
	3. iterates through all the connections and gives it a random weight (loop k) 
*/

		for (int i = 1; i < layers.Length; i++) //starts at the second layer since the first layer is input
		{
			List<float[]> layerWeightList = new List<float[]> (); //takes in the weights for every neuron 
			int neuronsInPreviousLayer = layers[i - 1];

			for (int j = 0; j < neurons [i].Length; j++) {

				float[] neuronWeights = new float[neuronsInPreviousLayer]; //neurons weights

				for (int k = 0; k < neuronsInPreviousLayer; k++)  //gives random weights to neuron weights
				{
					neuronWeights [k] = UnityEngine.Random.Range(-0.5f,0.5f);
				}//end for loop(k)

				layerWeightList.Add (neuronWeights);//neuron weights of the current layer are added to the weights list
			}//end for loop(j)

			weightsList.Add(layerWeightList.ToArray()); //convert the layer weights list to a 2d float array and then add it to the main weights list

		}//end for loop(i)

		weights = weightsList.ToArray(); // convert to 3d array

	}//end method InitWeights()


	//	void initializeLayers(int[] layers)
	//	{
	//		fillInputs (input);//Fills the first "0" layer with the values received from the game.
	//	}

	//void feedInputs(float[] input, 


	//	void fillInputs(float[] inputs)
	//	{
	//		input [0] = car.checkDistanceFromPipe();//Distance currently from the nearest pipe collision box
	//		input [1] = car.checkYDistanceFromPipe();//Distance currently from the center of the gap of the nearest pipe-obstacle scenario.
	//	}
	public NeuralNetwork(NeuralNetwork copyNetwork)
	{
		this.layers = new int[copyNetwork.layers.Length];
		for(int i = 0; i < copyNetwork.layers.Length; i++)
		{
			this.layers[i] = copyNetwork.layers[i];
		}//end for loop (i)

		InitNeurons();
		InitWeights();
		copyWeights(copyNetwork.weights);
	}//end deep copy constructor

	private void copyWeights(float[][][] copyWeights)
	{
		for (int i = 0; i < weights.Length; i++) //iterate all layers
		{
			for (int j = 0; j < weights [i].Length; j++) //iterate all neurons
			{
				for(int k = 0; k < weights [i][j].Length; k++) //iterate all weights connected to the neuron
				{
					weights[i][j][k] = copyWeights[i][j][k]; //sets every weight to the copyweight

				}//end for loop(k)
			}//end for loop(j)
		}//end for loop(i)
	}//end function copyweights

	public float[] feedForward(float[] inputs)
	{
		//this method basically needs to give the neurons in the hidden layer the inputs , then per neuron in the hidden layer a value needs to be computed- which is the 
		//sum of sum of all weight connections of the neuron + weight values connected to it from the previous layer, then normalize that value by using tanh() which will make it a value
		//between -1 and 1. After the activated value is passed (returned) to the neurons in the output layer, those will say basically if the value that just came to me is greater than (or = to) 0
		//then flap, and if its less than 0 then dont flap

		//value = sum of all weight connections of this neuron + weight values in previous layer
		//sum of all weights connected to the neuron: value = (float)Math.Tanh(inputs[0]);


		//return the last layer of neurons (output layer)



		for (int i = 0; i < inputs.Length; i++) 
		{
			neurons [0] [i] = inputs [i]; //give the input to the first layer in the neuron matrix (hence why the first layer is the input layer)
		}//end for loop

		for (int i = 1; i < layers.Length; i++)//iterate over every layer starting from the second layer
		{ 
			for (int j = 0; j < neurons [i].Length; j++) //iterate over every neuron in the layer
			{

				float value = 0f;//value that is going to be computed from the sum of all the neuron values in the previous layer with their weights

				for (int k = 0; k < neurons [i - 1].Length; k++)  //iterate over the neurons in the previous layer for the neuron that is targeted
				{
					//we need to get the weight at i-1 because we start from the second layer at [j] which is the current neuron at [k].
					//then we need to multiply this the value in the previous neuron (neurons at i-1 at k)
					value += weights [i - 1] [j] [k] * neurons [i - 1] [k]; //sum of all weight connections of this neuron + weight values in previous layer
				}//end for loop(k)

				//we need to place the value we just calculated back after apply an activation to it.
				//(hyperbolic tangent) activation which converts value to be between negative 1 and 1
				neurons[i][j] = (float)Math.Tanh(value);

			}//end for loop(j)
		}//end for loop(i)

		return neurons[neurons.Length-1];//returns last layer which is the output layer


	}
	public void Mutate()
	{
		for (int i = 0; i < weights.Length; i++) //iterate all layers
		{
			for (int j = 0; j < weights [i].Length; j++) //iterate all neurons
			{
				for(int k = 0; k < weights [i][j].Length; k++) //iterate all weights connected to the neuron
				{
					float weight = weights[i][j][k];

					//mutate weight value
					float randomNumber = UnityEngine.Random.Range(0f,100f);//random # 1 - 1000 , mutations are applied based on this #

					// 0.8% chance a mutation will occur
					//each mutation has a 0.2% chance

					//4 different types of mutations are applied
					if(randomNumber <= 2f) //if 1, flip sign of the weight
					{

						weight *= -1f;
					}
					else if(randomNumber <= 4f) //if 2, pick random weight between -1 and 1
					{
						weight = UnityEngine.Random.Range(-0.5f, 0.5f);
					}
					else if(randomNumber <= 6f) //if 3, randomly increase by 0% to 100%
					{
						float factor = UnityEngine.Random.Range(0f, 1f) + 1f;
						weight *= factor;
					}
					else if(randomNumber <= 8f) //if 4, randomly decrease by 0% to 100%
					{
						float factor = UnityEngine.Random.Range(0f, 1f);
						weight*= factor;
					}

					weights[i][j][k] = weight;
				}//end for loop(k)

			}//end for loop(j)

		}//end for loop (i)

	}//end function Mutate

	public void AddFitness(float fit)
	{
		fitness += fit;
	}//end function addFitness

	public void SetFitness(float fit)
	{
		fitness = fit;
	}//end function setFitness

	public float GetFitness()
	{
		return fitness;
	}//end function getFitness

	public int CompareTo(NeuralNetwork other)//sorts neural networks in ASCENDING order
	{
		if(other == null) return 1;

		if(fitness > other.fitness)
			return 1;
		else if(fitness < other.fitness)
			return -1;
		else
			return 0;
	}//end function CompareTo

}
