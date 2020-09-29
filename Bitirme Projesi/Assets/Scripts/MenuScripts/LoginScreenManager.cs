using UnityEngine;
using UnityEngine.UI;


public class LoginScreenManager : MonoBehaviour
{
    public GameObject loginPanel;
    public GameObject registerPanel;

    public Button loginPanelButton;
    public Button registerPanelButton;

    private Image loginButtonImage;
    private Image registerButtonImage;

    public Color defaultButtonColor;
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
            loginButtonImage.color = Color.gray;
            registerButtonImage.color = defaultButtonColor;
            

            loginPanel.SetActive(false);
            registerPanel.SetActive(true);
            
        }
    }

}
