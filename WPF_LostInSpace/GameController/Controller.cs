using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WPF_LostInSpace.GameObjects;
using WPF_LostInSpace.Interfaces;

namespace WPF_LostInSpace.GameController
{
    public class Controller
    {
        private IGameController logic;

        private bool goLeft = false;
        private bool goRight = false;
        private bool shoot = false;

        public Controller(IGameController logic)
        {
            this.logic = logic;
        }


        public void KeyDown(Key key)
        {
            if (key == Key.Left)
            {
                goLeft = true;
            }

            if (key == Key.Right)
            {
                goRight = true;
            }
        }

        public void KeyUp(Key key)
        {
            if (key == Key.Left)
            {
                goLeft = false;
            }

            if (key == Key.Right)
            {
                goRight = false;
            }
        }


        public void DecideMoveDirection()
        {
            if (goLeft)
            {
                MovePlayer(Key.Left);

            }

            if (goRight)
            {
                MovePlayer(Key.Right);
            }
        }

        private void MovePlayer(Key key)
        {
            switch (key)
            {
                case Key.Left:
                    logic.MovePlayer(PlayerController.Left);
                    break;
                case Key.Right:
                    logic.MovePlayer(PlayerController.Right);
                    break;
                default: break;
            }
        }

        public void SpaceDown(Key key)
        {
            if (key == Key.Space && !shoot)
            {
                logic.ShootLaser();
                shoot = true;
            }
        }

        public void SpaceUp(Key key)
        {
            if (key == Key.Space && shoot)
            {
                shoot = false;

            }
        }
    }
}
