using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AuthenticationManager : MonoBehaviour
{
    // Start is called before the first frame update
    public InputField registerName;
    public InputField registerSurname;
    public InputField dateOfBirth;
    public InputField registerEmail;
    public InputField registerPassword;


    public InputField email;
    public InputField password;


    public Text status;

    public void register()
    {
        var registeredUser = new User(registerName.text,registerSurname.text,dateOfBirth.text,registerEmail.text, registerPassword.text);
        DatabaseHandler.PostUser(registeredUser, registerEmail.text, () =>
        {
            DatabaseHandler.GetUser(registerEmail.text, user =>
            {
                //Debug.Log($"{user.username}'s mail is {user.email} and password is {user.password}");
            });

            /* DatabaseHandler.GetUsers(users =>
             {
                 foreach (var user in users)
                 {
                     Debug.Log($"{user.Value.username} {user.Value.email} {user.Value.password}");
                 }
             });
         });*/
        });
    }

    public void login()
    {
        if (string.IsNullOrEmpty(email.text) || string.IsNullOrEmpty(password.text))
        {
            status.text = "Username or password cannot be empty";
            return;
        }

        DatabaseHandler.GetUser(email.text, user =>
        {
            Debug.Log($"Your e-mail is {user.email} and password is {user.password}");

            if (user.password.Equals(password.text))
            {
                SceneManagement.instance.loadSceneCall();
            }
            else if (user.password.Equals(password.text))
            {
                status.text = "Password is incorrect";
            }
        });
    }
}