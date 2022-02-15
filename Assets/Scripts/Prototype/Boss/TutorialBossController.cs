using System.Collections;
using BezierSolution;
using Prototype.GameUI;
using Prototype.Player;
using UnityEngine;

namespace Prototype.Boss
{
    public class TutorialBossController : MonoBehaviour
    {
        private IEnumerator Start()
        {
            
            var boss = FindObjectOfType<TutorialBoss>();
            var tutorialWindow = FindObjectOfType<TutorialWindow>();
            var playerController = FindObjectOfType<PlayerController>();

            var soulMover = boss.Soul.GetComponent<BezierWalkerWithSpeed>();

            yield return new WaitForEndOfFrame();
            
            soulMover.SetActive(false);
            boss.SetActive(false);
            boss.Animator.enabled = false;
            playerController.SetActive(false);
            
            yield return new WaitForSeconds(1f);
            
            tutorialWindow.ShowTutorial();
            tutorialWindow.OnHide = () =>
            {
                boss.SetActive(true);
                boss.Animator.enabled = true;
                playerController.SetActive(true);
                soulMover.SetActive(true);
            };
        }
    }
}
