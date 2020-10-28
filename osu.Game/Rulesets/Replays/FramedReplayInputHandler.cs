﻿// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using JetBrains.Annotations;
using osu.Game.Input.Handlers;
using osu.Game.Replays;

namespace osu.Game.Rulesets.Replays
{
    /// <summary>
    /// The ReplayHandler will take a replay and handle the propagation of updates to the input stack.
    /// It handles logic of any frames which *must* be executed.
    /// </summary>
    public abstract class FramedReplayInputHandler<TFrame> : ReplayInputHandler
        where TFrame : ReplayFrame
    {
        private readonly Replay replay;

        protected List<ReplayFrame> Frames => replay.Frames;

        public TFrame CurrentFrame
        {
            get
            {
                if (!HasFrames || !currentFrameIndex.HasValue)
                    return null;

                return (TFrame)Frames[currentFrameIndex.Value];
            }
        }

        public TFrame NextFrame
        {
            get
            {
                if (!HasFrames)
                    return null;

                if (!currentFrameIndex.HasValue)
                    return (TFrame)Frames[0];

                if (currentDirection > 0)
                    return currentFrameIndex == Frames.Count - 1 ? null : (TFrame)Frames[currentFrameIndex.Value + 1];
                else
                    return currentFrameIndex == 0 ? null : (TFrame)Frames[nextFrameIndex];
            }
        }

        private int? currentFrameIndex;

        private int nextFrameIndex => currentFrameIndex.HasValue ? Math.Clamp(currentFrameIndex.Value + (currentDirection > 0 ? 1 : -1), 0, Frames.Count - 1) : 0;

        protected FramedReplayInputHandler(Replay replay)
        {
            this.replay = replay;
        }

        private bool advanceFrame()
        {
            int newFrame = nextFrameIndex;

            // ensure we aren't at an extent.
            if (newFrame == currentFrameIndex) return false;

            currentFrameIndex = newFrame;
            return true;
        }

        private const double sixty_frame_time = 1000.0 / 60;

        protected virtual double AllowedImportantTimeSpan => sixty_frame_time * 1.2;

        protected double? CurrentTime { get; private set; }

        private int currentDirection;

        /// <summary>
        /// When set, we will ensure frames executed by nested drawables are frame-accurate to replay data.
        /// Disabling this can make replay playback smoother (useful for autoplay, currently).
        /// </summary>
        public bool FrameAccuratePlayback = false;

        public bool HasFrames => Frames.Count > 0;

        private bool inImportantSection
        {
            get
            {
                if (!HasFrames || !FrameAccuratePlayback)
                    return false;

                var frame = currentDirection > 0 ? CurrentFrame : NextFrame;

                if (frame == null)
                    return false;

                return IsImportant(frame) && // a button is in a pressed state
                       Math.Abs(CurrentTime - NextFrame?.Time ?? 0) <= AllowedImportantTimeSpan; // the next frame is within an allowable time span
            }
        }

        protected virtual bool IsImportant([NotNull] TFrame frame) => false;

        /// <summary>
        /// Update the current frame based on an incoming time value.
        /// There are cases where we return a "must-use" time value that is different from the input.
        /// This is to ensure accurate playback of replay data.
        /// </summary>
        /// <param name="time">The time which we should use for finding the current frame.</param>
        /// <returns>The usable time value. If null, we should not advance time as we do not have enough data.</returns>
        public override double? SetFrameFromTime(double time)
        {
            updateDirection(time);

            Debug.Assert(currentDirection != 0);

            if (HasFrames)
            {
                var next = NextFrame;

                // check if the next frame is valid for the current playback direction.
                // validity is if the next frame is equal or "earlier"
                var compare = time.CompareTo(next?.Time);

                if (next != null && (compare == 0 || compare == currentDirection))
                {
                    if (advanceFrame())
                        return CurrentTime = CurrentFrame.Time;
                }
                else
                {
                    // this is the case where the frame can't be advanced (in the replay).
                    // even so, we may be able to move the clock forward due to being at the end of the replay or
                    // in a section where replay accuracy doesn't matter.

                    // important section is always respected to block the update loop.
                    if (inImportantSection)
                        return null;

                    if (next == null)
                    {
                        // in the case we have no more frames and haven't received the full replay, block.
                        if (!replay.HasReceivedAllFrames)
                            return null;

                        // else allow play to end.
                    }
                    else if (next.Time.CompareTo(time) != currentDirection)
                    {
                        // in the case we have more frames, block if the next frame's time is less than the current time.
                        return null;
                    }

                    // if we didn't change frames, we need to ensure we are allowed to run frames in between, else return null.
                }
            }
            else
            {
                // if we never received frames and are expecting to, block.
                if (!replay.HasReceivedAllFrames)
                    return null;
            }

            return CurrentTime = time;
        }

        private void updateDirection(double time)
        {
            if (!CurrentTime.HasValue)
            {
                currentDirection = 1;
            }
            else
            {
                currentDirection = time.CompareTo(CurrentTime);
                if (currentDirection == 0) currentDirection = 1;
            }
        }
    }
}
