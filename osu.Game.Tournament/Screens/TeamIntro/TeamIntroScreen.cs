// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Platform;
using osu.Game.Graphics;
using osu.Game.Graphics.Sprites;
using osu.Game.Tournament.Components;
using osu.Game.Tournament.Models;
using osu.Game.Tournament.Screens.Showcase;
using osuTK;
using osuTK.Graphics;

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
                new TourneyVideo(storage.GetStream(@"BG Team - Both OWC.mp4"))
                {
                    RelativeSizeAxes = Axes.Both,
                    Loop = true,
                },
                new TournamentLogo(false),
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

            mainContainer.Children = new Drawable[]
            {
                new TeamWithPlayers(match.NewValue.Team1.Value, true)
                {
                    RelativeSizeAxes = Axes.Both,
                    Width = 0.5f,
                    Height = 0.6f,
                    Anchor = Anchor.Centre,
                    Origin = Anchor.CentreRight
                },
                new TeamWithPlayers(match.NewValue.Team2.Value)
                {
                    RelativeSizeAxes = Axes.Both,
                    Width = 0.5f,
                    Height = 0.6f,
                    Anchor = Anchor.Centre,
                    Origin = Anchor.CentreLeft
                },
                new RoundDisplay(match.NewValue)
                {
                    RelativeSizeAxes = Axes.Both,
                    Height = 0.25f,
                    Anchor = Anchor.BottomCentre,
                    Origin = Anchor.BottomCentre,
                }
            };
        }

        private class RoundDisplay : CompositeDrawable
        {
            public RoundDisplay(TournamentMatch match)
            {
                InternalChildren = new Drawable[]
                {
                    new FillFlowContainer
                    {
                        AutoSizeAxes = Axes.Both,
                        Anchor = Anchor.TopCentre,
                        Origin = Anchor.TopCentre,
                        Direction = FillDirection.Vertical,
                        Spacing = new Vector2(0, 10),
                        Children = new Drawable[]
                        {
                            new OsuSpriteText
                            {
                                Anchor = Anchor.TopCentre,
                                Origin = Anchor.TopCentre,
                                Colour = Color4.White,
                                Text = "COMING UP NEXT",
                                Spacing = new Vector2(2, 0),
                                Font = OsuFont.GetFont(size: 15, weight: FontWeight.Black)
                            },
                            new OsuSpriteText
                            {
                                Anchor = Anchor.TopCentre,
                                Origin = Anchor.TopCentre,
                                Colour = Color4.White,
                                Text = match.Round.Value?.Name.Value ?? "Unknown Round",
                                Spacing = new Vector2(10, 0),
                                Font = OsuFont.GetFont(size: 50, weight: FontWeight.Light)
                            },
                            new OsuSpriteText
                            {
                                Anchor = Anchor.TopCentre,
                                Origin = Anchor.TopCentre,
                                Colour = Color4.White,
                                Text = match.Date.Value.ToLocalTime().ToString("MM/dd HH:mm UTC+8"),
                                Font = OsuFont.GetFont(size: 20)
                            },
                        }
                    }
                };
            }
        }

        private class TeamWithPlayers : CompositeDrawable
        {
            private readonly Color4 red = new Color4(245, 93, 93, 255);
            private readonly Color4 blue = new Color4(7, 184, 245, 255);
            public TeamWithPlayers(TournamentTeam team, bool left = false)
            {
                FillFlowContainer players;
                var colour = left ? red : blue;
                InternalChildren = new Drawable[]
                {
                    new TeamDisplay(team, left ? "Red" : "Blue", colour)
                    {
                        Anchor = left ? Anchor.CentreRight : Anchor.CentreLeft,
                        Origin = Anchor.Centre,
                        RelativePositionAxes = Axes.Both,
                        X = (left ? -1 : 1) * 0.36f,
                    },
                    players = new FillFlowContainer
                    {
                        Direction = FillDirection.Vertical,
                        AutoSizeAxes = Axes.Both,
                        Spacing = new Vector2(0, 5),
                        Padding = new MarginPadding(20),
                        Anchor = left ? Anchor.CentreRight : Anchor.CentreLeft,
                        Origin = left ? Anchor.CentreRight : Anchor.CentreLeft,
                        RelativePositionAxes = Axes.Both,
                        X = (left ? -1 : 1) * 0.66f,
                    },
                };

                if (team != null)
                {
                    foreach (var p in team.Players)
                    {
                        players.Add(new OsuSpriteText
                        {
                            Text = p.Username,
                            Font = OsuFont.GetFont(size: 24),
                            Colour = colour,
                            Anchor = left ? Anchor.CentreRight : Anchor.CentreLeft,
                            Origin = left ? Anchor.CentreRight : Anchor.CentreLeft,
                        });
                    }
                }
            }

            private class TeamDisplay : DrawableTournamentTeam
            {
                public TeamDisplay(TournamentTeam team, string teamName, Color4 colour)
                    : base(team)
                {
                    AutoSizeAxes = Axes.Both;

                    Flag.Anchor = Flag.Origin = Anchor.TopCentre;
                    Flag.RelativeSizeAxes = Axes.None;
                    Flag.Size = new Vector2(300, 300);
                    Flag.Scale = new Vector2(0.4f);
                    Flag.Margin = new MarginPadding { Bottom = 20 };

                    InternalChild = new FillFlowContainer
                    {
                        AutoSizeAxes = Axes.Both,
                        Direction = FillDirection.Vertical,
                        Spacing = new Vector2(0, 5),
                        Children = new Drawable[]
                        {
                            Flag,
                            new OsuSpriteText
                            {
                                Text = team.FullName.Value ?? "???",
                                Font = TournamentFont.GetFont(size:40, weight:FontWeight.Regular),
                                Colour = Color4.White,
                                Origin = Anchor.TopCentre,
                                Anchor = Anchor.TopCentre,
                            },
                            new OsuSpriteText
                            {
                                Text = teamName,
                                Font = TournamentFont.GetFont(size:20, weight:FontWeight.Regular),
                                Colour = colour,
                                Origin = Anchor.TopCentre,
                                Anchor = Anchor.TopCentre,
                            }
                        }
                    };
                }
            }
        }
    }
}
