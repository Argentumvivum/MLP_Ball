using System.Collections.Generic;

[System.Serializable]
public class InputData
{
    public float[] Data { get; set; }//ball position x,z (input), target postition x,z (input)
    public float[] Label { get; set; }//desired plane rotation x,z (label)
}

[System.Serializable]
public class InputDataList
{
    public List<InputData> Data { get; set; }
}
