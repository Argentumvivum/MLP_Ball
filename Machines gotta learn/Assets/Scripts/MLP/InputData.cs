using System.Collections.Generic;

[System.Serializable]
public class InputData
{
    public float[] data;//ball position x,z (input), target postition x,z (input)
    public float[] label;//desired plane rotation x,z (label)
}

[System.Serializable]
public class InputDataList
{
    public List<InputData> data;
}
