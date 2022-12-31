using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.System;

namespace SelfDrivingCar
{
    static class GameTime
    {
        static float _dt = 0;
        static float _accumulator = 0;
        static float _roadAcc = 0;
        static float _frameAcc = 0;
        static float _updateAcc = 0;
        static float _frameRate = 1f / 60f;
        static float _updateRate = 1f / 100f;
        static Clock _clk;

        /// <summary>
        /// Get delta time
        /// </summary>
        public static float DeltaTime { get => _dt; }

        /// <summary>
        /// Get accumulator
        /// </summary>
        public static float Accumulator { get => _accumulator; }

        /// <summary>
        /// Get accumulator
        /// </summary>
        public static float RoadAccu { get => _roadAcc; }

        /// <summary>
        /// Get frame delta time
        /// </summary>
        public static float DeltaTimeF { get => _frameAcc; }

        /// <summary>
        /// Get update delta time
        /// </summary>
        public static float DeltaTimeU { get => _updateAcc; }

        /// <summary>
        /// Get frame rate
        /// </summary>
        public static float FrameRate { get => _frameRate; }

        /// <summary>
        /// Get update rate
        /// </summary>
        public static float UpdateRate { get => _updateRate; }

        /// <summary>
        /// Start the clock
        /// </summary>
        public static void StartClock()
        {
            if (_clk == null)
            {
                _clk = new Clock();
                return;
            }

            Console.WriteLine("[GAMETIME][STARTCLOCK] Clock is already started");
        }

        /// <summary>
        /// Restart the clock
        /// </summary>
        /// <returns></returns>
        public static void RestartClock()
        {
            _dt = _clk.Restart().AsSeconds();
            _accumulator += _dt;
            _frameAcc += _dt;
            _updateAcc += _dt;
            _roadAcc += _dt;
        }

        /// <summary>
        /// Set accumulator to zero
        /// </summary>
        public static void ResetAccumulator()
        {
            _accumulator = 0;
        }

        /// <summary>
        /// Set road accumulator to zero
        /// </summary>
        public static void ResetRoadAccumulator()
        {
            _roadAcc = 0;
        }

        /// <summary>
        /// Set frame accumulator to zero
        /// </summary>
        public static void ResetFrameAcc()
        {
            _frameAcc = 0;
        }

        /// <summary>
        /// Set update accumulator to zero
        /// </summary>
        public static void ResetUpdateAcc()
        {
            _updateAcc = 0;
        }

        /// <summary>
        /// Set the frame rate
        /// </summary>
        /// <param name="rate"> number of frame in one second </param>
        public static void SetFrameRate(float rate)
        {
            if (rate == 0)
            {
                _frameRate = 0;
                return;
            }

            _frameRate = 1f / rate;
        }

        /// <summary>
        /// Set the update rate
        /// </summary>
        /// <param name="rate"> number of update in one second </param>
        public static void SetUpdateRate(float rate)
        {
            if (rate == 0)
            {
                _updateRate = 0;
                return;
            }

            _updateRate = 1f / rate;
        }
    }
}