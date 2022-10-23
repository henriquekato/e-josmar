using UnityEngine;
using UnityEngine.UI;

public class CancelRequestButton : MonoBehaviour
{
    [SerializeField] GameObject panelConfirm;
    [SerializeField] GameObject panelRequest;

    public void CancelRequest()
    {
        panelConfirm.SetActive(false);
        panelRequest.SetActive(true);
    }
}
