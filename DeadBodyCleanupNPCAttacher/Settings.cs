using Mutagen.Bethesda.Synthesis.Settings;
using StringCompareSettings;
using System.Collections.Generic;

namespace DeadBodyCleanupNPCAttacher
{
    public class Settings
    {
        [SynthesisTooltip("Name of the unofficial patch from which to get the script")]
        public string UnofficialPatchName { get; set; } = "Unofficial Skyrim Special Edition Patch.esp";

        [SynthesisTooltip("Only forward the deadbodycleanup script from the unofficial patch if it was overwritten elsewhere, and skip adding the script to other NPCs")]
        public bool OnlyScriptForward { get; set; } = false;

        [SynthesisTooltip("List of string keywords to skip npc by their editor id")]
        public HashSet<StringCompareSettingContainer> SkipList { get; set; } = new()
        {
            new StringCompareSettingContainer(){ StringSetting=new StringCompareSetting(){ Name="audiotemplate" } },
            new StringCompareSettingContainer(){ StringSetting=new StringCompareSetting(){ Name="voicetype" } },
            new StringCompareSettingContainer(){ StringSetting=new StringCompareSetting(){ Name="corpse", Comment="dont touch corpse npc" } },
            new StringCompareSettingContainer(){ StringSetting=new StringCompareSetting(){ Name="dummy", Comment="some dummy npc" } },
            new StringCompareSettingContainer(){ StringSetting=new StringCompareSetting(){ Name="preset", Comment="some preset npc" } },
            new StringCompareSettingContainer(){ StringSetting=new StringCompareSetting(){ Name="cb2_", Compare=CompareType.StartsWith, Comment="dummy npc from 'Collector bags 2' mod" } },
            new StringCompareSettingContainer(){ StringSetting=new StringCompareSetting(){ Name="fstest", Compare=CompareType.StartsWith, Comment="some test npc" } },
        };
    }
}
