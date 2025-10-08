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
public class Florist : Minion
{
    public override ActedInfo GetInfo(Character charRef)
    {
        return new ActedInfo("");
    }
    public override void Act(ETriggerPhase trigger, Character charRef)
    {
        if (trigger == ETriggerPhase.Start)
        {
            Characters instance = Characters.Instance;
            List<Character> list1 = instance.FilterAliveCharacters(Gameplay.CurrentCharacters);
            List<Character> list2 = instance.FilterRealCharacterType(list1, ECharacterType.Villager);
            List<Character> list3 = new List<Character>();
            foreach (Character character in list2)
            {
                if (character.dataRef.picking)
                {
                    list3.Add(character);
                }
            }
            if (list3.Count == 0)
            {
                return;
            }
            Character randomChar = list3[UnityEngine.Random.RandomRangeInt(0, list3.Count)];
            randomChar.uses = 0;
            randomChar.pickable.SetActive(false);
            randomChar.statuses.AddStatus(DrunkStatic.unusable, charRef);
            randomChar.statuses.AddStatus(ECharacterStatus.MessedUpByEvil, charRef);
        }
    }
    public override void ActOnDied(Character charRef)
    {
        List<Character> characters = Gameplay.CurrentCharacters;
        foreach (Character ch in characters)
        {
            if (ch.statuses.Contains(DrunkStatic.unusable))
            {
                ch.statuses.statuses.Remove(DrunkStatic.unusable);
                ch.RefreshCharacter();
                break;
            }
        }
    }
    public Florist() : base(ClassInjector.DerivedConstructorPointer<Florist>())
    {
        ClassInjector.DerivedConstructorBody((Il2CppObjectBase)this);
    }
    public Florist(IntPtr ptr) : base(ptr)
    {

    }
}

[HarmonyPatch(typeof(Character), nameof(Character.RefreshCharacter))]
public static class BlockUsable
{
    public static void Postfix(Character __instance)
    {
        if (__instance.statuses.Contains(DrunkStatic.unusable))
        {
            __instance.uses = 0;
            __instance.pickable.SetActive(false);
        }
    }
}

[HarmonyPatch(typeof(Character), nameof(Character.OnClick))]
public static class BlockUsable2
{
    public static void Postfix(Character __instance)
    {
        if (__instance.statuses.Contains(DrunkStatic.unusable))
        {
            __instance.uses = 0;
            __instance.pickable.SetActive(false);
        }
    }
}
