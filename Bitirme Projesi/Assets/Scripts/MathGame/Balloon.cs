using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Balloon : MonoBehaviour
{
    public GameObject exp;
    public string mathOperator;
    public int firstNum, secondNum;
    public Text operationTxt;
    public float answer = -1111;
    public float velocity;
    void Start()
    {
        velocity = GetComponent<Rigidbody2D>().velocity.y;
        operationTxt = GetComponentInChildren<Text>();
        createOperation(mathOperator, firstNum, secondNum);
        Destroy(gameObject, 30f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Barrier")
        {
            MathGameController.instance.health--;
            MathGameController.instance.checkHealth();
            MathGameController.instance.balloons.Remove(this);
            Instantiate(exp, new Vector2(gameObject.transform.position.x, gameObject.transform.position.y + 1), Quaternion.identity);
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
