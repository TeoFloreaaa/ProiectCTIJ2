using UnityEngine;

public class LampFlicker : MonoBehaviour
{
    private Light lampLight;
    public float minIntensity = 0.8f;
    public float maxIntensity = 1.2f;
    public float flickerSpeed = 0.1f;

    void Start()
    {
        lampLight = GetComponent<Light>();
    }

    void Update()
    {
        if (lampLight != null)
        {
            lampLight.intensity = Mathf.Lerp(minIntensity, maxIntensity, Mathf.PerlinNoise(Time.time * flickerSpeed, 0));
        }
    }
}
