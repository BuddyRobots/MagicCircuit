//
//  MeiyuPushSdkC.m
//  MeiYuPushDemo
//
//  Created by qingyun on 3/31/14.
//  Copyright (c) 2014 qingyun. All rights reserved.
//

#import "MeiyuPushSdkC.h"
#import "MeiyuPushSdk.h"


@interface MeiyuPushSdkC : NSObject<MeiyuPushSdkDelegate>

@end


@implementation MeiyuPushSdkC

- (id)init
{
    id object = [super init];
    
    [[MeiyuPushSdk my_initPush]setDelegate:self];
    //sdk.delegate = self;
    NSLog(@"initMeiyuPushSdkC");
    return object;
}

-(void)cTestResult
{
    UnitySendMessage("Main Camera", "testBtnResult", "成功啦.哇哈哈哈");
}


@end

MeiyuPushSdkC *m_pushSdk = NULL;

#if defined (__cplusplus)
extern "C"
{
#endif
    
    void c_ctest()
    {
        if(m_pushSdk == NULL)
        {
            m_pushSdk = [[MeiyuPushSdkC alloc]init];
        }
        MeiyuPushSdk *m = [MeiyuPushSdk my_initPush];
        
        //objc_msgSend(obj, @selector(setName:),@"balabala");
        [m cTest];
        NSLog(@"testSuccess---------");
        UnitySendMessage("Main Camera", "testResult", "-10862904$_^_$接口调用失败");
    }
    
#if defined (__cplusplus)
}
#endif