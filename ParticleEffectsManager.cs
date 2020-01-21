using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ParticleEffectsManager : MonoBehaviour
{
    [SerializeField] private GameObject[] _particleEffects;
    public static ParticleEffectsManager controller;

    void Awake()
    {
        if (controller == null)
            controller = this;

        DontDestroyOnLoad(gameObject);
    }

    public void PlayEffect(ParticleEffectType effectType, bool onLoop = false, float? startSpeed = null)
    {
        var effectGO = _particleEffects.SingleOrDefault(x => x.GetComponent<ParticleEffect>().EffectType == effectType);        
        var effect = effectGO.GetComponent<ParticleSystem>();
        
        var main = effect.main; //I dont understand why I have to do this.
        main.loop = onLoop;     // but I can't just call effect.main.loop, because it's stupid
                
        if(startSpeed.HasValue)
            main.startSpeed = startSpeed.Value;       
       
        effect.Play();
    }

    public void CreateAndPlayEffect(ParticleEffectType effectType)
    {
        var go = InstantiateEffectPrefab(effectType);
        var effect = go.GetComponent<ParticleSystem>();

        effect.Play();
        GameObject.Destroy(go, effect.main.duration);
    }
    
    public void CreateAndPlayEffect(ParticleEffectType effectType, GameObject parent)
    {
        var go = InstantiateEffectPrefab(effectType);
        var effect = go.GetComponent<ParticleSystem>();

        AttachParticleToGameObject(go, parent);
        effect.Play();
    }
    
    public void StopEffect(ParticleEffectType effectType)
    {
        var effectGO = _particleEffects.SingleOrDefault(x => x.GetComponent<ParticleEffect>().EffectType == effectType);
        var effect = effectGO.GetComponent<ParticleSystem>();
        
        effect.Stop();
    }

    public void AttachParticleToGameObject(GameObject particleFX, GameObject parent)
    {
        particleFX.transform.parent = parent.transform;
        particleFX.transform.localScale = new Vector3(1,1,1);
        particleFX.transform.localPosition = Vector3.zero;
        particleFX.transform.localRotation = Quaternion.identity;
    }
    
    private GameObject InstantiateEffectPrefab(ParticleEffectType effectType)
    {
        var effect = _particleEffects.SingleOrDefault(x => x.GetComponent<ParticleEffect>().EffectType == effectType);
        return GameObject.Instantiate(effect);
    }
}

public enum ParticleEffectType
{
    LeftSkiEffect,
    RightSkiEffect,
}
