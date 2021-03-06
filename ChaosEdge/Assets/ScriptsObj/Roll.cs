using UnityEngine;
using UnityEngine.UI;

public class Roll : MonoBehaviour
{
    private int p_x, p_y, p_z; // 色子的旋转坐标变换
    private float timer; // 计时器
    GameObject Dice;
    public int DiceFaceUpNum; // 记录色子点数
    public bool isRotating; // 记录色子是否在旋转
    // Start is called before the first frame update
    void Start()
    {
        this.GetComponent<Button>().onClick.AddListener(OnClick);
        isRotating = false;
        Dice = GameObject.Find("dice/Dice").gameObject;
    }
    void OnClick()
    {
      //取XYZ的随机旋转值 
       p_x = Random.Range(1, 30);
       p_y = Random.Range(1, 30);
       p_z = Random.Range(1, 30);
       timer = 0.0f;//点击后计时器清零
       isRotating = true;
    }

    // Update is called once per frame
    void Update()
    {
        // dice auto rotate
        if (timer < 3.0f)//规定 旋转时间
        {
            //旋转骰子
            Dice.transform.Rotate(new Vector3(transform.rotation.x + p_x, transform.rotation.y + p_y, transform.rotation.z + p_z));
            timer += 0.02f;
        }
        else isRotating = false;
        // print(isRotating);
        
    }
}
