using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace SelfDrivingCar
{
    static class Inputs
    {
        const int NBR_OF_KEYS = (int)Keyboard.Key.KeyCount;         //Number of keyboard keys
        const int NBR_OF_BUTTONS = (int)Mouse.Button.ButtonCount;   //Number of mouse button
        const int STATE_ARRAY_SIZE = NBR_OF_BUTTONS + NBR_OF_KEYS;  //Size of array of keys state
        static bool[] _oldState = new bool[STATE_ARRAY_SIZE];       //Contains the old state of keyboard's keys
        static bool[] _actState = new bool[STATE_ARRAY_SIZE];       //Contains the actual state of keyboard's keys
        static RenderWindow _window;                                //Contains the actual window

        /// <summary>
        /// Get/Set used window
        /// </summary>
        public static RenderWindow Window { get => _window; set => _window = value; }

        /// <summary>
        /// Updates the actual states of keys
        /// </summary>
        public static void Update()
        {
            //Update the old state
            Array.Copy(_actState, _oldState, _actState.Length);

            //Update the actual state
            for (int i = 0; i < STATE_ARRAY_SIZE; i++)
            {
                if (i < NBR_OF_KEYS)
                {
                    if (Keyboard.IsKeyPressed((Keyboard.Key)i))
                    {
                        _actState[i] = true;
                    }
                    else
                    {
                        _actState[i] = false;
                    }
                }
                else
                {
                    if (Mouse.IsButtonPressed((Mouse.Button)(i - NBR_OF_KEYS)))
                    {
                        _actState[i] = true;
                    }
                    else
                    {
                        _actState[i] = false;
                    }
                }
            }
        }

        /// <summary>
        /// Verify if a key is pressed
        /// </summary>
        /// <param name="key"> Key to verify </param>
        /// <returns> True if key is pressed </returns>
        public static bool IsPressed(Keyboard.Key key)
        {
            if (_actState[(int)key])
            {
                return true;
            }

            return false;
        }


        /// <summary>
        /// Verify if a mouse button is pressed
        /// </summary>
        /// <param name="button"> Button to verify </param>
        /// <returns> True if button is pressed </returns>
        public static bool IsPressed(Mouse.Button button)
        {
            if (_actState[NBR_OF_KEYS + (int)button])
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Verify if a key is clicked
        /// </summary>
        /// <param name="key"> Key to verify </param>
        /// <returns> True if key is clicked </returns>
        public static bool IsClicked(Keyboard.Key key)
        {
            if (!_oldState[(int)key] && _actState[(int)key])
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Verify if a mouse button is clicked
        /// </summary>
        /// <param name="button"> Button to verify </param>
        /// <returns> True if button is clicked </returns>
        public static bool IsClicked(Mouse.Button button)
        {
            if (!_oldState[NBR_OF_KEYS + (int)button] && _actState[NBR_OF_KEYS + (int)button])
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Verify if a key is released
        /// </summary>
        /// <param name="key"> Key to verify </param>
        /// <returns> True if key is released </returns>
        public static bool IsReleased(Keyboard.Key key)
        {
            if (_oldState[(int)key] && !_actState[(int)key])
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Verify if a mouse button is released
        /// </summary>
        /// <param name="button"> Button to verify </param>
        /// <returns> True if button is released </returns>
        public static bool IsReleased(Mouse.Button button)
        {
            if (_oldState[NBR_OF_KEYS + (int)button] && !_actState[NBR_OF_KEYS + (int)button])
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Check if key didn't change state
        /// </summary>
        /// <param name="key"> Key to check </param>
        /// <returns> True if key didn't change state </returns>
        public static bool IsSameState(Keyboard.Key key)
        {
            if (_oldState[(int)key] == _actState[(int)key])
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Check if button didn't change state
        /// </summary>
        /// <param name="button"> Button to check </param>
        /// <returns> True if button didn't change state </returns>
        public static bool IsSameState(Mouse.Button button)
        {
            if (_oldState[NBR_OF_KEYS + (int)button] == _actState[NBR_OF_KEYS + (int)button])
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Get mouse position
        /// </summary>
        /// <param name="relative"> Determine if the returned position is relative to the window </param>
        /// <returns> Mouse position </returns>
        public static Vector2f GetMousePosition(bool relative)
        {
            if (relative)
            {
                return _window.MapPixelToCoords(Mouse.GetPosition(_window));

            }
            else
            {
                return (Vector2f)Mouse.GetPosition();
            }
        }
    }
}