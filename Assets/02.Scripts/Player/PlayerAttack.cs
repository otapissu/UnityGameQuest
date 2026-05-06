using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttack : MonoBehaviour
{
    [Header("Combo")]
    public float comboResetTime = 3.0f;
    public float attackCooldown = 0.3f;

    private Animator animator;
    private Rigidbody rb;
    private int comboStep = 0;
    private float lastAttackTime = -999f;
    private bool isReady = false;

    private static readonly int Hash_Attack1 = Animator.StringToHash("Attack1");
    private static readonly int Hash_Attack2 = Animator.StringToHash("Attack2");
    private static readonly int Hash_Attack3 = Animator.StringToHash("Attack3");
    private static readonly int Hash_IsAttacking = Animator.StringToHash("IsAttacking");

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody>();
    }

    private IEnumerator Start()
    {
        yield return null;

        animator.ResetTrigger(Hash_Attack1);
        animator.ResetTrigger(Hash_Attack2);
        animator.ResetTrigger(Hash_Attack3);

        isReady = true;
    }

    private void Update()
    {
        bool attacking = IsAttackPlaying();
        animator.SetBool(Hash_IsAttacking, attacking);

        if (Time.time - lastAttackTime > comboResetTime)
        {
            comboStep = 0;
        }

        float horizontalSpeed = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z).magnitude;

        if (horizontalSpeed > 0.1f && comboStep > 0 && attacking == false)
        {
            ResetCombo();
        }
    }

    private void ResetCombo()
    {
        comboStep = 0;
        animator.ResetTrigger(Hash_Attack1);
        animator.ResetTrigger(Hash_Attack2);
        animator.ResetTrigger(Hash_Attack3);
    }

    private bool IsAttackPlaying()
    {
        AnimatorStateInfo info = animator.GetCurrentAnimatorStateInfo(0);
        bool inAttack = info.IsName("Attack1") || info.IsName("Attack2") || info.IsName("Attack3");
        return inAttack && info.normalizedTime < 1f;
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.started == false)
        {
            return;
        }

        if (isReady == false)
        {
            return;
        }

        if (IsAttackPlaying() == true)
        {
            return;
        }

        if (Time.time - lastAttackTime < attackCooldown)
        {
            return;
        }

        int[] hashes = { Hash_Attack1, Hash_Attack2, Hash_Attack3 };

        animator.SetTrigger(hashes[comboStep]);

        lastAttackTime = Time.time;
        comboStep = (comboStep + 1) % 3;
    }
}
