using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Transform ball;
    public Transform goal;
    public Transform plane;

    public Text rotationX;
    public Text rotationZ;
    public Button gatherDataButton;
    public Color toggleOn;
    public Color toggleOff;
    public GameObject scrollContent;
    public Button trainNetwork;
    public Button addData;

    public int[] networkSizes;

    public List<InputData> inputDatas;
    public List<InputData> currentData;

    public bool isTraining;
    public float learningRate;
    public float planeTiltMultiplier;
    public float sensitivity;

    private bool gatheringData;
    private string tempScrollText;
    private SavingSystem savingSystem;
    private NeuralNetwork neuralNetwork;
    private Dictionary<int, List<Node>> nodes;

    private void Awake()
    {
        inputDatas = new List<InputData>();
        savingSystem = new SavingSystem();
        nodes = new Dictionary<int, List<Node>>();
        neuralNetwork = new NeuralNetwork();

        gatheringData = true;
        trainNetwork.interactable = false;

        tempScrollText = string.Empty;

        rotationX.color = Color.green;
        rotationZ.color = Color.blue;

        var cb = gatherDataButton.colors;
        cb.normalColor = toggleOn;
        cb.highlightedColor = toggleOn;
        cb.pressedColor = toggleOn;
        gatherDataButton.colors = cb;

        inputDatas = savingSystem.LoadData(inputDatas, false);
    }

    private void Start()
    {
        neuralNetwork.SetupNetwork(networkSizes, nodes);
    }

    public void ResetData()
    {
        inputDatas = new List<InputData>();
        inputDatas = savingSystem.LoadData(inputDatas, true);

        scrollContent.GetComponent<Text>().text = string.Empty;
        savingSystem.SaveData(inputDatas);
    }

    public void AddData()
    {
        var data = new float[] { ball.position.x, ball.position.z, goal.position.x, goal.position.z };
        var label = new float[] { 0, 0 };

        var diff = (goal.position - ball.position).normalized;
        diff *= sensitivity;
        diff.y = 1;

        var target = Quaternion.FromToRotation(plane.up, diff);
        plane.rotation = Quaternion.Slerp(plane.rotation, target, Time.deltaTime * 30.0f);

        var eulerRotation = plane.rotation.eulerAngles;

        if (eulerRotation.z > 300)
            eulerRotation.z = -360 + eulerRotation.z;

        if (eulerRotation.x > 300)
            eulerRotation.x = -360 + eulerRotation.x;

        label = new float[] { Mathf.Clamp(eulerRotation.x, -1, 1), Mathf.Clamp(eulerRotation.z, -1, 1) };

        inputDatas.Add(new InputData
        {
            data = data,
            label = label
        });

        savingSystem.SaveData(inputDatas);
    }

    public void TrainNetwork()
    {
        isTraining = true;
        neuralNetwork.TrainMLPSpftSign(nodes, inputDatas, isTraining, learningRate);
        isTraining = false;
    }

    public void Randomize()
    {
        ball.position = new Vector3(Random.Range(-24f, 24f), 1, Random.Range(-24f, 24f));
    }

    public void GatheringDataToggle()
    {
        gatheringData = !gatheringData;

        if(gatheringData)
        {
            var cb = gatherDataButton.colors;
            cb.normalColor = toggleOn;
            cb.highlightedColor = toggleOn;
            cb.pressedColor = toggleOn;
            gatherDataButton.colors = cb;

            trainNetwork.interactable = false;
            addData.interactable = true;

            ball.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePosition;
        }
        else
        {
            var cb = gatherDataButton.colors;
            cb.normalColor = toggleOff;
            cb.highlightedColor = toggleOff;
            cb.pressedColor = toggleOff;
            gatherDataButton.colors = cb;

            plane.rotation = Quaternion.Euler(0, 0, 0);

            trainNetwork.interactable = true;
            addData.interactable = false;

            ball.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        }
    }

    private void Update()
    {
        if(nodes.Count > 0)
        {
            rotationX.text = "X rot: " + nodes[nodes.Count - 1][0].value.ToString("0.##");
            rotationZ.text = "Z rot: " + nodes[nodes.Count - 1][1].value.ToString("0.##");
        }

        if(inputDatas.Count > 0)
        {
            tempScrollText = string.Empty;

            for (int i = 0; i < inputDatas.Count; i++)
            {
                tempScrollText += $" Data {i + 1} (Ball X: {inputDatas[i].data[0]} Ball Z: {inputDatas[i].data[1]} Goal X: {inputDatas[i].data[2]} Goal Z: {inputDatas[i].data[3]}) | Rot X: {inputDatas[i].label[0].ToString("0.##")} Rot Z: {inputDatas[i].label[1].ToString("0.##")}\n";
            }

            scrollContent.GetComponent<Text>().text = tempScrollText;
        }

        if (gatheringData)
        {
            var diff = (goal.position - ball.position).normalized;
            diff *= sensitivity;
            diff.y = 1;

            var target = Quaternion.FromToRotation(plane.up, diff);
            plane.rotation = Quaternion.Slerp(plane.rotation, target, Time.deltaTime * 30.0f);
        }

        if(!isTraining && !gatheringData)
        {
            currentData = new List<InputData>();

            var data = new float[] { ball.position.x, ball.position.z, goal.position.x, goal.position.z };
            var label = new float[] { 0, 0 };

            var eulerRotation = plane.rotation.eulerAngles;

            currentData.Add(new InputData
            {
                data = data,
                label = label
            });

            neuralNetwork.MLPSoftSign(nodes, currentData, isTraining, learningRate);

            var target = Quaternion.Euler(nodes[nodes.Count -1][0].value * planeTiltMultiplier, 1, nodes[nodes.Count - 1][1].value * planeTiltMultiplier);
            plane.rotation = Quaternion.Slerp(plane.rotation, target, Time.deltaTime * 30.0f);
        }
    }
}
