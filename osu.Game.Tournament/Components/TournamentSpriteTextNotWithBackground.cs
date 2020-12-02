// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Game.Graphics;

namespace osu.Game.Tournament.Components
{
    public class TournamentSpriteTextNotWithBackground : CompositeDrawable
    {
        protected readonly TournamentSpriteText Text;

        public TournamentSpriteTextNotWithBackground(string text = "")
        {
            AutoSizeAxes = Axes.Both;

            InternalChildren = new Drawable[]
            {
                Text = new TournamentSpriteText
                {
                    Colour = TournamentGame.ELEMENT_FOREGROUND_COLOUR,
                    Font = OsuFont.Torus.With(weight: FontWeight.SemiBold, size: 50),
                    Padding = new MarginPadding { Left = 10, Right = 10 },
                    Text = text
                }
            };
        }
    }
}
