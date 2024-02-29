using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace GameRecorder
{
    public class RecorderHUDHandler : MonoBehaviour
    {
        [SerializeField] private Button _stopRecordButton;
        [SerializeField] private Button _startPlayModeButton;
        [SerializeField] private Button _stopPlayModeButton;
        [SerializeField] private Slider _manualControll;
        [SerializeField] private CustomButton _manualControllButton;

        private IGameRecorder _gameRecorder;
        private Coroutine _coroutine;
        private bool _hasClicked= false;

        private void OnDestroy()
        {
            _stopRecordButton.onClick.RemoveAllListeners();
            _manualControllButton.OnClickUp -= OnManualHandleClickedUp;
        }

        private void Update()
        {
            if(Input.GetMouseButtonUp(0) && _hasClicked && _manualControll.isActiveAndEnabled)
                OnManualHandleClickedUp();
        }

        public void Construct(IGameRecorder gameRecorder)
        {
            _gameRecorder = gameRecorder;

            _stopRecordButton.onClick.AddListener(StopRecord);
            _stopPlayModeButton.onClick.AddListener(StopPlayMode);
            _startPlayModeButton.onClick.AddListener(StartPlayMode);

            _manualControllButton.OnClickDown += OnManualHandleClickedDown;

            _startPlayModeButton.gameObject.SetActive(false);
            _stopPlayModeButton.gameObject.SetActive(false);
            _manualControll.gameObject.SetActive(false);
        }

        private void StopRecord()
        {
            _gameRecorder.StopRecoding();
            _stopRecordButton.gameObject.SetActive(false);
            _startPlayModeButton.gameObject.SetActive(true);
        }

        private void StartPlayMode()
        {
            _gameRecorder.StartPlayMode(() => 
            {
                _stopRecordButton.gameObject.SetActive(true);
                _stopPlayModeButton.gameObject.SetActive(false);
                OnPlayModeStoped();
            });

            OnPlayModeStarted();
            _startPlayModeButton.gameObject.SetActive(false);
            _stopPlayModeButton.gameObject.SetActive(true);
        }

        private void StopPlayMode()
        {
            OnPlayModeStoped();
            _gameRecorder.StopPlayMode(manualStop: true);

            _stopRecordButton.gameObject.SetActive(true);
            _startPlayModeButton.gameObject.SetActive(false);
            _stopPlayModeButton.gameObject.SetActive(false);
        }

        private void OnPlayModeStarted()
        {
            _manualControll.gameObject.SetActive(true);

            _manualControll.minValue = 0;
            _manualControll.maxValue = _gameRecorder.FramesData.Count;

            StopViewProgress();
            _coroutine = StartCoroutine(Playing());

            IEnumerator Playing()
            {
                while (_manualControll.value < _manualControll.maxValue)
                {
                    yield return null;
                    _manualControll.value = _gameRecorder.CurrentFrameIndex;
                }
            }
        }

        private void OnManualHandleClickedDown()
        {
            StopViewProgress();
            _hasClicked = true;
            _manualControllButton.gameObject.SetActive(false);
            _manualControll.onValueChanged.AddListener(SetTargetIndex);
            SetTargetIndex((int)_manualControll.value);
        }

        private void SetTargetIndex(float value) => _gameRecorder.ManualPlay((int)value);

        private void OnManualHandleClickedUp()
        {
            _hasClicked = false;
            _manualControllButton.gameObject.SetActive(true);
            _manualControll.onValueChanged.RemoveListener(SetTargetIndex);
            OnPlayModeStarted();
            _gameRecorder.ManualCancel();
        }

        private void OnPlayModeStoped()
        {
            StopViewProgress();
            _manualControll.value = 0;
            _manualControll.minValue = 0;
            _manualControll.maxValue = 1;
            _manualControll.gameObject.SetActive(false);
        }

        private void StopViewProgress()
        {
            if (_coroutine != null)
            {
                StopCoroutine(_coroutine);
                _coroutine = null;
            }
        }
    }
}
