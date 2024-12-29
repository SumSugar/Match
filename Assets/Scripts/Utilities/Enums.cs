using System;

[Flags]
public enum RoomType
{
    MinorEnemy = 1,

    ELiteEnemy = 2,

    Shop = 4,

    Treasure = 8,

    RestRoom = 16,

    Boss = 32,
}

public enum RoomState
{
    Locked,

    Visited,

    Attainable
}

public enum CardType
{
    Attack,

    Defense,

    Abilities
}

public enum DestinyType
{
    Attack,
    Abilities
}

public enum BreakerType
{
    Attack,
    Abilities
}

public enum EffectTargetType
{
    Self,
    Target,
    ALL,
}

public enum StatusType
{
    Poison,     // 中毒
    Burn,       // 燃烧
    Freeze,     // 冻结
    Stun,       // 眩晕
    Slow,       // 减速
    Silence,    // 沉默
    Bleed,      // 流血
    Shield,     // 护盾（增益示例）
    Regeneration // 再生（增益示例）
}

public enum StatusDurationType
{
    Temporary,  // 有限时间的状态
    Permanent,  // 无限时间的状态
}


public enum CharacterStateType
{
    Idle,
    Attack,
    Defend,
    // 可以添加更多状态
}

public enum VFXType
{
    Hit,
    Death,
    Posion,
    Cursed
}

public enum BreakerState
{
    Inactive,
    Selected,
    Drawn,
    InHand,
    Played,
    Destroyed,
    Highlighted
}

public enum LogType
{
    Info,
    Warning,
    Error,
    System_MapUpdate,
    System_BattleStart,
    Notification,
    Attack,
    Death,
}

public enum LevelType
{
    Normal, // 普通关卡
    Boss,   // Boss关卡
    Story   // 剧情关卡
}

public enum ItemType
{
    Weapon,
    Armor,
    Consumable,
    QuestItem
}

public enum UIPanelState
{
    Inactive,
    Selected,
    Active,
    IsAanimating
}

public enum BattleStateType
{
    PreBattle,
    CardInit,
    CardDraw,
    PreparationPhase,
    ActionPhase,
    BattleEnd
}