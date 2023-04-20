using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPF_LostInSpace.GameObjects;

namespace WPF_LostInSpace.Interfaces
{
    public interface IGameLogic
    {
      public List<GO_Background> GO_Backgrounds { get; set; }

        event EventHandler EventUpdateRender;//to update the view, after new posotion was set.
    }
}
