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

    public bool inTutorial = false;
    public GameObject tutorialMap;
    public GameObject tutorialCanvas;
    public Text tutorialText;

    private Coroutine isWaterDrawn;

    public GameObject tutorialArrow;
    public Vector3[] arrowPositions;


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
        Coroutine valveRotating = StartCoroutine(valveScript.Rotate(2f));
        Coroutine waterFlowing =StartCoroutine(waterManagerScript.DrawWaterSlow(pipesPassedTrough, valveRotating));        
        yield return waterFlowing;

      

        if (!fullyLinked)
        {
            pipesPassedTrough.Clear();
            waterManagerScript.ResetElements();
            FlowStatisticManager.instance.IncrementWrongAttempt();
            inputUnavailable = false;
        }
        else
        {
            if (!inTutorial)
            {
                FinishGame();
            }
            else
            {
               // pipesPassedTrough.Clear();
               // waterManagerScript.ResetElements();
                inputUnavailable = false;
            }
            
        }
  
    }
    public void FinishGame()
    {
        inputUnavailable = true;
        FlowStatisticManager.instance.timerOn = false;

        FlowStatisticManager.instance.EvaluateValues();
        FlowStatisticManager.instance.DisplayStatisticPanel();
        FlowStatisticManager.instance.InitializeStatisticObject();
        FlowStatisticManager.instance.InsertToDatabase();
      
    }

    public void StartGame()
    {
        mapGenerator.PositionSink();
        mapGenerator.PositionFinish();
        mapGenerator.PositionValve();
        mapGenerator.CreateCorners();
        mapGenerator.GenerateSolution();
        mapGenerator.MixSolution();
        mapGenerator.GenerateMap();
        mapGenerator.RepositionCamera();
        inputUnavailable = false;
        FlowStatisticManager.instance.timerOn = true;
    }

    public IEnumerator Tutorial()
    {
        inputUnavailable = true;
        inTutorial = true;
        for(int j =1; j < tutorialMap.transform.childCount; j++) //0 dan başlatmamamın sebebi ilk çocuğun collideri aktif olmasını istiyorum onu döndürecek
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
        yield return new WaitForSeconds(8.5f);

        tutorialText.text = "Borulara tıklayarak saat yönünde dönmelerini, suyun gireceği ve akacağı yönlerini değiştirmelerini\nsağlayabilirsiniz.";
        yield return new WaitForSeconds(8.5f);

        tutorialText.text = "Yönlendirmeyi doğru yaptığınızı düşünüyorsanız, vanaya tıklayarak suyun akmasını sağlayabilirsiniz.";
        yield return new WaitForSeconds(7.5f);

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

        for (int j = 1; j < tutorialMap.transform.childCount; j++) //0 dan başlatmamamın sebebi ilk çocuğun collideri aktif olmasını istiyorum onu döndürecek
        {
            BoxCollider collider = tutorialMap.transform.GetChild(j).GetComponent<BoxCollider>();
            if (collider != null)
            {
                collider.enabled = true;
            }
        }

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

        
        inTutorial = false;       
        //tutorialMap.SetActive(false);
        Destroy(tutorialMap);
        tutorialCanvas.SetActive(false);


        pipesPassedTrough.Clear();
        waterManagerScript.ResetElements();

        FlowStatisticManager.instance.tilesRotationCounts.Clear();

        StartGame();
        

    }

    public void SkipTutorial()
    {
        //StopCoroutine(Tutorial());
        StopAllCoroutines();
        waterManagerScript.StopAllCoroutines();
        valveScript.StopAllCoroutines();

        pipesPassedTrough.Clear();
        waterManagerScript.ResetElements();
        FlowStatisticManager.instance.tilesRotationCounts.Clear();

        //tutorialMap.SetActive(false);
        Destroy(tutorialMap);
        tutorialCanvas.SetActive(false);
        inputUnavailable = false;
        inTutorial = false;
        fullyLinked = false;
        
        StartGame();
    }

   

}
