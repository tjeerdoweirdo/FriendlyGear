using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;

namespace FriendlyGear.Core
{
    /// <summary>
    /// Simple integration test and example usage of the core gameplay systems.
    /// Attach this to a GameObject in the scene to test the systems.
    /// </summary>
    public class GameplaySystemsTest : MonoBehaviour
    {
        [Header("Test Configuration")]
        public bool runTestOnStart = false;

        [Header("References")]
        public DispatcherController dispatcher;
        public AgentDefinition testAgentDefinition;
        public DistressCallDefinition testCallDefinition;

        private void Start()
        {
            if (runTestOnStart)
            {
                RunIntegrationTest();
            }
        }

        /// <summary>
        /// Run a simple integration test of all systems.
        /// </summary>
        public void RunIntegrationTest()
        {
            Debug.Log("=== Starting Gameplay Systems Integration Test ===");

            // Test 1: AgentStats
            TestAgentStats();

            // Test 2: Mission Success Calculation
            TestMissionCalculation();

            // Test 3: Audio Manager
            TestAudioManager();

            Debug.Log("=== Integration Test Complete ===");
        }

        private void TestAgentStats()
        {
            Debug.Log("Test 1: AgentStats operations");

            AgentStats stats1 = new AgentStats(50, 40, 60, 30, 70, 50, 80);
            AgentStats stats2 = new AgentStats(30, 50, 40, 20, 50, 60, 70);

            AgentStats combined = stats1 + stats2;
            Debug.Log($"Combined stats total: {combined.GetTotal()}");

            AgentStats scaled = stats1.Scaled(1.5f);
            Debug.Log($"Scaled stats total: {scaled.GetTotal()}");

            AgentStats clamped = scaled.Clamped();
            Debug.Log($"Clamped stats - Fighting: {clamped.fighting} (should be ≤ 100)");

            Debug.Log("✓ AgentStats test passed");
        }

        private void TestMissionCalculation()
        {
            Debug.Log("Test 2: Mission Success Calculation");

            if (dispatcher == null || testCallDefinition == null)
            {
                Debug.LogWarning("Cannot test mission calculation - missing references");
                return;
            }

            // Create a mock distress call instance
            GameObject callObj = new GameObject("Test Distress Call");
            DistressCallInstance testCall = callObj.AddComponent<DistressCallInstance>();
            testCall.definition = testCallDefinition;

            // Create mock agents
            List<AgentController> testAgents = new List<AgentController>();
            for (int i = 0; i < 2; i++)
            {
                GameObject agentObj = new GameObject($"Test Agent {i}");
                agentObj.AddComponent<NavMeshAgent>();
                AgentController agent = agentObj.AddComponent<AgentController>();
                
                if (testAgentDefinition != null)
                {
                    agent.definition = testAgentDefinition;
                    agent.currentStats = testAgentDefinition.baseStats;
                }
                else
                {
                    // Use default stats if no definition
                    agent.currentStats = new AgentStats(50, 50, 50, 50, 50, 50, 50);
                }

                testAgents.Add(agent);
            }

            // Create mission
            Mission testMission = new Mission(testCall, testAgents);

            // Calculate success chance
            float successChance = testMission.CalculateSuccessChance();
            Debug.Log($"Mission success chance: {successChance:P0}");

            // Resolve mission
            MissionOutcome outcome = testMission.Resolve();
            Debug.Log($"Mission outcome: {outcome}");

            // Clean up
            Destroy(callObj);
            foreach (var agent in testAgents)
            {
                if (agent != null)
                    Destroy(agent.gameObject);
            }

            Debug.Log("✓ Mission calculation test passed");
        }

        private void TestAudioManager()
        {
            Debug.Log("Test 3: Audio Manager");

            AudioManager audioManager = AudioManager.Instance;
            if (audioManager != null)
            {
                Debug.Log($"AudioManager singleton initialized: {audioManager.gameObject.name}");
                Debug.Log($"Is playing voice line: {audioManager.IsPlayingVoiceLine()}");
                Debug.Log("✓ Audio Manager test passed");
            }
            else
            {
                Debug.LogWarning("AudioManager not available");
            }
        }

        /// <summary>
        /// Test the waypoint system (requires NavMesh to be baked).
        /// </summary>
        public void TestWaypointSystem()
        {
            Debug.Log("Test 4: Waypoint System");

            // Create waypoint path
            GameObject pathObj = new GameObject("Test Waypoint Path");
            WaypointPath path = pathObj.AddComponent<WaypointPath>();

            // Create some waypoints
            Transform[] waypoints = new Transform[3];
            for (int i = 0; i < 3; i++)
            {
                GameObject waypointObj = new GameObject($"Waypoint {i}");
                waypointObj.transform.position = new Vector3(i * 5, 0, 0);
                waypointObj.transform.parent = pathObj.transform;
                waypoints[i] = waypointObj.transform;
            }
            path.waypoints = waypoints;

            Debug.Log($"Created waypoint path with {path.WaypointCount} waypoints");

            // Create a follower
            GameObject followerObj = new GameObject("Test Waypoint Follower");
            followerObj.AddComponent<NavMeshAgent>();
            WaypointFollower follower = followerObj.AddComponent<WaypointFollower>();
            follower.waypointPath = path;
            follower.autoStart = false;

            Debug.Log($"Created waypoint follower at position {follower.transform.position}");

            // Clean up
            Destroy(pathObj);
            Destroy(followerObj);

            Debug.Log("✓ Waypoint system test passed");
        }
    }
}
