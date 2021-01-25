using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{

    public GameObject quitCanvas;
    public GameObject accountSettingsCanvas;
    public GameObject operationCanvas;

    public GameObject changePasswordPanel;
    public InputField oldPw1;
    public InputField oldPw2;
    public InputField newPw;
    public InputField newPw2;
    public Button submit;
    public Text warningText;
    public Button closeButton;

    public GameObject deleteAccountPanel;
    public InputField pw1;
    public InputField pw2;
    public Button checkBox;
    public Image checkImage;
    public Button submitButton;
    public Button closeButtonForDel;
    public Text warningTextForDel;
    public bool approvedToDeleteStatistics = false;




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

        accountSettingsCanvas = GameObject.Find("AccountSettingsMenu");
        accountSettingsCanvas.transform.Find("Panel/ChangePassword").GetComponent<Button>().onClick.AddListener(ChangePassword);
        accountSettingsCanvas.transform.Find("Panel/DeleteAccount").GetComponent<Button>().onClick.AddListener(DeleteAccount);
        accountSettingsCanvas.transform.Find("Panel/Close").GetComponent<Button>().onClick.AddListener(() => { CloseCanvas(accountSettingsCanvas); });



        operationCanvas = GameObject.Find("OperationCanvas");
        Debug.Log(operationCanvas.transform.name);
        if (operationCanvas != null)
        {
            InitializeChangePasswordCanvas();
            InitializeDeleteAccountCanvas();
        }



        //applicationSettingsCanvas.SetActive(false);
        CloseCanvas(accountSettingsCanvas);
        CloseCanvas(operationCanvas);
        CloseCanvas(changePasswordPanel);
        CloseCanvas(deleteAccountPanel);


    }

    public void InitializeChangePasswordCanvas()
    {
        changePasswordPanel = operationCanvas.transform.Find("ChangePasswordPanel").gameObject;

        oldPw1 = operationCanvas.transform.Find("ChangePasswordPanel/Old Password").GetComponent<InputField>();
        oldPw2 = operationCanvas.transform.Find("ChangePasswordPanel/Old Password 2").GetComponent<InputField>();
        newPw = operationCanvas.transform.Find("ChangePasswordPanel/New Password").GetComponent<InputField>();
        newPw2 = operationCanvas.transform.Find("ChangePasswordPanel/New Password 2").GetComponent<InputField>();
        submit = operationCanvas.transform.Find("ChangePasswordPanel/Submit").GetComponent<Button>();
        warningText = operationCanvas.transform.Find("ChangePasswordPanel/Warning Text").GetComponent<Text>();
        closeButton = operationCanvas.transform.Find("ChangePasswordPanel/Close").GetComponent<Button>();
        closeButton.onClick.AddListener(() => { CloseCanvas(changePasswordPanel); CloseCanvas(operationCanvas); OpenCanvas(accountSettingsCanvas); });
    }

    public void ChangePassword()
    {

        submit.onClick.AddListener(InsertNewPassword);
        CloseCanvas(accountSettingsCanvas);
        OpenCanvas(operationCanvas);
        OpenCanvas(changePasswordPanel);

    }

    public void InsertNewPassword()
    {
        string enteredOldPw1 = oldPw1.text;
        string enteredOldPw2 = oldPw2.text;
        string enteredNewPw1 = newPw.text;
        string enteredNewPw2 = newPw2.text;

        string hashedPwd = AuthenticationManager.GetMD5HashString(enteredOldPw1);
        string hashedNewPassword = AuthenticationManager.GetMD5HashString(enteredNewPw1); //**

        if (string.IsNullOrWhiteSpace(enteredOldPw1) || string.IsNullOrWhiteSpace(enteredOldPw2) || string.IsNullOrWhiteSpace(enteredNewPw1) || string.IsNullOrWhiteSpace(enteredNewPw2))
        {
            warningText.text = "Şifreler boş olamaz.";
        }
        else if (!enteredOldPw1.Equals(enteredOldPw2))
        {
            warningText.text = "Girilen eski şifreler uyuşmuyor.";
        }
        else if (!enteredNewPw1.Equals(enteredNewPw2))
        {
            warningText.text = "Girilen yeni şifreler uyuşmuyor.";
        }
        else
        {
            string emailEncoded = AuthenticationManager.instance.encode(DatabaseHandler.loggedInUser.email);

            DatabaseHandler.GetUser(emailEncoded, user =>
            {
                if (user.password.Equals(hashedPwd))
                {
                    User newUser = new User(user.name, user.surname, user.dob, emailEncoded, hashedNewPassword);
                    Debug.Log(newUser.name + " " + newUser.surname + " " + newUser.dob + " " + " " + newUser.email + " " + newUser.password);
                    warningText.text = "Şifre başarıyla değiştirildi.";
                    DatabaseHandler.PostUser(newUser, emailEncoded, () => { });
                }
                else if (!user.password.Equals(hashedPwd))
                {
                    warningText.text = "Girilen şifre yanlış.";
                }

            });
        }



    

    }

    public void DeleteAccount()
    {
        submit.onClick.AddListener(DeleteAccountFromDatabase);
        CloseCanvas(accountSettingsCanvas);
        OpenCanvas(operationCanvas);
        OpenCanvas(deleteAccountPanel);
    }


    public void InitializeDeleteAccountCanvas()
    {
        deleteAccountPanel = operationCanvas.transform.Find("DeleteAccountPanel").gameObject;

        pw1 = operationCanvas.transform.Find("DeleteAccountPanel/Password").GetComponent<InputField>();
        pw2 = operationCanvas.transform.Find("DeleteAccountPanel/Password 2").GetComponent<InputField>();
        submitButton = operationCanvas.transform.Find("DeleteAccountPanel/Submit").GetComponent<Button>();
        warningTextForDel = operationCanvas.transform.Find("DeleteAccountPanel/Warning Text").GetComponent<Text>();

        checkBox = operationCanvas.transform.Find("DeleteAccountPanel/CheckBox").GetComponent<Button>();
        checkImage = checkBox.transform.Find("CheckImage").GetComponent<Image>();

        closeButtonForDel = operationCanvas.transform.Find("DeleteAccountPanel/Close").GetComponent<Button>();


        checkBox.onClick.AddListener(() => { ManageCheckBox(); } );
        submitButton.onClick.AddListener(() => { DeleteAccountFromDatabase(); });
        closeButtonForDel.onClick.AddListener(() => { CloseCanvas(deleteAccountPanel); CloseCanvas(operationCanvas); OpenCanvas(accountSettingsCanvas); });
    }

    public void DeleteAccountFromDatabase()
    {
        string enteredPw1 =  pw1.text;
        string enteredPw2 =  pw2.text;

        

        if (string.IsNullOrWhiteSpace(enteredPw1) || string.IsNullOrWhiteSpace(enteredPw2))
        {
            warningTextForDel.text = "Şifreler boş olamaz.";
        }
        else if (!enteredPw1.Equals(enteredPw2))
        {
            warningTextForDel.text = "Girilen şifreler uyuşmuyor.";
        }
        else
        {
            string emailEncoded = AuthenticationManager.instance.encode(DatabaseHandler.loggedInUser.email);
            string hashedPwd = AuthenticationManager.GetMD5HashString(enteredPw1);

            DatabaseHandler.GetUser(emailEncoded, user =>
            {
                if (user.password.Equals(hashedPwd))
                {
                    DatabaseHandler.DeleteUser(DatabaseHandler.loggedInUser, approvedToDeleteStatistics);
                }
                else if (!user.password.Equals(hashedPwd))
                {
                    warningTextForDel.text = "Girilen şifre yanlış.";
                }

            });
        }

       

    }

    public void ManageCheckBox()
    {
        approvedToDeleteStatistics = !approvedToDeleteStatistics;
        checkImage.enabled = !checkImage.enabled;
   
    }


    public void OpenCanvas(GameObject canvasToOpen)
    {
        canvasToOpen.SetActive(true);
    }
    public void CloseCanvas(GameObject canvasToClose)
    {
        canvasToClose.SetActive(false);
    }
}