using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Options : MonoBehaviour
{
    public Slider BGMSlider,SoundEffectSlider;
    public AudioSource HomeBGM,ButtonClickSound;
    public Toggle FullScreen;
    public Button Resetbutton;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        ScreenControl();
        SoundControl();
    }
    public void ScreenControl()
    {
        if (FullScreen.isOn) Screen.fullScreen = true;
        else
        {
            Screen.SetResolution(1920, 1080, false);
        }
    }
    public void SoundControl()
    {
        HomeBGM.volume = BGMSlider.value;
        ButtonClickSound.volume = SoundEffectSlider.value;

        /*CanvasManager.Instance.BGM.volume = BGMSlider.value;
        CanvasManager.Instance.battleBGM.volume = BGMSlider.value;

        CanvasManager.Instance.attackSound.volume = SoundEffectSlider.value;
        CanvasManager.Instance.defendSound.volume = SoundEffectSlider.value;
        CanvasManager.Instance.evadeSound.volume = SoundEffectSlider.value;
        CanvasManager.Instance.getDamageSound.volume = SoundEffectSlider.value;
        CanvasManager.Instance.repairSound.volume = SoundEffectSlider.value;
        CanvasManager.Instance.errorSound.volume = SoundEffectSlider.value;
        CanvasManager.Instance.updateSound.volume = SoundEffectSlider.value;*/
    }
    public void Reset()
    {
        BGMSlider.value = 1;
        SoundEffectSlider.value = 1;
        FullScreen.isOn = true;
    }

}
