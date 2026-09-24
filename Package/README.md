# Tasty Adrenaline

Tasty Adrenaline is a small mod that adds configurable adrenaline interactions to Tasty Mead. The default behaviour is to add a tiny adrenaline gain during the duration of the Tasty Mead effect. This is intended to keep adrenaline from decaying at key times, so you can get your trinket to activate, but can be configured for other usecases!

## Effect

- Drinking Tasty Mead applies its normal status effect.
- While Tasty Mead is active, gain a configurable amount of adrenaline (default 1) on a 2-second interval.
- The effect lasts 10 seconds and can be configured with a first and final tick override. Both default to `0`.

## Configuration

In `BepInEx/config/com.celwood.tastyadrenaline.cfg` you can configure:

- `AdrenalinePerTick`: the normal amount gained on each regular tick. Defaults to `1`, resulting in 6 total adrenaline.
- `InitialAdrenalineAmount`: amount gained immediately at the start. Defaults to `0`, which uses `AdrenalinePerTick` instead.
- `FinalAdrenalineAmount`: amount gained on the final tick. Defaults to `0`, which uses `AdrenalinePerTick` instead.
- `AdrenalineMultiplier`: multiplier applied to other positive adrenaline gains while Tasty Mead is active. This does not include the adrenaline gain from all sources of tasty adrenaline (ticks, initial boost and final boost). Defaults to `1` (ie, disabled).

## Installation

Install these dependencies through your preferred Valheim mod manager:

- BepInExPack Valheim
- Jotunn

Place `TastyAdrenaline.dll` in your Valheim `BepInEx/plugins` folder, then launch the game.

## Issues / Support
Feel free to let me know if you experience any bugs, or have ideas for features or balance suggestions!
