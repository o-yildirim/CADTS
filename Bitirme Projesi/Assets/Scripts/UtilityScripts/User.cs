using System;

/// <summary>
/// The user class, which gets uploaded to the Firebase Database
/// </summary>

[Serializable] 
public class User
{
    public string name;
    public string surname;
    public string dob;
    public string email;
    public string password;
    public string contactMail;

    public User(string name,string surname,string dob, string email, string password)
    {
        this.name = name;
        this.surname = surname;
        this.dob = dob;
        this.email = email;
        this.password = password;
        this.contactMail = "";
    }

    public User(string name, string surname, string dob, string email, string password, string contactMail)
    {
        this.name = name;
        this.surname = surname;
        this.dob = dob;
        this.email = email;
        this.password = password;
        this.contactMail = contactMail;
    }
}