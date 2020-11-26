using UnityEngine;
using UnityEngine.UI;


public class LoginScreenManager : MonoBehaviour
{
    public static LoginScreenManager instance;

    public GameObject loginPanel;
    public GameObject registerPanel;

    public Button loginPanelButton;
    public Button registerPanelButton;

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

}
