using UnityEngine;
using UnityEngine.UI;

public class UpgradeMenu : MonoBehaviour {

  [SerializeField]
  private Text healthText;

  [SerializeField]
  private Text speedText;

  [SerializeField]
  private float healthMultiplier = 1.3f;
  [SerializeField]
  private float movementSpeedMultiplier = 1.2f;

  [SerializeField]
  private int upgradeCost = 50;

  private PlayerStats stats;

  private void OnEnable()
  {
    stats = PlayerStats.instance;
    UpdateValues();
  }

  void UpdateValues()
  {
    healthText.text = "Health: " + stats.maxHealth.ToString();
    speedText.text = "Speed: " + stats.movementSpeed.ToString();
  }

  public void UpgradeHealth()
  {
    if (GameMaster.money < upgradeCost)
    {
      AudioManager.instance.PlaySound("NoMoney");
      return;
    }

    stats.maxHealth = (int)(stats.maxHealth * healthMultiplier);
    GameMaster.money -= upgradeCost;
    AudioManager.instance.PlaySound("Money");

    UpdateValues();
  }

  public void UpgradeSpeed()
  {
    if (GameMaster.money < upgradeCost)
    {
      AudioManager.instance.PlaySound("NoMoney");
      return;
    }

    stats.movementSpeed = Mathf.RoundToInt(stats.movementSpeed *  movementSpeedMultiplier);
    GameMaster.money -= upgradeCost;
    AudioManager.instance.PlaySound("Money");

    UpdateValues();
  }

}
