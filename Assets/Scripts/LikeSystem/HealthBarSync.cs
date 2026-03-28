using UnityEngine;
using UnityEngine.UI;

public class HealthBarSync : MonoBehaviour
{
    private Slider mySlider;

    void Start()
    {
        mySlider = GetComponent<Slider>();
        // Tell the GameManager: "Hey, I'm the new health bar for this level!"
        GameManager.Instance.UpdateUIReference(mySlider);
    }
}