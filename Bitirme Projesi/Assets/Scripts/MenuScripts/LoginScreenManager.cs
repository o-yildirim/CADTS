using UnityEngine;
using UnityEngine.UI;


public class LoginScreenManager : MonoBehaviour
{
    public static LoginScreenManager instance;

    public GameObject loginPanel;
    public GameObject registerPanel;

    public Button loginPanelButton;
    public Button registerPanelButton;
    public Button rememberMeButton;

    private Image loginButtonImage;
    private Image registerButtonImage;

    public Color defaultButtonColor;
    public Color selectedColor;


    private void Awake()
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
    private void Start()
    {
        loginButtonImage = loginPanelButton.GetComponent<Image>();
        registerButtonImage = registerPanelButton.GetComponent<Image>();

        defaultButtonColor = registerButtonImage.color;

        if (PlayerPrefs.HasKey("remember_me"))
        {
            GameObject checkImage = rememberMeButton.transform.GetChild(0).gameObject;

            int rememberMe = PlayerPrefs.GetInt("remember_me");
            if (rememberMe == 0)
            {
                checkImage.SetActive(false);
            }
            else
            {
                AuthenticationManager.instance.email.text = PlayerPrefs.GetString("username");
                AuthenticationManager.instance.password.text = PlayerPrefs.GetString("password");
                checkImage.SetActive(true);
            }
        }
    }

    public void switchToLogin()
    {
        if (!loginPanel.activeSelf)
        {
            AuthenticationManager.instance.setStatus("");
            registerButtonImage.color = defaultButtonColor;
            loginButtonImage.color = selectedColor;

            registerPanel.SetActive(false);
            loginPanel.SetActive(true);
        }
    }

    public void switchToRegister()
    {
        if (!registerPanel.activeSelf)
        {
            AuthenticationManager.instance.setStatus("");
            loginButtonImage.color = defaultButtonColor;
            registerButtonImage.color = selectedColor;
            

            loginPanel.SetActive(false);
            registerPanel.SetActive(true);
            
        }
    }

    public void callQuitRequest()
    {
        SettingsManager.instance.QuitRequest();
    }


    public void manageRememberMe()
    {
        AuthenticationManager.instance.rememberMe = !AuthenticationManager.instance.rememberMe;

        GameObject checkImage = rememberMeButton.transform.GetChild(0).gameObject;
        checkImage.SetActive(!checkImage.activeSelf);

    }


}
