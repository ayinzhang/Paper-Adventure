using System.Collections;
using System.Collections.Generic;

namespace UnityEngine.Rendering.Universal
{
    public class Grey : VolumeComponent, IPostProcessComponent
    {
        public bool IsActive() => true;
        public bool IsTileCompatible() => false;
    }

    public class GreyPass : ScriptableRenderPass
    {
        static readonly string k_RenderTag = "Grey Effects";
        static readonly int MainTexId = Shader.PropertyToID("_MainTex");
        static readonly int TempTexId = Shader.PropertyToID("_TempTex");
        Grey grey;
        Material mat;
        RenderTargetIdentifier target;

        public GreyPass(RenderPassEvent evt)
        {
            renderPassEvent = evt;
            mat = CoreUtils.CreateEngineMaterial(Shader.Find("Grey"));
        }

        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {
            if (!renderingData.cameraData.postProcessEnabled) return;

            var stack = VolumeManager.instance.stack;
            grey = stack.GetComponent<Grey>();
            if (!grey) { return; }

            var cmd = CommandBufferPool.Get(k_RenderTag);
            Render(cmd, ref renderingData);
            context.ExecuteCommandBuffer(cmd);
            CommandBufferPool.Release(cmd);
        }

        public void Setup(in RenderTargetIdentifier target)
        {
            this.target = target;
        }

        void Render(CommandBuffer cmd, ref RenderingData renderingData)
        {
            ref var cameraData = ref renderingData.cameraData;
            var source = target;
            int temp = TempTexId;

            var w = cameraData.camera.scaledPixelWidth;
            var h = cameraData.camera.scaledPixelHeight;

            cmd.SetGlobalTexture(MainTexId, source);
            cmd.GetTemporaryRT(temp, w, h);
            cmd.Blit(source, temp, mat);
            cmd.Blit(temp, source);
        }
    }

    public class GreyFeature : ScriptableRendererFeature
    {
        GreyPass greyPass;

        public override void Create()
        {
            greyPass = new GreyPass(RenderPassEvent.BeforeRenderingPostProcessing);
        }

        public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
        {
            greyPass.Setup(renderer.cameraColorTarget);
            renderer.EnqueuePass(greyPass);
        }
    }
}
