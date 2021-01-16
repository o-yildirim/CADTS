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

    private bool inTutorial = false;
    public GameObject tutorialMap;
    public GameObject tutorialCanvas;
    public Text tutorialText;

    private Coroutine isWaterDrawn;

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
                pipesPassedTrough.Clear();
                waterManagerScript.ResetElements();
                inputUnavailable = false;
            }
            
        }
  
    }
    public void FinishGame()
    {
        inputUnavailable = true;
        //BURADA ISTATISTIK MANAGERDEN FALAN METOD CAGIRILIR
        gameOverCanvas.SetActive(true);
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
    }

    public IEnumerator Tutorial()
    {
        inputUnavailable = true;
        inTutorial = true;

        tutorialMap.SetActive(true);
        tutorialCanvas.SetActive(true);
        tutorialCanvas.GetComponentInChildren<Button>().onClick.AddListener(SkipTutorial);

        tutorialText.text = "Oyunun amacı, yukarıdaki yeşil borudan aşağıdakine\nsuyu ulaştıracak doğru yolu hazırlayabilmektir.";
        yield return new WaitForSeconds(5f);

        tutorialText.text = "Borulara tıklayarak saat yönünde dönmelerini,\nsuyun gireceği ve akacağı yönlerini değiştirmelerini\nsağlayabilirsiniz.";
        yield return new WaitForSeconds(5f);

        tutorialText.text = "Yönlendirmeyi doğru yaptığınızı düşünüyorsanız, vanaya\ntıklayarak suyun akmasını sağlayabilirsiniz.";
        yield return new WaitForSeconds(5f);

        tutorialText.text = "Şimdi deneyin!";
        inputUnavailable = false;

        while (!fullyLinked)
        {
            yield return null;
        }

        yield return isWaterDrawn;
        fullyLinked = false;

        tutorialText.text = "\tHarika!\nOyuna başlamak için herhangi bir tuşa basın.";
        while (!Input.anyKeyDown)
        {
            yield return null;
        }

        
        inTutorial = false;       
        tutorialMap.SetActive(false);
        tutorialCanvas.SetActive(false);


        pipesPassedTrough.Clear();
        waterManagerScript.ResetElements();
        
     

        StartGame();
        

    }

    public void SkipTutorial()
    {
        StopCoroutine(Tutorial());
        tutorialMap.SetActive(false);
        tutorialCanvas.SetActive(false);
        inputUnavailable = false;
        inTutorial = false;
        
        StartGame();
    }

   

}
