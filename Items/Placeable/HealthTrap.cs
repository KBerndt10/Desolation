﻿using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace Desolation.Items.Placeable
{
    // This item shows off using 1 class for loading multiple items from the same class. This is an alternate to typical inheritance.
    // CloneNewInstances, Autoload, and a new constructor are necessary to make this work.
    // The real strength of this approach happens when you have many items that vary by small changes, like how these 2 trap items vary only by placeStyle.
    public class HealthTrap : ModItem
    {
        // Here I define some strings that will be used as the ModItem.Name, the internal name of the ModItem.
        // We use these in the ExampleMod.Tiles.HealthTrap.Drop rather than ItemType<Items.Placeable.HealthTrap>() to retrieve the correct ItemID.
        public const string HealthTrapA = "HealthTrapA";

        public const string HealthTrapB = "HealthTrapB";

        // CloneNewInstances is needed so that fields in this class are Cloned onto new instances, such as when this item is crafted or hovered over.
        // By default, the game creates new instances rather than clone. By forcing Clone, we can preserve fields per Item added by the mod while sharing the same class.
        public override bool CloneNewInstances => true;

        private int placeStyle;

        public HealthTrap()
        {
        } // An empty constructor is needed for tModLoader to attempt Autoload

        public HealthTrap(int placeStyle) // This is the real constructor we use in Autoload
        {
            this.placeStyle = placeStyle;
        }

        // We use Autoload to prevent the regular loading of this class and instead load 2 versions of this class that we provide.
        public override bool Autoload(ref string name)
        {
            // We could also call AddItem in ExampleMod.Load, but keeping the code here is a little more organized. (The approach needs the empty constructor above.)
            mod.AddItem(HealthTrapA, new HealthTrap(0));
            mod.AddItem(HealthTrapB, new HealthTrap(1));
            return false; // returning false prevents the autoload, which is what we want since we loaded the 2 versions we wanted already.
        }

        public override void SetStaticDefaults()
        {
            // In mod.AddItem above, we set ModItem.Name to either HealthTrapA or HealthTrapB via the AddItem method. We check this now to know which item we are calling SetStaticDefaults on.
            if (Name == HealthTrapA)
                DisplayName.SetDefault("Example Trap - Ichor Bullet");
            if (Name == HealthTrapB)
                DisplayName.SetDefault("Example Trap - Chlorophyte Bullet");
        }

        public override void SetDefaults()
        {
            item.useStyle = ItemUseStyleID.SwingThrow;
            item.useTurn = true;
            item.useAnimation = 15;
            item.useTime = 10;
            item.autoReuse = true;
            item.maxStack = 999;
            item.consumable = true;
            item.createTile = TileType<Tiles.HealthTrap>();
            // With all the setup above, placeStyle will be either 0 or 1 for the 2 HealthTrap instances we've loaded.
            item.placeStyle = placeStyle;
            item.width = 12;
            item.height = 12;
            item.value = 10000;
            item.mech = true; // lets you see wires while holding.
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.DartTrap);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}