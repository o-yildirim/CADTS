using UnityEngine;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{

    public GameObject quitCanvas;
    public GameObject accountSettingsCanvas;
    public GameObject applicationSettingsCanvas;
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

    public GameObject screenPanel;
    public Dropdown screenModeDropdown;
    public Dropdown screenResolutionDropdown;
    public Button submitScreenSettings;
    public Button closeScreenButton;

    public GameObject audioPanel;



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

        applicationSettingsCanvas = GameObject.Find("ApplicationSettingsMenu");
        applicationSettingsCanvas.transform.Find("Panel/Screen").GetComponent<Button>().onClick.AddListener(ScreenSettings);
        //applicationSettingsCanvas.transform.Find("Panel/Audio").GetComponent<Button>().onClick.AddListener(AudioSettings);
        applicationSettingsCanvas.transform.Find("Panel/Close").GetComponent<Button>().onClick.AddListener(() => { CloseCanvas(applicationSettingsCanvas); });



        operationCanvas = GameObject.Find("OperationCanvas");

        if (operationCanvas != null)
        {
            InitializeScreenSettingsCanvas();
            //InitializeAudioSettingsCanvas();
            InitializeChangePasswordCanvas();
            InitializeDeleteAccountCanvas();
        }

        CloseAllCanvasses();

    }

    private void CloseAllCanvasses()
    {
        CloseCanvas(applicationSettingsCanvas);
        //CloseCanvas(audioPanel);
        CloseCanvas(screenPanel);
        CloseCanvas(accountSettingsCanvas);
        CloseCanvas(operationCanvas);
        CloseCanvas(changePasswordPanel);
        CloseCanvas(deleteAccountPanel);
    }

    public void InitializeChangePasswordCanvas()
    {
        changePasswordPanel = operationCanvas.transform.Find("ChangePasswordPanel").gameObject;

        if (changePasswordPanel != null)
        {
            oldPw1 = changePasswordPanel.transform.Find("Old Password").GetComponent<InputField>();
            oldPw2 = changePasswordPanel.transform.Find("Old Password 2").GetComponent<InputField>();
            newPw = changePasswordPanel.transform.Find("New Password").GetComponent<InputField>();
            newPw2 = changePasswordPanel.transform.Find("New Password 2").GetComponent<InputField>();
            submit = changePasswordPanel.transform.Find("Submit").GetComponent<Button>();
            warningText = changePasswordPanel.transform.Find("Warning Text").GetComponent<Text>();
            closeButton = changePasswordPanel.transform.Find("Close").GetComponent<Button>();
            closeButton.onClick.AddListener(() => { CloseCanvas(changePasswordPanel); CloseCanvas(operationCanvas); OpenCanvas(accountSettingsCanvas); });
        }
    }

    public void InitializeScreenSettingsCanvas()
    {
        screenPanel = operationCanvas.transform.Find("ScreenPanel").gameObject;

        if (screenPanel != null)
        {
            screenModeDropdown = screenPanel.transform.Find("Screen Mode Dropdown").GetComponent<Dropdown>();
            screenResolutionDropdown = screenPanel.transform.Find("Screen Resolution Dropdown").GetComponent<Dropdown>();

            submitScreenSettings = screenPanel.transform.Find("Submit").GetComponent<Button>();
            closeScreenButton = screenPanel.transform.Find("Close").GetComponent<Button>();
            closeScreenButton.onClick.AddListener(() => { ResolutionManager.ResetSelected();  CloseCanvas(screenPanel); CloseCanvas(operationCanvas); OpenCanvas(applicationSettingsCanvas); });
            submitScreenSettings.onClick.AddListener(() => { ResolutionManager.ApplySettings(); });

            ResolutionManager.InitializeResolutions();
            ResolutionManager.InitializeModesToDropdown(screenModeDropdown);
            ResolutionManager.InitializeResolutionsToDropdown(screenResolutionDropdown);
        }
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
        if (deleteAccountPanel != null)
        {
            pw1 = deleteAccountPanel.transform.Find("Password").GetComponent<InputField>();
            pw2 = deleteAccountPanel.transform.Find("Password 2").GetComponent<InputField>();
            submitButton = deleteAccountPanel.transform.Find("Submit").GetComponent<Button>();
            warningTextForDel = deleteAccountPanel.transform.Find("Warning Text").GetComponent<Text>();
            checkBox = deleteAccountPanel.transform.Find("CheckBox").GetComponent<Button>();
            checkImage = checkBox.transform.Find("CheckImage").GetComponent<Image>();
            closeButtonForDel = deleteAccountPanel.transform.Find("Close").GetComponent<Button>();


            checkBox.onClick.AddListener(() => { ManageCheckBox(); });
            submitButton.onClick.AddListener(() => { DeleteAccountFromDatabase(); });
            closeButtonForDel.onClick.AddListener(() => { CloseCanvas(deleteAccountPanel); CloseCanvas(operationCanvas); OpenCanvas(accountSettingsCanvas); });
        }
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

    public void ScreenSettings()
    {
        //submit.onClick.AddListener(ResolutionManager.ApplySettings);
        screenResolutionDropdown.value = ResolutionManager.GetSelectedResIndex();
        Debug.Log(screenResolutionDropdown.value.ToString());
        screenModeDropdown.value = ResolutionManager.GetSelectedModeIndex();
        Debug.Log(screenModeDropdown.value.ToString());
 

        CloseCanvas(applicationSettingsCanvas);
        OpenCanvas(operationCanvas);
        OpenCanvas(screenPanel);
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