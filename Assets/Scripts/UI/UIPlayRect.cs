using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIPlayRect : MonoBehaviour, IPointerDownHandler
{
    private float _damage;



    private void Awake()
    {
        _damage = Config.Instance.damage;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        DamageCharacter(eventData);
    }

    public void DamageCharacter(PointerEventData eventData)
    {
        var ray = Camera.main.ScreenPointToRay(eventData.pointerCurrentRaycast.screenPosition);
        RaycastHit hit;

        if (Physics.Raycast(ray.origin, ray.direction, out hit, 100))
        {
            var damageable = hit.transform.root.GetComponent<IDamagable>();

            if (damageable != null)
                damageable.TakeDamage(_damage);

        }
    }
}
