// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Game.Tournament.Models;
using osuTK;

namespace osu.Game.Tournament.Components
{
    public class DrawableTeamTitleWithHeader : CompositeDrawable
    {
        public DrawableTeamTitleWithHeader(TournamentTeam team, TeamColour colour, bool left = false)
        {
            AutoSizeAxes = Axes.Both;

            InternalChild = new FillFlowContainer
            {
                AutoSizeAxes = Axes.Both,
                Direction = FillDirection.Vertical,
                Spacing = new Vector2(0, 10),
                Children = new Drawable[]
                {
                    /*new DrawableTeamFlag(team)
                    {
                        Width = 128,
                        Height = 128,
                        Anchor = Anchor.Centre,
                        Origin = Anchor.Centre,
                    },*/
                    new DrawableTeamHeader(colour)
                    {
                        Anchor = left ? Anchor.CentreRight : Anchor.CentreLeft,
                        Origin = left ? Anchor.CentreRight : Anchor.CentreLeft,
                        Margin = new MarginPadding { Top = 70 }
                    },
                    new DrawableTeamTitle(team)
                    {
                        Anchor = left ? Anchor.CentreRight : Anchor.CentreLeft,
                        Origin = left ? Anchor.CentreRight : Anchor.CentreLeft,
                    },
                }
            };
        }
    }
}
