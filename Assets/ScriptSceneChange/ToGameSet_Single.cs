using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ToGameSet_Single : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {
        this.GetComponent<Button>().onClick.AddListener(OnClick);
    }

    void OnClick()
    {
        SceneManager.LoadScene("GameInitialize_SinglePlayer");
    }

    // Update is called once per frame
    void Update()
    {

    }
}
