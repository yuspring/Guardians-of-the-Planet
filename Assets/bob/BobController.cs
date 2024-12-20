using UnityEngine;
using System.Collections;

public class BobController : MonoBehaviour
{
    public GameObject bow;
    public GameObject one;
    public GameObject sword;

    public float moveSpeed = 5f;
    private Vector2 movement;
    private void Start()
    {
        // 確保默認狀態
        bow.SetActive(false);
        sword.SetActive(false);
        one.SetActive(true);
    }
    private enum ActionState
    {
        Normal,
        SwordAnimation,
        BowAnimation
    }

    private ActionState currentState = ActionState.Normal; // 初始狀態
    private void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal"); // 左右
        movement.y = Input.GetAxisRaw("Vertical");   // 上下

        // 按鍵切換狀態
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (currentState == ActionState.SwordAnimation)
            {
                currentState = ActionState.Normal;
            }
            else
            {
                currentState = ActionState.SwordAnimation;
            }
            Debug.Log("Switched to " + currentState);
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (currentState == ActionState.BowAnimation)
            {
                currentState = ActionState.Normal;
            }
            else
            {
                currentState = ActionState.BowAnimation;
            }
            Debug.Log("Switched to " + currentState);
        }

        // 執行對應的函式
        switch (currentState)
        {
            case ActionState.Normal:
                NormalAction();
                break;
            case ActionState.SwordAnimation:
                PlaySwordAnimation();
                break;
            case ActionState.BowAnimation:
                PlayBowAnimation();
                break;
        }
    }
    void FixedUpdate()
    {
        // 移動角色
        transform.Translate(movement * moveSpeed * Time.deltaTime);
    }
    private void NormalAction()
    {
        bow.SetActive(false);
        one.SetActive(true);
        sword.SetActive(false);
    }
    private void PlayBowAnimation()
    {
        // 切換到 bow_side
        bow.SetActive(true);
        one.SetActive(false);
        sword.SetActive(false);

    }
    private void PlaySwordAnimation()
    {
        bow.SetActive(false);
        one.SetActive(false);
        sword.SetActive(true);

    }

    private static BobController instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
