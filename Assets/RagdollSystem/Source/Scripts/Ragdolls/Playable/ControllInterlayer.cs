using UnityEngine;

namespace RagdollSystem
{
    public class ControllInterlayer : MonoBehaviour, IControllInterlayer
    {
        [SerializeField] private GameObject _characterParent;
        [SerializeField] private PlayerCharacterControll _playerControllPrefab;
        [SerializeField] private PlayableCharacterMark _playableCharacterMark;
        [SerializeField] private CharacterInteractionTrigger _characterInteractionTrigger;
        [Header("ControllField")][SerializeField] private BaseInputConfig _inputConfig;
        [SerializeField] private Transform _mainBody;
        [SerializeField] private PuppetRotator _puppetRotator;
        [SerializeField] private Character _character;

        private IInputListener _inputListener;
        private Player _player;
        private PlayerCharacterControll _playerCharacterControll;
        private ExitCharacterButton _exitCharacterButton;
        private GameComponets _gameComponets;

        public bool UnderControll { get; private set; }

        private void OnDestroy()
        {
            if (isActiveAndEnabled == false)
                return;

            _playableCharacterMark.OnClick -= OnMarkClick;
            _exitCharacterButton.OnClick -= OnExitClick;
        }

        public void Construct(GameComponets gameComponets)
        {
            _gameComponets = gameComponets;
            _exitCharacterButton = gameComponets.ExitCharacterButton;
            _inputListener = GetComponentInChildren<IInputListener>();

            if (_inputListener == null)
            {
                Debug.LogError($"Can`t find {typeof(IInputListener)}");
                DestroyImmediate(this);
            }

            _playableCharacterMark.OnClick += OnMarkClick;
            _exitCharacterButton.OnClick += OnExitClick;
        }

        private void OnMarkClick() => StartControll();

        private void OnExitClick()
        {
            if (UnderControll)
                StopControll();
        }

        public void StartControll()
        {
            _playableCharacterMark.gameObject.SetActive(false);
            _characterInteractionTrigger.gameObject.SetActive(true);
            _playerCharacterControll = Instantiate(_playerControllPrefab);

            _playerCharacterControll.transform.SetParent(_characterParent.transform);
            _playerCharacterControll.transform.SetLocalPositionAndRotation(Vector3.one, Quaternion.identity);

            _player = _playerCharacterControll.Player;
            _playerCharacterControll.Construct(_gameComponets);

            _player.Setup();
            _playerCharacterControll.Setup(_inputConfig, _inputListener);
            _playerCharacterControll.OnEnter();

            UnderControll = true;
            _inputListener.OnDie += OnListenerDie;
        }

        public void StopControll()
        {
            if (UnderControll == false) 
                return;

            _characterInteractionTrigger.gameObject.SetActive(false);
            _playerCharacterControll?.OnExit();

            _player = null;
            _playerCharacterControll = null;

            UnderControll = false;
            _inputListener.OnDie -= OnListenerDie;
            _playableCharacterMark.gameObject.SetActive(true);
        }

        private void OnListenerDie(BaseCharacter character)
        {
            _player.OnPlayerDie();
            StopControll();
        }
    }
}
