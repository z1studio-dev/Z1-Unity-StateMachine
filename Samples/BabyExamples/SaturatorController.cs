using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class SaturatorController : MonoBehaviour
{
    /* ---------- Inspector ---------- */

    [Header("Materials to affect")]
    [SerializeField] private Material[] litMaterials;
    [SerializeField] private Material[] customShaderMaterials;
    [SerializeField] private Material[] customShaderFloatMaterials;

    [Header("Custom Shader Color Properties")]
    [SerializeField] private string[] customsShaderColorProperties = { "_Fresnel_color", "_Interior_color", "_MainColor", "_EdgeColor" };
    [SerializeField] private string[] customsShaderFloatProperties;

    [Header("VFX Graph Systems")]
    [SerializeField] private VisualEffect[] vfxSystems;
    [SerializeField] private string[] vfxColorParameters = { "ParticleColor" };

    [Header("General")]
    [Range(0, 1)] public float saturation = 1f;
    [Range(0, 2)] public float luminosity = 1f;          // 1 = origine, <1 plus sombre, >1 plus clair
    [SerializeField] private bool applyOnStart = true;
    [SerializeField] private float defaultTransitionDuration = 0.5f;

    /* ---------- Runtime ---------- */

    private Coroutine transitionCoroutine;

    private readonly Dictionary<Material, Dictionary<string, Color>> originalColors        = new();
    private readonly Dictionary<VisualEffect, Dictionary<string, Color>> originalVFXColors = new();
    private readonly Dictionary<Material, Dictionary<string, float>> originalParameters   = new();
    
    private void OnEnable()
    {
        if (applyOnStart)
            ApplyCurrentSettings();
    }

    private void OnDisable()
    {
        RestoreOriginalColors();
    }
    
    public void ApplySaturation(float value)
    {
        saturation = Mathf.Clamp01(value);
        ApplyCurrentSettings();
    }
    
    public void TransitionSaturationAndLuminosity(float targetSaturation,
                                                  float targetLuminosity,
                                                  float? duration = null)
    {
        if (transitionCoroutine != null)
            StopCoroutine(transitionCoroutine);

        transitionCoroutine = StartCoroutine(
            TransitionCoroutine(targetSaturation,
                                targetLuminosity,
                                duration ?? defaultTransitionDuration));
    }
    
    public void SetBabySaturationAndLuminosity(float targetSaturation,
                                               float targetLuminosity,
                                               float? duration = null)
        => TransitionSaturationAndLuminosity(targetSaturation, targetLuminosity, duration);


    private IEnumerator TransitionCoroutine(float targetSat,
                                            float targetLum,
                                            float duration)
    {
        float startSat = saturation;
        float startLum = luminosity;
        float t        = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime / duration;
            float smooth = Mathf.SmoothStep(0f, 1f, t);

            saturation  = Mathf.Lerp(startSat, targetSat, smooth);
            luminosity  = Mathf.Lerp(startLum, targetLum, smooth);

            ApplyCurrentSettings();
            yield return null;
        }

        saturation  = targetSat;
        luminosity  = targetLum;
        ApplyCurrentSettings();   // assure la valeur finale

        transitionCoroutine = null;
    }

    private void ApplyCurrentSettings()
    {
        /* ---- Materials ---- */
        foreach (var mat in litMaterials)
        {
            ApplyToProperty(mat, "_BaseColor");
            ApplyToProperty(mat, "_EmissionColor");
        }

        foreach (var mat in customShaderMaterials)
        {
            foreach (var prop in customsShaderColorProperties)
                ApplyToProperty(mat, prop);
        }

        foreach (var mat in customShaderFloatMaterials)
        {
            foreach (var prop in customsShaderFloatProperties)
            {
                if (!mat.HasProperty(prop))
                {
                    Debug.LogWarning($"Property {prop} not found in material {mat.name}. Skipping.");
                    return;
                }
                
                if (!originalParameters.ContainsKey(mat))
                    originalParameters[mat] = new Dictionary<string, float>();

                if (!originalParameters[mat].ContainsKey(prop))
                    originalParameters[mat][prop] = mat.GetFloat(prop);

                mat.SetFloat(prop, saturation);
            }
        }

        /* ---- VFX ---- */
        foreach (var vfx in vfxSystems)
        {
            if (vfx == null) continue;

            foreach (var param in vfxColorParameters)
            {
                if (!vfx.HasVector4(param))
                {
                    Debug.LogWarning($"VFX « {vfx.name} » : paramètre « {param} » introuvable.");
                    continue;
                }

                if (!originalVFXColors.ContainsKey(vfx))
                    originalVFXColors[vfx] = new Dictionary<string, Color>();

                if (!originalVFXColors[vfx].ContainsKey(param))
                    originalVFXColors[vfx][param] = vfx.GetVector4(param);

                Color baseCol  = originalVFXColors[vfx][param];
                Color adjusted = AdjustColor(baseCol, saturation, luminosity);
                vfx.SetVector4(param, adjusted);
            }
        }
    }

    private void ApplyToProperty(Material mat, string property)
    {
        if (!mat.HasProperty(property))
        {
            // Debug.Log($"Property {property} not found in material {mat.name}. Skipping.");
            return;
        }

        if (!originalColors.ContainsKey(mat))
            originalColors[mat] = new Dictionary<string, Color>();

        if (!originalColors[mat].ContainsKey(property))
            originalColors[mat][property] = mat.GetColor(property);

        // Debug.Log($"Applying {property} to {mat.name}");
        Color baseCol  = originalColors[mat][property];
        Color adjusted = AdjustColor(baseCol, saturation, luminosity);
        mat.SetColor(property, adjusted);
    }

    private static Color AdjustColor(Color color, float sat, float lum)
    {
        Color.RGBToHSV(color, out float h, out float s, out float v);
        s = Mathf.Clamp01(sat);
        v = v * lum;
        Color outCol = Color.HSVToRGB(h, s, v, true);
        outCol.a = color.a;          // ← garde l’alpha d’origine
        return outCol;
    }

    /* ---------- Restore ---------- */

    private void RestoreOriginalColors()
    {
        foreach (var entry in originalColors)
        {
            Material mat    = entry.Key;
            var      props  = entry.Value;

            foreach (var kvp in props)
                if (mat.HasProperty(kvp.Key))
                    mat.SetColor(kvp.Key, kvp.Value);
        }

        foreach (var entry in originalVFXColors)
        {
            VisualEffect vfx = entry.Key;
            var          props = entry.Value;

            foreach (var kvp in props)
                if (vfx.HasVector4(kvp.Key))
                    vfx.SetVector4(kvp.Key, kvp.Value);
        }

        foreach (var prop in originalParameters)
        {
            Material mat    = prop.Key;
            var      props  = prop.Value;

            foreach (var values in props)
            {
                if(mat.HasProperty(values.Key))
                    mat.SetFloat(values.Key, values.Value);
            }
            
        }

        originalColors.Clear();
        originalVFXColors.Clear();
        originalParameters.Clear();
    }
}
