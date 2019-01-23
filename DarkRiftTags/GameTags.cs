namespace DarkRiftTags
{
    public class GameTags
    {
        private const ushort Shift = Tags.Game * Tags.TagsPerPlugin;

        private const ushort InitPlayer = 0 + Shift;
        private const ushort SetTarget = 1 + Shift;
        private const ushort MoveToTarget = 2 + Shift;
        public const ushort NearestSpaceObjects = 3 + Shift;
        private const ushort MessageFailed = 4 + Shift;
    }
}
