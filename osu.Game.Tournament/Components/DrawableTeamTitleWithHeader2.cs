// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Game.Tournament.Models;
using osuTK;

namespace osu.Game.Tournament.Components
{
    public class DrawableTeamTitleWithHeader2 : CompositeDrawable
    {
        public DrawableTeamTitleWithHeader2(TournamentTeam team, TeamColour colour)
        {
            AutoSizeAxes = Axes.Both;

            InternalChild = new FillFlowContainer
            {
                Anchor = colour == TeamColour.Red ? Anchor.TopRight : Anchor.TopLeft,
                Origin = colour == TeamColour.Red ? Anchor.TopRight : Anchor.TopLeft,
                AutoSizeAxes = Axes.Both,
                Direction = FillDirection.Vertical,
                Spacing = new Vector2(0, 10),
                Children = new Drawable[]
                {
                    new DrawableTeamHeader(colour)
                    {
                        Anchor = colour == TeamColour.Red ? Anchor.TopRight : Anchor.TopLeft,
                        Origin = colour == TeamColour.Red ? Anchor.TopRight : Anchor.TopLeft,
                    },
                    new DrawableTeamTitle2(team)
                    {
                        Anchor = colour == TeamColour.Red ? Anchor.TopRight : Anchor.TopLeft,
                        Origin = colour == TeamColour.Red ? Anchor.TopRight : Anchor.TopLeft,
                    },
                }
            };
        }
    }
}
