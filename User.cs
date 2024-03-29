using System;
using System.Collections.Generic;

public class User
{
    public int UserId;
    public string UserToken;
    public List<Key> UserKeys = new List<Key>();

    public static User user = new User();
}

[Serializable]
public class UserData
{
    public int id;
    public string token;

    public UserData(int id, string token)
    {
        this.id = id;
        this.token = token;
    }
}
