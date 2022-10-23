using UnityEngine;
using UnityEngine.UI;

public class ClosePanelButton : MonoBehaviour
{
    [SerializeField] GameObject panelCover;
    [SerializeField] GameObject panelRequest;
    [SerializeField] GameObject panelConfirm;
    [SerializeField] GameObject panelUpdateRequest;
    [SerializeField] GameObject panelCoverAllRequests;
    [SerializeField] GameObject panelAllRequests;
    [SerializeField] GameObject panelMsg;
    [SerializeField] Text txtMsg;

    public void ClosePanel()
    {
        panelCover.SetActive(false);
        panelRequest.SetActive(false);
        panelConfirm.SetActive(false);
        panelUpdateRequest.SetActive(false);
        panelCoverAllRequests.SetActive(false);
        panelAllRequests.SetActive(false);
        Utilities.ClearFields(TxtMsg:txtMsg, PanelMsg:panelMsg);
    }
}
