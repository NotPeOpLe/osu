// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Graphics;
using osu.Framework.Input;
using osu.Framework.Input.Events;
using osu.Game.Rulesets.Edit;
using osu.Game.Rulesets.Mania.Objects;
using osu.Game.Rulesets.Mania.UI;
using osuTK.Input;

namespace osu.Game.Rulesets.Mania.Edit.Blueprints
{
    public abstract class ManiaPlacementBlueprint<T> : PlacementBlueprint,
                                                       IRequireHighFrequencyMousePosition // the playfield could be moving behind us
        where T : ManiaHitObject
    {
        protected new T HitObject => (T)base.HitObject;

        protected Column Column;

        protected ManiaPlacementBlueprint(T hitObject)
            : base(hitObject)
        {
            RelativeSizeAxes = Axes.None;
        }

        protected override bool OnMouseDown(MouseDownEvent e)
        {
            if (e.Button != MouseButton.Left)
                return false;

            if (Column == null)
                return base.OnMouseDown(e);

            HitObject.Column = Column.Index;
            BeginPlacement(true);
            return true;
        }

        public override void UpdatePosition(SnapResult result)
        {
            if (!PlacementActive)
                Column = (result as ManiaSnapResult)?.Column;
        }
    }
}
