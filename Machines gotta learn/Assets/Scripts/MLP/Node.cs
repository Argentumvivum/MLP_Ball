public class Node
{
    public float Value { get; set; }
    public float[] Weights { get; set; }
    public float Error { get; set; }
    public float Dvalue { get; set; }

    public Node(float value, float[] weights, float error, float dvalue)
    {
        Value = value;
        Weights = weights;
        Error = error;
        Dvalue = dvalue;
    }
}
