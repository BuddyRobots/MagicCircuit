//
//  MeiyuPushSdk.h
//  MeiyuPushSdk
//
//  Created by qingyun on 3/28/14.
//  Copyright (c) 2014 qingyun. All rights reserved.
//

#import <Foundation/Foundation.h>
#import <UIKit/UIKit.h>

@protocol  MeiyuPushSdkDelegate;


@interface MeiyuPushSdk : NSObject

@property (nonatomic, assign)id<MeiyuPushSdkDelegate> delegate;


//初始化
+(MeiyuPushSdk*)my_initPush;

+ (void)my_setupWithOption:(NSDictionary *)launchingOption registerForRemoteNotificationTypes:(int)types ;      // 注册APNS类型
+ (void)my_registerDeviceToken:(NSData *)deviceToken;                                                           // 向服务器上传Device Token
+ (void)my_receiveRemoteNotification:(NSDictionary *)remoteInfo;                                                // 处理收到的APNS消息，向服务器上传收到APNS消息

-(void)cTest;


@end

@protocol MeiyuPushSdkDelegate <NSObject>
@optional
-(void)cTestResult;


@end
