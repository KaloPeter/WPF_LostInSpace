using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPF_LostInSpace.GameObjects;

namespace WPF_LostInSpace.Interfaces
{
    public interface IGameController
    {
        void MovePlayer(PlayerController pc);
    }
}
