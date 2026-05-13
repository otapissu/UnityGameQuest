using System.Collections;
using UnityEngine;

public class Potion : MonoBehaviour
{
    public float healAmount = 20f;
    public float respawnTime = 10f;

    private Renderer[] potionRenderers;
    private Collider potionCollider;

    private void Awake()
    {
        potionRenderers = GetComponentsInChildren<Renderer>();
        potionCollider = GetComponent<Collider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
        if (playerHealth == null)
        {
            return;
        }

        playerHealth.Heal(healAmount);
        StartCoroutine(Respawn());
    }

    private IEnumerator Respawn()
    {
        SetVisible(false);
        potionCollider.enabled = false;

        yield return new WaitForSeconds(respawnTime);

        SetVisible(true);
        potionCollider.enabled = true;
    }

    private void SetVisible(bool visible)
    {
        foreach (Renderer r in potionRenderers)
        {
            r.enabled = visible;
        }
    }
}
