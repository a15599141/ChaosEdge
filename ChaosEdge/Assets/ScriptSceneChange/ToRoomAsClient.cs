using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ToRoomAsClient : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {
        this.GetComponent<Button>().onClick.AddListener(OnClick);
    }

    void OnClick()
    {
        SceneManager.LoadScene("Room(Client)Sence");
    }

    // Update is called once per frame
    void Update()
    {

    }
}