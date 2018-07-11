using UnityEngine;

public class ActivationFunction
{

    public float SoftSign(float x)
    {
        return x / (1 + Mathf.Abs(x));
    }

    public float Sigmoid(float x)
    {
        return x / (1 + Mathf.Exp(-x));
    }

    public float ReLu(float x)
    {
        return x < 0 ? 0 : x;
    }

    public float TanH(float x)
    {
        var exp2X = Mathf.Exp(2 * x);
        return (exp2X - 1) / (exp2X + 1);
    }

    public float SoftSignDerivative(float x)
    {
        return 1 / Mathf.Pow((1 + Mathf.Abs(x)), 2);
    }

    public float SigmoidDerivative(float x)
    {
        var sig = Sigmoid(x);
        return sig * (1 - sig);
    }

    public float ReLuDerivative(float x)
    {
        return x < 0 ? 0 : 1;
    }

    public float TanHDerivative(float x)
    {
        return 1 - Mathf.Pow(TanH(x), 2);
    }
}
