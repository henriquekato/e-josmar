using UnityEngine;
using UnityEngine.UI;

public class ShowHidePasswordButton : MonoBehaviour
{
    [SerializeField] InputField inputPassword;
    [SerializeField] Sprite hidePasswordImg;
    [SerializeField] Sprite showPasswordImg;
    [SerializeField] Button btnShowHidePassword;

    public void ShowHidePassword()
    {
        if(inputPassword.contentType == InputField.ContentType.Password)
        {
            inputPassword.contentType = InputField.ContentType.Standard;
            btnShowHidePassword.image.sprite = hidePasswordImg;
            inputPassword.ForceLabelUpdate();
        }
        else
        {
            inputPassword.contentType = InputField.ContentType.Password;
            btnShowHidePassword.image.sprite = showPasswordImg;
            inputPassword.ForceLabelUpdate();
        }
    }
}
