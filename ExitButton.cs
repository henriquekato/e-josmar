using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitButton : MonoBehaviour
{
    public void Exit()
    {
        User.user.UserId = 0;
        User.user.UserToken = "";
        User.user.UserKeys.Clear();
        SceneManager.LoadScene("scene-login");
    }
}
