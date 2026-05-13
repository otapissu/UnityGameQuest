using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordDamage : MonoBehaviour
{
    public int attackDamage = 10;

    private Collider swordCollider;
    private HashSet<Enemy> hitThisSwing = new HashSet<Enemy>();
    private Coroutine deactivateRoutine;

    private void Awake()
    {
        swordCollider = GetComponent<Collider>();
        swordCollider.enabled = false;
    }

    public void ActivateSwing(float duration)
    {
        hitThisSwing.Clear();
        swordCollider.enabled = true;
        Debug.Log("[SwordDamage] 스윙 활성화");

        if (deactivateRoutine != null)
        {
            StopCoroutine(deactivateRoutine);
        }
        deactivateRoutine = StartCoroutine(DeactivateAfter(duration));
    }

    private IEnumerator DeactivateAfter(float duration)
    {
        yield return new WaitForSeconds(duration);
        swordCollider.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"[SwordDamage] 충돌 감지: {other.gameObject.name}");
        Enemy enemy = other.GetComponent<Enemy>();
        if (enemy != null && !hitThisSwing.Contains(enemy))
        {
            hitThisSwing.Add(enemy);
            enemy.TakeDamage(attackDamage);
            Debug.Log($"[SwordDamage] 데미지 {attackDamage} 적용");
        }
    }
}
