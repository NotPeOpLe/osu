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
    public class DrawableTeamWithPlayers : CompositeDrawable
    {
        public DrawableTeamWithPlayers(TournamentTeam team, bool left = false)
        {
            FillFlowContainer players;
            TeamColour colour = left ? TeamColour.Red : TeamColour.Blue;
            InternalChildren = new Drawable[]
            {
                    new DrawableTeamTitleWithHeader(team, colour, left)
                    {
                        Anchor = left ? Anchor.CentreRight : Anchor.CentreLeft,
                        Origin = left ? Anchor.CentreRight : Anchor.CentreLeft,
                        RelativePositionAxes = Axes.Both,
                        X = (left ? -1 : 1) * 0.1f,
                        Y = 0.23f
                    },
                    players = new FillFlowContainer
                    {
                        Direction = FillDirection.Vertical,
                        AutoSizeAxes = Axes.Both,
                        //Spacing = new Vector2(0, 5),
                        //Padding = new MarginPadding(20),
                        Anchor = left ? Anchor.CentreRight : Anchor.CentreLeft,
                        Origin = left ? Anchor.CentreRight : Anchor.CentreLeft,
                        RelativePositionAxes = Axes.Both,
                        X = (left ? -1 : 1) * 0.1f,
                    },
            };

            if (team != null)
            {
                foreach (var p in team.Players)
                {
                    players.Add(new TournamentSpriteText
                    {
                        Text = p.Username,
                        Font = OsuFont.GetFont(size: 24),
                        Colour = TournamentGame.TEXT_COLOUR,
                        Anchor = left ? Anchor.CentreRight : Anchor.CentreLeft,
                        Origin = left ? Anchor.CentreRight : Anchor.CentreLeft,
                    });
                }
            }
        }    
    }
}
