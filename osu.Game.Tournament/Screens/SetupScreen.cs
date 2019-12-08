// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Game.Graphics.Sprites;
using osu.Game.Graphics.UserInterface;
using osu.Game.Graphics.UserInterfaceV2;
using osu.Game.Online.API;
using osu.Game.Overlays;
using osu.Game.Rulesets;
using osu.Game.Tournament.IPC;
using osuTK;
using osuTK.Graphics;

namespace osu.Game.Tournament.Screens
{
    public class SetupScreen : TournamentScreen, IProvideVideo
    {
        private FillFlowContainer fillFlow;

        private LoginOverlay loginOverlay;

        [Resolved]
        private MatchIPCInfo ipc { get; set; }

        [Resolved]
        private IAPIProvider api { get; set; }

        [Resolved]
        private RulesetStore rulesets { get; set; }

        [BackgroundDependencyLoader]
        private void load()
        {
            InternalChild = fillFlow = new FillFlowContainer
            {
                RelativeSizeAxes = Axes.X,
                AutoSizeAxes = Axes.Y,
                Direction = FillDirection.Vertical,
                Padding = new MarginPadding(10),
                Spacing = new Vector2(10),
            };

            api.LocalUser.BindValueChanged(_ => Schedule(reload));
            reload();
        }

        private void reload()
        {
            var fileBasedIpc = ipc as FileBasedIPC;

            fillFlow.Children = new Drawable[]
            {
                new ActionableInfo
                {
                    Label = "目前IPC來源",
                    ButtonText = "刷新",
                    Action = () =>
                    {
                        fileBasedIpc?.LocateStableStorage();
                        reload();
                    },
                    Value = fileBasedIpc?.Storage?.GetFullPath(string.Empty) ?? "未找到",
                    Failing = fileBasedIpc?.Storage == null,
                    Description = "The osu!stable installation which is currently being used as a data source. If a source is not found, make sure you have created an empty ipc.txt in your stable cutting-edge installation, and that it is registered as the default osu! install.",
                },
                new ActionableInfo
                {
                    Label = "目前帳號",
                    ButtonText = "更換帳號",
                    Action = () =>
                    {
                        api.Logout();

                        if (loginOverlay == null)
                        {
                            AddInternal(loginOverlay = new LoginOverlay
                            {
                                Anchor = Anchor.TopRight,
                                Origin = Anchor.TopRight,
                            });
                        }

                        loginOverlay.State.Value = Visibility.Visible;
                    },
                    Value = api?.LocalUser.Value.Username,
                    Failing = api?.IsLoggedIn != true,
                    Description = "為了訪問API並顯示數據，必須需要登錄帳號。",
                },
                new LabelledDropdown<RulesetInfo>
                {
                    Label = "遊戲模式",
                    Description = "決定顯示哪些統計數據以及為玩家檢索哪些排名",
                    Items = rulesets.AvailableRulesets,
                    Current = LadderInfo.Ruleset,
                },
            };
        }

        public class LabelledDropdown<T> : LabelledComponent<OsuDropdown<T>, T>
        {
            public LabelledDropdown()
                : base(true)
            {
            }

            public IEnumerable<T> Items
            {
                get => Component.Items;
                set => Component.Items = value;
            }

            protected override OsuDropdown<T> CreateComponent() => new OsuDropdown<T>
            {
                RelativeSizeAxes = Axes.X,
                Width = 0.5f,
            };
        }

        private class ActionableInfo : LabelledDrawable<Drawable>
        {
            private OsuButton button;

            public ActionableInfo()
                : base(true)
            {
            }

            public string ButtonText
            {
                set => button.Text = value;
            }

            public string Value
            {
                set => valueText.Text = value;
            }

            public bool Failing
            {
                set => valueText.Colour = value ? Color4.Red : Color4.White;
            }

            public Action Action;

            private OsuSpriteText valueText;

            protected override Drawable CreateComponent() => new Container
            {
                AutoSizeAxes = Axes.Y,
                RelativeSizeAxes = Axes.X,
                Children = new Drawable[]
                {
                    valueText = new OsuSpriteText
                    {
                        Anchor = Anchor.CentreLeft,
                        Origin = Anchor.CentreLeft,
                    },
                    button = new TriangleButton
                    {
                        Anchor = Anchor.CentreRight,
                        Origin = Anchor.CentreRight,
                        Size = new Vector2(100, 30),
                        Action = () => Action?.Invoke()
                    },
                }
            };
        }
    }
}
