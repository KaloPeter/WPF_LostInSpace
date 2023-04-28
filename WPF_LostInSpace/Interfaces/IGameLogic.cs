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
        public List<GO_ControlPanel> GO_ControlPanels { get; set; }

        event EventHandler EventUpdateRender;//to update the view, after new position was set

        public List<GO_Item> GO_Items { get; set; }

        public GO_Player GO_Player { get; set; }
    }
}
