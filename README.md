# DistrictShrinker

DistrictManagement is a Timberborn mod to alter District configurations.

# Usage

There values to alter are listed below:

* BeaverArmsLength: how far up beavers can build (Default 1)
* District
** Range: The max distance of the roads of a district (Default 70)
** Pathfinding: The max distance of pathfinding on roads (Default 55)
** Resource: The max distance of resource gathering and planting (Farmhouse, Forestes, etc) over terrain (Default 20)
** Terrain: The max distance of pathfinding for builders on terrain (Default 10)
* RangeModifier
** Enabled: If modifications to the district range are enabled after each drought (Default false)
** ReductionAmount: By how much the distric will shrink each cycle (Default 1)
** CyclesPerReduction: After how many cycles the range will shrink (Default 1)
** MinimumRange: The lowest range possible (Default 35)
** MaximumRange: The highest range possible (Default 125)

Change the value of the variables in the config, which is probably in BepInEx\plugins\DistrictManagement\configs\DistrictManagement.json

If the config is not showing, try to launch the game and start up a save, that should create it.

# Issues

If there's a warning message in the log about a missing JSON property on game launch, delete the config file and restart the game to recreate it.

# Contributions
PRs are always welcome on the github page!

# Changelog

## v1.0.0 - 02.01.2023
- Initial release