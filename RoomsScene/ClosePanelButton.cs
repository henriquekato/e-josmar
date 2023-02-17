using UnityEngine;
using UnityEngine.UI;

public class ClosePanelButton : MonoBehaviour
{
    [SerializeField] GameObject panelCover;
    [SerializeField] GameObject panelRequest;
    [SerializeField] GameObject panelConfirm;
    [SerializeField] GameObject panelUpdateRequest;
    [SerializeField] GameObject panelAllRequests;
    [SerializeField] GameObject panelMsg;
    [SerializeField] Text txtMsg;

    public void ClosePanel()
    {
        panelCover.SetActive(false);
        panelRequest.SetActive(false);
        panelConfirm.SetActive(false);
        panelUpdateRequest.SetActive(false);
        panelAllRequests.SetActive(false);
        HandleFields.ClearFields(TxtMsg:txtMsg, PanelMsg:panelMsg);
    }
}
