# DeadBodyCleanupNPCAttacher

A [Synthesis](https://github.com) patcher for **The Elder Scrolls V: Skyrim Special Edition / Anniversary Edition** that automatically forwards and attaches `WIDeadBodyCleanupScript` to eligible NPCs across your load order.

> **Note:** This repository is a modernized rewrite/fork of the original [TokcDK/DeadBodyCleanupNPCAttacher](https://github.com), updated for .NET 8 and the latest Mutagen/Synthesis architecture—including full support for **ESL-flagged plugins** and **automated file splitting when exceeding the 255-master limit**.

---

## Why Use This Patcher?

In Skyrim, dead NPCs without `WIDeadBodyCleanupScript` may remain permanently in the world instead of being cleaned up and moved to `WIDeadBodyCleanupCell` (and having their belongings transferred to their respective Hall of the Dead coffins). Over long playthroughs with heavy modlists (such as *Lexy's LOTD*), uncleaned corpses can cause save bloat and clutter.

This patcher solves the issue in two steps:
1. **Script Forwarding:** Scans your Unofficial Patch (e.g., *Unofficial Skyrim Special Edition Patch.esp*) for NPCs that carry `WIDeadBodyCleanupScript` and forwards the script if a later mod in your load order overwrote the NPC record without it.
2. **Automatic Attachment:** Scans all winning NPC overrides and attaches `WIDeadBodyCleanupScript` (configured with `DeathContainer` and `WI` properties) to eligible `Unique` NPCs that don't already have it—while safely skipping the player, presets, summons, template-scripted NPCs, and custom EditorID keywords.

---

## Key Improvements in This Version

* **Modern Synthesis & .NET 8 Support:** Updated to `GameRelease.SkyrimSE` and modern Mutagen APIs, enabling native support for **Split Files if Max Masters Exceeded** on massive load orders (255+ masters).
* **Duplicate Script Prevention:** Prevents NPCs patched during the Unofficial Patch forwarding phase from receiving a second duplicate `WIDeadBodyCleanupScript` entry during the attachment phase.
* **Fixed Skip-List & Compiled Regex Caching:** Corrected skip-list evaluation logic and added thread-safe `RegexOptions.Compiled` caching with invalid-pattern protection for fast execution on large load orders.
* **Strict Null-Safety:** Built with `<Nullable>enable</Nullable>` and `<WarningsAsErrors>nullable</WarningsAsErrors>`.

---

## How to Install & Use in Synthesis

1. Open **Synthesis**.
2. Click **Git Repository** (the `+` button to add a patcher from GitHub) and enter:
   ```text
   https://github.com
   ```
3. *(Recommended for large modlists)* Open your **Skyrim SE Profile Settings** in Synthesis and enable **Split Files if Max Masters Exceeded** if your load order touches NPCs across more than 254 plugins.
4. **Load Order Placement:** Place this patcher near the bottom of your Synthesis patcher list (after any patchers that modify NPC records or scripts, and before inventory/loot patchers like *Open World Loot* if desired).
5. Run the patcher and enable the generated `.esp` file(s) in your mod manager.

---

## Configuration Settings

You can customize the patcher behavior directly inside the **Settings** tab in Synthesis:

| Setting | Default | Description |
| :--- | :--- | :--- |
| **UnofficialPatchName** | `Unofficial Skyrim Special Edition Patch.esp` | Plugin name from which `WIDeadBodyCleanupScript` edits are forwarded. |
| **OnlyScriptForward** | `false` | If enabled, only forwards scripts overwritten after the Unofficial Patch and skips attaching the script to other NPCs. |
| **SkipList** | *(Preconfigured keywords)* | List of EditorID rules (`Contains`, `StartsWith`, `EndsWith`, `Equals`, `Regex`) used to exclude dummy, corpse, preset, or audio-template NPCs. |

---

## Credits

* **Original Author:** [TokcDK](https://github.com)
* **Framework:** [Mutagen](https://github.com) & [Synthesis](https://github.com)
