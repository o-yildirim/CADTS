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

        registerButtonImage.color = Color.gray;
    }

    public void switchToLogin()
    {
        if (!loginPanel.activeSelf)
        {
            AuthenticationManager.instance.setStatus("");
            registerButtonImage.color = Color.gray;
            loginButtonImage.color = defaultButtonColor;

            registerPanel.SetActive(false);
            loginPanel.SetActive(true);
        }
    }

    public void switchToRegister()
    {
        if (!registerPanel.activeSelf)
        {
            AuthenticationManager.instance.setStatus("");
            loginButtonImage.color = Color.gray;
            registerButtonImage.color = defaultButtonColor;
            

            loginPanel.SetActive(false);
            registerPanel.SetActive(true);
            
        }
    }

}
