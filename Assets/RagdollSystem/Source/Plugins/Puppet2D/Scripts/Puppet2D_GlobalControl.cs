using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace Puppet2D
{
	[ExecuteInEditMode]
	public class Puppet2D_GlobalControl : MonoBehaviour
	{
		[SerializeField] private bool _internalFlip = false;
        [SerializeField] private bool _fFD_Visiblity = true;
        [SerializeField] private bool _combineMeshes = false;
        [SerializeField] private bool _controlsEnabled = true;
        [SerializeField] private bool _lateUpdate = true;
        [SerializeField] private float _startRotationY = 0;
		private Puppet2D_SplineControl[] _SplineControlsArray;
		private Puppet2D_IKHandle[] _IkhandlesArray;
		private Puppet2D_ParentControl[] _ParentControlsArray;
		private Puppet2D_FFDLineDisplay[] _ffdControlsArray;

		public List<Puppet2D_IKHandle> _handHandles = new();
		public List<Puppet2D_SplineControl> _SplineControls = new List<Puppet2D_SplineControl>();
		public List<Puppet2D_IKHandle> _Ikhandles = new List<Puppet2D_IKHandle>();
		public List<Puppet2D_ParentControl> _ParentControls = new List<Puppet2D_ParentControl>();
		public List<Puppet2D_FFDLineDisplay> _ffdControls = new List<Puppet2D_FFDLineDisplay>();

		private List<SpriteRenderer> _controls = new List<SpriteRenderer>();
		private List<Puppet2D_Bone> _bones = new List<Puppet2D_Bone>();
		private List<SpriteRenderer> _fFDControls = new List<SpriteRenderer>();

        [field: SerializeField] public bool ControlsVisiblity { get; set; } = true;
        [field: SerializeField] public bool BonesVisiblity { get; set; } = true;
        [field: SerializeField] public bool AutoRefresh { get; set; } = true;
        [field: SerializeField] public bool Flip { get; set; } = false;
        [field: SerializeField] public int FlipCorrection { get; set; } = 1;

		private void OnEnable()
		{
			if (AutoRefresh)
			{
				_Ikhandles.Clear();
				_SplineControls.Clear();
				_ParentControls.Clear();
				_controls.Clear();
				_bones.Clear();
				_fFDControls.Clear();
				_ffdControls.Clear();
				TraverseHierarchy(transform);
				InitializeArrays();
			}
		}

		private void Awake()
		{
			_internalFlip = Flip;

			if (Application.isPlaying)
				if (_combineMeshes)
					CombineAllMeshes();
		}

        [ContextMenu(nameof(BlockHandHandles))]
        public void BlockHandHandles() => _IkhandlesArray = _Ikhandles.Except(_handHandles).ToArray();
        [ContextMenu(nameof(UnlockHandHandles))]
        public void UnlockHandHandles() => _IkhandlesArray = _Ikhandles.ToArray();

        public void Refresh()
        {
            _Ikhandles.Clear();
            _SplineControls.Clear();
            _ParentControls.Clear();
            _controls.Clear();
            _bones.Clear();
            _fFDControls.Clear();
            _ffdControls.Clear();
            TraverseHierarchy(transform);
            InitializeArrays();
        }

        public void InitializeArrays()
		{
			_SplineControlsArray = _SplineControls.ToArray();
			_IkhandlesArray = _Ikhandles.ToArray();
			_ParentControlsArray = _ParentControls.ToArray();
			_ffdControlsArray = _ffdControls.ToArray();
		}

		public void Init()
		{
			_Ikhandles.Clear();
			_SplineControls.Clear();
			_ParentControls.Clear();
			_controls.Clear();
			_bones.Clear();
			_fFDControls.Clear();
			_ffdControls.Clear();
			TraverseHierarchy(transform);
			InitializeArrays();
		}

        private void OnValidate() => UpdateVisibility();

        public void UpdateVisibility()
		{
			if (AutoRefresh)
			{
				_Ikhandles.Clear();
				_SplineControls.Clear();
				_ParentControls.Clear();
				_controls.Clear();
				_bones.Clear();
				_fFDControls.Clear();
				_ffdControls.Clear();
				TraverseHierarchy(transform);
				InitializeArrays();
			}

			foreach (SpriteRenderer ctrl in _controls)
				if (ctrl && ctrl.enabled != ControlsVisiblity)
					ctrl.enabled = ControlsVisiblity;

			foreach (Puppet2D_Bone bone in _bones)
				if (bone && bone.enabled != BonesVisiblity)
					bone.enabled = BonesVisiblity;

			foreach (SpriteRenderer ffdCtrl in _fFDControls)
				if (ffdCtrl && ffdCtrl.transform.parent && ffdCtrl.transform.parent.gameObject && ffdCtrl.transform.parent.gameObject.activeSelf != _fFD_Visiblity)
					ffdCtrl.transform.parent.gameObject.SetActive(_fFD_Visiblity);
		}

		private void Update()
		{
			if (!_lateUpdate)
			{
#if UNITY_EDITOR
				if (AutoRefresh)
				{
					for (int i = _ParentControls.Count - 1; i >= 0; i--)
					{
						if (_ParentControls[i] == null)
							_ParentControls.RemoveAt(i);
					}
					for (int i = _Ikhandles.Count - 1; i >= 0; i--)
					{
						if (_Ikhandles[i] == null)
							_Ikhandles.RemoveAt(i);
					}
					for (int i = _SplineControls.Count - 1; i >= 0; i--)
					{
						if (_SplineControls[i] == null)
							_SplineControls.RemoveAt(i);
					}
					for (int i = _ffdControls.Count - 1; i >= 0; i--)
					{
						if (_ffdControls[i] == null)
							_ffdControls.RemoveAt(i);
					}
				}
#endif
				if (_controlsEnabled)
					Run();

				if (_internalFlip != Flip)
				{
					if (Flip)
					{
						transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, -transform.localScale.z);
						transform.localEulerAngles = new Vector3(transform.rotation.eulerAngles.x, _startRotationY + 180, transform.rotation.eulerAngles.z);
					}
					else
					{
						transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), Mathf.Abs(transform.localScale.y), Mathf.Abs(transform.localScale.z));
						transform.localEulerAngles = new Vector3(transform.rotation.eulerAngles.x, _startRotationY, transform.rotation.eulerAngles.z);
					}
					_internalFlip = Flip;
					Run();
				}
			}
		}

		private void LateUpdate()
		{
			if (_lateUpdate)
			{
#if UNITY_EDITOR
				if (AutoRefresh)
				{
					for (int i = _ParentControls.Count - 1; i >= 0; i--)
					{
						if (_ParentControls[i] == null)
							_ParentControls.RemoveAt(i);
					}
					for (int i = _Ikhandles.Count - 1; i >= 0; i--)
					{
						if (_Ikhandles[i] == null)
							_Ikhandles.RemoveAt(i);
					}
					for (int i = _SplineControls.Count - 1; i >= 0; i--)
					{
						if (_SplineControls[i] == null)
							_SplineControls.RemoveAt(i);
					}
					for (int i = _ffdControls.Count - 1; i >= 0; i--)
					{
						if (_ffdControls[i] == null)
							_ffdControls.RemoveAt(i);
					}
				}
#endif
				if (_controlsEnabled)
					Run();

				if (_internalFlip != Flip)
				{
					if (Flip)
					{

						transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, -transform.localScale.z);
						transform.localEulerAngles = new Vector3(transform.rotation.eulerAngles.x, _startRotationY + 180, transform.rotation.eulerAngles.z);
					}
					else
					{
						transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), Mathf.Abs(transform.localScale.y), Mathf.Abs(transform.localScale.z));
						transform.localEulerAngles = new Vector3(transform.rotation.eulerAngles.x, _startRotationY, transform.rotation.eulerAngles.z);
					}

					_internalFlip = Flip;
					Run();
				}
			}
		}

		public void Run()
		{
			for (int i = 0; i < _SplineControlsArray.Length; i++)
				if (_SplineControlsArray[i])
					_SplineControlsArray[i].Run();

			for (int i = 0; i < _ParentControlsArray.Length; i++)
				if (_ParentControlsArray[i])
					_ParentControlsArray[i].ParentControlRun();

			FaceCamera();

			for (int i = 0; i < _IkhandlesArray.Length; i++)
				if (_IkhandlesArray[i])
					_IkhandlesArray[i].CalculateIK();

			for (int i = 0; i < _ffdControlsArray.Length; i++)
				if (_ffdControlsArray[i])
					_ffdControlsArray[i].Run();
		}

		public void TraverseHierarchy(Transform root)
		{
			foreach (Transform child in root)
			{
				GameObject Go = child.gameObject;
				SpriteRenderer spriteRenderer = Go.transform.GetComponent<SpriteRenderer>();

				if (spriteRenderer && spriteRenderer.sprite)
				{
					if (spriteRenderer.sprite.name.Contains("Control"))
						_controls.Add(spriteRenderer);
					else if (spriteRenderer.sprite.name.Contains("ffd"))
						_fFDControls.Add(spriteRenderer);					
				}

				Puppet2D_Bone boneRender = Go.transform.GetComponent<Puppet2D_Bone>();
				if (boneRender)
					_bones.Add(boneRender);

				Puppet2D_ParentControl newParentCtrl = Go.transform.GetComponent<Puppet2D_ParentControl>();

				if (newParentCtrl)
					_ParentControls.Add(newParentCtrl);

				Puppet2D_IKHandle newIKCtrl = Go.transform.GetComponent<Puppet2D_IKHandle>();
				if (newIKCtrl)
					_Ikhandles.Add(newIKCtrl);

				Puppet2D_FFDLineDisplay ffdCtrl = Go.transform.GetComponent<Puppet2D_FFDLineDisplay>();
				if (ffdCtrl)
					_ffdControls.Add(ffdCtrl);

				Puppet2D_SplineControl splineCtrl = Go.transform.GetComponent<Puppet2D_SplineControl>();
				if (splineCtrl)
					_SplineControls.Add(splineCtrl);

				TraverseHierarchy(child);
			}
		}

		private void CombineAllMeshes()
		{
			Vector3 originalScale = transform.localScale;
			Quaternion originalRot = transform.rotation;
			Vector3 originalPos = transform.position;
			transform.localScale = Vector3.one;
			transform.SetPositionAndRotation(Vector3.zero, Quaternion.identity);

			SkinnedMeshRenderer[] smRenderers = GetComponentsInChildren<SkinnedMeshRenderer>();
			List<Transform> bones = new List<Transform>();
			List<BoneWeight> boneWeights = new List<BoneWeight>();
			List<CombineInstance> combineInstances = new List<CombineInstance>();
			List<Texture2D> textures = new List<Texture2D>();
			Material currentMaterial = null;

			int numSubs = 0;
			var smRenderersDict = new Dictionary<SkinnedMeshRenderer, float>(smRenderers.Length);
			bool updateWhenOffscreen = false;

			foreach (SkinnedMeshRenderer smr in smRenderers)
			{
				smRenderersDict.Add(smr, smr.transform.position.z);
				updateWhenOffscreen = smr.updateWhenOffscreen;
			}


            IOrderedEnumerable<KeyValuePair<SkinnedMeshRenderer, float>> items = from pair in smRenderersDict
						orderby pair.Key.sortingOrder ascending
						select pair;

            items = from pair in items
                    orderby pair.Value descending
					select pair;

			foreach (KeyValuePair<SkinnedMeshRenderer, float> pair in items)
				numSubs += pair.Key.sharedMesh.subMeshCount;

			int[] meshIndex = new int[numSubs];
			int boneOffset = 0;
			int s = 0;

			foreach (KeyValuePair<SkinnedMeshRenderer, float> pair in items)
			{
				SkinnedMeshRenderer smr = pair.Key;

				if (currentMaterial == null)
					currentMaterial = smr.sharedMaterial;
				else if (currentMaterial.mainTexture && smr.sharedMaterial.mainTexture && currentMaterial.mainTexture != smr.sharedMaterial.mainTexture)
					continue;

				bool ffdMesh = false;
				foreach (Transform boneToCheck in smr.bones)
				{
					Puppet2D_FFDLineDisplay ffdLine = boneToCheck.GetComponent<Puppet2D_FFDLineDisplay>();
					if (ffdLine && ffdLine.outputSkinnedMesh != smr)
						ffdMesh = true;
				}

				if (ffdMesh)
					continue;

				BoneWeight[] meshBoneweight = smr.sharedMesh.boneWeights;

				foreach (BoneWeight bw in meshBoneweight)
				{
					BoneWeight bWeight = bw;

					bWeight.boneIndex0 += boneOffset;
					bWeight.boneIndex1 += boneOffset;
					bWeight.boneIndex2 += boneOffset;
					bWeight.boneIndex3 += boneOffset;
					boneWeights.Add(bWeight);
				}

				boneOffset += smr.bones.Length;
				Transform[] meshBones = smr.bones;

				foreach (Transform bone in meshBones)
					bones.Add(bone);

				if (smr.material.mainTexture != null)
					textures.Add(smr.GetComponent<Renderer>().material.mainTexture as Texture2D);

				CombineInstance ci = new CombineInstance();
				ci.mesh = smr.sharedMesh;
				meshIndex[s] = ci.mesh.vertexCount;
				ci.transform = smr.transform.localToWorldMatrix;
				combineInstances.Add(ci);

				Destroy(smr.gameObject);
				s++;
			}

			List<Matrix4x4> bindposes = new List<Matrix4x4>();

			for (int b = 0; b < bones.Count; b++)
			{
				if (bones[b].GetComponent<Puppet2D_FFDLineDisplay>())
				{
					Vector3 boneparentPos = bones[b].transform.parent.parent.position;
					Quaternion boneparentRot = bones[b].transform.parent.parent.rotation;
					bones[b].transform.parent.parent.SetPositionAndRotation(Vector3.zero, Quaternion.identity);
					bindposes.Add(bones[b].worldToLocalMatrix * transform.worldToLocalMatrix);
					bones[b].transform.parent.parent.SetPositionAndRotation(boneparentPos, boneparentRot);
				}
				else
					bindposes.Add(bones[b].worldToLocalMatrix * transform.worldToLocalMatrix);
			}

			SkinnedMeshRenderer r = gameObject.AddComponent<SkinnedMeshRenderer>();

			r.updateWhenOffscreen = updateWhenOffscreen;
			r.sharedMesh = new Mesh();
			r.sharedMesh.CombineMeshes(combineInstances.ToArray(), true, true);

			Material combinedMat;
			if (currentMaterial != null)
				combinedMat = currentMaterial;
			else
				combinedMat = new Material(Shader.Find("Unlit/Transparent"));

			combinedMat.mainTexture = textures[0];
			r.sharedMesh.uv = r.sharedMesh.uv;
			r.sharedMaterial = combinedMat;

			r.bones = bones.ToArray();
			r.sharedMesh.boneWeights = boneWeights.ToArray();
			r.sharedMesh.bindposes = bindposes.ToArray();
			r.sharedMesh.RecalculateBounds();

			transform.localScale = originalScale;
			transform.SetPositionAndRotation(originalPos, originalRot);
		}

		private void FaceCamera()
		{
			for (int i = 0; i < _IkhandlesArray.Length; ++i)
				_IkhandlesArray[i].AimDirection = transform.forward.normalized * FlipCorrection;
		}
	}
}
