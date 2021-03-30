using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class SettingsManager : MonoBehaviour
{

    public GameObject quitCanvas;
    private GameObject accountSettingsCanvas;
    private GameObject applicationSettingsCanvas;
    private GameObject operationCanvas;

    private GameObject changePasswordPanel;
    private InputField oldPw1;
    private InputField oldPw2;
    private InputField newPw;
    private InputField newPw2;
    private Button submit;
    private Text warningText;
    private Button closeButton;

    private GameObject deleteAccountPanel;
    private InputField pw1;
    private InputField pw2;
    private Button checkBox;
    private Image checkImage;
    private Button submitButton;
    private Button closeButtonForDel;
    private Text warningTextForDel;
    private bool approvedToDeleteStatistics = false;

    private GameObject screenPanel;
    public Dropdown screenModeDropdown;
    public Dropdown screenResolutionDropdown;
    private Button submitScreenSettings;
    private Button closeScreenButton;

    private GameObject audioPanel;
    private Slider volumeSlider;
    public AudioMixer audioMixer;
    private AudioSource sampleSource;
    


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
        sampleSource = GetComponent<AudioSource>();

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
        applicationSettingsCanvas.transform.Find("Panel/Audio").GetComponent<Button>().onClick.AddListener(OpenAudioSettings);
        applicationSettingsCanvas.transform.Find("Panel/Close").GetComponent<Button>().onClick.AddListener(() => { CloseCanvas(applicationSettingsCanvas); });



        operationCanvas = GameObject.Find("OperationCanvas");

        if (operationCanvas != null)
        {
            InitializeScreenSettingsCanvas();
            InitializeAudioSettingsCanvas();
            InitializeChangePasswordCanvas();
            InitializeDeleteAccountCanvas();
        }

        CloseAllCanvasses();

    }

    public void CloseAllCanvasses()
    {
        CloseCanvas(applicationSettingsCanvas);
        CloseCanvas(audioPanel);
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
            closeScreenButton.onClick.AddListener(() => { CloseCanvas(screenPanel); CloseCanvas(operationCanvas); OpenCanvas(applicationSettingsCanvas); });
            submitScreenSettings.onClick.AddListener(() => { ResolutionManager.ApplySettings(); });

            ResolutionManager.InitializeResolutions();
            ResolutionManager.InitializeModesToDropdown(screenModeDropdown);
            ResolutionManager.InitializeResolutionsToDropdown(screenResolutionDropdown);
        }
    }

    public void InitializeAudioSettingsCanvas()
    {
        audioPanel = operationCanvas.transform.Find("AudioPanel").gameObject;

        if (audioPanel != null)
        {
            sampleSource = audioPanel.GetComponent<AudioSource>();

            volumeSlider = audioPanel.transform.Find("Volume Slider").GetComponent<Slider>();
            float currentVolume = PlayerPrefs.GetFloat("volume");
            volumeSlider.value = currentVolume;
            audioMixer.SetFloat("volume", currentVolume);
            volumeSlider.onValueChanged.AddListener(SetVolume);

           
            
            closeScreenButton = audioPanel.transform.Find("Close").GetComponent<Button>();
            closeScreenButton.onClick.AddListener(() => { CloseCanvas(audioPanel); CloseCanvas(operationCanvas); OpenCanvas(applicationSettingsCanvas); });
    


        }
    }

    public void OpenAudioSettings()
    {
        CloseCanvas(applicationSettingsCanvas);
        OpenCanvas(operationCanvas);
        OpenCanvas(audioPanel);
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
                    PlayerPrefs.DeleteKey("username");
                    PlayerPrefs.DeleteKey("password");
                    PlayerPrefs.DeleteKey("remember_me");
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

        //screenResolutionDropdown.value = PlayerPrefs.GetInt("ResolutionIndex");
        //screenModeDropdown.value = PlayerPrefs.GetInt("ModeIndex");

        //screenModeDropdown.RefreshShownValue();
        //screenResolutionDropdown.RefreshShownValue();

        //Text tempText = screenPanel.transform.Find("Title").GetComponent<Text>();
        //tempText.text = screenResolutionDropdown.value.ToString();

        CloseCanvas(applicationSettingsCanvas);
        OpenCanvas(operationCanvas);
        OpenCanvas(screenPanel);
    } 

    public void SetVolume(float volume)
    {
        audioMixer.SetFloat("volume", volume);
        PlayerPrefs.SetFloat("volume", volume);
        if (!sampleSource.isPlaying) sampleSource.Play();
        //sampleSource.Play();

        // Debug.Log(volume);
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