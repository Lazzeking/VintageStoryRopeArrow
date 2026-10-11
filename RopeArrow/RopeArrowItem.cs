using System.Text;
using Vintagestory.API.Common;
using Vintagestory.API.Config;

namespace RopeArrow;

public sealed class RopeArrowItem : Item
{
    public override void GetHeldItemInfo(ItemSlot inSlot, StringBuilder description, IWorldAccessor world, bool withDebugInfo)
    {
        base.GetHeldItemInfo(inSlot, description, world, withDebugInfo);
        description.AppendLine(Lang.Get("rope_arrow:itemdesc-arrow-rope"));
    }
}