using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ToRoomAsHost : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {
        this.GetComponent<Button>().onClick.AddListener(OnClick);
    }

    void OnClick()
    {
        SceneManager.LoadScene("Room(Host)Sence");
    }

    // Update is called once per frame
    void Update()
    {

    }
}
