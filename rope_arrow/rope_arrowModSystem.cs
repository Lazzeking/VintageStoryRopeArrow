using Vintagestory.API.Common;

namespace rope_arrow;

public class rope_arrowModSystem : ModSystem
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
