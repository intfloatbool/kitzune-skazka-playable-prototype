using System.Collections;
using DG.Tweening;
using Prototype.Boss;
using Prototype.Managers;
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
        
        private bool _isDone = false;
        private readonly TreeBossFightGameController _bossFightController;

        public TentaclesStageBGameplayState(TreeBossFightGameController bossFightGameController,TentaclesController tentaclesController)
        {
            this._tentaclesController = tentaclesController;

            _bossFightController = bossFightGameController;
            _bossTree = GameManager.Instance.StaticFindObjectOfType<Boss.BossTree>();
            _playerController = GameManager.Instance.StaticFindObjectOfType<PlayerController>();
            _treeThroat = GameManager.Instance.StaticFindObjectOfType<TreeThroat>();
        }
        

        public override void OnStateStarted()
        {
            GameManager.Instance.StartCustomCoroutine(AnimationCoroutine(), OnDone);  
        }

        private IEnumerator AnimationCoroutine()
        {
            SoundManager.PlayMusic("music_tree_boss_stage_2_loop", _bossFightController.gameObject, true);
            _bossTree.SetActive(false);
            _playerController.SetActive(false);
            _bossTree.ThroatCollider.SetActive(false);
            _treeThroat.SetActive(false);
            _bossTree.transform.DOMove(_bossTree.StartPosition, 2f);

            var foxPlayer = _playerController.GetComponent<FoxPlayer>();
            foxPlayer.MakeImmortal();
            
            var treeKillCollider = _bossTree.GetComponentInChildren<PlayerKillTrigger>();
            treeKillCollider.SetActive(false);
            var playerSafePosition = GameObject.Find("PlayerSafePosition");
            
            _playerController.MainAnimator.gameObject.SetActive(false);
            _playerController.MoveAnimator.gameObject.SetActive(true);
            
            SoundManager.PlaySound("tree_scream");

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
            var treeBody = _bossTree.transform.GetChild(0);

            float treeShakeDuration = 0f;
            int treeShakeVibratio = 20;
            int treeShakeRandomness = 45;
            bool treeShakeSnapping = true;
            bool treeShakeFadeOut = false;
            

            float.TryParse(_parameters["treeShakeDuration"], out treeShakeDuration);
            int.TryParse(_parameters["treeShakeStrength"], out treeShakeVibratio);
            int.TryParse(_parameters["treeShakeVibratio"], out treeShakeRandomness);
            bool.TryParse(_parameters["treeShakeRandomness"], out treeShakeSnapping);
            bool.TryParse(_parameters["treeShakeFadeOut"], out treeShakeFadeOut);
            
            treeBody.DOShakePosition(1f, shakeStrength, 20, 45, true, false);

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
            foxPlayer.MakeMortal();
            _bossTree.SetActive(true);
            _playerController.SetActive(true);
            _bossTree.ThroatCollider.SetActive(true);
            _treeThroat.SetActive(true);
            treeKillCollider.SetActive(true);
            
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