public class Node
{
    public float value;
    public float[] weights;
    public float error;
    public float dvalue;

    public Node(float value, float[] weights, float error, float dvalue)
    {
        this.value = value;
        this.weights = weights;
        this.error = error;
        this.dvalue = dvalue;
    }
}
