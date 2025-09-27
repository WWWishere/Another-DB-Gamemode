using Il2Cpp;
using Il2CppInterop.Runtime.Injection;
using Il2CppInterop.Runtime.InteropTypes;
using MelonLoader;
using System;
using UnityEngine;
using HarmonyLib;
using Il2CppSystem.Collections.Generic;

namespace DrunksModeExtraMinions;

[RegisterTypeInIl2Cpp]
public class Trickster : Minion
{
    public override ActedInfo bcq(Character charRef)
    {
        return new ActedInfo("");
    }
    public override void bcs(ETriggerPhase trigger, Character charRef)
    {
        List<Character> characters = Gameplay.CurrentCharacters;
        List<Character> list1 = CharactersHelper.tl(characters, charRef);
        List<Character> list2 = new List<Character>();
        list2.Add(list1[1]);
        list2.Add(list1[list1.Count - 1]);
        foreach (Character neighbor in list2)
        {
            if (neighbor.dataRef.type == ECharacterType.Villager && neighbor.alignment == EAlignment.Good)
            {
                neighbor.ek(charRef.dataRef);
                neighbor.statuses.fm(ECharacterStatus.MessedUpByEvil, charRef);
            }
        }
    }
    public Trickster() : base(ClassInjector.DerivedConstructorPointer<Trickster>())
    {
        ClassInjector.DerivedConstructorBody((Il2CppObjectBase)this);
    }
    public Trickster(IntPtr ptr) : base(ptr)
    {

    }
}
