
using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDKBase;
using VRC.Udon;

namespace iruca.ExtensionMenu
{
    public enum VRCMenu
    {
        NoMenu,
        QuickMenu,
        MainMenu
    }

    public enum MenuHand
    {
        NoHand,
        LeftHand,
        RightHand
    }

    [UdonBehaviourSyncMode(BehaviourSyncMode.None)]
    public class QMExtension : UdonSharpBehaviour
    {
        float BaseHeight = 1.575837f;   // 基準のカメラスケール
        Vector3 HeadMenuPositionOffset = new Vector3(0f, 0.2f, 0.5f);   // デスクトップモードでのメニューの位置
        [Header("VRモードでの左手メニューの位置オフセット")]
        [SerializeField] Vector3 LeftMenuPositionOffset;
        [Header("VRモードでの右手メニューの位置オフセット")]
        [SerializeField] Vector3 RightMenuPositionOffset;
        [Space(20)]
        [SerializeField] Transform trackingScale;
        [SerializeField] Transform ExMenu;
        [SerializeField] Transform Head;
        [SerializeField] Transform LeftHand;
        [SerializeField] Transform LeftHandMenuJudge;
        [SerializeField] Transform RightHand;
        [SerializeField] Transform RightHandMenuJudge;
        [SerializeField] Transform LeftHandMenu;
        [SerializeField] Transform RightHandMenu;
        int intUI;
        int intUILeft;
        int intUIRight;
        VRCPlayerApi localPlayer;
        VRCMenu menu;
        MenuHand menuhand = MenuHand.NoHand;
        bool HeadMenuSet = false;
        Vector3 TrackingScale;
        float Magnification;

        private void Start()
        {
            localPlayer = Networking.LocalPlayer;
            ExMenu.gameObject.SetActive(false);
        }

        public override void PostLateUpdate()
        {
            Head.SetPositionAndRotation(localPlayer.GetTrackingData(VRCPlayerApi.TrackingDataType.Head).position, localPlayer.GetTrackingData(VRCPlayerApi.TrackingDataType.Head).rotation);
            LeftHand.SetPositionAndRotation(localPlayer.GetTrackingData(VRCPlayerApi.TrackingDataType.LeftHand).position, localPlayer.GetTrackingData(VRCPlayerApi.TrackingDataType.LeftHand).rotation);
            RightHand.SetPositionAndRotation(localPlayer.GetTrackingData(VRCPlayerApi.TrackingDataType.RightHand).position, localPlayer.GetTrackingData(VRCPlayerApi.TrackingDataType.RightHand).rotation);
        }

        private void LateUpdate()
        {
            intUI = Physics.OverlapSphere(localPlayer.GetPosition(), 10F, 524288).Length;
            if (trackingScale.localScale != TrackingScale)
            {
                TrackingScale = trackingScale.localScale;
                Magnification = BaseHeight / TrackingScale.x;
                this.transform.localScale = Vector3.one * Magnification;
            }

            if (intUI == 3 || intUI == 11 || intUI == 12 || intUI == 13)
            {
                if (menu != VRCMenu.QuickMenu)
                {
                    menu = VRCMenu.QuickMenu;
                    ExMenu.gameObject.SetActive(true);
                }
                if (localPlayer.IsUserInVR())
                {
                    intUILeft = Physics.OverlapSphere(LeftHandMenuJudge.position, 0.007F, 524288).Length;
                    intUIRight = Physics.OverlapSphere(RightHandMenuJudge.position, 0.007F, 524288).Length;
                    if (intUILeft == 1 && intUIRight != 1)  // 左手にメニューを検出
                    {
                        if (menuhand != MenuHand.LeftHand)
                        {
                            ExMenu.SetParent(LeftHandMenu);
                            ExMenu.localPosition = LeftMenuPositionOffset;
                            ExMenu.localRotation = Quaternion.identity;
                            menuhand = MenuHand.LeftHand;
                            HeadMenuSet = false;
                        }
                    }
                    else if (intUILeft != 1 && intUIRight == 1)     // 右手にメニューを検出
                    {
                        if (menuhand != MenuHand.RightHand)
                        {
                            ExMenu.SetParent(RightHandMenu);
                            ExMenu.localPosition = RightMenuPositionOffset;
                            ExMenu.localRotation = Quaternion.identity;
                            menuhand = MenuHand.RightHand;
                            HeadMenuSet = false;
                        }
                    }
                    else if (((intUILeft != 1 && intUIRight != 1) || (intUILeft == 1 && intUIRight == 1)) && menuhand == MenuHand.NoHand)
                    {
                        if (!HeadMenuSet)
                        {
                            ExMenu.SetParent(Head);
                            ExMenu.localPosition = new Vector3(0, 0, 0.5f * Magnification);
                            ExMenu.localRotation = Quaternion.identity;
                            ExMenu.parent = null;
                            HeadMenuSet = true;
                        }
                    }
                }
                else
                {
                    ExMenu.SetParent(Head);
                    ExMenu.localPosition = HeadMenuPositionOffset;
                    ExMenu.localRotation = Quaternion.identity;
                }
            }
            else
            {
                if (intUI == 8 || intUI == 9 || intUI == 17 || intUI == 18 || intUI == 19 || intUI == 20)
                {
                    if (menu != VRCMenu.MainMenu)
                    {
                        menu = VRCMenu.MainMenu;
                        menuhand = MenuHand.NoHand;
                        ExMenu.gameObject.SetActive(false);
                        HeadMenuSet = false;
                    }
                }
                else
                {
                    if (menu != VRCMenu.NoMenu)
                    {
                        menu = VRCMenu.NoMenu;
                        menuhand = MenuHand.NoHand;
                        ExMenu.gameObject.SetActive(false);
                        HeadMenuSet = false;
                    }
                }
            }
        }
    }
}
