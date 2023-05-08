using UnityEngine;
using System.Collections;

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
    LineRenderer lineRenderer;
    [SerializeField] bool startCoroutine;
    private Coroutine randomizeCoroutine;

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = numberOfPoints;
        if (isRandom)
        {
            Randomize();
        }
    }
    IEnumerator RandomizeEvery10Seconds()
    {
        while (true)
        {
            yield return new WaitForSeconds(10.0f);
            Randomize();
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
        if (startCoroutine && randomizeCoroutine == null)
        {
            randomizeCoroutine = StartCoroutine(RandomizeEvery10Seconds());
        }
        else if (!startCoroutine && randomizeCoroutine != null)
        {
            StopCoroutine(randomizeCoroutine);
            randomizeCoroutine = null;
        }
    }
    private void Randomize()
    {
        //amplitude = Mathf.Round(Random.Range(0.5f, 1.0f) * 10.0f) / 10.0f;
        frequency = Random.Range(-3, 4);
        phase = Random.Range(-3, 4);
    }
}
/*
    The * 10 and / 10 are used to move the decimal point one digit to the right and then back to the left again, 
    effectively rounding the value to one digit after the decimal point.
    Here's how it works:
       Random.Range(0.0f, 1.0f) generates a random float value between 0 and 1 with many digits after the decimal point, such as 0.7632845.
       Multiplying this value by 10 results in a larger number with the same number of digits after the decimal point, such as 7.632845.
       Rounding this value to the nearest integer using Mathf.Round results in either 7 or 8, depending on whether the original value was closer to 7 or 8.
       Dividing this rounded value by 10 moves the decimal point one digit to the left, resulting in a final value with one digit after the decimal point, such as 0.7 or 0.8.
    The * 10 and / 10 operations are necessary to ensure that the rounding is done correctly. If we omitted these operations, the rounding would be incorrect because Mathf.
    Round rounds to the nearest integer, not the nearest tenth.
*/