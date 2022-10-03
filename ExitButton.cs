using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitButton : MonoBehaviour
{
    public void Exit()
    {
        User.user.UserToken = "";
        SceneManager.LoadScene("scene-login");
    }
}
