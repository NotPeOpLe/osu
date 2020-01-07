using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Game.Graphics.Cursor;
using osu.Game.Tournament.Screens.Drawings;

namespace osu.Game.Tournament.Tests.Screens
{
    public class TestSceneDrawingsScreen : TournamentTestScene
    {
        [BackgroundDependencyLoader]
        private void load()
        {
            Add(new OsuContextMenuContainer
            {
                RelativeSizeAxes = Axes.Both,
                Child = new DrawingsScreen()
            });
        }
    }
}
