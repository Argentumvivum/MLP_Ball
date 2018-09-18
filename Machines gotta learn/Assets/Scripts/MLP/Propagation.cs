using System.Collections.Generic;

public class Propagation
{

    private ActivationFunction activation = new ActivationFunction();

    public void FeedForward(Dictionary<int,List<Node>> nodes, List<InputData> inputData, bool isTraining, float learningRate)
    {
        foreach (var input in inputData)
        {
            
            //Input Layer
            for (int i = 1; i < nodes[0].Count; i++)
            {
                nodes[0][i].Value = input.Data[i - 1];
            }

            //Hidden Layers
            for (int i = 1; i < nodes.Count - 1; i++)
            {
                for (int j = 1; j < nodes[i].Count; j++)
                {
                    var tempSum = 0.0f;

                    for (int k = 0; k < nodes[i - 1].Count; k++)
                    {
                        tempSum += nodes[i - 1][k].Value * nodes[i - 1][k].Weights[j - 1];
                    }

                    nodes[i][j].Value = activation.SoftSign(tempSum);
                    nodes[i][j].Dvalue = activation.SoftSignDerivative(tempSum);
                }
            }

            //Output Layer
            for (int i = nodes.Count - 1; i < nodes.Count; i++)
            {
                for (int j = 0; j < nodes[i].Count; j++)
                {
                    var tempSum = 0.0f;

                    for (int k = 0; k < nodes[i - 1].Count; k++)
                    {
                        tempSum += nodes[i - 1][k].Value * nodes[i - 1][k].Weights[j];
                    }

                    nodes[i][j].Value = activation.SoftSign(tempSum);
                    nodes[i][j].Dvalue = activation.SoftSignDerivative(tempSum);
                    nodes[i][j].Error = input.Label[j] - nodes[i][j].Value;
                }
            }

            if (isTraining)
                BackPropagation(nodes, inputData, learningRate);
        }
    }

    private void BackPropagation(Dictionary<int, List<Node>> nodes, List<InputData> inputData, float learningRate)
    {
        //Calculate error for Second to last Layer
        for (int i = nodes.Count - 2; i < nodes.Count - 1; i++)
        {
            for (int j = 1; j < nodes[i].Count; j++)
            {
                for (int k = 0; k < nodes[i][j].Weights.Length; k++)
                {
                    nodes[i][j].Error += nodes[i][j].Weights[k] * nodes[i + 1][k].Error;
                }
            }
        }

        //Calculate error for other Hidden Layers if they exist
        for (int i = 1; i < nodes.Count - 2; i++)
        {
            for (int j = 1; j < nodes[i].Count; j++)
            {
                for (int k = 1; k < nodes[i][j].Weights.Length; k++)
                {
                    nodes[i][j].Error += nodes[i][j].Weights[k] * nodes[i + 1][k].Error;
                }
            }
        }

        //Change weights for input and n Hidden Layers
        for (int i = 0; i < nodes.Count - 2; i++)
        {
            for (int j = 0; j < nodes[i].Count; j++)
            {
                for (int k = 0; k < nodes[i][j].Weights.Length; k++)
                {
                    if (j == 0)
                    {
                        nodes[i][j].Weights[k] += learningRate * 
                                                  nodes[i + 1][k + 1].Error * 
                                                  nodes[i + 1][k + 1].Dvalue;
                    }
                    else
                    {
                        nodes[i][j].Weights[k] += learningRate * 
                                                  nodes[i + 1][k + 1].Error * 
                                                  nodes[i + 1][k + 1].Dvalue * 
                                                  nodes[i][j].Value;
                    }
                }
            }
        }

        //Change weights for second to last Layer
        for (int i = nodes.Count - 2; i < nodes.Count - 1; i++)
        {
            for (int j = 0; j < nodes[i].Count; j++)
            {
                for (int k = 0; k < nodes[i][j].Weights.Length; k++)
                {
                    if (j == 0)
                    {
                        nodes[i][j].Weights[k] += learningRate * 
                                                  nodes[i + 1][k].Error * 
                                                  nodes[i + 1][k].Dvalue;
                    }
                    else
                    {
                        nodes[i][j].Weights[k] += learningRate * 
                                                  nodes[i + 1][k].Error * 
                                                  nodes[i + 1][k].Dvalue * 
                                                  nodes[i][j].Value;
                    }
                }
            }
        }

    }
}
