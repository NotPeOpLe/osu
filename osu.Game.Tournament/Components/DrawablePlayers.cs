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
    public class DrawablePlayers : CompositeDrawable
    {
        public DrawablePlayers(TournamentTeam team, TeamColour colour, bool centre = false)
        {
            AutoSizeAxes = Axes.Both;
            Anchor = Anchor.TopCentre;

            InternalChildren = new Drawable[]
            {
                new FillFlowContainer
                {
                    AutoSizeAxes = Axes.Both,
                    Direction = FillDirection.Vertical,
                    Spacing = new Vector2(30),
                    Children = new Drawable[]
                    {
                        new DrawableTeamTitleWithHeader2(team, colour),
                        new FillFlowContainer
                        {
                            AutoSizeAxes = Axes.Both,
                            //Anchor = Anchor.TopCentre,
                            //Origin = Anchor.Centre,
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
                    }
                },
            };

            TournamentSpriteText createPlayerText(User p) =>
                new TournamentSpriteText
                {
                    Text = p.Username,
                    Font = OsuFont.Torus.With(size: 24, weight: FontWeight.SemiBold),
                    Colour = Color4.White,
                };
        }
    }
}
