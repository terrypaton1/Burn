using UnityEngine;
using UnityEngine.Assertions;

public class PlayerModelManager : MonoBehaviour
{
    [SerializeField]
    protected PlayerModel[] models;

    protected void OnEnable()
    {
        DisableAllModels();
    }

    public void Show(PlayerDisplayType type)
    {
        DisableAllModels();
        var totalModels = models.Length;
        var index = (int) type;
        Assert.IsTrue(index > -1 || index > totalModels, $"Model: {index}  out of range {totalModels}");

        var currentModel = models[index];
        currentModel.Show();
        currentModel.transform.localPosition = Vector3.zero;
    }

    public void DisableAllModels()
    {
        foreach (var model in models)
        {
            model.Hide();
        }
    }
}