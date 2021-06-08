//
//  IAPManager.h
//  Unity-iPhone
//
//  Created by jiansheng8150 on 2018/10/5.
//

#ifndef IAPManager_h
#define IAPManager_h


#endif /* IAPManager_h */

#import <StoreKit/StoreKit.h>

//SKProductsRequestDelegate需要实现productsRequest函数
//SKPaymentTransactionObserver需要实现productsRequest函数
@interface IAPManager : NSObject<SKProductsRequestDelegate,SKPaymentTransactionObserver>

+(IAPManager *) sharedInstance;
-(void)buyProduct:(NSString*)productid;

@end

