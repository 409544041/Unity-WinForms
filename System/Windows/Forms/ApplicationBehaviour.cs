﻿using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Forms;

namespace System.Windows.Forms
{
    public sealed class ApplicationBehaviour : MonoBehaviour
    {
        private static Texture2D _defaultSprite;

        public static Texture2D DefaultSprite
        {
            get
            {
                if (_defaultSprite != null) return _defaultSprite;

                _defaultSprite = new Texture2D(32, 32);
                for (int i = 0; i < _defaultSprite.height; i++)
                    for (int k = 0; k < _defaultSprite.width; k++)
                        _defaultSprite.SetPixel(k, i, Color.white);
                _defaultSprite.Apply();
                return _defaultSprite;
            }
        }
        public static AppResources Resources { get; private set; }

        public AppResources _Resources;
        public static bool ShowControlProperties { get; set; }

        private List<invokeAction> actions = new List<invokeAction>();
        private Application _controller;
        private float _lastWidth;
        private float _lastHeight;
        private bool _paused;

        private void Awake()
        {
            Resources = _Resources;

            _lastWidth = UnityEngine.Screen.width;
            _lastHeight = UnityEngine.Screen.height;

            _controller = new Application(this);
            _controller.UpdatePaintClipRect();
            Control.DefaultController = _controller;
        }
        private void Update()
        {
            if (_lastWidth != UnityEngine.Screen.width || _lastHeight != UnityEngine.Screen.height)
            {
                System.Drawing.Size deltaSize = new System.Drawing.Size(
                    (int)(_lastWidth - UnityEngine.Screen.width),
                    (int)(_lastHeight - UnityEngine.Screen.height));
                for (int i = 0; i < _controller.ModalForms.Count; i++)
                    _controller.ModalForms[i].UWF_AddjustSizeToScreen(deltaSize);
                for (int i = 0; i < _controller.Forms.Count; i++)
                    _controller.Forms[i].UWF_AddjustSizeToScreen(deltaSize);
                _controller.UpdatePaintClipRect();
            }
            _lastWidth = UnityEngine.Screen.width;
            _lastHeight = UnityEngine.Screen.height;

            _controller.Update();

            for (int i = 0; i < actions.Count; i++)
            {
                var a = actions[i];
                a.Seconds -= Time.deltaTime;
                if (a.Seconds > 0) continue;
                
                a.Action();
                actions.RemoveAt(i);
                i--;
            }
        }
        private void OnApplicationFocus(bool focusStatus)
        {
            _paused = !focusStatus;

            UnityEngine.Cursor.visible = Cursor.IsVisible;
        }
        private void OnGUI()
        {
            if (_paused == false)
            {
                _controller.ProccessMouse(Input.mousePosition.x, UnityEngine.Screen.height - Input.mousePosition.y);
                _controller.ProccessKeys();
            }

            _controller.Draw();
        }

        internal invokeAction Invoke(Action a, float seconds)
        {
            if (a == null) return null;

            var ia = new invokeAction()
            {
                Action = a,
                Seconds = seconds
            };

            actions.Add(ia);
            return ia;
        }

        internal class invokeAction
        {
            public Action Action { get; set; }
            public float Seconds { get; set; }
        }
    }
}