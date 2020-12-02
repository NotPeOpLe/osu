// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Platform;
using osu.Game.Tournament.Components;
using osu.Game.Tournament.Models;
using osuTK;

namespace osu.Game.Tournament.Screens.TeamIntro
{
    public class TeamIntroScreen : TournamentScreen, IProvideVideo
    {
        private Container mainContainer;

        private readonly Bindable<TournamentMatch> currentMatch = new Bindable<TournamentMatch>();

        [BackgroundDependencyLoader]
        private void load(Storage storage)
        {
            RelativeSizeAxes = Axes.Both;

            InternalChildren = new Drawable[]
            {
                new TourneyVideo("teamintro")
                {
                    RelativeSizeAxes = Axes.Both,
                    Loop = true,
                },
                mainContainer = new Container
                {
                    RelativeSizeAxes = Axes.Both,
                }
            };

            currentMatch.BindValueChanged(matchChanged);
            currentMatch.BindTo(LadderInfo.CurrentMatch);
        }

        private void matchChanged(ValueChangedEvent<TournamentMatch> match)
        {
            if (match.NewValue == null)
            {
                mainContainer.Clear();
                return;
            }

            //const float y_flag_offset = 292;

            const float y_offset = 360;

            mainContainer.Children = new Drawable[]
            {
                new RoundDisplay(match.NewValue)
                {
                    Y = -100,
                    Anchor = Anchor.BottomCentre,
                    Origin = Anchor.BottomCentre,
                },
                /*new DrawableTeamFlag(match.NewValue.Team1.Value)
                {
                    Position = new Vector2(440, y_flag_offset),
                },*/
                new DrawableTeamWithPlayers2(match.NewValue.Team1.Value, TeamColour.Red)
                {
                    Origin = Anchor.TopRight,
                    Position = new Vector2(600, y_offset),
                },
                /*new DrawableTeamFlag(match.NewValue.Team2.Value)
                {
                    Position = new Vector2(760, y_flag_offset),
                },*/
                new DrawableTeamWithPlayers2(match.NewValue.Team2.Value, TeamColour.Blue)
                {
                    Position = new Vector2(760, y_offset),
                },
            };
        }
    }
}
