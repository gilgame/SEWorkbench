using System;
using System.Collections.Generic;

using VRage.Components;
using VRage.ModAPI;
using VRage.ObjectBuilders;
using VRage.Utils;
using VRageMath;

namespace Gilgame.SEWorkbench.ScriptAPI
{
    public class MyEntity : IMyEntity
    {
        public event Action<IMyEntity> OnClose;

        public event Action<IMyEntity> OnClosing;

        public event Action<IMyEntity> OnMarkForClose;

        public event Action<IMyEntity> OnPhysicsChanged;

        public MyEntityComponentContainer Components { get; set; }

        public MyPhysicsComponentBase Physics { get; set; }

        public MyPositionComponentBase PositionComp { get; set; }

        public MyRenderComponentBase Render { get; set; }

        public MyEntityComponentBase GameLogic { get; set; }

        public MyHierarchyComponentBase Hierarchy { get; set; }

        public MySyncComponentBase SyncObject { get; set; }

        public EntityFlags Flags { get; set; }

        public long EntityId { get; set; }

        public string Name { get; set; }

        public bool MarkedForClose { get; set; }

        public bool Closed { get; set; }

        public bool DebugAsyncLoading { get; set; }

        public bool Save { get; set; }

        public MyPersistentEntityFlags2 PersistentFlags { get; set; }

        public MyEntityUpdateEnum NeedsUpdate { get; set; }

        public IMyEntity Parent { get; set; }

        public Matrix LocalMatrix { get; set; }

        public bool NearFlag { get; set; }

        public bool CastShadows { get; set; }

        public bool FastCastShadowResolve { get; set; }

        public bool NeedsResolveCastShadow { get; set; }

        public float MaxGlassDistSq { get; set; }

        public bool NeedsDraw { get; set; }

        public bool NeedsDrawFromParent { get; set; }

        public bool Transparent { get; set; }

        public bool ShadowBoxLod { get; set; }

        public bool SkipIfTooSmall { get; set; }

        public bool Visible { get; set; }

        public bool InScene { get; set; }

        public bool InvalidateOnMove { get; set; }

        public BoundingBoxD WorldAABB { get; set; }

        public BoundingBoxD WorldAABBHr { get; set; }

        public MatrixD WorldMatrix { get; set; }

        public MatrixD WorldMatrixInvScaled { get; set; }

        public MatrixD WorldMatrixNormalizedInv { get; set; }

        public BoundingSphereD WorldVolume { get; set; }

        public BoundingSphereD WorldVolumeHr { get; set; }

        public bool IsVolumetric { get; set; }

        public BoundingBox LocalAABB { get; set; }

        public BoundingBox LocalAABBHr { get; set; }

        public BoundingSphere LocalVolume { get; set; }

        public Vector3 LocalVolumeOffset { get; set; }

        public Vector3 LocationForHudMarker { get; set; }

        public bool IsCCDForProjectiles { get; set; }

        public string DisplayName { get; set; }

        public string GetFriendlyName()
        {
            throw new NotImplementedException();
        }

        public void Close()
        {
            throw new NotImplementedException();
        }

        public void Delete()
        {
            throw new NotImplementedException();
        }

        public MyObjectBuilder_EntityBase GetObjectBuilder(bool copy = false)
        {
            throw new NotImplementedException();
        }

        public void BeforeSave()
        {
            throw new NotImplementedException();
        }

        public IMyEntity GetTopMostParent(Type type = null)
        {
            throw new NotImplementedException();
        }

        public void SetLocalMatrix(Matrix localMatrix, object source = null)
        {
            throw new NotImplementedException();
        }

        public void GetChildren(List<IMyEntity> children, Func<IMyEntity, bool> collect = null)
        {
            throw new NotImplementedException();
        }

        public Vector3 GetDiffuseColor()
        {
            throw new NotImplementedException();
        }

        public bool IsVisible()
        {
            throw new NotImplementedException();
        }

        public void DebugDraw()
        {
            throw new NotImplementedException();
        }

        public void DebugDrawInvalidTriangles()
        {
            throw new NotImplementedException();
        }

        public void EnableColorMaskForSubparts(bool enable)
        {
            throw new NotImplementedException();
        }

        public void SetColorMaskForSubparts(Vector3 colorMaskHsv)
        {
            throw new NotImplementedException();
        }

        public float GetDistanceBetweenCameraAndBoundingSphere()
        {
            throw new NotImplementedException();
        }

        public float GetDistanceBetweenCameraAndPosition()
        {
            throw new NotImplementedException();
        }

        public float GetLargestDistanceBetweenCameraAndBoundingSphere()
        {
            throw new NotImplementedException();
        }

        public float GetSmallestDistanceBetweenCameraAndBoundingSphere()
        {
            throw new NotImplementedException();
        }

        public void OnRemovedFromScene(object source)
        {
            throw new NotImplementedException();
        }

        public void OnAddedToScene(object source)
        {
            throw new NotImplementedException();
        }

        public MatrixD GetViewMatrix()
        {
            throw new NotImplementedException();
        }

        public MatrixD GetWorldMatrixNormalizedInv()
        {
            throw new NotImplementedException();
        }

        public void SetWorldMatrix(MatrixD worldMatrix, object source = null)
        {
            throw new NotImplementedException();
        }

        public Vector3D GetPosition()
        {
            throw new NotImplementedException();
        }

        public void SetPosition(Vector3D pos)
        {
            throw new NotImplementedException();
        }

        public Vector3? GetIntersectionWithLineAndBoundingSphere(ref LineD line, float boundingSphereRadiusMultiplier)
        {
            throw new NotImplementedException();
        }

        public bool GetIntersectionWithSphere(ref BoundingSphereD sphere)
        {
            throw new NotImplementedException();
        }

        public void GetTrianglesIntersectingSphere(ref BoundingSphereD sphere, Vector3? referenceNormalVector, float? maxAngle, List<MyTriangle_Vertex_Normals> retTriangles, int maxNeighbourTriangles)
        {
            throw new NotImplementedException();
        }

        public bool DoOverlapSphereTest(float sphereRadius, Vector3D spherePos)
        {
            throw new NotImplementedException();
        }

        public void AddToGamePruningStructure()
        {
            throw new NotImplementedException();
        }

        public void RemoveFromGamePruningStructure()
        {
            {
                throw new NotImplementedException();
            }
        }

        public void UpdateGamePruningStructure()
        {
            {
                throw new NotImplementedException();
            }
        }
    }
}
