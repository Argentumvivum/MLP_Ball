using System.Collections.Generic;
using UnityEngine;

public class NeuralNetwork
{
    private Propagation propagation = new Propagation();

    public void MLPSoftSign(Dictionary<int, List<Node>> nodes, List<InputData> inputData, bool isTraining, float learningRate)
    {
        propagation.FeedForward(nodes, inputData, isTraining, learningRate);
    }

    public void TrainMLPSoftSign(Dictionary<int, List<Node>> nodes, List<InputData> inputData, bool isTraining, float learningRate)
    {
        for (int i = 0; i < 20000; i++)
        {
            inputData.Shuffle();
            propagation.FeedForward(nodes, inputData, isTraining, learningRate);
        }
    }

    //Prepare network
    public void SetupNetwork(int[] networkSizes, Dictionary<int, List<Node>> nodes)
    {
        var tempNodes = new List<Node>();

        for (int i = 0; i < networkSizes.Length; i++)
        {
            //last layer
            if(i == networkSizes.GetUpperBound(0))
            {
                for(int j = 0; j < networkSizes[i]; j++)
                {
                    tempNodes.Add(new Node(1, new float[0], 0, 0));
                }
            }
            //second to last layer
            else if (i == networkSizes.GetUpperBound(0) - 1)
            {
                for (int j = 0; j < networkSizes[i]; j++)
                {
                    tempNodes.Add(new Node(1, new float[networkSizes[i + 1]], 0, 0));
                }
            }
            //other layers
            else
            {
                for(int j = 0; j < networkSizes[i]; j++)
                {
                    tempNodes.Add(new Node(1, new float[networkSizes[i + 1] - 1], 0, 0));
                }
            }
        }
        
        OrganizeNodes(networkSizes, nodes, tempNodes);
        RandomizeWeights(nodes);
    }

    //Divide nodes into layers
    private void OrganizeNodes(int[] networkSizes, Dictionary<int, List<Node>> nodes, List<Node> tempNodes)
    {
        var tempIndex = 0;
        
        for (int i = 0; i < networkSizes.Length; i++)
        {
            nodes.Add(i, tempNodes.GetRange(tempIndex, networkSizes[i]));

            tempIndex += networkSizes[i];
        }
    }

    private void RandomizeWeights(Dictionary<int, List<Node>> nodes)
    {
        for(int i = 0; i < nodes.Count; i++)
        {
            for(int j = 0; j < nodes[i].Count; j++)
            {
                for(int k = 0; k < nodes[i][j].weights.Length; k++)
                {
                    nodes[i][j].weights[k] = SetRandomValue();
                }
            }
        }
    }

    public float SetRandomValue()
    {
        return Random.Range(-2f, 2f);
    }
}
