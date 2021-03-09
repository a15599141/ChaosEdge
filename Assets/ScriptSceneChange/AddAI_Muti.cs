using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class AddAI_Muti : MonoBehaviour
{
    // Use this for initialization
    void Start()
    {
        this.GetComponent<Button>().onClick.AddListener(OnClick);
    }

    void OnClick()
    {
        SceneManager.LoadScene("AIPlayerSetting(MutiPlayer)");
    }

    // Update is called once per frame
    void Update()
    {

    }
}
