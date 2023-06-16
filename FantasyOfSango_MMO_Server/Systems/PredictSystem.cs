using FantasyOfSango_MMO_Server.Bases;
using SangoMMOCommons.Classs;
using SangoMMOCommons.Structs;

//Developer : SangonomiyaSakunovi

namespace FantasyOfSango_MMO_Server.Systems
{
    public class PredictSystem : BaseSystem
    {
        public static PredictSystem Instance = null;

        public override void InitSystem()
        {
            base.InitSystem();
            Instance = this;
        }

        public TransformOnline PredictNextTransform(string account, TransformOnline lastTrans, TransformOnline currentTrans)
        {
            Vector3Position predictVector3Position = Vector3Position.EdgePosition(lastTrans.Vector3Position, currentTrans.Vector3Position);
            QuaternionRotation predictQuaternionRotation = currentTrans.QuaternionRotation;
            TransformOnline predictTransformOnline = new TransformOnline
            {
                Account = account,
                Vector3Position = predictVector3Position,
                QuaternionRotation = predictQuaternionRotation
            };
            return predictTransformOnline;
        }
    }
}
