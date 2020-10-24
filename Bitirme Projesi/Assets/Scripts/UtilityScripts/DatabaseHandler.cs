using System;
using System.Collections.Generic;
using UnityEngine;
using Proyecto26;
using FullSerializer;
public class DatabaseHandler
{

    public static User loggedInUser;
    // Start is called before the first frame update
    //bitirme-projesi-df6c6
    private const string projectId = "bitirme-projesi-df6c6"; // You can find this in your Firebase project settings
    private static readonly string databaseURL = $"https://{projectId}.firebaseio.com/";

    private static fsSerializer serializer = new fsSerializer();

    public delegate void PostUserCallback();
    public delegate void GetUserCallback(User user);
    public delegate void GetUsersCallback(Dictionary<string, User> users);
    /*
    private void Update()
    {
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            Debug.Log("Error. Check internet connection!");
            AuthenticationManager.instance.setStatus("İnternet bağlantısı kurulamıyor.");
        }
    }*/


    public static void PostUser(User user, string userEmail, PostUserCallback callback)
    {
        //RestClient.Put<User>($"{databaseURL}users/{userId}.json", user).Then(response => { callback(); }).Catch(error => Debug.Log(error)); 
        RestClient.Put<User>(databaseURL +"users/"+userEmail+".json", user).Then(response => {callback(); }).Catch(error => AuthenticationManager.instance.setStatus("Kayıt olunamadı"));
    }

    public static void GetUser(string userEmail, GetUserCallback callback)
    {
        RestClient.Get<User>($"{databaseURL}users/{userEmail}.json").Then(user => { callback(user); }).Catch(error => AuthenticationManager.instance.setStatus("Email veya şifre yanlış"));
    }


    public static void registerUser(User userToRegister, string userId, GetUserCallback callback)
    {
        RestClient.Get<User>($"{databaseURL}users/{userId}.json").Then(user => { callback(user); AuthenticationManager.instance.setStatus("Bu e-maile ait bir hesap bulunmakta."); })
            .Catch(error =>
            handleJsonException(userId, userToRegister, error)
            );
    }

    public static void handleJsonException(string userId, User user, System.Exception error)
    {
        Debug.Log(error);
        if (error.Message.Equals("JSON must represent an object type."))
        {
            AuthenticationManager.instance.createTokenAsync();
            RestClient.Put<User>(databaseURL + "users/" + userId + ".json", user);
            LoginScreenManager.instance.switchToLogin();
            AuthenticationManager.instance.setStatus("Hesap başarıyla kaydedildi");

        }
        
        else if (error.Message.Equals("Cannot resolve destination host") || error.Message.Equals("Cannot connect to destination host"))
        {
            AuthenticationManager.instance.setStatus("İnternet bağlantısı kurulamıyor.");
        }
    }

    public static void GetAllUsers(GetUsersCallback callback)
    {
        RestClient.Get($"{databaseURL}users.json").Then(response =>
        {
            var responseJson = response.Text;

            // Using the FullSerializer library: https://github.com/jacobdufault/fullserializer
            // to serialize more complex types (a Dictionary, in this case)
            var data = fsJsonParser.Parse(responseJson);
            object deserialized = null;
            serializer.TryDeserialize(data, typeof(Dictionary<string, User>), ref deserialized);

            var users = deserialized as Dictionary<string, User>;
            callback(users);
        });
    }

    public static void InsertStatistic(Statistic statistic)
    {

        Debug.Log(DateTime.Now.ToString().Replace(".","/"));

        RestClient.Put<Statistic>(databaseURL + "statistics/category/"
                                              + statistic.GetCategory() + "/"
                                              + statistic.GetMinigameName() + "/"
                                              + statistic.GetOwner().email.Replace(".", ",") + "/"
                                              + DateTime.Now.ToString().Replace(".","-") + 
                                              ".json",
                                              statistic
                                 );
    }

}
