# Friendly Gear - Core Gameplay Scripts

This folder contains the core gameplay systems for the Friendly Gear dispatch-based tower defense game.

## Structure

### Agents/
- **AgentStats.cs** - Serializable struct for agent stats with operators and helper methods
- **AgentAbility.cs** - Abstract base class for agent abilities
- **AdrenalineBoostAbility.cs** - Example ability implementation that boosts fighting and mobility
- **AgentDefinition.cs** - ScriptableObject defining agent characteristics
- **AgentController.cs** - MonoBehaviour for agent movement and mission behavior (requires NavMeshAgent)

### Distress/
- **DistressCallDefinition.cs** - ScriptableObject defining distress call properties
- **DistressCallInstance.cs** - MonoBehaviour for active distress calls in the scene

### Missions/
- **MissionOutcome.cs** - Enum for mission resolution outcomes
- **Mission.cs** - Core mission class handling success calculation and resolution

### Dispatcher/
- **DispatcherController.cs** - Main controller managing agents, missions, and dispatch logic

### Waypoints/
- **WaypointPath.cs** - MonoBehaviour holding an ordered list of waypoint transforms
- **WaypointFollower.cs** - Generic NavMeshAgent-based waypoint following component

### Audio/
- **AudioManager.cs** - Singleton for playing voice lines and audio

## Usage

All classes are in the `FriendlyGear.Core` namespace.

### Creating Agent Definitions
1. Right-click in Project window → Create → FriendlyGear → Agent Definition
2. Configure agent ID, display name, portrait, stats, and abilities

### Creating Distress Call Definitions
1. Right-click in Project window → Create → FriendlyGear → Distress Call Definition
2. Configure call properties, required stats, and difficulty

### Creating Abilities
1. Right-click in Project window → Create → FriendlyGear → Abilities → Adrenaline Boost
2. Configure stat boosts or create custom abilities by extending AgentAbility

### Scene Setup
1. Add a GameObject with **DispatcherController** component
2. Assign the base location Transform
3. Create GameObjects with **AgentController** components (requires NavMeshAgent)
4. Create GameObjects with **DistressCallInstance** components at call locations
5. Assign definitions to instances
6. Optionally add **AudioManager** to the scene for voice lines

## Dependencies
- Unity AI Navigation package (com.unity.ai.navigation) for NavMesh support
- Unity 2023.2 or later
