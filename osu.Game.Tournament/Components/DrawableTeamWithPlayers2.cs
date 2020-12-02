// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Linq;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Game.Graphics;
using osu.Game.Tournament.Models;
using osu.Game.Users;
using osuTK;
using osuTK.Graphics;

namespace osu.Game.Tournament.Components
{
    public class DrawableTeamWithPlayers2 : CompositeDrawable
    {
        public DrawableTeamWithPlayers2(TournamentTeam team, TeamColour colour)
        {
            AutoSizeAxes = Axes.Both;

            InternalChildren = new Drawable[]
            {
                new FillFlowContainer
                {
                    AutoSizeAxes = Axes.Both,
                    Direction = FillDirection.Vertical,
                    Spacing = new Vector2(30),
                    Children = new Drawable[]
                    {
                        new FillFlowContainer
                        {
                            Anchor = colour == TeamColour.Red ? Anchor.TopRight : Anchor.TopLeft,
                            Origin = colour == TeamColour.Red ? Anchor.TopRight : Anchor.TopLeft,
                            AutoSizeAxes = Axes.Both,
                            Direction = FillDirection.Horizontal,
                            Padding = new MarginPadding { Left = 10 },
                            Spacing = new Vector2(30),
                            Children = new Drawable[]
                            {

                                new FillFlowContainer
                                {
                                    Direction = FillDirection.Vertical,
                                    AutoSizeAxes = Axes.Both,
                                    ChildrenEnumerable = team?.Players.Select(createPlayerText) ?? Enumerable.Empty<Drawable>()
                                }
                            }
                        },
                        new DrawableTeamTitleWithHeader2(team, colour)
                        {
                            Anchor = colour == TeamColour.Red ? Anchor.TopRight : Anchor.TopLeft,
                            Origin = colour == TeamColour.Red ? Anchor.TopRight : Anchor.TopLeft,
                        },
                    }
                },
            };

            TournamentSpriteText createPlayerText(User p) =>
                new TournamentSpriteText
                {
                    Anchor = colour == TeamColour.Red ? Anchor.TopRight : Anchor.TopLeft,
                    Origin = colour == TeamColour.Red ? Anchor.TopRight : Anchor.TopLeft,
                    Text = p.Username,
                    Font = OsuFont.Torus.With(size: 24, weight: FontWeight.SemiBold),
                    Colour = TournamentGame.TEXT_COLOUR,
                };
        }
    }
}
