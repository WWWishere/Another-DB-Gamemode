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
    public override ActedInfo bcq(Character charRef)
    {
        return new ActedInfo("");
    }
    public override void bcs(ETriggerPhase trigger, Character charRef)
    {
        
    }
    public override CharacterData bcz(Character charRef)
    {
        charRef.statuses.fm(ECharacterStatus.Corrupted, charRef);
        charRef.statuses.fm(ECharacterStatus.HealthyBluff, charRef);
        // Cannot grab bcz from Minion
        return DrunkStatic.minion.bcz(charRef);
    }
    public Brewer() : base(ClassInjector.DerivedConstructorPointer<Brewer>())
    {
        ClassInjector.DerivedConstructorBody((Il2CppObjectBase)this);
    }
    public Brewer(IntPtr ptr) : base(ptr)
    {

    }
}
