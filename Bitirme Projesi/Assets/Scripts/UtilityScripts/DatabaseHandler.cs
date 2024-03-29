﻿using System;
using System.Collections.Generic;
using UnityEngine;
using Proyecto26;
using FullSerializer;
using UnityEditor;

public class DatabaseHandler
{

    public static User loggedInUser;
    private const string projectId = "bitirme-projesi-df6c6"; 
    private static readonly string databaseURL = $"https://{projectId}.firebaseio.com/";

    private static fsSerializer serializer = new fsSerializer();

    public delegate void PostUserCallback();
    public delegate void GetUserCallback(User user);
    public delegate void GetUsersCallback(Dictionary<string, User> users);

    public delegate void PostUserStatisticsCallback(); 
    public delegate void GetUserStatisticsCallback(Dictionary<string, Statistic> dates);
    public delegate void GetGlobalStatisticCallback(GlobalStatistic globalStatistic);



    public static void PostUser(User user, string userEmail, PostUserCallback callback)
    {
        RestClient.Put<User>($"{databaseURL}users/{userEmail}.json", user).Then(response => { callback(); }).Catch(error => Debug.Log(error));
    }

    public static void GetUser(string userEmail, GetUserCallback callback)
    {
        RestClient.Get<User>($"{databaseURL}users/{userEmail}.json").Then(user => { callback(user); }).Catch(error => AuthenticationManager.instance.setStatus("Email veya şifre yanlış"));
    }

    public static void DeleteUser(User userToDelete, bool deleteStatistics)
    {
        string encodedEmail = AuthenticationManager.instance.encode(userToDelete.email);
        Debug.Log(encodedEmail);

        RestClient.Delete($"{databaseURL}users/{encodedEmail}.json").Then(response => {

            if (deleteStatistics)
            {
                RestClient.Delete($"{databaseURL}statistics/{encodedEmail}.json").Then(response2 => { SettingsManager.instance.LogOut(); });
            }
            else
            {
                SettingsManager.instance.LogOut();
            }

        });
       

    }

    public static void registerUser(User userToRegister, string userId, GetUserCallback callback)
    {
        RestClient.Get<User>($"{databaseURL}users/{userId}.json").Then(user => { callback(user); AuthenticationManager.instance.setStatus("Bu e-maile ait bir hesap bulunmakta."); })
            .Catch(error =>
            handleJsonException(userId, userToRegister, error)
            );
    }

    public static void handleJsonException(string userId, User user, Exception error)
    {
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



    public static void GetUserStatistics(string email, string category, string game, GetUserStatisticsCallback callback)
    {
        RestClient.Get($"{databaseURL}/statistics/{email}/{category}/{game}.json").Then(response =>
        {
            var responseJson = response.Text;
            // Using the FullSerializer library: https://github.com/jacobdufault/fullserializer
            // to serialize more complex types (a Dictionary, in this case)
            var data = fsJsonParser.Parse(responseJson);
            object deserialized = null;
            serializer.TryDeserialize(data, typeof(Dictionary<string, Statistic>), ref deserialized);

            var users = deserialized as Dictionary<string, Statistic>;
            callback(users);
        });

    }

    public static void sendMail(string userId, MailInfo info)
    {
        RestClient.Put<MailInfo>(databaseURL + "mail/" + userId + ".json", info).Then(response => { StaisticsPanelManager.instance.mailAckText.text = "Başarıyla gönderildi."; });    
    }

    public static void sendMailToContact(string userId, MailInfo info)
    {
        RestClient.Put<MailInfo>(databaseURL + "contactMail/" + userId + ".json", info).Then(response => { StaisticsPanelManager.instance.mailAckText.text = "Başarıyla gönderildi."; });
    }

    public static void InsertStatistic(Statistic statistic)
    {
        RestClient.Put<Statistic>(databaseURL + "statistics/"
                                                + statistic.GetOwner().email.Replace(".", ",") + "/"
                                                + statistic.GetCategory() + "/"
                                                + statistic.GetMinigameName() + "/"
                                                + statistic.GetDate()
                                                + ".json",
                                                statistic
         );
    }

    public static void GetGlobalStatistic(string category,string game,int ageGapLower,int ageGapUpper, GetGlobalStatisticCallback callback)
    {
       RestClient.Get<GlobalStatistic>(databaseURL + "globalStatistics/" +ageGapLower +"-" + ageGapUpper +  "/" + category + "/" + game +".json").Then(globalStatistic => { callback(globalStatistic); });    
    }

}
