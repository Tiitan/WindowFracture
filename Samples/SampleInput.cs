using UnityEngine;

#if ENABLE_INPUT_SYSTEM && !ENABLE_LEGACY_INPUT_MANAGER
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
#endif

namespace WindowFracture.Sample
{
    internal static class SampleInput
    {
        public static bool GetMouseButtonDown(int button)
        {
#if ENABLE_INPUT_SYSTEM && !ENABLE_LEGACY_INPUT_MANAGER
            if (Mouse.current == null)
                return false;

            return button switch
            {
                0 => Mouse.current.leftButton.wasPressedThisFrame,
                1 => Mouse.current.rightButton.wasPressedThisFrame,
                2 => Mouse.current.middleButton.wasPressedThisFrame,
                _ => false
            };
#else
            return Input.GetMouseButtonDown(button);
#endif
        }

        public static bool GetKey(KeyCode key)
        {
#if ENABLE_INPUT_SYSTEM && !ENABLE_LEGACY_INPUT_MANAGER
            var keyboard = Keyboard.current;
            if (keyboard == null)
                return false;

            var control = ResolveKeyControl(key, keyboard);
            return control != null && control.isPressed;
#else
            return Input.GetKey(key);
#endif
        }

        public static bool GetKeyDown(KeyCode key)
        {
#if ENABLE_INPUT_SYSTEM && !ENABLE_LEGACY_INPUT_MANAGER
            var keyboard = Keyboard.current;
            if (keyboard == null)
                return false;

            var control = ResolveKeyControl(key, keyboard);
            return control != null && control.wasPressedThisFrame;
#else
            return Input.GetKeyDown(key);
#endif
        }

        public static float GetMouseScrollY()
        {
#if ENABLE_INPUT_SYSTEM && !ENABLE_LEGACY_INPUT_MANAGER
            return Mouse.current == null ? 0f : Mouse.current.scroll.ReadValue().y;
#else
            return Input.mouseScrollDelta.y;
#endif
        }

        public static float GetMouseDeltaX()
        {
#if ENABLE_INPUT_SYSTEM && !ENABLE_LEGACY_INPUT_MANAGER
            return Mouse.current == null ? 0f : Mouse.current.delta.ReadValue().x;
#else
            return Input.GetAxis("Mouse X");
#endif
        }

        public static float GetMouseDeltaY()
        {
#if ENABLE_INPUT_SYSTEM && !ENABLE_LEGACY_INPUT_MANAGER
            return Mouse.current == null ? 0f : Mouse.current.delta.ReadValue().y;
#else
            return Input.GetAxis("Mouse Y");
#endif
        }

        public static bool IsForwardPressed()
        {
            return GetKey(KeyCode.Z) || GetKey(KeyCode.W);
        }

        public static bool IsLeftPressed()
        {
            return GetKey(KeyCode.Q) || GetKey(KeyCode.A);
        }

#if ENABLE_INPUT_SYSTEM && !ENABLE_LEGACY_INPUT_MANAGER
        private static KeyControl ResolveKeyControl(KeyCode keyCode, Keyboard keyboard)
        {
            return keyCode switch
            {
                KeyCode.A => keyboard.aKey,
                KeyCode.B => keyboard.bKey,
                KeyCode.C => keyboard.cKey,
                KeyCode.D => keyboard.dKey,
                KeyCode.E => keyboard.eKey,
                KeyCode.F => keyboard.fKey,
                KeyCode.G => keyboard.gKey,
                KeyCode.H => keyboard.hKey,
                KeyCode.I => keyboard.iKey,
                KeyCode.J => keyboard.jKey,
                KeyCode.K => keyboard.kKey,
                KeyCode.L => keyboard.lKey,
                KeyCode.M => keyboard.mKey,
                KeyCode.N => keyboard.nKey,
                KeyCode.O => keyboard.oKey,
                KeyCode.P => keyboard.pKey,
                KeyCode.Q => keyboard.qKey,
                KeyCode.R => keyboard.rKey,
                KeyCode.S => keyboard.sKey,
                KeyCode.T => keyboard.tKey,
                KeyCode.U => keyboard.uKey,
                KeyCode.V => keyboard.vKey,
                KeyCode.W => keyboard.wKey,
                KeyCode.X => keyboard.xKey,
                KeyCode.Y => keyboard.yKey,
                KeyCode.Z => keyboard.zKey,
                KeyCode.LeftShift => keyboard.leftShiftKey,
                KeyCode.RightShift => keyboard.rightShiftKey,
                KeyCode.Space => keyboard.spaceKey,
                KeyCode.Escape => keyboard.escapeKey,
                KeyCode.UpArrow => keyboard.upArrowKey,
                KeyCode.DownArrow => keyboard.downArrowKey,
                KeyCode.LeftArrow => keyboard.leftArrowKey,
                KeyCode.RightArrow => keyboard.rightArrowKey,
                _ => null
            };
        }
#endif
    }
}
