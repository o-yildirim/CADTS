using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StringManager : MonoBehaviour
{
    public static StringManager instance;
    string x;

    string[] characters = { "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z" };
    void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else if(instance != this)
        {
            Destroy(gameObject);
        }
    }


    public string generateString(int length) //Length uzunluğunda rasgele string üretiyor. NOT: Karakterler tekrar edebiliyor.
    {
        string generated = "";

        for (int i = 0; i < length; i++)
        {
            int randomIndex = (int)Random.Range(0, characters.Length);
            generated = generated + characters[randomIndex];
        }

        return generated;
    }

    public string assignRandomUppercase(string stringToUse) //Stringin içindeki rasgele bir karakteri uppercase yapıyor.
    {


        int randomIndex = (int)Random.Range(0, stringToUse.Length);

        char uppercasedLetter = char.ToUpper(stringToUse[randomIndex]);
  
        stringToUse = stringToUse.Substring(0, randomIndex) + uppercasedLetter + stringToUse.Substring(randomIndex+1, stringToUse.Length-randomIndex-1);

        return stringToUse;

    }

}
