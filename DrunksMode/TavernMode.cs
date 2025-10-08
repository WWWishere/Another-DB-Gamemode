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
    public CharacterData? bartenderData;
    public override EGameMode GetGameMode()
    {
        return EGameMode.Standard;
    }
    public override void Init()
    {
        GameplayEvents.OnRoundWon += action;
        GameplayEvents.OnDied += action2;
        UIEvents.OnUIUpdate += action4;

        GameData.CurrentVillage = this.tavern;
    }

    public override GameMode LoadGame()
    {
        return TavernSave.tavern;
    }

    public override void DeInit()
    {
        GameplayEvents.OnRoundWon -= action;
        GameplayEvents.OnDied -= action2;
        UIEvents.OnUIUpdate -= action4;
    }

    public override int GetStartingLevel()
    {
        return this.tavern;
    }
    public override int GetResetLevel()
    {
        return this.tavern;
    }

    public override void AbandonRun()
    {
        this.DeInit();
        score = 0;
        tavern = 1;
        TavernSave.tavern = this;

        UIEvents.OnUIUpdate.Invoke();
    }

    public override void OnStageCompleted()
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
                else if (character.statuses.Contains(TavernSave.bt))
                {
                    addScore(bartenderBonus);
                }
                else if (character.statuses.Contains(ECharacterStatus.Corrupted))
                {
                    addScore(corruptBonus);
                }
            }
        }
        UIEvents.OnUIUpdate.Invoke();
        TavernSave.tavern = this;
    }

    public override void UpdateScore(int score, int level)
    {
        if (this.tavern < level)
        {
            this.tavern = level;
        }
    }

    public override int GetScore()
    {
        return this.score;
    }

    public override int GetCurrentLevel()
    {
        return this.tavern;
    }

    public override bool IsLocked()
    {
        return false;
    }

    public override string GetScores()
    {
        int bestScore = this.bestScore;
        string str1 = string.Format("<size=24>Highest Score: <color=yellow>{0} </size></color>\n<size=24><color=grey>Best Tavern: </color><color=green>{1} </size></color>", bestScore, bestTavern);
        return str1;
    }

    public override string GetSummaryScores()
    {
        return string.Format("Score: <color=yellow>{0}<size=22></color>\n\nScore resets on round loss.", this.score);
    }

    public string GetScoresText()
    {
        int currentScore = this.score;
        string result = string.Format("<color=grey><size=20>Score: </color><color=yellow><size=24>{0}</color>", currentScore);
        return result;
    }

    public new void OnFailed()
    {
        score = 0;
        tavern = 1;
        // TavernSave.createPool();
        GameData.CurrentVillage = 0;

        TavernSave.tavern = this;
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
            textScore.text = this.GetScoresText();
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
        charRef.Init(newRole);
        charRef.statuses.CheckIfCanCurePoisonAndCure();
        charRef.statuses.AddStatus(ECharacterStatus.CorruptionResistant, charRef);
        charRef.statuses.AddStatus(ECharacterStatus.UnkillableByDemon, charRef);
        charRef.statuses.AddStatus(TavernSave.bt, charRef);
    }

    public override AscensionsData GetCurrentAscension()
    {
        // return TavernSave.tavernData;
        return TavernSave.tavernDataSet;
    }

    public override AscensionsData GetPreviousAscension()
    {
        // return TavernSave.tavernData;
        return TavernSave.tavernDataSet;
    }

    public override bool CanResetLevel()
    {
        return true;
    }

    public TavernMode() : base(ClassInjector.DerivedConstructorPointer<TavernMode>())
    {
        ClassInjector.DerivedConstructorBody((Il2CppObjectBase)this);
        action = new Action(OnStageCompleted);
        action2 = new Action(OnFailed);
        action4 = new Action(editUI);
    }
    public TavernMode(IntPtr ptr) : base(ptr)
    {
        action = new Action(OnStageCompleted);
        action2 = new Action(OnFailed);
        action4 = new Action(editUI);
    }
}