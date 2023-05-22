using System;
using System.Collections.Generic;
using WPF_LostInSpace.GameObjects;


namespace WPF_LostInSpace.Interfaces
{
    public interface IGameLogic
    {
        List<GO_Background> GO_Backgrounds { get; set; }
        List<GO_ControlPanel> GO_ControlPanels { get; set; }

        event EventHandler EventUpdateRender;//to update the view, after new position was set

        List<GO_Item> GO_Items { get; set; }

        List<GO_Laser> GO_Lasers { get; set; }

        GO_Player GO_Player { get; set; }

        List<byte[]> Cooldown_RGB { get; set; }

    }
}
