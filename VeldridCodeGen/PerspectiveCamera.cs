using System;
using System.Numerics;
using UAlbion.Api;
using UAlbion.Core.SpriteBatch;
using Veldrid;

namespace UAlbion.Core
{
    public class PerspectiveCamera : Component, ICamera
    {
        readonly SingleBuffer<ViewMatrix> _viewMatrix = new(new ViewMatrix(), BufferUsage.UniformBuffer, "M_View");
        readonly SingleBuffer<ProjectionMatrix> _projectionMatrix = new(new ProjectionMatrix(), BufferUsage.UniformBuffer, "M_Projection");

        Vector3 _position = new(0, 0, 0);
        Vector3 _lookDirection = new(0, -.3f, -1f);
        Vector2 _viewport = Vector2.One;

        float _yaw;
        float _pitch;
        bool _useReverseDepth;
        bool _isClipSpaceYInverted;
        bool _depthZeroToOne;
        bool _dirty = true;

        public SingleBuffer<ViewMatrix> ViewMatrix { get { Recalculate(); return _viewMatrix; } }
        public SingleBuffer<ProjectionMatrix> ProjectionMatrix { get { Recalculate(); return _projectionMatrix; } }
        void Recalculate()
        {
            if (!_dirty)
                return;
            _viewMatrix.Data = new ViewMatrix(CalculateView());
            _projectionMatrix.Data = new ProjectionMatrix(CalculateProjection());
            _dirty = false;
        }
        public Vector3 LookDirection { get { Recalculate(); return _lookDirection; } }
        public Vector2 Viewport
        {
            get => _viewport;
            set
            {
                if (_viewport == value) return;
                _viewport = value;
                _dirty = true;
            }
        }

        public float FieldOfView { get; private set; }
        public float NearDistance { get; private set; } = 10f;
        public float FarDistance { get; private set; } = 512.0f * 256.0f * 2.0f;
        public bool LegacyPitch { get; }
        public float AspectRatio => _viewport.X / _viewport.Y;
        public float Magnification { get; set; } // Ignored.
        public float Yaw { get => _yaw; set { _yaw = value; _dirty = true; } } // Radians

        public float Pitch // Radians
        {
            get => _pitch;
            set
            {
                _pitch = LegacyPitch
                    ? Math.Clamp(value, -0.48f, 0.48f)
                    : Math.Clamp(value, (float)-Math.PI / 2, (float)Math.PI / 2);

                _dirty = true;
            }
        }

        public Vector3 Position
        {
            get => _position;
            set { _position = value; _dirty = true; }
        }

        public PerspectiveCamera(bool legacyPitch = false)
        {
            On<CreateDeviceObjectsEvent>(_ => _dirty = true);
            /*
            OnAsync<ScreenCoordinateSelectEvent, Selection>(TransformSelect);
            On<EngineFlagEvent>(_ => UpdateBackend());
            On<CameraPositionEvent>(e => Position = new Vector3(e.X, e.Y, e.Z));
            On<CameraDirectionEvent>(e =>
            {
                Yaw = ApiUtil.DegToRad(e.Yaw);
                Pitch = ApiUtil.DegToRad(e.Pitch);
            });
            On<CameraPlanesEvent>(e =>
            {
                NearDistance = e.Near; FarDistance = e.Far; 
                _dirty = true;
            });

            On<SetFieldOfViewEvent>(e =>
            {
                if (e.Degrees == null)
                {
                    Info($"FOV {ApiUtil.RadToDeg(FieldOfView)}");
                }
                else
                {
                    FieldOfView = ApiUtil.DegToRad(e.Degrees.Value);
                    _dirty = true;
                }
            });
            */

            LegacyPitch = legacyPitch;
            FieldOfView = (float)(Math.PI * (legacyPitch ? 60 : 80) / 180);
        }

        protected override void Subscribed()
        {
            // Viewport = Resolve<IWindowManager>().Size;
            _dirty = true;
        }

        /*
        bool TransformSelect(ScreenCoordinateSelectEvent e, Action<Selection> continuation)
        {
            var normalisedScreenPosition = new Vector3(2 * e.Position.X / _viewport.X - 1.0f, -2 * e.Position.Y / _viewport.Y + 1.0f, 0.0f);
            var rayOrigin = UnprojectNormToWorld(normalisedScreenPosition + Vector3.UnitZ);
            var rayDirection = UnprojectNormToWorld(normalisedScreenPosition) - rayOrigin;
            RaiseAsync(new WorldCoordinateSelectEvent(rayOrigin, rayDirection), continuation);
            return true;
        }

        void UpdateBackend()
        {
            var settings = TryResolve<IEngineSettings>();
            var e = TryResolve<IEngine>();
            if (settings != null && e != null)
            {
                _useReverseDepth = (settings.Flags & EngineFlags.FlipDepthRange) == EngineFlags.FlipDepthRange;
                _depthZeroToOne = e.IsDepthRangeZeroToOne;
                _isClipSpaceYInverted = (settings.Flags & EngineFlags.FlipYSpace) == EngineFlags.FlipYSpace
                    ? !e.IsClipSpaceYInverted
                    : e.IsClipSpaceYInverted;
            }

            _dirty = true;
        }
        */

        Matrix4x4 CalculateProjection() =>
            LegacyPitch
                ? MatrixUtil.CreateLegacyPerspective(
                    _isClipSpaceYInverted,
                    _useReverseDepth,
                    _depthZeroToOne,
                    FieldOfView,
                    AspectRatio,
                    NearDistance,
                    FarDistance,
                    Pitch)
                : MatrixUtil.CreatePerspective(
                    _isClipSpaceYInverted,
                    _useReverseDepth,
                    _depthZeroToOne,
                    FieldOfView,
                    AspectRatio,
                    NearDistance,
                    FarDistance);

        Matrix4x4 CalculateView()
        {
            Quaternion lookRotation = Quaternion.CreateFromYawPitchRoll(Yaw, LegacyPitch ? 0f : Pitch, 0f);
            _lookDirection = Vector3.Transform(-Vector3.UnitZ, lookRotation);
            return Matrix4x4.CreateLookAt(_position, _position + _lookDirection, Vector3.UnitY);
        }

        public Vector3 ProjectWorldToNorm(Vector3 worldPosition)
        {
            var v = Vector4.Transform(new Vector4(worldPosition, 1.0f), ViewMatrix.Data.Matrix * ProjectionMatrix.Data.Matrix);
            return new Vector3(v.X / v.W, v.Y / v.W, v.Z / v.W);
        }

        public Vector3 UnprojectNormToWorld(Vector3 normPosition)
        {
            var v = Vector4.Transform(new Vector4(normPosition, 1.0f), (ViewMatrix.Data.Matrix * ProjectionMatrix.Data.Matrix).Inverse());
            return new Vector3(v.X / v.W, v.Y / v.W, v.Z / v.W);
        }
    }
}
