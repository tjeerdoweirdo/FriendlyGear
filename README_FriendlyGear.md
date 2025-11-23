# Friendly Gear — Dispatch Tower Defense Scaffold

This repo includes a foundational architecture for a dispatch-driven tower defense. It provides data-driven Agents, Abilities, and Distress Calls, plus runtime systems for missions, spawners, basic navigation, and audio banter hooks.

## Core Concepts

- Agents: Scriptable definitions with base stats and abilities, instantiated as `AgentController` with NavMesh navigation.
- Distress Calls: Scriptable definitions with weighted stat requirements and durations.
- Dispatching: Select agents and send to active distress calls; success chances depend on stat alignment, luck, stability, and difficulty.
- Navigation: Uses `NavMeshAgent`; optional WaypointPath support.
- Spawners: Friendly base spawns agents; enemy spawners are placeholders.
- Banter: Simple audio hooks for agent/dispatcher/distress VO.

## Files & Folders

- `Assets/Unity/FriendlyGear/Scripts/Data`
  - `AgentDefinition`, `AbilityDefinition`, `DistressCallDefinition`, `StatType`
- `Assets/Unity/FriendlyGear/Scripts/Runtime`
  - `AgentController`, `MissionManager`, `DistressCallInstance`, `GameManager`
  - `BaseSpawner`, `EnemySpawner`, `DispatcherUIManager`
  - `Waypoint`, `WaypointPath`, `StatBlock`, `BanterSystem`

## Quick Setup (Scene)

1. Create an empty scene and save it.
2. Add GameObjects:
   - `GameManager` (attach `GameManager`)
   - `MissionManager` (attach `MissionManager`)
   - `Base` (attach `BaseSpawner`, set a `spawnPoint` child)
   - Optional `EnemySpawner` objects with an `enemyPrefab`
   - `DispatcherUI` (attach `DispatcherUIManager`)
   - `BanterSystem` with an `AudioSource`
3. Bake NavMesh: Add a `NavMeshSurface` (from AI Navigation package), mark walkable geometry as Navigation Static, then Bake.
4. Create Agent Prefab:
   - Make a simple capsule, add `NavMeshAgent` and `AgentController`.
   - Add an `AudioSource`.
   - Prefab this under `Assets/Prefabs/Agents`.
5. Create ScriptableObjects:
   - Right-click in Project → Create → `FriendlyGear/Agent` and assign the prefab to `agentPrefab`.
   - Create abilities via `FriendlyGear/Ability` and assign them to the agent’s abilities.
   - Create one or more `FriendlyGear/Distress Call` definitions.
6. Wire references:
   - On `BaseSpawner`, add your `AgentDefinition` to `startingAgents` and set `spawnPoint`.
   - On `MissionManager`, adjust `travelTimePerMeter` and `successCurve` if desired.
   - On `DispatcherUIManager`, reference the `MissionManager` and `BaseSpawner` (or leave to auto-find).

## Minimal Flow (No UI)

- At runtime, `BaseSpawner` spawns starting agents.
- To trigger a distress call from script:
  ```csharp
  // Example: create and dispatch one available agent
  var ui = FindObjectOfType<FriendlyGear.Runtime.DispatcherUIManager>();
  var mission = FindObjectOfType<FriendlyGear.Runtime.MissionManager>();
  var def = /* reference to a DistressCallDefinition */;
  var call = mission.CreateDistress(def, new Vector3(10, 0, 10));
  if (ui.TrySelectAnyAvailable()) ui.DispatchSelectedTo(call);
  ```
- Agents path to the call using NavMesh. When the timer elapses, the mission resolves with success/fail based on stats.

## Stats & Success

- Agent base stats are in `AgentDefinition`.
- Abilities apply flat then percentage modifiers while active.
- Success is shaped by a weighted alignment between agent stats and distress call weights, with luck/stability/difficulty influences.

## Extending

- Add real enemy AI and combat resolution.
- Build UI for selecting agents, viewing calls, and dispatching.
- Save/load agent upgrades and progression.
- Expand BanterSystem to manage VO priorities and subtitles.

## Notes

- Unity will generate `.meta` files as needed.
- Ensure any agent prefab includes `NavMeshAgent` and `AgentController`.
- Audio clips are optional; systems no-op if not present.
