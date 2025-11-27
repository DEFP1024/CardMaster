using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static System.Net.Mime.MediaTypeNames;

public class PlayerUI : MonoBehaviour
{
    [SerializeField] private Player player;
    [SerializeField] private TextMeshProUGUI hp;
    [SerializeField] private TextMeshProUGUI ap;
    [SerializeField] private TextMeshProUGUI armor;

    private string hpTemplate;

    private void Awake()
    {
        hpTemplate = hp.text;

       
        player = FindAnyObjectByType<Player>();

        player.OnHpChanged += UpdateHpUI;
        player.OnAPChanged += UpdateAPUI;
        player.OnArmorChanged += UpdateArmorUI;
    }

    private void Start()
    {
        UpdateHpUI(player.Hp);

        if (ap != null)
        {
            UpdateAPUI(player.AP);
        }

        if (armor != null)
        {
            UpdateArmorUI(player.Armor);
        }
    }

    private void OnDestroy()
    {
        player.OnHpChanged -= UpdateHpUI;
        player.OnAPChanged -= UpdateAPUI;
        player.OnArmorChanged -= UpdateArmorUI;
    }

    private void UpdateHpUI(int currentHp)
    {
        hp.text = hpTemplate.Replace("{Hp}", currentHp.ToString());
    }

    private void UpdateAPUI(int currentAP)
    {
        if (ap == null)
            return;

        ap.text = currentAP.ToString();
    }

    private void UpdateArmorUI(int currentArmor)
    {
        if (armor ==  null)
            return;
        armor.text = currentArmor.ToString();
    }
}
