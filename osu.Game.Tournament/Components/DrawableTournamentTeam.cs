// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.IO;
using JetBrains.Annotations;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.Textures;
using osu.Game.Graphics;
using osu.Game.Graphics.Sprites;
using osu.Game.Tournament.Models;

namespace osu.Game.Tournament.Components
{
    public abstract class DrawableTournamentTeam : CompositeDrawable
    {
        public readonly TournamentTeam Team;

        protected readonly Sprite Flag;
        protected readonly OsuSpriteText AcronymText;

        [UsedImplicitly]
        private Bindable<string> acronym;

        [UsedImplicitly]
        private Bindable<string> flag;

        protected DrawableTournamentTeam(TournamentTeam team)
        {
            Team = team;

            Flag = new Sprite
            {
                RelativeSizeAxes = Axes.Both,
                FillMode = FillMode.Fit
            };

            AcronymText = new OsuSpriteText
            {
                Font = OsuFont.GetFont(weight: FontWeight.Regular),
            };
        }

        [BackgroundDependencyLoader]
        private void load(TextureStore textures)
        {
            if (Team == null) return;

            (acronym = Team.Acronym.GetBoundCopy()).BindValueChanged(acronym =>
            {
                AcronymText.Text = Team.Acronym.Value;
                Team.FullName.Value = AcronymText.Text;
            }, true);
            (flag = Team.FlagName.GetBoundCopy()).BindValueChanged(acronym =>
            {
                if (long.TryParse(Team.FlagName.Value, out long userid) && userid > 0)
                {
                    if (!File.Exists($"User Avatar\\{userid}.png"))
                    {
                        try
                        {
                            if (!Directory.Exists("User Avatar")) Directory.CreateDirectory("User Avatar");

                            var fileWebRequest = new Framework.IO.Network.WebRequest($@"https://a.ppy.sh/{userid}")
                            {
                                Method = System.Net.Http.HttpMethod.Get
                            };
                            fileWebRequest.Perform();

                            File.WriteAllBytes($"User Avatar\\{userid}.png", fileWebRequest.GetResponseData());
                        }
                        catch (Exception) { }
                    }
                    else Flag.Texture = Texture.FromStream(File.OpenRead($"User Avatar\\{userid}.png"));
                }
            }, true);
        }
    }
}
