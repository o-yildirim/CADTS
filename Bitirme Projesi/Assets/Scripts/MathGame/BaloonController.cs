using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BaloonController : MonoBehaviour
{
    [SerializeField]
    public string mathOperator;
    [SerializeField]
    public int firstNum, secondNum;
    public Text operationTxt;
    public float answer = -1111;
    public static BaloonController instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        operationTxt = GetComponentInChildren<Text>();
        createOperation(mathOperator, firstNum, secondNum);
        Destroy(gameObject, 8f);
    }

    void Update()
    {
        transform.position = new Vector2(transform.position.x, transform.position.y + 0.005f);
    }
    /*
    private void OnMouseDown()
    {
        if (gameObject.CompareTag("Baloon")) {
            Destroy(gameObject);
        }
    }*/

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Barrier")
        {
            MathGameController.instance.health--;
            Destroy(gameObject); 
        }
    }

    public void createOperation(string mathOperator, int firstNum, int secondNum)
    {
        string[] operatorArr = {"+","-","/","X" };
        mathOperator = operatorArr[Random.Range(0, operatorArr.Length)];
        firstNum = Random.Range(1, 9);
        secondNum = Random.Range(1, 9);
        findAnswer(mathOperator, firstNum, secondNum);
        string firstStr = firstNum.ToString();
        string secondStr = secondNum.ToString();
        operationTxt.text = firstStr + "\n" + mathOperator + "\n" + secondStr;
    }

    public void findAnswer(string mathOperator, int firstNum, int secondNum)
    {
        switch (mathOperator)
        {
            case "+":
                answer = firstNum + secondNum;
                break;
            case "-":
                answer = firstNum - secondNum;
                break;
            case "/":
                answer = firstNum / secondNum;
                break;
            case "X":
                answer = firstNum * secondNum;
                break;
            default:
                break;
        }
    }
}
