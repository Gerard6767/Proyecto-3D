using UnityEngine;

public class UiScript : MonoBehaviour
{
    public GameObject panel;

    void Start()
    {
        panel.SetActive(false);
        
    }

    // Update is called once per frame
    public void ShowInstructions(){
       
        panel.SetActive(true);
    }
}
