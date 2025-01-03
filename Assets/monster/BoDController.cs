using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class BoDController : MonoBehaviour
{
    public float detectRange = 5f;       // 偵測範圍
    public float attackRange = 3f;      // 攻擊範圍
    public float TpRange = 7f;          // 順移範圍
    public float attackInterval = 1.2f;   // 攻擊間隔
    public float moveSpeed = 2f;        // 移動速度
    public HealthBar healthBar;
    private GameObject player;          // 主角物件
    private BobController playerController; // 主角的腳本
    private Animator animator;          // 動畫控制器
    private bool isAttacking = false;   // 是否正在攻擊
    private bool isPlayerInRange = false; // 主角是否在偵測範圍內
    private bool isTakingDamage = false; // 是否正在撥放受傷動畫
    public int mons_hp = 10;            // 敵人血量

    void Start()
    {
        mons_hp = 30;
        // 找到主角物件
        player = GameObject.FindWithTag("bob");
        if (player != null)
        {
            playerController = player.GetComponent<BobController>();
        }

        // 初始化動畫控制器
        animator = this.GetComponent<Animator>();

        // 確保初始狀態為 Idle
        if (animator != null)
        {
            animator.Play("Idle");
        }
        if (healthBar == null)
        {
            GameObject healthBar_ = GameObject.FindWithTag("health");
            healthBar = healthBar_.GetComponent<HealthBar>();

        }
        StartCoroutine(CheckAndAct());
    }

    private IEnumerator CheckAndAct()
    {
        while (true)
        {
            if (player != null)
            {
                float distance = Vector3.Distance(transform.position, player.transform.position);

                if (distance <= TpRange)
                {
                    isPlayerInRange = true;

                    if (!isTakingDamage) // 確保不打斷受傷動畫
                    {
                        if (distance > detectRange && distance <= TpRange && !isAttacking)
                        {
                            // 撥放 Cast 動畫，然後順移
                            animator.Play("Cast");
                            yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
                            MoveTowardsPlayer();
                            // 順移至主角位置附近
                            transform.position = player.transform.position + (transform.position - player.transform.position).normalized * attackRange;

                            // 撥放 Spell 動畫
                            animator.Play("Spell");
                            yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
                        }
                        else if (distance < detectRange && distance > attackRange && !isAttacking)
                        {
                            // 切換到 Walk 並移動向主角
                            animator.Play("Walk");
                            MoveTowardsPlayer();
                        }
                        else if (distance <= attackRange && !isAttacking)
                        {
                            // 發動攻擊
                            StartCoroutine(AttackPlayer());
                        }
                    }
                }
                else
                {
                    isPlayerInRange = false;
                    // 切換到 Idle
                    if (!isTakingDamage)
                    {
                        animator.Play("Idle");
                    }
                }
            }
            else
            {
                isPlayerInRange = false;
                // 切換到 Idle
                if (!isTakingDamage)
                {
                    animator.Play("Idle");
                }
            }

            yield return new WaitForSeconds(0.1f); // 每 0.1 秒檢查一次
        }
    }

    private void MoveTowardsPlayer()
    {
        if (player != null)
        {
            Vector3 direction = (player.transform.position - transform.position).normalized;
            transform.position += direction * moveSpeed * Time.deltaTime * 10;
            if (direction.x < 0)
            {
                FlipCharacter(true);
            }
            if (direction.x > 0)
            {
                FlipCharacter(false);
            }
        }
    }

    private IEnumerator AttackPlayer()
    {
        isAttacking = true;

        // 切換到 Attack 動畫
        if (animator != null)
        {
            animator.Play("Attack");
        }
        //yield return new WaitForSeconds(0.4f);
        //playerController.TriggerActionByTag("hit");
        //healthBar.subHealth(2);
        // 等待攻擊動畫完成
        yield return new WaitForSeconds(attackInterval);
        // 檢查攻擊命中
        if (player != null)
        {
            float distance = Vector3.Distance(transform.position, player.transform.position);
            if (distance <= attackRange)
            {
                Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, attackRange);
                foreach (Collider2D collider in hitColliders)
                {
                    if (collider.CompareTag("bob"))
                    {
                        playerController.TriggerActionByTag("hit");
                        healthBar.subHealth(4);
                        break;
                    }
                }
            }
        }
        isAttacking = false;
    }

    public void TakeDamage(int damage)
    {
        if (isTakingDamage) return; // 如果正在撥放受傷動畫，忽略其他攻擊

        mons_hp -= damage;

        if (animator != null)
        {
            StartCoroutine(PlayHitAnimation());
        }

        if (mons_hp <= 0)
        {
            StartCoroutine(Die());
        }
    }

    private IEnumerator PlayHitAnimation()
    {
        isTakingDamage = true;

        // 撥放 Hit 動畫
        animator.Play("Hurt");

        // 等待動畫完成
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length / 2);

        isTakingDamage = false;
    }

    private IEnumerator Die()
    {
        isTakingDamage = true;

        // 撥放 Hit 動畫
        animator.Play("Death");

        // 等待動畫完成
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length / 2);

        isTakingDamage = false;
        // 播放死亡動畫或處理死亡邏輯
        Debug.Log("Enemy defeated!");

        GameObject timer = GameObject.FindWithTag("timer");
        Destroy(gameObject);

        bool GoodEnd = timer.GetComponent<Timer>().goodend;

        if(GoodEnd){
            SceneManager.LoadScene("GoodEnd");
        }
        else{
            SceneManager.LoadScene("BadEnd");
        }
    }

    private void OnDrawGizmosSelected()
    {
        // 可視化偵測範圍
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectRange);

        // 可視化攻擊範圍
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);

        // 可視化順移範圍
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, TpRange);
    }

    void FlipCharacter(bool faceLeft)
    {
        Vector3 scale = transform.localScale;
        scale.x = faceLeft ? Mathf.Abs(scale.x) : -Mathf.Abs(scale.x); // 根據方向調整 x 軸縮放
        transform.localScale = scale;
    }
}
