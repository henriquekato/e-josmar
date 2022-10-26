using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitButton : MonoBehaviour
{
    public static void Exit()
    {
        User.user.UserId = 0;
        User.user.UserToken = "";
        User.user.UserKeys.Clear();
        SaveSystem.SaveUser(new UserData(User.user.UserId, User.user.UserToken));
        SceneManager.LoadScene("scene-login");
    }
}
