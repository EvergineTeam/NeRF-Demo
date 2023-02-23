using Evergine.Components.Graphics3D;
using Evergine.Framework;
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
            this.ngp = new InstantNGPBehavior() { NerfPath = "Data/fox"};            

            var camera = this.Managers.EntityManager.FindAllByTag("MainCamera").First();
            camera.AddComponent(this.ngp);

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


