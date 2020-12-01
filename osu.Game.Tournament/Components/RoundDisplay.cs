// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Game.Graphics;
using osu.Game.Tournament.Models;

namespace osu.Game.Tournament.Components
{
    public class RoundDisplay : CompositeDrawable
    {
        public RoundDisplay(TournamentMatch match, bool box = false)
        {
            AutoSizeAxes = Axes.Y;
            RelativeSizeAxes = Axes.X;

            InternalChildren = new Drawable[]
            {
                new FillFlowContainer
                {
                    AutoSizeAxes = Axes.Y,
                    RelativeSizeAxes = Axes.X,
                    Direction = FillDirection.Vertical,
                    Children = new Drawable[]
                    {
                        new TournamentSpriteText
                        {
                            Anchor = Anchor.TopCentre,
                            Origin = Anchor.TopCentre,
                            Text = match.Round.Value?.Name.Value ?? "Unknown Round",
                            Font = OsuFont.Torus.With(size: 30, weight: FontWeight.SemiBold),
                            Colour = TournamentGame.TEXT_COLOUR
                        },
                    }
                }
            };
        }
    }
}
