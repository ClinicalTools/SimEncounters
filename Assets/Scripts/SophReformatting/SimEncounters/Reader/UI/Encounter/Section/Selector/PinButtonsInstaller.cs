﻿using UnityEngine;
using Zenject;

namespace ClinicalTools.SimEncounters
{
    public class PinButtonsInstaller : MonoInstaller
    {
        public virtual RectTransform PoolParent { get => poolParent; set => poolParent = value; }
        [SerializeField] private RectTransform poolParent;

        public virtual BaseUserPinGroupDrawer PinButtonsPrefab { get => pinButtonsPrefab; set => pinButtonsPrefab = value; }
        [SerializeField] private BaseUserPinGroupDrawer pinButtonsPrefab;
        public virtual BaseUserDialoguePinDrawer DialoguePinButtonPrefab { get => dialoguePinButtonPrefab; set => dialoguePinButtonPrefab = value; }
        [SerializeField] private BaseUserDialoguePinDrawer dialoguePinButtonPrefab;
        public virtual BaseUserQuizPinDrawer QuizPinButtonPrefab { get => quizPinButtonPrefab; set => quizPinButtonPrefab = value; }
        [SerializeField] private BaseUserQuizPinDrawer quizPinButtonPrefab;

        public override void InstallBindings()
        {
            Container.Bind<IReaderPanelDisplay>().To<ReaderPanelDisplay>().AsTransient();
            Container.BindMemoryPool<BaseUserPinGroupDrawer, BaseUserPinGroupDrawer.Pool>()
                .FromComponentInNewPrefab(PinButtonsPrefab)
                .UnderTransform(PoolParent);
            Container.BindMemoryPool<BaseUserDialoguePinDrawer, BaseUserDialoguePinDrawer.Pool>()
                .FromComponentInNewPrefab(DialoguePinButtonPrefab)
                .UnderTransform(PoolParent);
            Container.BindMemoryPool<BaseUserQuizPinDrawer, BaseUserQuizPinDrawer.Pool>()
                .FromComponentInNewPrefab(QuizPinButtonPrefab)
                .UnderTransform(PoolParent);
        }
    }
}