using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using FcmSharp.Requests;
using FcmSharp.Settings;
using System.Web;
using FcmSharp;
using FcmSharp.Responses;
using System.IO;

/// <summary>
/// FcmManager 需要使用FcmSharp
/// </summary>
namespace Eki_Web_MVC
{
    public class FcmManager : IDisposable
    {
        public static FcmManager New()
        {
            return new FcmManager();
        }

        private FcmClient client;
        private CancellationTokenSource cts = new CancellationTokenSource();
        FcmManager()
        {

            var settings = StreamBasedFcmClientSettings.CreateFromStream(FcmConfig.project_id, new MemoryStream(Properties.Resources.FcmApiFile));
            //var settings = FileBasedFcmClientSettings.CreateFromFile(HttpContext.Current.Server.MapPath(FcmConfig.jsonFilePath));
            client = new FcmClient(settings);
        }

        //這個 暫時先不要用 因為會有手機的問題
        //public FcmBatchResponse SendMultiClient<T>(List<string> tokens,BroadCastMsg<T> broadCastMsg)where T:IBroadCastContent
        //{
        //    var message = new Message()
        //    {
        //        Data = new Dictionary<string, string>().Also(dic => 
        //        {
        //            dic.Add(FcmConfig.methodKey, broadCastMsg.Method);
        //            dic.Add(FcmConfig.contentKey, broadCastMsg.Content.toJsonString());
        //        })
        //    };
        //    return client.SendMulticastMessage(tokens.ToArray(), message, false, cts.Token).GetAwaiter().GetResult();
        //}
        public FcmMessageResponse SendTo(IFcmSet set, IFcmMsg broadCastMsg)
        {
            return SendTo(set.fcmToken(), set.device(), broadCastMsg);
        }

        public FcmMessageResponse SendTo(string token, MobilType type, IFcmMsg broadCastMsg)
        {

            try
            {
                FcmMessage message;

                switch (type)
                {
                    case MobilType.iOS:
                        message = iosFcmMsg_CustomData(token, broadCastMsg);
                        //if (broadCastMsg.hasInterface<IiosPushNotify>())
                        //{
                        //    //var iosPush = broadCastMsg as IiosPushNotify;
                        //    //broadCastMsg.saveLog("Send fcm by IiosPushNotify");
                        //    message = iosFcmMsg_ApsAlert(token, broadCastMsg);
                        //}
                        //else
                        //{   //這是舊的
                        //    message = iosFcmMsg_CustomData(token, broadCastMsg);
                        //}
                        break;
                    case MobilType.Android:
                        message = androidFcmMsg(token, broadCastMsg);
                        break;
                    default:
                        message = webFcmMsg(token, broadCastMsg);
                        break;
                }


                // Send the Message and wait synchronously:
                var result = client.SendAsync(message, cts.Token).GetAwaiter().GetResult();
                return result;
            }
            catch (Exception e)
            {
                //e.saveLog("SendFcmFail", "send", broadCastMsg.toJsonString());
            }
            return null;
        }
        private FcmMessage iosFcmMsg_ApsAlert(string token, IFcmMsg broadCastMsg)
        {
            return new FcmMsg(new Message
            {
                Token = token,
                ApnsConfig = new ApnsConfig
                {
                    Payload = new ApnsConfigPayload
                    {
                        Aps = new Aps
                        {
                            Alert = iosPushAlert(broadCastMsg as IiosPushNotify)
                        }
                    }
                }
            });
        }
        private FcmMessage iosFcmMsg_CustomData(string token, IFcmMsg broadCastMsg)
        {
            ApsAlert alert = null;
            if (broadCastMsg.hasInterface<IiosPushNotify>())
                alert = iosPushAlert(broadCastMsg as IiosPushNotify);

            return new FcmMsg(new Message
            {
                Token = token,
                ApnsConfig = new ApnsConfig
                {
                    Payload = new ApnsConfigPayload
                    {
                        Aps = new Aps
                        {
                            MutableContent = true,
                            Alert = alert
                        },
                        CustomData = iosData(broadCastMsg)
                    }
                }
            });
        }
        private FcmMessage androidFcmMsg(string token, IFcmMsg broadCastMsg)
        {
            return new FcmMsg(new Message
            {
                Token = token,
                Data = androidData(broadCastMsg)
            });
        }
        private FcmMessage webFcmMsg(string token, IFcmMsg broadCastMsg)
        {
            return new FcmMsg(new Message
            {
                Token = token,
                WebpushConfig = new WebpushConfig
                {
                    Data = androidData(broadCastMsg)
                }
            });
        }

        private ApsAlert iosPushAlert(IiosPushNotify iosPush)
        {
            return new ApsAlert()
            {
                Body = iosPush.body(),
                Title = iosPush.title()
            };
        }

        private class FcmMsg : FcmMessage
        {
            internal FcmMsg(Message msg)
            {
                ValidateOnly = false;
                Message = msg;
            }
        }

        private Dictionary<string, object> iosData(IFcmMsg broadCastMsg) => new Dictionary<string, object>() {
            {FcmConfig.methodKey,broadCastMsg.fcmMethod() },
            {FcmConfig.contentKey,broadCastMsg.toJsonString() }
        };

        private Dictionary<string, string> androidData(IFcmMsg broadCastMsg) => new Dictionary<string, string>{
            { FcmConfig.methodKey, broadCastMsg.fcmMethod()},
            { FcmConfig.contentKey, broadCastMsg.toJsonString()}
        };

        public void Dispose()
        {
            cts.Cancel();
            cts.Dispose();
            client.Dispose();
        }
    }
}
