using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProblemSolvingGameManager : MonoBehaviour
{
    public bool fullyLinked = false;
    public bool inputUnavailable = false;

    public GameObject sink;
    public GameObject finish;
    public GameObject valve;

    public FlowingWaterManager waterManagerScript;
    public MapGenerator mapGenerator;
    public Valve valveScript;

    public List<GameObject> pipesPassedTrough;

    public GameObject gameOverCanvas;



    public static ProblemSolvingGameManager instance;
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
       
    }
    private void Start()
    {
        pipesPassedTrough = new List<GameObject>();
        waterManagerScript = GetComponent<FlowingWaterManager>();
        valveScript = valve.GetComponentInChildren<Valve>();
        mapGenerator = GetComponent<MapGenerator>();

        //gameOverCanvas.transform.GetChild(0).transform.GetChild(0).GetComponent<Button>().onClick.AddListener(SceneManagement.instance.loadMainMenu);
        gameOverCanvas.GetComponentInChildren<Button>().onClick.AddListener(SceneManagement.instance.loadMainMenu);
    }

    public void startCheckingSequence()
    {
        inputUnavailable = true;
        pipesPassedTrough.Add(sink);

        RaycastHit hitTile;
        if(Physics.Raycast(sink.transform.position,-sink.transform.up,out hitTile))
        {
            Tile hitTileScript = hitTile.transform.GetComponent<Tile>();
            if(hitTileScript != null)
            {
                hitTileScript.checkTransmission(0);
            }
        }
        StartCoroutine(initiateWater());
       
    }
    public IEnumerator initiateWater()
    {
        Coroutine valveRotating = StartCoroutine(valveScript.Rotate(2f));
        Coroutine waterFlowing =StartCoroutine(waterManagerScript.DrawWaterSlow(pipesPassedTrough, valveRotating));
        yield return waterFlowing;
        if (!fullyLinked)
        {
            pipesPassedTrough.Clear();
            waterManagerScript.ResetElements();
            inputUnavailable = false;
        }
        else
        {
            finishGame();
        }

    }
    public void finishGame()
    {
        inputUnavailable = true;
        //BURADA ISTATISTIK MANAGERDEN FALAN METOD CAGIRILIR
        gameOverCanvas.SetActive(true);
    }
}
