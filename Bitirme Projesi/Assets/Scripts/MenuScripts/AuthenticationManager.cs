using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Google.Apis.Auth.OAuth2;
using Newtonsoft.Json;
using System;
using Proyecto26;

public class AuthenticationManager : MonoBehaviour
{
    public static AuthenticationManager instance;
    // Start is called before the first frame update
    public InputField registerName;
    public InputField registerSurname;
    public InputField dateOfBirth;
    public InputField registerEmail;
    public InputField registerPassword;


    public InputField email;
    public InputField password;


    public Text status;

    void Start()
    {
        createTokenAsync();
    }

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
        StartCoroutine(ActivateOnTimer());
    }

    private IEnumerator ActivateOnTimer()
    {
        while (true)
        {
            yield return new WaitForSeconds(3000); // 50 dakikada bir yeni token üretsin
            createTokenAsync();
        }
    }

    public async System.Threading.Tasks.Task createTokenAsync() //visual studio kendi önerdi, değiştirilebilir
    {
        try
        {
            string json = @"{
                                ""type"": ""service_account"",
                                ""project_id"": ""bitirme-projesi-df6c6"",
                                ""private_key_id"": ""40044593dbabd1b8d2e2713a360dd891e3415687"",
                                ""private_key"": ""-----BEGIN PRIVATE KEY-----\nMIIEvgIBADANBgkqhkiG9w0BAQEFAASCBKgwggSkAgEAAoIBAQDzVolAgyYZlruQ\nxb5PwVOn1KEqbkiVrJQ36cyBkD0RoZQLvle6U/gW1FYaNRd8tb+p15zV+ir75wsW\naFd9RJAWha1qBjwAAOw68giCYdYHN9chBbClTTHO6qNDx8YUhuOPR/Fkx3wAcP0A\ntqql8R1SiBZdkPUn+PH1pGAY+VYYRW6rHq5LTanmUvcI3XD1qQiqW4Rn90PmwwRq\nbJjqwClC/z6eNNkvE8c47aGCIsAmj22fPMxw7zBPyTmL2uAk95HGuBmzGdVXH8ec\nFimIuI3BlnKK8flOKSrsNl2CHf4UyWL8beMdugGeSxTfWdn4MqkMg9L2JSkaBORv\nESs0+dv1AgMBAAECggEAaSKU54IihoDJJlpBiRz6w3MLdRU0GDL50RSbTslekU0R\n9Wb2aWNZN6fjUaxpqvCt0Dh/ozXUt4SSFkrbrhxe8tQr4jiyWMANUMR857RUsEaw\nlKxsL2dkY0WeZ60bSIglLqVDRysnSNuIfHPv1AWSiTOSEjgMrAoTpHGyMWgdChux\n8dJ7Bpkzi3sXI5kvTlOxc+9lN37+oFLPWG+9c9uQwhsqm3Yf+bfwMd/3nKt2GwND\nM83QZD1iaSA5VvfFkUfNUsAfbnrDTFXvuOk4dLcjVK82naPkUqNvRgy7DvlAKDwM\njpenmi+2IrZYhHuJMGAQ+Wvet+FcWVw1xZx5kedLOwKBgQD5nn39K7bxt3Ek46EY\niblKpLuhmJnmrzHkr6ugJ7ku6eXzU15byBzJPuL/HsFofAVIwrfQfdcAP50W5jft\nN7S166zxdH9dGo2DLhOWLOaYWQsl3/vsha26LChyG/WWIoBmDfEtE6Yv6mLfku+N\nRli1eE3nU8HyMjS4WuxGg5bA+wKBgQD5jvDL3/zmPaPqinSmJHC9tFIClL21UY5H\nbkZ8R7eUNeKFmwQGfNApbxSEKI8WW929UHzvmV+v4B90psryKGiRXKono0usy6SP\nq5DtWBukaWuz+wA6/J+tcB5T86Dcx1fUpxj95gc2jj4Tz+8XjqFBv1ZocDbCBGQl\nEIMNtVyjzwKBgBzydEnXt+9wBn7wps3hqsstL1UeDTrjNjX+6Tg5YxA9r5z4Hgkm\nu6/ayxgqyOME/tfhPM/AyB4PQnhVWkb4Hsy59+RITjzNx3te7IaNzm/8Y36Q6vYS\nyoK8fDQ/actvVNSA6WcA0FTeKXkj4QPRFJh2yxH4dJVRnSuD38KNn/3hAoGBALWf\nIgA+c3/dPH5lZZ/ExYhKw6S9O5PKVxIFX6bzStfCn6k8uCFAlP6SqQvFuN083pP5\nD/QQW2NUaZAsE2dJoVCpb+yJG3oWex1Ub5VMmlF9p7TE59YGJO/EuU2/8UK8kV9i\nfP3Le1RpESS6H/e1BgcjiW9Yu4FYUMnoyngfldxhAoGBANwhZS7TcSmOIWTbdHxU\n1ETY29FHLO5yVkL7EFAOeveGsl2DYTCB8BMNRDLAYG6xn7qEhrOATLZhSXcmm21G\nl1CsFPix3fgqxfeaK+MbuIygCgAwils7+Lm6PXOyKEpnSZgVe+GmaS9VJeONv4/7\nhsCwTSE7v2nBf3oDsBK0gqcc\n-----END PRIVATE KEY-----\n"",
                                ""client_email"": ""firebase-adminsdk-itguz@bitirme-projesi-df6c6.iam.gserviceaccount.com"",
                                ""client_id"": ""116539589345835353869"",
                                ""auth_uri"": ""https://accounts.google.com/o/oauth2/auth"",
                                ""token_uri"": ""https://oauth2.googleapis.com/token"",
                                ""auth_provider_x509_cert_url"": ""https://www.googleapis.com/oauth2/v1/certs"",
                                ""client_x509_cert_url"": ""https://www.googleapis.com/robot/v1/metadata/x509/firebase-adminsdk-itguz%40bitirme-projesi-df6c6.iam.gserviceaccount.com""
                            }
    "; 
                var cr = JsonConvert.DeserializeObject<ServiceAccount>(json); // service account credential
            //Debug.Log("Email: " + cr.client_email + "PK: " + cr.private_key);
            // Create an explicit ServiceAccountCredential credential
            var xCred = new ServiceAccountCredential(new ServiceAccountCredential.Initializer(cr.client_email)
            {
                Scopes = new[] {
                    "https://www.googleapis.com/auth/userinfo.email",
                    "https://www.googleapis.com/auth/firebase.database",
                }
            }.FromPrivateKey(cr.private_key));
            string accessToken = await xCred.GetAccessTokenForRequestAsync();
            RestClient.DefaultRequestHeaders["Authorization"] = "Bearer " + accessToken;
            Debug.Log("Access token: " + accessToken);
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
        }
    }

    public void register()
    {
        var registeredUser = new User(registerName.text,registerSurname.text,dateOfBirth.text,registerEmail.text, registerPassword.text);
        string email = encode(registerEmail.text); //**
        DatabaseHandler.PostUser(registeredUser, email, () =>
        {/*
            DatabaseHandler.GetUser(registerEmail.text, user =>
            {
                //Debug.Log($"{user.username}'s mail is {user.email} and password is {user.password}");
            });*/

            /* DatabaseHandler.GetUsers(users =>
             {
                 foreach (var user in users)
                 {
                     Debug.Log($"{user.Value.username} {user.Value.email} {user.Value.password}");
                 }
             });
         });*/
            LoginScreenManager.instance.switchToLogin();
            //status.text = "Başarıyla kayıt olundu.";
        });
    }

    public void login()
    {
        if (string.IsNullOrEmpty(email.text) || string.IsNullOrEmpty(password.text))
        {
            status.text = "Username or password cannot be empty";
            return;
        }

        email.text = encode(email.text); //**

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

    public string encode(string email)
    {
        email = email.Replace(".", ",");
        return email;
    }

    public string decode(string email)
    {
        email = email.Replace(",", ".");
        return email;
    }

    public void setStatus(string sentence)
    {
        status.text = sentence;
    }
}