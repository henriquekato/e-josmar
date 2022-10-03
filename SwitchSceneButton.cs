using UnityEngine;
using UnityEngine.SceneManagement;

public class SwitchSceneButton : MonoBehaviour
{
    public void GoToKeysScene()
    {
        SceneManager.LoadScene("scene-keys");
    }

    public void GoToEditUserScene()
    {
        SceneManager.LoadScene("scene-edituser");
    }
}
