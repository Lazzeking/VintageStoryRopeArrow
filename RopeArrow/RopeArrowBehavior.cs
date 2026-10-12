using System;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.MathTools;

namespace RopeArrow;

public sealed class RopeArrowBehavior : EntityBehavior
{
    private bool finished;

    public RopeArrowBehavior(Entity entity) : base(entity)
    {
    }

    public override string PropertyName()
    {
        return "ropearrow";
    }

    public override void OnGameTick(float deltaTime)
    {
        base.OnGameTick(deltaTime);

        if (finished || entity.World.Side != EnumAppSide.Server)
        {
            return;
        }

        if (entity is not IProjectile projectile || !projectile.Stuck)
        {
            return;
        }

        projectile.Collectible = false;

        BlockPos anchorPosition = entity.Pos.AsBlockPos;
        BlockPos belowBlockCheck = anchorPosition.DownCopy();
        string facing = GetRopeFacing(entity);
        if (entity.World.BlockAccessor.GetBlock(anchorPosition).BlockId != 0)
        {
            if (entity.World.BlockAccessor.GetBlockEntity(anchorPosition) is BlockEntityRopeArrowAnchor existingAnchor &&
                existingAnchor.HasArrowEntity(entity.EntityId))
            {
                projectile.Collectible = false;
                finished = true;
                return;
            }

            projectile.Collectible = true;
            finished = true;
            return;
        }

        if (entity.World.BlockAccessor.GetBlock(belowBlockCheck).BlockId != 0)
        {
            projectile.Collectible = true;
            finished = true;
            return;
        }

        Block? anchorBlock = entity.World.GetBlock(new AssetLocation($"ropearrow:ropearrowanchor-{facing}"));
        if (anchorBlock == null)
        {
            finished = true;
            return;
        }

        entity.World.BlockAccessor.SetBlock(anchorBlock.BlockId, anchorPosition);
        if (entity.World.BlockAccessor.GetBlockEntity(anchorPosition) is BlockEntityRopeArrowAnchor anchor)
        {
            anchor.SetArrowEntity(entity, GetSourceArrowMaterial(projectile));
            anchor.StartUnspooling(facing);
        }
        else
        {
            entity.World.BlockAccessor.SetBlock(0, anchorPosition);
            projectile.Collectible = true;
            finished = true;
            return;
        }

        finished = true;
    }

    private static string GetRopeFacing(Entity projectile)
    {
        double motionX = -Math.Sin(projectile.Pos.Yaw);
        double motionZ = -Math.Cos(projectile.Pos.Yaw);

        if (Math.Abs(motionX) > Math.Abs(motionZ))
        {
            return motionX > 0 ? "east" : "west";
        }

        return motionZ > 0 ? "south" : "north";
    }

    private static string GetSourceArrowMaterial(IProjectile projectile)
    {
        return projectile.ProjectileStack?.Collectible.Variant?["material"] ?? "crude";
    }
}