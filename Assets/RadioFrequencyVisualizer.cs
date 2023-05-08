using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class RadioFrequencyVisualizer : MonoBehaviour
{
    
    int numberOfPoints = 100;//ignore this value
    public float amplitude = 1f;
    public float wavelength = 5f;
    [Range(-3, 3)]
    public int frequency;
    [Range(-3, 3)]
    public int phase;
    [SerializeField] bool isRandom;
    private LineRenderer lineRenderer;

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = numberOfPoints;
        if (isRandom)
        {
            RandomizeFrequencyAndPhase();
        }
    }
    private void Update()
    {
        float deltaX = wavelength / numberOfPoints;
        float x = 0f;

        for (int i = 0; i < numberOfPoints; i++)
        {
            float y = amplitude * Mathf.Sin(2f * Mathf.PI * frequency * x + phase);
            Vector3 position = new Vector3(x, y, 0f);
            lineRenderer.SetPosition(i, position);

            x += deltaX;
        }
    }
    private void RandomizeFrequencyAndPhase()
    {
        frequency = Random.Range(-3, 4);
        phase = Random.Range(-3, 4);
    }
}
