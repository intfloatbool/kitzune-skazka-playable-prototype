using System.Collections;
using BezierSolution;
using DG.Tweening;
using Prototype.GameUI;
using Prototype.Player;
using UnityEngine;

namespace Prototype.Boss
{
    public class TutorialBossController : MonoBehaviour
    {
        [SerializeField] private GameObject _closedEye;
        [SerializeField] private GameObject _bossGo;
        [SerializeField] private GameObject _openedEye;
        [SerializeField] private GameObject _hands;
        [SerializeField] private GameObject _soul;
        private IEnumerator Start()
        {
            
            var boss = FindObjectOfType<TutorialBoss>();
            var tutorialWindow = FindObjectOfType<TutorialWindow>();
            var playerController = FindObjectOfType<PlayerController>();

            var soulMover = boss.Soul.GetComponent<BezierWalkerWithSpeed>();
            _closedEye.SetActive(true);
            _bossGo.transform.localScale = Vector3.zero;
            _soul.SetActive(true);
            _soul.transform.localScale = Vector3.zero;
            yield return new WaitForEndOfFrame();
            
            soulMover.SetActive(false);
            boss.SetActive(false);
            boss.Animator.enabled = false;
            playerController.SetActive(false);
            
            yield return new WaitForSeconds(1f);
            
            _closedEye.SetActive(false);
            _openedEye.SetActive(true);
            _hands.SetActive(true);
            var handsPos = _hands.transform.position;
            handsPos.y = -1.5f;
            _hands.transform.DOMove(handsPos, 1.4f);
            
            yield return new WaitForEndOfFrame();
            
            yield return new WaitForSeconds(1f);
            _soul.transform.DOScale(Vector3.one * 0.17f, 1f);
            yield return new WaitForSeconds(1f);

            tutorialWindow.ShowTutorial();
            tutorialWindow.OnHide = () =>
            {
                _soul.SetActive(false);
                _openedEye.SetActive(false);
                _hands.gameObject.SetActive(false);
                _bossGo.transform.localScale = Vector3.one;
                boss.SetActive(true);
                boss.Animator.enabled = true;
                playerController.SetActive(true);
                soulMover.SetActive(true);
            };
        }
    }
}
