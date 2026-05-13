using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    [Header("Stats")]
    public int maxHp = 50;

    [Header("UI")]
    public Slider hpSlider;
    public Transform hpBarRoot;

    private int currentHp;
    private Transform mainCam;
    private Animator animator;
    private bool isDead = false;

    private static readonly int Hash_Death = Animator.StringToHash("Death");

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
    }

    private void Start()
    {
        mainCam = Camera.main.transform;
        ResetEnemy();
    }

    private void LateUpdate()
    {
        if (hpBarRoot != null && mainCam != null)
        {
            hpBarRoot.LookAt(hpBarRoot.position + mainCam.forward);
        }
    }

    public void TakeDamage(int damage)
    {
        if (isDead)
        {
            return;
        }

        currentHp = Mathf.Max(0, currentHp - damage);
        RefreshBar();

        if (currentHp <= 0)
        {
            StartCoroutine(Die());
        }
    }

    public void ResetEnemy()
    {
        isDead = false;
        currentHp = maxHp;
        RefreshBar();

        if (hpBarRoot != null)
        {
            hpBarRoot.gameObject.SetActive(true);
        }

        animator.Rebind();
        animator.Update(0f);

        gameObject.SetActive(true);
    }

    private IEnumerator Die()
    {
        isDead = true;

        if (hpBarRoot != null)
        {
            hpBarRoot.gameObject.SetActive(false);
        }

        animator.SetTrigger(Hash_Death);

        yield return null;

        float waited = 0f;
        while (!animator.GetCurrentAnimatorStateInfo(0).IsName("Death") && waited < 1f)
        {
            waited += Time.deltaTime;
            yield return null;
        }

        float deathLength = animator.GetCurrentAnimatorStateInfo(0).length;
        yield return new WaitForSeconds(deathLength);

        gameObject.SetActive(false);
    }

    private void RefreshBar()
    {
        if (hpSlider != null)
        {
            hpSlider.value = (float)currentHp / maxHp;
        }
    }
}
