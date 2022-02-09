using System.Collections;
using DG.Tweening;
using Prototype.Boss;
using Prototype.Player;
using UnityEngine;

namespace Prototype.GameStates.BossTree
{
    public class TentaclesStageBGameplayState : GameplayStateBase
    {
        private readonly TentaclesController _tentaclesController;
        private readonly Boss.BossTree _bossTree;
        private readonly PlayerController _playerController;
        private readonly TreeThroat _treeThroat;

        public TentaclesStageBGameplayState(TentaclesController tentaclesController)
        {
            this._tentaclesController = tentaclesController;
            
            _bossTree = GameManager.Instance.StaticFindObjectOfType<Boss.BossTree>();
            _playerController = GameManager.Instance.StaticFindObjectOfType<PlayerController>();
            _treeThroat = GameManager.Instance.StaticFindObjectOfType<TreeThroat>();
        }

        private bool _isDone = false;
        
        public override void OnStateStarted()
        {
            GameManager.Instance.StartCustomCoroutine(AnimationCoroutine(), OnDone);  
        }

        private IEnumerator AnimationCoroutine()
        {
            _bossTree.SetActive(false);
            _playerController.SetActive(false);
            _bossTree.ThroatCollider.SetActive(false);
            _treeThroat.SetActive(false);
            _bossTree.transform.DOMove(_bossTree.StartPosition, 2f);

            var playerSafePosition = GameObject.Find("PlayerSafePosition");
            
            _playerController.MainAnimator.gameObject.SetActive(false);
            _playerController.MoveAnimator.gameObject.SetActive(true);

            _playerController.transform.DOMove(playerSafePosition.transform.position, 2f).onComplete = () =>
            {
                _playerController.MainAnimator.gameObject.SetActive(true);
                _playerController.MoveAnimator.gameObject.SetActive(false);
            };

            var spriteRenderer = _treeThroat.GetComponent<SpriteRenderer>();
            if (spriteRenderer)
            {
                spriteRenderer.sprite = _treeThroat.Sprites[3];
            }

            foreach (var kvp in _tentaclesController.BehaviorChangedTentaclesList)
            {
                var anchor = kvp.Item1;
                var tentacle = kvp.Item2;
                
                tentacle.SetActive(false);
            }

            var shakeStrength = Vector3.right * 1f;
            _bossTree.transform.GetChild(0).DOShakePosition(3f, shakeStrength);

            yield return new WaitForSeconds(1f);
            
            foreach (var kvp in _tentaclesController.BehaviorChangedTentaclesList)
            {
                var anchor = kvp.Item1;
                var tentacle = kvp.Item2;
                
                float offset = 5f;
                var targetAngle = 0f;
                var targetWordPosition = anchor.position + (anchor.right * offset);
                var targetAngles = anchor.GetChild(0).eulerAngles;

                var hidePosition = tentacle.transform.position + (tentacle.transform.up * 10);
                tentacle.transform.DOMove(hidePosition, 1f);
            }
            
            yield return new WaitForSeconds(1f);
            
            foreach (var kvp in _tentaclesController.BehaviorChangedTentaclesList)
            {
                var anchor = kvp.Item1;
                var tentacle = kvp.Item2;
                
                float offset = 2f;
                var targetAngle = 0f;
                var targetWordPosition = anchor.position + (anchor.right * offset);
                var targetAngles = anchor.GetChild(0).eulerAngles;

                tentacle.transform.position = anchor.position;
                tentacle.transform.eulerAngles = targetAngles;
                
                tentacle.transform.DOMove(targetWordPosition, 1f);
            }
            
            yield return new WaitForSeconds(1f);
            
            // Resume
            _bossTree.SetActive(true);
            _playerController.SetActive(true);
            _bossTree.ThroatCollider.SetActive(true);
            _treeThroat.SetActive(true);
            
            foreach (var kvp in _tentaclesController.BehaviorChangedTentaclesList)
            {
                var anchor = kvp.Item1;
                var tentacle = kvp.Item2;
                
                anchor.gameObject.SetActive(true);
                tentacle.transform.parent = anchor;
                
                float offset = 5f;
                tentacle.transform.localPosition = Vector3.right * 2;
                tentacle.transform.DOLocalMove(Vector3.right * offset, 1f);
                tentacle.transform.localRotation = Quaternion.Euler(0,0,90);
                tentacle.ResetBodyMoveData();
                
                tentacle.SetActive(true);
            }
            
        }

        private void OnDone()
        {
            _isDone = true;
        }

        public override void Update()
        {
            if(_isDone)
                return;

            if (!_isDone)
            {
                _bossTree.SetActive(false);
            }
        }
        
        
        public override void OnStateStopped()
        {
            
        }
    }
}