using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class Shield : MonoBehaviour
{
    private Renderer _renderer;
    [SerializeField] private float _DisolveSpeed;

    private bool _shieldOn;
    private Coroutine _disolveCoroutine;

    // Input System referansı
    private Keyboard keyboard;

    void Start()
    {
        _renderer = GetComponent<Renderer>();
        keyboard = Keyboard.current;
    }

    void Update()
    {
        // F tuşu - Input System ile
        
    }

    public void OpenCloseShield()
    {
        float target = _shieldOn ? 1f : 0f;
        _shieldOn = !_shieldOn;

        if (_disolveCoroutine != null)
        {
            StopCoroutine(_disolveCoroutine);
        }

        _disolveCoroutine = StartCoroutine(Coroutine_DisolveShield(target));
    }

    private IEnumerator Coroutine_DisolveShield(float target)
    {
        float start = _renderer.material.GetFloat("_Disolve");
        float lerp = 0f;
        while (lerp < 1f)
        {
            _renderer.material.SetFloat("_Disolve", Mathf.Lerp(start, target, lerp));
            lerp += Time.deltaTime * _DisolveSpeed;
            yield return null;
        }
    }
}