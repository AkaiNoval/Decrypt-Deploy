using UnityEngine;
using DG.Tweening;

public class DoTweenController : MonoBehaviour
{

    [SerializeField] SpriteRenderer material;
    [SerializeField] float duration;
    [SerializeField] Ease ease;
    [SerializeField] GameObject diamond;
    Sequence sequence;
    private void Awake()
    {
        sequence = DOTween.Sequence();
    }
    void Start()
    {
       material= GetComponent<SpriteRenderer>();
       sequence.Append(transform.DOMove(new Vector3(5, 0, 0), duration, false).SetEase(ease));
       sequence.AppendInterval(5f);
       sequence.Append(material.DOColor(Color.green, duration));
       sequence.SetLoops(-1, LoopType.Yoyo); // Loop the sequence indefinitely, reversing the animation
       // Play the sequence
       sequence.Play();
        //transform.DOMove(new Vector3(5, 0, 0), duration, false)
        //      
        //      .SetLoops(-1,LoopType.Yoyo);
        //material.DOColor(Color.green, duration);
        //diamond.transform.DORotate(new Vector3(0, 360, 0), duration*0.5f,RotateMode.FastBeyond360)
        //     .SetEase(ease)
        //     .SetLoops(-1,LoopType.Restart);
        //diamond.transform.DOLocalMove(new Vector3(0, -3,0),duration)
        //     .SetEase(ease)
        //     .SetLoops(-1, LoopType.Yoyo);

    }

}
