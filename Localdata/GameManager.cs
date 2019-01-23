using Chat;
using DarkRift;
using DarkRift.Client;
using DarkRiftTags;
using Launcher;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game
{
    public class GameManager : MonoBehaviour
    {
        #region Events

        public delegate void NearestShipsDataEventHandler(string friendName);


        public static event NearestShipsDataEventHandler onNearestShipData;


        #endregion

        private void Start()
        {
            GameControl.Client.MessageReceived += OnDataHandler;
        }

        private void OnDestroy()
        {
            if (GameControl.Client == null)
                return;

            GameControl.Client.MessageReceived -= OnDataHandler;
        }

        #region Network Calls

        public static void SendPlayerShipCommands(string friendName)
        {
            using (var writer = DarkRiftWriter.Create())
            {
                writer.Write(friendName);

                using (var msg = Message.Create(FriendTags.FriendRequest, writer))
                {
                    GameControl.Client.SendMessage(msg, SendMode.Reliable);
                }
            }
        }
        #endregion

        private static void OnDataHandler(object sender, MessageReceivedEventArgs e)
        {
            using (var message = e.GetMessage())
            {
                // Check if message is meant for this plugin
                if (message.Tag < Tags.TagsPerPlugin * Tags.Game || message.Tag >= Tags.TagsPerPlugin * (Tags.Game+ 1))
                    return;

                switch (message.Tag)
                {
                    // New friend request received
                    case FriendTags.FriendRequest:
                        {
                            using (var reader = message.GetReader())
                            {
                                var friendName = reader.ReadString();
                                ChatManager.ServerMessage(friendName + " wants to add you as a friend!", MessageType.Info);

                                onNewFriendRequest?.Invoke(friendName);
                            }
                            break;
                        }

                    case FriendTags.RequestSuccess:
                        {
                            using (var reader = message.GetReader())
                            {
                                var friendName = reader.ReadString();
                                ChatManager.ServerMessage("Friend request sent.", MessageType.Info);

                                onSuccessfulFriendRequest?.Invoke(friendName);
                            }
                            break;
                        }

                }
            }

        }
    }
}