using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class WheelView : UIComponentBase
{
    [Header("Auto-Bound References")]
    [SerializeField] private RectTransform spinContainer;      // ui_animContainer_wheel_spin
    [SerializeField] private RectTransform slicesContainer;    // ui_container_wheel_slices
    [SerializeField] private TextMeshProUGUI zoneIndexText;           // ui_text_zoneIndex_value
    [SerializeField] private GameObject slicePrefabSource;          // referans slice prefabı (opsiyonel, Resources'tan da yüklenebilir)

    [Header("Spin Settings")]
    [SerializeField] private float spinDuration = 3f;
    [SerializeField] private int extraFullSpins = 4;

    private List<GameObject> _spawnedSlices = new();

    protected override void AutoBindReferences()
    {
        if (spinContainer == null)
            spinContainer = transform.Find("ui_animContainer_wheel_spin")?.GetComponent<RectTransform>();

        if (slicesContainer == null && spinContainer != null)
            slicesContainer = spinContainer.Find("ui_container_wheel_slices")?.GetComponent<RectTransform>();

        if (zoneIndexText == null)
            zoneIndexText = GetComponentInChildren<TextMeshProUGUI>();
    }

    public void BuildSlices(List<WheelSliceData> slices)
    {
        ClearSlices();

        float angleStep = 360f / slices.Count;

        for (int i = 0; i < slices.Count; i++)
        {
            GameObject sliceGO = CreateSliceVisual(slices[i], i, angleStep);
            _spawnedSlices.Add(sliceGO);

            SliceView sliceView = sliceGO.GetComponent<SliceView>();

            sliceView.Configure(slices[i], i);
        }
    }

    private GameObject CreateSliceVisual(WheelSliceData data, int index, float angleStep)
    {
        GameObject go = Instantiate(slicePrefabSource);
        go.name = $"ui_image_slice_{index}" ;

        var rect = go.GetComponent<RectTransform>();
        rect.localRotation = Quaternion.Euler(0, 0, -index * angleStep);

        go.transform.SetParent(slicesContainer, false);

        SliceView sliceView = go.GetComponent<SliceView>();

        sliceView.Configure(data, index);

        return go;
    }

    private void ClearSlices()
    {
        foreach (var go in _spawnedSlices)
        {
            if (go != null) DestroyImmediate(go);
        }

        _spawnedSlices.Clear();
    }

    public void PlaySpinAnimation(int targetSliceIndex, int totalSlices, Action onComplete)
    {
        float angleStep = 360f / totalSlices;
        // Pointer'ın 0 derecede (üstte) olduğunu varsayıyoruz,
        // hedef slice'ı pointer'a getirecek açıyı hesaplıyoruz.
        float targetAngle = -(targetSliceIndex * angleStep);
        float fullSpinAngle = 360f * extraFullSpins;
        float finalRotationZ = targetAngle - fullSpinAngle;

        spinContainer.DOKill(); // önceki tween varsa temizle
        spinContainer.localRotation = Quaternion.identity;

        spinContainer
            .DORotate(new Vector3(0, 0, finalRotationZ), spinDuration, RotateMode.FastBeyond360)
            .SetEase(Ease.OutQuint)
            .OnComplete(() => onComplete?.Invoke());
    }

    public void SetZoneIndexText(int zoneIndex)
    {
        if (zoneIndexText != null)
            zoneIndexText.text = zoneIndex.ToString();
    }
}