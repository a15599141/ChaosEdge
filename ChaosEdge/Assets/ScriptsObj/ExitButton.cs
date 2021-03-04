using UnityEngine;
using UnityEngine.UI;

public class ExitButton : MonoBehaviour
{
    // Use this for initialization
    void Start()
    {
        this.GetComponent<Button>().onClick.AddListener(OnClick);
    }

    void OnClick()
    {
        
        Application.Quit();
    }

    // Update is called once per frame
    void Update()
    {

    }
}