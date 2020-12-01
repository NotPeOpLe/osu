// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using JetBrains.Annotations;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics.Textures;
using osu.Game.Graphics;
using osu.Game.Tournament.Models;

namespace osu.Game.Tournament.Components
{
    public class DrawableTeamTitle : TournamentSpriteText
    {
        private readonly TournamentTeam team;

        [UsedImplicitly]
        private Bindable<string> acronym;

        public DrawableTeamTitle(TournamentTeam team)
        {
            this.team = team;
        }

        [BackgroundDependencyLoader]
        private void load(TextureStore textures)
        {
            if (team == null) return;

            (acronym = team.Acronym.GetBoundCopy()).BindValueChanged(acronym => Text = team?.FullName.Value ?? string.Empty, true);
            Font = OsuFont.Torus.With(weight: FontWeight.SemiBold, size: 50);
            Colour = TournamentGame.TEXT_COLOUR;
        }
    }
}
