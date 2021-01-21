using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{

    public GameObject quitCanvas;

    public GameObject operationCanvas;
    public Text titleText;
    public InputField oldPw1;
    public InputField oldPw2;
    public InputField newPw;
    public Button submit;
    public Text warningText;
    public Button closeButton;



    //public GameObject applicationSettingsPanel


    public static SettingsManager instance;
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != null)
        {
            Destroy(gameObject);
        }

    }

    public void Start()
    {
        quitCanvas = gameObject.transform.GetChild(0).gameObject;
    }
    public void QuitRequest()
    {
        Time.timeScale = 0f;
        quitCanvas.SetActive(true);

    }
    public void RejectQuit()
    {
        quitCanvas.SetActive(false);
        Time.timeScale = 1f;
    }

    public void QuitApplication()
    {
        Application.Quit();
    }

    public void LogOut()
    {
        DatabaseHandler.loggedInUser = null;
        //TOKENI YOKET
        SceneManagement.instance.loadLoginScreen();
    }


    public void InitializePanels()
    {
        if (operationCanvas == null)
        {
            operationCanvas = GameObject.Find("OperationCanvas");
            Debug.Log(operationCanvas.transform.name);
            if (operationCanvas != null)
            {

                titleText = operationCanvas.transform.Find("Panel/Title").GetComponent<Text>();
                oldPw1 = operationCanvas.transform.Find("Panel/Old Password").GetComponent<InputField>();
                oldPw2 = operationCanvas.transform.Find("Panel/Old Password 2").GetComponent<InputField>();
                newPw = operationCanvas.transform.Find("Panel/New Password").GetComponent<InputField>();
                submit = operationCanvas.transform.Find("Panel/Submit").GetComponent<Button>();
                warningText = operationCanvas.transform.Find("Panel/Warning Text").GetComponent<Text>();
                closeButton = operationCanvas.transform.Find("Panel/Close").GetComponent<Button>();



                DesktopManager temp = new DesktopManager();

                closeButton.onClick.AddListener(() => { temp.CloseCanvas(operationCanvas); });
            }

        }
    }

    public void ChangePassword()
    {
        titleText.text = "Şifre değiştir";
        submit.onClick.AddListener(InsertNewPassword);
        operationCanvas.SetActive(true);
    }

    public void InsertNewPassword()
    {
        string enteredOldPw1 = oldPw1.text;
        string enteredOldPw2 = oldPw2.text;
        string enteredNewPw = newPw.text;

        string hashedPwd = AuthenticationManager.GetMD5HashString(enteredOldPw1);
        string hashedNewPassword = AuthenticationManager.GetMD5HashString(enteredNewPw); //**

        if (string.IsNullOrEmpty(enteredOldPw1) || string.IsNullOrEmpty(enteredOldPw2) || string.IsNullOrEmpty(enteredNewPw))
        {
            warningText.text = "Şifreler boş olamaz.";
            return;
        }
        else
        {
           if (!enteredOldPw1.Equals(enteredOldPw2))
            //if (1 == 0)
            {
                Debug.Log("İlki: " + oldPw1.text);
                Debug.Log("İkincisi: " + oldPw2.text);
                warningText.text = "Girilen eski şifreler uyuşmuyor.";
            }
            else
            {
                //Eski sifreyi hashle databasedeki ile uyusuyor mu bak
                string emailEncoded = AuthenticationManager.instance.encode(DatabaseHandler.loggedInUser.email);
                
                DatabaseHandler.GetUser(emailEncoded, user =>
                {
                    if (user.password.Equals(hashedPwd))
                    {
                        User newUser = new User(user.name, user.surname, user.dob, emailEncoded, hashedNewPassword);
                        Debug.Log(newUser.name + " " + newUser.surname + " " + newUser.dob + " " + " " + newUser.email + " " + newUser.password);
                        DatabaseHandler.PostUser(newUser, emailEncoded, () => { });
                        //DatabaseHandler.registerUser(newUser, newUser.email, user2 => { });
                        /*******************************************************************************************************************
                        *******************************************************************************************************************
                                     BURAYA YENI hashedNewPassword U KAYITLI EMAILIN SIFRESI OLACAK KOD GELECEK
                        *******************************************************************************************************************
                        *******************************************************************************************************************
                        *********************************************************************************************************************/
                    }
                    else if (!user.password.Equals(hashedPwd))
                    {
                        warningText.text = "Girilen şifre yanlış.";
                    }

                });
            }



        }



    }

    public void DeleteAccount() { }
}