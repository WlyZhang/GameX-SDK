using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// 单个游戏体生命周期接口定义
/// </summary>
public interface IObjectLoader
{
    void Awake();

    void OnEnable(GameObject go);

    void Start(GameObject go);

    void OnDisable(GameObject go);

    void OnDestroy(GameObject go);

    void OnTriggerEnter(GameObject self, Collider collition);

    void OnTriggerStay(GameObject self, Collider collition);

    void OnTriggerExit(GameObject self, Collider collition);

    void OnTriggerEnter2D(GameObject self, Collider2D collition);

    void OnTriggerStay2D(GameObject self, Collider2D collition);

    void OnTriggerExit2D(GameObject self, Collider2D collition);

    void OnCollisionEnter(GameObject self, Collision collition);

    void OnCollisionStay(GameObject self, Collision collition);

    void OnCollisionExit(GameObject self, Collision collition);

    void OnCollisionEnter2D(GameObject self, Collision2D collition);

    void OnCollisionStay2D(GameObject self, Collision2D collition);

    void OnCollisionExit2D(GameObject self, Collision2D collition);

    void OnPointerClick(GameObject self, PointerEventData eventData);

    void OnPointerDown(GameObject self, PointerEventData eventData);

    void OnPointerUp(GameObject self, PointerEventData eventData);

    void OnPointerEnter(GameObject self, PointerEventData eventData);

    void OnPointerExit(GameObject self, PointerEventData eventData);

    void OnBeginDrag(GameObject self, PointerEventData eventData);

    void OnDrag(GameObject self, PointerEventData eventData);

    void OnEndDrag(GameObject self, PointerEventData eventData);
}