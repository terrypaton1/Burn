using UnityEngine;

public class HealthDisplay : MonoBehaviour
{
    [SerializeField]
    protected HealthBar healthBar;

    public void DisplayHealth()
    {
        var percent = CoreConnector.Player.GetCurrentHealth() / GameSettings.HealthMax;
        healthBar.SetValue(percent);
    }

    public void HideInstantly()
    {
        healthBar.HideInstantly();
    }

    public void Show()
    {
        healthBar.Show();
    }

    public void Hide()
    {
        healthBar.Hide();
    }
}