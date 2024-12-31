using UnityEngine;
using System.Collections;

public class BobController : MonoBehaviour
{
    public GameObject bow;
    public GameObject one;
    public GameObject sword;
    public GameObject hammer;

    public float moveSpeed = 5f;
    private Vector2 movement;
    private string currentTag = "none"; // 當前觸發的 Tag
    private void Start()
    {
        // 確保默認狀態
        bow.SetActive(false);
        sword.SetActive(false);
        one.SetActive(true);
        hammer.SetActive(false);
    }
    private enum ActionState
    {
        Normal,
        SwordAnimation,
        BowAnimation,
        HammerAnimation
    }

    private ActionState currentState = ActionState.Normal; // 初始狀態
    private void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal"); // 左右
        movement.y = Input.GetAxisRaw("Vertical");   // 上下

        // 按鍵切換狀態
        /*if (Input.GetKeyDown(KeyCode.E))
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
        }*/
        // 檢查按鍵來觸發 Tag 動作
        if (Input.GetKeyDown(KeyCode.Alpha6)) // 按下鍵盤上的 "1"
        {
            TriggerActionByTag("sword");
        }
        else if (Input.GetKeyDown(KeyCode.Alpha7)) // 按下鍵盤上的 "2"（可擴展）
        {
            TriggerActionByTag("bow");
        }
        else if (Input.GetKeyDown(KeyCode.Alpha8))
        {
            TriggerActionByTag("hammer");
        }

        // 執行對應的函式
        switch (currentTag)
        {
            case "sword":
                PlaySwordAnimation();
                break;
            case "bow":
                PlayBowAnimation();
                break;
            case "hammer":
                PlayHammerAnimation();
                break;
            default:
                NormalAction();
                break;
        }
    }
    void FixedUpdate()
    {
        // 移動角色
        transform.Translate(movement * moveSpeed * Time.deltaTime);
    }

    // 根據 Tag 觸發對應的動作
    public void TriggerActionByTag(string tag)
    {
        if(tag == "none"){
            currentTag = "none";
            Debug.Log("Current action triggered by tag: " + currentTag);
            return;
        }

        if (sword.CompareTag(tag))
        {
            currentTag = "sword";
        }
        else if (bow.CompareTag(tag))
        {
            currentTag = "bow";
        }
        else if (hammer.CompareTag(tag))
        {
            currentTag = "hammer";
        }
        else
        {
            currentTag = "none";
        }
        Debug.Log("Current action triggered by tag: " + currentTag);
    }
    private void NormalAction()
    {
        bow.SetActive(false);
        one.SetActive(true);
        sword.SetActive(false);
        hammer.SetActive(false);
    }
    private void PlayBowAnimation()
    {
        // 切換到 bow_side
        bow.SetActive(true);
        one.SetActive(false);
        sword.SetActive(false);
        hammer.SetActive(false);
    }
    public void PlaySwordAnimation()
    {
        bow.SetActive(false);
        one.SetActive(false);
        sword.SetActive(true);
        hammer.SetActive(false);
    }
    public void PlayHammerAnimation()
    {
        bow.SetActive(false);
        one.SetActive(false);
        sword.SetActive(false);
        hammer.SetActive(true);
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
