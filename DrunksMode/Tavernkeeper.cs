using MelonLoader;
using UnityEngine;
using Il2CppInterop.Runtime.Injection;
using Il2CppInterop.Runtime.InteropTypes;
using System;
using Il2CppSystem.Collections.Generic;
using Il2Cpp;

namespace DrunksMode;

[RegisterTypeInIl2Cpp]
public class Tavernkeeper : Role
{
    public override ActedInfo bcq(Character charRef)
    {
        List<Character> characters = Gameplay.CurrentCharacters;
        List<Character> newList = new List<Character>();
        foreach (Character character in characters)
        {
            bool corrupted = false;
            if (character.statuses != null)
            {
                corrupted = character.statuses.fo(ECharacterStatus.Corrupted);
            }
            if (corrupted)
            {
                newList.Add(character);
            }
        }
        string line = string.Format("There are {0} Corrupted visitors", newList.Count);
        ActedInfo actedInfo = new ActedInfo(line);
        return actedInfo;
    }
    public override ActedInfo bcr(Character charRef)
    {
        return bcq(charRef);
    }
    public override void bcs(ETriggerPhase trigger, Character charRef)
    {
        if (trigger == ETriggerPhase.Start)
        {
            charRef.statuses.fm(ECharacterStatus.CorruptionResistant, charRef);
            charRef.statuses.fm(ECharacterStatus.UnkillableByDemon, charRef);
        }
        else if (trigger == ETriggerPhase.Day)
        {
            this.onActed.Invoke(this.bcq(charRef));
        }
    }
    public override void bcx(ETriggerPhase trigger, Character charRef)
    {
        this.bcs(trigger, charRef);
    }

    public Tavernkeeper() : base(ClassInjector.DerivedConstructorPointer<Tavernkeeper>())
    {
        ClassInjector.DerivedConstructorBody((Il2CppObjectBase)this);
    }
    public Tavernkeeper(IntPtr ptr) : base(ptr)
    {

    }
}