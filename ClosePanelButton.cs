using UnityEngine;
using UnityEngine.UI;

public class ClosePanelButton : MonoBehaviour
{
    [SerializeField] GameObject panelCover;
    [SerializeField] GameObject panelRequest;
    [SerializeField] GameObject panelUpdateRequest;
    [SerializeField] GameObject panelMsg;

    public void ClosePanel()
    {
        panelCover.SetActive(false);
        panelRequest.SetActive(false);
        panelUpdateRequest.SetActive(false);
        panelMsg.SetActive(false);
    }
}
