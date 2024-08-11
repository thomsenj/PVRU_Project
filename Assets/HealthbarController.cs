using UnityEngine;
using UnityEngine.UI;

public class HealthbarController : MonoBehaviour
{

    public Slider slider;
    public int initialValue;
    public TMPro.TextMeshProUGUI textMesh;

    // Start is called before the first frame update
    void Start()
    {
        SetMaxValue(initialValue);
        slider.onValueChanged.AddListener(delegate { SyncText(); });
    }

    public void SetMaxValue(int value)
    {
        slider.maxValue = value;
        slider.value = value;
        SyncText();
    }

    public void SetValue(int value)
    {
        if (value >= 0)
        {
            slider.value = value;
        }
        SyncText();
    }

    public void ApplyDelta(int delta)
    {
        if (slider.value + delta >= 0 && slider.value + delta <= slider.maxValue)
        {
            slider.value += delta;
        }
        else
        {
            if (delta < 0)
            {
                slider.value = 0;
            }
            else
            {
                slider.value = slider.maxValue;
            }
        }
        SyncText();
    }

    private void SyncText()
    {
        textMesh.SetText(slider.value.ToString());
    }
}