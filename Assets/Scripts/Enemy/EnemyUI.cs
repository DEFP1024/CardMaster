using TMPro;
using UnityEngine;

public class EnemyUI : MonoBehaviour
{
    [SerializeField] private EnemyUnit enemy;
    [SerializeField] private TextMeshProUGUI hp;
    [SerializeField] private TextMeshProUGUI armor;

    private string hpTemplate;

    private void Awake()
    {
        hpTemplate = hp.text;
    }

    public void SetUp(EnemyUnit unit)
    {
        enemy = unit;
        enemy.OnHpChanged += UpdateHpUI;
        enemy.OnArmorChanged += UpdateArmorUI;
        UpdateHpUI(enemy.CurrentHp);
        UpdateArmorUI(enemy.CurrentArmor);
    }

    private void OnDestroy()
    {
        enemy.OnHpChanged -= UpdateHpUI;
        enemy.OnArmorChanged -= UpdateArmorUI;
    }

    private void UpdateHpUI(int currentHp)
    {
        hp.text = hpTemplate.Replace("{Hp}", currentHp.ToString());
    }
    private void UpdateArmorUI(int currentArmor)
    {
        armor.text = currentArmor.ToString();
    }
}
