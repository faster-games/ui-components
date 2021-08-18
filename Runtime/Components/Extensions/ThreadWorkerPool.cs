using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace FasterGames.UI.Components.Extensions
{
    /// <summary>
    /// Extension methods for ThreadWorkerPool
    /// </summary>
    public static class ThreadWorkerPoolExtensions
    {
        /// <summary>
        /// Invokes work on the thread worker pool
        /// </summary>
        /// <param name="el">self</param>
        /// <param name="ts">timespan at which to execute</param>
        /// <param name="work">work</param>
        public static void Invoke(this VisualElement el, TimeSpan ts, Action work)
        {
            ThreadWorkerPool.Invoke(el, ts, work);
        }
    }
    
    /// <summary>
    /// A sibling UIDocument behavior to allow Components access to the game loop
    /// </summary>
    [RequireComponent(typeof(UIDocument))]
    public class ThreadWorkerPool: MonoBehaviour
    {
        /// <summary>
        /// All allocated pools
        /// </summary>
        private static volatile List<ThreadWorkerPool> _pools = new List<ThreadWorkerPool>();
        
        /// <summary>
        /// Invokes work on the correct pool if the element is inside a pool
        /// </summary>
        /// <param name="el">element requesting the work</param>
        /// <param name="ts">timespan at which to execute</param>
        /// <param name="work">work to run</param>
        /// <exception cref="ArgumentException">element is not inside a pool</exception>
        public static void Invoke(VisualElement el, TimeSpan ts, Action work)
        {
            foreach (var pool in _pools)
            {
                if (pool.m_Root.Contains(el))
                {
                    pool.Invoke(ts, work);
                    return;
                }
            }

            throw new ArgumentException(
                "Unable to find parent pool. Are you missing a ThreadWorkerPool component next to your UIDocument?");
        }
        
        /// <summary>
        /// storage for the root
        /// </summary>
        private VisualElement m_Root;
        
        /// <nodoc />
        private void Awake()
        {
            m_Root = GetComponent<UIDocument>().rootVisualElement;
        }

        /// <nodoc />
        private void OnEnable()
        {
            if (!_pools.Contains(this))
            {
                _pools.Add(this);
            }
        }

        /// <nodoc />
        private void OnDisable()
        {
            if (_pools.Contains(this))
            {
                _pools.Remove(this);
            }
        }

        /// <summary>
        /// Invoke work on this pool
        /// </summary>
        /// <param name="ts">timespan at which to execute</param>
        /// <param name="work">work to execute</param>
        private void Invoke(TimeSpan ts, Action work)
        {
            StartCoroutine(CoInvoke(ts, work));
        }

        /// <summary>
        /// Helper to invoke as coroutine
        /// </summary>
        /// <param name="ts">timespan</param>
        /// <param name="work">work</param>
        /// <returns>coroutine</returns>
        private IEnumerator CoInvoke(TimeSpan ts, Action work)
        {
            while (true)
            {
                yield return new WaitForSeconds((float) ts.TotalSeconds);
                work();
            }
        }
    }
}