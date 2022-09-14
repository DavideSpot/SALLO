using System;
using UnityEngine;
using UnityEditor;
using System.IO;
using UnityEditor.Scripting.Python;
using Python.Runtime;
using System.Collections;
using UnityEngine.Events;

namespace SALLO
{
    /// <summary>
    /// <strong>Psychophysical method</strong> component. Implements the QUEST adaptive procedure <see href="https://doi.org/10.3758/BF03202828"/>.
    /// </summary>
    /// <remarks>
    /// C# wrapper class. Internally, it calls the methods the <c>QuestClient</c> python class exposes, see the <see cref="./QuestZClient.py"/> <c>Python</c> file for more details.
    /// It uses the <c>Quest</c> python class imported from the <c>VisionEgg</c> python package <see href="https://visionegg.readthedocs.io/en/latest/Quest.html"/>
    /// The communication between Unity and Python relies on the Unity Package <c>Python for Unity</c> <see href="https://docs.unity3d.com/Packages/com.unity.scripting.python@2.1/manual/index.html"/>
    /// </remarks>
    public class pyQuest : PsyMethod
    {
        /// <summary>
        /// client name, hard-coded inside the QuestZClient.py file
        /// </summary>
        const string ClientName = "Quest Z Client";

        /// <summary>
        /// Quest update step size in degrees
        /// </summary>
        public static float grain = 1f;
        /// <summary>
        /// Range of possible values the Quest can probe, in degrees.
        /// </summary>
        /// <remarks>The final range will be [-range/2,+range/2].
        /// </remarks>
        public static int range = 10;
        /// <summary>
        /// Decide whether to add some gaussian random noise to the Quest estimate. The noise standard deviation is hard-coded to be 5% of the range under investigation in the QuestZClient.py file.
        /// </summary>
        public static bool addNoise = true;

        /// <summary>
        /// counter to keep track of the number of updates whose step was below a given <see cref="threshold"/>  Part of the convergence check procedure
        /// </summary>
        [ObsoleteAttribute("Arbitrary convergence check procedure. Use discouraged")]
        private static int counter = 0;
        /// <summary>
        /// Quest Update step size threshold to increase the <see cref="counter"/>.  Part of the convergence check procedure.
        /// </summary>
        [ObsoleteAttribute("Arbitrary convergence check procedure. Use discouraged")]
        private static float threshold { get => range * 0.01f; }

        /// <summary>
        /// number the <see cref="counter"/> must reach to consider the Quest procedure has converged and stop it.   Part of the convergence check procedure.
        /// </summary>
        [ObsoleteAttribute("Arbitrary convergence check procedure. Use discouraged")]
        private const int stopcriterion = 10;
        
        /// <summary>
        /// quest value obtained at the previous update call, without noise regardless of the <see cref="addNoise"/> value.
        /// </summary>
        private static float oldbest = 0.0f;
        /// <summary>
        /// quest value obtained at the current update call, without noise regardless of the <see cref="addNoise"/> value.
        /// </summary>
        private static float thisbest = 0.0f;
        /// <summary>
        /// quest value actually probed at the current update call.
        /// </summary>
        public static float thisquery = 0.0f;

        /// <summary>
        /// Decide whether to check the Quest procedure convergence.Part of the convergence check procedure.
        /// </summary>
        [ObsoleteAttribute("Arbitrary convergence check procedure. Keep at false value")]
        public static bool checkConvergence = false;

        /// <summary>
        /// Unity Action triggered as the Quest procedure converges.
        /// <seealso cref="UnityAction"/>
        /// </summary>
        [ObsoleteAttribute("Arbitrary convergence check procedure. Keep at false value")]
        public static UnityAction Converged;

        /// <summary>
        /// Hack to get the current file's directory
        /// </summary>
        /// <param name="fileName">Leave it blank to the current file's directory</param>
        /// <returns></returns>
        private static string __DIR__([System.Runtime.CompilerServices.CallerFilePath] string fileName = "")
        {
            string mypath = Path.GetDirectoryName(fileName);
            Debug.Log($"path is {mypath}");
            return Path.GetDirectoryName(fileName);
        }

        /// <summary>
        /// Method to launch the python client. Call on session start. Waits until the python client is active
        /// </summary>
        public static void LaunchPy()
        {
            SpawnClient();
            
            IEnumerator Waiter = PythonRunner.WaitForConnection(ClientName);
            while (Waiter.MoveNext())
                continue;

            init_quest();
        }

        /// <summary>
        /// Opens a communication with a python client aware of the QuestClient python class
        /// </summary>
        [MenuItem("Python/Examples/Quest Example/Launch Client")]
        public static void SpawnClient()
        {
            PythonRunner.SpawnClient(
                        file: $"{__DIR__()}/QuestZClient.py",
                        wantLogging: true);
        }

        /// <summary>
        /// Instantiate a Quest python object
        /// </summary>
        /// <remarks>
        /// Calls the <c>QuestClient.exposed_initQuest</c> python method in the available python client
        /// </remarks>
        [MenuItem("Python/Examples/Quest Example/Init Quest")]
        private static void init_quest()
        {
            float pGrain = grain / (float)range;
            PythonRunner.CallServiceOnClient(ClientName, "initQuest", pGrain);
        }
        /// <summary>
        /// Get the current value the Quest instance recommends
        /// </summary>
        /// <remarks>
        /// Calls the <c>QuestClient.exposed_query_quest</c> python method in the available python client
        /// </remarks>
        [MenuItem("Python/Examples/Quest Example/query quest")]
        public static float query_quest()
        {
            using (Py.GIL())
            {
                // If it is not needed, the return value can safely be discarded.
                dynamic pybest = PythonRunner.CallServiceOnClient(ClientName, "query_quest", false, false);
                thisbest = MapQuest(UnPy<float>(pybest));
                dynamic pyreal = PythonRunner.CallServiceOnClient(ClientName, "query_quest", pyQuest.addNoise);
                thisquery = MapQuest(UnPy<float>(pyreal));
                Debug.Log($"quest says {thisbest} degs. Testing {thisquery}");
                return thisquery;
            }
        }

        /// <summary>
        /// Update the Quest instance according to the participant's answers
        /// </summary>
        /// <remarks>
        /// Calls the <c>QuestClient.exposed_update_quest</c> python method in the available python client
        /// </remarks>
        public static void update_quest(int response)
        {
            PythonRunner.CallServiceOnClient(ClientName, "update_quest", response);
            Debug.Log($"Tested {thisquery} degs. Answer is {response}");
            if (checkConvergence)
                CheckConvergence();
            oldbest = thisbest;
        }

        /// <summary>
        /// Reset the Quest instance
        /// </summary>
        /// <remarks>
        /// Calls the <c>QuestClient.exposed_query_quest</c> python method in the available python client
        /// </remarks>
        [MenuItem("Python/Examples/Quest Example/restart quest")]
        public static void restart_quest()
        {
            PythonRunner.CallServiceOnClient(ClientName, "restart_quest");
            counter = 0;
        }

        /// <summary>
        /// Method to close the python client.
        /// </summary>
        [MenuItem("Python/Examples/Quest Example/close client")]
        public static void StopPy()
        {
            PythonRunner.CloseClient(
                        ClientName,
                        inviteRetry: false);
        }

        /// <summary>
        /// import the values from python to Unity
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Pystimulus"></param>
        /// <returns></returns>
        public static T UnPy<T>(dynamic Pystimulus)
        {
            T stimulus = Pystimulus;
            return stimulus;
        }

        /// <summary>
        /// Scale the values the Quest instance returns in the desired inerval
        /// </summary>
        /// <param name="Zstimulus">The value returned by the Quest instance, always in the interval[0,1]</param>
        /// <returns>The value scaled to match the desired interval [-range/2,+range/2] </returns>
        public static float MapQuest(float Zstimulus) => (Zstimulus - 0.5f) * pyQuest.range;

        /// <summary>
        /// Scale the values back to the [0,1] interval
        /// </summary>
        /// <param name="Mapped">The value in the interval [-range/2,+range/2]</param>
        /// <returns>The value scaled to the interval[0,1] </returns>
        public static float UnmapQuest(float Mapped) => (Mapped / pyQuest.range) + 0.5f;

        /// <summary>
        /// Check whether the Quest procedure has converged.
        /// </summary>
        [Obsolete("Arbitrary convergence check procedure. Use discouraged")]
        private static void CheckConvergence()
        {
            counter =
                Mathf.Abs(thisbest - oldbest) < threshold ?
                counter + 1 : 0;
            if (counter >= stopcriterion)
                Converged?.Invoke();
        }
    }
}
