using Game.Core;

namespace Game.Merging
{
    public static class SuperEggHelper
    {
        public static bool AlreadyAdded(string itemID)
        {
            var stash = GC.ItemsStash.Stash.classes[3];
            var group = GC.ActiveGroupSO.Group();
            var groupContains = group.Contains(itemID);
            var stashContains = stash.items.Find(t => t.item_id == itemID) != null;
            return groupContains || stashContains;
        }
    }
}