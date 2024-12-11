using Spine.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.VFX;

public abstract class CharacterBase : MonoBehaviour
{
    public int teamID;

    public string characterName;

    public int speed;
    //public int maxHP;

    //public IntVariable hp;

    //public IntVariable defense;

    //public IntVariable AttackRange;

    //public IntVariable buffRound;
    //public int CurrentHP { get => hp.currentValue; set => hp.SetValue(value); }
    //public int MaxHP { get => hp.maxValue; }
    public CharacterDataSO characterData;
    public List<CharacterBase> targets = new();
    public CharacterBase currentTarget;
    public Transform vfxTransform;
    public Animator animator;
    private Renderer render;
    public bool isAttacking;

    public bool isDead;

    public int newSortingOrder;

    //力量有关
    public float baseStrength = 1f;
    //private float strengthEffect = 0.5f;

    [Header(header: "卡组")]
    public DestinyDeck destinyDeck;
    public BreakerDeck breakerDeck;

    [Header(header: "状态&异常状态")]
    public GameObject buff;
    public GameObject debuff;

    protected List<StatusEffect> buffStatusEffects = new List<StatusEffect>();   // 增益状态
    protected List<StatusEffect> debuffStatusEffects = new List<StatusEffect>(); // 减益状态
    

    public List<Effect> activeEffects = new List<Effect>(); // 持续效果

    [Header(header: "广播")]
    public ObjectEventSO characterDeadEvent;
    public ObjectEventSO TargetChangeEvent;
    public ObjectEventSO FindTargetsEvent;
    public ObjectEventSO HitEvent;
    public ObjectEventSO AttackEvent;

    // 角色完成动作时的事件
    public ObjectEventSO CharacterActionCompleteEvent;

    public virtual void StartAction()
    {
        Debug.Log($"{characterName} 开始行动");
        ApplyStatusEffects(buffStatusEffects);  // 先应用增益效果
        ApplyStatusEffects(debuffStatusEffects); // 再应用减益效果
    }

    public virtual void CompleteAction()
    {
       
        
    }

    public void Initialize(CharacterDataSO data)
    {
        characterData = data;
        breakerDeck.Initialize(this);
        destinyDeck.Initialize();
    }

    protected virtual void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        vfxTransform = GetComponentInChildren<VFXController>().transform;

        breakerDeck = GetComponentInChildren<BreakerDeck>();

        destinyDeck = GetComponentInChildren<DestinyDeck>();
        render = GetComponentInChildren<Renderer>();
        //假设速度
        speed = UnityEngine.Random.Range(3, 50);

    }

    protected virtual void Start()
    {
        newSortingOrder = Mathf.FloorToInt(transform.position.y * -10);
        render.sortingOrder = newSortingOrder;
    }

    protected virtual void Update()
    {

    }

    public virtual void TakeDamage(int damage)
    {
        //var currentDamage = (damage - defense.currentValue) >=0 ? (damage - defense.currentValue) : 0;
        //var currentdefense = (damage - defense.currentValue) >= 0 ? 0:(defense.currentValue - damage);
        //defense.SetValue(currentdefense);

        HitEvent.RaiseEvent(this, this);

        if (destinyDeck.IsDestinyEmpty())
        {
            OnDeath();
            return;
        }

        VFXManager.Instance.TriggerEffect(VFXType.Hit, vfxTransform.position);
        //CurrentHP -= damage;
        if (!isAttacking && !isDead)
        {
            animator.SetTrigger("hit");
        }


        destinyDeck.OnHit();
        //TimeEffectManager.Instance.TriggerSlowMotion(0.1f, 0.5f);
    }

    public virtual void TakeDotDamage(int damage)
    {
        //var currentDamage = (damage - defense.currentValue) >=0 ? (damage - defense.currentValue) : 0;
        //var currentdefense = (damage - defense.currentValue) >= 0 ? 0:(defense.currentValue - damage);
        //defense.SetValue(currentdefense);

        //HitEvent.RaiseEvent(this, this);

        if (destinyDeck.IsDestinyEmpty())
        {
            OnDeath();
            return;
        }

        //VFXManager.Instance.TriggerEffect(VFXType.Hit, vfxTransform.position);
        //CurrentHP -= damage;
        if (!isAttacking && !isDead)
        {
            animator.SetTrigger("hit");
        }


        destinyDeck.OnHit();
        //TimeEffectManager.Instance.TriggerSlowMotion(0.1f, 0.5f);
    }

    public virtual void OnDeath()
    {
        isDead = true;
        breakerDeck.isAllow = false;
        animator.SetBool("isDead",true);
        ClearStatusEffects();
        VFXManager.Instance.TriggerEffect(VFXType.Death, vfxTransform.position);
        characterDeadEvent.RaiseEvent(this, this);
        breakerDeck.OnDeath();
    }

    // 添加状态效果到合适的列表
    public void AddStatusEffect(StatusEffect effect)
    {
        if (effect.IsBuff)
        {
            AddEffectToList(buffStatusEffects, effect);
        }
        else
        {
            AddEffectToList(debuffStatusEffects, effect);
        }
    }

    // 通用的添加效果方法，确保效果类型唯一或叠加
    private void AddEffectToList(List<StatusEffect> effectList, StatusEffect newEffect)
    {
        StatusEffect existingEffect = effectList.Find(e => e.Type == newEffect.Type);

        if (existingEffect != null && existingEffect.IsStackable)
        {
            existingEffect.StackEffect(); // 如果效果可以叠加，增加叠加次数
        }
        else
        {
            effectList.Add(newEffect); // 否则，添加新的效果
            Debug.Log($"{characterName} 获得了 {newEffect.StatusName} 效果");
        }
    }

    // 应用所有状态效果并移除过期效果
    protected void ApplyStatusEffects(List<StatusEffect> effects)
    {
        for (int i = effects.Count - 1; i >= 0; i--)
        {
            if (isDead)
            {
                return;
            }
            StatusEffect effect = effects[i];
            effect.ApplyTurnEffect();

            if (effect.IsExpired())
            {
                Debug.Log($"{characterName} 的 {effect.StatusName} 效果已结束");
                effects.RemoveAt(i);
            }
        }
    }

    // 清除所有状态效果
    public void ClearStatusEffects()
    {
        buffStatusEffects.Clear();
        debuffStatusEffects.Clear();
        Debug.Log($"{characterName} 的所有异常状态已被清除");
    }

    public void HealHealth(int amount)
    {
        //CurrentHP += amount;
        //CurrentHP = Mathf.Min(CurrentHP, MaxHP);
        buff.SetActive(true);
    }

    public bool TryGetTarget()
    {
        FindTargetsEvent.RaiseEvent(this, this);
        return currentTarget? true : false;
    }


}
