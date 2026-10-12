using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using Vintagestory.API.Common;

namespace RopeArrow;

public class RopeArrowModSystem : ModSystem
{
	private const string ConfigFileName = "ropearrow.json";

	public static RopeArrowSettings Settings { get; private set; } = new();

	public override void Start(ICoreAPI api)
	{
		if (!TryRegisterManagedSettings(api))
		{
			LoadNativeSettings(api);
		}

		api.RegisterEntityBehaviorClass("ropearrow", typeof(RopeArrowBehavior));
		api.RegisterItemClass("RopeArrowItem", typeof(RopeArrowItem));
		api.RegisterBlockClass("RopeArrowAnchor", typeof(BlockRopeArrowAnchor));
		api.RegisterBlockClass("RopeArrowRope", typeof(BlockRopeArrowRope));
		api.RegisterBlockEntityClass("RopeArrowAnchor", typeof(BlockEntityRopeArrowAnchor));
	}

	private static bool TryRegisterManagedSettings(ICoreAPI api)
	{
		if (api.ModLoader.IsModEnabled("configlib"))
		{
			return RegisterManagedSettings(api, "ConfigLib.ConfigLibModSystem");
		}

		return api.ModLoader.IsModEnabled("configkit") && RegisterManagedSettings(api, "ConfigKit.ConfigKitModSystem");
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static bool RegisterManagedSettings(ICoreAPI api, string modSystemTypeName)
	{
		ModSystem? configSystem = api.ModLoader.GetModSystem(modSystemTypeName);
		if (configSystem == null)
		{
			api.Logger.Warning($"[Rope Arrow] Could not find {modSystemTypeName}; using the local config file instead.");
			return false;
		}

		Type[] signature = { typeof(string), typeof(object), typeof(string), typeof(Action), typeof(Action<string>), typeof(Action) };
		MethodInfo? register = configSystem.GetType().GetMethod("RegisterCustomManagedConfig", signature);
		if (register == null)
		{
			api.Logger.Warning($"[Rope Arrow] {modSystemTypeName} does not expose the managed config API; using the local config file instead.");
			return false;
		}

		try
		{
			register.Invoke(configSystem, new object?[] { "ropearrow", Settings, ConfigFileName, null, null, null });
			return true;
		}
		catch (Exception exception)
		{
			api.Logger.Error($"[Rope Arrow] Could not register settings with {modSystemTypeName}: {exception}");
			return false;
		}
	}

	private static void LoadNativeSettings(ICoreAPI api)
	{
		try
		{
			Settings = api.LoadModConfig<RopeArrowSettings>(ConfigFileName) ?? new RopeArrowSettings();
			api.StoreModConfig(Settings, ConfigFileName);
		}
		catch (Exception exception)
		{
			api.Logger.Warning($"[Rope Arrow] Could not load {ConfigFileName}; using defaults. {exception.Message}");
			Settings = new RopeArrowSettings();
		}
	}
}
