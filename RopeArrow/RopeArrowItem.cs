using System.Text;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Config;

namespace RopeArrow;

public sealed class RopeArrowItem : Item
{
    private MultiTextureMeshRef? heldArrowMesh;

    public override void OnBeforeRender(ICoreClientAPI capi, ItemStack itemstack, EnumItemRenderTarget target, ref ItemRenderInfo renderinfo)
    {
        base.OnBeforeRender(capi, itemstack, target, ref renderinfo);

        if (target is not (EnumItemRenderTarget.HandTp or EnumItemRenderTarget.HandTpOff))
        {
            return;
        }

        heldArrowMesh ??= GetHeldArrowMesh(capi, itemstack);
        if (heldArrowMesh != null)
        {
            renderinfo.ModelRef = heldArrowMesh;
        }
    }

    public override void GetHeldItemInfo(ItemSlot inSlot, StringBuilder description, IWorldAccessor world, bool withDebugInfo)
    {
        base.GetHeldItemInfo(inSlot, description, world, withDebugInfo);
        description.AppendLine(Lang.Get("ropearrow:itemdesc-arrow-rope"));
    }

    private static MultiTextureMeshRef? GetHeldArrowMesh(ICoreClientAPI capi, ItemStack itemstack)
    {
        string material = itemstack.Collectible.LastCodePart();
        Item? arrow = capi.World.GetItem(new AssetLocation("game", "arrow-" + material));
        return arrow == null ? null : capi.TesselatorManager.GetDefaultItemMeshRef(arrow);
    }
}