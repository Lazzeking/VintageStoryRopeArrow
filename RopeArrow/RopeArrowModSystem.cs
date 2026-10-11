using Vintagestory.API.Common;

namespace RopeArrow;

public class RopeArrowModSystem : ModSystem
{
	public override void Start(ICoreAPI api)
	{
		api.RegisterEntityBehaviorClass("ropearrow", typeof(RopeArrowBehavior));
		api.RegisterItemClass("RopeArrowItem", typeof(RopeArrowItem));
		api.RegisterBlockClass("RopeArrowAnchor", typeof(BlockRopeArrowAnchor));
		api.RegisterBlockClass("RopeArrowRope", typeof(BlockRopeArrowRope));
		api.RegisterBlockEntityClass("RopeArrowAnchor", typeof(BlockEntityRopeArrowAnchor));
	}
}
