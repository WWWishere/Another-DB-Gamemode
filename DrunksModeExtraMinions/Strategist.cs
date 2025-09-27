using Il2Cpp;
using Il2CppInterop.Runtime.Injection;
using Il2CppInterop.Runtime.InteropTypes;
using MelonLoader;
using System;
using UnityEngine;
using HarmonyLib;
using System.Collections.Generic;

namespace DrunksModeExtraMinions;

[RegisterTypeInIl2Cpp]
public class Strategist : Minion
{
    public override ActedInfo bcq(Character charRef)
    {
        return new ActedInfo("");
    }
    public override void bcs(ETriggerPhase trigger, Character charRef)
    {
        
    }
    public void escape(Character charRef)
    {
        Characters instance = Characters.Instance;
        Il2CppSystem.Collections.Generic.List<Character> characters = instance.hi(Gameplay.CurrentCharacters);
        Il2CppSystem.Collections.Generic.List<Character> list = instance.gs(characters, ECharacterType.Minion);
        List<Character> list2 = new List<Character>();
        foreach (Character ch in list)
        {
            if (ch.id != charRef.id)
            {
                list2.Add(ch);
            }
        }
        if (list2.Count == 0)
        {
            return;
        }
        Character randomMinion = list2[UnityEngine.Random.RandomRangeInt(0, list2.Count)];
        CharacterStatuses stats1 = new CharacterStatuses();
        CharacterStatuses stats2 = new CharacterStatuses();
        stats1.targetCharacter = charRef.statuses.targetCharacter;
        foreach (ECharacterStatus status in charRef.statuses.statuses)
        {
            stats1.statuses.Add(status);
        }
        stats2.targetCharacter = randomMinion.statuses.targetCharacter;
        foreach (ECharacterStatus status2 in randomMinion.statuses.statuses)
        {
            stats2.statuses.Add(status2);
        }
        CharacterData minionData = randomMinion.dataRef;
        replaceChar(randomMinion, charRef.dataRef);
        replaceChar(charRef, minionData);
        charRef.statuses = stats2;
        randomMinion.statuses = stats1;
        charRef.em();
    }
    public void replaceChar(Character ch, CharacterData data)
    {
        ch.dataRef = data;
        ch.uses = 1;
        ch.alignment = data.startingAlignment;
    }
    public Strategist() : base(ClassInjector.DerivedConstructorPointer<Strategist>())
    {
        ClassInjector.DerivedConstructorBody((Il2CppObjectBase)this);
    }
    public Strategist(IntPtr ptr) : base(ptr)
    {

    }
}

[HarmonyPatch(typeof(Character), nameof(Character.ep))]
public static class StrategistEscapes
{
    public static bool Prefix(Character __instance)
    {
        Role role = __instance.dataRef.role;
        if (role is Strategist)
        {
            Strategist strategist = (Strategist)role;
            strategist.escape(__instance);
        }
        return true;
    }
}