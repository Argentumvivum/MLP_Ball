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
                nodes[0][i].value = input.data[i - 1];
            }

            //Hidden Layers
            for (int i = 1; i < nodes.Count - 1; i++)
            {
                for (int j = 1; j < nodes[i].Count; j++)
                {
                    var tempSum = 0.0f;

                    for (int k = 0; k < nodes[i - 1].Count; k++)
                    {
                        tempSum += nodes[i - 1][k].value * nodes[i - 1][k].weights[j - 1];
                    }

                    nodes[i][j].value = activation.SoftSign(tempSum);
                    nodes[i][j].dvalue = activation.SoftSignDerivative(tempSum);
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
                        tempSum += nodes[i - 1][k].value * nodes[i - 1][k].weights[j];
                    }

                    nodes[i][j].value = activation.SoftSign(tempSum);
                    nodes[i][j].dvalue = activation.SoftSignDerivative(tempSum);
                    nodes[i][j].error = input.label[j] - nodes[i][j].value;
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
                for (int k = 0; k < nodes[i][j].weights.Length; k++)
                {
                    nodes[i][j].error += nodes[i][j].weights[k] * nodes[i + 1][k].error;
                }
            }
        }

        //Calculate error for other Hidden Layers if they exist
        for (int i = 1; i < nodes.Count - 2; i++)
        {
            for (int j = 1; j < nodes[i].Count; j++)
            {
                for (int k = 1; k < nodes[i][j].weights.Length; k++)
                {
                    nodes[i][j].error += nodes[i][j].weights[k] * nodes[i + 1][k].error;
                }
            }
        }

        //Change weights for input and n Hidden Layers
        for (int i = 0; i < nodes.Count - 2; i++)
        {
            for (int j = 0; j < nodes[i].Count; j++)
            {
                for (int k = 0; k < nodes[i][j].weights.Length; k++)
                {
                    if (j == 0)
                    {
                        nodes[i][j].weights[k] += learningRate * 
                                                  nodes[i + 1][k + 1].error * 
                                                  nodes[i + 1][k + 1].dvalue;
                    }
                    else
                    {
                        nodes[i][j].weights[k] += learningRate * 
                                                  nodes[i + 1][k + 1].error * 
                                                  nodes[i + 1][k + 1].dvalue * 
                                                  nodes[i][j].value;
                    }
                }
            }
        }

        //Change weights for second to last Layer
        for (int i = nodes.Count - 2; i < nodes.Count - 1; i++)
        {
            for (int j = 0; j < nodes[i].Count; j++)
            {
                for (int k = 0; k < nodes[i][j].weights.Length; k++)
                {
                    if (j == 0)
                    {
                        nodes[i][j].weights[k] += learningRate * 
                                                  nodes[i + 1][k].error * 
                                                  nodes[i + 1][k].dvalue;
                    }
                    else
                    {
                        nodes[i][j].weights[k] += learningRate * 
                                                  nodes[i + 1][k].error * 
                                                  nodes[i + 1][k].dvalue * 
                                                  nodes[i][j].value;
                    }
                }
            }
        }

    }
}
