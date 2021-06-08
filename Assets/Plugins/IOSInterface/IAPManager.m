//
//  IAPManager.m
//  Unity-iPhone
//
//  Created by jiansheng8150 on 2018/10/5.
//

#import "IAPManager.h"

@interface IAPManager()<SKProductsRequestDelegate,SKPaymentTransactionObserver>

@end

@implementation IAPManager

static IAPManager *instance = nil;
+(IAPManager *)sharedInstance{
    @synchronized(self) {
        if(instance == nil) {
            instance = [[[self class] alloc] init];
            [instance addEventListener];
        }
    }
    return instance;
}

//移除监听(暂时没用到)
- (void)removeEventListener {
    [[SKPaymentQueue defaultQueue] removeTransactionObserver:self];
}

//添加监听
- (void)addEventListener {
    [[SKPaymentQueue defaultQueue] addTransactionObserver:self];
}

#pragma mark － 用户点击购买商品 －－－－－－－
//productid 商品id， 事先在App Store Connect中添加好的，已存在的付费项目。
- (void)buyProduct:(NSString *)productid {
    //判断是否允许内付费
    if ([SKPaymentQueue canMakePayments]) {
        //获取商品信息
        [self getProductInfo:productid];
    } else {
		const char *retChar1 = "100";
        UnitySendMessage("GameObjectIOSInterface", "OnBuyProductCallBack", retChar1);
        NSLog(@"用户禁止应用内付费购买！");
    }
}

//productid是事先在App Store Connect中添加好的，已存在的付费项目。否则获取商品信息会失败。
- (void)getProductInfo:(NSString *)productid {
    //NSSet * set = [NSSet setWithArray:@[productid]];
    NSArray *product = [[NSArray alloc] initWithObjects:productid, nil];
    NSSet *set = [NSSet setWithArray:product];
    SKProductsRequest * request = [[SKProductsRequest alloc] initWithProductIdentifiers:set];
    request.delegate = self;
    [request start];
    NSLog(@"请求商品信息中，请稍等。。。");
}

#pragma mark － getProductInfo中SKProductsRequest请求的回调函数 －－－－－－－
- (void)productsRequest:(SKProductsRequest *)request didReceiveResponse:(SKProductsResponse *)response {
    NSArray *myProduct = response.products;
    NSLog(@"ProductInfo:%@",myProduct);
    if (myProduct.count == 0) {
        NSLog(@"无法获取商品信息，购买失败！");
		const char *retChar1 = "200";
        UnitySendMessage("GameObjectIOSInterface", "OnBuyProductCallBack", retChar1);
        return;
    }
    SKPayment * payment = [SKPayment paymentWithProduct:myProduct[0]];
    [[SKPaymentQueue defaultQueue] addPayment:payment];
}

#pragma mark － productsRequest中SKPayment请求的回调函数 －－－－－－－
- (void)paymentQueue:(SKPaymentQueue *)queue updatedTransactions:(NSArray *)transactions {
    const char *retChar1 = "5";
    const char *retChar2 = "6";
    for (SKPaymentTransaction *transaction in transactions)
    {
        switch (transaction.transactionState)
        {
            case SKPaymentTransactionStatePurchased://交易完成
                NSLog(@"购买成功！");
                NSLog(@"transactionIdentifier:%@", transaction.transactionIdentifier);
                [self completeTransaction:transaction];
                break;
            case SKPaymentTransactionStateFailed://交易失败
                NSLog(@"购买失败！");
                [self failedTransaction:transaction];
                break;
            case SKPaymentTransactionStateRestored://已经购买过该商品
                NSLog(@"已经购买过该商品！");
                [self restoreTransaction:transaction];
                break;
            case SKPaymentTransactionStatePurchasing://商品添加进列表
                NSLog(@"商品添加进列表！");
				//返回结果
				UnitySendMessage("GameObjectIOSInterface", "OnBuyProductCallBack", retChar1);
                break;
            default:
				//返回结果
				UnitySendMessage("GameObjectIOSInterface", "OnBuyProductCallBack", retChar2);
                break;
        }
    }
}

//购买成功
- (void)completeTransaction:(SKPaymentTransaction *)transaction {
    NSString * productIdentifier = transaction.payment.productIdentifier;
    if ([productIdentifier length] > 0) {
        //向自己的服务器验证购买凭证
        //获得购买凭证
        NSURL *receiptUrl=[[NSBundle mainBundle] appStoreReceiptURL];
        NSData *receiptData=[NSData dataWithContentsOfURL:receiptUrl];
        NSString *receipt=[receiptData base64EncodedStringWithOptions:NSDataBase64EncodingEndLineWithLineFeed];//转化为base64字符串
        
        //NSString * receipt = [[NSString alloc]initWithData:transaction.transactionReceipt encoding:NSUTF8StringEncoding];
        //这里要用base64编码
        
        NSLog(@"receipt:%@",receipt);
		const char *retChar1 = [receipt UTF8String];
        UnitySendMessage("GameObjectIOSInterface", "OnBuyProductCallBack", retChar1);
    }else{
		const char *retChar2 = "10";
		UnitySendMessage("GameObjectIOSInterface", "OnBuyProductCallBack", retChar2);
	}
    //Remove the transaction from the payment queue.
    [[SKPaymentQueue defaultQueue] finishTransaction: transaction];
}

//购买失败或用户取消购买
- (void)failedTransaction:(SKPaymentTransaction *)transaction {
	
    if(transaction.error.code != SKErrorPaymentCancelled) {
        NSLog(@"购买失败");
		const char *retChar1 = "2";
		UnitySendMessage("GameObjectIOSInterface", "OnBuyProductCallBack", retChar1);
    } else {
        NSLog(@"用户取消购买");
		const char *retChar2 = "3";
		UnitySendMessage("GameObjectIOSInterface", "OnBuyProductCallBack", retChar2);
    }
    //Remove the transaction from the payment queue.
    [[SKPaymentQueue defaultQueue] finishTransaction: transaction];
}

//重复购买
- (void)restoreTransaction:(SKPaymentTransaction *)transaction {
    // 对于已购商品，处理恢复购买的逻辑

    //返回结果
	const char *retChar = "4";
    UnitySendMessage("GameObjectIOSInterface", "OnBuyProductCallBack", retChar);
    //Remove the transaction from the payment queue.
    [[SKPaymentQueue defaultQueue] finishTransaction: transaction];
}

@end




#ifdef __cplusplus
extern "C" {
#endif
    
    void _buyProduct(const char *productid)
    {
        NSString *productidStr = [[NSString alloc] initWithCString:(const char*)productid encoding:NSUTF8StringEncoding];
        [[IAPManager sharedInstance] buyProduct:productidStr];
    }
    
#ifdef __cplusplus
}
#endif





