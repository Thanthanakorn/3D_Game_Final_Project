using UnityEngine;
using UnityEngine.UI;

public class UIEnemyHealthBar : MonoBehaviour
{
    private Slider _slider;
    private float _timeUntilBarIsHidden = 1;

    private void Awake()
    {
        _slider = GetComponentInChildren<Slider>();
    }

    public void SetHealth(int health)
    {
        _slider.value = health;
        _timeUntilBarIsHidden = 3;
    }

    public void SetMaxHealth(int maxHealth)
    {
        _slider.maxValue = maxHealth;
        _slider.value = maxHealth;
    }

    private void Update()
    {
        _timeUntilBarIsHidden -= Time.deltaTime;
        if (_slider == null) return;
        if (_timeUntilBarIsHidden <= 0)
        {
            _timeUntilBarIsHidden = 0;
            _slider.gameObject.SetActive(false);
        }
        else
        {
            if (!_slider.gameObject.activeInHierarchy)
            {
                _slider.gameObject.SetActive(true);
            }
        }

        if (_slider.value <= 0)
        {
            Destroy(_slider.gameObject);
        }
    }
}
