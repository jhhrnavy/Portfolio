using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;

public class TextEffect : MonoBehaviour
{
    public enum EffectType
    {
        FadeIn,
        FadeOut,
        Typing,
    }
    [SerializeField] private EffectType _type;
    [SerializeField] private bool _playOnAwake;
    [SerializeField] private float _fadeDuration = 1f;
    [SerializeField] private float _typingDelay = 0.5f;
    private TextMeshProUGUI _target;
    [SerializeField] private Color _initialColor;
    [SerializeField] private float _initialFontSize;
    public bool IsEnded { get; private set; }

    private void Awake()
    {
        _target = GetComponent<TextMeshProUGUI>();
    }

    private void Start()
    {
        IsEnded = true;

        if (_target != null)
        {
            // 초기값 저장
            _initialColor = _target.color;
            _initialFontSize = _target.fontSize;
            //Debug.Log($"{this.gameObject.name} : 초기값 저장 /n InitColor : {_initialColor}/n InitFontSize : {_initialFontSize}");
        }

        if (_playOnAwake)
            Play();
    }

    private void OnEnable()
    {
        //if (_playOnAwake)
            //Play();
    }

    private void OnDisable()
    {
        IsEnded = true;
    }

    public void SetInit()
    {
        _target.color = _initialColor;
        _target.fontSize = _initialFontSize;
    }

    public void Play()
    {
        if (!IsEnded) return;

        IsEnded = false;
        SetInit();

        SetEffect(_type);
    }

    public void PlayEffect(EffectType effectType)
    {
        if (!IsEnded) return;

        IsEnded = false;
        SetInit();

        SetEffect(effectType);
    }

    private void SetEffect(EffectType effectType)
    {
        switch (effectType)
        {
            case EffectType.FadeIn:
                StartCoroutine(FadeIn());
                break;
            case EffectType.FadeOut:
                StartCoroutine(FadeOut());
                break;
            case EffectType.Typing:
                StartCoroutine(Typing());
                break;
        }
    }

    public IEnumerator FadeIn()
    {
        //Debug.Log("페이드 인");
        float currentTime = 0f;
        Color originalColor = _target.color;
        while (currentTime < _fadeDuration)
        {
            float alpha = Mathf.Lerp(0f, 1f, currentTime / _fadeDuration);
            _target.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
            currentTime += Time.deltaTime;
            yield return null;
        }
        _target.color = new Color(originalColor.r, originalColor.g, originalColor.b, 1f);

        IsEnded = true;
    }

    public IEnumerator FadeOut()
    {
        //Debug.Log("페이드 아웃");
        float currentTime = 0f;
        Color originalColor = _target.color;
        while (currentTime < _fadeDuration)
        {
            float alpha = Mathf.Lerp(1f, 0f, currentTime / _fadeDuration);
            _target.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
            currentTime += Time.deltaTime;
            yield return null;
        }
        _target.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0f);

        IsEnded = true;
    }

    public IEnumerator Typing()
    {
        string content = _target.text;
        _target.text = "";
        for(int i = 0; i <= content.Length; i++)
        {
            _target.text = content.Substring(0,i);
            yield return new WaitForSeconds(_typingDelay);
        }
    }
}
