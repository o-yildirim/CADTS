using System;

/// <summary>
/// The user class, which gets uploaded to the Firebase Database
/// </summary>

[Serializable] // This makes the class able to be serialized into a JSON
public class User
{
    public string name;
    public string surname;
    public string dob;
    public string email;
    public string password;

    public User(string name,string surname,string dob, string email, string password)
    {
        this.name = name;
        this.surname = surname;
        this.dob = dob;
        this.email = email;
        this.password = password;
    }
}