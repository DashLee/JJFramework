﻿using UnityEngine;
using JJFramework.Runtime.Extension;

namespace JJFramework.Runtime.UI
{
    [DisallowMultipleComponent]
    public abstract class BaseUI : ExMonoBehaviour
    {
        public int id
        {
            get;
            private set;
        }

        //[ComponentPath] protected Button btnClose;

        public bool isCalledCloseAction { get; private set; }
        public virtual void CloseAction()
        {
            isCalledCloseAction = true;
        }

        protected override void Awake()
        {
            base.Awake();

            id = this.GetHashCode(); // TODO(jjo): GenerateUID Function
            
            //btnClose.OnClickAsObservable().Subscribe(_ => CloseAction());
        }

        public virtual BaseUI Show()
        {
            gameObject.SetActive(true);

            return this;
        }

        public virtual BaseUI Hide()
        {
            gameObject.SetActive(false);

            return this;
        }
    }
}
