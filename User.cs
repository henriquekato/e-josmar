using System.Collections.Generic;

public class User
{
    public int UserId;
    public string UserToken = "";
    public List<Key> UserKeys = new List<Key>();

    public static User user = new User();
}
