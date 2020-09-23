using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    private string withUppercase;
    private string withoutUppercase;

    public Text leftText;
    public Text rightText;

    void Start()
    {
        //Zaman tutulacak
        newQuestion();
    }

    private void Update()
    {
        //Kullanıcıdan input alınacak
    }

    public void newQuestion()
    {
        int length = (int)Random.Range(1, 7);
        withoutUppercase = StringManager.instance.generateString(length);
        withUppercase = StringManager.instance.generateString(length);

        withUppercase = StringManager.instance.assignRandomUppercase(withUppercase);

        int randomTextIndex = (int)Random.Range(0, 2);

        if (randomTextIndex == 0)
        {
            leftText.text = withUppercase;
            rightText.text = withoutUppercase;
            //O zaman doğru cevap sol olacak
        }
        else
        {
            rightText.text = withUppercase;
            leftText.text = withoutUppercase;
            //Doğru cevap sağ olacak
        }


    }

}

