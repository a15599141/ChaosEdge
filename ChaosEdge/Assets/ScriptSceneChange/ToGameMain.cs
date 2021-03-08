using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ToGameMain : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {
        this.GetComponent<Button>().onClick.AddListener(OnClick);
    }

    void OnClick()
    {
        SceneManager.LoadScene("MainScene");
    }

    // Update is called once per frame
    void Update()
    {

    }
}
