using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class Input_Test
    {
        /// <summary>
        /// Tests the MyInput class alone. Kinda hard to test as it requires Input.GetAxis on Windows to function propperly and assign to
        /// value :P
        /// </summary>
        [Test]
        public void Input_Initialization()
        {   //Test the base input with everything assigned
            MyInput input = new MyInput
            {
                name = "TestInput",
                axisName = "Horizontal"
            };

            input.UpdateInput();

            Debug.Assert(input.GetValue == 0);
            Debug.Assert(input.PressedThisFrame());

            //Test the base input without assigning an axis
            MyInput input2 = new MyInput
            {
                name = "Horizontal"
            };

            Debug.Assert(input2.axisName == "NULL");

            input2.UpdateInput();

            Debug.Assert(input2.GetValue == 0);
            Debug.Assert(input2.PressedThisFrame());
        }
        /// <summary>
        /// Tests the input manager. Its kinda hard to test tho because we can't use the Input.GetAxis since it'll always return 0.
        /// Nor can we assign to MyInput a value unless we are in Android mode
        /// </summary>
        [Test]
        public void InputManager_Test()
        {
            MyInput[] inputs = new MyInput[3];
            inputs[0] = new MyInput
            {
                name = "Forward"
            };
            inputs[1] = new MyInput
            {
                name = "Jump"
            };
            inputs[2] = new MyInput
            {
                name = "Crouch"
            };
            //Manually assign the inputs
            InputManager.inputs = inputs;
            //Valid Test for Input
            Debug.Assert(InputManager.GetInput("Forward") == 0);
            //Test invalid input
            LogAssert.Expect(LogType.Error, "Invalid input name: NotAnInput");
            Debug.Assert(InputManager.GetInput("NotAnInput") == 0);
            //Test another input
            Debug.Assert(InputManager.GetInput("Crouch") == 0);
            //Valid input test
            Debug.Assert(InputManager.NewInput("Jump") == 0);
            //Test invalid input
            LogAssert.Expect(LogType.Error, "Invalid input name: NotAnInput");
            Debug.Assert(InputManager.NewInput("NotAnInput") == 0);
        }
        // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
        // yield return null;` to skip a frame.
        /*
        [UnityTest]
        public IEnumerator Input_TestWithEnumeratorPasses()
        {
            // Use the Assert class to test conditions.
            // Use yield to skip a frame.
            yield return null;
        }
        */
    }
}
