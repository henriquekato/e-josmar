using UnityEngine;
using UnityEngine.UI;

public class SwitchTimeInputButton : MonoBehaviour
{
    [SerializeField] Dropdown dpdTime;
    [SerializeField] InputField inputHour;
    [SerializeField] Text txtColon;
    [SerializeField] InputField inputMin;
    [SerializeField] Text txtBtnSwitchInput;

    public void SwitchTimeInput()
    {
        if(dpdTime.gameObject.activeInHierarchy)
        {
            dpdTime.gameObject.SetActive(false);
            inputHour.gameObject.SetActive(true);
            txtColon.gameObject.SetActive(true);
            inputMin.gameObject.SetActive(true);
            txtBtnSwitchInput.text = "Trocar";
        }
        else
        {
            dpdTime.gameObject.SetActive(true);
            inputHour.gameObject.SetActive(false);
            txtColon.gameObject.SetActive(false);
            inputMin.gameObject.SetActive(false);
            txtBtnSwitchInput.text = "Digitar";
        }
    }
}
