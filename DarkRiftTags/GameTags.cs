namespace DarkRiftTags
{
    public class GameTags
    {
        private const ushort Shift = Tags.Game * Tags.TagsPerPlugin;

        public const ushort InitPlayer = 0 + Shift;
        public const ushort SetTarget = 1 + Shift;
        public const ushort MoveToTarget = 2 + Shift;
        public const ushort NearestSpaceObjects = 3 + Shift;
        public const ushort MessageFailed = 4 + Shift;
    }
}
