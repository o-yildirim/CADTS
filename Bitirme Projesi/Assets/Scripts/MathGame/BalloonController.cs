using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BalloonController : MonoBehaviour
{
    [SerializeField]
    public string mathOperator;
    [SerializeField]
    public int firstNum, secondNum;
    public Text operationTxt;
    public float answer = -1111;
    public static BalloonController instance;

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
        string[] operatorArr = { "+", "-", "/", "X" };
        mathOperator = operatorArr[Random.Range(0, operatorArr.Length)];
        firstNum = Random.Range(1, 9);
        secondNum = Random.Range(1, 9);

        if (mathOperator == "/")
            firstNum = checkDivision(mathOperator, firstNum, secondNum);

        findAnswer(mathOperator, firstNum, secondNum);
        string firstStr = firstNum.ToString();
        string secondStr = secondNum.ToString();
        operationTxt.text = firstStr + "\n" + mathOperator + "\n" + secondStr;
    }

    public int checkDivision(string mathOperator, int firstNum, int secondNum)
    {
        if (firstNum % secondNum != 0)
            firstNum = secondNum * Random.Range(1, 9);
        return firstNum;
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
