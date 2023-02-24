using Evergine.Common.Graphics;
using Evergine.Components.Graphics3D;
using Evergine.Framework;
using Evergine.Framework.Graphics;
using Evergine.Mathematics;
using NeRFDemo.Components;
using NeRFDemo.Effects;
using System.Linq;

namespace NeRFDemo
{
    public class MyScene : Scene
    {
        private FullQuad material;
        private InstantNGPBehavior ngp;

        public override void RegisterManagers()
        {
            base.RegisterManagers();
            
            this.Managers.AddManager(new global::Evergine.Bullet.BulletPhysicManager3D());            
        }

        protected override void CreateScene()
        {
            // Cameras           
            var cameraEntity = new Entity("Camera3D") 
                                .AddComponent(new Transform3D()
                                {
                                    LocalPosition = new Vector3(0, 0, 4),
                                })                
                                .AddComponent(new Camera3D()
                                {                    
                                    HDREnabled = true, 
                                    BackgroundColor = Color.Black, 
                                })
                                .AddComponent(new InstantNGPBehavior() { NerfPath = "Data/water_pump" })
                                ;

            this.ngp = cameraEntity.FindComponent<InstantNGPBehavior>();
            cameraEntity.FindComponent<Transform3D>().LookAt(Vector3.Zero);
            
            var parentCameraEntity = new Entity() 
                                .AddComponent(new Transform3D()) 
                                .AddComponent(new CameraBehavior(
                                                        CameraBehavior.Modes.Orbit | 
                                                        CameraBehavior.Modes.Zoom |
                                                        CameraBehavior.Modes.Pan)
                                             );

            var childCameraEntity = new Entity() 
                                .AddComponent(new Transform3D()); 

            parentCameraEntity.AddChild(childCameraEntity); 
            childCameraEntity.AddChild(cameraEntity);
            
            this.Managers.EntityManager.Add(parentCameraEntity);             

            Entity plane = this.Managers.EntityManager.FindAllByTag("Screen").First();
            var materialComponent = plane.FindComponent<MaterialComponent>();
            this.material = new FullQuad(materialComponent.Material);
        }

        protected override void Start()
        {
            base.Start();
            material.Input = ngp.Texture;
        }
    }
}


