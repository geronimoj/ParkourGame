using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class Movement_Storage_Test
    {   
        /// <summary>
        /// Tests the Get & Set functions for direction
        /// </summary>
        [Test]
        public void Movement_Set_Get_Direction()
        {   //Test direction with expected values
            MovementDirection m = new MovementDirection
            {
                Direction = Vector3.right
            };
            //Test Get
            Debug.Assert(m.Direction == Vector3.right);
            //Test Set with a non unit vector
            m.Direction = Vector3.up * 714;
            //Test Get
            Debug.Assert(m.Direction == Vector3.up);
            //Create a big, non-linear vector
            Vector3 v = new Vector3(0.5f, 12, 0.1f);
            //Set the direction
            m.Direction = v;
            //Get the dection and hope its normalized
            Debug.Assert(m.Direction == v.normalized);
        }
        /// <summary>
        /// Tests the Get & Set functions for the horizontal speed cap
        /// </summary>
        [Test]
        public void Movement_Set_Get_MaxHozSpeed()
        {
            MovementDirection m = new MovementDirection
            {
                MaxHozSpeed = 0
            };
            //Check that hozSpeed isn't being capped if maxSpeed is 0.
            m.Direction = Vector3.right;
            m.HozSpeed = 5;
            Debug.Assert(m.MaxHozSpeed == 0);
            Debug.Assert(m.HozSpeed == 5);
            //Check that a negative maxSpeed doesn't affect hozSpeed
            m.MaxHozSpeed = -10;
            Debug.Assert(m.MaxHozSpeed == 0);
            Debug.Assert(m.HozSpeed == 5);
            //Check that maxSpeed has been changed & hozSpeed hasn't
            m.MaxHozSpeed = 20;
            Debug.Assert(m.MaxHozSpeed == 20);
            Debug.Assert(m.HozSpeed == 5);
            //Check that hozSpeed is clamped and maxHozspeed isn't changed
            m.HozSpeed = 25;
            Debug.Assert(m.MaxHozSpeed == 20);
            Debug.Assert(m.HozSpeed == 20);
            //Check that reducing maxHozSpeed also reduced hozSpeed
            m.MaxHozSpeed = 10;
            Debug.Assert(m.MaxHozSpeed == 10);
            Debug.Assert(m.HozSpeed == 10);
            //Check that setting hozSpeed to 0 doesn't change hozSpeed
            m.MaxHozSpeed = 0;
            Debug.Assert(m.MaxHozSpeed == 0);
            Debug.Assert(m.HozSpeed == 10);
        }
        /// <summary>
        /// Tests the Get & Set functions for the horizontal speed
        /// </summary>
        [Test]
        public void Movement_Set_Get_HozSpeed()
        {
            MovementDirection m = new MovementDirection();
            //Make sure speed is 0
            Debug.Assert(m.HozSpeed == 0);
            //set it to 5
            m.HozSpeed = 5f;
            //It should be zero since we don't have a direction
            Debug.Assert(m.HozSpeed == 0);
            //Set the direction
            m.Direction = Vector3.right;
            //The speed should still be 0
            Debug.Assert(m.HozSpeed == 0);
            //Give speed
            m.HozSpeed = 3.1415f;
            //The speed should now be assigned
            Debug.Assert(m.HozSpeed == 3.1415f);
            //Set speed to a negative number
            m.HozSpeed = -10;
            //Make sure negative speed results in a speed of 0
            Debug.Assert(m.HozSpeed == 0);
            //Set the max hoz speed to 0
            m.MaxHozSpeed = 0;
            //Hoz speed should not be limited
            m.HozSpeed = 10000;
            //Check if it was limited
            Debug.Assert(m.HozSpeed == 10000);
            //Assign a max hoz Speed
            m.MaxHozSpeed = 5;
            //Assign a large value
            m.HozSpeed = 250;
            //Check that the max hoz speed is clamped
            Debug.Assert(m.HozSpeed == 5);
            //Assign a lower maxHoz speed
            m.HozSpeed = 2;
            //Check the value hasn't be changed since it should be below the max speed
            Debug.Assert(m.HozSpeed == 2);
            //Assign a negative max speed
            m.MaxHozSpeed = -1;
            //Assign a speed
            m.HozSpeed = 7;
            //The speed is expected to be 7 as max speed should be 0 
            Debug.Assert(m.HozSpeed == 7);
            //Make sure hozSpeed isn't affected by vert speed
            m.VertSpeed = 20;
            Debug.Assert(m.HozSpeed == 7);
        }
        /// <summary>
        /// Tests the Get & Set functions for the vertical speed cap
        /// </summary>
        [Test]
        public void Movement_Set_Get_MaxVertSpeed()
        {
            MovementDirection m = new MovementDirection();

            Debug.Assert(m.MaxVertSpeed == 0);

            m.VertSpeed = 5;
            Debug.Assert(m.MaxVertSpeed == 0);
            Debug.Assert(m.VertSpeed == 5);

            m.VertSpeed = -10;
            Debug.Assert(m.VertSpeed == -10);

            m.MaxVertSpeed = 7;
            Debug.Assert(m.MaxVertSpeed == 7);
            Debug.Assert(m.VertSpeed == -7);

            m.VertSpeed = 20;
            Debug.Assert(m.VertSpeed == 7);

            m.MaxVertSpeed = 10;

            Debug.Assert(m.VertSpeed == 7);
        }
        /// <summary>
        /// Tests the Get & Set functions for the vertical speed
        /// </summary>
        [Test]
        public void Movement_Set_Get_VertSpeed()
        {
            MovementDirection m = new MovementDirection();
            //Check the default value
            Debug.Assert(m.VertSpeed == 0);
            //Assign a Value to VertSpeed and check that it updates the direction
            m.VertSpeed = 5;
            Debug.Assert(m.VertSpeed == 5);
            Debug.Assert(m.Direction == Vector3.up);
            //Check that vertspeed is being capped
            m.MaxVertSpeed = 3;
            Debug.Assert(m.VertSpeed == 3);
            //Check that vertspeed is being capped but the other way around
            m.VertSpeed = -10;
            Debug.Assert(m.VertSpeed == -3);
            Debug.Assert(m.Direction == -Vector3.up);
            //Check that the direction is being assigned correctly when vertSpeed is set given that there is pre-existing horizontal speed
            m.Direction = Vector3.right;
            m.HozSpeed = 10;
            m.VertSpeed = 3;
            Debug.Assert(m.VertSpeed == 3);
            Debug.Assert(m.Direction == new Vector3(10, 3, 0).normalized);
        }
        /// <summary>
        /// Tests the Get for the total speed cap
        /// </summary>
        [Test]
        public void Movement_Get_TotalMaxSpeed()
        {
            MovementDirection m = new MovementDirection
            {
                MaxHozSpeed = 3,
                MaxVertSpeed = 4
            };

            Debug.Assert(m.TotalMaxSpeed == 5);
        }
        /// <summary>
        /// Tests the Get & Set for the total speed
        /// </summary>
        [Test]
        public void Movement_Set_Get_TotalSpeed()
        {
            MovementDirection m = new MovementDirection
            {
                Direction = Vector3.right
            };

            Debug.Assert(m.TotalSpeed == 0);

            m.HozSpeed = 3;
            m.VertSpeed = 4;

            Debug.Assert(m.TotalSpeed == 5);

            Vector3 dir = m.Direction;

            m.TotalSpeed = 10;

            Debug.Assert(m.Direction == dir);
            Debug.Assert(m.TotalSpeed == 10);

            m.MaxHozSpeed = 3;
            m.MaxVertSpeed = 4;
            m.TotalSpeed = 10;

            Debug.Assert(m.TotalMaxSpeed == 5);
            Debug.Assert(m.TotalSpeed == 5);
        }
        /// <summary>
        /// Tests the Get for getting the horizontal component of the Direction as a unit vector
        /// </summary>
        [Test]
        public void Movement_Get_HozDirection()
        {
            MovementDirection m = new MovementDirection();

            Debug.Assert(m.HozDirection == Vector3.zero);

            m.Direction = Vector3.right;

            Debug.Assert(m.HozDirection == Vector3.right);

            m.Direction = Vector3.up;

            Debug.Assert(m.HozDirection == Vector3.zero);

            Vector3 v = new Vector3(10, 5, 7);

            m.Direction = v;
            v.y = 0;

            Debug.Assert(m.HozDirection == v.normalized);
        }
        /// <summary>
        /// Tests the Get for getting the total vector, direction & speed
        /// </summary>
        [Test]
        public void Movement_Get_TotalVector()
        {
            MovementDirection m = new MovementDirection();
            m.Direction = Vector3.right;
            m.HozSpeed = 10;

            Debug.Assert(m.TotalVector == Vector3.right * 10);
        }
    }
}
