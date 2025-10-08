using Il2Cpp;
using Il2CppInterop.Runtime.Injection;
using Il2CppInterop.Runtime.InteropTypes;
using MelonLoader;
using System;
using UnityEngine;

namespace DrunksModeExtraMinions;

[RegisterTypeInIl2Cpp]
public class Brewer : Minion
{
    public ECharacterStatus uncurable = (ECharacterStatus)204;
    public override ActedInfo GetInfo(Character charRef)
    {
        return new ActedInfo("");
    }
    public override void Act(ETriggerPhase trigger, Character charRef)
    {
        
    }
    public override CharacterData GetBluffIfAble(Character charRef)
    {
        charRef.statuses.AddStatus(ECharacterStatus.Corrupted, charRef);
        charRef.statuses.AddStatus(uncurable, charRef);
        charRef.statuses.AddStatus(ECharacterStatus.HealthyBluff, charRef);
        // Cannot grab bcz from Minion
        return DrunkStatic.minion.GetBluffIfAble(charRef);
    }
    public Brewer() : base(ClassInjector.DerivedConstructorPointer<Brewer>())
    {
        ClassInjector.DerivedConstructorBody((Il2CppObjectBase)this);
    }
    public Brewer(IntPtr ptr) : base(ptr)
    {

    }
}
