using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Evergine.InstantNGP
{
    public static unsafe class InstantNGP
    {
        public static void LoadLibraryAccordingToGPUCapacity()
        {
            float compute_capacity = GetGPUComputeCapacity();

            if (compute_capacity == 7.5f)
            {
                NativeLibrary.Load(@".\runtimes\win-x64\rtx2\native\ngp_shared.dll");
            }
            else if (compute_capacity == 8.6f)
            {
                NativeLibrary.Load(@".\runtimes\win-x64\rtx3\native\ngp_shared.dll");
            }
            else if (compute_capacity == 8.9f)
            {
                NativeLibrary.Load(@".\runtimes\win-x64\rtx4\native\ngp_shared.dll");
            }
            else
            {
                throw new InvalidOperationException($"The GPU is not supported, please use an NVidia RTX model series 2/3/4.");
            }
        }

        [DllImport("ngp_shared", EntryPoint = "nerf_initialize", CharSet = CharSet.Ansi)]
        public static extern void initialize(string scene, string snapshot, bool use_dlss);

        [DllImport("ngp_shared", EntryPoint = "nerf_deinitialize")]
        public static extern void deinitialize();

        [DllImport("ngp_shared", EntryPoint = "nerf_create_textures")]
        public static extern void create_textures(int num_views, float* fov, int width, int height, float scaleFactor, uint* handles);

        public static uint[] create_textures(int num_views, float[] fov, int width, int height, float scaleFactor)
        {
            uint[] handles = new uint[num_views];
            fixed(float* fov_ptr = fov)
            fixed(uint* handles_ptr = handles)
            {                
                create_textures(num_views, fov_ptr, width, height, scaleFactor, handles_ptr);
            }

            return handles;
        }

        [DllImport("ngp_shared", EntryPoint = "nerf_update_textures")]
        public static extern void update_textures(float* camera_matrix);

        public static void update_textures(float[] camera_matrix)
        {
            fixed (float* cam_ptr = camera_matrix)
            {
                update_textures(cam_ptr);
            }
        }

        [DllImport("ngp_shared", EntryPoint = "nerf_set_fov")]
        public static extern void set_fov(float val);

        [DllImport("ngp_shared", EntryPoint = "nerf_update_aabb_crop")]
        public static extern void update_aabb_crop(float* aabb_min, float* aabb_max);

        public static void update_aabb_crop(float[] aabb_min, float[] aabb_max)
        {
            fixed (float* aabb_min_ptr = aabb_min)
            {
                fixed (float* aabb_max_ptr = aabb_max)
                {
                    update_aabb_crop(aabb_min_ptr, aabb_max_ptr);
                }
            }
        }

        [DllImport("ngp_shared", EntryPoint = "nerf_reset_camera")]
        public static extern void reset_nerf_camera();

        private static float GetGPUComputeCapacity()
        {
            Process process = new Process();
            process.StartInfo = new ProcessStartInfo()
            {
                UseShellExecute = false,
                CreateNoWindow = true,
                WindowStyle = ProcessWindowStyle.Hidden,
                FileName = "cmd.exe",
                Arguments = "/C nvidia-smi --query-gpu=compute_cap --format=csv",
                RedirectStandardError = true,
                RedirectStandardOutput = true
            };
            process.Start();
            var compute_capacity_str = process.StandardOutput.ReadToEnd();
            process.WaitForExit();

            return float.Parse(compute_capacity_str.Split(new[] { Environment.NewLine }, StringSplitOptions.None)[1]);
        }
    }
}
