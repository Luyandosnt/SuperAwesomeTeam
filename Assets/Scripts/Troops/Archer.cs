using UnityEngine;

public class Archer : Troop
{
    public GameObject arrowPrefab; // Prefab of the arrow projectile
    public RuntimeAnimatorController[] allAnimators;

    protected override void Attack()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.right, attackRange, enemyLayer);
        if (hit.collider != null && hit.collider.CompareTag("Enemy"))
        {
            Debug.Log($"{gameObject.name} is shooting at {hit.collider.name}!");
            animator.SetBool("Attack", true);
            attackCooldown = 1f / attackFireRate; // Reset cooldown
        }
        else
        {
            animator.SetBool("Attack", false);
        }
    }

    public void SpawnArrow(Vector3 targetPosition)
    {
        GameObject arrow = Instantiate(arrowPrefab, transform.position, Quaternion.identity);
        Arrow arrowScript = arrow.GetComponent<Arrow>();
        arrowScript.owner = this;
        arrowScript.SetTarget(targetPosition, damage);
    }

    public void LifeSteal(float enemyHealth)
    {
        if (enemyHealth <= 0)
        {
            // Heal the archer based on the percentage of the enemy's max health, to maximum of the archer's max health
            float healAmount = Mathf.Min((enemyHealth * lifeStealPercentage), health - maxHealth);
            health += healAmount;
            Debug.Log($"{gameObject.name} healed for {healAmount} health! Health remaining: {health}");
        }
    }


    protected override void DoDamage() { }
    protected override void SetElement(Variant variant)
    {
        if (!canLifesteal)
        {
            if (variant == Variant.Flame)
            {
                troopName = "Flame Archer";
                animator.runtimeAnimatorController = allAnimators[1];
            }
            else if (variant == Variant.Frost)
            {
                troopName = "Frost Archer";
                animator.runtimeAnimatorController = allAnimators[2];
            }
            else if (variant == Variant.Venom)
            {
                troopName = "Venom Archer";
                animator.runtimeAnimatorController = allAnimators[3];
            }
        }
        else
        {
            if (variant == Variant.Base)
            {
                troopName = "Arcane Archer";
                animator.runtimeAnimatorController = allAnimators[0];
            }
            else if (variant == Variant.Flame)
            {
                troopName = "Arcane Flame Archer";
                animator.runtimeAnimatorController = allAnimators[1];
            }
            else if (variant == Variant.Frost)
            {
                troopName = "Arcane Frost Archer";
                animator.runtimeAnimatorController = allAnimators[2];
            }
            else if (variant == Variant.Venom)
            {
                troopName = "Arcane Venom Archer";
                animator.runtimeAnimatorController = allAnimators[3];
            }
        }
    }
    protected override void ProduceCoins() { }

    // Draw attack range in the editor
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
