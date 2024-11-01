using UnityEngine;

public class CollectableBankController : MonoBehaviour
{
    public int initialCount;
    public TMPro.TextMeshProUGUI textMesh;

    private int state;

    void Start()
    {
        state = initialCount;
        SyncText();
    }

    public void SetCount(int count)
    {
        if (count >= 0)
        {
            state = count;
        }
        SyncText();
    }

    public void AddCount(int delta)
    {
        if (state + delta >= 0)
        {
            state += delta;
        }
        else
        {
            state = 0;
        }
        SyncText();
    }

    private void SyncText()
    {
        textMesh.SetText(FormatCount(state));
    }

    private string FormatCount(int count)
    {
        if (count >= 1000)
        {
            return (count / 1000f).ToString("0.0") + "K";
        }
        return count.ToString();
    }
}