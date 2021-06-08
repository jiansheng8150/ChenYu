//
//  UnityConnectIOS.mm
//  Unity-iPhone
//
//  Created by jiansheng8150 on 2018/9/11.
//

#import "UnityInterface.h"
#import "UnityConnectIOS.h"
/*
//微信分享
#import "WXApiRequestHandler.h"
#import "WXApiManager.h"
#import "Constant.h"
#import "WechatAuthSDK.h"
#import "UIAlertView+WX.h"
//微信分享
*/

@implementation UnityConnectIOS :NSObject
    static UnityConnectIOS *instance = nil;
    +(UnityConnectIOS *)sharedInstance{
        @synchronized(self) {
            if(instance == nil) {
                instance = [[[self class] alloc] init];
            }
        }
        return instance;
    }
    NSString *popupType = @"";
    -(void)ShowNativeTip:(const char*)type title:(const char*)title msg:(const char*)msg button1:(const char*)button1 button2:(const char*)button2{
        popupType = [[NSString alloc] initWithCString:(const char*)type encoding:NSUTF8StringEncoding];
        NSString *titleTmp = [[NSString alloc] initWithCString:(const char*)title encoding:NSUTF8StringEncoding];
        NSString *msgTmp = [[NSString alloc] initWithCString:(const char*)msg encoding:NSUTF8StringEncoding];
        NSString *buttonTmp1 = [[NSString alloc] initWithCString:(const char*)button1 encoding:NSUTF8StringEncoding];
        NSString *buttonTmp2;
        if (button2 != NULL) {
            buttonTmp2 = [[NSString alloc] initWithCString:(const char*)button2 encoding:NSUTF8StringEncoding];
        }

        UIAlertView *alertView = [[UIAlertView alloc] initWithTitle:titleTmp
                                                            message:msgTmp
                                                           delegate:self
                                                  cancelButtonTitle:buttonTmp1
                                                otherButtonTitles:buttonTmp2,
                                                                  nil
                                                                  ];
        [alertView show];
    }


    - (void)alertView:(UIAlertView *)alertView clickedButtonAtIndex:(NSInteger)buttonIndex{
        NSString *retStr = [NSString stringWithFormat:@"%@,%ld",popupType,buttonIndex];
        const char *retChar = [retStr UTF8String];
        //const char *retChar = [retStr cStringUsingEncoding:NSUTF8StringEncoding];
        UnitySendMessage("GameObjectIOSInterface", "OnSelectTitleDialogCallBack", retChar);
    }
@end


#ifdef __cplusplus
extern "C" {
#endif
///*
    void _showSelectTitleDialog(const char *type, const char *title, const char *msg, const char *button1, const char *button2)
    {
        [[UnityConnectIOS sharedInstance] ShowNativeTip:type title:title msg:msg button1:button1 button2:button2];
    }
    /*
    void _shareToWeChat(const char *thumbImage, const char *imageName, const char *imageType, const char *textLog){
		NSString *thumbImageTmp = [[NSString alloc] initWithCString:(const char*)thumbImage encoding:NSUTF8StringEncoding];
		NSString *imageNameTmp = [[NSString alloc] initWithCString:(const char*)imageName encoding:NSUTF8StringEncoding];
		NSString *imageTypeTmp = [[NSString alloc] initWithCString:(const char*)imageType encoding:NSUTF8StringEncoding];
		NSString *textLogTmp = [[NSString alloc] initWithCString:(const char*)textLog encoding:NSUTF8StringEncoding];

        NSData *imageData = [NSData dataWithContentsOfFile:imageNameTmp];
        UIImage *thumbImageUI = [UIImage imageNamed:thumbImageTmp];
        [WXApiRequestHandler sendImageData:imageData
                                   TagName:@"WECHAT_TAG_JUMP_APP"
                                MessageExt:textLogTmp
                                    Action:@"<action>dotalist</action>"
                                ThumbImage:thumbImageUI
                                   InScene:WXSceneTimeline];
    }
    */
#ifdef __cplusplus
}
#endif


