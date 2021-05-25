using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProblemSolvingGameManager : MonoBehaviour
{
    public int levelCount = 2;
    public int currentLevel = 0;
    public int[,] levelRowColumns = { {4,7}, {5,8}, { 5, 12 } };


    public bool fullyLinked = false;
    public bool inputUnavailable = false;

    public GameObject sink;
    public GameObject finish;
    public GameObject valve;

    public FlowingWaterManager waterManagerScript;
    public MapGenerator mapGenerator;
    public Valve valveScript;

    public List<GameObject> pipesPassedTrough;

    public bool inTutorial = false;
    public GameObject tutorialMap;
    public GameObject tutorialCanvas;
    public Text tutorialText;

    public GameObject endGameButtonCanvas;
    public Text levelText;

    private Coroutine isWaterDrawn;

    public GameObject tutorialArrow;
    public Vector3[] arrowPositions;

    public float tutorialTextTime = 8.5f;

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

        endGameButtonCanvas.GetComponentInChildren<Button>().onClick.AddListener(SceneManagement.instance.loadMainMenu);

        StartCoroutine(Tutorial());
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
       isWaterDrawn = StartCoroutine(initiateWater());
       
    }
    public IEnumerator initiateWater()
    {
        Coroutine valveRotating = StartCoroutine(valveScript.Rotate());
        Coroutine waterFlowing = StartCoroutine(waterManagerScript.DrawWaterSlow(pipesPassedTrough, valveRotating));        
        yield return waterFlowing;

      

        if (!fullyLinked)
        {
            pipesPassedTrough.Clear();
            waterManagerScript.ResetElements();
            waterManagerScript.waterSoundSource.Stop();
            FlowStatisticManager.instance.IncrementWrongAttempt();
            inputUnavailable = false;
        }
        else
        {
            if (!inTutorial)
            {
                FlowStatisticManager.instance.pathCostByPipes[currentLevel] = pipesPassedTrough.Count - 2;
                if (currentLevel == levelCount)
                {
                    FinishGame();
                }
                else
                {
                    FlowStatisticManager.instance.timerOn = false;                   
                    currentLevel++;
                    ResetLevel();
                    StartGame();

                } 
            }
            else
            {
                inputUnavailable = false;
            }
            
        }
  
    }
    public void FinishGame()
    {
        waterManagerScript.waterSoundSource.volume /= 2f;
        inputUnavailable = true;
        FlowStatisticManager.instance.timerOn = false;
        endGameButtonCanvas.SetActive(false);

        FlowStatisticManager.instance.EvaluateValues();
        FlowStatisticManager.instance.DisplayStatisticPanel();
        FlowStatisticManager.instance.InitializeStatisticObject();
        FlowStatisticManager.instance.InsertToDatabase();
      
    }

    public void StartGame()
    {
        mapGenerator.rows = levelRowColumns[currentLevel, 0];
        mapGenerator.columns = levelRowColumns[currentLevel, 1];
        mapGenerator.mapGameObject = new GameObject("Map");
        
        levelText.text = (currentLevel+1) + ". seviye";

        mapGenerator.InitializeMapValues();
        mapGenerator.PositionSink();
        mapGenerator.PositionFinish();
        mapGenerator.PositionValve();
        mapGenerator.CreateCorners();
        mapGenerator.GenerateSolution();
        mapGenerator.MixSolution();
        mapGenerator.GenerateMap();
        mapGenerator.RepositionCamera();
        endGameButtonCanvas.SetActive(true);
        inputUnavailable = false;
        FlowStatisticManager.instance.timerOn = true;
    }

    public IEnumerator Tutorial()
    {
        inputUnavailable = true;
        inTutorial = true;

        SphereCollider valveCollider = valve.GetComponent<SphereCollider>();
        BoxCollider sinkCollider = sink.GetComponent<BoxCollider>();
        valveCollider.enabled = sinkCollider.enabled = false;

        for (int j =1; j < tutorialMap.transform.childCount; j++) 
        {
            BoxCollider collider = tutorialMap.transform.GetChild(j).GetComponent<BoxCollider>();
            if(collider != null)
            {
                collider.enabled = false;
            }
        }

        tutorialMap.SetActive(true);
        tutorialCanvas.SetActive(true);
        tutorialCanvas.GetComponentInChildren<Button>().onClick.AddListener(SkipTutorial);

        tutorialText.text = "Oyunun amacı, yukarıdaki kalın borudan aşağıdakine suyu ulaştıracak doğru yolu hazırlayabilmektir.";
        yield return new WaitForSeconds(tutorialTextTime);

        tutorialText.text = "Oyunda üç seviye bulunmaktadır, her seviyede boru sayısı artmaktadır.";
        yield return new WaitForSeconds(tutorialTextTime);

        tutorialText.text = "Borulara tıklayarak saat yönünde dönmelerini, suyun gireceği ve akacağı yönlerini değiştirmelerini\nsağlayabilirsiniz.";
        yield return new WaitForSeconds(tutorialTextTime);

        tutorialText.text = "Yönlendirmeyi doğru yaptığınızı düşünüyorsanız, vanaya tıklayarak suyun akmasını sağlayabilirsiniz.";
        yield return new WaitForSeconds(tutorialTextTime);

        tutorialText.text = "Okun gösterdiği boruyu döndürmek için tıklayın!";
       

        int i = 0;
        tutorialArrow.transform.position = arrowPositions[i];
        tutorialArrow.SetActive(true);


        Tile tileToRotate = tutorialMap.transform.GetChild(0).GetComponent<Tile>();
        int currentStateOfTile = tileToRotate.currentState;

        inputUnavailable = false;

        while (tileToRotate.currentState != (currentStateOfTile + 1) % tileToRotate.totalStates)
        {
            yield return null;
        }

        for (int j = 1; j < tutorialMap.transform.childCount; j++) 
        {
            BoxCollider collider = tutorialMap.transform.GetChild(j).GetComponent<BoxCollider>();
            if (collider != null)
            {
                collider.enabled = true;
            }
        }
        valveCollider.enabled = sinkCollider.enabled = true;

        tutorialArrow.SetActive(false);
        tutorialText.text = "Harika!";
        yield return new WaitForSeconds(1.8f);

        tutorialText.text = "Artık iki kalın boru arasındaki yol sağlandı. Suyu açmak için vanaya tıklayın!";
        i++;
        tutorialArrow.transform.position = arrowPositions[i];
        tutorialArrow.SetActive(true);

        while (!fullyLinked)
        {
            yield return null;
        }

        yield return isWaterDrawn;
        fullyLinked = false;
        tutorialArrow.SetActive(false);

        tutorialText.text = "\t\t\tHarika!\nOyuna başlamak için herhangi bir tuşa basın.";
        while (!Input.anyKeyDown)
        {
            yield return null;
        }

        valveScript.audioSource.Stop();
        inTutorial = false;       
        tutorialCanvas.SetActive(false);
        pipesPassedTrough.Clear();
        waterManagerScript.waterSoundSource.Stop();
        waterManagerScript.ResetElements();
        Destroy(tutorialMap);
        FlowStatisticManager.instance.ResetAttributes();

        StartGame();
        

    }

    public void SkipTutorial()
    {
        fullyLinked = false;

        valve.GetComponent<SphereCollider>().enabled = true;
        sink.GetComponent<BoxCollider>().enabled = true;

        StopAllCoroutines();
        waterManagerScript.StopAllCoroutines();
        waterManagerScript.waterSoundSource.Stop();
        valveScript.StopAllCoroutines();

        valveScript.audioSource.Stop();
        pipesPassedTrough.Clear();
        waterManagerScript.ResetElements();
        FlowStatisticManager.instance.ResetAttributes();

    
        Destroy(tutorialMap);
        tutorialCanvas.SetActive(false);
        inputUnavailable = false;
        inTutorial = false;
       
        
        StartGame();
    }

    public void ResetLevel()
    {
        StopAllCoroutines();
        waterManagerScript.StopAllCoroutines();
        waterManagerScript.waterSoundSource.Stop();
        valveScript.StopAllCoroutines();
        

        pipesPassedTrough.Clear();
        waterManagerScript.ResetElements();       
        Destroy(mapGenerator.mapGameObject);
        fullyLinked = false;

    }

   
}
