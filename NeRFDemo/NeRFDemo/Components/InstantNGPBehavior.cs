using Evergine.Common.Attributes;
using Evergine.Common.Graphics;
using Evergine.Framework;
using Evergine.Framework.Graphics;
using Evergine.Framework.Services;
using Evergine.InstantNGP;
using Evergine.Mathematics;
using Evergine.OpenGL;
using System;
using System.IO;

namespace NeRFDemo.Components
{
    public class InstantNGPBehavior : Behavior
    {
        [BindService]
        private GraphicsPresenter presenter = null;

        /// <summary>
        /// The texture that is used to display the contents of the NGP file.
        /// </summary>
        [DontRenderProperty]
        public Texture Texture;

        /// <summary>
        /// property is a string that stores the path of the NGP file that is being displayed.
        /// </summary>
        [IgnoreEvergine]
        public string NerfPath { get; set; }

        /// <inheritdoc/>
        protected override bool OnAttached()
        {
            if (!base.OnAttached())
            {
                return false;
            }                  

            InstantNGP.LoadLibraryAccordingToGPUCapacity();

            try
            {
                InstantNGP.initialize(
                    NerfPath,
                    Path.Combine(NerfPath, "base.msgpack"),
                    true);
            }
            catch (System.Runtime.InteropServices.SEHException e)
            {
                var path = Path.GetFullPath(NerfPath);
                throw new FileNotFoundException($"Could not find nerf scene at '{path}', or its checkpoint 'base.msgpack'", e);
            }

            return true;
        }

        /// <inheritdoc/>
        protected override void OnActivated()
        {
            base.OnActivated();

            var graphicContext = Application.Current.Container.Resolve<GraphicsContext>();
            int width = (int)presenter.FocusedDisplay.Width;
            int height = (int)presenter.FocusedDisplay.Height;

            var textureId = InstantNGP.create_textures(1, new float[0], width, height, 0.5f /*0.0625f*/)[0];

            TextureDescription description = new TextureDescription()
            {
                Type = TextureType.Texture2D,
                Width = (uint)width,
                Height = (uint)height,
                Format = PixelFormat.R32G32B32A32_Float,
                Depth = 1,
                ArraySize = 1,
                Faces = 1,
                MipLevels = 1,
                Flags = TextureFlags.ShaderResource,
                CpuAccess = ResourceCpuAccess.None,
                SampleCount = TextureSampleCount.None,
                Usage = ResourceUsage.Default,
            };

            this.Texture = GLTexture.FromNativeOpenGLTexture(graphicContext as GLGraphicsContext, textureId, ref description);
        }

        /// <inheritdoc/>
        protected override void Update(TimeSpan gameTime)
        {
            var camera = this.Managers.RenderManager.ActiveCamera3D;
            Vector3 left_positionPos = camera.Transform.Position;
            Vector3 left_forwardPos = camera.Transform.WorldTransform.Forward;
            Vector3 left_upPos = camera.Transform.WorldTransform.Up;
            Vector3 left_rightPos = camera.Transform.WorldTransform.Right;

            float[] camera_matrix = new float[3 * 4]
            {
                left_rightPos.X,    left_rightPos.Y,    left_rightPos.Z,
                left_upPos.X,       left_upPos.Y,       left_upPos.Z,
                left_forwardPos.X,  left_forwardPos.Y,  left_forwardPos.Z,
                left_positionPos.X, left_positionPos.Y, left_positionPos.Z
            };

            InstantNGP.update_textures(camera_matrix);
        }

        /// <inheritdoc/>
        protected override void OnDetach()
        {
            InstantNGP.deinitialize();

            base.OnDetach();
        }
    }
}