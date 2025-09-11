using Il2Cpp;
using MelonLoader;
using UnityEngine;
using Il2CppInterop.Runtime.Injection;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppTMPro;
using System;

namespace DrunksMode;

[RegisterTypeInIl2Cpp]
public class TavernMode : AdvancedMode
{
    public int score;
    public int tavern;
    public new int bestScore;
    public int bestTavern;
    public int corruptBonus = 10;
    public int drunkBonus = 50;
    public int bartenderBonus = 60;
    public Il2CppSystem.Action action;
    public Il2CppSystem.Action action2;
    public Il2CppSystem.Action action4;
    public int bartender_id = 0;
    public CharacterData? bartenderData;
    public override EGameMode bha()
    {
        return EGameMode.Standard;
    }
    public override void bhb()
    {
        GameplayEvents.OnRoundWon += action;
        GameplayEvents.OnDied += action2;
        UIEvents.OnUIUpdate += action4;

        GameData.CurrentVillage = this.tavern;
    }

    public override GameMode bhc()
    {
        return TavernSave.tavern;
    }

    public override void bhe()
    {
        GameplayEvents.OnRoundWon -= action;
        GameplayEvents.OnDied -= action2;
        UIEvents.OnUIUpdate -= action4;
    }

    public override int bhf()
    {
        return this.tavern;
    }
    public override int bhg()
    {
        return this.tavern;
    }

    public override void bhl()
    {
        this.bhe();
        score = 0;
        tavern = 1;
        TavernSave.tavern = this;

        UIEvents.OnUIUpdate.Invoke();
    }

    public override void bhm()
    {
        Il2CppSystem.Collections.Generic.List<Character> characters = Gameplay.CurrentCharacters;
        foreach (Character character in characters)
        {
            if (character.state != ECharacterState.Dead)
            {
                if (character.dataRef.name == "Drunk")
                {
                    addScore(drunkBonus);
                }
                else if (character.id == this.bartender_id)
                {
                    addScore(bartenderBonus);
                }
                else if (character.statuses.fo(ECharacterStatus.Corrupted))
                {
                    addScore(corruptBonus);
                }
            }
        }
        UIEvents.OnUIUpdate.Invoke();
        this.bartender_id = 0;
        TavernSave.tavern = this;
    }

    public override void bhn(int score, int level)
    {
        if (this.tavern < level)
        {
            this.tavern = level;
        }
    }

    public override int bho()
    {
        return this.score;
    }

    public override int bhq()
    {
        return this.tavern;
    }

    public override bool bhr()
    {
        return false;
    }

    public override string bhs()
    {
        int bestScore = this.bestScore;
        string str1 = string.Format("<size=24>Highest Score: <color=yellow>{0} </size></color>\n<size=24><color=grey>Best Tavern: </color><color=green>{1} </size></color>", bestScore, bestTavern);
        return str1;
    }

    public override string bht()
    {
        int currentScore = this.score;
        string result = string.Format("<color=grey><size=20>Score: </color><color=yellow><size=24>{0}</color>", currentScore);
        return result;
    }

    public void bhx()
    {
        score = 0;
        tavern = 1;
        TavernSave.createPool();
        GameData.CurrentVillage = 0;

        TavernSave.tavern = this;
    }

    public bool bhy(int mod = 0)
    {
        int currentLevel = this.tavern;
        return currentLevel <= mod + 5;
    }

    public void editUI()
    {
        GameObject objScore2 = TavernSave.objTavernScore;
        GameObject objCurrentVillage2 = TavernSave.objTavern;
        if (objScore2 == null)
        {
            return;
        }
        TMP_Text textScore = objScore2.transform.FindChild("Bg/Text (TMP)").gameObject.GetComponent<TMP_Text>();
        TMP_Text textVillage = objCurrentVillage2.transform.FindChild("Bg/Text (TMP)").gameObject.GetComponent<TMP_Text>();
        if (textScore != null)
        {
            textScore.text = this.bht();
            textVillage.text = string.Format("<color=grey><size=20>Tavern: </color><color=white><size=24>{0}</color>", this.tavern);
        }
    }
    private void addScore(int amt)
    {
        score += amt;
        if (score > bestScore)
        {
            bestScore = score;
        }
    }

    public void replaceBartender(Character charRef, CharacterData newRole)
    {
        charRef.dv(newRole);
        this.bartender_id = charRef.id;
        charRef.statuses.fn();
        charRef.statuses.fm(ECharacterStatus.CorruptionResistant, charRef);
        charRef.statuses.fm(ECharacterStatus.UnkillableByDemon, charRef);
    }

    public override AscensionsData bhh()
    {
        return TavernSave.tavernData;
    }

    public override AscensionsData bhi()
    {
        return TavernSave.tavernData;
    }

    public override bool bhj()
    {
        return true;
    }

    public override int bhp()
    {
        int savedMaxStandardAscension = SavesGame.SavedMaxStandardAscension;
        return savedMaxStandardAscension;
    }

    public TavernMode() : base(ClassInjector.DerivedConstructorPointer<TavernMode>())
    {
        ClassInjector.DerivedConstructorBody((Il2CppObjectBase)this);
        action = new Action(bhm);
        action2 = new Action(bhx);
        action4 = new Action(editUI);
    }
    public TavernMode(IntPtr ptr) : base(ptr)
    {
        action = new Action(bhm);
        action2 = new Action(bhx);
        action4 = new Action(editUI);
    }
}