using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ToLobbyEntry : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {
        this.GetComponent<Button>().onClick.AddListener(OnClick);
    }

    void OnClick()
    {
        SceneManager.LoadScene("LobbyEntryScene");
    }

    // Update is called once per frame
    void Update()
    {

    }
}
