#import <Foundation/Foundation.h>
#import <Security/Security.h>

NSString * const KEY_UDID_INSTEAD = @"com.jianshenglin.tomato.udid";

/**
�������ǵõ� UUID �����ϵͳ�е� keychain �ķ���
������� plist �ļ�
����ɾ������װ,�Կ��Եõ���ͬ��Ψһ��ʾ
���ǵ�ϵͳ��������ˢ����,ϵͳ�е�Կ�״��ᱻ���,��ʱ������ʧЧ
*/

@interface DeviceKeychain:NSObject

@end
